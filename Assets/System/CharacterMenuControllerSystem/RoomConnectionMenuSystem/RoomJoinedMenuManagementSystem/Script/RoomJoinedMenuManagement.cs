using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using Takechi.CharacterController.Parameters;
using Takechi.UI.CanvasMune.DisplayListUpdate;
using Takechi.PlayableCharacter.FadingCanvas;

using static Takechi.ScriptReference.CustomPropertyKey.CustomPropertyKeyReference;
using static Takechi.ScriptReference.NetworkEnvironment.ReferencingNetworkEnvironmentDetails;
using static Takechi.ScriptReference.SceneInformation.ReferenceSceneInformation;
using UnityEngine.Rendering;
using Takechi.CharacterController.RoomStatus;
using Takechi.CharacterController.Address;

namespace Takechi.UI.RoomJoinedMenu
{
    /// <summary>
    /// ルーム接続後に表示されるメニューを、管理する。
    /// </summary>
    public class RoomJoinedMenuManagement : DisplayListUpdateManagement
    {
        #region SerializeField
        [Header("=== CharacterStatusManagement ===")]
        [SerializeField] private CharacterStatusManagement m_characterStatusManagement;
        [Header("=== CharacterAddressManagement === ")]
        [SerializeField] private CharacterAddressManagement m_characterAddressManagement;

        [Header(" ui setting ")]
        [SerializeField] private Text   m_roomInfometionText;
        [SerializeField] private Button m_gameStartButton;
        [SerializeField] private Button m_closeButton;

        [Header(" memberList setting ")]
        [SerializeField] private Text m_instansText;
        [SerializeField] private GameObject m_teamAContent;
        [SerializeField] private GameObject m_teamBContent;
        [SerializeField] private ToFade     m_toFade;
        [SerializeField] private PhotonView m_thisPhotnView;

        #endregion

        #region private variable
        private CharacterAddressManagement addressManagement => m_characterAddressManagement;
        private CharacterStatusManagement statusManagement => m_characterStatusManagement;
        private RoomStatusManagement roomStatusManagement => addressManagement.GetMyRoomStatusManagement();
        private List<Player> m_teamA_memberList = new List<Player>(4);
        private List<Player> m_teamB_memberList = new List<Player>(4);

        #endregion

        #region update variable
        private void updateRoomInfometionText() { m_roomInfometionText.text = WhatTheTextDisplays(); }

        #endregion

        #region reset variable
        private void resetRoomInfometionText() { m_roomInfometionText.text = "\r\n\r\nconnecting..."; }

        #endregion

        #region private async

        /// <summary>
        /// 同期時間経過後に、実行されます。
        /// </summary>
        /// <param name="s"></param>
        private async void StartAfterSync(int s)
        {
            await Task.Delay(s);

            StartCoroutine(nameof(ListUpdate));
            StartCoroutine(nameof(InitialTeamSetup));

            updateRoomInfometionText();

            m_closeButton.interactable = true;
            Debug.Log(" m_closeButton.interactable : true <color=green>to set</color>.");
        }

        #endregion

        #region Unity Event
        private void Reset()
        {
            m_thisPhotnView = this.transform.GetComponent<PhotonView>();
        }
       
        public override void OnEnable()
        {
            base.OnEnable();

            m_closeButton.interactable = false;
            Debug.Log(" m_closeButton.interactable : false <color=green>to set</color>.");

            StartAfterSync(NetworkSyncSettings.connectionSynchronizationTime);

            OnlyClientsDisplay(m_gameStartButton.gameObject);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            DestroyChildObjects(m_teamAContent);
            DestroyChildObjects(m_teamBContent);

            resetRoomInfometionText();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);

            updateRoomInfometionText();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);

            updateRoomInfometionText();
        }

        #endregion

        #region PunCallbacks

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (!PhotonNetwork.LocalPlayer.IsLocal) return;

            OnlyClientsDisplay(m_gameStartButton.gameObject);
        }

        #endregion

        #region event system finction

        public void OnSceneSyncChange()
        {
            m_thisPhotnView.RPC( nameof(RPC_SceneSyncChange),RpcTarget.AllBufferedViaServer);
            roomStatusManagement.SetIsOpen(false);
            roomStatusManagement.SetIsVisible(false);

             StartCoroutine(DelayMethod( NetworkSyncSettings.fadeProductionTime_Seconds, () =>
            {
                SceneSyncChange( SceneName.characterSelectionScene);
            }));
        }

        [PunRPC] 
        private void RPC_SceneSyncChange()
        {
            m_toFade.OnFadeOut("NowLoading...");
        }

        public void OnLeaveRoom() 
        {
            statusManagement.SetThisPhotonViewRequestOwnership();
            LeaveRoom();
        }

        public void OnChangeTeam(string teamName)
        {
            if (!PhotonNetwork.LocalPlayer.IsLocal) return;

            if ( CharacterTeamStatusName.teamAName != teamName && CharacterTeamStatusName.teamBName != teamName)
            {
                Debug.LogError("The variable you are setting for the button is different from the expected value.");
                return;
            }

            if ( teamName == CharacterTeamStatusName.teamAName)
            {
                if (!MaximumOfMembers( m_teamA_memberList))
                {
                    statusManagement.SetCustomPropertiesTeamName(teamName);
                }
                else
                {
                    Debug.LogWarning(" The change has been canceled because the new team member is full. ");
                }
            }
            else if( teamName == CharacterTeamStatusName.teamBName)
            {
                if (!MaximumOfMembers( m_teamB_memberList))
                {
                    statusManagement.SetCustomPropertiesTeamName(teamName);
                }
                else
                {
                    Debug.LogWarning(" The change has been canceled because the new team member is full. ");
                }
            }

            PlayerInformationDisplay( PhotonNetwork.LocalPlayer, " ChangeTeamPlayerInfometion ", CharacterStatusKey.allKeys);
        }

        #endregion

        #region IEnumerator Update 

        protected override IEnumerator ListUpdate()
        {
            base.ListUpdate();
            
            while ( this.gameObject.activeSelf)
            {
                m_teamA_memberList = new List<Player>();
                m_teamB_memberList = new List<Player>();

                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if ((statusManagement.GetCustomPropertiesTeamName(player) == CharacterTeamStatusName.teamAName))
                    {
                        m_teamA_memberList.Add(player);
                    }
                    else
                    {
                        m_teamB_memberList.Add(player);
                    }
                }

                RefreshTheListViewing();

                m_gameStartButton.interactable = AppropriateConfirmationOfTheNumberOfPeople();

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        protected override void RefreshTheListViewing()
        {
            DestroyChildObjects(m_teamAContent);
            DestroyChildObjects(m_teamBContent);

            InstantiateTheContentsOfListAndChangeText<Text, Player>( m_instansText, m_teamA_memberList, m_teamAContent.transform);
            InstantiateTheContentsOfListAndChangeText<Text, Player>( m_instansText, m_teamB_memberList, m_teamBContent.transform);
        }

        #endregion

        #region private finction

        private bool AppropriateConfirmationOfTheNumberOfPeople()
        {
            return true;
            //return m_teamA_memberList.Count == m_teamB_memberList.Count ? true : false;
        }

        private string WhatTheTextDisplays()
        {
            string s
               = $" PlayerSlots : <color=green>{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}</color> \n" +
                 $" RoomName : <color=green>{PhotonNetwork.CurrentRoom.Name}</color> \n" +
                 $" HostName : <color=green>{PhotonNetwork.MasterClient.NickName}</color> \n"+
                 $" MapName  : <color=green>{(string)PhotonNetwork.CurrentRoom.CustomProperties[RoomStatusKey.mapKey]}</color> \n"+
                 $" GameType : <color=green>{(string)PhotonNetwork.CurrentRoom.CustomProperties[RoomStatusKey.gameTypeKey]}</color> \n";

            return s;
        }

        private IEnumerator InitialTeamSetup()
        {
            statusManagement.SetCustomPropertiesTeamName(CharacterTeamStatusName.teamAName);

            yield return new WaitForSeconds( Time.deltaTime);

            if (!MaximumOfMembers( m_teamB_memberList))
            {
                statusManagement.SetCustomPropertiesTeamName( CharacterTeamStatusName.teamBName);
            }
            else if (!MaximumOfMembers( m_teamA_memberList))
            {
                statusManagement.SetCustomPropertiesTeamName( CharacterTeamStatusName.teamAName);
            }
        }

        #endregion
    }
}

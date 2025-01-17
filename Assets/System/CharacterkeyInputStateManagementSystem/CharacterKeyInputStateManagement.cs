using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Takechi.CharacterController.Address;
using Takechi.CharacterController.RoomStatus;
using Takechi.CharacterController.Parameters;
using static Takechi.ScriptReference.CustomPropertyKey.CustomPropertyKeyReference;
using UnityEngine.Video;

namespace Takechi.CharacterController.KeyInputStete
{
    [RequireComponent(typeof(CharacterAddressManagement))]
    [RequireComponent(typeof(CharacterStatusManagement))]
    [RequireComponent(typeof(RoomStatusManagement))]
    public class CharacterKeyInputStateManagement : MonoBehaviour
    {
        #region SerializeField
        [Header("=== CharacterAddressManagement === ")]
        [SerializeField] private CharacterAddressManagement m_characterAddressManagement;
        [Header("=== CharacterStatusManagement ===")]
        [SerializeField] private CharacterStatusManagement  m_characterStatusManagement;
        [Header("=== RoomStatusManagement ===")]
        [SerializeField] private RoomStatusManagement m_roomStatusManagement;

        [SerializeField] private KeyCode m_dashKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode m_jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode m_deathblowKey = KeyCode.Q;
        [SerializeField] private KeyCode m_displayMapKey= KeyCode.M;
        [SerializeField] private KeyCode m_ability1Key = KeyCode.E;
        [SerializeField] private KeyCode m_ability2Key = KeyCode.C;
        [SerializeField] private KeyCode m_ability3Key = KeyCode.V;
        [SerializeField] private KeyCode m_userMenuKey = KeyCode.Escape;

        #endregion

        #region privet variable
        private CharacterStatusManagement  statusManagement  => m_characterStatusManagement;
        private CharacterAddressManagement addressManagement => m_characterAddressManagement;
        private RoomStatusManagement roomStatusManagement => m_roomStatusManagement;
        private PhotonView myPhotonView => addressManagement.GetMyPhotonView();

        private bool m_operation = true;
        private bool m_isUserMenu = false;
        private bool m_isDisplayMap = false;
        private bool m_isStan = false;
        private Dictionary<string, Action> m_gameMain = new Dictionary<string, Action>();
     
        #endregion

        public event Action< CharacterStatusManagement, CharacterAddressManagement> InputToStartDash = delegate { };
        public event Action< CharacterStatusManagement, CharacterAddressManagement> InputToStopDash  = delegate { };
        public event Action< CharacterStatusManagement, CharacterAddressManagement> InputToJump      = delegate { };
        public event Action< CharacterStatusManagement, CharacterAddressManagement> InputToNormalAttack = delegate { };
        public event Action< CharacterStatusManagement, CharacterAddressManagement> InputToDeathblow = delegate { };
        public event Action< CharacterStatusManagement, CharacterAddressManagement> InputToAblity1   = delegate { };
        public event Action< CharacterStatusManagement, CharacterAddressManagement> InputToAblity2   = delegate { };
        public event Action< CharacterStatusManagement, CharacterAddressManagement> InputToAblity3   = delegate { };
        public event Action< CharacterStatusManagement, CharacterAddressManagement> InputUserMenu = delegate { };
        public event Action< CharacterStatusManagement, CharacterAddressManagement> InputDisplayMapMenu = delegate { };
        public event Action< CharacterStatusManagement, CharacterAddressManagement, float , float> InputToMovement  = delegate { };
        public event Action< CharacterStatusManagement, CharacterAddressManagement, float , float> InputToViewpoint = delegate { };

        void Reset()
        {
            m_characterStatusManagement  = this.transform.GetComponent<CharacterStatusManagement>();
            m_characterAddressManagement = this.transform.GetComponent<CharacterAddressManagement>();
            m_roomStatusManagement = this.transform.GetComponent<RoomStatusManagement>();
        }

        void OnEnable()
        {
            setupOfOnEnable();
        }

        void OnDisable()
        {
            setupOfOnDisable();
        }

        void Update()
        {
            if (!myPhotonView.IsMine) return;

            if (!m_isUserMenu)
            {
                if (Input.GetKeyDown(m_displayMapKey)) { InputDisplayMapMenu(statusManagement, addressManagement); }
            }

            if (!m_isDisplayMap)
            {
                if (Input.GetKeyDown(m_userMenuKey)) { InputUserMenu(statusManagement, addressManagement); }
            }

            if (m_isDisplayMap) return;
            if (m_isUserMenu) return;
            if (!m_operation) return;
            if (m_isStan) return;

            m_gameMain[roomStatusManagement.GetGameState()]();
        }

        #region get function
        public bool GetOperation() => m_operation;
        public bool GetIsUserMenu() => m_isUserMenu;
        public bool GetIsStan() => m_isStan;
        public bool GetIsDisplayMap() => m_isDisplayMap;

        #endregion

        #region set function
        public void SetOperation( bool value) 
        {
            m_operation = value;
            Debug.Log($" SetOperation <color=green>{m_operation}</color> to set.");
        }
        public void SetIsUserMenu(bool value)
        {
            m_isUserMenu = value;
            Debug.Log($" SetIsUserMenu <color=green>{m_isUserMenu}</color> to set.");
        }
        public void SetIsDisplayMap(bool value)
        {
            m_isDisplayMap = value;
            Debug.Log($" SetIsUserMenu <color=green>{m_isDisplayMap}</color> to set.");
        }
        public void SetIsStan(bool value)
        {
            m_isStan = value;
            Debug.Log($" SetIsStan <color=green>{m_isStan}</color> to set.");
        }

        #endregion

        #region set up function
        private void setupOfOnEnable()
        {
            m_gameMain.Add(RoomStatusName.GameState.beforeStart, GameState_BeforeStart);
            Debug.Log($"m_gameMain.<color=yellow>Add</color>( <color=green>RoomStatusName</color>.<color=green>GameState</color>.beforeStart, <color=yellow>GameState_BeforeStart</color>)");
            m_gameMain.Add(RoomStatusName.GameState.running, GameState_Running);
            Debug.Log($"m_gameMain.<color=yellow>Add</color>( <color=green>RoomStatusName</color>.<color=green>GameState</color>.running, <color=yellow>GameState_Running</color>)");
            m_gameMain.Add(RoomStatusName.GameState.end, GameState_End);
            Debug.Log($"m_gameMain.<color=yellow>Add</color>( <color=green>RoomStatusName</color>.<color=green>GameState</color>.end, <color=yellow>GameState_End</color>)");
            m_gameMain.Add(RoomStatusName.GameState.stopped, GameState_Stopped);
            Debug.Log($"m_gameMain.<color=yellow>Add</color>( <color=green>RoomStatusName</color>.<color=green>GameState</color>.stopped, <color=yellow>GameState_Stopped</color>)");
            m_gameMain.Add(RoomStatusName.GameState.NULL, GameState_NULL);
            Debug.Log($"m_gameMain.<color=yellow>Add</color>( <color=green>RoomStatusName</color>.<color=green>GameState</color>.NULL, <color=yellow>GameState_NULL</color>)");
        }
        private void setupOfOnDisable()
        {
            m_gameMain.Remove(RoomStatusName.GameState.beforeStart);
            Debug.Log($"m_gameMain.<color=yellow>Remove</color>( <color=green>RoomStatusName</color>.<color=green>GameState</color>.beforeStart");
            m_gameMain.Remove(RoomStatusName.GameState.running);
            Debug.Log($"m_gameMain.<color=yellow>Remove</color>( <color=green>RoomStatusName</color>.<color=green>GameState</color>.running");
            m_gameMain.Remove(RoomStatusName.GameState.end);
            Debug.Log($"m_gameMain.<color=yellow>Remove</color>( <color=green>RoomStatusName</color>.<color=green>GameState</color>.end");
            m_gameMain.Remove(RoomStatusName.GameState.stopped);
            Debug.Log($"m_gameMain.<color=yellow>Remove</color>( <color=green>RoomStatusName</color>.<color=green>GameState</color>.stopped");
            m_gameMain.Remove(RoomStatusName.GameState.NULL);
            Debug.Log($"m_gameMain.<color=yellow>Remove</color>( <color=green>RoomStatusName</color>.<color=green>GameState</color>.NULL");
        }

        #endregion

        #region main finction
        private void GameState_BeforeStart()
        {
            if (Input.GetKeyDown(m_jumpKey) && statusManagement.GetIsGrounded()) { InputToJump(statusManagement, addressManagement); }
            InputToViewpoint(statusManagement, addressManagement, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        private void GameState_Running()
        {
            if (Input.GetMouseButtonDown(0)) { InputToNormalAttack(statusManagement, addressManagement); }
            if (Input.GetKey(m_dashKey)) { InputToStartDash(statusManagement, addressManagement); }
            else { InputToStopDash(statusManagement, addressManagement); }

            if (Input.GetKeyDown(m_jumpKey) && statusManagement.GetIsGrounded()) { InputToJump(statusManagement, addressManagement); }
            if (Input.GetKeyDown(m_deathblowKey) && statusManagement.GetCanUseDeathblow()) { InputToDeathblow(statusManagement, addressManagement); }
            if (Input.GetKeyDown(m_ability1Key) && statusManagement.GetCanUseAbility1()) { InputToAblity1(statusManagement, addressManagement); }
            if (Input.GetKeyDown(m_ability2Key) && statusManagement.GetCanUseAbility2()) { InputToAblity2(statusManagement, addressManagement); }
            if (Input.GetKeyDown(m_ability3Key) && statusManagement.GetCanUseAbility3()) { InputToAblity3(statusManagement, addressManagement); }

            InputToViewpoint(statusManagement, addressManagement, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            InputToMovement(statusManagement, addressManagement, Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        private void GameState_End()
        {
            if (Input.GetKeyDown(m_jumpKey) && statusManagement.GetIsGrounded()) { InputToJump(statusManagement, addressManagement); }
            InputToViewpoint(statusManagement, addressManagement, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        private void GameState_Stopped()
        {

        }

        private void GameState_NULL()
        {
            if (Input.GetMouseButtonDown(0)) { InputToNormalAttack(statusManagement, addressManagement); }
            if (Input.GetKey(m_dashKey)) { InputToStartDash(statusManagement, addressManagement); }
            else { InputToStopDash(statusManagement, addressManagement); }

            if (Input.GetKeyDown(m_jumpKey) && statusManagement.GetIsGrounded()) { InputToJump(statusManagement, addressManagement); }
            if (Input.GetKeyDown(m_deathblowKey) && statusManagement.GetCanUseDeathblow()) { InputToDeathblow(statusManagement, addressManagement); }
            if (Input.GetKeyDown(m_ability1Key) && statusManagement.GetCanUseAbility1()) { InputToAblity1(statusManagement, addressManagement); }
            if (Input.GetKeyDown(m_ability2Key) && statusManagement.GetCanUseAbility2()) { InputToAblity2(statusManagement, addressManagement); }
            if (Input.GetKeyDown(m_ability3Key) && statusManagement.GetCanUseAbility3()) { InputToAblity3(statusManagement, addressManagement); }

            InputToViewpoint(statusManagement, addressManagement, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            InputToMovement(statusManagement, addressManagement, Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        #endregion
    }
}
using UnityEngine;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;

using Takechi.CharacterController.Address;
using Takechi.CharacterController.AnimationEvent;
using Takechi.CharacterController.SoundEffects;
using Takechi.CharacterController.Parameters;

using static Takechi.ScriptReference.AnimatorControlVariables.ReferencingTheAnimatorControlVariablesName;
using static Takechi.ScriptReference.SearchForPrefabs.ReferencingSearchForPrefabs;

namespace Takechi.CharacterController.EnemySearchAnimationEvent
{
    public class MechanicalWarriorHandOnlyEnemySearchAnimationEventManager : AnimationEventManagement
    {
        #region SerializeField
        [Header("=== MechanicalWarriorAddressManagement === ")]
        [SerializeField] private MechanicalWarriorAddressManagement m_mechanicalWarriorAddressManagement;
        [Header("=== MechanicalWarriorStatusManagement ===")]
        [SerializeField] private MechanicalWarriorStatusManagement  m_mechanicalWarriorStatusManagement;
        [Header("=== MechanicalWarriorSoundEffectsManagement ===")]
        [SerializeField] private MechanicalWarriorSoundEffectsManagement m_mechanicalWarriorSoundEffectsManagement;

        #endregion

        #region private variable
        private MechanicalWarriorAddressManagement addressManagement => m_mechanicalWarriorAddressManagement;
        private MechanicalWarriorStatusManagement  statusManagement => m_mechanicalWarriorStatusManagement;
        private MechanicalWarriorSoundEffectsManagement soundEffectsManagement => m_mechanicalWarriorSoundEffectsManagement;
        private GameObject networkModelAssaultRifle =>   addressManagement.GetNetworkModelAssaultRifleObject();
        private GameObject handOnlyModelAssaultRifle =>  addressManagement.GetHandOnlyModelAssaultRifleObject();
        private Animator   networkModelAnimator =>  addressManagement.GetNetworkModelAnimator(); 
        private Animator   handOnlyModelAnimator => addressManagement.GetHandOnlyModelAnimator();
        private PhotonView myPhotonView => addressManagement.GetMyPhotonView();
        private float  enemySearch_Seconds => statusManagement.GetEnemySearch_Seconds();
        private string playerCharacterPrefabTag => SearchForPrefabTag.playerCharacterPrefabTag;
        private string characterModelPrefabName => SearchForPrefabName.characterModelPrefabName;
        private bool   IsMine => myPhotonView.IsMine;
        /// <summary>
        /// 重みの一時保管
        /// </summary>
        private float m_networkModelAnimatorWeight = 0;
        private float m_handOnlyModelAnimatorWeight = 0;
        #endregion

        private void OnEnable()
        {
            statusManagement.InitializeCharacterInstanceStateSettings += () => { resetEnemySearch(); };
            Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} :  statusManagement.InitializeCharacterInstanceStateSettings <color=yellow>resetEnemySearch</color>() <color=green>to add.</color>");
        }

        private void OnDisable()
        {
            statusManagement.InitializeCharacterInstanceStateSettings -= () => { resetEnemySearch(); };
            Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} :  statusManagement.InitializeCharacterInstanceStateSettings <color=yellow>resetEnemySearch</color>() <color=green>to remove.</color>");
        }

        #region uinty animation event
        /// <summary>
        /// MechanicalWarrior EnemySearch Start
        /// </summary>
        void MechanicalWarriorEnemySearchStart()
        {
            // operation
            keyInputStateManagement.SetOperation(false);

            // assaultRifle
            setAssaultRifleSetActive(false);

            // animation weiht
            m_networkModelAnimatorWeight  = networkModelAnimator.GetLayerWeight(networkModelAnimator.GetLayerIndex(AnimatorLayers.overrideLayer));
            m_handOnlyModelAnimatorWeight = networkModelAnimator.GetLayerWeight(handOnlyModelAnimator.GetLayerIndex(AnimatorLayers.overrideLayer));
            SetLayerWeight(networkModelAnimator,  AnimatorLayers.overrideLayer, 0f);
            // SetLayerWeight(handOnlyModelAnimator, AnimatorLayers.overrideLayer, 0f);

            // ik weiht
            controllerReferenceManagement.GetIKAnimationController().SetAllIkWeight(0);
           
        }

        /// <summary>
        /// MechanicalWarrior EnemySearch Click Button
        /// </summary>
        void MechanicalWarriorEnemySearchClickButton()
        {
            // sound Effect
            soundEffectsManagement.PlayOneShotEnemySearchClickButton();
        }

        /// <summary>
        /// MechanicalWarrior EnemySearch End
        /// </summary>
        void MechanicalWarriorEnemySearchEnd()
        {
            // sound Effect
            soundEffectsManagement.PlayOneShotEnemyOnSearch();

            // operation
            keyInputStateManagement.SetOperation(true);

            // assaultRifle
            setAssaultRifleSetActive(true);

            // animation weiht
            SetLayerWeight(networkModelAnimator,  AnimatorLayers.overrideLayer,  m_networkModelAnimatorWeight);
            // SetLayerWeight(handOnlyModelAnimator, AnimatorLayers.overrideLayer, m_handOnlyModelAnimatorWeight);

            // ik weiht
            controllerReferenceManagement.GetIKAnimationController().ResetAllIkWeight();

            // enemySearch
            onEnemySearch();
            StartCoroutine(DelayMethod(enemySearch_Seconds, () => { resetEnemySearch(); }));
        }

        #endregion

        #region private finction
        private void setAssaultRifleSetActive(bool flag)
        {
            handOnlyModelAssaultRifle.SetActive(flag);
            networkModelAssaultRifle.SetActive(flag);
        }
        private void onEnemySearch()
        {
            if (!IsMine) return;

            foreach (GameObject o in GameObject.FindGameObjectsWithTag(playerCharacterPrefabTag))
            {
                Outline outline = o.transform.Find(characterModelPrefabName).GetComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
            }
        }
        private void resetEnemySearch()
        {
            if (!IsMine) return;

            foreach (GameObject o in GameObject.FindGameObjectsWithTag(playerCharacterPrefabTag))
            {
                Outline outline = o.transform.Find(characterModelPrefabName).GetComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineVisible;
            }
        }

        #endregion
    }
}

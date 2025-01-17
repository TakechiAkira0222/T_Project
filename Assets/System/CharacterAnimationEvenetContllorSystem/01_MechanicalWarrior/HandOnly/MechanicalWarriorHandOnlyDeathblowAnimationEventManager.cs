using Photon.Pun;

using System;
using System.Collections;
using System.Collections.Generic;

using Takechi.CharacterController.Address;
using Takechi.CharacterController.AnimationEvent;
using Takechi.CharacterController.KeyInputStete;
using Takechi.CharacterController.Parameters;
using Takechi.CharacterController.Reference;
using Takechi.CharacterController.SoundEffects;
using Takechi.UI.GameLogTextScrollView;
using UnityEngine;
using UnityEngine.Playables;

using static Takechi.ScriptReference.AnimatorControlVariables.ReferencingTheAnimatorControlVariablesName;
using static Takechi.ScriptReference.CustomPropertyKey.CustomPropertyKeyReference;

namespace Takechi.CharacterController.DeathblowAnimationEvent
{
    public class MechanicalWarriorHandOnlyDeathblowAnimationEventManager : AnimationEventManagement
    {
        #region SerializeField
        [Header("=== MechanicalWarriorAddressManagement === ")]
        [SerializeField] private MechanicalWarriorAddressManagement     m_mechanicalWarriorAddressManagement;
        [Header("=== MechanicalWarriorStatusManagement ===")]
        [SerializeField] private MechanicalWarriorStatusManagement      m_mechanicalWarriorStatusManagement;
        [Header("=== MechanicalWarriorSoundEffectsManagement ===")]
        [SerializeField] private MechanicalWarriorSoundEffectsManagement m_mechanicalWarriorSoundEffectsManagement;
        
        #endregion

        #region private variable
        private MechanicalWarriorAddressManagement addressManagement => m_mechanicalWarriorAddressManagement;
        private MechanicalWarriorStatusManagement  statusManagement => m_mechanicalWarriorStatusManagement;
        private MechanicalWarriorSoundEffectsManagement soundEffectsManagement => m_mechanicalWarriorSoundEffectsManagement;
        private GameLogTextScrollViewController logTextScrollViewController => addressManagement.GetGameLogTextScrollViewController();
        private PhotonView myPhotonView => addressManagement.GetMyPhotonView();
        private GameObject myAvater     => addressManagement.GetMyAvater();
        private PlayableAsset deathblowTimeline => addressManagement.GetDeathblowTimeline();
        private PlayableDirector myAvatarPlayableDirector => addressManagement.GetMyAvatarPlayableDirector();
        private Animator   networkModelAnimator => addressManagement.GetNetworkModelAnimator();
        private GameObject networkModelObject   => addressManagement.GetNetworkModelObject();
        private GameObject handOnlyModelObject  => addressManagement.GetHandOnlyModelObject();
        private GameObject bulletsInstans => addressManagement.GetDeathblowBulletsInstans();
        private Transform  magazineTransfrom => addressManagement.GetMagazineTransfrom();
        private Rigidbody  myRb     => addressManagement.GetMyRigidbody();
        private string bulletsPath  => addressManagement.GetDeathblowBulletsPath();
        private float  force => statusManagement.GetDeathblowShootingForce();
        private float  durationTime => statusManagement.GetDeathblowDurationOfBullet();
        /// <summary>
        /// 重みの一時保管
        /// </summary>
        private float m_networkModelAnimatorWeight = 0;
        #endregion

        #region UnityAnimatorEvent
        /// <summary>
        /// Mechanical Warrior Deathblow Start
        /// </summary>
        void MechanicalWarriorDeathblowStart()
        {
            // play and set playableDirector
            PlayAndSettingOfPlayableDirector();

            // rb
            statusManagement.SetIsKinematic(true);

            // operation
            keyInputStateManagement.SetOperation(false);

            // animation Controler
            controllerReferenceManagement.GetMovementAnimationControler().SetInterfere(false);

            // animation weiht
            m_networkModelAnimatorWeight = networkModelAnimator.GetLayerWeight( networkModelAnimator.GetLayerIndex(AnimatorLayers.overrideLayer));
            SetLayerWeight( networkModelAnimator, AnimatorLayers.overrideLayer, 0f);
            controllerReferenceManagement.GetIKAnimationController().SetAllIkWeight(0);

            // isMine active
            if (statusManagement.photonView.IsMine)
            {
                StateChangeOnCanvas(addressManagement.GetReticleCanvas());
                handOnlyModelObject.SetActive(false);
                networkModelObject.SetActive(true);

                // logText
                if (statusManagement.GetCustomPropertiesTeamName() == CharacterTeamStatusName.teamAName)
                {
                    logTextScrollViewController.AddTextContent($"<color=red>{PhotonNetwork.LocalPlayer.NickName}</color> 必殺技発動");
                }
                else if ((statusManagement.GetCustomPropertiesTeamName() == CharacterTeamStatusName.teamBName))
                {
                    logTextScrollViewController.AddTextContent($"<color=blue>{PhotonNetwork.LocalPlayer.NickName}</color> 必殺技発動");
                }
            }
        }

        /// <summary>
        /// Mechanical Warrior Deathblow Shoot
        /// </summary>
        void MechanicalWarriorDeathblowShoot()
        {
            // sound Effect
            soundEffectsManagement.PlayOneShotDeathbolwShoot();

            // not isMine active
            if (!myPhotonView.IsMine) return;

            Shooting( magazineTransfrom, force);

            myRb.transform.Translate( 0, 0, -1.5f);
        }

        /// <summary>
        /// Mechanical Warrior Deathblow Reload
        /// </summary>
        void MechanicalWarriorDeathblowReload()
        {
            // sound Effect
            soundEffectsManagement.PlayOneShotDeathbolwReload();
        }

        /// <summary>
        /// Mechanical Warrior Deathblow Cocking
        /// </summary>
        void MechanicalWarriorDeathblowCocking()
        {
            // sound Effect
            soundEffectsManagement.PlayOneShotDeathbolwCocking();
        }

        /// <summary>
        /// Mechanical Warrior Deathblow End
        /// </summary>
        void MechanicalWarriorDeathblowEnd()
        {
            // rb
            statusManagement.SetIsKinematic(false);

            // operation
            keyInputStateManagement.SetOperation(true);

            // animation Controler
            controllerReferenceManagement.GetMovementAnimationControler().SetInterfere(true);

            // animation weiht
            SetLayerWeight( networkModelAnimator, AnimatorLayers.overrideLayer, m_networkModelAnimatorWeight);
            controllerReferenceManagement.GetIKAnimationController().ResetAllIkWeight();

            // isMine active
            if (statusManagement.photonView.IsMine)
            {
                StateChangeOnCanvas(addressManagement.GetReticleCanvas());
                handOnlyModelObject.SetActive(true);
                networkModelObject.SetActive(false);
            }
        }

        #endregion

        #region Recursive function
        private void Shooting(Transform magazine, float force)
        {
            GameObject instans =
            PhotonNetwork.Instantiate( bulletsPath + bulletsInstans.name, magazine.position, Quaternion.identity);

            Rigidbody rb = instans.GetComponent<Rigidbody>();
            rb.AddForce( myAvater.transform.forward * force, ForceMode.Impulse);

            StartCoroutine( DelayMethod( durationTime, () => { PhotonNetwork.Destroy(instans); }));
        }

        /// <summary>
        /// PlayableDirector 再生と設定
        /// </summary>
        private void PlayAndSettingOfPlayableDirector()
        {
            // set playableDirector
            myAvatarPlayableDirector.playableAsset = deathblowTimeline;
            Debug.Log(" myAvatarPlayableDirector.playableAsset deathblowTimeline to set.");

            // playableDirector
            myAvatarPlayableDirector.Play();
        }

        #endregion
    }
}
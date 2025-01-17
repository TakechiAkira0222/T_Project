using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Takechi.CharacterController.Address;
using Takechi.CharacterController.AnimationEvent;
using Takechi.CharacterController.Parameters;
using Takechi.CharacterController.SoundEffects;
using UnityEngine;


namespace Takechi.CharacterController.AttackAnimationEvent
{
    public class SlimeHandOnlyAttackAnimationEventManager : AnimationEventManagement
    {
        #region SerializeField
        [Header("=== SlimeAddressManagement === ")]
        [SerializeField] private SlimeAddressManagement m_slimeAddressManagement;
        [Header("=== SlimeStatusManagement ===")]
        [SerializeField] private SlimeStatusManagement  m_slimeStatusManagement;
        [Header("=== SlimeSoundEffectsManagement ===")]
        [SerializeField] private SlimeSoundEffectsManagement m_slimeSoundEffectsManagement;

        #endregion

        #region private variable
        private SlimeAddressManagement addressManagement => m_slimeAddressManagement;
        private SlimeStatusManagement  statusManagement => m_slimeStatusManagement;
        private SlimeSoundEffectsManagement soundEffectsManagement => m_slimeSoundEffectsManagement;
        private PhotonView myPhotonView => addressManagement.GetMyPhotonView();
        private Transform  attackLaserPointTransfrom => addressManagement.GetAttackLaserPointTransfrom();
        private GameObject attackEffectInstans => addressManagement.GetAttackLaserEffectInstans();
        private string     attackEffectPath => addressManagement.GetAttackLaserEffectPath();
        private float  dsuration => statusManagement.GetAttackLaserEffectDsuration_Seconds();
        private bool   isMine => myPhotonView.IsMine;

        #endregion

        public void SlimeAttackStart()
        {
            soundEffectsManagement.PlayOneShotNormalShot();

            if (!isMine) return;
            Shooting( attackLaserPointTransfrom);
        }

        public void SlimeAttackEnd()
        {

        }

        #region Recursive function
        private void Shooting(Transform laserPoint) 
        { 
            GameObject instans = PhotonNetwork.Instantiate(attackEffectPath + attackEffectInstans.name, laserPoint.position, laserPoint.rotation);
            StartCoroutine( DelayMethod( dsuration,() => { PhotonNetwork.Destroy(instans); }));
        }
        #endregion
    }
}

using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

using Takechi.CharacterController.Address;
using Takechi.CharacterController.AnimationEvent;
using Takechi.CharacterController.Parameters;
using Takechi.CharacterController.Reference;
using static Takechi.ScriptReference.SearchForPrefabs.ReferencingSearchForPrefabs;
using static Takechi.ScriptReference.DamagesThePlayerObject.ReferencingObjectWithContactDetectionThePlayer;
using System.ComponentModel;

namespace Takechi.CharacterController.ThrowAnimationEvent
{
    public class OfficeWorkerHandOnlyThrowAnimationEventManager : AnimationEventManagement
    {
        #region SerializeField
        [Header("=== OfficeWorkerAddressManagement === ")]
        [SerializeField] private OfficeWorkerAddressManagement      m_officeWorkerAddressManagement;
        [Header("=== OfficeWorkerStatusManagement ===")]
        [SerializeField] private OfficeWorkerStatusManagement       m_officeWorkerStatusManagement;
        [Header("=== OfficeWorkerSoundEffectsManagement ===")]
        [SerializeField] private OfficeWorkerSoundEffectsManagement m_officeWorkerSoundEffectsManagement;
        #endregion

        #region private variable
        private OfficeWorkerAddressManagement addressManagement => m_officeWorkerAddressManagement;
        private OfficeWorkerSoundEffectsManagement soundEffectsManagement => m_officeWorkerSoundEffectsManagement;
        private OfficeWorkerStatusManagement statusManagement => m_officeWorkerStatusManagement;
        private GameObject throwInstans =>   addressManagement.GetThrowInstans();
        private Transform  throwTransfrom => addressManagement.GetThrowTransform();
        private string throwInstansPath =>   addressManagement.GetThrowInstansFolderNamePath();
        private float  force =>  statusManagement.GetThrowForce();
        private bool   isMine => addressManagement.GetMyPhotonView().IsMine;

        #endregion
        /// <summary>
        /// OfficeWorker Throw Start
        /// </summary>
        void OfficeWorkerThrowStart()
        {
            controllerReferenceManagement.GetIKAnimationController().SetAllIkWeight(0);
        }

        /// <summary>
        /// OfficeWorker Throw Timing
        /// </summary>
        void OfficeWorkerThrowTiming()
        {
            soundEffectsManagement.PlayOneShotVoiceOfThrowWatermelon();

            if (!isMine) return;
            throwing( throwTransfrom, force);
        }

        /// <summary>
        /// OfficeWorker Throw End
        /// </summary>
        void OfficeWorkerThrowEnd()
        {
            controllerReferenceManagement.GetIKAnimationController().ResetAllIkWeight();
        }

        private void throwing( Transform throwTransform, float force)
        {
            GameObject instans =
            PhotonNetwork.Instantiate( throwInstansPath + throwInstans.name, throwTransform.position, Quaternion.identity);

            instans.GetComponent<Rigidbody>().AddForce((throwTransform.forward + ( throwTransform.up / 10)) * force, ForceMode.Impulse);
        }
    }
}

using UnityEngine;
using Photon.Pun;

using System.Collections;
using System.Collections.Generic;

using Takechi.CharacterController.Parameters;
using Takechi.CharacterController.SpecificParameters.MechanicalWarreior;
using System.IO;
using System;

namespace Takechi.CharacterController.AttackAnimationEvent
{
    public class MechanicalWarriorHandOnlyAttackAnimationEventManager : MonoBehaviour
    {
        #region SerializeField

        [Header("=== CharacterStatusManagement ===")]
        [SerializeField] private CharacterStatusManagement m_characterStatusManagement;

        [Header("=== MechanicalWarreiorSpecificParameters ===")]
        [SerializeField] private MechanicalWarreiorSpecificParameters m_specificParameters;

        [Header("=== ScriptSetting ===")]
        [SerializeField] private PhotonView m_thisPhotonView;

        [SerializeField] private GameObject m_instans;
        [SerializeField] private Transform  m_magazineTransfrom;

     
        #endregion

        #region private
        private PhotonView thisPhotonView => m_thisPhotonView;
        private CharacterStatusManagement characterStatusManagement => m_characterStatusManagement;
        private float force => m_specificParameters.GetShootingForce();
        private float durationTime => m_specificParameters.GetDurationOfBullet();

        private string path => m_specificParameters.GetBulletsPath();


        #endregion

        private void Awake()
        {
            
        }

        private void Reset()
        {
            m_thisPhotonView = this.transform.GetComponent<PhotonView>();
        }

        #region UnityAnimatorEvent

        /// <summary>
        /// FirstShot Animation Start
        /// </summary>
        void MechanicalWarriorFirstShot()
        {
            if (!characterStatusManagement.GetMyPhotonView().IsMine) return;

            Shooting(m_magazineTransfrom, force);
        }

        /// <summary>
        /// SecondShot Animation End
        /// </summary>
        void MechanicalWarriorSecondShot()
        {
            if (!characterStatusManagement.GetMyPhotonView().IsMine) return;

            Shooting(m_magazineTransfrom, force);
        }

        /// <summary>
        /// ThirdShot Animation End
        /// </summary>
        void MechanicalWarriorThirdShot()
        {
            if (!characterStatusManagement.GetMyPhotonView().IsMine) return;
            Shooting(m_magazineTransfrom, force);
        }

        #endregion

        #region set Function

        #endregion

        #region Recursive function

        private void Shooting( Transform magazine, float force)
        {
            GameObject instans =
            PhotonNetwork.Instantiate( path + m_instans.name, magazine.position, Quaternion.identity);

            instans.GetComponent<Rigidbody>().AddForce( magazine.forward * 100 * force);

            StartCoroutine(DelayMethod( durationTime, () =>
            {
                PhotonNetwork.Destroy(instans);
            }));
        }

        private IEnumerator DelayMethod(float waitTime, Action action)
        {
            yield return new WaitForSeconds(waitTime);
            action();
        }

        #endregion
    }
}
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Takechi.CharacterController.Parameters;
using UnityEngine;

namespace Takechi.CharacterController.AttackAnimationEvent
{
    public class OfficeWorkerHandOnlyAttackAnimationEventManager : MonoBehaviour
    {
        #region SerializeField

        [Header("=== CharacterStatusManagement ===")]
        [SerializeField] private CharacterStatusManagement m_characterStatusManagement;

        [Header("=== ScriptSetting ===")]
        [SerializeField] private Rigidbody  m_myAvatarRb;
        [SerializeField] private PhotonView m_thisPhotonView;
        [SerializeField] private GameObject m_sword;
        [SerializeField] private GameObject m_swordEffectTrail;
        [SerializeField] private string m_targetTagName = "PlayerCharacter";
        [SerializeField, Range(1.0f, 2.5f)] private float m_withinRange = 1.3f;
        [SerializeField, Range(2.5f, 8f)] private float m_outOfRange = 5f;
        [SerializeField, Range(5, 15)] private int m_trackingFrame = 10;

        #endregion

        #region private
        private Collider m_swordCollider;

        #endregion

        #region UnityAnimatorEvent

        private void Awake()
        {
            m_swordCollider = m_sword.GetComponent<Collider>();
            Physics.IgnoreCollision(m_swordCollider, m_characterStatusManagement.Collider, false);
        }

        /// <summary>
        /// FastAttack Animation Start
        /// </summary>
        void OfficeWorkerFastAttackStart()
        {
            if (m_characterStatusManagement.PhotonView.IsMine)
            {
                StartCoroutine(nameof(AttackTowardsTarget));
            }
            else
            {
                setSwordStatus(true);
            }
        }

        /// <summary>
        /// FastAttack Animation End
        /// </summary>
        void OfficeWorkerFastAttackEnd()
        {
            if (m_characterStatusManagement.PhotonView.IsMine) return;

            setSwordStatus(false);
        }

        /// <summary>
        /// SecondAttack Animation Start
        /// </summary>
        void OfficeWorkerSecondAttackStart()
        {
            if (m_characterStatusManagement.PhotonView.IsMine)
            {
                StartCoroutine(nameof(AttackTowardsTarget));
            }
            else
            {
                setSwordStatus(true);
            }
        }

        /// <summary>
        /// SecondAttack Animation End
        /// </summary>
        void OfficeWorkerSecondAttackEnd()
        {
            if (m_characterStatusManagement.PhotonView.IsMine) return;

            setSwordStatus(false);
        }

        /// <summary>
        /// ThirdAttack Animation Start
        /// </summary>
        void OfficeWorkerThirdAttackStart()
        {
            if (m_characterStatusManagement.PhotonView.IsMine)
            {
                StartCoroutine(nameof(AttackTowardsTarget));
            }
            else
            {
                setSwordStatus(true);
            }
        }

        /// <summary>
        /// ThirdAttack Animation End
        /// </summary>
        void OfficeWorkerThirdAttackEnd()
        {
            if (m_characterStatusManagement.PhotonView.IsMine) return;

            setSwordStatus(false);
        }

        #endregion

        #region set Function
        void setSwordStatus(bool flag)
        {
            m_swordEffectTrail.SetActive(flag);
            m_swordCollider.enabled = flag;
        }

        #endregion

        #region Recursive function
        /// <summary>
        /// 攻撃時の非同期追従
        /// </summary>
        /// <returns></returns>
        private IEnumerator AttackTowardsTarget()
        {
            GameObject nearObj = serchTag(m_myAvatarRb.gameObject, m_targetTagName);

            Vector3 tagetDir = Vector3.Normalize(nearObj.transform.position - m_myAvatarRb.gameObject.transform.position);

            float dis = Vector3.Distance(nearObj.transform.position, m_myAvatarRb.gameObject.transform.position);

            if (dis > m_withinRange && dis < m_outOfRange)
            {
                for (int turn = 0; turn < m_trackingFrame; turn++)
                {
                    m_myAvatarRb.gameObject.transform.position += tagetDir / m_trackingFrame;
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }

        /// <summary>
        /// 一番近い距離の指定タグオブジェクトを知る。
        /// </summary>
        /// <param name="nowObj"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        private GameObject serchTag(GameObject nowObj, string tagName)
        {
            float tmpDis = 0;           //距離用一時変数
            float nearDis = 0;          //最も近いオブジェクトの距離
                                        //string nearObjName = "";    //オブジェクト名称
            GameObject targetObj = null; //オブジェクト

            //タグ指定されたオブジェクトを配列で取得する
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
            {
                //自身と取得したオブジェクトの距離を取得
                tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

                //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
                //一時変数に距離を格納
                if (nearDis == 0 || nearDis > tmpDis)
                {
                    nearDis = tmpDis;
                    //nearObjName = obs.name;
                    targetObj = obs;
                }
            }
            //最も近かったオブジェクトを返す
            //return GameObject.Find(nearObjName);
            return targetObj;
        }
        #endregion
    }
}
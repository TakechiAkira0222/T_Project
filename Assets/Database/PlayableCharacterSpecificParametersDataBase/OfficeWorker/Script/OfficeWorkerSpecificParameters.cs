using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Takechi.CharacterController.SpecificParameters.OfficeWorker
{
    [Serializable]
    [CreateAssetMenu(fileName = "OfficeWorkerSpecificParameters", menuName = "OfficeWorkerSpecificParameters")]
    public class OfficeWorkerSpecificParameters : ScriptableObject
    {
        #region SerializeField
        [Header("=== DeathblowStatusIncreaseSetting ===")]
        [SerializeField, Range(10.0f, 30.0f), Tooltip("�K�E�Z�̃X�e�[�^�X�㏸���@AttackPower")]
        private float m_attackPowerIncrease  = 30.0f;
        [SerializeField, Range(3.0f, 10.0f), Tooltip("�K�E�Z�̃X�e�[�^�X�㏸���@MoveingSpeed")]
        private float m_moveingSpeedIncrease = 30.0f;
        [SerializeField, Range(30.0f, 200.0f), Tooltip("�K�E�Z�̃X�e�[�^�X�㏸���@JumpPower")]
        private float m_jumpPowerIncrease    = 300.0f;
        [SerializeField, Tooltip("�K�E�Z�̌p�����ԁ@Seconds")]
        private float m_specialMoveDuration_Seconds = 30;
        [SerializeField, Tooltip(" �A�r���e�B 1�@��ԗ�")]
        private float m_ablity1FlyForce = 500;

        #endregion

        #region GetStatusFunction
        public float GetAttackPowerIncrease() { return m_attackPowerIncrease; }
        public float GetMoveingSpeedIncrease() { return m_moveingSpeedIncrease; }
        public float GetJumpPowerIncrease() { return m_jumpPowerIncrease; }
        public float GetSpecialMoveDuration_Seconds() {return m_specialMoveDuration_Seconds;}
        public float GetAblity1FlyForce() { return m_ablity1FlyForce; }
        #endregion
    }
}

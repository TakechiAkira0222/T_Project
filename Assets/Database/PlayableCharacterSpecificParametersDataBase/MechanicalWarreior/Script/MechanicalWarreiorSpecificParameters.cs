using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Takechi.CharacterController.SpecificParameters.MechanicalWarreior
{
    [Serializable]
    [CreateAssetMenu(fileName = "MechanicalWarreiorSpecificParameters", menuName = "MechanicalWarreiorSpecificParameters")]
    public class MechanicalWarreiorSpecificParameters : ScriptableObject
    {
        [SerializeField, Range( 1.0f, 10.0f),  Header(" ΛΜΠΝ@ΚνU ")]
        private float m_normalShootingForce = 3.0f;
        [SerializeField, Range( 1.0f, 10.0f), Header(" eΜΆέΤ Κν ")]
        private float m_normalDurationOfBullet = 5;
        [SerializeField, Range( 1.0f, 10.0f),  Header(" ΛΜΠΝ@KEZ ")]
        private float m_deathblowShootingForce = 3.0f;
        [SerializeField, Range( 3.0f, 10.0f), Header(" eΜΆέΤ KEZ ")]
        private float m_deathblowDurationOfBullet = 5;
        [SerializeField, Range(10, 30), Header(" EnemySearch p±Τ")]
        private float m_enemySearch_Seconds;
        [SerializeField, Range(20, 50), Header(" WallΜΆέΤ")]
        private float m_wallDuration_Seconds;

        #region GetStatusFunction
        public float GetNormalShootingForce() { return m_normalShootingForce; }
        public float GetNormalDurationOfBullet() { return m_normalDurationOfBullet; }
        public float GetDeathblowShootingForce() { return m_deathblowShootingForce; }
        public float GetDeathblowDurationOfBullet() { return m_deathblowDurationOfBullet; }
        public float GetEnemySearch_Seconds() { return m_enemySearch_Seconds; }
        public float GetWallDuration_Seconds() { return m_wallDuration_Seconds; }

        #endregion
    }
}

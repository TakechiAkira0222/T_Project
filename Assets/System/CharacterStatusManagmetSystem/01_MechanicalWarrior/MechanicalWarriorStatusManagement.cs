using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Takechi.CharacterController.SpecificParameters.MechanicalWarreior;

namespace Takechi.CharacterController.Parameters
{
    public class MechanicalWarriorStatusManagement : CharacterStatusManagement
    {
        [Header("=== MechanicalWarreiorStatus Setting===")]
        [SerializeField] private MechanicalWarreiorSpecificParameters m_mechanicalWarreiorSpecificParameters;

        private float m_normalShootingForce;
        private float m_normalDurationOfBullet;
        private float m_deathblowShootingForce;
        private float m_deathblowDurationOfBullet;
        private float m_enemySearch_Seconds;
        private float m_wallDuration_Seconds;
        private float m_smokeDuration_Seconds;
        private float m_throwSmokeForce;

        protected override void Awake()
        {
            base.Awake();

            SetupMechanicalWarreiorSpecificParameters();
         
            if (!base.thisPhotonView.IsMine) return;
        }

      
        #region set up function
        /// <summary>
        /// SpecificStatus を、データベースの変数で設定します。
        /// </summary>
        private void SetupMechanicalWarreiorSpecificParameters()
        {
            SetNormalShootingForce(m_mechanicalWarreiorSpecificParameters.GetNormalShootingForce());
            SetNormalDurationOfBullet(m_mechanicalWarreiorSpecificParameters.GetNormalDurationOfBullet());
            SetDeathblowShootingForce(m_mechanicalWarreiorSpecificParameters.GetDeathblowShootingForce());
            SetDeathblowDurationOfBullet(m_mechanicalWarreiorSpecificParameters.GetDeathblowDurationOfBullet());
            SetEnemySearch_Seconds(m_mechanicalWarreiorSpecificParameters.GetEnemySearch_Seconds());
            SetWallDuration_Seconds(m_mechanicalWarreiorSpecificParameters.GetWallDuration_Seconds());
            SetSmokeDuration_Seconds(m_mechanicalWarreiorSpecificParameters.GetSmokeDuration_Seconds());
            SetThrowSmokeForce(m_mechanicalWarreiorSpecificParameters.GetThrowSmokeForce());

            Debug.Log($"<color=green> setupMechanicalWarriorSpecificParameters </color>\n" +
                      $"<color=blue> info</color>\n" +
                      $" NickName : { PhotonNetwork.LocalPlayer.NickName} \n" +
                      $" m_normalShootingForce = { m_normalShootingForce}\n" +
                      $" m_deathblowShootingForce = { m_deathblowShootingForce}\n" +
                      $" m_normalDurationOfBullet = { m_normalDurationOfBullet}\n" +
                      $" m_deathblowDurationOfBullet = { m_deathblowDurationOfBullet}\n"+
                      $" m_normalDurationOfBullet = { m_normalDurationOfBullet}\n"+
                      $" m_wallDuration_Seconds = {m_wallDuration_Seconds}\n"+
                      $" m_smokeDuration_Seconds = {m_smokeDuration_Seconds}\n"+
                      $" m_throwSmokeForce = {m_throwSmokeForce}\n"
                      );
        }

        #endregion

        #region SetFunction
        public void SetNormalShootingForce(float changeValue)
        {
            Debug.Log($" normalShootingForce({ m_normalShootingForce}) = {changeValue}");
            m_normalShootingForce = changeValue;
        }
        public void SetNormalDurationOfBullet(float changeValue)
        {
            Debug.Log($" normalDurationOfBullet({ m_normalDurationOfBullet}) = {changeValue}");
            m_normalDurationOfBullet = changeValue;
        }
        public void SetDeathblowShootingForce(float changeValue)
        {
            Debug.Log($" deathblowShootingForce({ m_deathblowShootingForce}) = {changeValue}");
            m_deathblowShootingForce = changeValue;
        }
        public void SetDeathblowDurationOfBullet(float changeValue)
        {
            Debug.Log($" deathblowDurationOfBullet({ m_deathblowDurationOfBullet}) = {changeValue}");
            m_deathblowDurationOfBullet = changeValue;
        }
        public void SetEnemySearch_Seconds(float changeValue)
        {
            Debug.Log($" enemySearch_Seconds({m_enemySearch_Seconds}) = {changeValue}");
            m_enemySearch_Seconds = changeValue;
        }
        public void SetWallDuration_Seconds(float changeValue)
        {
            Debug.Log($" wallDuration_Seconds({m_wallDuration_Seconds}) = {changeValue}");
            m_wallDuration_Seconds = changeValue;
        }
        public void SetSmokeDuration_Seconds(float changeValue)
        {
            Debug.Log($" smokeDuration_Seconds({m_smokeDuration_Seconds}) = {changeValue}");
            m_smokeDuration_Seconds = changeValue;
        }
        public void SetThrowSmokeForce(float changeValue)
        {
            Debug.Log($" m_throwSmokeForce({m_throwSmokeForce}) = {changeValue}");
            m_throwSmokeForce = changeValue;
        }

        #endregion

        #region GetStatusFunction
        public float GetNormalShootingForce() { return m_normalShootingForce; }
        public float GetNormalDurationOfBullet() { return m_normalDurationOfBullet; }
        public float GetDeathblowShootingForce() { return m_deathblowShootingForce; }
        public float GetDeathblowDurationOfBullet() { return m_deathblowDurationOfBullet; }
        public float GetEnemySearch_Seconds() { return m_enemySearch_Seconds; }
        public float GetWallDuration_Seconds() { return m_wallDuration_Seconds; }
        public float GetSmokeDuration_Seconds() { return m_smokeDuration_Seconds; }
        public float GetThrowSmokeForce() { return m_throwSmokeForce; }

        #endregion
    }
}


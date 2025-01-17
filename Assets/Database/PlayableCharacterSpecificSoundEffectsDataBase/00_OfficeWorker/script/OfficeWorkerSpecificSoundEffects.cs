using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Takechi.CharacterController.SpecificSoundEffects.OfficeWorker
{
    [Serializable]
    [CreateAssetMenu(fileName = "OfficeWorkerSpecificSoundEffects", menuName = "OfficeWorkerData/OfficeWorkerSpecificSoundEffects")]
    public class OfficeWorkerSpecificSoundEffects : ScriptableObject
    {
        #region SerializeField

        [SerializeField, Header("通常攻撃1の声")] private AudioClip m_voiceOfFirstNormalAttackClip;
        [SerializeField, Range( 0f,1f)] private float m_voiceOfFirstNormalAttackVolume = 0.5f;

        [SerializeField, Header("通常攻撃2の声")] private AudioClip m_voiceOfSecondNormalAttackClip;
        [SerializeField, Range( 0f,1f)] private float m_voiceOfSecondNormalAttackVolume = 0.5f;

        [SerializeField, Header("必殺技 叫び")] private AudioClip m_voiceOfCallOutClip;
        [SerializeField, Range( 0f,1f)] private float m_voiceOfCallOutVolume = 0.5f;

        [SerializeField, Header("必殺技　始まりの声")] private AudioClip m_voiceOfDeathblowStartClip;
        [SerializeField, Range( 0f, 1f)] private float m_voiceOfDeathblowStartVolume = 0.5f;

        [SerializeField, Header("スイカを投げる声")] private AudioClip m_voiceOfThrowWatermelonClip;
        [SerializeField, Range(0f, 1f)] private float m_voiceOfThrowWatermelonVolume = 0.5f;

        [SerializeField, Header("必殺技　パワーアップ")] private AudioClip m_deathblowPowerUpClip;
        [SerializeField, Range( 0f, 1f)] private float m_deathblowPowerUpVolume = 0.5f;

        [SerializeField, Header("通常攻撃1")] private AudioClip m_firstNormalAttackClip;
        [SerializeField, Range( 0f, 1f)] private float m_firstNormalAttackVolume = 0.5f;

        [SerializeField, Header("通常攻撃2")] private AudioClip m_secondNormalAttackClip;
        [SerializeField, Range( 0f, 1f)] private float m_secondNormalAttackVolume = 0.5f;

        [SerializeField, Header("飛ぶ力")] private AudioClip m_flyingClip;
        [SerializeField, Range(0f, 1f)] private float m_flyingVolume = 0.5f;

        [SerializeField, Header("祈りの効果音")] private AudioClip m_praySoundClip;
        [SerializeField, Range(0f, 1f)] private float m_praySoundVolume = 0.5f;

        [SerializeField, Header("スイカが割れる音")] private AudioClip m_watermelonBreakingClip;
        [SerializeField, Range(0f, 1f)] private float m_watermelonBreakingVolume = 0.5f;

        #endregion

        #region GetAudioClipFunction
        public AudioClip GetVoiceOfFirstNormalAttackClip()  { return m_voiceOfFirstNormalAttackClip; }
        public AudioClip GetVoiceOfSecondNormalAttackClip() { return m_voiceOfSecondNormalAttackClip; }
        public AudioClip GetVoiceOfCallOutClip() { return m_voiceOfCallOutClip; }
        public AudioClip GetVoiceOfDeathblowStartClip() { return m_voiceOfDeathblowStartClip; }
        public AudioClip GetFirstNormalAttackClip()  { return m_firstNormalAttackClip; }
        public AudioClip GetSecondNormalAttackClip() { return m_secondNormalAttackClip; }
        public AudioClip GetDeathblowPowerUpClip() { return m_deathblowPowerUpClip; }
        public AudioClip GetFlyingClip() { return m_flyingClip; }
        public AudioClip GetPraySoundClip() => m_praySoundClip;
        public AudioClip GetVoiceOfThrowWatermelonClip() => m_voiceOfThrowWatermelonClip;
        public AudioClip GetWatermelonBreakingClip() => m_watermelonBreakingClip;

        public float GetVoiceOfFirstNormalAttackVolume() { return m_voiceOfFirstNormalAttackVolume; }
        public float GetVoiceOfSecondNormalAttackVolume() { return m_voiceOfSecondNormalAttackVolume; }
        public float GetVoiceOfCallOutVolume() { return m_voiceOfCallOutVolume; }
        public float GetVoiceOfDeathblowStartVolume() { return m_voiceOfDeathblowStartVolume; }
        public float GetFirstNormalAttackVolume()  { return m_firstNormalAttackVolume; }
        public float GetSecondNormalAttackVolume() { return m_secondNormalAttackVolume; }
        public float GetDeathblowPowerUpVolume()   { return m_deathblowPowerUpVolume; }
        public float GetFlyingVolume() { return m_flyingVolume; }
        public float GetPraySoundVolume() => m_praySoundVolume;
        public float GetVoiceOfThrowWatermelonVolume() => m_voiceOfThrowWatermelonVolume;
        public float GetWatermelonBreakingVolume() => m_watermelonBreakingVolume;
        #endregion
    }
}

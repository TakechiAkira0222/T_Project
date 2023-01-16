using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Takechi.CharacterController.SoundEffects;
using Takechi.CharacterController.SpecificParameters.MechanicalWarreior;
using Takechi.CharacterController.SpecificSoundEffects.MechanicalWarreior;
using UnityEngine;

namespace Takechi.CharacterController.Parameters
{
    public class MechanicalWarriorSoundEffectsManagement : CharacterBasicSoundEffectsStateManagement
    {
        [Header("=== MechanicalWarreiorSpecificSoundEffects ===")]
        [SerializeField] private MechanicalWarreiorSpecificSoundEffects  mechanicalWarreiorSpecificSoundEffects;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}


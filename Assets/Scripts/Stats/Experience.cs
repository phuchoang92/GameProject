using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float EXP = 0;

        public object CaptureState()
        {
            return EXP;
        }

        public void GainExperience(float experience)
        {
            EXP += experience;
        }

        public void RestoreState(object state)
        {
            EXP = (float)state;
        }

        public float GetEXP()
        {
            return EXP;
        }
    }
}

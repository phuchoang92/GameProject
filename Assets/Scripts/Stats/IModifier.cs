using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Stats
{
    public interface IModifier
    {
        IEnumerable<float> GetAdditiveModifiers(Stats stats);
        IEnumerable<float> GetPercentageModifiers(Stats stats);
    }
}
using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_hod_latitia3 : EmotionCardAbilityBase
    {

        public override void ChangeDiceResult(BattleDiceBehavior behavior, ref int diceResult)
        {
            base.ChangeDiceResult(behavior, ref diceResult);
            if (behavior.Index != 0)
                return;
            if (RandomUtil.valueForProb <= 0.5)
            {
                diceResult = behavior.GetDiceMax();
            }
            else
            {
                diceResult = 1;
                _owner.battleCardResultLog?.SetCreatureAbilityEffect("3/Latitia_Boom", 1.5f);
                _owner.TakeDamage(RandomUtil.Range(2, 7), DamageType.Emotion, _owner);
            }
        }
    }
}

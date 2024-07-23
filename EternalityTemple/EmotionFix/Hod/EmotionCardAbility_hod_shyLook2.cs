using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Hod
{
    public class EmotionCardAbility_hod_shyLook2 : EmotionCardAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.Detail != BehaviourDetail.Guard)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = RandomUtil.Range(1, 2),
                guardBreakAdder = RandomUtil.Range(2, 7)
            });
        }

        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            if (behavior.Detail != BehaviourDetail.Guard)
                return;
            _owner.battleCardResultLog?.SetCreatureAbilityEffect("3/ShyLookToday_Guard", 0.8f);
        }
    }
}

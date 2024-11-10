using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_chesed_scarecrow2 : EmotionCardAbilityBase
    {
        private bool trigger;
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            BattleUnitModel target = behavior.card?.target;
            if (target == null || target.breakDetail.breakGauge <= _owner.breakDetail.breakGauge)
                return;
            trigger = true;
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            _owner.breakDetail.RecoverBreak(RandomUtil.Range(2, 5));
            if (!trigger)
                return;
            trigger = false;
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Scarecrow_Drink");
            behavior?.card?.target.TakeBreakDamage(RandomUtil.Range(2, 4), DamageType.Emotion, _owner);
        }
    }
}

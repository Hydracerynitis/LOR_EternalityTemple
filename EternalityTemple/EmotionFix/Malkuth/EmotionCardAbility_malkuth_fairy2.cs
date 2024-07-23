using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Malkuth
{
    public class EmotionCardAbility_malkuth_fairy2 : EmotionCardAbilityBase
    {
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            BattleUnitModel target = behavior.card.target;
            if (target == null || target.history.takeDamageAtOneRound <= 0)
                return;
            int num = RandomUtil.Range(1, 3);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = num
            });
            target.battleCardResultLog?.SetCreatureAbilityEffect("1/Fairy_Gluttony2");
            _owner.RecoverHP(Math.Max(2,(int)(0.02*_owner.MaxHp)));
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Fairy_Dead");
        }
    }
}

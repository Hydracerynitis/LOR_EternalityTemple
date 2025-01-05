using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_binah_longbird1 : EmotionCardAbilityBase
    {
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            base.OnRollDice(behavior);
            if (behavior.DiceVanillaValue > behavior.GetDiceMin())
                return;
            _owner.TakeDamage((int)(_owner.MaxHp*0.05), DamageType.Emotion, _owner);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = RandomUtil.Range(2, 3) });
            _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Judgement", 3f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/LongBird_Stun");
        }
    }
}

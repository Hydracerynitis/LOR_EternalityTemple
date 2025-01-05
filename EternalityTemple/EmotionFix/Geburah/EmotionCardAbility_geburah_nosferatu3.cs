using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_geburah_nosferatu3 : EmotionCardAbilityBase
    {
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            base.OnRollDice(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || target.hp>target.MaxHp/2)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = RandomUtil.Range(2,4)
            });
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || target.hp > target.MaxHp / 2)
                return;
            target.battleCardResultLog?.SetCreatureAbilityEffect("6/Nosferatu_Emotion_Bat", 3f);
            target.battleCardResultLog?.SetCreatureEffectSound("Nosferatu_Atk_Bat");
            _owner.RecoverHP(RandomUtil.Range(2,5));
        }
    }
}


using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_geburah_nosferatu1 : EmotionCardAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (!IsAttackDice(behavior.Detail))
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = RandomUtil.Range(3, 7)
            });
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            _owner.LoseHp(RandomUtil.Range(2, 7));
            behavior?.card?.target?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 2, _owner);
            behavior?.card?.target?.battleCardResultLog?.SetCreatureAbilityEffect("6/Nosferatu_Emotion_BloodDrain");
            behavior?.card?.target?.battleCardResultLog?.SetCreatureEffectSound("Nosferatu_Changed_BloodEat");
        }
    }
}


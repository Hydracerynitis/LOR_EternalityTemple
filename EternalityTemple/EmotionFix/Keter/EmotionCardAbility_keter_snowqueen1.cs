using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_keter_snowqueen1 : EmotionCardAbilityBase
    {
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || RandomUtil.valueForProb > 0.5)
                return;
            target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, RandomUtil.Range(1, 3), _owner);
            if (target.bufListDetail.GetReadyBufList().Find(x => x is EmotionCardAbility_snowqueen1.BattleUnitBuf_Emotion_Snowqueen_Aura) != null)
                return;
            target.bufListDetail.AddReadyBuf(new EmotionCardAbility_snowqueen1.BattleUnitBuf_Emotion_Snowqueen_Aura());
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || !IsAttackDice(behavior.Detail) || target.bufListDetail.GetActivatedBufList().Find(x => x.bufType == KeywordBuf.Binding) == null)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                breakDmg = RandomUtil.Range(2, 5)
            });
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_SnowUnATK", 2f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/SnowQueen_Atk");
        }
    }
}

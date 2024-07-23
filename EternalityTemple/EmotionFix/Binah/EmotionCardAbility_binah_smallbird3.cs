using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix.Binah
{
    public class EmotionCardAbility_binah_smallbird3 : EmotionCardAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.Detail != BehaviourDetail.Evasion)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = RandomUtil.Range(1,2)
            });
        }

        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            if (behavior.Detail != BehaviourDetail.Evasion)
                return;
            BattlePlayingCardDataInUnitModel card = behavior.card;
            if (card != null)
                card.ApplyDiceStatBonus(DiceMatch.NextAttackDice, new DiceStatBonus()
                {
                    power = RandomUtil.Range(1,2)
                });
            _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Feather", 3f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Smallbird_Wing");
        }
    }
}

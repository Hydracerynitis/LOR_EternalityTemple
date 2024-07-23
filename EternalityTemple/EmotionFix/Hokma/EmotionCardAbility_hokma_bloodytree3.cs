using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix.Hokma
{
    public class EmotionCardAbility_hokma_bloodytree3 : EmotionCardAbilityBase
    {
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            if (behavior.Detail != BehaviourDetail.Guard)
                return;
            _owner.battleCardResultLog?.SetCreatureAbilityEffect("9/HokmaFirst_Guard", 0.8f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/SnowWhite_NormalAtk");
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null)
                return;
            BattlePlayingCardDataInUnitModel currentDiceAction = target.currentDiceAction;
            if (currentDiceAction != null)
            {
                BattleDiceBehavior currentBehavior = currentDiceAction.currentBehavior;
                if (currentBehavior != null)
                    currentBehavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        guardBreakMultiplier = 2
                    });
            }
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                guardBreakMultiplier = 2
            });
        }
    }
}

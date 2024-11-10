using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_hod_shyLook3 : EmotionCardAbilityBase
    {
        private int count;
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            count = 0;
            foreach (DiceBehaviour originalDiceBehavior in curCard.GetOriginalDiceBehaviorList())
            {
                if (originalDiceBehavior.Type == BehaviourType.Def)
                    ++count;
            }
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.Detail != BehaviourDetail.Guard)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = count
            });
        }
    }
}

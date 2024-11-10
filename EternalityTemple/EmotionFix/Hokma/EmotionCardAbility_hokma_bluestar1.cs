using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_hokma_bluestar1 : EmotionCardAbilityBase
    {
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            if (behavior == null)
                return;
            int dmg = RandomUtil.Range(2, 4);
            _owner.LoseHp(dmg);
            _owner.view.Damaged(dmg, BehaviourDetail.None, dmg, _owner);
            _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("9_H/FX_IllusionCard_9_H_Martyr", 3f);
            SoundEffectPlayer.PlaySound("Creature/BlueStar_In");
            double ratio = 1 - (_owner.hp / _owner.MaxHp);
            double breakrate = ratio/0.75;
            if (breakrate >= 1)
                breakrate = 1;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                breakRate = (int)(breakrate * 100)
            });
            behavior.card.target.battleCardResultLog?.SetNewCreatureAbilityEffect("9_H/FX_IllusionCard_9_H_MartyrExplo", 3f);
        }
    }
}

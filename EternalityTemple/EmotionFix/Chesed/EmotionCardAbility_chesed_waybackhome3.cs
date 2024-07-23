using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Chesed
{
    public class EmotionCardAbility_chesed_waybackhome3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            _owner.bufListDetail.AddBuf(new Lion());
        }

        public override void OnWaveStart()
        {
            _owner.bufListDetail.AddBuf(new Lion());
        }
        public class Lion : BattleUnitBuf
        {
            public override string keywordId => "EF_Lion";
            public override string keywordIconId => "Wizard_Lion";
            private int Pow => RandomUtil.Range(1, 2);
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                base.OnStartParrying(card);
                if (card.earlyTarget==card.target)
                    card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
                    {
                        power = Pow
                    });
                _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Together", 2f);
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/House_MakeRoad");
            }
        }
    }
}

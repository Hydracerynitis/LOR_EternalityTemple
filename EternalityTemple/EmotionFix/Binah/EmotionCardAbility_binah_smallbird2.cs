using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_binah_smallbird2 : EmotionCardAbilityBase
    {
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (_owner.history.takeDamageAtOneRound > 0)
                return;
            _owner.bufListDetail.AddBuf(new Beak());
        }

        public class Beak : BattleUnitBuf
        {
            private int Dmg => RandomUtil.Range(3, 7);
            public override string keywordId => "EF_Beak";
            public override string keywordIconId => "SmallBird_Emotion_Buri";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void BeforeGiveDamage(BattleDiceBehavior behavior)
            {
                base.BeforeGiveDamage(behavior);
                int dmg = Dmg;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = dmg,
                    breakDmg = dmg
                });
                behavior.card.target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Attack", 2f);
                behavior.card.target.battleCardResultLog?.SetCreatureEffectSound("Creature/SmallBird_Atk");
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}

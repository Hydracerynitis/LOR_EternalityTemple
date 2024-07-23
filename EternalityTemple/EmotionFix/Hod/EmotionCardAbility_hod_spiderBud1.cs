using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Hod
{
    public class EmotionCardAbility_hod_spiderBud1 : EmotionCardAbilityBase
    {
        private bool Trigger;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            Trigger = false;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior.DiceVanillaValue != behavior.GetDiceMax() || Trigger)
                return;
            BattleUnitModel target = behavior.card.target;
            if (target == null)
                return;
            target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Paralysis, 3, _owner);
            target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, 3, _owner);
            target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 3, _owner);
            behavior.card.target?.battleCardResultLog?.SetCreatureAbilityEffect("3/Spider_Cocoon", 2f);
            Trigger = true;
        }
    }
}

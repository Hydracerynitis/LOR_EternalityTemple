using System;
using System.Reflection;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_tiphereth_clownofnihil2 : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            behavior.card.target.allyCardDetail.ExhaustACard(RandomUtil.SelectOne(behavior.card.target.allyCardDetail.GetHand()));
            if(behavior.card.target.bufListDetail.GetActivatedBufList().Find(x => x.GetType() == typeof(VoidBuf))==null)
                behavior.card.target.bufListDetail.AddBuf(new VoidBuf());
        }
        public class VoidBuf: BattleUnitBuf
        {
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 2);
            }
        }
    }
}

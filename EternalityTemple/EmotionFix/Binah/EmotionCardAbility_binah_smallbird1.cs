using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using Punish = EmotionCardAbility_smallbird1.BattleUnitBuf_Emotion_SmallBird_Punish;
using Sound;

namespace EternalityEmotion
{
    public class EmotionCardAbility_smallbird1 : EmotionCardAbilityBase
    {
        private bool dmged;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            dmged = false;
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            base.BeforeTakeDamage(attacker, dmg);
            if (_owner.IsImmuneDmg() || _owner.IsInvincibleHp(attacker))
                return false;
            dmged = true;
            return false;
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            if (dmged)
                _owner.bufListDetail.AddBuf(new Punish());
            dmged = false;
        }
    }
}

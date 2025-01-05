using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_geburah_nosferatu2 : EmotionCardAbilityBase
    {
        private bool _trigger;
        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_TeathATK");
            target.battleCardResultLog?.SetCreatureAbilityEffect("6/Nosferatu_Emotion_BloodDrain");
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Nosferatu_Change");
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
                alive.RecoverHP(10);
            _trigger = true;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!_trigger)
                return;
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 2);
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction).FindAll(x=> x!=_owner))
                alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 2);
            _trigger = false;
        }
    }
}


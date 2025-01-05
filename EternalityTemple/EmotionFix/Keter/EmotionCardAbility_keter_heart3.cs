using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_keter_heart3 : EmotionCardAbilityBase
    {
        private int count;

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            count = 0;
            DiceEffectManager.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_FastBeat", 1f, _owner.view, _owner.view);
            SoundEffectManager.Instance.PlayClip("Creature/Heart_Fast")?.SetGlobalPosition(_owner.view.WorldPosition);
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            DiceEffectManager.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_FastBeat", 1f, _owner.view, _owner.view);
            SoundEffectManager.Instance.PlayClip("Creature/Heart_Fast")?.SetGlobalPosition(_owner.view.WorldPosition);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (count > 0)
                return;
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 4, _owner);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 4, _owner);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 4, _owner);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, 4, _owner);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            _owner.bufListDetail.AddBuf(new Exhaust());
            count += 1;
        }
        public class Exhaust: BattleUnitBuf
        {
            public override string keywordIconId => "Stun";
            public override string keywordId => "EF_HeartExhaust";
            public override int SpeedDiceBreakedAdder() => 100;
            public override int GetDamageReduction(BattleDiceBehavior behavior) => -2;
            public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail) => -2;
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void OnRoundEnd()
            {
                Destroy();
            }
        }
    }
}

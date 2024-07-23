using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Hod
{
    public class EmotionCardAbility_hod_latitia1 : EmotionCardAbilityBase
    {
        private BattleUnitModel _target;
        public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
        {
            base.OnParryingStart(card);
            BattleUnitModel target = card?.target;
            if (target == null || _target != null)
                return;
            BattleUnitBuf_Emotion_Latitia_Gift emotionLatitiaGift = new BattleUnitBuf_Emotion_Latitia_Gift(_owner);
            target.bufListDetail.AddBuf(emotionLatitiaGift);
            _target = target;
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _target = null;
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
            if (unit != _target)
                return;
            _target = null;
        }
        public class BattleUnitBuf_Emotion_Latitia_Gift : BattleUnitBuf
        {
            private BattleUnitModel _giver;
            public override string keywordId => "EF_Gift_Eternal";
            public override string keywordIconId => "Latitia_Heart";
            public override string bufActivatedText => BattleEffectTextsXmlList.Instance.GetEffectTextDesc(keywordId, _giver == null ? "██" : _giver.UnitData.unitData.name);
            public BattleUnitBuf_Emotion_Latitia_Gift(BattleUnitModel giver) => _giver = giver;
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                base.OnStartParrying(card);
                BattleUnitModel target = card?.target;
                if (target == null || _giver == null || RandomUtil.valueForProb>0.5)
                    return;
                _owner.battleCardResultLog?.SetCreatureAbilityEffect("3/Latitia_Boom", 1.5f);
                _owner.TakeDamage(RandomUtil.Range(3,8), DamageType.Buf,_giver);
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Bleeding, 2, _giver);
            }
        }
    }
}

using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix.Tiphereth
{
    public class EmotionCardAbility_tiphereth_knightofdespair2 : EmotionCardAbilityBase
    {
        private int stack;
        private SpriteFilter_Despair _filter;
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            base.OnDieOtherUnit(unit);
            if (unit.faction != _owner.faction)
                return;
            ++stack;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStartOnce();
            for(;stack>0 ;stack--)
            {
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, RandomUtil.Range(2, 3));
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.DmgUp, RandomUtil.Range(2, 3));
                if (Helper.SearchEmotion(_owner, "NihilClown_fusion_Enemy") == null)
                {
                    int num = (int)(_owner.breakDetail.GetDefaultBreakGauge() * 0.25);
                    _owner.breakDetail.LoseBreakGauge(num);
                    _owner.view.BreakDamaged(num, BehaviourDetail.Penetrate, _owner, AtkResist.Normal);
                }
                if (_filter == null)
                {
                    _filter = new GameObject().AddComponent<SpriteFilter_Despair>();
                    _filter.Init("EmotionCardFilter/KnightOfDespair_Gaho", true, 1f);
                }
            }
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            DestroyFilter();
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyFilter();
        }
        public void DestroyFilter()
        {
            if (_filter == null)
                return;
            _filter.ManualDestroy();
            _filter = null;
        }
    }
}

using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using BaseMod;

namespace EmotionalFix.Hod
{
    public class EmotionCardAbility_hod_blackswan3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _owner.bufListDetail.AddBufByEtc<Nettle>(6);
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/BlackSwan_Filter_Nettle", false, 2f);
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.bufListDetail.AddBufByEtc<Nettle>(6);
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/BlackSwan_Filter_Nettle", false, 2f);
        }
        public class Nettle : BattleUnitBuf
        {
            public override string keywordId => "EF_Nettle_Eternal";
            public override string keywordIconId => "BlackSwan_Nettle";

            public override void OnRoundStart()
            {
                base.OnRoundStart();
                foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(_owner.faction))
                {
                    unit.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Protection, stack);
                    unit.bufListDetail.AddKeywordBufByEtc(KeywordBuf.BreakProtection, stack);
                }
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                stack--;
                if(stack <= 0)
                    Destroy();
            }
        }
    }
}

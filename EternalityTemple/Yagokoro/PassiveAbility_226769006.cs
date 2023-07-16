using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Yagokoro
{
    public class PassiveAbility_226769006: PassiveAbilityBase
    {
        private static KeywordBuf[] HitInflict = new KeywordBuf[] { KeywordBuf.Bleeding, KeywordBuf.Burn, KeywordBuf.Decay, KeywordBuf.Vulnerable, KeywordBuf.Paralysis };
        private static KeywordBuf[] SlashInflict = new KeywordBuf[] { KeywordBuf.Bleeding, KeywordBuf.Burn, KeywordBuf.Binding, KeywordBuf.Disarm, KeywordBuf.Smoke };
        private static KeywordBuf[] PenetrateInflict = new KeywordBuf[] { KeywordBuf.Bleeding, KeywordBuf.Burn, KeywordBuf.Vulnerable_break, KeywordBuf.Weak, KeywordBuf.Vulnerable };
        private static KeywordBuf[] DefenseGain = new KeywordBuf[] { KeywordBuf.Protection, KeywordBuf.Endurance,KeywordBuf.BreakProtection};
        private static KeywordBuf[] EvadeGain = new KeywordBuf[] { KeywordBuf.Quickness, KeywordBuf.Protection, KeywordBuf.Strength };

        private int DefenseCharge;
        public override void OnRoundStart()
        {
            DefenseCharge = 0;
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            BattleUnitModel target = behavior.card.target;
            switch (behavior.Detail)
            {
                case (BehaviourDetail.Hit):
                    target.bufListDetail.AddKeywordBufByEtc(RandomUtil.SelectOne(HitInflict), 1);
                    break;
                case (BehaviourDetail.Slash):
                    target.bufListDetail.AddKeywordBufByEtc(RandomUtil.SelectOne(SlashInflict), 1);
                    break;
                case (BehaviourDetail.Penetrate):
                    target.bufListDetail.AddKeywordBufByEtc(RandomUtil.SelectOne(PenetrateInflict), 1);
                    break;
                case (BehaviourDetail.Guard):
                    if (DefenseCharge > 3)
                        return;
                    owner.bufListDetail.AddKeywordBufByEtc(RandomUtil.SelectOne(DefenseGain), 1);
                    DefenseCharge++;
                    break;
                case (BehaviourDetail.Evasion):
                    if (DefenseCharge > 3)
                        return;
                    owner.bufListDetail.AddKeywordBufByEtc(RandomUtil.SelectOne(EvadeGain), 1);
                    DefenseCharge++;
                    break;
            }
        }
    }
}

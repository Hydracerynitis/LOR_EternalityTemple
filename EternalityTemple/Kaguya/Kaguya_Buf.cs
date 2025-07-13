using EternalityTemple.Inaba;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EternalityTemple.Kaguya
{
    public class BattleUnitBuf_KaguyaBuf : BattleUnitBuf
    {
        public override string keywordId => "KaguyaBuf" + stack.ToString();
        public override string keywordIconId => "Kaguya_Buf时辰11";
        public override string bufActivatedText => Singleton<BattleEffectTextsXmlList>.Instance.GetEffectTextDesc("KaguyaBuf" + stack.ToString(), paramInBufDesc);
        public BattleUnitBuf_KaguyaBuf(int stack)
        {
            this.stack = stack;
            _bufIcon = EternalityInitializer.ArtWorks["Kaguya_Buf时辰11"];
            _iconInit = true;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if(stack >= 1)  
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1, breakDmg = 1 });
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            if (stack >= 2)
                return -1;
            else
                return base.GetDamageReduction(behavior);
        }
        public override int SpeedDiceNumAdder()
        {
            if (stack >= 7)
                return 1;
            return base.SpeedDiceNumAdder();
        }
        public override void OnRoundStart()
        {
            if (stack >= 3)
                _owner.cardSlotDetail.RecoverPlayPoint(1);
            if (stack >= 4)
                _owner.allyCardDetail.DrawCards(1);
            if (stack >= 5)
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 1);
            if (stack >= 6)
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
            if (stack >= 7)
                _owner.allyCardDetail.DrawCards(1);
        }
        public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail)
        {
            if (stack >= 2)
                return -1;
            else
                return base.GetBreakDamageReduction(behaviourDetail);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            if (stack < 7)
            {
                stack++;
                _bufIcon = EternalityInitializer.ArtWorks["Kaguya_Buf时辰" + (9 + stack * 2)];
            }
                
        }
        public static int GetStack(BattleUnitModel unit)
        {
            BattleUnitBuf_KaguyaBuf battleUnitBuf = unit.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_KaguyaBuf) as BattleUnitBuf_KaguyaBuf;
            if (battleUnitBuf == null)
                return -1;
            else
                return battleUnitBuf.stack;
        }
    }
}

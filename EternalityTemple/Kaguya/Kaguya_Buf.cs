using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EternalityTemple.Kaguya
{
    public class KaguyaBuf : BattleUnitBuf
    {
        public override string keywordIconId => "Silence_StopTime";
    }
    public class BattleUnitBuf_KaguyaBuf1: KaguyaBuf
    {
        public override string keywordId => "KaguyaBuf1";
        public override void Init(BattleUnitModel owner)
        {
            stack = 1;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1, breakDmg = 1 });
        }
        public override void OnRoundEnd()
        {
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_KaguyaBuf2());
            Destroy();
        }
    }
    public class BattleUnitBuf_KaguyaBuf2: KaguyaBuf
    {
        public override string keywordId => "KaguyaBuf2";
        public override void Init(BattleUnitModel owner)
        {
            stack = 2;
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            return -1;
        }
        public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail)
        {
            return -1;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1, breakDmg = 1 });
        }
        public override void OnRoundEnd()
        {
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_KaguyaBuf3());
            Destroy();
        }
    }
    public class BattleUnitBuf_KaguyaBuf3 : KaguyaBuf
    {
        public override string keywordId => "KaguyaBuf3";
        public override void Init(BattleUnitModel owner)
        {
            stack = 3;
        }
        public override void OnRoundStart()
        {
            _owner.cardSlotDetail.RecoverPlayPoint(1);
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            return -1;
        }
        public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail)
        {
            return -1;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1, breakDmg = 1 });
        }
        public override void OnRoundEnd()
        {
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_KaguyaBuf4());
            Destroy();
        }
    }
    public class BattleUnitBuf_KaguyaBuf4 : KaguyaBuf
    {
        public override string keywordId => "KaguyaBuf4";
        public override void Init(BattleUnitModel owner)
        {
            stack = 4;
        }
        public override void OnRoundStart()
        {
            _owner.allyCardDetail.DrawCards(1);
            _owner.cardSlotDetail.RecoverPlayPointByCard(1);
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            return -1;
        }
        public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail)
        {
            return -1;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1, breakDmg = 1 });
        }
        public override void OnRoundEnd()
        {
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_KaguyaBuf5());
            Destroy();
        }
    }
    public class BattleUnitBuf_KaguyaBuf5 : KaguyaBuf
    {
        public override string keywordId => "KaguyaBuf5";
        public override void Init(BattleUnitModel owner)
        {
            stack = 5;
        }
        public override void OnRoundStart()
        {
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 1);
            _owner.allyCardDetail.DrawCards(1);
            _owner.cardSlotDetail.RecoverPlayPointByCard(1);
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            return -1;
        }
        public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail)
        {
            return -1;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1, breakDmg = 1 });
        }
        public override void OnRoundEnd()
        {
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_KaguyaBuf6());
            Destroy();
        }
    }
    public class BattleUnitBuf_KaguyaBuf6 : KaguyaBuf
    {
        public override string keywordId => "KaguyaBuf6";
        public override void Init(BattleUnitModel owner)
        {
            stack = 6;
        }
        public override void OnRoundStart()
        {
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 1);
            _owner.allyCardDetail.DrawCards(1);
            _owner.cardSlotDetail.RecoverPlayPointByCard(1);
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            return -1;
        }
        public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail)
        {
            return -1;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1, breakDmg = 1 });
        }
        public override void OnRoundEnd()
        {
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_KaguyaBuf7());
            Destroy();
        }
    }
    public class BattleUnitBuf_KaguyaBuf7 : KaguyaBuf
    {
        public override string keywordId => "KaguyaBuf7";
        public override void Init(BattleUnitModel owner)
        {
            stack = 7;
        }
        public override void OnRoundStart()
        {
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, 1);
            _owner.allyCardDetail.DrawCards(2);
            _owner.cardSlotDetail.RecoverPlayPointByCard(1);
        }
        public override int GetDamageReduction(BattleDiceBehavior behavior)
        {
            return -1;
        }
        public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail)
        {
            return -1;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { dmg = 1, breakDmg = 1 });
        }
        public override int SpeedDiceNumAdder()
        {
            return 1;
        }
    }
}

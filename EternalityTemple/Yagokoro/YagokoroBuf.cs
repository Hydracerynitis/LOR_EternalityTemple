using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Yagokoro
{
    public class MoonBuf: BattleUnitBuf
    {
        public override string keywordIconId => "GalaxyBoy_Stone";
        public virtual void Update()
        {

        }
    }
    public class BattleUnitBuf_Moon1: MoonBuf
    {
        public override string keywordId => "YagokoroBuf1";
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 1);
        }
        public override void Update()
        {
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_Moon2());
            Destroy();
        }
    }
    public class BattleUnitBuf_Moon2 : MoonBuf
    {
        public override string keywordId => "YagokoroBuf2";
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = 1, max = 1 });
        }
        public override void Update()
        {
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_Moon3());
            Destroy();
        }
    }
    public class BattleUnitBuf_Moon3 : MoonBuf
    {
        public override string keywordId => "YagokoroBuf3";
        public override int MaxPlayPointAdder()
        {
            return 1;
        }
        public override void OnRoundStart()
        {
            _owner.cardSlotDetail.RecoverPlayPoint(1);
        }
    }
}

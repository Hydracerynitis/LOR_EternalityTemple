using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EternalityTemple.Inaba;

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
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 1);
        }
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
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, 1);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = 1, max = 1 });
        }
    }
    public class BattleDiceCardBuf_YagokoroCardBuf1 : BattleDiceCardBuf
    {
        public override string keywordIconId => "YagokoroCardBuf1";
        public override string keywordId => "YagokoroCardBuf1";
        public override void OnUseCard(BattleUnitModel owner, BattlePlayingCardDataInUnitModel playingCard)
        {
            base.OnUseCard(owner, playingCard);
            playingCard.ApplyDiceStatBonus(DiceMatch.AllDice,new DiceStatBonus() { power = 5 });
            this.Destroy();
        }
        public BattleDiceCardBuf_YagokoroCardBuf1()
        {
            this._priority = 1;
        }
    }
    public class YagokoroBuf4 : BattleUnitBuf
    {
        public override string keywordIconId => "Stun";
        public override string keywordId => "YagokoroBuf4";
        public override bool IsTargetable()
        {
            return false;
        }
        public override bool IsActionable()
        {
            return false;
        }
        public override int SpeedDiceBreakedAdder()
        {
            return 10;
        }
        public override void OnRoundEnd()
        {
            Destroy();
        }
    }
    public class YagokoroBuf5 : YagokoroBuf4
    {
        public override string keywordIconId => "Stun";
        public override string keywordId => "YagokoroBuf5";
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            _owner.RecoverHP(100);
            _owner.breakDetail.RecoverBreak(_owner.breakDetail.GetDefaultBreakGauge());
        }
    }
    public class YagokoroBuf6 : YagokoroBuf4
    {
        public override string keywordIconId => "Stun";
        public override string keywordId => "YagokoroBuf6";
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            _owner.TakeBreakDamage(20);
            _owner.bufListDetail.AddReadyBuf(new YagokoroBuf6_PowerDown());
        }
        public class YagokoroBuf6_PowerDown : BattleUnitBuf
        {
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = -2, max = -2 });
            }
            public override void OnRoundEnd()
            {
                Destroy();
            }
        }
    }
    public class YagokoroBuf7 : YagokoroBuf4
    {
        public override string keywordIconId => "Stun";
        public override string keywordId => "YagokoroBuf7";
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            BattleUnitBuf_InabaBuf2.AddReadyStack(_owner, stack);
        }
        public YagokoroBuf7(int stack)
        {
            this.stack = stack;
        }
    }
    public class YagokoroBuf8 : BattleUnitBuf
    {
        public override string keywordIconId => "Stun";
        public override string keywordId => "YagokoroBuf8";
        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            base.OnUseCard(card);
            card.DestroyDice((DiceMatch x) => x.abiliity.behaviourInCard.Detail == LOR_DiceSystem.BehaviourDetail.Penetrate);
        }
        public override void OnRoundEnd()
        {
            Destroy();
        }
    }
    public class YagokoroBuf9 : BattleUnitBuf
    {
        public override string keywordIconId => "Stun";
        public override string keywordId => "YagokoroBuf9";
        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            base.OnUseCard(card);
            card.DestroyDice((DiceMatch x) => x.abiliity.behaviourInCard.Detail == LOR_DiceSystem.BehaviourDetail.Slash);
        }
        public override void OnRoundEnd()
        {
            Destroy();
        }
    }
    public class YagokoroBuf10 : BattleUnitBuf
    {
        public override string keywordIconId => "Stun";
        public override string keywordId => "YagokoroBuf10";
        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            base.OnUseCard(card);
            card.DestroyDice((DiceMatch x) => x.abiliity.behaviourInCard.Detail == LOR_DiceSystem.BehaviourDetail.Hit);
        }
        public override void OnRoundEnd()
        {
            Destroy();
        }
    }
    public class YagokoroBuf12 : BattleUnitBuf
    {
        public override string keywordIconId => "GalaxyBoy_Stone";
        public override string keywordId => "YagokoroBuf12";
        public override void OnRoundEnd()
        {
            Destroy();
        }
    }
}

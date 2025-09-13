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

        public BattleUnitBuf_Moon1()
        {
            _bufIcon = EternalityInitializer.ArtWorks["ICON_Eirin月相1"]; //有美术资源的话换成别的
            _iconInit = true;
            stack = 0;
        }
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

        public BattleUnitBuf_Moon2()
        {
            _bufIcon = EternalityInitializer.ArtWorks["ICON_Eirin月相3"]; //有美术资源的话换成别的
            _iconInit = true;
            stack = 0;
        }
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
        public BattleUnitBuf_Moon3()
        {
            _bufIcon = EternalityInitializer.ArtWorks["ICON_Eirin月相5"]; //有美术资源的话换成别的
            _iconInit = true;
            stack = 0;
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
            _owner.RecoverHP(100);
            _owner.breakDetail.RecoverBreak(_owner.breakDetail.GetDefaultBreakGauge());
            base.OnRoundEnd();
        }
    }
    public class YagokoroBuf6 : YagokoroBuf4
    {
        public override string keywordIconId => "Stun";
        public override string keywordId => "YagokoroBuf6";
        public override void OnRoundEnd()
        {
            _owner.TakeBreakDamage(20);
            _owner.bufListDetail.AddReadyBuf(new YagokoroBuf6_PowerDown());
            base.OnRoundEnd();
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
            BattleUnitBuf_InabaBuf2.AddReadyStack(_owner, stack);
            base.OnRoundEnd();
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
    public class YagokoroBuf13 : BattleUnitBuf
    {
        
        public HashSet<int> moonCompletion=new HashSet<int>();
        public override string keywordIconId => "GalaxyBoy_Stone";
        public override string keywordId => "YagokoroBuf13";
        public override string bufActivatedText => BattleEffectTextsXmlList.Instance.GetEffectTextDesc(keywordId,
                GetCompletionText());
        private string GetCompletionText()
        {
            string str = "";
            if (moonCompletion.Contains(1))
                str += "(1)";
            if (moonCompletion.Contains(2))
                str += "(2)";
            if (moonCompletion.Contains(3))
                str += "(3)";
            if (moonCompletion.Contains(4))
                str += "(4)";
            if (moonCompletion.Contains(5))
                str += "(5)";
            if (str == "")
                str = TextDataModel.GetText("KaguyaBuf_None");
            return str;
        }
    }
}

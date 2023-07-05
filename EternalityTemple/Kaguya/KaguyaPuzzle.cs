using EternalityTemple.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CharacterSound;

namespace EternalityTemple.Kaguya
{
    public class BattleUnitBuf_PuzzleBuf : BattleUnitBuf
    {
        public List<int> CompletePuzzle = new List<int>();
        public override string keywordIconId => "KeterFinal_Cogito";
        public override string keywordId => "KaguyaBuf8";
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            stack = 0;
        }
        public void AddPuzzle(int i)
        {
            if (!CompletePuzzle.Contains(i))
                CompletePuzzle.Add(i);
        }
        public override string bufActivatedText => String.Format(BattleEffectTextsXmlList.Instance.GetEffectTextDesc(keywordId), CompletePuzzle.Count,getPuzzleName(), getPuzzleDiceDesc(), getPuzzleBonus());
        private string getPuzzleName()
        {
            string str= "";
            if (CompletePuzzle.Contains(1))
                str += TextDataModel.GetText("KaguyaBuf_Puzzle1");
            if (CompletePuzzle.Contains(2))
                str += TextDataModel.GetText("KaguyaBuf_Puzzle2");
            if (CompletePuzzle.Contains(3))
                str += TextDataModel.GetText("KaguyaBuf_Puzzle3");
            if (CompletePuzzle.Contains(4))
                str += TextDataModel.GetText("KaguyaBuf_Puzzle4");
            if (CompletePuzzle.Contains(5))
                str += TextDataModel.GetText("KaguyaBuf_Puzzle5");
            if(str=="")
                str= TextDataModel.GetText("KaguyaBuf_None");
            return str;
        }
        private string getPuzzleBonus()
        {
            string str = "";
            if (CompletePuzzle.Contains(1))
                str += TextDataModel.GetText("KaguyaBuf_Nt1");
            if (CompletePuzzle.Contains(2))
                str += TextDataModel.GetText("KaguyaBuf_Nt2");
            if (CompletePuzzle.Contains(3))
                str += TextDataModel.GetText("KaguyaBuf_Nt3");
            if (CompletePuzzle.Contains(4))
                str += TextDataModel.GetText("KaguyaBuf_Nt4");
            if (CompletePuzzle.Contains(5))
                str += TextDataModel.GetText("KaguyaBuf_Nt5");
            if (str == "")
                str = TextDataModel.GetText("KaguyaBuf_None");
            return str;
        }
        private string getPuzzleDiceDesc()
        {
            string str = "";
            if (CompletePuzzle.Contains(1))
                str += TextDataModel.GetText("KaguyaBuf_NtDice1");
            if (CompletePuzzle.Contains(2))
                str += TextDataModel.GetText("KaguyaBuf_NtDice2");
            if (CompletePuzzle.Contains(3))
                str += TextDataModel.GetText("KaguyaBuf_NtDice3");
            if (CompletePuzzle.Contains(4))
                str += TextDataModel.GetText("KaguyaBuf_NtDice4");
            if (CompletePuzzle.Contains(5))
                str += TextDataModel.GetText("KaguyaBuf_NtDice5");
            if (str == "")
                str = TextDataModel.GetText("KaguyaBuf_None");
            return str;
        }
    }
    public class KaguyaPuzzleAbility : DiceCardSelfAbilityBase
    {
        public override bool IsTargetableAllUnit()
        {
            return true;
        }
        public override bool IsValidTarget(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            return unit.bufListDetail.HasBuf<BattleUnitBuf_PuzzleBuf>() && (unit.faction != targetUnit.faction || targetUnit.bufListDetail.HasBuf<BattleUnitBuf_PuzzleBuf>());
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            BattleUnitBuf buf = null;
            switch (getPuzzleId())
            {
                case 1:
                    buf = new KaguyaPuzzle1(unit);
                    break;
                case 2:
                    buf = new KaguyaPuzzle2(unit);
                    break;
                case 3:
                    buf = new KaguyaPuzzle3(unit);
                    break;
                case 4:
                    buf = new KaguyaPuzzle4(unit);
                    break;
                case 5:
                    buf = new KaguyaPuzzle5(unit);
                    break;
            }
            if (buf != null)
                if (targetUnit.faction != unit.faction)
                    unit.bufListDetail.AddBuf(buf);
                else
                    targetUnit.bufListDetail.AddBuf(buf);
            unit.personalEgoDetail.RemoveCard(self.XmlData.id);
        }
        public virtual int getPuzzleId() => -1;
    }
    public class DiceCardSelfAbility_KaguyaPuzzle1: KaguyaPuzzleAbility
    {
        public override int getPuzzleId() => 1;
    }
    public class DiceCardSelfAbility_KaguyaPuzzle2 : KaguyaPuzzleAbility
    {
        public override int getPuzzleId() => 2;
    }
    public class DiceCardSelfAbility_KaguyaPuzzle3 : KaguyaPuzzleAbility
    {
        public override int getPuzzleId() => 3;
    }
    public class DiceCardSelfAbility_KaguyaPuzzle4 : KaguyaPuzzleAbility
    {
        public override int getPuzzleId() => 4;
    }
    public class DiceCardSelfAbility_KaguyaPuzzle5 : KaguyaPuzzleAbility
    {
        public override int getPuzzleId() => 5;
    }
    public class KaguyaPuzzle: BattleUnitBuf
    {
        private BattleUnitModel Kaguya;
        protected int count = 0;
        public override string keywordIconId => RandomUtil.SelectOne("PlutoUnfairAtk", "PlutoUnfairLight", "PlutoUnfairProtect");
        public override int paramInBufDesc => count;
        public KaguyaPuzzle(BattleUnitModel kaguya)
        {
            Kaguya = kaguya;
        }
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            stack = 0;
        }
        private BattleUnitBuf_PuzzleBuf getPuzzleBuf(BattleUnitModel unit) => unit.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_PuzzleBuf) as BattleUnitBuf_PuzzleBuf;

        protected void CompletePuzzle()
        {
            getPuzzleBuf(_owner)?.AddPuzzle(getPuzzleId());
            if (Kaguya!=null && _owner!=Kaguya)
                getPuzzleBuf(Kaguya)?.AddPuzzle(getPuzzleId());
            Destroy();
        }
        public virtual int getPuzzleId() => -1;
    }
    public class KaguyaPuzzle1: KaguyaPuzzle, OnGiveOtherBuf
    {
        
        public KaguyaPuzzle1(BattleUnitModel kaguya) : base(kaguya)
        {
        }
        public override int getPuzzleId() => 1;
        public override string keywordId => "KaguyaPuzzle1";
        public void OnGiveBuf(BattleUnitBuf buf, int stack)
        {
            if (buf.positiveType == BufPositiveType.Negative)
            {
                count += stack;
                if (count >= 30)
                    CompletePuzzle();
            }
        }
    }
    public class KaguyaPuzzle2 : KaguyaPuzzle
    {
        public KaguyaPuzzle2(BattleUnitModel kaguya) : base(kaguya)
        {
        }
        public override int getPuzzleId() => 2;
        public override string keywordId => "KaguyaPuzzle2";
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            if (IsDefenseDice(behavior.Detail))
            {
                count++;
                if (count >= 10)
                    CompletePuzzle();
            }
        }
    }
    public class KaguyaPuzzle3 : KaguyaPuzzle, OnGiveOtherBuf
    {
        public KaguyaPuzzle3(BattleUnitModel kaguya) : base(kaguya)
        {
        }
        public override int getPuzzleId() => 3;
        public override string keywordId => "KaguyaPuzzle3";
        public void OnGiveBuf(BattleUnitBuf buf, int stack)
        {
            if (buf is BattleUnitBuf_burn)
            {
                count += stack;
                if (count >= 20)
                    CompletePuzzle();
            }
        }
    }
    public class KaguyaPuzzle4 : KaguyaPuzzle, OnRecoverHP
    {
        public KaguyaPuzzle4(BattleUnitModel kaguya) : base(kaguya)
        {
        }
        public override int getPuzzleId() => 4;
        public override string keywordId => "KaguyaPuzzle4";
        public void OnHeal(int num)
        {
            count ++;
            if (count >= 50)
                CompletePuzzle();
        }
    }
    public class KaguyaPuzzle5 : KaguyaPuzzle
    {
        public KaguyaPuzzle5(BattleUnitModel kaguya) : base(kaguya)
        {
        }
        public override int getPuzzleId() => 5;
        public override string keywordId => "KaguyaPuzzle5";
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            count ++;
            if (count >= 70)
                CompletePuzzle();
        }
    }
}

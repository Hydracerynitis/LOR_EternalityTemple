using EternalityTemple.Util;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Kaguya
{
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
                    buf = new KaguyaPuzzle1(unit) { stack=0};
                    break;
                case 2:
                    buf = new KaguyaPuzzle2(unit) { stack = 0 };
                    break;
                case 3:
                    buf = new KaguyaPuzzle3(unit) { stack = 0 };
                    break;
                case 4:
                    buf = new KaguyaPuzzle4(unit) { stack = 0 };
                    break;
                case 5:
                    buf = new KaguyaPuzzle5(unit) { stack = 0 };
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
        private bool replay;
        public override string keywordIconId => RandomUtil.SelectOne("PlutoUnfairAtk", "PlutoUnfairLight", "PlutoUnfairProtect");
        public KaguyaPuzzle(BattleUnitModel kaguya, bool replay=false)
        {
            Kaguya = kaguya;
            this.replay = replay;
        }
        public override void Init(BattleUnitModel owner)
        {
            base.Init(owner);
            if(!replay)
                EternalityParam.GetFaction(owner.faction).QuestLog.Add(new PuzzleQuestData() { QuestId = getPuzzleId(), QuestProgress = stack, questGiver = owner.UnitData });
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
    //[难题]龙颈之玉
    //[条件]持有时施加X层[负面状态]
    public class KaguyaPuzzle1: KaguyaPuzzle, OnGiveOtherBuf
    {
        
        public KaguyaPuzzle1(BattleUnitModel kaguya, bool replay = false) : base(kaguya, replay)
        {
        }
        public override int getPuzzleId() => 1;
        public override string keywordId => "KaguyaPuzzle1";
        public void OnGiveBuf(BattleUnitBuf buf, int stack)
        {
            if (buf.positiveType == BufPositiveType.Negative)
            {
                this.stack += stack;
                if (this.stack >= 20) //修改这里的数值来修改任务的需求
                    CompletePuzzle();
            }
        }
    }
    //[难题]佛御石之钵
    //[条件]持有时以[防御型]骰子取得拼点胜利X次
    public class KaguyaPuzzle2 : KaguyaPuzzle
    {
        public KaguyaPuzzle2(BattleUnitModel kaguya, bool replay = false) : base(kaguya, replay)
        {
        }
        public override int getPuzzleId() => 2;
        public override string keywordId => "KaguyaPuzzle2";
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            if (IsDefenseDice(behavior.Detail))
            {
                stack++;
                if (stack >= 5)//修改这里的数值来修改任务的需求
                    CompletePuzzle();
            }
        }
    }
    //[难题]火鼠的皮衣
    //[条件]持有时施加X层[烧伤]
    public class KaguyaPuzzle3 : KaguyaPuzzle, OnGiveOtherBuf
    {
        public KaguyaPuzzle3(BattleUnitModel kaguya, bool replay = false) : base(kaguya, replay)
        {
        }
        public override int getPuzzleId() => 3;
        public override string keywordId => "KaguyaPuzzle3";
        public void OnGiveBuf(BattleUnitBuf buf, int stack)
        {
            if (buf is BattleUnitBuf_burn)
            {
                this.stack += stack;
                if (this.stack >= 15)//修改这里的数值来修改任务的需求
                    CompletePuzzle();
            }
        }
    }
    //[难题]燕的子安贝
    //[条件]持有时恢复X点体力
    public class KaguyaPuzzle4 : KaguyaPuzzle, OnRecoverHP
    {
        public KaguyaPuzzle4(BattleUnitModel kaguya, bool replay = false) : base(kaguya, replay)
        {
        }
        public override int getPuzzleId() => 4;
        public override string keywordId => "KaguyaPuzzle4";
        public void OnHeal(int num)
        {
            stack +=num;
            if (stack >= 20)//修改这里的数值来修改任务的需求
                CompletePuzzle();
        }
    }
    //[难题]蓬莱玉枝
    //[条件]投掷X次骰子
    public class KaguyaPuzzle5 : KaguyaPuzzle
    {
        public KaguyaPuzzle5(BattleUnitModel kaguya, bool replay = false) : base(kaguya, replay)
        {
        }
        public override int getPuzzleId() => 5;
        public override string keywordId => "KaguyaPuzzle5";
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            stack ++;
            if (stack >= 35)//修改这里的数值来修改任务的需求
                CompletePuzzle();
        }
    }
}

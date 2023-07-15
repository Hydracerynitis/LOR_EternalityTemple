using EternalityTemple.Util;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Kaguya
{
    public class BattleUnitBuf_PuzzleBuf : BattleUnitBuf, OnGiveOtherBuf, OnAddOtherBuf
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
        public override string bufActivatedText => String.Format(BattleEffectTextsXmlList.Instance.GetEffectTextDesc(keywordId), CompletePuzzle.Count, getPuzzleName(), getPuzzleDiceDesc(), getPuzzleBonus());
        private string getPuzzleName()
        {
            string str = "";
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
            if (str == "")
                str = TextDataModel.GetText("KaguyaBuf_None");
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
        public override void OnUseCard(BattlePlayingCardDataInUnitModel card)
        {
            if (CompletePuzzle.Contains(5) && card.GetDiceBehaviorList().Count >= 3)
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = 2 });
            int unavailable = _owner.speedDiceResult.FindAll(x => x.breaked).Count;
            if (CompletePuzzle.Contains(1) && card.slotOrder == unavailable + 0)
                card.GetDiceBehaviorList().ForEach(x => x.AddAbility(new PuzzleDiceAbility1() { behavior = x }));
            if (CompletePuzzle.Contains(2) && card.slotOrder == unavailable + 1)
                card.GetDiceBehaviorList().ForEach(x => x.AddAbility(new PuzzleDiceAbility2() { behavior = x }));
            if (CompletePuzzle.Contains(3) && card.slotOrder == unavailable + 2)
                card.GetDiceBehaviorList().ForEach(x => x.AddAbility(new PuzzleDiceAbility3() { behavior = x }));
            if (CompletePuzzle.Contains(4) && card.slotOrder == unavailable + 3)
                card.GetDiceBehaviorList().ForEach(x => x.AddAbility(new PuzzleDiceAbility4() { behavior = x }));
            if (CompletePuzzle.Contains(5) && card.slotOrder == unavailable + 4/* && card.card.GetID()!=new LorId(EternalityInitializer.packageId, 226769010)*/)
                card.GetDiceBehaviorList().ForEach(x => x.AddAbility(new PuzzleDiceAbility5() { behavior = x }));
        }
        public override void OnRoundStart()
        {
            if (CompletePuzzle.Contains(2))
            {
                ResetResist();
                List<(string, BehaviourDetail)> NotEndure = new List<(string, BehaviourDetail)>();
                foreach (BehaviourDetail bd in new BehaviourDetail[] { BehaviourDetail.Slash, BehaviourDetail.Penetrate, BehaviourDetail.Hit })
                {
                    if ((int)_owner.Book.GetResistHP(bd) < 4)
                        NotEndure.Add(("hp", bd));
                    if ((int)_owner.Book.GetResistBP(bd) < 4)
                        NotEndure.Add(("bp", bd));
                }
                (string, BehaviourDetail) Selected = RandomUtil.SelectOne(NotEndure);
                if (Selected.Item1 == "hp")
                    _owner.Book.SetResistHP(Selected.Item2, AtkResist.Endure);
                else
                    _owner.Book.SetResistBP(Selected.Item2, AtkResist.Endure);
            }
        }
        private void ResetResist()
        {
            _owner.Book.SetResistHP(BehaviourDetail.Slash, _owner.Book.ClassInfo.EquipEffect.SResist);
            _owner.Book.SetResistHP(BehaviourDetail.Penetrate, _owner.Book.ClassInfo.EquipEffect.PResist);
            _owner.Book.SetResistHP(BehaviourDetail.Hit, _owner.Book.ClassInfo.EquipEffect.HResist);
            _owner.Book.SetResistBP(BehaviourDetail.Slash, _owner.Book.ClassInfo.EquipEffect.SBResist);
            _owner.Book.SetResistBP(BehaviourDetail.Penetrate, _owner.Book.ClassInfo.EquipEffect.PBResist);
            _owner.Book.SetResistBP(BehaviourDetail.Hit, _owner.Book.ClassInfo.EquipEffect.HBResist);
        }
        public override void OnRoundEnd()
        {
            if (CompletePuzzle.Contains(4))
            {
                _owner.RecoverHP(10);
                _owner.breakDetail.RecoverBreak(10);
            }
        }

        public void OnGiveBuf(BattleUnitBuf buf, int stack)
        {
            if (CompletePuzzle.Contains(1) && buf.positiveType == BufPositiveType.Negative)
                buf.stack += 1;
        }

        public void OnAddBuf(BattleUnitBuf buf, int stack)
        {
            if (CompletePuzzle.Contains(3) && buf is BattleUnitBuf_burn)
            {
                buf.stack -= 1;
                if (buf.stack <= 0)
                    buf.Destroy();
            }
        }
        public static bool IsPuzzlePage(LorId pageId)
        {
            return pageId.packageId == EternalityInitializer.packageId && pageId.id == 226769100;
        }
    }
    public class PuzzleDiceAbility1 : DiceCardAbilityBase
    {
        public override void BeforeRollDice()
        {
            if(BattleUnitBuf_PuzzleBuf.IsPuzzlePage(card.card.GetID()))
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = 2, max = 2 });
            else
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { min = 1, max = 1 });
        }
    }
    public class PuzzleDiceAbility2 : DiceCardAbilityBase
    {
        public override void OnLoseParrying()
        {
            if (BattleUnitBuf_PuzzleBuf.IsPuzzlePage(card.card.GetID()))
                behavior.TargetDice?.ApplyDiceStatBonus(new DiceStatBonus() { dmg = -4 });
            else
                behavior.TargetDice?.ApplyDiceStatBonus(new DiceStatBonus() { dmg = -2 });
        }
    }
    public class PuzzleDiceAbility3 : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            if (BattleUnitBuf_PuzzleBuf.IsPuzzlePage(card.card.GetID()))
                card.target.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, 4);
            else
                card.target.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, 2);
        }
    }
    public class PuzzleDiceAbility4 : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            BattleUnitModel LeastHp = null;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (LeastHp == null)
                    LeastHp = unit;
                else if (LeastHp.hp > unit.hp)
                    LeastHp = unit;
            }
            if (BattleUnitBuf_PuzzleBuf.IsPuzzlePage(card.card.GetID()))
            {
                LeastHp.RecoverHP(6);
                LeastHp.breakDetail.RecoverBreak(6);
            }
            else
            {
                LeastHp.RecoverHP(3);
                LeastHp.breakDetail.RecoverBreak(3);
            }    
        }
    }
    public class PuzzleDiceAbility5 : DiceCardAbilityBase
    {
        private bool active = false;
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (active)
                return;
            BattleDiceCardModel card = RandomUtil.SelectOne(owner.allyCardDetail.GetHand());
            if (BattleUnitBuf_PuzzleBuf.IsPuzzlePage(this.card.card.GetID()))
            {
                card.AddBuf(new CostDownSelfBuf());
                card.AddBuf(new CostDownSelfBuf());
            }
            else
                card.AddBuf(new CostDownSelfBuf());
            active = true;
        }
        public class CostDownSelfBuf : BattleDiceCardBuf
        {
            public override int GetCost(int oldCost) => oldCost - 1;
            public override void OnUseCard(BattleUnitModel owner) => Destroy();
        }
    }
}

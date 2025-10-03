using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.UI.GridLayoutGroup;

namespace EternalityTemple.Kaguya
{
    public class PassiveAbility_226769104: PassiveAbilityBase
    {
        int count = 0;
        int nextPuzzle = 0;
        public override void OnAfterRollSpeedDice()
        {
            if (nextPuzzle <= 0 || nextPuzzle > owner.speedDiceCount)
                return;
            int unavailable=owner.speedDiceResult.FindAll(x => x.breaked).Count();
            owner.cardOrder = nextPuzzle - 1;
            int puzzlepageId = getPuzzlePage(nextPuzzle);
            if (puzzlepageId < 0)
                return;
            BattleDiceCardModel card = owner.allyCardDetail.AddNewCard(EternalityInitializer.GetLorId(puzzlepageId));
            card.SetCostToZero();
            card.temporary = true;
            BattleUnitModel target = RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(owner.faction));
            owner.cardSlotDetail.AddCard(card, target,RandomUtil.Range(0,target.speedDiceCount-1));
            nextPuzzle = 0;
        }
        public override void OnRoundEndTheLast()
        {
            if (BattleUnitBuf_KaguyaBuf.GetStack(owner) < 7)
                return;
            if (count > 0)
            {
                count--;
                return;
            }
            BattleUnitBuf_PuzzleBuf puzzlebuf = owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_PuzzleBuf) as BattleUnitBuf_PuzzleBuf;
            if (puzzlebuf == null)
                return;
            List<int> AllPuzzle = new List<int> { 1, 2, 3, 4, 5 };
            if (puzzlebuf.CompletePuzzle.Count < 5)
            {
                AllPuzzle.RemoveAll(x => puzzlebuf.CompletePuzzle.Contains(x));
                nextPuzzle = AllPuzzle[0];
                puzzlebuf.AddPuzzle(nextPuzzle, false);
                BattleUnitModel randomAlly = RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList(owner.faction).FindAll(x => x != owner));
                BattleUnitBuf_PuzzleBuf allyPuzzle = randomAlly.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_PuzzleBuf) as BattleUnitBuf_PuzzleBuf;
                if (allyPuzzle != null)
                    allyPuzzle.AddPuzzle(nextPuzzle, false);
            }
            else
                nextPuzzle = RandomUtil.SelectOne(AllPuzzle);
            count = 1;
        }
        private int getPuzzlePage(int puzzleId)
        {
            switch (puzzleId)
            {
                case 1:
                    return 226769006;
                case 2:
                    return 226769007;
                case 3:
                    return 226769008;
                case 4:
                    return 226769009;
                case 5:
                    return 226769010;
            }
            return -1;
        }
    }
}

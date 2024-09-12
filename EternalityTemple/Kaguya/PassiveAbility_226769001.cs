using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Kaguya
{
    public class PassiveAbility_226769001: PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            List<int> remainingQuest=new List<int>() { 1,2,3,4,5 };
            List<(UnitBattleDataModel, int)> puzzleLog = EternalityParam.GetFaction(owner.faction).PuzzleLog;
            List<PuzzleQuestData> questLog= EternalityParam.GetFaction(owner.faction).QuestLog;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                BattleUnitBuf_PuzzleBuf puzzleBuf = new BattleUnitBuf_PuzzleBuf();
                unit.bufListDetail.AddBuf(puzzleBuf);
                puzzleLog.FindAll(x => x.Item1 == unit.UnitData).ForEach(x =>
                {
                    puzzleBuf.AddPuzzle(x.Item2, true);
                    remainingQuest.Remove(x.Item2);
                });
                foreach(PuzzleQuestData PQD in questLog.FindAll(x => x.questGiver == unit.UnitData))
                {
                    BattleUnitBuf buf = null;
                    switch (PQD.QuestId)
                    {
                        case 1:
                            buf = new KaguyaPuzzle1(owner,true) { stack = PQD.QuestProgress };
                            break;
                        case 2:
                            buf = new KaguyaPuzzle2(owner, true) { stack = PQD.QuestProgress };
                            break;
                        case 3:
                            buf = new KaguyaPuzzle3(owner, true) { stack = PQD.QuestProgress };
                            break;
                        case 4:
                            buf = new KaguyaPuzzle4(owner, true) { stack = PQD.QuestProgress };
                            break;
                        case 5:
                            buf = new KaguyaPuzzle5(owner, true) { stack = PQD.QuestProgress };
                            break;
                    }
                    if (buf != null)
                        unit.bufListDetail.AddBuf(buf);
                    remainingQuest.Remove(PQD.QuestId);
                }
            }
            if(remainingQuest.Contains(1))
                owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769011));
            if (remainingQuest.Contains(2))
                owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769012));
            if (remainingQuest.Contains(3))
                owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769013));
            if (remainingQuest.Contains(4))
                owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769014));
            if (remainingQuest.Contains(5))
                owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769015));
        }
        public override void OnRoundEndTheLast_ignoreDead()
        {
            EternalityInitializer.ResetSpeedDiceColor();
        }
        public override void OnDie()
        {
            BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_PuzzleBuf)));
            BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.bufListDetail.RemoveBufAll(typeof(KaguyaPuzzle)));
        }
    }
}

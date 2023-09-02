using EternalityTemple.Inaba;
using EternalityTemple.Kaguya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EI = EternalityTemple.EternalityInitializer;

namespace EternalityTemple.Universal
{
    public static class EternalityParam
    {
        public static int InabaBufGainNum = 0;
        public static bool EgoCoolDown = false;
        public static int KaguyaStack = 1;
        public static Dictionary<UnitBattleDataModel, List<int>> Puzzle = new Dictionary<UnitBattleDataModel, List<int>>();
    }
    public class EnemyTeamStageManager_Eternality: EnemyTeamStageManager
    {
        public override void OnStageClear()
        {
            for (int index = 0; index < 5; ++index)
            {
                StageController.Instance.OnEnemyDropBookForAdded(new DropBookDataForAddedReward(new LorId(EI.packageId,226769001), true));
                StageController.Instance.OnEnemyDropBookForAdded(new DropBookDataForAddedReward(new LorId(EI.packageId, 226769002), true));
                StageController.Instance.OnEnemyDropBookForAdded(new DropBookDataForAddedReward(new LorId(EI.packageId, 226769003), true));
                BattleManagerUI.Instance.ui_emotionInfoBar.DropBook(new List<string>()
                {
                  TextDataModel.GetText("BattleUI_GetBook", (object) DropBookXmlList.Instance.GetData(new LorId(EI.packageId, 226769001)).Name),
                  TextDataModel.GetText("BattleUI_GetBook", (object) DropBookXmlList.Instance.GetData(new LorId(EI.packageId, 226769002)).Name),
                  TextDataModel.GetText("BattleUI_GetBook", (object) DropBookXmlList.Instance.GetData(new LorId(EI.packageId, 226769003)).Name)
                });
            }
        }
        public override void OnRoundEndTheLast()
        {
            bool KagyaStackUpdate=false;
            bool InabaStackUpdate=false;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                if(!KagyaStackUpdate)
                {
                    int stack = BattleUnitBuf_KaguyaBuf.GetStack(unit);
                    if (stack != -1)
                    {
                        EternalityParam.KaguyaStack = stack;
                        KagyaStackUpdate = true;
                    }
                }
                if (!InabaStackUpdate)
                {
                    int stack = BattleUnitBuf_InabaBuf1.GetStack(unit);
                    if (stack != 0)
                    {
                        EternalityParam.InabaBufGainNum = stack;
                        InabaStackUpdate = true;
                    }
                }
                /*if(unit.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_PuzzleBuf) is BattleUnitBuf_PuzzleBuf PB)
                    EternalityParam.Puzzle.Add(unit.UnitData, PB.CompletePuzzle);*/
            }
        }
    }
}

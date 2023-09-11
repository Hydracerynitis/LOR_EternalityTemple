using EternalityTemple.Inaba;
using EternalityTemple.Kaguya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EI = EternalityTemple.EternalityInitializer;

namespace EternalityTemple.Universal
{
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
    }
}

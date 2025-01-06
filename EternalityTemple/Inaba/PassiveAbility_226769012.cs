using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Inaba
{
    public class PassiveAbility_226769012 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            int num = Singleton<StageController>.Instance.GetStageModel().GetUsedFloorList().Count-1;
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
            {
                if (battleUnitModel.emotionDetail.EmotionLevel <num)
                {
                    battleUnitModel.emotionDetail.SetEmotionLevel(battleUnitModel.emotionDetail.EmotionLevel + 1);
                    battleUnitModel.cardSlotDetail.RecoverPlayPoint(9);
                }
            }
        }
    }
}

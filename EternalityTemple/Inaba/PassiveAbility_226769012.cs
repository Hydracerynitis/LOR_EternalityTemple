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
            int num = Singleton<StageModel>.Instance.GetUsedFloorList().Count - Singleton<StageController>.Instance.RoundTurn;
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(this.owner.faction))
            {
                if (num >= 0)
                {
                    battleUnitModel.emotionDetail.SetEmotionLevel(battleUnitModel.emotionDetail.EmotionLevel + 1);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
public class PassiveAbility_226769021 : PassiveAbility_226769019
{
    public override void OnWaveStart()
    {
        base.OnWaveStart();
		Singleton<StageController>.Instance.GetStageModel().ClassInfo.floorNum += 1;
	}
	public override void OnBattleEnd()
	{
		if(BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Count<=0)
        {
			foreach (StageLibraryFloorModel stageLibraryFloorModel in Singleton<StageController>.Instance.GetStageModel().floorList)
			{
				stageLibraryFloorModel.Defeat();
			}
			Singleton<StageController>.Instance.EndBattle();
			Singleton<StageController>.Instance.GetStageModel().ClassInfo.floorNum = 1;
			Singleton<StageController>.Instance.GetStageModel().ClassInfo.floorOnlyList = new List<SephirahType> { SephirahType.Malkuth };
			return;
        }
		SephirahType sephirah = Singleton<StageController>.Instance.CurrentFloor + 1;
		Singleton<StageController>.Instance._currentFloor = sephirah;
		Singleton<StageController>.Instance.GetStageModel().ClassInfo.floorOnlyList = new List<SephirahType> { sephirah };
	}
}
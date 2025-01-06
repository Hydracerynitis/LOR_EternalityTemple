using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSave;
using UnityEngine;
using System.Threading.Tasks;

namespace EternalityTemple.Kaguya
{
    public class PassiveAbility_226769013 : PassiveAbilityBase
    {
        private int KaguyaDmg = 0;
        private int InabaDmg = 0;
        private int YagokoroDmg = 0;
        public override void OnBattleEnd()
        {
            base.OnBattleEnd();
            foreach(BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                battleUnitModel.emotionDetail.PassiveList.Clear();
            }
            if (BattleObjectManager.instance.GetAliveList(owner.faction).Count <= 0)
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
            SaveData saveData = Singleton<EternalityTempleSaveManager>.Instance.LoadData("passFloor");
            Dictionary<string, SaveData> dic = saveData.GetDictionarySelf();
            dic[Singleton<StageController>.Instance.CurrentFloor.ToString() + "Kaguya"] = new SaveData(KaguyaDmg);
            dic[Singleton<StageController>.Instance.CurrentFloor.ToString() + "Inaba"] = new SaveData(InabaDmg);
            dic[Singleton<StageController>.Instance.CurrentFloor.ToString() + "Yagokoro"] = new SaveData(YagokoroDmg);
            saveData._dic = dic;
            Singleton<EternalityTempleSaveManager>.Instance.SaveData(saveData, "passFloor");
            SephirahType sephirah = Singleton<StageController>.Instance.CurrentFloor + 1;
            Singleton<StageController>.Instance._currentFloor = sephirah;
            Singleton<StageController>.Instance.GetStageModel().ClassInfo.floorOnlyList = new List<SephirahType> { sephirah };
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            KaguyaDmg += BattleObjectManager.instance.GetAliveList(Faction.Player).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769103).history.takeDamageAtOneRound;
            YagokoroDmg += BattleObjectManager.instance.GetAliveList(Faction.Player).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769104).history.takeDamageAtOneRound;
            InabaDmg += BattleObjectManager.instance.GetAliveList(Faction.Player).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769105).history.takeDamageAtOneRound;
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            SaveData saveData = Singleton<EternalityTempleSaveManager>.Instance.LoadData("passFloor");
            SaveData saveData2;
            if (saveData == null) return;
            if (!saveData.GetDictionarySelf().TryGetValue(Singleton<StageController>.Instance.CurrentFloor.ToString() + "Kaguya", out saveData2)) return;
            Debug.Log("Active!!!!!");
            Debug.Log("CCCCC:" + saveData._dic[Singleton<StageController>.Instance.CurrentFloor.ToString() + "Kaguya"].GetIntSelf());
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769137));
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if(StageController.Instance.RoundTurn == 2) owner.personalEgoDetail.RemoveCard(new LorId(EternalityInitializer.packageId, 226769137));
        }
    }
}
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
        public static int KaguyaHP = 0;
        public static int InabaHP = 0;
        public static int YagokoroHP = 0;
        public static void RecordHP()
        {
            KaguyaHP = RecordUnitHP(226769103);
            YagokoroHP = RecordUnitHP(226769104);
            InabaHP = RecordUnitHP(226769105);
            SaveData saveData = Singleton<EternalityTempleSaveManager>.Instance.LoadData("passFloor");
            Dictionary<string, SaveData> dic = saveData.GetDictionarySelf();
            string currentFloor = StageController.Instance.CurrentFloor.ToString();
            dic[currentFloor + "Kaguya"] = new SaveData(KaguyaHP);
            dic[currentFloor + "Inaba"] = new SaveData(InabaHP);
            dic[currentFloor + "Yagokoro"] = new SaveData(YagokoroHP);
            saveData._dic = dic;
            Singleton<EternalityTempleSaveManager>.Instance.SaveData(saveData, "passFloor");
        }
        public override void OnBattleEnd()
        {
            base.OnBattleEnd();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(Faction.Player))
                battleUnitModel.emotionDetail.PassiveList.Clear();
            if (StageController.Instance.GetStageModel().ClassInfo.id == EternalityInitializer.GetLorId(226769001)
                && BattleObjectManager.instance.GetAliveList_opponent(owner.faction).Count <= 0)
            {
                RecordHP();
                SephirahType sephirah = Singleton<StageController>.Instance.CurrentFloor + 1;
                Singleton<StageController>.Instance._currentFloor = sephirah;
                Singleton<StageController>.Instance.GetStageModel().ClassInfo.floorOnlyList = new List<SephirahType> { sephirah };
            }
        }
        private static int RecordUnitHP(int unitId)
        {
            BattleUnitModel unit = BattleObjectManager.instance.GetAliveList(Faction.Player).Find((BattleUnitModel x) 
                => x.Book.GetBookClassInfoId() == EternalityInitializer.GetLorId(unitId));
            if (unit == null)
                return 0;
            else
                return (int) unit.hp;
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            SaveData saveData = Singleton<EternalityTempleSaveManager>.Instance.LoadData("passFloor");
            if (saveData == null) 
                return;
            if (!saveData.GetDictionarySelf().TryGetValue(Singleton<StageController>.Instance.CurrentFloor.ToString() + "Kaguya", out SaveData saveData2)) 
                return;
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769137));
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if(StageController.Instance.RoundTurn == 3) owner.personalEgoDetail.RemoveCard(new LorId(EternalityInitializer.packageId, 226769137));
        }
    }
}
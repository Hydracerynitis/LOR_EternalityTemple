using System;
using System.Collections.Generic;
using UnityEngine;

namespace EternalityTemple
{
    public class PassiveAbility_226769024 : PassiveAbility_226769023
    {
        public int teamEmotionLevel = 0;
        public List<EmotionCardXmlInfo> emotionFloor = new List<EmotionCardXmlInfo>();
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            Singleton<StageController>.Instance.GetStageModel().ClassInfo.floorNum += 1;
            emotionFloor = new List<EmotionCardXmlInfo>();
            for (int i = 1; i <= 15; i++)
            {
                int id = 190000 + (int)Singleton<StageController>.Instance.CurrentFloor * 100 + i;
                emotionFloor.Add(EmotionCardXmlList.Instance.GetData(id, SephirahType.None));
            }
            emotionFloor.Sort((EmotionCardXmlInfo x, EmotionCardXmlInfo y) => (int)(x.EmotionLevel - y.EmotionLevel));
        }
        public override void OnRoundEndTheLast_ignoreDead()
        {
            while (Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionLevel > teamEmotionLevel)
            {
                teamEmotionLevel++;
                switch (Singleton<StageController>.Instance.CurrentFloor)
                {
                    case SephirahType.Tiphereth:
                        ChooseEmotionCard_TA();
                        break;
                    case SephirahType.Gebura:
                        ChooseEmotionCard_Geburah();
                        break;
                    case SephirahType.Chesed:
                        ChooseEmotionCard_Chesed();
                        break;
                }
            }
        }
        public void ChooseEmotionCard_TA()
        {
            for (int i = 0; i < 3; i++)
            {
                BattleUnitModel battleUnitModel;
                EmotionCardXmlInfo xmlInfo = emotionFloor[0];
                emotionFloor.RemoveAt(0);
                if (xmlInfo.id == 190503 || xmlInfo.id == 190505 || xmlInfo.id == 190509 || xmlInfo.id == 190510 || xmlInfo.id == 190515 || xmlInfo.id == 190502 || xmlInfo.id == 190508 || xmlInfo.id == 190511)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769031);
                }
                else if (xmlInfo.id == 190501 || xmlInfo.id == 190504 || xmlInfo.id == 190506 || xmlInfo.id == 190507 || xmlInfo.id == 190513 || xmlInfo.id == 190514)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769032);
                }
                else
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList_random(Faction.Enemy, 1)[0];
                }
                if (battleUnitModel == null)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList_random(Faction.Enemy, 1)[0];
                }
                if (battleUnitModel == null || battleUnitModel.IsDead())
                {
                    return;
                }
                battleUnitModel.emotionDetail.ApplyEmotionCard(xmlInfo);
            }
        }
        public void ChooseEmotionCard_Geburah()
        {
            for (int i = 0; i < 3; i++)
            {
                BattleUnitModel battleUnitModel;
                EmotionCardXmlInfo xmlInfo = emotionFloor[0];
                emotionFloor.RemoveAt(0);
                battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769036);
                if (battleUnitModel == null)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList_random(Faction.Enemy, 1)[0];
                }
                if (battleUnitModel == null | battleUnitModel.IsDead())
                    return;
                battleUnitModel.emotionDetail.ApplyEmotionCard(xmlInfo);
            }
        }
        public void ChooseEmotionCard_Chesed()
        {
        }
    }
}

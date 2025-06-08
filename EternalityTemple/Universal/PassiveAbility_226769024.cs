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
                    case SephirahType.Binah:
                        ChooseEmotionCard_Binah();
                        break;
                    case SephirahType.Hokma:
                        ChooseEmotionCard_Hokma();
                        break;
                    case SephirahType.Keter:
                        ChooseEmotionCard_Keter();
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
            for (int i = 0; i < 3; i++)
            {
                BattleUnitModel battleUnitModel;
                EmotionCardXmlInfo xmlInfo = emotionFloor[0];
                emotionFloor.RemoveAt(0);
                if (xmlInfo.id == 190701 || xmlInfo.id == 190705 || xmlInfo.id == 190710 || xmlInfo.id == 190713)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769042);
                }
                else if (xmlInfo.id == 190707 || xmlInfo.id == 190709 || xmlInfo.id == 190711)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769041);
                }
                else if (xmlInfo.id == 190704 || xmlInfo.id == 190714 || xmlInfo.id == 190715)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769043);
                }
                else if (xmlInfo.id == 190702 || xmlInfo.id == 190703 || xmlInfo.id == 190708)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769044);
                }
                else if (xmlInfo.id == 190706)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769045);
                }
                else if (xmlInfo.id == 190712)
                {
                    foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                    {
                        model.emotionDetail.ApplyEmotionCard(xmlInfo);
                    }
                    continue;
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
        public void ChooseEmotionCard_Binah()
        {
            for (int i = 0; i < 3; i++)
            {
                BattleUnitModel battleUnitModel;
                EmotionCardXmlInfo xmlInfo = emotionFloor[0];
                emotionFloor.RemoveAt(0);
                if (xmlInfo.id == 190810 || xmlInfo.id == 190811 || xmlInfo.id == 190812 || xmlInfo.id == 190813)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769046);
                }
                if (xmlInfo.id == 190807 || xmlInfo.id == 190806 || xmlInfo.id == 190804)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769047);
                }
                if (xmlInfo.id == 190801 || xmlInfo.id == 190802 || xmlInfo.id == 190805)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769048);
                }
                if (xmlInfo.id == 190814 || xmlInfo.id == 190808)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769049);
                }
                if (xmlInfo.id == 190803 || xmlInfo.id == 190815)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769050);
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

        public void ChooseEmotionCard_Hokma()
        {
            for (int i = 0; i < 3; i++)
            {
                BattleUnitModel battleUnitModel;
                EmotionCardXmlInfo xmlInfo = emotionFloor[0];
                emotionFloor.RemoveAt(0);
                if (xmlInfo.id == 190910 || xmlInfo.id == 190911 || xmlInfo.id == 190913 || xmlInfo.id == 190914 || xmlInfo.id == 190915)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769051);
                }
                if (xmlInfo.id == 190901 || xmlInfo.id == 190902 || xmlInfo.id == 190903)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769052);
                }
                if (xmlInfo.id == 190905)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769053);
                }
                if (xmlInfo.id == 190907 || xmlInfo.id == 190908 || xmlInfo.id == 190909)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769054);
                }
                if (xmlInfo.id == 190913 || xmlInfo.id == 190906)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769055);
                }
                else if (xmlInfo.id == 190904)
                {
                    foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                    {
                        model.emotionDetail.ApplyEmotionCard(xmlInfo);
                    }
                    continue;
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
        public void ChooseEmotionCard_Keter()
        {
            for (int i = 0; i < 3; i++)
            {
                BattleUnitModel battleUnitModel;
                EmotionCardXmlInfo xmlInfo = emotionFloor[0];
                emotionFloor.RemoveAt(0);
                if (xmlInfo.id == 191013 || xmlInfo.id == 191014 || xmlInfo.id == 191015) continue;
                battleUnitModel = BattleObjectManager.instance.GetAliveList_random(Faction.Enemy, 1)[0];
                if (battleUnitModel == null || battleUnitModel.IsDead())
                {
                    return;
                }
                battleUnitModel.emotionDetail.ApplyEmotionCard(xmlInfo);
            }
        }
    }
}

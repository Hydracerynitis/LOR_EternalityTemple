﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace EternalityTemple
{
    public class PassiveAbility_226769021 : PassiveAbility_226769019
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
        public override void OnRoundStart()
        {
            if (Singleton<StageController>.Instance.CurrentFloor == SephirahType.Netzach)
            {
                foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(owner.faction))
                {
                    foreach (BattleDiceCardModel battleDiceCardModel in battleUnitModel.allyCardDetail.GetAllDeck())
                    {
                        if (battleDiceCardModel.GetID() == 407007)
                            battleDiceCardModel.XmlData.Script = "ally2strength1thisRound";
                    }
                }
            }
        }
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
        {
            return BattleObjectManager.instance.GetAliveList(Faction.Player).Find((BattleUnitModel x) => x.bufListDetail.HasBuf<EmotionCardAbility_alriune3.BattleUnitBuf_Emotion_Alriune>());
        }
        public override void OnRoundEndTheLast_ignoreDead()
        {
            while (Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionLevel > teamEmotionLevel)
            {
                teamEmotionLevel++;
                switch (Singleton<StageController>.Instance.CurrentFloor)
                {
                    case SephirahType.Malkuth:
                        ChooseEmotionCard_Malkuth();
                        break;
                    case SephirahType.Yesod:
                        ChooseEmotionCard_Yesod();
                        break;
                    case SephirahType.Hod:
                        ChooseEmotionCard_Hod();
                        break;
                    case SephirahType.Netzach:
                        ChooseEmotionCard_Netzach();
                        break;
                }
            }
        }
        public void ChooseEmotionCard_Malkuth()
        {
            for (int i = 0; i < 3; i++)
            {
                BattleUnitModel battleUnitModel;
                EmotionCardXmlInfo xmlInfo = emotionFloor[0];
                emotionFloor.RemoveAt(0);
                if (xmlInfo == null)
                {
                    Debug.Log("我情感卡呢????");
                    return;
                }
                if (xmlInfo.id == 190101 || xmlInfo.id == 190102 || xmlInfo.id == 190108 || xmlInfo.id == 190110 || xmlInfo.id == 190112 || xmlInfo.id == 190114 || xmlInfo.id == 190115)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769011);
                }
                else if (xmlInfo.id == 190103 || xmlInfo.id == 190104 || xmlInfo.id == 190105 || xmlInfo.id == 190106)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769012);
                }
                else if (xmlInfo.id == 190107)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769013);
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
                    Debug.Log("角色已死亡或不存在");
                    return;
                }
                battleUnitModel.emotionDetail.ApplyEmotionCard(xmlInfo);
            }
        }
        public void ChooseEmotionCard_Yesod()
        {
            for (int i = 0; i < 3; i++)
            {
                BattleUnitModel battleUnitModel;
                EmotionCardXmlInfo xmlInfo = emotionFloor[0];
                emotionFloor.RemoveAt(0);
                if (xmlInfo.id == 190201 || xmlInfo.id == 190202 || xmlInfo.id == 190209)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769019);
                }
                else if (xmlInfo.id == 190208)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769020);
                }
                else
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769016);
                }
                if (battleUnitModel == null)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList_random(Faction.Enemy, 1)[0];
                }
                if (battleUnitModel == null | battleUnitModel.IsDead())
                    return;
                battleUnitModel.emotionDetail.ApplyEmotionCard(xmlInfo);
            }
        }
        public void ChooseEmotionCard_Hod()
        {
            for (int i = 0; i < 3; i++)
            {
                BattleUnitModel battleUnitModel;
                EmotionCardXmlInfo xmlInfo = emotionFloor[0];
                emotionFloor.RemoveAt(0);
                if (xmlInfo.id == 190302 || xmlInfo.id == 190303)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769025);
                }
                else if (xmlInfo.id == 190312)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769021);
                }
                else if (xmlInfo.id == 190306 || xmlInfo.id == 190311 || xmlInfo.id == 190313)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769022);
                }
                else if (xmlInfo.id == 190314 || xmlInfo.id == 190315)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769024);
                }
                else if (xmlInfo.id == 190305)
                {
                    foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                    {
                        model.emotionDetail.ApplyEmotionCard(xmlInfo);
                    }
                    continue;
                }
                else if (xmlInfo.id == 190301)
                {
                    foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList())
                    {
                        model.emotionDetail.ApplyEmotionCard(xmlInfo);
                    }
                    continue;
                }
                else
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769023);
                }
                if (battleUnitModel == null)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList_random(Faction.Enemy, 1)[0];
                }
                if (battleUnitModel == null | battleUnitModel.IsDead())
                    return;
                battleUnitModel.emotionDetail.ApplyEmotionCard(xmlInfo);
            }
        }
        public void ChooseEmotionCard_Netzach()
        {
            for (int i = 0; i < 3; i++)
            {
                BattleUnitModel battleUnitModel;
                EmotionCardXmlInfo xmlInfo = emotionFloor[0];
                emotionFloor.RemoveAt(0);
                if (xmlInfo.id == 190402 || xmlInfo.id == 190403 || xmlInfo.id == 190407)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769030);
                }
                else if (xmlInfo.id == 190401 || xmlInfo.id == 190404 || xmlInfo.id == 190408 || xmlInfo.id == 190410)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769029);
                }
                else if (xmlInfo.id == 190405)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769027);
                }
                else if (xmlInfo.id == 190411)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769028);
                }
                else if (xmlInfo.id == 190409 || xmlInfo.id == 190406)
                {
                    foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                    {
                        model.emotionDetail.ApplyEmotionCard(xmlInfo);
                    }
                    continue;
                }
                else
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769026);
                }
                if (battleUnitModel == null)
                {
                    battleUnitModel = BattleObjectManager.instance.GetAliveList_random(Faction.Enemy, 1)[0];
                }
                if (battleUnitModel == null | battleUnitModel.IsDead())
                    return;
                battleUnitModel.emotionDetail.ApplyEmotionCard(xmlInfo);
            }
        }
    }
}

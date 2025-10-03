using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple
{
    public class AbnormalityAttribution
    {
        public static List<EmotionCardXmlInfo> emotionFloor = new List<EmotionCardXmlInfo>();
        public static void ResetEmotionDeck()
        {
            emotionFloor.Clear();
            Singleton<StageController>.Instance.GetStageModel().ClassInfo.floorNum += 1;
            for (int i = 1; i <= 15; i++)
            {
                int id = 190000 + (int)Singleton<StageController>.Instance.CurrentFloor * 100 + i;
                emotionFloor.Add(EmotionCardXmlList.Instance.GetData(id, SephirahType.None));
            }
            emotionFloor.Sort((EmotionCardXmlInfo x, EmotionCardXmlInfo y) => (int)(x.EmotionLevel - y.EmotionLevel));
        }
        public static void PickEmotionCard()
        {
            for (int i = 0; i < 3; i++)
            {
                BattleUnitModel battleUnitModel = null;
                EmotionCardXmlInfo xmlInfo = emotionFloor[0];
                emotionFloor.RemoveAt(0);
                if (ApplyAllEmotion(xmlInfo))
                    continue;
                switch (Singleton<StageController>.Instance.CurrentFloor)
                {
                    case SephirahType.Malkuth:
                        battleUnitModel = ChooseEmotionUnit_Malkuth(xmlInfo);
                        break;
                    case SephirahType.Yesod:
                        battleUnitModel = ChooseEmotionUnit_Yesod(xmlInfo);
                        break;
                    case SephirahType.Hod:
                        battleUnitModel = ChooseEmotionUnit_Hod(xmlInfo);
                        break;
                    case SephirahType.Netzach:
                        battleUnitModel = ChooseEmotionUnit_Netzach(xmlInfo);
                        break;
                    case SephirahType.Tiphereth:
                        battleUnitModel = ChooseEmotionUnit_TA(xmlInfo);
                        break;
                    case SephirahType.Gebura:
                        battleUnitModel = ChooseEmotionUnit_Gebura(xmlInfo);
                        break;
                    case SephirahType.Chesed:
                        battleUnitModel = ChooseEmotionUnit_Chesed(xmlInfo);
                        break;
                    case SephirahType.Binah:
                        battleUnitModel = ChooseEmotionUnit_Binah(xmlInfo);
                        break;
                    case SephirahType.Hokma:
                        battleUnitModel = ChooseEmotionUnit_Hokma(xmlInfo);
                        break;
                    case SephirahType.Keter:
                        battleUnitModel = ChooseEmotionUnit_Keter(xmlInfo);
                        break;
                }
                if (battleUnitModel == null)
                    battleUnitModel=GetLeastEmotion();
                if (battleUnitModel == null)
                    return;
                battleUnitModel.emotionDetail.ApplyEmotionCard(xmlInfo);
            }
            
        }
        public static bool ApplyAllEmotion(EmotionCardXmlInfo xmlInfo)
        {
            if(xmlInfo.id== 190305 || xmlInfo.id==190409 || xmlInfo.id==190506 || xmlInfo.id == 190712
                || xmlInfo.id == 190814 || xmlInfo.id == 190904 || xmlInfo.id == 190912 || xmlInfo.id == 191004)
            {
                foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                    model.emotionDetail.ApplyEmotionCard(xmlInfo);
                return true;
            }
            if(xmlInfo.id == 191007)
            {
                foreach (BattleUnitModel model in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
                {
                    if (model.Book.GetBookClassInfoId().id == 226769056)
                        continue;
                    model.emotionDetail.ApplyEmotionCard(xmlInfo);
                }
            }
            return false;
        }
        public static BattleUnitModel GetLeastEmotion()
        {
            List<BattleUnitModel> enemies = BattleObjectManager.instance.GetAliveList(Faction.Enemy);
            if(enemies.Count <= 0) 
                return null;
            enemies.Sort((x,y)=> x.emotionDetail.PassiveList.Count - y.emotionDetail.PassiveList.Count);
            return enemies[0];
        }
        public static BattleUnitModel ChooseEmotionUnit_Malkuth(EmotionCardXmlInfo xmlInfo)
        {
            if (xmlInfo == null)
                return null;
            if (xmlInfo.id == 190101 || xmlInfo.id == 190110 || xmlInfo.id == 190112 || xmlInfo.id == 190114 || xmlInfo.id == 190115)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                    (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769011);
            else if (xmlInfo.id == 190103 || xmlInfo.id == 190104 || xmlInfo.id == 190105 || xmlInfo.id == 190106 || xmlInfo.id == 190113)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769012);
            else if (xmlInfo.id == 190102 || xmlInfo.id == 190107 || xmlInfo.id == 190108 || xmlInfo.id == 190109 || xmlInfo.id == 190111)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769015);
            else
                return null;
        }
        public static BattleUnitModel ChooseEmotionUnit_Yesod(EmotionCardXmlInfo xmlInfo)
        {
            if (xmlInfo == null)
                return null;
            if (xmlInfo.id == 190204 || xmlInfo.id == 190205 || xmlInfo.id == 190208)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769016);
            else if (xmlInfo.id == 190203 || xmlInfo.id == 190211 || xmlInfo.id == 190215)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769017);
            else if (xmlInfo.id == 190207 || xmlInfo.id == 190210 || xmlInfo.id == 190213)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769018);
            else if(xmlInfo.id == 190202 || xmlInfo.id == 190206 || xmlInfo.id == 190209)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769019);
            else if (xmlInfo.id == 190201 || xmlInfo.id == 190212 || xmlInfo.id == 190214)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769020);
            return null;
        }
        public static BattleUnitModel ChooseEmotionUnit_Hod(EmotionCardXmlInfo xmlInfo)
        {
            if (xmlInfo == null)
                return null;
            if (xmlInfo.id == 190302 || xmlInfo.id == 190303 || xmlInfo.id == 190312 || xmlInfo.id == 190314)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769021);
            else if (xmlInfo.id == 190301 || xmlInfo.id == 190304 || xmlInfo.id == 190306 || xmlInfo.id == 190308)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769022);
            else if (xmlInfo.id == 190311)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769023);
            else if (xmlInfo.id == 190315)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769024);
            else if (xmlInfo.id == 190307 || xmlInfo.id == 190309 || xmlInfo.id == 190310 || xmlInfo.id == 190313)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769025);
            return null;
        }
        public static BattleUnitModel ChooseEmotionUnit_Netzach(EmotionCardXmlInfo xmlInfo)
        {
            if (xmlInfo == null)
                return null;
            if (xmlInfo.id == 190402 || xmlInfo.id == 190407 || xmlInfo.id == 190412 || xmlInfo.id == 190415)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769026);
            else if (xmlInfo.id == 190406 || xmlInfo.id == 190414)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769027);
            else if (xmlInfo.id == 190403 || xmlInfo.id == 190405)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769028);
            else if (xmlInfo.id == 190401 || xmlInfo.id == 190404 || xmlInfo.id == 190408 || xmlInfo.id == 190410)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769029);
            else if (xmlInfo.id == 190411 || xmlInfo.id == 190413)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769030);
            return null;
        }
        public static BattleUnitModel ChooseEmotionUnit_TA(EmotionCardXmlInfo xmlInfo)
        {
            if (xmlInfo == null)
                return null;
            if (xmlInfo.id == 190503 || xmlInfo.id == 190509 || xmlInfo.id == 190511 || xmlInfo.id == 190515)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769031);
            else if (xmlInfo.id == 190506 || xmlInfo.id == 190507 || xmlInfo.id == 190508 || xmlInfo.id == 190514)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769032);
            else if (xmlInfo.id == 190504 || xmlInfo.id == 190510)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769033);
            else if (xmlInfo.id == 190502 || xmlInfo.id == 190501)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769034);
            else if (xmlInfo.id == 190512 || xmlInfo.id == 190513)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769035);
            return null;
        }
        public static BattleUnitModel ChooseEmotionUnit_Gebura(EmotionCardXmlInfo xmlInfo)
        {
            return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769036);
        }
        public static BattleUnitModel ChooseEmotionUnit_Chesed(EmotionCardXmlInfo xmlInfo)
        {
            if (xmlInfo == null)
                return null;
            if (xmlInfo.id == 190701 || xmlInfo.id == 190705 || xmlInfo.id == 190706 )
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769041);
            else if (xmlInfo.id == 190704 || xmlInfo.id == 190707 || xmlInfo.id == 190709)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769042);
            else if (xmlInfo.id == 190711 || xmlInfo.id == 190713)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769043);
            else if (xmlInfo.id == 190702 || xmlInfo.id == 190703 || xmlInfo.id == 190708)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769044);
            else if (xmlInfo.id == 190710 || xmlInfo.id == 190714 || xmlInfo.id == 190715)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769045);
            return null;
        }
        public static BattleUnitModel ChooseEmotionUnit_Binah(EmotionCardXmlInfo xmlInfo)
        {
            if (xmlInfo == null)
                return null;
            if (xmlInfo.id == 190810 || xmlInfo.id == 190811|| xmlInfo.id == 190812 || xmlInfo.id == 190813)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769046);
            else if (xmlInfo.id == 190806 || xmlInfo.id == 190809)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769047);
            else if (xmlInfo.id == 190801 || xmlInfo.id == 190802 || xmlInfo.id == 190805)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769048);
            else if (xmlInfo.id == 190803 || xmlInfo.id == 190807 || xmlInfo.id == 190808)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769049);
            else if (xmlInfo.id == 190804 || xmlInfo.id == 190815)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769050);
            return null;
        }
        public static BattleUnitModel ChooseEmotionUnit_Hokma(EmotionCardXmlInfo xmlInfo)
        {
            if (xmlInfo == null)
                return null;
            if (xmlInfo.id == 190910 || xmlInfo.id == 190911 || xmlInfo.id == 190915)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769051);
            else if (xmlInfo.id == 190901 || xmlInfo.id == 190902 || xmlInfo.id == 190903)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769052);
            else if (xmlInfo.id == 190905 || xmlInfo.id == 190907)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769053);
            else if (xmlInfo.id == 190908 || xmlInfo.id == 190909 || xmlInfo.id == 190914)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769054);
            else if (xmlInfo.id == 190906 || xmlInfo.id == 190913)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769055);
            return null;
        }
        public static BattleUnitModel ChooseEmotionUnit_Keter(EmotionCardXmlInfo xmlInfo)
        {
            if (xmlInfo == null)
                return null;
            if (xmlInfo.id == 191002 || xmlInfo.id == 191006 || xmlInfo.id == 191012 || xmlInfo.id == 191014)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769056);
            else if (xmlInfo.id == 191008 || xmlInfo.id == 191011)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769057);
            else if (xmlInfo.id == 191003 || xmlInfo.id == 191010 || xmlInfo.id==191015 )
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769058);
            else if (xmlInfo.id == 191009 || xmlInfo.id == 191013)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769059);
            else if (xmlInfo.id == 191001 || xmlInfo.id == 191005)
                return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Find(
                        (BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769060);
            return null;
        }
    }
}

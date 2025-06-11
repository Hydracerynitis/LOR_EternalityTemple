using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using System.IO;
using LOR_XML;
using System.Xml.Serialization;
using LOR_DiceSystem;
using EI = EternalityTemple.EternalityInitializer;
using System.Xml;

namespace EternalityTemple
{
    [HarmonyPatch]
    static class LocalizeManager
    {
        [HarmonyPatch(typeof(LocalizedTextLoader), nameof(LocalizedTextLoader.LoadOthers))]
        [HarmonyPostfix]
        public static void LocalizedTextLoader_LoadOthers_Post(string language)
        {
            if (language != "cn") //未支持的语言设为中文
                language = "cn"; 
            LoadFormation();
            LoadStageName(language);
            LoadCharacter(language);
            LoadDropBook(language);
            LoadCardAbility(language);
            LoadEffect(language);
            LoadPassive(language);
            LoadCard(language);
            LoadExtra(language);
            LoadBook(language);
            AbnormalityLoader.LoadAbnormality(language);
        }
        public static void LoadFormation()
        {
                FormationXmlRoot formationXmlRoot;
                using (StringReader stringReader = new StringReader(File.ReadAllText(EternalityInitializer.ModPath + "/Data//FormationInfo.txt")))
                {
                    formationXmlRoot = (FormationXmlRoot)new XmlSerializer(typeof(FormationXmlRoot)).Deserialize(stringReader);
                }
                ((List<FormationXmlInfo>)typeof(FormationXmlList).GetField("_list", AccessTools.all).GetValue(Singleton<FormationXmlList>.Instance)).AddRange(formationXmlRoot.list);
        }
        public static void LoadStageName(string language)
        {
            Debug.Log("Eternality: Localization Path " + EI.ModPath + "/Localization/" + language + "/StageName/");
            foreach(FileInfo file in new DirectoryInfo(EI.ModPath + "/Localization/" + language + "/StageName/").GetFiles())
            try
            {
                using (StringReader stringReader = new StringReader(File.ReadAllText(file.FullName)))
                {
                    CharactersNameRoot charactersNameRoot = (CharactersNameRoot)new XmlSerializer(typeof(CharactersNameRoot)).Deserialize(stringReader);
                    foreach (StageClassInfo stage in Singleton<StageClassInfoList>.Instance.GetAllWorkshopData()[EI.packageId])
                            stage.stageName = charactersNameRoot.nameList.Find((x => x.ID == stage.id.id)).name;
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(EI.ModPath + "/StageLTLError.txt", ex.ToString());
            }
        }
        public static void LoadCharacter(string language)
        {
            Debug.Log("Eternality: Localization Path " + EI.ModPath + "/Localization/" + language + "/CharactersName/");
            List<EnemyUnitClassInfo> EUCI= Singleton<EnemyUnitClassInfoList>.Instance.GetAllWorkshopData()[EI.packageId];
            foreach (FileInfo file in new DirectoryInfo(EI.ModPath+ "/Localization/" + language + "/CharactersName/").GetFiles())
            {
                try
                {
                    using (StringReader stringReader = new StringReader(File.ReadAllText(file.FullName)))
                    {
                        CharactersNameRoot charactersNameRoot = (CharactersNameRoot)new XmlSerializer(typeof(CharactersNameRoot)).Deserialize(stringReader);
                        foreach(CharacterName CN in charactersNameRoot.nameList)
                        {
                            EnemyUnitClassInfo unit = EUCI.Find(x => x.nameId == CN.ID);
                            if (unit != null)
                                unit.name = CN.name;
                        }
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(EI.ModPath + "/CharacterLTLError_"+file.Name+".txt", ex.ToString());
                }
            }
        }
        public static void LoadDropBook(string language)
        {
            Debug.Log("Eternality: Localization Path " + EI.ModPath + "/Localization/" + language + "/DropBooks/");
            foreach (FileInfo file in new DirectoryInfo(EI.ModPath + "/Localization/" + language + "/DropBooks/").GetFiles())
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(File.ReadAllText(file.FullName));
                    foreach (XmlNode selectNode in xmlDocument.SelectNodes("localize/text"))
                    {
                        string str = string.Empty;
                        if (selectNode.Attributes.GetNamedItem("id") != null)
                            str = selectNode.Attributes.GetNamedItem("id").InnerText;
                        string key = str;
                        string innerText = selectNode.InnerText;
                        TextDataModel._dic[key] = innerText;
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(EI.ModPath + "/DropBookLTLError_"+file.Name+".txt", ex.ToString());
                }
            }
        }
        public static void LoadExtra(string language)
        {
            Debug.Log("Eternality: Localization Path " + EI.ModPath + "/Localization/" + language + "/etc/");
            foreach (FileInfo file in new DirectoryInfo(EI.ModPath+ "/Localization/" + language + "/etc/").GetFiles())
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(File.ReadAllText(file.FullName));
                    foreach (XmlNode selectNode in xmlDocument.SelectNodes("localize/text"))
                    {
                        string str = string.Empty;
                        if (selectNode.Attributes.GetNamedItem("id") != null)
                            str = selectNode.Attributes.GetNamedItem("id").InnerText;
                        string key = str;
                        string innerText = selectNode.InnerText;
                        TextDataModel._dic[key] = innerText;
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(EI.ModPath + "/EtcLTLError_" + file.Name + ".txt", ex.ToString());
                }
            }
        }
        public static void LoadCardAbility(string language)
        {
            Debug.Log("Eternality: Localization Path " + EI.ModPath + "/Localization/" + language + "/BattleCardAbilities/");
            foreach (FileInfo file in new DirectoryInfo(EI.ModPath + "/Localization/"+language+"/BattleCardAbilities/").GetFiles())
            {
                try
                {
                    using (StringReader stringReader = new StringReader(File.ReadAllText(file.FullName)))
                    {
                        Dictionary<string, BattleCardAbilityDesc> dict = new Dictionary<string, BattleCardAbilityDesc>();
                        foreach (BattleCardAbilityDesc cardDesc in ((BattleCardAbilityDescRoot)new XmlSerializer(typeof(BattleCardAbilityDescRoot)).Deserialize(stringReader)).cardDescList)
                            dict.Add(cardDesc.id, cardDesc);
                        Singleton<BattleCardAbilityDescXmlList>.Instance.AddByMode(EI.packageId, dict);
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(EI.ModPath + "/CardAbilityLTLError_"+ file.Name + ".txt", ex.ToString());
                }
            }
            
        }
        public static void LoadEffect(string language)
        {
            Debug.Log("Eternality: Localization Path " + EI.ModPath + "/Localization/" + language + "/EffectTexts/");
            foreach (FileInfo file in new DirectoryInfo(EI.ModPath + "/Localization/" +language + "/EffectTexts/").GetFiles())
            {
                try
                {
                    using (StringReader stringReader = new StringReader(File.ReadAllText(file.FullName)))
                    {
                        Dictionary<string, BattleEffectText> dict = BattleEffectTextsXmlList.Instance._dictionary;
                        foreach (BattleEffectText effectText in ((BattleEffectTextRoot)new XmlSerializer(typeof(BattleEffectTextRoot)).Deserialize(stringReader)).effectTextList)
                        {
                            if(dict.Keys.Contains(effectText.ID))
                                dict[effectText.ID] = effectText;
                            dict.Add(effectText.ID, effectText);
                        }
                            
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(EI.ModPath + "/EffectLTLError_"+file.Name+".txt", ex.ToString());
                }
            }
            
        }
        public static void LoadPassive(string language)
        {
            Debug.Log("Eternality: Localization Path " + EI.ModPath + "/Localization/" + language + "/PassiveDesc/");
            foreach (FileInfo file in new DirectoryInfo(EI.ModPath + "/Localization/" +language + "/PassiveDesc/").GetFiles())
            {
                try
                {
                    using (StringReader stringReader = new StringReader(File.ReadAllText(file.FullName)))
                    {
                        PassiveDescRoot passiveDescRoot = (PassiveDescRoot)new XmlSerializer(typeof(PassiveDescRoot)).Deserialize(stringReader);
                        foreach (PassiveXmlInfo passiveXmlInfo in Singleton<PassiveXmlList>.Instance.GetDataAll().FindAll(x => x.id.packageId == EI.packageId))
                        {
                            passiveXmlInfo.name = passiveDescRoot.descList.Find(x => x.ID == passiveXmlInfo.id.id).name;
                            passiveXmlInfo.desc = passiveDescRoot.descList.Find(x => x.ID == passiveXmlInfo.id.id).desc;
                        }
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(EI.ModPath + "/PassiveLTLError_"+file.Name+".txt", ex.ToString());
                }
            }
        }
        public static void LoadCard(string language)
        {
            Debug.Log("Eternality: Localization Path " + EI.ModPath + "/Localization/" + language + "/BattlesCards/");
            foreach (FileInfo file in new DirectoryInfo(EI.ModPath + "/Localization/" +language + "/BattlesCards/").GetFiles())
            {
                try
                {
                    using (StringReader stringReader = new StringReader(File.ReadAllText(file.FullName)))
                    {
                        BattleCardDescRoot battleCardDescRoot = (BattleCardDescRoot)new XmlSerializer(typeof(BattleCardDescRoot)).Deserialize(stringReader);
                        foreach (DiceCardXmlInfo diceCardXmlInfo in ItemXmlDataList.instance.GetAllWorkshopData()[EI.packageId])
                            diceCardXmlInfo.workshopName = battleCardDescRoot.cardDescList.Find(x => x.cardID == diceCardXmlInfo.id.id)?.cardName;
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(EI.ModPath + "/CardLTLError_" + file.Name+".txt", ex.ToString());
                }
            }
        }
        public static void LoadBook(string language)
        {
            Debug.Log("Eternality: Localization Path " + EI.ModPath + "/Localization/" + language + "/Books/");
            foreach (FileInfo file in new DirectoryInfo(EI.ModPath + "/Localization/" +language + "/Books/").GetFiles())
            {
                try
                {
                    using (StringReader stringReader = new StringReader(File.ReadAllText(file.FullName)))
                    {
                        BookDescRoot bookDescRoot = (BookDescRoot)new XmlSerializer(typeof(BookDescRoot)).Deserialize(stringReader);
                        foreach (BookXmlInfo bookXmlInfo in Singleton<BookXmlList>.Instance.GetList().FindAll(x => x.id.packageId == EI.packageId))
                        {
                            BookDesc bookDesc = bookDescRoot.bookDescList.Find(x => x.bookID == bookXmlInfo.TextId);
                            if(bookDesc != null)
                            {
                                bookXmlInfo.InnerName = bookDesc.bookName;
                            }
                            else
                            {
                                string originBookName = BookDescXmlList.Instance.GetBookName(new LorId(bookXmlInfo.TextId));
                                if (originBookName != string.Empty)
                                {
                                    bookXmlInfo.InnerName = originBookName;
                                }
                            }
                        }
                        Singleton<BookDescXmlList>.Instance.AddBookTextByMod(EI.packageId, bookDescRoot.bookDescList);
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(EI.ModPath + "/BookLTLError_" + file.Name + ".txt", ex.ToString());
                }
            }
        }
    }
}
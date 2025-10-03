using Battle.DiceAttackEffect;
using EternalityTemple.Inaba;
using EternalityTemple.Kaguya;
using EternalityTemple.Yagokoro;
using GameSave;
using HarmonyLib;
using LOR_BattleUnit_UI;
using LOR_DiceSystem;
using Mod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Workshop;
using static UnityEngine.UI.GridLayoutGroup;
using Diagonis = System.Diagnostics;
using FileInfo = System.IO.FileInfo;
using UnityObject = UnityEngine.Object;

namespace EternalityTemple
{
    [HarmonyPatch]
    public class EternalityInitializer : ModInitializer
    {
        public static string ModPath;
        public static Dictionary<string, Sprite> ArtWorks = new Dictionary<string, Sprite>();
        public static List<UnitBattleDataModel> SecondBattleLibrarians = new List<UnitBattleDataModel>();
        public const string packageId = "TheWorld_Eternity";
        private static Dictionary<SpeedDiceUI, Color> ChangedSpeedDiceUI = new Dictionary<SpeedDiceUI, Color>();
        public static Dictionary<string, AssetBundle> assetBundle;
        public static Dictionary<string, Type> CustomEffects;

        public static LorId GetLorId(int id) => new LorId(packageId, id);
        public override void OnInitializeMod()
        {
            base.OnInitializeMod();
            string AssemblyPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            ModPath = AssemblyPath.Substring(0, AssemblyPath.Length - 11);
            Debug.Log("Eternality: ModPath " + ModPath);
            GetArtWorks(new DirectoryInfo(ModPath + "/Resource/ExtraArtWork"));
            Harmony harmony = new Harmony("Eternality");
            harmony.PatchAll(typeof(EternalityInitializer));
            harmony.PatchAll(typeof(LocalizeManager));
            harmony.PatchAll(typeof(AbnormalityLoader));
            harmony.PatchAll(typeof(ExtraArtworkPatch));
            RemoveError();
            AbnormalityLoader.LoadEmotion();
            LocalizeManager.LocalizedTextLoader_LoadOthers_Post(TextDataModel.CurrentLanguage);
            assetBundle = new Dictionary<string, AssetBundle>();
            CustomEffects = new Dictionary<string, Type>();
            AddAssets();
            if (Singleton<EternalityTempleSaveManager>.Instance.LoadData("passFloor") == null) 
            {
                Singleton<EternalityTempleSaveManager>.Instance.SaveData(new SaveData(SaveDataType.Dictionary), "passFloor");
            }
            bool BaseModLoaded = false;
            foreach(Assembly dll in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (dll.GetName().Name == "UnitRenderUtils")
                {
                    BaseModLoaded = true;
                    break;
                }
            }
            if (!BaseModLoaded)
                harmony.PatchAll(typeof(BaseModPatch));
            LoadEternalitySkin();
        }
        //添加特效AB包
        public static void AddAssets()
        {
            AddAsset("Eternality", ModPath + "/AB/EternalityFX.ab");
        }
        public static void AddAsset(string name, string path)
        {
            AssetBundle value = AssetBundle.LoadFromFile(path);
            assetBundle.Add(name, value);
        }
        //加载S动作
        public static void LoadEternalitySkin()
        {
            try
            {
                string path = ModPath + "/Resource/CharacterSkin";
                Debug.Log($"Eternality SkinDirectory: {path}");
                List<WorkshopSkinData> list = new List<WorkshopSkinData>();
                if (!Directory.Exists(path))
                    return;
                string[] directories = Directory.GetDirectories(path);
                for (int index = 0; index < directories.Length; ++index)
                {
                    string modInfoPath = directories[index] + "/ModInfo.xml";
                    Debug.Log($"Eternality Skinpath: {modInfoPath}");
                    WorkshopAppearanceInfo workshopAppearanceInfo = File.Exists(modInfoPath) ?
                        LoadEternalityAppearanceInfo(directories[index], modInfoPath) : null;
                    if (workshopAppearanceInfo != null)
                    {
                        string[] strArray = directories[index].Split('\\');
                        string str = strArray[strArray.Length - 1];
                        workshopAppearanceInfo.path = directories[index];
                        workshopAppearanceInfo.uniqueId = packageId;
                        workshopAppearanceInfo.bookName = str;
                        Debug.Log((object)("workshop bookName : " + workshopAppearanceInfo.bookName));
                        if (workshopAppearanceInfo.isClothCustom)
                            list.Add(new WorkshopSkinData()
                            {
                                dic = workshopAppearanceInfo.clothCustomInfo,
                                dataName = workshopAppearanceInfo.bookName,
                                contentFolderIdx = workshopAppearanceInfo.uniqueId,
                                id = index
                            });
                    }
                }
                CustomizingBookSkinLoader.Instance._bookSkinData[packageId] = list;
            }
            catch (Exception ex)
            {
                Debug.LogError((object)ex);
            }
        }
        private static WorkshopAppearanceInfo LoadEternalityAppearanceInfo(string rootPath, string xml)
        {
            WorkshopAppearanceInfo workshopAppearanceInfo = new WorkshopAppearanceInfo();
            if (string.IsNullOrEmpty(xml))
                return null;
            StreamReader streamReader = new StreamReader(xml);
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(streamReader.ReadToEnd());
                XmlNode xmlNode1 = xmlDocument.SelectSingleNode("ModInfo");
                XmlNode xmlNode3 = xmlNode1.SelectSingleNode("ClothInfo");
                if (xmlNode3 != null)
                {
                    workshopAppearanceInfo.isClothCustom = true;
                    string innerText = xmlNode3.SelectSingleNode("Name").InnerText;
                    if (!string.IsNullOrEmpty(innerText))
                        workshopAppearanceInfo.bookName = innerText;
                    for (int index = 0; index <= 30; ++index)
                    {
                        ActionDetail key = (ActionDetail)index;
                        switch (key)
                        {
                            case ActionDetail.Standing:
                            case ActionDetail.NONE:
                                continue;
                            default:
                                string xpath = key.ToString();
                                try
                                {
                                    XmlNode xmlNode5 = xmlNode3.SelectSingleNode(xpath);
                                    if (xmlNode5 == null)
                                    {
                                        xpath = "Penetrate";
                                        xmlNode5 = xmlNode3.SelectSingleNode(xpath);
                                    }
                                    if (xmlNode5 != null)
                                    {
                                        string path1 = rootPath + "/ClothCustom/" + xpath + ".png";
                                        string path2 = rootPath + "/ClothCustom/" + xpath + "_front.png";
                                        XmlNode xmlNode6 = xmlNode5.SelectSingleNode("Pivot");
                                        XmlNode namedItem1 = xmlNode6.Attributes.GetNamedItem("pivot_x");
                                        XmlNode namedItem2 = xmlNode6.Attributes.GetNamedItem("pivot_y");
                                        XmlNode xmlNode7 = xmlNode5.SelectSingleNode("Head");
                                        XmlNode namedItem3 = xmlNode7.Attributes.GetNamedItem("head_x");
                                        XmlNode namedItem4 = xmlNode7.Attributes.GetNamedItem("head_y");
                                        XmlNode namedItem5 = xmlNode7.Attributes.GetNamedItem("rotation");
                                        XmlNode xmlNode8 = xmlNode5.SelectSingleNode("Direction");
                                        XmlNode namedItem6 = xmlNode7.Attributes.GetNamedItem("head_enable");
                                        float num1 = float.Parse(namedItem1.InnerText);
                                        float num2 = float.Parse(namedItem2.InnerText);
                                        float num3 = float.Parse(namedItem3.InnerText);
                                        float num4 = float.Parse(namedItem4.InnerText);
                                        float num5 = float.Parse(namedItem5.InnerText);
                                        bool result = true;
                                        if (namedItem6 != null)
                                            bool.TryParse(namedItem6.InnerText, out result);
                                        Vector2 vector2_1 = new Vector2((float)(((double)num1 + 512.0) / 1024.0), (float)(((double)num2 + 512.0) / 1024.0));
                                        Vector2 vector2_2 = new Vector2(num3 / 100f, num4 / 100f);
                                        bool flag1 = false;
                                        string str1 = path1;
                                        string str2 = path2;
                                        bool flag2 = false;
                                        bool flag3 = false;
                                        if (File.Exists(path1))
                                            flag2 = true;
                                        if (File.Exists(path2))
                                        {
                                            flag1 = true;
                                            flag3 = true;
                                        }
                                        CharacterMotion.MotionDirection motionDirection = CharacterMotion.MotionDirection.FrontView;
                                        if (xmlNode8.InnerText == "Side")
                                            motionDirection = CharacterMotion.MotionDirection.SideView;
                                        ClothCustomizeData clothCustomizeData = new ClothCustomizeData()
                                        {
                                            spritePath = str1,
                                            frontSpritePath = str2,
                                            hasFrontSprite = flag1,
                                            pivotPos = vector2_1,
                                            headPos = vector2_2,
                                            headRotation = num5,
                                            direction = motionDirection,
                                            headEnabled = result,
                                            hasFrontSpriteFile = flag3,
                                            hasSpriteFile = flag2
                                        };
                                        if (str1 != null)
                                        {
                                            workshopAppearanceInfo.clothCustomInfo.Add(key, clothCustomizeData);
                                            continue;
                                        }
                                        continue;
                                    }
                                    continue;
                                }
                                catch (Exception ex)
                                {
                                    Debug.LogError((object)ex);
                                    continue;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError((object)ex);
            }
            return workshopAppearanceInfo;
        }
        [HarmonyPatch(typeof(WorkshopSkinDataSetter),nameof(WorkshopSkinDataSetter.SetData),typeof(WorkshopSkinData))]
        [HarmonyPrefix]
        public static void WorkshopSkinDataSetter_SetData(WorkshopSkinDataSetter __instance,WorkshopSkinData data)
        {
            if (data.contentFolderIdx == packageId)
            {
                foreach(ActionDetail key in data.dic.Keys)
                {
                    CharacterMotion characterMotion = __instance.Appearance.GetCharacterMotion(key);
                    if (characterMotion == null)
                    {
                        characterMotion = UnityEngine.Object.Instantiate<CharacterMotion>(__instance.Appearance._motionList[0], __instance.transform);
                        characterMotion.gameObject.name = "Custom_" + key.ToString();
                        characterMotion.actionDetail = key;
                        characterMotion.motionSpriteSet.Clear();
                        characterMotion.motionSpriteSet.Add(new SpriteSet(characterMotion.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>(), CharacterAppearanceType.Body));
                        characterMotion.motionSpriteSet.Add(new SpriteSet(characterMotion.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>(), CharacterAppearanceType.Head));
                        characterMotion.motionSpriteSet.Add(new SpriteSet(characterMotion.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>(), CharacterAppearanceType.Body));
                        __instance.Appearance._motionList.Add(characterMotion);
                    }
                }
            }
        }
        //加载骰子特效
        [HarmonyPatch(typeof(DiceEffectManager), nameof(DiceEffectManager.CreateBehaviourEffect))]
        [HarmonyPrefix]
        public static bool DiceEffectManager_CreateBehaviourEffect_Pre(DiceEffectManager __instance, ref DiceAttackEffect __result, string resource, float scaleFactor, BattleUnitView self, BattleUnitView target, float time = 1f)
        {
            bool result;
            if (resource == null)
            {
                __result = null;
                result = false;
            }
            else
            {
                if (!EternalityInitializer.CustomEffects.ContainsKey(resource) && resource != string.Empty)
                {
                    foreach (Type type in Assembly.LoadFrom(EternalityInitializer.ModPath + "/Assemblies/EternalityTemple.dll").GetTypes())
                    {
                        if (type.Name == "DiceAttackEffect_" + resource)
                        {
                            Type value = type;
                            EternalityInitializer.CustomEffects[resource] = value;
                            break;
                        }
                    }
                }
                if (EternalityInitializer.CustomEffects.ContainsKey(resource))
                {
                    Type componentType = EternalityInitializer.CustomEffects[resource];
                    DiceAttackEffect diceAttackEffect = new GameObject(resource).AddComponent(componentType) as DiceAttackEffect;
                    diceAttackEffect.Initialize(self, target, 1f);
                    diceAttackEffect.SetScale(scaleFactor);
                    __result = diceAttackEffect;
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }
        //移除重复加载同一DLL的错误提示
        public static void RemoveError()
        {
            List<string> LoadMod = new List<string>() { "0Harmony","Mono.Cecil","MonoMod" };
            List<string> ErrorLogs = new List<string>();
            foreach (string errorLog in Singleton<ModContentManager>.Instance.GetErrorLogs())
            {
                if (LoadMod.Exists(x => errorLog.Contains(x)))
                    ErrorLogs.Add(errorLog);
            }
            foreach (string str in ErrorLogs)
                ModContentManager.Instance.GetErrorLogs().Remove(str);
        }
        //额外加载美术资源
        public static void GetArtWorks(DirectoryInfo dir)
        {
            if (dir.GetDirectories().Length != 0)
            {
                foreach (DirectoryInfo directory in dir.GetDirectories())
                    GetArtWorks(directory);
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(File.ReadAllBytes(file.FullName));
                Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.0f, 0.0f));
                string withoutExtension = Path.GetFileNameWithoutExtension(file.FullName);
                ArtWorks[withoutExtension] = sprite;
            }
        }
        [HarmonyPatch(typeof(EmotionCardXmlList), nameof(EmotionCardXmlList.GetEnemyEmotionNeutralCardList))]
        [HarmonyPostfix]
        public static void EmotionCardXmlList_GetEnemyEmotionNeutralCardList(ref List<EmotionCardXmlInfo> __result)
        {
            __result.Remove(Singleton<EmotionCardXmlList>.Instance.GetData(1, SephirahType.None));
            __result.Remove(Singleton<EmotionCardXmlList>.Instance.GetData(4, SephirahType.None));
        }
        //mod战斗需要的基础书和无限数字显示
        [HarmonyPatch(typeof(UIInvitationDropBookSlot), nameof(UIInvitationDropBookSlot.SetData_DropBook))]
        [HarmonyPostfix]
        public static void UIInvitationDropBookSlot_SetData_DropBook_Post(ref TextMeshProUGUI ___txt_bookNum, LorId bookId)
        {
            if (Singleton<DropBookInventoryModel>.Instance.GetBookCount(bookId) == 0)
                ___txt_bookNum.text = "∞";
        }
        [HarmonyPatch(typeof(DropBookInventoryModel), nameof(DropBookInventoryModel.GetBookList_invitationBookList))]
        [HarmonyPostfix]
        public static void DropBookInventoryModel_GetBookList_invitationBookList(List<LorId> __result)
        {
            __result.Add(GetLorId(226769000));
        }
        //被动额外扳机 IsBufImmune 是否免疫特定buff
        [HarmonyPatch(typeof(BattleUnitBufListDetail), nameof(BattleUnitBufListDetail.CanAddBuf))]
        [HarmonyPostfix]
        public static void BattleUnitBufListDetail_CanAddBuf(BattleUnitBufListDetail __instance, ref bool __result, BattleUnitBuf buf)
        {
            List<PassiveAbilityBase> passiveInterface = __instance._self.passiveDetail.PassiveList.FindAll(x => x is IsBufImmune);
            foreach (PassiveAbilityBase passiveAbility in passiveInterface)
            {
                if ((passiveAbility as IsBufImmune).IsImmune(buf))
                    __result = false;
            }
        }
        //Buf额外扳机 OnAddOtherBuf和OnGiveOtherBuf  获取或施加其他buff时会触发此扳机
        [HarmonyPatch(typeof(BattleUnitBufListDetail), nameof(BattleUnitBufListDetail.AddKeywordBufThisRoundByCard))]
        [HarmonyPostfix]
        public static void BattleUnitBufListDetail_AddKeywordBufThisRoundByCard_Post(BattleUnitBufListDetail __instance, KeywordBuf bufType, ref int stack, BattleUnitModel actor)
        {
            BattleUnitBuf buf = __instance.AddNewKeywordBufInList(BufReadyType.ThisRound, bufType);
            TriggerOnGiveBuff(actor, buf, stack);
        }
        [HarmonyPatch(typeof(BattleUnitBufListDetail), nameof(BattleUnitBufListDetail.AddKeywordBufThisRoundByEtc))]
        [HarmonyPostfix]
        public static void BattleUnitBufListDetail_AddKeywordBufThisRoundByEtc_Post(BattleUnitBufListDetail __instance, KeywordBuf bufType, ref int stack, BattleUnitModel actor)
        {
            BattleUnitBuf buf = __instance.AddNewKeywordBufInList(BufReadyType.ThisRound, bufType);
            TriggerOnGiveBuff(actor, buf, stack);
        }
        [HarmonyPatch(typeof(BattleUnitBufListDetail), nameof(BattleUnitBufListDetail.AddKeywordBufByCard))]
        [HarmonyPostfix]
        public static void BattleUnitBufListDetail_AddKeywordBufByCard_Post(BattleUnitBufListDetail __instance, KeywordBuf bufType, ref int stack, BattleUnitModel actor)
        {
            if (bufType == KeywordBuf.Burn)
                return;
            BattleUnitBuf buf = __instance.AddNewKeywordBufInList(BufReadyType.NextRound, bufType);
            TriggerOnGiveBuff(actor, buf, stack);
        }
        [HarmonyPatch(typeof(BattleUnitBufListDetail), nameof(BattleUnitBufListDetail.AddKeywordBufByEtc))]
        [HarmonyPostfix]
        public static void BattleUnitBufListDetail_AddKeywordBufByEtc_Post(BattleUnitBufListDetail __instance, KeywordBuf bufType, ref int stack, BattleUnitModel actor)
        {
            if (bufType == KeywordBuf.Burn)
                return;
            BattleUnitBuf buf = __instance.AddNewKeywordBufInList(BufReadyType.NextRound, bufType);
            TriggerOnGiveBuff(actor, buf, stack);
        }
        [HarmonyPatch(typeof(BattleUnitBufListDetail), nameof(BattleUnitBufListDetail.AddKeywordBufNextNextByCard))]
        [HarmonyPostfix]
        public static void BattleUnitBufListDetail_AddKeywordBufNextNextByCard_Post(BattleUnitBufListDetail __instance, KeywordBuf bufType, ref int stack, BattleUnitModel actor)
        {
            if (bufType == KeywordBuf.Burn)
                return;
            BattleUnitBuf buf = __instance.AddNewKeywordBufInList(BufReadyType.NextNextRound, bufType);
            TriggerOnGiveBuff(actor, buf, stack);
        }
        private static void TriggerOnGiveBuff(BattleUnitModel actor, BattleUnitBuf buf, int stack)
        {
            if (buf == null)
                return;
            List<BattleUnitBuf> BufInterface = actor.bufListDetail.GetActivatedBufList().FindAll(x => x is OnGiveOtherBuf);
            foreach (BattleUnitBuf BufAbility in BufInterface)
                (BufAbility as OnGiveOtherBuf).OnGiveBuf(buf, stack);
            BufInterface = buf._owner.bufListDetail.GetActivatedBufList().FindAll(x => x is OnAddOtherBuf);
            foreach (BattleUnitBuf BufAbility in BufInterface)
                (BufAbility as OnAddOtherBuf).OnAddBuf(buf, stack);
        }
        //Buf额外扳机 OnRecoverHP  恢复生命时额外触发此扳机
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnRecoverHp))]
        [HarmonyPostfix]
        public static void BattleUnitModel_OnRecoverHp_Post(BattleUnitModel __instance, int recoverAmount)
        {
            List<BattleUnitBuf> BufInterface = __instance.bufListDetail.GetActivatedBufList().FindAll(x => x is OnRecoverHP);
            foreach (BattleUnitBuf BufAbility in BufInterface)
                (BufAbility as OnRecoverHP).OnHeal(recoverAmount);
        }
        //对面书页Tooltip描述添加和月相描述增加
        [HarmonyPatch(typeof(BattleDiceCardUI),nameof(BattleDiceCardUI.SetCard))]
        [HarmonyPostfix]
        public static void BattleDiceCardUI_SetCard_Post(BattleDiceCardUI __instance, BattleDiceCardModel cardModel)
        {
            if (__instance.isProfileCard && cardModel!=null && cardModel.owner?.UnitData.unitData.EnemyUnitId!=LorId.None
                 && cardModel.XmlData.id.packageId == packageId)
            {
                __instance.KeywordListUI.Init(cardModel.XmlData, cardModel.GetBehaviourList());
                __instance.KeywordListUI.Activate();
            }
            else
            {
                __instance.KeywordListUI.Deactivate();
            }
            if(cardModel._script!= null && cardModel._script is MoonCardAbility)
            {
                MoonCardAbility moonAbility=cardModel._script as MoonCardAbility;
                if (moonAbility.moonPreview != 0)
                {
                    string text = "";
                    List<DiceBehaviour> behaviourList = cardModel.GetBehaviourList();
                    string defaultString = BattleCardAbilityDescXmlList.Instance.GetDefaultAbilityDescString(cardModel.XmlData);
                    string abilityString;
                    if (cardModel.owner.bufListDetail.HasBuf<BattleUnitBuf_Moon3>())
                        abilityString = moonAbility.GetFullMoonAbilityText();
                    else
                        abilityString = moonAbility.GetMoonAbilityText();
                    if (!string.IsNullOrEmpty(defaultString))
                        text = defaultString + "\n" + abilityString;
                    else
                        text = abilityString;
                    __instance.txt_selfAbility.text = TextUtil.TransformConditionKeyword(text);
                    if (!string.IsNullOrEmpty(text))
                    {
                        __instance.selfAbilityArea.SetActive(true);
                        __instance.txt_selfAbility.text = TextUtil.TransformConditionKeyword(text);
                        float preferredHeight = __instance.txt_selfAbility.preferredHeight;
                        int num = Mathf.Min(preferredHeight >= 260.0 ? (preferredHeight >= 480.0 ? (preferredHeight >= 700.0 ?
                            3 : 2) : 1) : 0, 4 -   cardModel.GetBehaviourList().Count);
                        RectTransform component = __instance.selfAbilityArea.GetComponent<RectTransform>();
                        if (component != null)
                        {
                            switch (num)
                            {
                                case 1:
                                    component.sizeDelta = new Vector2(component.sizeDelta.x, 440f);
                                    break;
                                case 2:
                                    component.sizeDelta = new Vector2(component.sizeDelta.x, 660f);
                                    break;
                                case 3:
                                    component.sizeDelta = new Vector2(component.sizeDelta.x, 880f);
                                    break;
                                default:
                                    component.sizeDelta = new Vector2(component.sizeDelta.x, 220f);
                                    break;
                            }
                        }
                    }
                    else
                        __instance.selfAbilityArea.SetActive(false);
                    for (int index = 0; index < behaviourList.Count; ++index)
                    {
                        __instance.ui_behaviourDescList[index].SetBehaviourInfo(behaviourList[index], cardModel.GetID(), cardModel.GetBehaviourList(), false);
                        __instance.ui_behaviourDescList[index].gameObject.SetActive(true);
                        Sprite sprite = behaviourList[index].Type == BehaviourType.Standby ? UISpriteDataManager.instance.CardStandbyBehaviourDetailIcons[(int)behaviourList[index].Detail] : UISpriteDataManager.instance._cardBehaviourDetailIcons[(int)behaviourList[index].Detail];
                        __instance.img_behaviourDetatilList[index].sprite = sprite;
                        __instance.img_behaviourDetatilList[index].gameObject.SetActive(true);
                    }
                    for (int count = behaviourList.Count; count < __instance.ui_behaviourDescList.Count; ++count)
                    {
                        __instance.ui_behaviourDescList[count].gameObject.SetActive(false);
                        __instance.img_behaviourDetatilList[count].gameObject.SetActive(false);
                    }
                }
                
            }
        }
        //速度骰染色： 涉及此功能的有 谜题骰，疯狂骰和月相骰
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.RollSpeedDice))]
        [HarmonyPostfix]
        public static void BattleUnitModel_RollSpeedDice_Post(BattleUnitModel __instance)
        {
            if (__instance.IsBreakLifeZero() || __instance.IsKnockout() || __instance.turnState == BattleUnitTurnState.BREAK || __instance.bufListDetail.HasStun())
                return;
            SpeedDiceSetter SDS = __instance.view.speedDiceSetterUI;
            int unavailable = __instance.speedDiceResult.FindAll(x => x.breaked).Count;
            if (__instance.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_PuzzleBuf) is BattleUnitBuf_PuzzleBuf puzzlebuf)
            {
                if (puzzlebuf.CompletePuzzle.Contains(1) && __instance.speedDiceCount - unavailable >= 1)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable + 0), Color.grey, "ICON_1", "ICON_1Glow", "ICON_NoneHovered");
                if (puzzlebuf.CompletePuzzle.Contains(2) && __instance.speedDiceCount - unavailable >= 2)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable + 1), new Color(1f, 0.5f, 0f), "ICON_2", "ICON_2Glow", "ICON_NoneHovered");
                if (puzzlebuf.CompletePuzzle.Contains(3) && __instance.speedDiceCount - unavailable >= 3)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable + 2), Color.magenta, "ICON_3", "ICON_3Glow", "ICON_NoneHovered");
                if (puzzlebuf.CompletePuzzle.Contains(4) && __instance.speedDiceCount - unavailable >= 4)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable + 3), new Color(0.5f, 0f, 0.5f), "ICON_4", "ICON_4Glow", "ICON_NoneHovered");
                if (puzzlebuf.CompletePuzzle.Contains(5) && __instance.speedDiceCount - unavailable >= 5)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable + 4), Color.yellow, "ICON_5", "ICON_5Glow", "ICON_NoneHovered");
            }
            PassiveAbility_226769005 passive = __instance.passiveDetail.PassiveList.Find((PassiveAbilityBase x) => x is PassiveAbility_226769005) as PassiveAbility_226769005;
            if (passive != null)
            {
                if (passive.IsActivate || passive.TempActivate)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        SpeedDiceUI speedDiceUI = SDS.GetSpeedDiceByIndex(unavailable + i - 1);
                        ChangeSpeedDiceColor(speedDiceUI, Color.cyan, "ICON_Eirin月相" + i, "ICON_EirinGlow", "ICON_EirinHovered");
                    }
                }   
            }
            if (BattleUnitBuf_InabaBuf2.GetStack(__instance) > 0)
            {
                for (int i = unavailable; i < BattleUnitBuf_InabaBuf2.GetStack(__instance)+unavailable; i++)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(i), Color.red, "ICON_Reisen", "ICON_ReisenGlow", "ICON_ReisenHovered");
            }
        }
        private static Sprite defaultSpeedDice1;
        private static Sprite defaultSpeedDice2;
        private static Sprite defaultSpeedDice3;
        private static void ChangeSpeedDiceColor(SpeedDiceUI ui, Color DiceColor, string normal, string glow, string hovered, bool reset = false) //DiceColor不能是白色，因为白色只会让数字变成白色，骰子框仍为原来颜色
        {
            if (ui == null)
                return;
            if (defaultSpeedDice1 == null)
            {
                defaultSpeedDice1 = ui.img_normalFrame.sprite;
                defaultSpeedDice2 = ui.img_lightFrame.sprite;
                defaultSpeedDice3 = ui.img_highlightFrame.sprite;
            }
            if (!reset)
            {
                if(!ChangedSpeedDiceUI.ContainsKey(ui))
                    ChangedSpeedDiceUI.Add(ui, ui.img_normalFrame.color);
            }
            if (normal != "")
            {
                ui.img_normalFrame.sprite = ArtWorks[normal];
                ui.img_lightFrame.sprite = ArtWorks[glow];
                ui.img_highlightFrame.sprite = ArtWorks[hovered];
            }
            else
            {
                ui.img_normalFrame.sprite = defaultSpeedDice1;
                ui.img_lightFrame.sprite = defaultSpeedDice2;
                ui.img_highlightFrame.sprite = defaultSpeedDice3;
            }
            ui._txtSpeedRange.color = DiceColor;
            ui._rouletteImg.color = DiceColor;
            ui._txtSpeedMax.color = DiceColor;
            ui.img_tensNum.color = DiceColor;
            ui.img_unitsNum.color = DiceColor;
            DiceColor.a -= 0.6f;
            ui.img_breakedFrame.color = DiceColor;
            ui.img_breakedLinearDodge.color = DiceColor;
            ui.img_lockedFrame.color = DiceColor;
            ui.img_lockedIcon.color = DiceColor;
        }
        private static void ChangeSpeedNumColor(SpeedDiceUI ui, Color NumColor, bool reset = false)
        {
            if (ui == null)
                return;
            if (!reset && !ChangedSpeedDiceUI.ContainsKey(ui))
                ChangedSpeedDiceUI.Add(ui, ui.img_normalFrame.color);
            ui.img_tensNum.color = NumColor;
            ui.img_unitsNum.color = NumColor;
            ui._txtSpeedMax.color = NumColor;
        }
        public static void ResetSpeedDiceColor() //重制所有染色的速度骰，任何要有染色的被动都需要有引用这个方法
        {
            foreach (SpeedDiceUI ui in ChangedSpeedDiceUI.Keys)
                ChangeSpeedDiceColor(ui, ChangedSpeedDiceUI[ui], "", "", "", true);
            ChangedSpeedDiceUI.Clear();
        }
        //修复不可控制的速度骰不会高亮的问题
        [HarmonyPatch(typeof(SpeedDiceUI), nameof(SpeedDiceUI.SetLightOn))]
        [HarmonyPrefix]
        public static bool SpeedDiceUI_SetLightOn_Pre(bool b)
        {
            if (b == true && StageController.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                return false;
            return true;
        }
        //观测被动，玩家额外选取2个异想体
        [HarmonyPatch(typeof(StageLibraryFloorModel),nameof(StageLibraryFloorModel.OnPickPassiveCard))]
        [HarmonyPrefix]
        public static void StageLibraryFloorModel_OnPickPassiveCard_Pre(StageLibraryFloorModel __instance)
        {
            int skillPoint = __instance.team.emotionLevel - __instance.team.currentSelectEmotionLevel;
            if (IsEternalityBattle() && EternalityParam.PickedEmotionCard==-5 && skillPoint >= 0)
            {
                EternalityParam.PickedEmotionCard = 2 * (skillPoint+1);
            }
            if (EternalityParam.PickedEmotionCard > 0)
            {
                EternalityParam.PickedEmotionCard--;
                __instance.team.currentSelectEmotionLevel --;
            }
        }
        public static bool IsEternalityBattle()
        {
            return BattleObjectManager.instance.GetAliveList(Faction.Enemy).Exists(x => x.passiveDetail.HasPassive<PassiveAbility_226769004>());
        }
        //修复皮肤切换bug
        [HarmonyPatch(typeof(BattleUnitView), nameof(BattleUnitView.ChangeSkin))]
        [HarmonyPrefix]
        public static bool BattleUnitView_ChangeSkin_Pre(string charName, BattleUnitModel ___model)
        {
            bool result;
            if (charName == "Reisen2" || charName == "Reisen")
            {
                ___model.view._skinInfo.state= BattleUnitView.SkinState.Changed;
                ___model.view._skinInfo.skinName = charName;
                ActionDetail currentMotionDetail = ___model.view.charAppearance.GetCurrentMotionDetail();
                ___model.view.DestroySkin();
                string resourceName;
                GameObject gameObject = Singleton<AssetBundleManagerRemake>.Instance.LoadCharacterPrefab(charName, "", out resourceName);
                if (gameObject != null)
                {
                    UnitCustomizingData customizeData = ___model.UnitData.unitData.customizeData;
                    GiftInventory giftInventory = ___model.UnitData.unitData.giftInventory;
                    GameObject gameObject2 = UnityObject.Instantiate<GameObject>(gameObject, ___model.view.characterRotationCenter);
                    WorkshopSkinData workshopBookSkinData = Singleton<CustomizingBookSkinLoader>.Instance.GetWorkshopBookSkinData(EternalityInitializer.packageId, charName);
                    gameObject2.GetComponent<WorkshopSkinDataSetter>().SetData(workshopBookSkinData);
                    ___model.view.charAppearance = gameObject2.GetComponent<CharacterAppearance>();
                    ___model.view.charAppearance.Initialize(resourceName);
                    ___model.view.charAppearance.InitCustomData(customizeData, ___model.UnitData.unitData.defaultBook.GetBookClassInfoId());
                    ___model.view.charAppearance.InitGiftDataAll(giftInventory.GetEquippedList());
                    ___model.view.charAppearance.ChangeMotion(currentMotionDetail);
                    ___model.view.charAppearance.ChangeLayer("Character");
                    ___model.view.charAppearance.SetLibrarianOnlySprites(___model.faction);
                    if (customizeData != null)
                    {
                        ___model.view.ChangeHeight(customizeData.height);
                    }
                }
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
        //修复观测被动而导致的UI问题
        [HarmonyPatch(typeof(LevelUpUI),nameof(LevelUpUI.InitBase))]
        [HarmonyPrefix]
        public static void LevelUpUI_InitBase(LevelUpUI __instance, ref int selectedCount)
        {
            if (selectedCount > __instance._emotionLevels.Length - 1)
                selectedCount = __instance._emotionLevels.Length - 1;
        }
        //Buf额外扳机，给InabaBufAbility在战斗开始时触发OnStartBattle
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnStartBattle))]
        [HarmonyPostfix]
        public static void BattleUnitModel_OnStartBattle_Post(BattleUnitModel __instance)
        {
            foreach (BattleUnitBuf battleUnitBuf in __instance.bufListDetail.GetActivatedBufList())
                if (battleUnitBuf != null && battleUnitBuf is InabaBufAbility)
                    ((InabaBufAbility)battleUnitBuf).OnStartBattle();
        }
        //设定Malkuth，Yesod，Hod...的外观，让他们穿上对应的衣服
        [HarmonyPatch(typeof(UnitDataModel),nameof(UnitDataModel.Init))]
        [HarmonyPostfix]
        public static void UnitDataModel_Init_Post(UnitDataModel __instance, LorId defaultBook)
        {
            if (defaultBook.packageId == packageId) 
            {
                switch(defaultBook.id) 
                {
                    case 226769011:
                        __instance._defaultBook=new BookModel(BookXmlList.Instance.GetData(1));
                        __instance._customizeData = new UnitCustomizingData(new LorId(1));
                        break;
                    case 226769016:
                        __instance._defaultBook = new BookModel(BookXmlList.Instance.GetData(2));
                        __instance._customizeData = new UnitCustomizingData(new LorId(2));
                        break;
                    case 226769021:
                        __instance._defaultBook = new BookModel(BookXmlList.Instance.GetData(3));
                        __instance._customizeData = new UnitCustomizingData(new LorId(3));
                        break;
                    case 226769026:
                        __instance._defaultBook = new BookModel(BookXmlList.Instance.GetData(4));
                        __instance._customizeData = new UnitCustomizingData(new LorId(4));
                        break;
                    case 226769031:
                        __instance._defaultBook = new BookModel(BookXmlList.Instance.GetData(5));
                        __instance._customizeData = new UnitCustomizingData(new LorId(5));
                        break;
                    case 226769036:
                        __instance._defaultBook = new BookModel(BookXmlList.Instance.GetData(6));
                        __instance._customizeData = new UnitCustomizingData(new LorId(6));
                        break;
                    case 226769041:
                        __instance._defaultBook = new BookModel(BookXmlList.Instance.GetData(7));
                        __instance._customizeData = new UnitCustomizingData(new LorId(7));
                        break;
                    case 226769046:
                        __instance._defaultBook = new BookModel(BookXmlList.Instance.GetData(8));
                        __instance._customizeData = new UnitCustomizingData(new LorId(8));
                        break;
                    case 226769051:
                        __instance._defaultBook = new BookModel(BookXmlList.Instance.GetData(9));
                        __instance._customizeData = new UnitCustomizingData(new LorId(9));
                        break;
                    case 226769056:
                        __instance._defaultBook = new BookModel(BookXmlList.Instance.GetData(10));
                        __instance._customizeData = new UnitCustomizingData(new LorId(10));
                        break;
                    default:
                        break;
                }
            }
        }
        //开始接待时重设跨幕的参数
        [HarmonyPatch(typeof(StageController), nameof(StageController.InitCommon))]
        [HarmonyPostfix]
        public static void StageController_InitCommon(StageController __instance, StageClassInfo stage)
        {
            EternalityParam.Librarian.Reset();
            EternalityParam.Enemy.Reset();
            EternalityParam.PickedEmotionCard = 0;
            if(stage.id==new LorId(packageId, 226769001))
            {
                Singleton<StageController>.Instance.GetStageModel().ClassInfo.floorNum = 1;
                Singleton<StageController>.Instance.GetStageModel().ClassInfo.floorOnlyList = new List<SephirahType> { SephirahType.Malkuth };
            }
        }
        //每一幕结束时记录下双方阵营的时辰狂气(以及未来谜题的进度)
        [HarmonyPatch(typeof(StageController),nameof(StageController.ClearResources))]
        [HarmonyPrefix]
        public static void StageController_ClearResources()
        {
            EternalityParam.Librarian.EndBattleRecord();
            EternalityParam.Enemy.EndBattleRecord();
        }
        [HarmonyPatch(typeof(BattlePlayingCardSlotDetail),nameof(BattlePlayingCardSlotDetail.AddCard))]
        [HarmonyPrefix]
        public static void BattlePlayingCardSlotDetail_AddCard_Pre(BattlePlayingCardSlotDetail __instance,
            BattleDiceCardModel card,ref BattleUnitModel target,ref int targetSlot, bool isEnemyAuto = false)
        {
            BattleUnitModel owner = __instance._self;
            if(card!=null && BattleUnitBuf_InabaBuf2.isEnemy(owner) && owner.faction==Faction.Player
                && BattleUnitBuf_InabaBuf2.CheckFrenzy(owner,owner.cardOrder))
            {
                if (card.GetID() == GetLorId(226769035) || card.GetID() == GetLorId(226769036)
                    || card.XmlData.Spec.Ranged == CardRange.FarArea || card.XmlData.Spec.Ranged == CardRange.FarAreaEach)
                    return;
                if (BattleUnitBuf_InabaBuf2.frenzyTargets.ContainsKey(card))
                {
                    target=BattleUnitBuf_InabaBuf2.frenzyTargets[card].target;
                    targetSlot = BattleUnitBuf_InabaBuf2.frenzyTargets[card].targetSlot;
                }
                else
                {
                    target = BattleUnitBuf_InabaBuf2.GetTarget_enemy(owner);
                    targetSlot = UnityEngine.Random.Range(0, target.speedDiceResult.Count);
                    BattleUnitBuf_InabaBuf2.frenzyTargets.Add(card, new BattleUnitBuf_InabaBuf2.targetSetter()
                        { target = target,targetSlot=targetSlot });
                }
                if (target.cardSlotDetail.cardAry[targetSlot] != null)
                {
                    target.cardSlotDetail.cardAry[targetSlot].target = owner;
                    target.cardSlotDetail.cardAry[targetSlot].targetSlotOrder = owner.cardOrder;
                }
            }
        }
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.CanChangeAttackTarget))]
        [HarmonyPostfix]
        public static void CanChangeAttackTarget(BattleUnitModel __instance, ref bool __result, int myIndex = 0)
        {
            if (__result == false && BattleUnitBuf_InabaBuf2.CheckFrenzy(__instance, myIndex) && !BattleUnitBuf_InabaBuf2.isEnemy(__instance))
            {
                __result = true;
            }
        }
        //第二战更改UI的方向
        [HarmonyPatch(typeof(StageController),nameof(StageController.IsKeterFinalBattle))]//修改这个方式好在_allyFormationDirection被赋值后又在其被其他方法引用前修改其数值
        [HarmonyPostfix]
        public static void StageController_ChangeAllyForamtionDirection(StageController __instance)
        {
            if (__instance.GetStageModel().ClassInfo.id == new LorId(packageId, 226769001))
                __instance._allyFormationDirection = Direction.LEFT;
        }
        //第二战设置友方司书
        [HarmonyPatch(typeof(StageModel),nameof(StageModel.Init))]
        [HarmonyPrefix]
        public static void StageModel_Init(StageModel __instance, StageClassInfo classInfo)
        {
            if(classInfo.id==new LorId(packageId, 226769001))
            {
                __instance._classInfo = classInfo;
                SecondBattleLibrarians.Clear();
                SecondBattleLibrarians.AddRange(GetCustomLibrarianUnit(__instance, new List<int> { 226769103, 226769104, 226769105 }));
            }
        }
        [HarmonyPatch(typeof(StageLibraryFloorModel), nameof(StageLibraryFloorModel.InitUnitList))]
        [HarmonyPrefix]
        public static bool StageLibraryFloorModel_InitUnitList(StageLibraryFloorModel __instance, StageModel stage, LibraryFloorModel floor)
        {
            bool result;
            if (stage.ClassInfo.id.packageId != packageId || stage.ClassInfo.id.id != 226769001/* || Singleton<StageController>.Instance.CurrentWave != 1 */)
                result = true;
            else
            {
                __instance._unitList.AddRange(SecondBattleLibrarians);
                result = false;
            }
            return result;
        }
        public static List<UnitBattleDataModel> GetCustomLibrarianUnit(StageModel stage,  List<int> battleUnits)
        {
            List<UnitBattleDataModel> list = new List<UnitBattleDataModel>();
            foreach (int equipID in battleUnits)
            {
                LorId lorId = new LorId(packageId, equipID);
                UnitDataModel unitDataModel = new UnitDataModel(lorId, SephirahType.Malkuth, true);
                unitDataModel.SetTemporaryPlayerUnitByBook(lorId);
                unitDataModel.SetCustomName(unitDataModel.bookItem.Name);
                unitDataModel.CreateDeckByDeckInfo();
                unitDataModel.forceItemChangeLock = true;
                UnitBattleDataModel unitBattleDataModel = new UnitBattleDataModel(stage, unitDataModel);
                unitBattleDataModel.Init();
                list.Add(unitBattleDataModel);
            }
            return list;
        }
        //设置专属书页
        [HarmonyPatch(typeof(BookModel), nameof(BookModel.SetXmlInfo))]
        [HarmonyPostfix]
        public static void BookModel_SetXmlInfo_Post(BookModel __instance, BookXmlInfo ____classInfo, ref List<DiceCardXmlInfo> ____onlyCards)
        {
            if (__instance.BookId.packageId == packageId)
            {
                foreach (int id in ____classInfo.EquipEffect.OnlyCard)
                {
                    DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(packageId, id), false);
                    ____onlyCards.Add(cardItem);
                }
            }
        }
        //第二战禁用异想体
        [HarmonyPatch(typeof(StageLibraryFloorModel), nameof(StageLibraryFloorModel.HasSkillPoint))]
        [HarmonyPostfix]
        public static void StageLibraryFloorModel_HasSkillPoint(ref bool __result)
        {
            if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id == new LorId(packageId, 226769001))
                __result = false;
        }
        //绀珠之药描述
        [HarmonyPatch(typeof(BattleCardAbilityDescXmlList),nameof(BattleCardAbilityDescXmlList.GetAbilityDesc),typeof(string))]
        [HarmonyPostfix]
        public static void BattleCardAbilityDescXmlList_GetAbilityDesc(string id, List<string> __result)
        {
            if(id== "ETpassfloor")
            {
                string desc = __result[0];
                SaveData saveData = Singleton<EternalityTempleSaveManager>.Instance.LoadData("passFloor");
                if (saveData == null)
                {
                    __result[0] = string.Format(desc, "N/A", "N/A", "N/A");
                    return;
                }
                int hp1 = saveData.GetInt(Singleton<StageController>.Instance.CurrentFloor.ToString() + "Kaguya");
                int hp2 = saveData.GetInt(Singleton<StageController>.Instance.CurrentFloor.ToString() + "Yagokoro");
                int hp3 = saveData.GetInt(Singleton<StageController>.Instance.CurrentFloor.ToString() + "Inaba");
                __result[0]=string.Format(desc, hp1, hp2, hp3);
            }
        }

        [HarmonyPatch]
        public class BaseModPatch  //Copied from UnitRenderUtil.dll by Cyaminthe from Basemod
        {
            [HarmonyPatch(typeof(UIBattleSettingWaveList), "SetData")]
            [HarmonyPrefix]
            private static void UIBattleSettingWaveList_SetData_Prefix(UIBattleSettingWaveList __instance, StageModel stage)
            {
                try
                {
                    ScrollRect scrollRect = __instance.transform.parent.GetComponent<ScrollRect>();
                    if (!(bool)(UnityEngine.Object)scrollRect)
                    {
                        RectTransform transform = __instance.gameObject.transform as RectTransform;
                        GameObject gameObject = new GameObject("[Rect]WaveListView");
                        RectTransform parent = gameObject.AddComponent<RectTransform>();
                        parent.SetParent(transform.parent);
                        parent.localPosition = new Vector3(0.0f, -35f, 0.0f);
                        parent.localEulerAngles = Vector3.zero;
                        parent.localScale = Vector3.one;
                        parent.sizeDelta = Vector2.one * 820f;
                        gameObject.AddComponent<RectMask2D>();
                        transform.SetParent((Transform)parent, true);
                        scrollRect = gameObject.AddComponent<ScrollRect>();
                        scrollRect.content = transform;
                        __instance.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                    }
                    scrollRect.scrollSensitivity = 15f;
                    scrollRect.horizontal = false;
                    scrollRect.vertical = true;
                    scrollRect.movementType = ScrollRect.MovementType.Elastic;
                    scrollRect.elasticity = 0.1f;
                    RectTransform transform1 = __instance.transform as RectTransform;
                    transform1.pivot = stage.waveList.Count <= 5 ? new Vector2(transform1.pivot.x, 0.5f) : new Vector2(transform1.pivot.x, 0.0f);
                    if (stage.waveList.Count > __instance.waveSlots.Count)
                    {
                        List<UIBattleSettingWaveSlot> collection = new List<UIBattleSettingWaveSlot>(stage.waveList.Count - __instance.waveSlots.Count);
                        for (int count = __instance.waveSlots.Count; count < stage.waveList.Count; ++count)
                        {
                            UIBattleSettingWaveSlot battleSettingWaveSlot = UnityEngine.Object.Instantiate<UIBattleSettingWaveSlot>(__instance.waveSlots[0], __instance.waveSlots[0].transform.parent);
                            battleSettingWaveSlot.name = string.Format("[Rect]WaveSlot ({0})", (object)count);
                            collection.Add(battleSettingWaveSlot);
                        }
                        collection.Reverse();
                        __instance.waveSlots.InsertRange(0, (IEnumerable<UIBattleSettingWaveSlot>)collection);
                    }
                    for (int index = 0; index < __instance.waveSlots.Count; ++index)
                        __instance.waveSlots[index].gameObject.transform.localScale = Vector3.one;
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
    }
}

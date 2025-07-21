using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using Mod;
using UI;
using TMPro;
using LOR_BattleUnit_UI;
using LOR_DiceSystem;
using Diagonis = System.Diagnostics;
using UnityObject = UnityEngine.Object;
using EternalityTemple.Kaguya;
using System.Linq;
using EternalityTemple.Yagokoro;
using EternalityTemple.Inaba;
using GameSave;
using Battle.DiceAttackEffect;
using UnityEngine.UI;
using Workshop;
using FileInfo = System.IO.FileInfo;

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
            EternalityInitializer.assetBundle = new Dictionary<string, AssetBundle>();
            EternalityInitializer.CustomEffects = new Dictionary<string, Type>();
            EternalityInitializer.AddAssets();
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
                harmony.PatchAll(typeof(EternalityInitializer.BaseModPatch));
        }
        //添加特效AB包
        public static void AddAssets()
        {
            EternalityInitializer.AddAsset("Eternality", EternalityInitializer.ModPath + "/AB/EternalityFX.ab");
        }
        public static void AddAsset(string name, string path)
        {
            AssetBundle value = AssetBundle.LoadFromFile(path);
            EternalityInitializer.assetBundle.Add(name, value);
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
        //速度骰染色： 涉及此功能的有 谜题骰，疯狂骰和月相骰
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.RollSpeedDice))]
        [HarmonyPostfix]
        public static void BattleUnitModel_RollSpeedDice_Post(BattleUnitModel __instance)
        {
            if (__instance.IsBreakLifeZero() || __instance.IsKnockout() || __instance.turnState == BattleUnitTurnState.BREAK || __instance.bufListDetail.HasStun())
                return;
            SpeedDiceSetter SDS = __instance.view.speedDiceSetterUI;
            if (__instance.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_PuzzleBuf) is BattleUnitBuf_PuzzleBuf puzzlebuf)
            {
                int unavailable = __instance.speedDiceResult.FindAll(x => x.breaked).Count;
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
                if (passive.IsActivate)
                {
                    int i = 1;
                    foreach (SpeedDiceUI speedDiceUI in __instance.view.speedDiceSetterUI._speedDices)
                    {
                        if (Singleton<StageController>.Instance.RoundTurn % 2 == 1) ChangeSpeedDiceColor(speedDiceUI, Color.cyan, "ICON_Eirin月相" + i, "ICON_EirinGlow", "ICON_EirinHovered");
                        else ChangeSpeedDiceColor(speedDiceUI, Color.cyan, "ICON_Eirin月相" + (6-i), "ICON_EirinGlow", "ICON_EirinHovered");
                        i++;
                    }
                }   
            }
            if (BattleUnitBuf_InabaBuf2.GetStack(__instance) > 0)
            {
                for (int i = 0; i < BattleUnitBuf_InabaBuf2.GetStack(__instance); i++)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(i), Color.red, "ICON_Reisen", "ICON_ReisenGlow", "ICON_ReisenHovered");
                if (BattleUnitBuf_InabaBuf3.GetStack(__instance) > 0)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(BattleUnitBuf_InabaBuf3.GetStack(__instance) - 1), Color.red, "ICON_Reisen", "ICON_ReisenGlow", "ICON_ReisenHovered");
            }
        }
        private static Sprite defaultSpeedDice1;
        private static Sprite defaultSpeedDice2;
        private static Sprite defaultSpeedDice3;
        private static void ChangeSpeedDiceColor(SpeedDiceUI ui, Color DiceColor, string normal, string glow, string hovered, bool reset = false) //DiceColor不能是白色，因为白色只会让数字变成白色，骰子框仍为原来颜色
        {
            if (defaultSpeedDice1 == null)
            {
                defaultSpeedDice1 = ui.img_normalFrame.sprite;
                defaultSpeedDice2 = ui.img_lightFrame.sprite;
                defaultSpeedDice3 = ui.img_highlightFrame.sprite;
            }
            if (ui == null)
                return;
            if (!reset)
                ChangedSpeedDiceUI.Add(ui, ui.img_normalFrame.color);
            if (normal != "")
            {
                ui.img_normalFrame.sprite = EternalityInitializer.ArtWorks[normal];
                ui.img_lightFrame.sprite = EternalityInitializer.ArtWorks[glow];
                ui.img_highlightFrame.sprite = EternalityInitializer.ArtWorks[hovered];
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
            if (charName == "Reisen2")
            {
                ((BattleUnitView.SkinInfo)___model.view.GetType().GetField("_skinInfo", AccessTools.all).GetValue(___model.view)).state = BattleUnitView.SkinState.Changed;
                ((BattleUnitView.SkinInfo)___model.view.GetType().GetField("_skinInfo", AccessTools.all).GetValue(___model.view)).skinName = charName;
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
        //修复readyBuf不显示图标
        [HarmonyPatch(typeof(BattleUnitBufListDetail), "OnRoundStart")]
        [HarmonyPrefix]
        private static bool BattleUnitBufListDetail_OnRoundStart_Pre(BattleUnitBufListDetail __instance, BattleUnitModel ____self, List<BattleUnitBuf> ____bufList, List<BattleUnitBuf> ____readyBufList, List<BattleUnitBuf> ____readyReadyBufList)
        {
            try
            {
                using (List<BattleUnitBuf>.Enumerator enumerator = ____readyBufList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        BattleUnitBuf ReadyBuf = enumerator.Current;
                        bool flag = !ReadyBuf.IsDestroyed();
                        if (flag)
                        {
                            BattleUnitBuf battleUnitBuf = ____bufList.Find((BattleUnitBuf x) => x.GetType() == ReadyBuf.GetType() && !x.IsDestroyed());
                            bool flag2 = battleUnitBuf != null && !ReadyBuf.independentBufIcon && battleUnitBuf.GetBufIcon() != null;
                            if (flag2)
                            {
                                battleUnitBuf.stack += ReadyBuf.stack;
                                battleUnitBuf.OnAddBuf(ReadyBuf.stack);
                            }
                            else
                            {
                                __instance.AddBuf(ReadyBuf);
                                ReadyBuf.OnAddBuf(ReadyBuf.stack);
                            }
                        }
                    }
                }
                ____readyBufList.Clear();
                using (List<BattleUnitBuf>.Enumerator enumerator2 = ____readyReadyBufList.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        BattleUnitBuf ReadyReadyBuf = enumerator2.Current;
                        bool flag3 = !ReadyReadyBuf.IsDestroyed();
                        if (flag3)
                        {
                            BattleUnitBuf battleUnitBuf2 = ____readyBufList.Find((BattleUnitBuf x) => x.GetType() == ReadyReadyBuf.GetType() && !x.IsDestroyed());
                            bool flag4 = battleUnitBuf2 != null && !ReadyReadyBuf.independentBufIcon && battleUnitBuf2.GetBufIcon() != null;
                            if (flag4)
                            {
                                battleUnitBuf2.stack += ReadyReadyBuf.stack;
                                battleUnitBuf2.OnAddBuf(ReadyReadyBuf.stack);
                            }
                            else
                            {
                                ____readyBufList.Add(ReadyReadyBuf);
                                ReadyReadyBuf.OnAddBuf(ReadyReadyBuf.stack);
                            }
                        }
                    }
                }
                ____readyReadyBufList.Clear();
                bool flag5 = ____self.faction == Faction.Player && Singleton<StageController>.Instance.GetStageModel().ClassInfo.chapter == 3;
                if (flag5)
                {
                    int kewordBufStack = __instance.GetKewordBufStack(KeywordBuf.Endurance);
                    ____self.UnitData.historyInStage.maxEndurance = Mathf.Max(____self.UnitData.historyInStage.maxEndurance, kewordBufStack);
                }
                foreach (BattleUnitBuf battleUnitBuf3 in ____bufList.ToArray())
                {
                    try
                    {
                        bool flag6 = !battleUnitBuf3.IsDestroyed();
                        if (flag6)
                        {
                            battleUnitBuf3.OnRoundStart();
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.LogException(exception);
                    }
                }
                __instance.CheckDestroyedBuf();
                __instance.CheckAchievements();
                return false;
            }
            catch (Exception ex)
            {
                File.WriteAllText(Application.dataPath + "/Mods/ReadyBufFixerror.log", ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return true;
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
        [HarmonyPatch(typeof(StageController), nameof(StageController.ActivateStartBattleEffectPhase))]
        [HarmonyPostfix]
        public static void ActivateStartBattleEffectPhase_Post()
        {
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(false))
                foreach (BattleUnitBuf battleUnitBuf in battleUnitModel.bufListDetail.GetActivatedBufList())
                    if (battleUnitBuf != null && battleUnitBuf is InabaBufAbility)
                        ((InabaBufAbility)battleUnitBuf).OnStartBattle();
        }
        //阻止疯狂速度骰切换目标骰的攻击敌人 好像没生效
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.CanChangeAttackTarget))]
        [HarmonyPrefix]
        public static bool CanChangeAttackTarget(BattleUnitModel __instance, ref bool __result, int myIndex = 0)
        {
            if (myIndex < BattleUnitBuf_InabaBuf2.GetStack(__instance))
            {
                __result = false;
                return false;
            }
            return true;
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
            EternalityParam.EgoCoolDown = false;
        }
        //每一幕结束时记录下双方阵营的时辰狂气(以及未来谜题的进度)
        [HarmonyPatch(typeof(StageController),nameof(StageController.ClearResources))]
        [HarmonyPrefix]
        public static void StageController_ClearResources()
        {
            EternalityParam.Librarian.EndBattleRecord();
            EternalityParam.Enemy.EndBattleRecord();
        }
        //禁用mod的核心书页检查xml里重复的被动
        [HarmonyPatch(typeof(BookModel),nameof(BookModel.TryGainUniquePassive))]
        [HarmonyPrefix]
        public static bool BookModel_TryGainUniquePassive(BookModel __instance)
        {
            if (__instance.ClassInfo.id.packageId != packageId)
                return true;
            List<PassiveModel> passiveList = new List<PassiveModel>();
            //passiveList.AddRange(__instance._activatedAllPassives.FindAll(x => x.originpassive != null));
            foreach (LorId lorId in __instance._classInfo.EquipEffect.PassiveList.FindAll(x => PassiveXmlList.Instance.GetData(x) != null))
            {
                PassiveModel passiveModel = new PassiveModel(lorId, __instance.instanceId);
                int index = passiveList.FindIndex(x => x.originpassive.id == 9999999);
                if (index > 0)
                    passiveList.Insert(index, passiveModel);
                else
                    passiveList.Add(passiveModel);
            }
            List<PassiveModel> removal = new List<PassiveModel>();
            foreach (PassiveModel passiveModel in passiveList)
            {
                PassiveModel pmodel = passiveModel;
                if (pmodel.originpassive == null)
                    removal.Add(pmodel);
                else if (__instance._classInfo.EquipEffect.PassiveList.Find(x => x == pmodel.originpassive.id) == null && !(pmodel.originpassive.id == 9999999))
                    removal.Add(pmodel);
            }
            foreach (PassiveModel passiveModel in removal)
                passiveList.Remove(passiveModel);
            if (passiveList.Count < __instance.ClassInfo.SuccessionPossibleNumber)
            {
                int num = __instance.ClassInfo.SuccessionPossibleNumber - passiveList.Count;
                for (int index = 0; index < num; ++index)
                {
                    PassiveModel passiveModel = new PassiveModel(LorId.None, __instance.instanceId, 1);
                    passiveList.Add(passiveModel);
                }
            }
            __instance.SortPassive(passiveList);
            if (passiveList.Count > __instance.ClassInfo.SuccessionPossibleNumber)
            {
                while (passiveList.Count > __instance.ClassInfo.SuccessionPossibleNumber)
                {
                    int lastIndex = passiveList.FindLastIndex(x => x.originpassive.id == 9999999);
                    if (lastIndex != -1)
                        passiveList.RemoveAt(lastIndex);
                    else
                        break;
                }
            }
            __instance._activatedAllPassives.Clear();
            __instance._activatedAllPassives.AddRange(passiveList);
            return false;
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
        //获取第二战敌人的列队参数没有用处，所以已经弃用，包括FormationInfo.txt
        /*[HarmonyPatch(typeof(LibraryFloorModel), nameof(LibraryFloorModel.GetFormationPosition))]
        [HarmonyPostfix]
        public static void LibraryFloorModel_GetFormationPosition_Post(int i, ref FormationPosition __result)
        {
            if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id == new LorId(packageId, 226769001))
            {
                FormationModel formationModel = new FormationModel(Singleton<FormationXmlList>.Instance.GetData(226768));
                __result = formationModel.PostionList[i];
            }
        }*/
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
        //第二战获取异想体？这个根本没有用处，上一个Patch你早就禁用了玩家司书的异想体了
        [HarmonyPatch(typeof(EmotionCardXmlList), nameof(EmotionCardXmlList.GetDataList), new Type[]{
                    typeof(SephirahType),
                    typeof(int),
                    typeof(int)})]
        [HarmonyPrefix]
        public static bool EmotionCardXmlList_GetDataList(List<EmotionCardXmlInfo> ____list, ref List<EmotionCardXmlInfo> __result)
        {
            if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != new LorId(packageId, 226769001))
                return true;
            __result = (from x in ____list
                        where x.Sephirah > SephirahType.None
                        select x).ToList();
            return false;
        }
        //第二战设定敌人的EGO书页
        [HarmonyPatch(typeof(StageController), nameof(StageController.ApplyEnemyCardPhase))]
        [HarmonyPostfix]
        public static void StageController_ApplyEnemyCardPhase()
        {
            if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != new LorId(packageId, 226769001))
                return;
            if (StageController.Instance.CurrentFloor == SephirahType.Keter) return;
            if(EternalityParam.EgoCoolDown)
            {
                EternalityParam.EgoCoolDown = false;
                return;
            }
            List<BattleUnitModel> list = Singleton<StageController>.Instance.GetActionableEnemyList().FindAll((BattleUnitModel x) => x.emotionDetail.EmotionLevel >= 3 && x.turnState != BattleUnitTurnState.BREAK);
            if (list.Count == 0)
                return;
            BattleUnitModel egoTarget = RandomUtil.SelectOne(list);
            List<BattleUnitModel> source = BattleObjectManager.instance.GetAliveList(Faction.Player).FindAll(x => x.IsTargetable(egoTarget));
            if (source.Count() <= 0)
                return;
            int cardOrder = egoTarget.cardOrder;
            int num = -1;
            for (int i = 0; i < egoTarget.cardSlotDetail.cardAry.Count; i++)
            {
                if (egoTarget.cardSlotDetail.cardAry[i] == null && !egoTarget.speedDiceResult[i].breaked)
                {
                    num = i;
                    break;
                }
            }
            if (num == -1)
                num = egoTarget.speedDiceResult.FindLastIndex((SpeedDice x) => !x.breaked);
            if (num == -1)
                return;
            List<EmotionEgoXmlInfo> list2 = Singleton<EmotionEgoXmlList>.Instance.GetDataList(StageController.Instance.CurrentFloor);
            if (list2 == null || list2.Count == 0)
                return;
            List<DiceCardXmlInfo> list3 = (from x in list2
                                           where x.CardId != 910015 && x.CardId != 910031 && x.CardId != 910050
                                           select ItemXmlDataList.instance.GetCardItem(x.CardId, false)).ToList();
            list3.RemoveAll(x => x.Spec.Ranged == CardRange.Instance);
            DiceCardXmlInfo cardInfo = RandomUtil.SelectOne(list3);
            egoTarget.SetCurrentOrder(num);
            BattleDiceCardModel battleDiceCardModel = BattleDiceCardModel.CreatePlayingCard(cardInfo);
            battleDiceCardModel.owner = egoTarget;
            battleDiceCardModel.SetCostToZero();
            EternalityParam.EgoCoolDown = true;
            egoTarget.cardSlotDetail.AddCard(null, null, 0, false);
            egoTarget.cardSlotDetail.AddCard(battleDiceCardModel, RandomUtil.SelectOne(source), 0, false);
            try
            {
                SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.ClearCloneArrows();
                SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.ActiveTargetParent(true);
                if (SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.autoCardButton != null)
                    SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.autoCardButton.SetActivate(true);
                if (SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.unequipcardallButton != null)
                    SingletonBehavior<BattleManagerUI>.Instance.ui_emotionInfoBar.unequipcardallButton.SetActivate(true);
                SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
            }
            catch (Exception)
            {
            }
        }
        [HarmonyPatch]
        public class BaseModPatch
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

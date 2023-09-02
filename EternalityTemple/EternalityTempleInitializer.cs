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
using EternalityTemple.Universal;
using EternalityTemple.Kaguya;
using EternalityTemple.Util;
using System.Linq;
using EternalityTemple.Yagokoro;
using EternalityTemple.Inaba;

namespace EternalityTemple
{
    [HarmonyPatch]
    public class EternalityInitializer : ModInitializer
    {
        public static string ModPath;
        public static Dictionary<string, Sprite> ArtWorks;
        
        public const string packageId = "TheWorld_Eternity";
        private static Dictionary<SpeedDiceUI, Color> ChangedSpeedDiceUI = new Dictionary<SpeedDiceUI, Color>();
        
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
            RemoveError();
            LocalizeManager.LocalizedTextLoader_LoadOthers_Post(TextDataModel.CurrentLanguage);
            //InitPublicValue();
        }
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
        [HarmonyPatch(typeof(BattleUnitModel), nameof(BattleUnitModel.OnRecoverHp))]
        [HarmonyPostfix]
        public static void BattleUnitModel_OnRecoverHp_Post(BattleUnitModel __instance, int recoverAmount)
        {
            List<BattleUnitBuf> BufInterface = __instance.bufListDetail.GetActivatedBufList().FindAll(x => x is OnRecoverHP);
            foreach (BattleUnitBuf BufAbility in BufInterface)
                (BufAbility as OnRecoverHP).OnHeal(recoverAmount);
        }
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
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable + 0), Color.blue);
                if (puzzlebuf.CompletePuzzle.Contains(2) && __instance.speedDiceCount - unavailable >= 2)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable + 1), Color.magenta);
                if (puzzlebuf.CompletePuzzle.Contains(3) && __instance.speedDiceCount - unavailable >= 3)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable + 2), Color.red);
                if (puzzlebuf.CompletePuzzle.Contains(4) && __instance.speedDiceCount - unavailable >= 4)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable + 3), Color.green);
                if (puzzlebuf.CompletePuzzle.Contains(5) && __instance.speedDiceCount - unavailable >= 5)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable + 4), Color.yellow);
            }
            PassiveAbility_226769005 passive = __instance.passiveDetail.PassiveList.Find((PassiveAbilityBase x) => x is PassiveAbility_226769005) as PassiveAbility_226769005;
            if (passive != null)
            {
                if (passive.IsActivate)
                    __instance.view.speedDiceSetterUI._speedDices.ForEach(sd => ChangeSpeedDiceColor(sd, Color.cyan));
            }
            if (BattleUnitBuf_InabaBuf2.GetStack(__instance) > 0)
            {
                for (int i = 0; i < BattleUnitBuf_InabaBuf2.GetStack(__instance); i++)
                    ChangeSpeedNumColor(SDS.GetSpeedDiceByIndex(i), Color.red);
                if (BattleUnitBuf_InabaBuf3.GetStack(__instance) > 0)
                    ChangeSpeedNumColor(SDS.GetSpeedDiceByIndex(BattleUnitBuf_InabaBuf3.GetStack(__instance) - 1), Color.red);
            }
        }
        [HarmonyPatch(typeof(SpeedDiceUI), nameof(SpeedDiceUI.SetLightOn))]
        [HarmonyPrefix]
        public static bool SpeedDiceUI_SetLightOn_Pre(bool b)
        {
            if (b==true && StageController.Instance.Phase != StageController.StagePhase.ApplyLibrarianCardPhase)
                return false;
            return true;
        }
        private static void ChangeSpeedDiceColor(SpeedDiceUI ui, Color DiceColor, bool reset = false) //DiceColor不能是白色，因为白色只会让数字变成白色，骰子框仍为原来颜色
        {
            if (ui == null)
                return;
            if (!reset && !ChangedSpeedDiceUI.ContainsKey(ui))
                ChangedSpeedDiceUI.Add(ui, ui.img_normalFrame.color);
            ui._txtSpeedRange.color = DiceColor;
            ui._rouletteImg.color = DiceColor;
            ui._txtSpeedMax.color = DiceColor;
            ui.img_tensNum.color = DiceColor;
            ui.img_unitsNum.color = DiceColor;
            ui.img_normalFrame.color = DiceColor;
        }
        private static void ChangeSpeedNumColor(SpeedDiceUI ui,Color NumColor, bool reset = false)
        {
            if (ui == null)
                return;
            if (!reset && !ChangedSpeedDiceUI.ContainsKey(ui))
                ChangedSpeedDiceUI.Add(ui, ui.img_normalFrame.color);
            ui.img_tensNum.color = NumColor;
            ui.img_unitsNum.color = NumColor;
            ui._txtSpeedMax.color = NumColor;
        }
        public static void ResetSpeedDiceColor()
        {
            foreach (SpeedDiceUI ui in ChangedSpeedDiceUI.Keys)
                ChangeSpeedDiceColor(ui, ChangedSpeedDiceUI[ui], true);
            ChangedSpeedDiceUI.Clear();
        }
        [HarmonyPatch(typeof(BattleAllyCardDetail), nameof(BattleAllyCardDetail.GetHand))]
        [HarmonyPostfix]
        public static void BattleUnitCardsInHandUI_UpdateCardList_BattleAllyCardDetail_GetHand_Post(BattleAllyCardDetail __instance, List<BattleDiceCardModel> __result)
        {
            string name = new Diagonis.StackTrace().GetFrame(2).GetMethod().FullDescription();
            if (!name.Contains("BattleUnitCardsInHandUI::UpdateCardList"))
                return;
            BattleUnitCardsInHandUI CardUI = BattleManagerUI.Instance.ui_unitCardsInHand;
            BattleUnitModel displayed = CardUI.SelectedModel;
            if (CardUI.CurrentHandState != BattleUnitCardsInHandUI.HandState.BattleCard || __instance._self != displayed || !displayed.passiveDetail.HasPassive<PassiveAbility_226769001>())
                return;
            SpeedDiceUI speedDice = BattleManagerUI.Instance.selectedAllyDice;
            if (speedDice == null)
                return;
            if (displayed.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_PuzzleBuf) is BattleUnitBuf_PuzzleBuf puzzlebuf)
            {
                int unavailable = displayed.speedDiceResult.FindAll(x => x.breaked).Count;
                DiceCardXmlInfo xml = null;
                if (puzzlebuf.CompletePuzzle.Contains(1) && speedDice._speedDiceIndex == unavailable + 0)
                    xml = ItemXmlDataList.instance.GetCardItem(new LorId(packageId, 226769006));
                if (puzzlebuf.CompletePuzzle.Contains(2) && speedDice._speedDiceIndex == unavailable + 1)
                    xml = ItemXmlDataList.instance.GetCardItem(new LorId(packageId, 226769007));
                if (puzzlebuf.CompletePuzzle.Contains(3) && speedDice._speedDiceIndex == unavailable + 2)
                    xml = ItemXmlDataList.instance.GetCardItem(new LorId(packageId, 226769008));
                if (puzzlebuf.CompletePuzzle.Contains(4) && speedDice._speedDiceIndex == unavailable + 3)
                    xml = ItemXmlDataList.instance.GetCardItem(new LorId(packageId, 226769009));
                if (puzzlebuf.CompletePuzzle.Contains(5) && speedDice._speedDiceIndex == unavailable + 4)
                    xml = ItemXmlDataList.instance.GetCardItem(new LorId(packageId, 226769010));
                if (xml == null)
                    return;
                BattleDiceCardModel card = BattleDiceCardModel.CreatePlayingCard(xml);
                card.temporary = true;
                card.owner = displayed;
                __result.Add(card);
            }
        }
        [HarmonyPatch(typeof(BattleAllyCardDetail), nameof(BattleAllyCardDetail.ReturnCardToHand))]
        [HarmonyPrefix]
        public static bool BattleAllyCardDetail_ReturnCardToHand_Pre(BattleAllyCardDetail __instance, BattleDiceCardModel appliedCard)
        {
            LorId id = appliedCard.GetID();
            if (id.packageId == packageId && id.id <= 226769010 && id.id >= 226769006)
            {
                __instance._self.cardSlotDetail.ReserveCost(-appliedCard.GetCost());
                __instance._cardInUse.Remove(appliedCard);
                __instance._cardInReserved.Remove(appliedCard);
                return false;
            }
            return true;
        }
        /*[HarmonyPatch(typeof(StageLibraryFloorModel), nameof(StageLibraryFloorModel.CreateSelectableList))]
        [HarmonyPrefix]
        public static bool StageLibraryFloorModel_CreateSelectableList_Pre(StageLibraryFloorModel __instance, ref List<EmotionCardXmlInfo> __result, int emotionLevel)
        {
            if (BattleObjectManager.instance.GetAliveList().FindAll(x => x.passiveDetail.HasPassive<PassiveAbility_226769004>()).Count > 0)
            {
                int PosCoin = 0;
                int NegCoin = 0;
                foreach (UnitBattleDataModel unit in __instance._unitList)
                {
                    if (unit.IsAddedBattle)
                    {
                        PosCoin += unit.emotionDetail.totalPositiveCoins.Count;
                        NegCoin += unit.emotionDetail.totalNegativeCoins.Count;
                    }
                }
                int floorLevel = 0;
                LibraryFloorModel floor = LibraryModel.Instance.GetFloor(Singleton<StageController>.Instance.CurrentFloor);
                if (floor != null)
                    floorLevel = !Singleton<StageController>.Instance.IsRebattle ? floor.Level : floor.TemporaryLevel;
                int EmotionTier = emotionLevel > 2 ? (emotionLevel > 4 ? 3 : 2) : 1;
                List<EmotionCardXmlInfo> dataList = EmotionCardXmlList.Instance.GetDataList(StageController.Instance.CurrentFloor, floorLevel, EmotionTier);
                foreach (EmotionCardXmlInfo selected in __instance._selectedList)
                    dataList.Remove(selected);
                int center = 0;
                int TotalCoin = PosCoin + NegCoin;
                float ratio = 0.5f;
                if (TotalCoin > 0)
                    ratio = (float)(PosCoin - NegCoin) / (float)TotalCoin;
                float f = ratio / (float)((11.0 - (double)emotionLevel) / 10.0);
                center = (double)Mathf.Abs(f) >= 0.1 ? ((double)Mathf.Abs(f) >= 0.3 ? ((double)f <= 0.0 ? -2 : 2) : ((double)f <= 0.0 ? -1 : 1)) : 0;
                dataList.Sort((x, y) => Mathf.Abs(x.EmotionRate - center) - Mathf.Abs(y.EmotionRate - center));
                __result = dataList.Take(5).ToList();
                return false;
            }
            return true;
        }
        [HarmonyPatch(typeof(LevelUpUI),nameof(LevelUpUI.Init))]
        [HarmonyPrefix]
        public static void LevelUpUI_Init_Pre(LevelUpUI __instance, int count, List<EmotionCardXmlInfo> cardList)
        {
            if (cardList.Count > 3)
            {
                LevelUpUIScroller UIScroller= __instance.gameObject.AddComponent<LevelUpUIScroller>();
                UIScroller.Init(__instance, cardList);
            }
        }
        [HarmonyPatch(typeof(LevelUpUI),nameof(LevelUpUI.OnSelectPassive))]
        [HarmonyPostfix]
        public static void LevelUpUI_OnSelectPassive_Post(LevelUpUI __instance)
        {
            LevelUpUIScroller UIScroller = __instance.gameObject.GetComponent<LevelUpUIScroller>();
            if (UIScroller != null)
                UnityObject.Destroy(UIScroller);
        }*/

        [HarmonyPatch(typeof(StageController), nameof(StageController.ActivateStartBattleEffectPhase))]
        [HarmonyPostfix]
        public static void ActivateStartBattleEffectPhase_Post()
        {
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(false))
                foreach (BattleUnitBuf battleUnitBuf in battleUnitModel.bufListDetail.GetActivatedBufList())
                    if (battleUnitBuf != null && battleUnitBuf is InabaBufAbility)
                        ((InabaBufAbility)battleUnitBuf).OnStartBattle();
        }
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
        [HarmonyPatch(typeof(StageController), nameof(StageController.InitCommon))]
        [HarmonyPostfix]
        public static void StageLibraryFloorModel_Init(StageLibraryFloorModel __instance, StageClassInfo stage)
        {
            if (stage.id.packageId == packageId)
            {
                EternalityParam.KaguyaStack = 1;
                EternalityParam.InabaBufGainNum = 0;
                EternalityParam.EgoCoolDown = false;
                EternalityParam.Puzzle.Clear();
            }
        }
        public static UnitBattleDataModel AddCustomFixUnitModel(StageLibraryFloorModel __instance, StageModel stage, LibraryFloorModel floor, int EquipID)
        {
            LorId lorId = new LorId(packageId, EquipID);
            UnitDataModel unitDataModel = new UnitDataModel(lorId, floor.Sephirah, true);
            unitDataModel.SetTemporaryPlayerUnitByBook(lorId);
            unitDataModel.SetCustomName(unitDataModel.bookItem.Name);
            unitDataModel.CreateDeckByDeckInfo();
            unitDataModel.forceItemChangeLock = true;
            UnitBattleDataModel unitBattleDataModel = new UnitBattleDataModel(stage, unitDataModel);
            unitBattleDataModel.Init();
            return unitBattleDataModel;
        }
        public static void UnitModelList(StageLibraryFloorModel __instance, StageModel stage, LibraryFloorModel floor, List<int> battleUnits)
        {
            List<UnitBattleDataModel> list = new List<UnitBattleDataModel>();
            foreach (int equipID in battleUnits)
            {
                list.Add(AddCustomFixUnitModel(__instance, stage, floor, equipID));
            }
            __instance._unitList= list;
        }
        [HarmonyPatch(typeof(StageLibraryFloorModel), nameof(StageLibraryFloorModel.InitUnitList))]
        [HarmonyPrefix]
        public static bool StageLibraryFloorModel_InitUnitList(StageLibraryFloorModel __instance, StageModel stage, LibraryFloorModel floor)
        {
            bool flag = stage.ClassInfo.id.packageId != packageId || stage.ClassInfo.id.id != 226769001/* || Singleton<StageController>.Instance.CurrentWave != 1 */;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                UnitModelList(__instance, stage, floor, new List<int>{226769103,226769104,226769105});
                result = false;
            }
            return result;
        }
        [HarmonyPatch(typeof(LibraryFloorModel), nameof(LibraryFloorModel.GetFormationPosition))]
        [HarmonyPostfix]
        public static void LibraryFloorModel_GetFormationPosition_Post(int i, ref FormationPosition __result)
        {
            if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id == new LorId(packageId, 226769001))
            {
                FormationModel formationModel = new FormationModel(Singleton<FormationXmlList>.Instance.GetData(226768));
                __result = formationModel.PostionList[i];
            }
        }
        [HarmonyPatch(typeof(BookModel), nameof(BookModel.SetXmlInfo))]
        [HarmonyPostfix]
        public static void BookModel_SetXmlInfo_Post(BookModel __instance, global::BookXmlInfo ____classInfo, ref List<DiceCardXmlInfo> ____onlyCards)
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
        [HarmonyPatch(typeof(StageLibraryFloorModel), nameof(StageLibraryFloorModel.HasSkillPoint))]
        [HarmonyPostfix]
        public static void StageLibraryFloorModel_HasSkillPoint(ref bool __result)
        {
            if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id == new LorId(packageId, 226769001))
                __result = false;
        }
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
        [HarmonyPatch(typeof(StageController), nameof(StageController.ApplyEnemyCardPhase))]
        [HarmonyPostfix]
        public static void StageController_ApplyEnemyCardPhase()
        {
            if (Singleton<StageController>.Instance.GetStageModel().ClassInfo.id != new LorId(packageId, 226769001))
                return;
            if(EternalityParam.EgoCoolDown)
            {
                EternalityParam.EgoCoolDown = false;
                return;
            }
            List<BattleUnitModel> list = Singleton<StageController>.Instance.GetActionableEnemyList().FindAll((BattleUnitModel x) => x.emotionDetail.EmotionLevel >= 3 && x.turnState != BattleUnitTurnState.BREAK);
            if (list.Count == 0)
                return;
            BattleUnitModel egoTarget = RandomUtil.SelectOne(list);
            List<BattleUnitModel> source=BattleObjectManager.instance.GetAliveList(Faction.Player).FindAll(x => x.IsTargetable(egoTarget));
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





        /*        [HarmonyPatch(typeof(ItemXmlDataList),nameof(ItemXmlDataList.GetBasicCardList))]
                [HarmonyPostfix]
                public static void LoadBasicCardForTesting(ItemXmlDataList __instance, List<DiceCardXmlInfo> __result)
                {
                    __result.Add(__instance.GetCardItem(new LorId(packageId, 226769006)));
                    __result.Add(__instance.GetCardItem(new LorId(packageId, 226769007)));
                    __result.Add(__instance.GetCardItem(new LorId(packageId, 226769008)));
                    __result.Add(__instance.GetCardItem(new LorId(packageId, 226769009)));
                    __result.Add(__instance.GetCardItem(new LorId(packageId, 226769010)));
                }*/
    }
}

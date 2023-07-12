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

namespace EternalityTemple
{
    [HarmonyPatch]
    public class EternalityInitializer: ModInitializer
    {
        public static string ModPath;
        public static Dictionary<string, Sprite> ArtWorks;
        public const string packageId= "TheWorld_Eternity";
        private static List<(SpeedDiceUI, Color)> ChangedSpeedDiceUI = new List<(SpeedDiceUI, Color)>();
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
        }
        public static void RemoveError()
        {
            List<string> LoadMod = new List<string>() { "0Harmony"};
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
        [HarmonyPatch(typeof(DropBookInventoryModel),nameof(DropBookInventoryModel.GetBookList_invitationBookList))]
        [HarmonyPostfix]
        public static void DropBookInventoryModel_GetBookList_invitationBookList(List<LorId> __result)
        {
            __result.Add(GetLorId(226769000));
        }
        [HarmonyPatch(typeof(BattleUnitBufListDetail),nameof(BattleUnitBufListDetail.CanAddBuf))]
        [HarmonyPostfix]
        public static void BattleUnitBufListDetail_CanAddBuf(BattleUnitBufListDetail __instance,ref bool __result, BattleUnitBuf buf)
        {
            List<PassiveAbilityBase> passiveInterface = __instance._self.passiveDetail.PassiveList.FindAll(x => x is IsBufImmune);
            foreach(PassiveAbilityBase passiveAbility in passiveInterface)
            {
                if ((passiveAbility as IsBufImmune).IsImmune(buf))
                    __result = false;
            }
        }
        [HarmonyPatch(typeof(BattleUnitBufListDetail),nameof(BattleUnitBufListDetail.AddKeywordBufByCard))]
        [HarmonyPostfix]
        public static void BattleUnitBufListDetail_AddKeywordBufByCard_Post(BattleUnitBufListDetail __instance, KeywordBuf bufType,ref int stack, BattleUnitModel actor)
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
        private static void TriggerOnGiveBuff(BattleUnitModel actor,BattleUnitBuf buf,int stack)
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
        [HarmonyPatch(typeof(BattleUnitModel),nameof(BattleUnitModel.OnRecoverHp))]
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
            if (__instance.IsBreakLifeZero() || __instance.IsKnockout() || __instance.turnState==BattleUnitTurnState.BREAK || __instance.bufListDetail.HasStun())
                return;
            if(__instance.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_PuzzleBuf) is BattleUnitBuf_PuzzleBuf puzzlebuf)
            {
                SpeedDiceSetter SDS = __instance.view.speedDiceSetterUI;
                int unavailable = __instance.speedDiceResult.FindAll(x => x.breaked).Count;
                if (puzzlebuf.CompletePuzzle.Contains(1) && __instance.speedDiceCount - unavailable >= 1)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable+0),Color.blue);
                if (puzzlebuf.CompletePuzzle.Contains(2) && __instance.speedDiceCount - unavailable >= 2)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable+1), Color.magenta);
                if (puzzlebuf.CompletePuzzle.Contains(3) && __instance.speedDiceCount - unavailable >= 3)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable+2), Color.red);
                if (puzzlebuf.CompletePuzzle.Contains(4) && __instance.speedDiceCount - unavailable >= 4)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable+3), Color.green);
                if (puzzlebuf.CompletePuzzle.Contains(5) && __instance.speedDiceCount - unavailable >= 5)
                    ChangeSpeedDiceColor(SDS.GetSpeedDiceByIndex(unavailable+4), Color.yellow);
            }
        }
        private static void ChangeSpeedDiceColor(SpeedDiceUI ui, Color DiceColor, bool reset = false)
        {
            if (ui == null)
                return;
            if (!reset)
                ChangedSpeedDiceUI.Add((ui, ui.img_normalFrame.color));
            ui._txtSpeedRange.color = DiceColor;
            ui._rouletteImg.color = DiceColor;
            ui._txtSpeedMax.color = DiceColor;
            ui.img_tensNum.color = DiceColor;
            ui.img_unitsNum.color = DiceColor;
            ui.img_normalFrame.color = DiceColor;
        }
        public static void ResetSpeedDiceColor()
        {
            ChangedSpeedDiceUI.ForEach(x => ChangeSpeedDiceColor(x.Item1, x.Item2, true));
            ChangedSpeedDiceUI.Clear();
        }
        [HarmonyPatch(typeof(BattleAllyCardDetail),nameof(BattleAllyCardDetail.GetHand))]
        [HarmonyPostfix]
        public static void BattleUnitCardsInHandUI_UpdateCardList_BattleAllyCardDetail_GetHand_Post(BattleAllyCardDetail __instance, List<BattleDiceCardModel> __result)
        {
            string name = new Diagonis.StackTrace().GetFrame(2).GetMethod().FullDescription();
            if (!name.Contains("BattleUnitCardsInHandUI::UpdateCardList"))
                return;
            BattleUnitCardsInHandUI CardUI = BattleManagerUI.Instance.ui_unitCardsInHand;
            BattleUnitModel displayed = CardUI.SelectedModel;
            if (CardUI.CurrentHandState != BattleUnitCardsInHandUI.HandState.BattleCard || __instance._self!=displayed || !displayed.passiveDetail.HasPassive<PassiveAbility_226769001>())
                return;
            SpeedDiceUI speedDice = BattleManagerUI.Instance.selectedAllyDice;
            if (speedDice == null)
                return;
            if (displayed.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_PuzzleBuf) is BattleUnitBuf_PuzzleBuf puzzlebuf)
            {
                int unavailable = displayed.speedDiceResult.FindAll(x => x.breaked).Count;
                DiceCardXmlInfo xml=null;
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
                BattleDiceCardModel card=BattleDiceCardModel.CreatePlayingCard(xml);
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
        [HarmonyPatch(typeof(StageLibraryFloorModel), nameof(StageLibraryFloorModel.CreateSelectableList))]
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

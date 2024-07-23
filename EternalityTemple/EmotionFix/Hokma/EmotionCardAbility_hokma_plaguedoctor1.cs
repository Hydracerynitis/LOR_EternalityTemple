using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;
using BaseMod;

namespace EmotionalFix.Hokma
{
    public class EmotionCardAbility_hokma_plaguedoctor1 : EmotionCardAbilityBase
    {
        public static Dictionary<UnitBattleDataModel, int> WhiteNightClock = new Dictionary<UnitBattleDataModel, int>();
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            WhiteNightClock.Add(_owner.UnitData, 0);
            if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is PlagueDoctor) != null)
                return;
            _owner.bufListDetail.AddBuf(new PlagueDoctor());
        }
        public override void OnWaveStart()
        {
            if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is PlagueDoctor) != null)
                return;
            _owner.bufListDetail.AddBuf(new PlagueDoctor());
        }
        public class PlagueDoctor : BattleUnitBuf
        {
            private int bless;
            private List<BattleUnitModel> patient;
            public override int SpeedDiceNumAdder() => bless;
            public override void Init(BattleUnitModel model)
            {
                Init(model);
                patient = new List<BattleUnitModel>();
            }
            public override void OnRoundStart()
            {
                patient.Clear();
                List<BattleUnitModel> alive = BattleObjectManager.instance.GetAliveList(_owner.faction).FindAll(x => x != _owner);
                for (int i = 0; i < 2; i++)
                {
                    if (alive.Count <= 0)
                        break;
                    BattleUnitModel victim = RandomUtil.SelectOne(alive);
                    patient.Add(victim);
                    alive.Remove(victim);
                }
                bless = patient.Count;
            }
            public override void OnDrawCard()
            {
                base.OnDrawCard();
                for (int i = 1; i < bless + 1; i++)
                {
                    DiceCardXmlInfo bless = ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(1108401)).Copy(true);
                    bless.Script = "BlessingPlague";
                    if (Helper.SearchEmotion(_owner, "WhiteNight_Red_Enemy") != null)
                        bless.Script = "BlessingPlagueUpGraded";
                    int num = WhiteNightClock[_owner.UnitData] + i;
                    bless.DiceBehaviourList[0].Dice = num;
                    bless.DiceBehaviourList[0].Min = num;
                    BattleDiceCardModel card = BattleDiceCardModel.CreatePlayingCard(bless);
                    card.temporary = true;
                    _owner.allyCardDetail._cardInHand.Add(card);
                }
            }
            public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
            {
                if (card.GetID() != 1108401 || patient.Count <= 0)
                    return base.ChangeAttackTarget(card, idx);
                BattleUnitModel target = patient[0];
                patient.Remove(target);
                return target;
            }
        }
    }
}

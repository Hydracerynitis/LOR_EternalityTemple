using System;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using System.Collections.Generic;
using EI = EternalityTemple.EternalityInitializer;
using LOR_DiceSystem;

namespace EmotionalFix
{
    public class EmotionCardAbility_malkuth_snowwhite3: EmotionCardAbilityBase
    {
        private Dictionary<BattleUnitModel, int> dmgData = new Dictionary<BattleUnitModel, int>();
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            base.OnTakeDamageByAttack(atkDice, dmg);
            BattleUnitModel owner = atkDice.owner;
            if (owner == null)
                return;
            if (!dmgData.ContainsKey(owner))
                dmgData.Add(owner, dmg);
            else
                dmgData[owner] += dmg;
        }
        public override void OnRoundEnd()
        {
            if (dmgData.Count > 0)
            {
                int num = 0;
                BattleUnitModel battleUnitModel = null;
                foreach (KeyValuePair<BattleUnitModel, int> keyValuePair in dmgData)
                {
                    if (keyValuePair.Value > num && !keyValuePair.Key.IsDead())
                    {
                        num = keyValuePair.Value;
                        battleUnitModel = keyValuePair.Key;
                    }
                }
                if (battleUnitModel.bufListDetail.GetActivatedBufList().Find(x => x is Malice_Enemy) is Malice_Enemy malice)
                    malice.stack += 1;
                else
                    battleUnitModel.bufListDetail.AddBuf(new Malice_Enemy());
            }
            else
            {
                List<BattleUnitModel> enemy = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
                if (enemy.Count == 0)
                    return;
                BattleUnitModel victim=RandomUtil.SelectOne(enemy);
                if (victim.bufListDetail.GetActivatedBufList().Find(x => x is Malice_Enemy) is Malice_Enemy malice)
                    malice.stack += 1;
                else
                    victim.bufListDetail.AddBuf(new Malice_Enemy());
            }
            dmgData.Clear();
            new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/SnowWhite_Filter_Vine", false, 2f);
        }
        public override void OnStartBattle()
        {
            int num = 0;
            List<BattleUnitModel> victim = new List<BattleUnitModel>();
            foreach (BattleUnitModel enemy in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
            {
                if (enemy.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_binding) is BattleUnitBuf_binding bind)
                {
                    num += bind.stack;
                    victim.Add(enemy);
                }
            }
            if (victim.Count == 0)
                return;
            DiceCardXmlInfo xml = ItemXmlDataList.instance.GetCardItem(new LorId(EI.packageId ,1101501)).Copy(true);
            DiceBehaviour dice = xml.DiceBehaviourList[0];
            dice.Dice = dice.Dice += num;
            BattleDiceCardModel Aoe = BattleDiceCardModel.CreatePlayingCard(xml);
            BattleUnitModel target = RandomUtil.SelectOne(victim);
            victim.Remove(target);
            BattlePlayingCardDataInUnitModel Card = new BattlePlayingCardDataInUnitModel
            {
                owner = _owner,
                card = Aoe,
                cardAbility = Aoe.CreateDiceCardSelfAbilityScript(),
                target = target,
                targetSlotOrder = RandomUtil.Range(0, target.cardSlotDetail.cardAry.Count - 1),
                slotOrder = RandomUtil.Range(0, _owner.cardSlotDetail.cardAry.Count - 1)
            };
            foreach (BattleUnitModel battleUnitModel in victim)
            {
                if (battleUnitModel != target && battleUnitModel.IsTargetable(_owner))
                {
                    BattlePlayingCardSlotDetail cardSlotDetail = battleUnitModel.cardSlotDetail;
                    int num1;
                    if (cardSlotDetail == null)
                    {
                        num1 = 0;
                    }
                    else
                    {
                        int? count = cardSlotDetail.cardAry?.Count;
                        int num2 = 0;
                        num1 = count.GetValueOrDefault() > num2 & count.HasValue ? 1 : 0;
                    }
                    if (num1 != 0)
                        Card.subTargets.Add(new BattlePlayingCardDataInUnitModel.SubTarget()
                        {
                            target = battleUnitModel,
                            targetSlotOrder = UnityEngine.Random.Range(0, battleUnitModel.speedDiceResult.Count)
                        });
                }
            }
            Card.ResetExcludedDices();
            Card.ResetCardQueueWithoutStandby();
            StageController.Instance._allCardList.Add(Card);
        }
        public class Malice_Enemy: BattleUnitBuf
        {
            public override string keywordId => "EF_Malice_Eternal";
            public override string keywordIconId => "Snowwhite_Vine";
            public override void OnRoundStart()
            {
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, stack);
            }
        }
    }
}

﻿using EternalityTemple.Inaba;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using EternalityTemple.Yagokoro;
using LOR_BattleUnit_UI;
using HyperCard;
using GameSave;

namespace EternalityTemple.Kaguya
{
    //月之都市 书页
    public class DiceCardSelfAbility_EternityCard1 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            owner.cardSlotDetail.RecoverPlayPointByCard(1);
            if (BattleUnitBuf_KaguyaBuf.GetStack(owner)>=7)
            {
                owner.allyCardDetail.DrawCards(1);
            }
            if (BattleObjectManager.instance.GetAliveList(base.owner.faction).Find((BattleUnitModel x) => x.bufListDetail.HasBuf<BattleUnitBuf_Moon3>()) != null)
            {
                owner.cardSlotDetail.RecoverPlayPointByCard(1);
            }
        }
    }
    //新难题[艾哲红石] 书页
    public class DiceCardSelfAbility_EternityCard2 : DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            BattleUnitBuf buf = owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_KaguyaBuf);
            if (buf != null)
                card.ApplyDiceStatBonus(DiceMatch.AllDice,new DiceStatBonus() { power = -buf.stack });
        }
    }
    //新难题[艾哲红石] 骰子
    public class DiceCardAbility_RedStone : DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            int burnstack = 4;
            BattleUnitBuf buf = owner.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_KaguyaBuf);
            if (buf != null)
                burnstack -= buf.stack / 2;
            target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Burn, burnstack,owner);
        }
    }
    //新难题[Mysterium] 书页
    public class DiceCardSelfAbility_EternityCard3 : DiceCardSelfAbilityBase
    {
        public override void OnApplyCard()
        {
            base.OnApplyCard();
            if (this.owner.cardOrder + 1 < BattleUnitBuf_InabaBuf2.GetStack(owner) || this.owner.cardOrder + 1 == BattleUnitBuf_InabaBuf3.GetStack(owner))
            {
                DiceCardXmlInfo xmlData = this.card.card.XmlData;
                DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769003), false);
                xmlData.workshopName = cardItem.Name;
                xmlData.DiceBehaviourList = cardItem.DiceBehaviourList;
                xmlData.Script = cardItem.Script;
            }
        }
        public override void OnUseCard()
        {
            card.GetDiceBehaviorList().ForEach(x => x.abilityList.Add(new Mysterium() { behavior = x }));
        }
        class Mysterium: DiceCardAbilityBase
        {
            public override void OnWinParrying()
            {
                KeywordBuf keyword = RandomUtil.SelectOne(KeywordBuf.Strength, KeywordBuf.Endurance, KeywordBuf.Protection);
                owner.bufListDetail.AddKeywordBufThisRoundByCard(keyword, 1);
            }
            public override void OnSucceedAttack()
            {
                KeywordBuf keyword = RandomUtil.SelectOne(KeywordBuf.Weak, KeywordBuf.Disarm, KeywordBuf.Vulnerable, KeywordBuf.Binding);
                behavior.card.target.bufListDetail.AddKeywordBufByCard(keyword, 1,owner);
            }
        }
    }
    //恐惧[崔斯特姆的诅咒] 书页
    public class DiceCardSelfAbility_EternityCard3_1 : DiceCardSelfAbilityBase
    {
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
            DiceCardXmlInfo xmlData = self.XmlData;
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769002), false);
            xmlData.workshopName = cardItem.Name;
            xmlData.DiceBehaviourList = cardItem.DiceBehaviourList;
            xmlData.Script = cardItem.Script;
        }
        public override void OnReleaseCard()
        {
            base.OnReleaseCard();
            DiceCardXmlInfo xmlData = this.card.card.XmlData;
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769002), false);
            xmlData.workshopName = cardItem.Name;
            xmlData.DiceBehaviourList = cardItem.DiceBehaviourList;
            xmlData.Script = cardItem.Script;
        }
        public override void OnSucceedAttack()
        {
            if(!card.target.bufListDetail.HasBuf<secondAttack>() && card.target.bufListDetail.HasBuf<firstAttack>())
                card.target.bufListDetail.AddBuf(new secondAttack(owner));
            if (!card.target.bufListDetail.HasBuf<firstAttack>())
                card.target.bufListDetail.AddBuf(new firstAttack());
        }
        public class firstAttack : BattleUnitBuf
        {
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, 1);
            }
        }
        public class secondAttack : BattleUnitBuf
        {
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                _kaguya.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1);
            }
            public secondAttack(BattleUnitModel Kaguya)
            {
                _kaguya = Kaguya;
            }
            private BattleUnitModel _kaguya;
        }
    }
    //恐惧[崔斯特姆的诅咒] 骰子
    public class DiceCardAbility_EternityDice1 : DiceCardAbilityBase
    {
        public override void OnSucceedAttack()
        {
            BattleUnitBuf_InabaBuf2.AddReadyStack(behavior.card.target, 1);
        }
    }
    //永远与须臾
    public class DiceCardSelfAbility_EternityCard4 : DiceCardSelfAbilityBase
    {
        public override bool IsTargetableAllUnit()
        {
            return true;
        }
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            BattleUnitModel benifitial = unit;
            if (unit.faction == targetUnit.faction)
                benifitial = targetUnit;
            benifitial.bufListDetail.AddBuf(new Recast());
            List<(KeywordBuf, int)> Buffs=new List<(KeywordBuf, int)>();
            foreach(BattleUnitBuf buf in benifitial.bufListDetail.GetActivatedBufList())
            {
                if (buf.bufType != KeywordBuf.None)
                    Buffs.Add((buf.bufType, buf.stack));
            }
            benifitial.bufListDetail.AddBuf(new Statis((int)benifitial.hp, benifitial.breakDetail.IsBreakLifeZero(), benifitial.breakDetail.breakGauge, benifitial.cardSlotDetail.PlayPoint, Buffs));
        }
        public class Recast : BattleUnitBuf
        {
            public override void OnEndBattle(BattlePlayingCardDataInUnitModel curCard)
            {
                DiceCardSpec DCS = curCard.card.XmlData.Spec;
                if (DCS.Ranged == CardRange.FarArea || DCS.Ranged == CardRange.FarAreaEach || DCS.affection == CardAffection.TeamNear)
                    return;
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
                if (aliveList.Count <= 0)
                    return;
                StageController.Instance.AddAllCardListInBattle(curCard, RandomUtil.SelectOne(aliveList));
                Destroy();
            }
        }
        public class Statis : BattleUnitBuf
        {
            private int hp;
            private bool isBreaked;
            private int bp;
            private int light;
            private List<(KeywordBuf, int)> Buffs = new List<(KeywordBuf, int)> ();
            public Statis(int hp,bool isBreaklifeZero, int bp, int light, List<(KeywordBuf,int)> buffs)
            {
                this.hp = hp;
                isBreaked = isBreaklifeZero;
                this.bp = bp;
                this.light = light;
                Buffs.AddRange(buffs);
            }
            public override void OnRoundEnd()
            {
                _owner.bufListDetail._readyBufList.Clear();
                _owner.SetHp(hp);
                if(!isBreaked && _owner.breakDetail.IsBreakLifeZero())
                {
                    _owner.RecoverBreakLife(this._owner.MaxBreakLife);
                    _owner.breakDetail.nextTurnBreak = false;
                }
                _owner.breakDetail.breakGauge=bp;
                _owner.cardSlotDetail._playPoint = light;
                _owner.bufListDetail._readyBufList.Clear();
                Buffs.ForEach(x => _owner.bufListDetail.AddKeywordBufByEtc(x.Item1,x.Item2));
                Destroy(); 
            }
        }
    }
    //耀眼的龙玉 书页
    public class DiceCardSelfAbility_PrizeCard : DiceCardSelfAbilityBase
    {
        public virtual int index => 0;
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            SpeedDiceUI SDUI = BattleManagerUI.Instance.selectedAllyDice;
            int unavailable = owner.speedDiceResult.FindAll(x => x.breaked).Count;
            return SDUI != null && SDUI._speedDiceIndex == unavailable + index;
        }
    }
    //火蜥蜴之盾 书页
    public class DiceCardSelfAbility_EternityCard5: DiceCardSelfAbility_PrizeCard
    {
        public override int index => 2;
        public override void OnUseCard()
        {
            owner.bufListDetail.AddBuf(new RejunateBurn());
        }
        class RejunateBurn : BattleUnitBuf
        {
            public override void OnRoundEnd()
            {
                int burn_stack = 0;
                foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList())
                {
                    BattleUnitBuf burn = unit.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_burn);
                    if (burn == null)
                        continue;
                    burn_stack += burn.stack;
                    burn.Destroy();
                }
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, burn_stack / 5);
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Burn, burn_stack);
                Destroy();
            }
        }
    }
    public class DiceCardAbility_EternityDice2 : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            BattleObjectManager.instance.GetAliveList_opponent(owner.faction).ForEach(x => x.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Burn, 1));
        }
    }
    //无限的生命之泉 书页
    public class DiceCardSelfAbility_EternityCard6 : DiceCardSelfAbility_PrizeCard
    {
        public override int index => 3;
        public override void OnUseCard()
        {
            owner.bufListDetail.AddBuf(new CheckUnused(card.GetDiceBehaviorList()));
        }
        class CheckUnused : BattleUnitBuf
        {
            List<BattleDiceBehavior> CheckingBehaviours;
            public CheckUnused(List<BattleDiceBehavior> checkingBehaviours)
            {
                CheckingBehaviours = checkingBehaviours;
            }
            public override void OnRoundEnd()
            {
                if (ExistUnused())
                    BattleObjectManager.instance.GetAliveList(_owner.faction).ForEach(x => x.RecoverHP((x.MaxHp - (int) x.hp) / 10));
                Destroy();
            }
            public bool ExistUnused()
            {
                foreach(BattleDiceBehavior b in CheckingBehaviours)
                {
                    if (!b.isUsed)
                        return true;
                }
                return false;
            }
        }
    }
    //佛体的金刚石 书页
    public class DiceCardSelfAbility_EternityCard7 : DiceCardSelfAbility_PrizeCard
    {
        public override int index => 1;
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            card.DestroyPlayingCard();
            List<BattleUnitModel> ally = BattleObjectManager.instance.GetAliveList(owner.faction);
            card.GetDiceBehaviorList().ForEach(b => ally.ForEach(x => x.cardSlotDetail.keepCard.AddBehaviour(card.card, b)));
        }
    }
    //佛体的金刚石 骰子
    public class DiceCardAbility_Shared : DiceCardAbilityBase
    {
        public override void AfterAction()
        {
            behavior.DestroyDice(DiceUITiming.AttackAfter);
        }
    }
    public class DiceCardAbility_EternityDice3 : DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            List<BattleUnitBuf> NegativeBuf= target.bufListDetail.GetActivatedBufList().FindAll(x => x.positiveType == BufPositiveType.Negative);
            foreach(BattleUnitBuf b in NegativeBuf)
            {
                b.stack += 1;
                b.OnAddBuf(1);
            }
        }
    }
    public class DiceCardAbility_EternityDice4 : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            BattleObjectManager.instance.GetAliveList(owner.faction).ForEach(x => x.RecoverHP(2));
        }
    }
    //神宝[蓬莱的弹枝-七色的弹幕-] 书页
    public class DiceCardSelfAbility_EternityCard8 : DiceCardSelfAbility_PrizeCard
    {
        public override int index => 4;
        public override bool OnChooseCard(BattleUnitModel owner)
        {
            SpeedDiceUI SDUI = BattleManagerUI.Instance.selectedAllyDice;
            int unavailable = owner.speedDiceResult.FindAll(x => x.breaked).Count;
            return SDUI != null && SDUI._speedDiceIndex == unavailable + index;
        }
    }
    //神宝[蓬莱的弹枝-七色的弹幕-] 骰子
    public class DiceCardAbility_EternityDice5 : DiceCardAbilityBase
    {
        private static DiceBehaviour BulletsStorm = new DiceBehaviour() { 
            Min = 5, Dice = 7, Type = BehaviourType.Atk, Detail = BehaviourDetail.Hit, MotionDetail=MotionDetail.F,
            EffectRes = "", Script = "", ActionScript = "",  Desc = ""};
        public override void BeforeRollDice()
        {
            behavior.SetBlocked(true);
        }
        public override void OnAfterAreaAtk(List<BattleUnitModel> damagedList, List<BattleUnitModel> defensedList)
        {
            if (damagedList.Count <= 0)
                return;
            for(int i=0; i<7; i++)
            {
                BattleDiceCardModel card = RandomUtil.SelectOne(owner.allyCardDetail.GetHand());
                card.AddBuf(new PuzzleDiceAbility5.CostDownSelfBuf());
            }
            
            foreach(BattleUnitModel b in damagedList)
            {
                for (int i = 0; i < 7; i++)
                    BattleVoidBehaviour.ExtraHit(b, card,BulletsStorm);
            }
        }
    }
    public class DiceCardSelfAbility_ETpassfloor : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            base.OnUseInstance(unit, self, targetUnit);
            SaveData saveData = Singleton<EternalityTempleSaveManager>.Instance.LoadData("passFloor");
            int dmg1 = saveData.GetInt(Singleton<StageController>.Instance.CurrentFloor.ToString() + "Kaguya");
            int dmg2 = saveData.GetInt(Singleton<StageController>.Instance.CurrentFloor.ToString() + "Yagokoro");
            int dmg3 = saveData.GetInt(Singleton<StageController>.Instance.CurrentFloor.ToString() + "Inaba");
            BattleObjectManager.instance.GetAliveList(Faction.Player).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769103).TakeDamage(dmg1);
            BattleObjectManager.instance.GetAliveList(Faction.Player).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769104).TakeDamage(dmg2);
            BattleObjectManager.instance.GetAliveList(Faction.Player).Find((BattleUnitModel x) => x.Book.GetBookClassInfoId().id == 226769105).TakeDamage(dmg3);
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(Faction.Enemy))
            {
                battleUnitModel.Die(null, true);
            }
            Singleton<StageController>.Instance.CheckEndBattle();
        }
    }

}

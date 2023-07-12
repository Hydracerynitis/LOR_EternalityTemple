using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EternalityTemple.Kaguya
{
    public class DiceCardSelfAbility_EternityCard5: DiceCardSelfAbilityBase
    {
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
    public class DiceCardSelfAbility_EternityCard6 : DiceCardSelfAbilityBase
    {
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
    public class DiceCardSelfAbility_EternityCard7 : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            card.DestroyPlayingCard();
            List<BattleUnitModel> ally = BattleObjectManager.instance.GetAliveList(owner.faction);
            card.GetDiceBehaviorList().ForEach(b => ally.ForEach(x => x.cardSlotDetail.keepCard.AddBehaviour(card.card, b)));
        }

    }
    public class DiceCardAbility_EternityDice2 : DiceCardAbilityBase
    {
        public override void OnWinParrying()
        {
            BattleObjectManager.instance.GetAliveList_opponent(owner.faction).ForEach(x => x.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Burn, 1));
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
    public class DiceCardAbility_Shared : DiceCardAbilityBase
    {
        public override void AfterAction()
        {
            behavior.DestroyDice(DiceUITiming.AttackAfter);
        }
    }
}

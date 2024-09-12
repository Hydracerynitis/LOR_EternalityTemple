using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using UnityEngine;
using EternalityTemple.Kaguya;
using EternalityTemple.Yagokoro;

namespace EternalityTemple.Inaba
{
	public class DiceCardSelfAbility_EternityXS_Card1 : DiceCardSelfAbilityBase
	{
        public override void OnApplyCard()
        {
			base.OnApplyCard();
			if (owner.cardOrder + 1 <= BattleUnitBuf_InabaBuf2.GetStack(owner) || owner.cardOrder + 1 == BattleUnitBuf_InabaBuf3.GetStack(owner))
            {
				DiceCardXmlInfo xmlData = this.card.card.XmlData;
				List<DiceBehaviour> list = new List<DiceBehaviour>();
				DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769311), false);
				foreach (DiceBehaviour diceBehaviour in cardItem.DiceBehaviourList)
				{
					DiceBehaviour diceBehaviour2 = diceBehaviour.Copy();
					list.Add(diceBehaviour2);
				}
				xmlData.DiceBehaviourList = list;
			}
		}
        public override void OnUseCard()
		{
			owner.cardSlotDetail.RecoverPlayPointByCard(1);
			if(BattleUnitBuf_KaguyaBuf.GetStack(owner)>=7)
            {
				owner.allyCardDetail.DrawCards(1);
			}
			if (BattleObjectManager.instance.GetAliveList(base.owner.faction).Find((BattleUnitModel x) => x.bufListDetail.HasBuf<BattleUnitBuf_Moon3>()) != null)
            {
				owner.cardSlotDetail.RecoverPlayPointByCard(1);
			}
		}
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
			DiceCardXmlInfo xmlData = self.XmlData;
			List<DiceBehaviour> list = new List<DiceBehaviour>();
			DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769300), false);
			foreach (DiceBehaviour diceBehaviour in cardItem.DiceBehaviourList)
			{
				DiceBehaviour diceBehaviour2 = diceBehaviour.Copy();
				list.Add(diceBehaviour2);
			}
			xmlData.DiceBehaviourList = list;
		}
        public override void OnReleaseCard()
        {
            base.OnReleaseCard();
			DiceCardXmlInfo xmlData = this.card.card.XmlData;
			List<DiceBehaviour> list = new List<DiceBehaviour>();
			DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769300), false);
			foreach (DiceBehaviour diceBehaviour in cardItem.DiceBehaviourList)
			{
				DiceBehaviour diceBehaviour2 = diceBehaviour.Copy();
				list.Add(diceBehaviour2);
			}
			xmlData.DiceBehaviourList = list;
		}
    }
	public class DiceCardSelfAbility_InabaCard1 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			firstDiceLoseParrying = false;
		}
		public bool firstDiceLoseParrying;
	}
	public class DiceCardSelfAbility_InabaCard2 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(owner.faction))
			{
				BattleUnitBuf_InabaBuf4.AddStack(battleUnitModel, 1);
			}
		}
		public override void OnStartBattle()
		{
			base.OnStartBattle();
			foreach (BattlePlayingCardDataInUnitModel battlePlayingCardDataInUnitModel in Singleton<StageController>.Instance.GetAllCards())
            {
				if(battlePlayingCardDataInUnitModel.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>())
                {
					battlePlayingCardDataInUnitModel.target = owner;
					battlePlayingCardDataInUnitModel.targetSlotOrder = -1;
				}
			}
		}
	}
	public class DiceCardSelfAbility_InabaCard3 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			if (card.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>() && BattleUnitBuf_InabaBuf5.GetStack(owner) >= 3)
			{
				owner.bufListDetail.RemoveBufAll(typeof(BattleUnitBuf_InabaBuf5));
				owner.TakeDamage(40);
				foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
				{
					battleUnitModel.TakeDamage(40);
				}
				return;
			}
			BattleUnitBuf_InabaBuf5.AddStack(owner, 1);
		}
	}
	public class DiceCardSelfAbility_InabaCard4 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			if (card.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>())
			{
				this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
				{
					power = 3
				});
			}
		}
	}
	public class DiceCardSelfAbility_InabaCard5 : DiceCardSelfAbilityBase
	{
	}
	public class DiceCardSelfAbility_InabaCard6 : DiceCardSelfAbilityBase
	{
        public override void OnSucceedAttack()
		{
			if (card.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>())
			{
				card.target.allyCardDetail.DiscardACardRandomlyByAbility(2);
			}
		}
	}
	public class DiceCardSelfAbility_InabaCard7 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			if(BattleUnitBuf_InabaBuf1.GetStack(owner)<150)
            {
				return;
            }
			BattleUnitBuf_InabaBuf1.AddStack(owner, -150);
			BattleUnitBuf_InabaBuf7.AddReadyStack(owner, 2);
			owner.view.ChangeSkin("Reisen2");
		}
        public override bool OnChooseCard(BattleUnitModel owner)
        {
			return BattleUnitBuf_InabaBuf1.GetStack(owner) >= 150;
		}
    }
	public class DiceCardSelfAbility_InabaCard8 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			BattleUnitBuf_InabaBuf3.AddReadyStack(card.target, card.targetSlotOrder + 1);
			if (card.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>())
			{
				BattleUnitModel battleUnitModel = RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList(owner.faction));
				if (battleUnitModel != null)
                {
					BattleUnitBuf_InabaBuf2.AddReadyStack(battleUnitModel, 1);
				}
			}
		}
		public override bool IsValidTarget(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
		{
			return targetUnit != null && targetUnit.faction == unit.faction;
		}
		public override bool IsOnlyAllyUnit()
		{
			return true;
		}
        public override bool IsTargetChangable(BattleUnitModel attacker)
        {
            return false;
        }
    }
	public class DiceCardSelfAbility_InabaCard9 : DiceCardSelfAbilityBase
	{
		public override void OnUseCard()
		{
			int num = owner.breakDetail.breakGauge;
			int num2 = owner.breakDetail.GetDefaultBreakGauge();
			owner.breakDetail.RecoverBreak((num2 - num) / 2);
			int num3 = card.target.breakDetail.breakGauge;
			int num4 = card.target.breakDetail.GetDefaultBreakGauge();
			card.target.breakDetail.RecoverBreak((num4 - num3) / 2);
			if (card.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>())
			{
				foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(owner.faction))
				{
					battleUnitModel.TakeBreakDamage(Mathf.Min((num2 + num4 - num - num3) / 4, 20));
				}
			}
		}
		public override bool IsValidTarget(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
		{
			return targetUnit != null && targetUnit.faction == unit.faction;
		}
		public override bool IsOnlyAllyUnit()
		{
			return true;
		}
		public override bool IsTargetChangable(BattleUnitModel attacker)
		{
			return false;
		}
	}
}
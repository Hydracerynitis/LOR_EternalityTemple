using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using UnityEngine;

namespace EternalityTemple.Inaba
{
	public class DiceCardSelfAbility_EternityXS_Card1 : InabaExtraCardAbility
	{
        public override void OnApplyCard()
        {
			base.OnApplyCard();
			if(this.owner.cardOrder+1 < BattleUnitBuf_InabaBuf2.GetStack(owner))
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
	public class DiceCardSelfAbility_InabaCard1 : InabaExtraCardAbility
	{
		public override void OnUseCard()
		{
			firstDiceLoseParrying = false;
		}
		public bool firstDiceLoseParrying;
	}
	public class DiceCardSelfAbility_InabaCard2 : InabaExtraCardAbility
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
	public class DiceCardSelfAbility_InabaCard3 : InabaExtraCardAbility
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
	public class DiceCardSelfAbility_InabaCard4 : InabaExtraCardAbility
	{
		public override void OnUseCard()
		{
			if (card.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>())
			{
				this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
				{
					power = 5
				});
			}
		}
	}
	public class DiceCardSelfAbility_InabaCard5 : InabaExtraCardAbility
	{
	}
	public class DiceCardSelfAbility_InabaCard6 : InabaExtraCardAbility
	{
        public override void OnSucceedAttack()
		{
			if (card.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>())
			{
				card.target.allyCardDetail.DiscardACardRandomlyByAbility(2);
			}
		}
	}
	public class DiceCardSelfAbility_InabaCard7 : InabaExtraCardAbility
	{
		public override void OnUseCard()
		{
			if(BattleUnitBuf_InabaBuf1.GetStack(owner)<150)
            {
				return;
            }
			BattleUnitBuf_InabaBuf1.AddStack(owner, -150);
			BattleUnitBuf_InabaBuf7.AddReadyStack(owner, 2);
		}
	}
}
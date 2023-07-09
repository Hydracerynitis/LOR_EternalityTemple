using System;
using LOR_DiceSystem;
using System.Collections.Generic;

namespace EternalityTemple.Inaba
{
	public class DiceCardSelfAbility_EternityXS_Card1 : InabaExtraCardAbility
	{
        public override void OnInabaBuf()
        {
            base.OnInabaBuf();
			DiceCardXmlInfo xmlData = this.card.card.XmlData;
			DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769311), false);
			xmlData.DiceBehaviourList = cardItem.DiceBehaviourList;
		}
        public override void OnUseCard()
		{
			owner.cardSlotDetail.RecoverPlayPointByCard(1);
		}
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
			DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769300), false);
			self.XmlData.DiceBehaviourList = cardItem.DiceBehaviourList;
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
			if (card.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>())
			{
				foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(owner.faction))
				{
					BattleUnitBuf_InabaBuf4.AddStack(battleUnitModel, 1);
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
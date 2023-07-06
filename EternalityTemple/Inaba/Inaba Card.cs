using System;

namespace EternalityTemple.Inaba
{
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
}
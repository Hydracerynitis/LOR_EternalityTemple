using System;
using UnityEngine;

namespace EternalityTemple.Inaba
{
	public class DiceCardAbility_InabaDice1 : DiceCardAbilityBase
	{
		public override void OnLoseParrying()
		{
			this.behavior.card.ApplyDiceStatBonus(DiceMatch.LastDice, new DiceStatBonus
			{
				power = 5
			});
			DiceCardSelfAbility_InabaCard1 diceCardSelfAbility = base.card.cardAbility as DiceCardSelfAbility_InabaCard1;
			if (diceCardSelfAbility != null)
			{
				if (diceCardSelfAbility.firstDiceLoseParrying && base.card.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>())
				{
					card.target.currentDiceAction.DestroyDice(DiceMatch.AllDice, DiceUITiming.Start);
					return;
				}
				diceCardSelfAbility.firstDiceLoseParrying = true;
			}
		}
	}
	public class DiceCardAbility_InabaDice2 : DiceCardAbilityBase
	{
		public override void BeforeRollDice()
		{
			this.behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				breakRate = 100
			});
		}
	}
	public class DiceCardAbility_InabaDice3 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			target.TakeDamage(owner.history.damageAtOneRoundByDice);
			if(card.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>())
            {
				BattleUnitBuf_InabaBuf2.AddReadyStack(target, owner.history.damageAtOneRoundByDice / 5);
				Debug.Log("aaaaa");
            }
		}
	}
	public class DiceCardAbility_InabaDice4 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			target.bufListDetail.AddReadyBuf(new BattleUnitBuf_InabaBuf6());
		}
	}
	public class DiceCardAbility_InabaDice5 : DiceCardAbilityBase
	{
		public override void OnWinParrying()
		{
			card.owner.cardSlotDetail.RecoverPlayPointByCard(1);
			card.target.cardSlotDetail.RecoverPlayPointByCard(1);
		}
	}
	public class DiceCardAbility_InabaDice6 : DiceCardAbilityBase
	{
		public override void OnWinParrying()
		{
			card.owner.allyCardDetail.DrawCards(1);
			card.target.allyCardDetail.DrawCards(1);
		}
	}


}
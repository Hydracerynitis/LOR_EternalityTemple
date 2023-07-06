using System;

namespace EternalityTemple.Inaba
{
	public class DiceCardAbility_InabaDice1 : DiceCardAbilityBase
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00002621 File Offset: 0x00000821
		public override void OnLoseParrying()
		{
			this.behavior.card.ApplyDiceStatBonus(DiceMatch.LastDice, new DiceStatBonus
			{
				power = 5
			});
		}
	}
	public class DiceCardAbility_InabaDice2 : DiceCardAbilityBase
	{
		// Token: 0x0600003D RID: 61 RVA: 0x0000226E File Offset: 0x0000046E
		public override void BeforeRollDice()
		{
			this.behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				breakRate = 100
			});
		}
	}
	// Token: 0x020000CD RID: 205
	public class DiceCardAbility_InabaDice3 : DiceCardAbilityBase
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00002621 File Offset: 0x00000821
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			target.TakeDamage(owner.history.damageAtOneRoundByDice);
		}
	}
	public class DiceCardAbility_InabaDice5 : DiceCardAbilityBase
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00002621 File Offset: 0x00000821
		public override void OnWinParrying()
		{
			card.owner.cardSlotDetail.RecoverPlayPointByCard(1);
			card.target.cardSlotDetail.RecoverPlayPointByCard(1);
		}
	}
	public class DiceCardAbility_InabaDice6 : DiceCardAbilityBase
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00002621 File Offset: 0x00000821
		public override void OnWinParrying()
		{
			card.owner.allyCardDetail.DrawCards(1);
			card.target.allyCardDetail.DrawCards(1);
		}
	}


}
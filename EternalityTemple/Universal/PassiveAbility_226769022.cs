using System;
using System.Collections.Generic;
using UnityEngine;
public class PassiveAbility_226769022 : PassiveAbilityBase
{
	public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
	{
		if (curCard.card.GetOriginCost() >= 4)
		{
			this.owner.cardSlotDetail.RecoverPlayPoint(2);
		}
	}
	public override void BeforeRollDice(BattleDiceBehavior behavior)
	{
		BattleCardTotalResult battleCardResultLog = this.owner.battleCardResultLog;
		if (battleCardResultLog != null)
		{
			battleCardResultLog.SetPassiveAbility(this);
		}
		behavior.ApplyDiceStatBonus(new DiceStatBonus
		{
			power = 4
		});
	}
	public override void OnWaveStart()
	{
		this._stack = 0;
	}

	// Token: 0x060056F4 RID: 22260 RVA: 0x001CA7F8 File Offset: 0x001C89F8
	public override void OnRoundStart()
	{
		if (this._stack > 0)
		{
			this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this._stack, this.owner);
			this.owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Endurance, this._stack, this.owner);
		}
	}

	// Token: 0x060056F5 RID: 22261 RVA: 0x001CA84A File Offset: 0x001C8A4A
	public override void OnDieOtherUnit(BattleUnitModel unit)
	{
		if (unit.faction == this.owner.faction && this._stack < 2)
		{
			this._stack++;
		}
	}
	private int _stack;
}
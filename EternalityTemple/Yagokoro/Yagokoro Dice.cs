using System;
using UnityEngine;
using System.Collections.Generic;
namespace EternalityTemple.Yagokoro
{
	public class DiceCardAbility_YagokoroDice1 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			DiceCardSelfAbility_YagokoroCard1 diceCardSelfAbility = base.card.cardAbility as DiceCardSelfAbility_YagokoroCard1;
			if (diceCardSelfAbility != null)
            {
				if (diceCardSelfAbility.dreamType == 1)
                {
					target.bufListDetail.AddReadyBuf(new YagokoroBuf5());
				}
				if (diceCardSelfAbility.dreamType == 2)
				{
					target.bufListDetail.AddReadyBuf(new YagokoroBuf6());
				}
			}
			target.bufListDetail.AddReadyBuf(new YagokoroBuf4());
		}
	}
	public class DiceCardAbility_YagokoroDice2 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			DiceCardSelfAbility_YagokoroCard2 diceCardSelfAbility = base.card.cardAbility as DiceCardSelfAbility_YagokoroCard2;
			int num = 1;
			if (diceCardSelfAbility.stackBoost)
				num++;
			target.bufListDetail.AddBuf(new YagokoroBuf7(num));
		}
	}
	public class DiceCardAbility_YagokoroDice3 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(base.owner.faction);
			DiceCardSelfAbility_YagokoroCard4 diceCardSelfAbility = base.card.cardAbility as DiceCardSelfAbility_YagokoroCard4;
			if (aliveList.Count > 0)
			{
				aliveList.Sort((BattleUnitModel x, BattleUnitModel y) => (int)(x.hp - y.hp));
				aliveList[0].RecoverHP(behavior.DiceResultValue / 2);
				aliveList[0].breakDetail.RecoverBreak(behavior.DiceResultValue / 2);
				if (diceCardSelfAbility.firstDice)
				{
					aliveList[0].RecoverHP(behavior.DiceResultValue / 2);
					aliveList[0].breakDetail.RecoverBreak(behavior.DiceResultValue / 2);
				}
			}
		}
	}
	public class DiceCardAbility_YagokoroDice4 : DiceCardAbilityBase
	{
        public override void OnWinParrying()
        {
			List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(base.owner.faction);
			DiceCardSelfAbility_YagokoroCard4 diceCardSelfAbility = base.card.cardAbility as DiceCardSelfAbility_YagokoroCard4;
			if (aliveList.Count > 0)
			{
				aliveList.Sort((BattleUnitModel x, BattleUnitModel y) => (int)(x.hp - y.hp));
				aliveList[0].RecoverHP(behavior.DiceResultValue / 2);
				aliveList[0].breakDetail.RecoverBreak(behavior.DiceResultValue / 2);
				if (diceCardSelfAbility.secondDice)
                {
					aliveList[0].RecoverHP(behavior.DiceResultValue / 2);
					aliveList[0].breakDetail.RecoverBreak(behavior.DiceResultValue / 2);
				}
				
			}
		}
	}
	public class DiceCardAbility_YagokoroDice5 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Decay, 1, owner);
		}
	}
	public class DiceCardAbility_YagokoroDice_aoe1 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			DiceCardSelfAbility_YagokoroCard7 diceCardSelfAbility = base.card.cardAbility as DiceCardSelfAbility_YagokoroCard7;
			if (diceCardSelfAbility == null)
				return;
			if (behavior.Index == 0 && diceCardSelfAbility.firstMoon)
			{
				target.bufListDetail.AddReadyBuf(new YagokoroBuf8());
				diceCardSelfAbility.firstMoon = false;
			}
		}
	}
	public class DiceCardAbility_YagokoroDice_aoe2 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			DiceCardSelfAbility_YagokoroCard7 diceCardSelfAbility = base.card.cardAbility as DiceCardSelfAbility_YagokoroCard7;
			if (diceCardSelfAbility == null)
				return;
			if (behavior.Index == 1 && diceCardSelfAbility.thirdMoon)
			{
				target.bufListDetail.AddReadyBuf(new YagokoroBuf9());
				diceCardSelfAbility.thirdMoon = false;
			}
		}
	}
	public class DiceCardAbility_YagokoroDice_aoe3 : DiceCardAbilityBase
	{
		public override void OnSucceedAttack(BattleUnitModel target)
		{
			DiceCardSelfAbility_YagokoroCard7 diceCardSelfAbility = base.card.cardAbility as DiceCardSelfAbility_YagokoroCard7;
			if (diceCardSelfAbility == null)
				return;
			if (behavior.Index == 2 && diceCardSelfAbility.fifthMoon)
			{
				target.bufListDetail.AddReadyBuf(new YagokoroBuf10());
				diceCardSelfAbility.fifthMoon = false;
			}
		}
	}
}
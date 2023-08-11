using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using UnityEngine;
using EternalityTemple.Kaguya;
using EternalityTemple.Inaba;

namespace EternalityTemple.Yagokoro
{
	public class DiceCardSelfAbility_EternityTs_Card1 : MoonCardAbility
	{
        public override bool CanActivateMoon(int slot)
        {
            return slot >= 1 && slot <= 4;
        }
		public override void OnUseCard()
		{
			owner.cardSlotDetail.RecoverPlayPointByCard(1);
			if (owner.bufListDetail.HasBuf<BattleUnitBuf_KaguyaBuf7>())
			{
				owner.allyCardDetail.DrawCards(1);
			}
			if (BattleObjectManager.instance.GetAliveList(base.owner.faction).Find((BattleUnitModel x) => x.bufListDetail.HasBuf<BattleUnitBuf_Moon3>()) != null)
			{
				owner.cardSlotDetail.RecoverPlayPointByCard(1);
			}
		}
        public override void OnFirstMoon()
        {
            base.OnFirstMoon();
			this.card.ApplyDiceStatBonus(DiceMatch.NextDice, new DiceStatBonus
			{
				power = 1
			});
		}
		public override void OnSecondMoon()
		{
			DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(613007, false);
			BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
			battleDiceBehavior.behaviourInCard = cardItem.DiceBehaviourList[0].Copy();
			battleDiceBehavior.behaviourInCard.EffectRes = "";
			battleDiceBehavior.behaviourInCard.Min = 1;
			battleDiceBehavior.behaviourInCard.Dice = 4;
			battleDiceBehavior.behaviourInCard.Detail = BehaviourDetail.Penetrate;
			this.card.AddDice(battleDiceBehavior);
		}
		public override void OnThirdMoon()
		{
			base.OnFirstMoon();
			this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
			{
				power = 1
			});
		}
        public override void OnForthMoon()
        {
			DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(613007, false);
			BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
			battleDiceBehavior.behaviourInCard = cardItem.DiceBehaviourList[0].Copy();
			battleDiceBehavior.behaviourInCard.EffectRes = "";
			battleDiceBehavior.behaviourInCard.Min = 1;
			battleDiceBehavior.behaviourInCard.Dice = 4;
			battleDiceBehavior.behaviourInCard.Detail = BehaviourDetail.Guard;
			battleDiceBehavior.behaviourInCard.Type = BehaviourType.Def;
			this.card.AddDice(battleDiceBehavior);
			this.card.ApplyDiceStatBonus(DiceMatch.DiceByIdx(1), new DiceStatBonus
			{
				power = 3
			});
		}
	}
	public class DiceCardSelfAbility_YagokoroCard1 : MoonCardAbility
	{
		public override bool CanActivateMoon(int slot)
		{
			return slot == 1 || slot == 5;
		}
		public override void OnApplyCard()
		{
			base.OnApplyCard();
			dreamType = 0;
			if (this.owner.cardOrder + 1 < BattleUnitBuf_InabaBuf2.GetStack(owner) || this.owner.cardOrder + 1 == BattleUnitBuf_InabaBuf3.GetStack(owner))
			{
				DiceCardXmlInfo xmlData = this.card.card.XmlData;
				DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769017), false);
				xmlData.workshopName = cardItem.Name;
				xmlData.DiceBehaviourList = cardItem.DiceBehaviourList;
				xmlData.Script = cardItem.Script;
			}
		}
		public override void OnFirstMoon()
		{
			base.OnFirstMoon();
			dreamType = 1;
		}
		public override void OnFifthMoon()
		{
			base.OnFirstMoon();
			dreamType = 2;
		}
		public override bool IsValidTarget(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
		{
			return targetUnit != null && targetUnit != unit;
		}
		public int dreamType = 0;
	}
	public class DiceCardSelfAbility_YagokoroCard2 : MoonCardAbility
	{
		public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
		{
			base.OnEnterCardPhase(unit, self);
			DiceCardXmlInfo xmlData = self.XmlData;
			DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769016), false);
			xmlData.workshopName = cardItem.Name;
			xmlData.DiceBehaviourList = cardItem.DiceBehaviourList;
			xmlData.Script = cardItem.Script;
		}
		public override void OnReleaseCard()
		{
			base.OnReleaseCard();
			DiceCardXmlInfo xmlData = this.card.card.XmlData;
			DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769016), false);
			xmlData.workshopName = cardItem.Name;
			xmlData.DiceBehaviourList = cardItem.DiceBehaviourList;
			xmlData.Script = cardItem.Script;
		}
		public override bool CanActivateMoon(int slot)
		{
			return slot == 3;
		}
        public override void OnThirdMoon()
        {
            base.OnThirdMoon();
			stackBoost = true;
		}
		public bool stackBoost = false;
	}
	public class DiceCardSelfAbility_YagokoroCard3 : MoonCardAbility
	{
		public override bool CanActivateMoon(int slot)
		{
			return slot >= 1 && slot <= 5;
		}
		public override void OnFirstMoon()
		{
			DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(613007, false);
			BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
			battleDiceBehavior.behaviourInCard = cardItem.DiceBehaviourList[0].Copy();
			battleDiceBehavior.behaviourInCard.EffectRes = "";
			battleDiceBehavior.behaviourInCard.Min = 4;
			battleDiceBehavior.behaviourInCard.Dice = 8;
			battleDiceBehavior.behaviourInCard.Detail = BehaviourDetail.Penetrate;
			battleDiceBehavior.behaviourInCard.Type = BehaviourType.Atk;
			this.card.AddDice(battleDiceBehavior);
			this.card.AddDice(battleDiceBehavior);
			this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
			{
				power = -4
			});
		}
		public override void OnSecondMoon()
		{
			DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(613007, false);
			BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
			battleDiceBehavior.behaviourInCard = cardItem.DiceBehaviourList[0].Copy();
			battleDiceBehavior.behaviourInCard.EffectRes = "";
			battleDiceBehavior.behaviourInCard.Min = 4;
			battleDiceBehavior.behaviourInCard.Dice = 8;
			battleDiceBehavior.behaviourInCard.Detail = BehaviourDetail.Penetrate;
			battleDiceBehavior.behaviourInCard.Type = BehaviourType.Atk;
			this.card.AddDice(battleDiceBehavior);
			this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
			{
				power = -2
			});
		}
		public override void OnThirdMoon()
		{
			this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
			{
				power = 1
			});
		}
		public override void OnForthMoon()
		{
			this.card.DestroyDice(DiceMatch.NextDice);
			this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
			{
				power = 5
			});
		}
		public override void OnFifthMoon()
		{
			this.card.card.AddBuf(new BattleDiceCardBuf_YagokoroCardBuf1());
		}
	}
	public class DiceCardSelfAbility_YagokoroCard4 : MoonCardAbility
	{
		public override bool CanActivateMoon(int slot)
		{
			return slot >= 3 && slot <= 5;
		}
		public override void OnThirdMoon()
		{
			this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
			{
				power = 2
			});
		}
		public override void OnForthMoon()
		{
			firstDice = true;
		}
		public override void OnFifthMoon()
		{
			secondDice = true;
		}
        public override void OnApplyCard()
        {
			firstDice = false;
			secondDice = false;
		}
		public bool firstDice;
		public bool secondDice;
	}
	public class DiceCardSelfAbility_YagokoroCard5 : MoonCardAbility
	{
		public override bool CanActivateMoon(int slot)
		{
			return slot >= 1 && slot <= 3;
		}
		public override void OnFirstMoon()
		{
			DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(613007, false);
			BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior();
			battleDiceBehavior.behaviourInCard = cardItem.DiceBehaviourList[0].Copy();
			battleDiceBehavior.behaviourInCard.EffectRes = "";
			battleDiceBehavior.behaviourInCard.Min = 4;
			battleDiceBehavior.behaviourInCard.Dice = 8;
			battleDiceBehavior.behaviourInCard.Detail = BehaviourDetail.Hit;
			battleDiceBehavior.behaviourInCard.Type = BehaviourType.Atk;
			this.card.AddDiceFront(battleDiceBehavior);
		}
		public override void OnSecondMoon()
		{
			secondMoonDecay = true;
		}
		public override void OnThirdMoon()
		{
			this.card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
			{
				power = 2
			});
		}
		public override void OnApplyCard()
		{
			secondMoonDecay = false;
		}
		public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
			if(secondMoonDecay)
			{ 
				behavior.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Decay, 1, owner); 
			}
        }
        private bool secondMoonDecay;
	}
	public class DiceCardSelfAbility_YagokoroCard6 : MoonCardAbility
	{
		public override bool CanActivateMoon(int slot)
		{
			return slot >= 1 && slot <= 5;
		}
		public override void OnFirstMoon()
		{
			firstMoon = true;
		}
		public override void OnSecondMoon()
		{
			secondMoon = true;
		}
		public override void OnThirdMoon()
		{
			thirdMoon = true;
		}
		public override void OnForthMoon()
		{
			forthMoon = true;
		}
		public override void OnFifthMoon()
		{
			fifthMoon = true;
		}
		public override void OnApplyCard()
		{
			firstMoon = false;
			secondMoon = false;
			thirdMoon = false;
			forthMoon = false;
			fifthMoon = false;
		}
		public override void OnSucceedAttack(BattleDiceBehavior behavior)
		{
			base.OnSucceedAttack(behavior);
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(owner.faction))
			{
				if(firstMoon)
                {
					battleUnitModel.bufListDetail.AddKeywordBufByCard(KeywordBuf.Strength, 1, owner);
				}
				if(fifthMoon)
                {
					battleUnitModel.bufListDetail.AddKeywordBufByCard(KeywordBuf.BreakProtection, 1, owner);
				}
			}
			if (secondMoon)
			{
				behavior.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Disarm, 1, owner);
			}
			if (thirdMoon)
			{
				behavior.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Binding, 1, owner);
			}
			if (forthMoon)
			{
				behavior.card.target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 1, owner);
			}
		}
		private bool firstMoon;
		private bool secondMoon;
		private bool thirdMoon;
		private bool forthMoon;
		private bool fifthMoon;
	}
	public class DiceCardSelfAbility_YagokoroCard7 : MoonCardAbility
	{
		public override bool CanActivateMoon(int slot)
		{
			return slot == 1 || slot == 3 || slot == 5;
		}
		public override void OnFirstMoon()
		{
			firstMoon = true;
		}
		public override void OnThirdMoon()
		{
			thirdMoon = true;
		}
		public override void OnFifthMoon()
		{
			fifthMoon = true;
		}
		public bool firstMoon;
		public bool thirdMoon;
		public bool fifthMoon;
	}

	public class DiceCardSelfAbility_YagokoroCard13 : MoonCardAbility
	{
        public override bool OnChooseCard(BattleUnitModel owner)
        {
			return !owner.bufListDetail.HasBuf<YagokoroBuf12>();
        }
        public override void OnUseCard()
        {
            base.OnUseCard();
			owner.bufListDetail.AddReadyBuf(new YagokoroBuf12());
        }
    }
}
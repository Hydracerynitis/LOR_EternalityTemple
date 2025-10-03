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
        public override List<string> GetMoonKeywords()
        {
			List<string> output = base.GetMoonKeywords();
			output.Add("YagokoroBuf_txt");
            return output;
        }
        public override string GetMoonAbilityText()
        {
            string id = "EternityTs_Card1";
            switch (moonPreview)
            {
                case -1:
                    id = "EternityTs_Card1_NoMoon";
                    break;
                case 1:
                    id = "EternityTs_Card1_Moon1";
                    break;
                case 2:
                    id = "EternityTs_Card1_Moon2";
                    break;
                case 3:
                    id = "EternityTs_Card1_Moon3";
                    break;
                case 4:
                    id = "EternityTs_Card1_Moon4";
                    break;

            }
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
        }
        public override string GetFullMoonAbilityText()
        {
            string id = "EternityTs_Card1";
			if (moonPreview != 0)
				id = "EternityTs_Card1_FullMoon";
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
        }
        public override bool CanActivateMoon(int slot)
        {
            return slot >= 1 && slot <= 4;
        }
		public override void OnUseCard()
		{
			owner.cardSlotDetail.RecoverPlayPointByCard(1);
			if (BattleUnitBuf_KaguyaBuf.GetStack(owner)>=7)
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
        public bool activate;
        public override bool CanActivateMoon(int slot)
		{
			if (!activate)
				return slot == 1 || slot == 5;
			else
				return slot == 2;
		}
        public override List<string> GetMoonKeywords()
        {
			List<string> output = base.GetMoonKeywords();
			output.AddRange(new string[] { "EternityCard2_Keyword", "YagokoroBuf_txt" });
			if (!activate)
			{
                if (moonPreview == 1)
                    output.Add("YagokoroBuf5");
                if (moonPreview == 5)
                    output.Add("YagokoroBuf6");
                if (moonPreview == -1)
                    output.Add("YagokoroBuf4");
                if (moonPreview == 0)
                    output.AddRange(new string[] { "YagokoroBuf4", "YagokoroBuf5", "YagokoroBuf6" });
            }
			else
                output.Add("YagokoroBuf7");
            return output;
        }

        public override string GetMoonAbilityText()
        {
			string id;
			if (!activate)
			{
                id = "YagokoroCard1";
                switch (moonPreview)
                {
                    case -1:
                        id = "YagokoroCard1_NoMoon";
                        break;
                    case 1:
                        id = "YagokoroCard1_Moon1";
                        break;
                    case 5:
                        id = "YagokoroCard1_Moon5";
                        break;
                }
            }
			else
			{
				id = "YagokoroCard2";
				switch (moonPreview)
				{
                    case -1:
                        id = "YagokoroCard2_NoMoon";
                        break;
                    case 2:
                        id = "YagokoroCard2_Moon2";
                        break;
                }
            }
			return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
        }
        public override string GetFullMoonAbilityText()
        {
            return GetMoonAbilityText();
        }
        public void SetCardScriptActivate(bool activate)
		{
			DiceCardSelfAbility_YagokoroCard1 BattleDiceCardModelAbility = card?.card?._script as DiceCardSelfAbility_YagokoroCard1;
			if(BattleDiceCardModelAbility != null )
				BattleDiceCardModelAbility.activate= activate;
        }
		public override void OnApplyCard()
		{
			base.OnApplyCard();
			dreamType = 0;
            if (BattleUnitBuf_InabaBuf2.CheckFrenzy(owner, owner.cardOrder))
			{
                DiceCardXmlInfo xmlData = card.card.XmlData;
                DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(EternalityInitializer.GetLorId(226769017), false);
                xmlData.workshopName = cardItem.Name;
                xmlData.DiceBehaviourList = cardItem.DiceBehaviourList;
                xmlData.Rarity = cardItem.Rarity;
				dreamType = 3;
                activate = true;
            }
			SetCardScriptActivate(activate);
        }
        public override void OnReleaseCard()
        {
            base.OnReleaseCard();
            DiceCardXmlInfo xmlData = card.card.XmlData;
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(EternalityInitializer.GetLorId(226769016), false);
            xmlData.workshopName = cardItem.Name;
            xmlData.DiceBehaviourList = cardItem.DiceBehaviourList;
            xmlData.Rarity = cardItem.Rarity;
			activate = false;
			dreamType = 0;
            SetCardScriptActivate(activate);
        }
        public override void OnEnterCardPhase(BattleUnitModel unit, BattleDiceCardModel self)
        {
            base.OnEnterCardPhase(unit, self);
            DiceCardXmlInfo xmlData = self.XmlData;
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(new LorId(EternalityInitializer.packageId, 226769016), false);
            xmlData.workshopName = cardItem.Name;
            xmlData.DiceBehaviourList = cardItem.DiceBehaviourList;
            xmlData.Rarity = cardItem.Rarity;
            activate = false;
            SetCardScriptActivate(activate);
        }
        public override void OnFirstMoon()
		{
			base.OnFirstMoon();
			if (activate)
				return;
			if (card.target.faction == owner.faction)
			{
				dreamType = 1;
				return;
			}
			dreamType = 0;
		}
        public override void OnSecondMoon()
        {

			if (!activate)
				return;
			dreamType = 4;
        }
		public override void OnFifthMoon()
		{
			base.OnFirstMoon();
			if (activate)
				return;
			dreamType = 2;
		}
        public override bool IsTargetableAllUnit()
        {
			return true;
        }
        public int dreamType = 0;
	}
	public class DiceCardSelfAbility_YagokoroCard2 : DiceCardAbilityBase
	{
        //描述用,效果全在 DiceCardSelfAbility_YagokoroCard1 里
	}
	public class DiceCardSelfAbility_YagokoroCard3 : MoonCardAbility
	{
		public override bool CanActivateMoon(int slot)
		{
			return slot >= 1 && slot <= 5;
		}
        public override List<string> GetMoonKeywords()
        {
			List<string> output = base.GetMoonKeywords();
			if (moonPreview == 0 || moonPreview == 5 || owner.bufListDetail.HasBuf<BattleUnitBuf_Moon3>())
				output.Add("CardBuf_YagokoroCardBuf1");
            return output;
        }
        public override string GetMoonAbilityText()
        {
            string id = "YagokoroCard3";
            switch (moonPreview)
            {
                case -1:
                    id = "YagokoroCard3_NoMoon";
                    break;
                case 1:
                    id = "YagokoroCard3_Moon1";
                    break;
                case 2:
                    id = "YagokoroCard3_Moon2";
                    break;
                case 3:
                    id = "YagokoroCard3_Moon3";
                    break;
                case 4:
                    id = "YagokoroCard3_Moon4";
                    break;
                case 5:
                    id = "YagokoroCard3_Moon5";
                    break;
            }
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
        }
        public override string GetFullMoonAbilityText()
        {
            string id = "YagokoroCard3";
            if (moonPreview != 0)
                id = "YagokoroCard3_FullMoon";
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
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
        public override string GetMoonAbilityText()
        {
            string id = "YagokoroCard4";
            switch (moonPreview)
            {
                case -1:
                    id = "YagokoroCard4_NoMoon";
                    break;
                case 3:
                    id = "YagokoroCard4_Moon3";
                    break;
                case 4:
                    id = "YagokoroCard4_Moon4";
                    break;
                case 5:
                    id = "YagokoroCard4_Moon5";
                    break;
            }
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
        }
        public override string GetFullMoonAbilityText()
        {
            string id = "YagokoroCard4";
            if (moonPreview != 0)
                id = "YagokoroCard4_FullMoon";
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
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
			base.OnApplyCard();
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
        public override string GetMoonAbilityText()
        {
            string id = "YagokoroCard5";
            switch (moonPreview)
            {
                case -1:
                    id = "YagokoroCard5_NoMoon";
                    break;
                case 1:
                    id = "YagokoroCard5_Moon1";
                    break;
                case 2:
                    id = "YagokoroCard5_Moon2";
                    break;
                case 3:
                    id = "YagokoroCard5_Moon3";
                    break;
            }
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
        }
        public override string GetFullMoonAbilityText()
        {
            string id = "YagokoroCard5";
            if (moonPreview != 0)
                id = "YagokoroCard5_FullMoon";
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
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
			base.OnApplyCard();
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
		private int count=0;
        public override string GetMoonAbilityText()
        {
            string id = "YagokoroCard6";
            switch (moonPreview)
            {
                case -1:
                    id = "YagokoroCard6_NoMoon";
                    break;
                case 1:
                    id = "YagokoroCard6_Moon1";
                    break;
                case 2:
                    id = "YagokoroCard6_Moon2";
                    break;
                case 3:
                    id = "YagokoroCard6_Moon3";
                    break;
                case 4:
                    id = "YagokoroCard6_Moon4";
                    break;
                case 5:
                    id = "YagokoroCard6_Moon5";
                    break;
            }
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
        }
        public override string GetFullMoonAbilityText()
        {
            string id = "YagokoroCard6";
            if (moonPreview != 0)
                id = "YagokoroCard6_FullMoon";
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
        }
        public override bool OnChooseCard(BattleUnitModel owner)
		{
			return Singleton<StageController>.Instance.RoundTurn >= 5;
		}
		public void ExhaustAndReturn()
		{
			this.card.card.exhaust = true;
			base.owner.bufListDetail.AddBuf(new BattleUnitBuf_addAfter(this.card.card.GetID(), 4));
		}
		public override void OnUseCard()
		{
			this.ExhaustAndReturn();
		}
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
			base.OnApplyCard();
			firstMoon = false;
			secondMoon = false;
			thirdMoon = false;
			forthMoon = false;
			fifthMoon = false;
		}
        public override void OnSucceedAreaAttack(BattleUnitModel target)
        {
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (firstMoon)
				{
					count++;
					if (count > 3)
						return;
                    battleUnitModel.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, owner);
                }
                if (fifthMoon)
                    battleUnitModel.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.BreakProtection, 1, owner);
            }
            if (secondMoon)
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Disarm, 1, owner);
            if (thirdMoon)
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Binding, 1, owner);
            if (forthMoon)
                target.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 1, owner);
        }
		private bool firstMoon;
		private bool secondMoon;
		private bool thirdMoon;
		private bool forthMoon;
		private bool fifthMoon;
	}
	public class DiceCardSelfAbility_YagokoroCard7 : MoonCardAbility
	{
        public override string GetMoonAbilityText()
        {
            string id = "YagokoroCard7";
            switch (moonPreview)
            {
                case -1:
                    id = "YagokoroCard7_NoMoon";
                    break;
                case 1:
                    id = "YagokoroCard7_Moon1";
                    break;
                case 3:
                    id = "YagokoroCard7_Moon3";
                    break;
                case 5:
                    id = "YagokoroCard7_Moon5";
                    break;
            }
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
        }
        public override string GetFullMoonAbilityText()
        {
            string id = "YagokoroCard7";
            if (moonPreview != 0)
                id = "YagokoroCard7_FullMoon";
            return string.Join("\n", BattleCardAbilityDescXmlList.Instance.GetAbilityDesc(id));
        }
        public override bool OnChooseCard(BattleUnitModel owner)
		{
			return Singleton<StageController>.Instance.RoundTurn >= 5 && base.OnChooseCard(owner);
		}
		public void ExhaustAndReturn()
		{
			this.card.card.exhaust = true;
			base.owner.bufListDetail.AddBuf(new BattleUnitBuf_addAfter(this.card.card.GetID(), 4));
		}
		public override void OnUseCard()
		{
			this.ExhaustAndReturn();
		}
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
			if(owner.passiveDetail.PassiveList.Find(x => x is PassiveAbility_226769005) is PassiveAbility_226769005
				MooonPassive && MooonPassive.IsActivate)
				return;
			owner.bufListDetail.AddReadyBuf(new YagokoroBuf12() { stack=0});
        }
    }
}
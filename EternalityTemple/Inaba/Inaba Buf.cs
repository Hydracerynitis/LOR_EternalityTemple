using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using EternalityTemple.Kaguya;

namespace EternalityTemple.Inaba
{
	public class BattleUnitBuf_InabaBuf1 : BattleUnitBuf
	{
		public override string keywordIconId => "QueenOfHatred_Hatred";
		public override string keywordId => "InabaBuf1";
		public BattleUnitBuf_InabaBuf1(BattleUnitModel model)
		{
			this._owner = model;
		}
		public void Add(int add)
		{
			this.stack += add;
			if (this.stack <= 0)
			{
				this.Destroy();
			}
		}
		public static void AddStack(BattleUnitModel model, int value)
		{
			BattleUnitBuf_InabaBuf1 battleUnitBuf = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_InabaBuf1) as BattleUnitBuf_InabaBuf1;
			if (battleUnitBuf == null)
			{
				battleUnitBuf = new BattleUnitBuf_InabaBuf1(model);
				battleUnitBuf.stack = value;
				model.bufListDetail.AddBuf(battleUnitBuf);
				return;
			}
			battleUnitBuf.Add(value);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0001429C File Offset: 0x0001249C
		public static int GetStack(BattleUnitModel model)
		{
			BattleUnitBuf_InabaBuf1 battleUnitBuf = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_InabaBuf1) as BattleUnitBuf_InabaBuf1;
			int result;
			if (battleUnitBuf == null)
			{
				result = 0;
			}
			else
			{
				result = battleUnitBuf.stack;
			}
			return result;
		}
	}

	public class BattleUnitBuf_InabaBuf2 : InabaBufAbility
	{
		public override string keywordIconId => "Roland_4th_Gaze";
		public override string keywordId => _owner.passiveDetail.HasPassive<PassiveAbility_226769010>() ? "InabaBuf2_self" : (_owner.passiveDetail.HasPassive<PassiveAbility_226769001>() ? "InabaBuf2_ally" : "InabaBuf2");

		public BattleUnitBuf_InabaBuf2(BattleUnitModel model)
		{
			this._owner = model;
		}
		public void Add(int add)
		{
			this.stack += add;
			if (this.stack <= 0)
			{
				this.Destroy();
			}
		}
		public static void AddStack(BattleUnitModel model, int value)
		{
			BattleUnitBuf_InabaBuf2 battleUnitBuf = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_InabaBuf2) as BattleUnitBuf_InabaBuf2;
			if (battleUnitBuf == null)
			{
				battleUnitBuf = new BattleUnitBuf_InabaBuf2(model);
				battleUnitBuf.stack = value;
				model.bufListDetail.AddBuf(battleUnitBuf);
				return;
			}
			battleUnitBuf.Add(value);
		}
		public static void AddReadyStack(BattleUnitModel model, int value)
		{
			BattleUnitBuf_InabaBuf2 battleUnitBuf = model.bufListDetail.GetReadyBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_InabaBuf2) as BattleUnitBuf_InabaBuf2;
			if (battleUnitBuf == null)
			{
				battleUnitBuf = new BattleUnitBuf_InabaBuf2(model);
				battleUnitBuf.stack = value;
				model.bufListDetail.AddReadyBuf(battleUnitBuf);
				return;
			}
			battleUnitBuf.Add(value);
		}
		// Token: 0x0600020D RID: 525 RVA: 0x0001429C File Offset: 0x0001249C
		public static int GetStack(BattleUnitModel model)
		{
			BattleUnitBuf_InabaBuf2 battleUnitBuf = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_InabaBuf2) as BattleUnitBuf_InabaBuf2;
			int result;
			if (battleUnitBuf == null)
			{
				result = 0;
			}
			else
			{
				result = battleUnitBuf.stack;
			}
			return result;
		}
		public override void OnAfterRollSpeedDice()
		{
			if(isEnemy())
            {
				return;
            }
			base.OnAfterRollSpeedDice();
			int count = this._owner.speedDiceResult.Count;
			int num = Mathf.Min(this.stack, count);
			for (int i = 0; i < num; i++)
			{
				int max = -1;
				int idx = -1;
				for (int j = 0; j < count; j++)
				{
					if (this._owner.speedDiceResult[j].value >= 1 && this._owner.speedDiceResult[j].isControlable)
					{
						if (this._owner.speedDiceResult[j].value > max)
						{
							idx = j;
							max = this._owner.speedDiceResult[j].value;
						}
					}
				}
				if (idx < 0)
				{
					break;
				}
				this._owner.SetCurrentOrder(idx);
				this._owner.speedDiceResult[idx].isControlable = false;
				List<BattleDiceCardModel> list = _owner.allyCardDetail.GetHand().FindAll((BattleDiceCardModel x) => x.GetSpec().Ranged != CardRange.Instance);
				if (list.Count <= 0)
				{
					break;
				}
				BattleDiceCardModel card = RandomUtil.SelectOne<BattleDiceCardModel>(list);
				_owner.allyCardDetail.GetHand().Remove(card);
				_owner.allyCardDetail.AddNewCardToDeck(card.GetID());
				DiceCardSelfAbilityBase diceCardSelfAbilityBase = card.CreateDiceCardSelfAbilityScript();
				if (diceCardSelfAbilityBase != null && diceCardSelfAbilityBase is InabaExtraCardAbility)
				{
					((InabaExtraCardAbility)diceCardSelfAbilityBase).OnInabaBuf();
				}
				card.AddBuf(new BattleDiceCardBuf_checkInaba());
				BattleUnitModel target = this.GetTarget_player();
				if (target != null)
				{
					int targetSlot = UnityEngine.Random.Range(0, target.speedDiceResult.Count);
					this._owner.cardSlotDetail.AddCard(card, target, targetSlot, false);
					if(target.cardSlotDetail.cardAry[targetSlot] != null && target.faction != _owner.faction)
                    {
						target.cardSlotDetail.cardAry[targetSlot].target = _owner;
						target.cardSlotDetail.cardAry[targetSlot].targetSlotOrder = idx;
					}
				}
			}
			SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
		}
        public override void OnStartBattle()
        {
            base.OnStartBattle();
			if(!isEnemy())
            {
				return;
            }
			int count = this._owner.speedDiceResult.Count;
			int num = Mathf.Min(this.stack, count);
			for (int i = 0; i < num; i++)
			{
				int max = -1;
				int idx = -1;
				for (int j = 0; j < count; j++)
				{
					if (this._owner.speedDiceResult[j].value >= 1 && this._owner.speedDiceResult[j].isControlable)
					{
						if (this._owner.speedDiceResult[j].value > max)
						{
							idx = j;
							max = this._owner.speedDiceResult[j].value;
						}
					}
				}
				if (idx < 0)
				{
					break;
				}
				BattleDiceCardModel card = _owner.cardSlotDetail.cardAry[idx].card;
				card.AddBuf(new BattleDiceCardBuf_checkInaba());
			}
		}
        public override void OnRoundEndTheLast()
		{
			base.OnRoundEndTheLast();
			this.Destroy();
		}
		private bool isEnemy()
        {
			return _owner.passiveDetail.HasPassive<PassiveAbility_226769010>() || _owner.passiveDetail.HasPassive<PassiveAbility_226769001>();

		}
		private BattleUnitModel GetTarget_player()
		{
			BattleUnitModel result = null;
			List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(false);
			aliveList.Remove(this._owner);
			aliveList.RemoveAll((BattleUnitModel x) => !x.IsTargetable(this._owner));
			if (aliveList.Count > 0)
			{
				result = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
			}
			return result;
		}
		public class BattleDiceCardBuf_checkInaba : BattleDiceCardBuf
		{
			public override void OnRoundEnd()
			{
				Destroy();
			}
            public override void OnUseCard(BattleUnitModel owner, BattlePlayingCardDataInUnitModel playingCard)
            {
                base.OnUseCard(owner, playingCard);
				playingCard.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus
				{
					power = 2
				});
			}
        }
	}
	public class BattleUnitBuf_InabaBuf4 : BattleUnitBuf
	{
		public override string keywordIconId => "Resistance";
		public override string keywordId => "InabaBuf4";
		public BattleUnitBuf_InabaBuf4(BattleUnitModel model)
		{
			this._owner = model;
		}
		public void Add(int add)
		{
			this.stack += add;
			if (this.stack <= 0)
			{
				this.Destroy();
			}
		}
		public static void AddStack(BattleUnitModel model, int value)
		{
			BattleUnitBuf_InabaBuf4 battleUnitBuf = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_InabaBuf4) as BattleUnitBuf_InabaBuf4;
			if (battleUnitBuf == null)
			{
				battleUnitBuf = new BattleUnitBuf_InabaBuf4(model);
				battleUnitBuf.stack = value;
				model.bufListDetail.AddBuf(battleUnitBuf);
				return;
			}
			battleUnitBuf.Add(value);
		}
		// Token: 0x0600020D RID: 525 RVA: 0x0001429C File Offset: 0x0001249C
		public static int GetStack(BattleUnitModel model)
		{
			BattleUnitBuf_InabaBuf4 battleUnitBuf = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_InabaBuf4) as BattleUnitBuf_InabaBuf4;
			int result;
			if (battleUnitBuf == null)
			{
				result = 0;
			}
			else
			{
				result = battleUnitBuf.stack;
			}
			return result;
		}
		public override int GetMinHp()
		{
			return (int)_owner.hp;
		}
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
			stack--;
			if(stack<=0)
			{ 
				Destroy();
			}
        }
    }
	public class BattleUnitBuf_InabaBuf5 : BattleUnitBuf
	{
		public override string keywordIconId => "BlackSilenceCardCount";
		public override string keywordId => "InabaBuf5_Txt" + stack.ToString();
		public BattleUnitBuf_InabaBuf5(BattleUnitModel model)
		{
			this._owner = model;
		}
		public void Add(int add)
		{
			this.stack += add;
			if (this.stack <= 0)
			{
				this.Destroy();
			}
		}
		public static void AddStack(BattleUnitModel model, int value)
		{
			BattleUnitBuf_InabaBuf5 battleUnitBuf = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_InabaBuf5) as BattleUnitBuf_InabaBuf5;
			if (battleUnitBuf == null)
			{
				battleUnitBuf = new BattleUnitBuf_InabaBuf5(model);
				battleUnitBuf.stack = value;
				model.bufListDetail.AddBuf(battleUnitBuf);
				return;
			}
			battleUnitBuf.Add(value);
		}
		// Token: 0x0600020D RID: 525 RVA: 0x0001429C File Offset: 0x0001249C
		public static int GetStack(BattleUnitModel model)
		{
			BattleUnitBuf_InabaBuf5 battleUnitBuf = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_InabaBuf5) as BattleUnitBuf_InabaBuf5;
			int result;
			if (battleUnitBuf == null)
			{
				result = 0;
			}
			else
			{
				result = battleUnitBuf.stack;
			}
			return result;
		}
		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			int num = stack;
			if (num <= 0)
			{
				return;
			}
			if (num >= 3)
			{
				num = 4;
			}
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				dmg = num
			});	
		}
		public override int GetDamageReduction(BattleDiceBehavior behavior)
		{
			int num = stack;
			if (num <= 0)
			{
				return 0;
			}
			if (num >= 3)
			{
				num = 4;
			}
			return num;
		}
	}
	// Token: 0x020007EA RID: 2026
	public class BattleUnitBuf_InabaBuf6 : BattleUnitBuf
	{
		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06002F10 RID: 12048 RVA: 0x000BD9B2 File Offset: 0x000BBBB2
		public override string keywordIconId => "BigBird_Sleep";
		public override string keywordId => "InabaBuf6";

		// Token: 0x06002F12 RID: 12050 RVA: 0x000BD9CB File Offset: 0x000BBBCB
		public override bool IsImmune(BufPositiveType posType)
		{
			return posType == BufPositiveType.Positive;
		}
		public override void OnRoundEnd()
		{
			this.Destroy();
		}
	}
	public class BattleUnitBuf_InabaBuf7 : BattleUnitBuf
	{
		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06002F10 RID: 12048 RVA: 0x000BD9B2 File Offset: 0x000BBBB2
		public override string keywordIconId => "WhiteNight_Awe";
		public override string keywordId => "InabaBuf7";

        // Token: 0x06002F12 RID: 12050 RVA: 0x000BD9CB File Offset: 0x000BBBCB
        public override void OnRoundStart()
        {
			BattleUnitBuf_InabaBuf2.AddStack(this._owner, 2);
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_opponent(this._owner.faction))
            {
				BattleUnitBuf_InabaBuf2.AddStack(battleUnitModel, 1);
			}
		}
		public BattleUnitBuf_InabaBuf7(BattleUnitModel model)
		{
			this._owner = model;
		}
		public void Add(int add)
		{
			this.stack += add;
			if (this.stack <= 0)
			{
				this.Destroy();
			}
		}
		public static void AddReadyStack(BattleUnitModel model, int value)
		{
			BattleUnitBuf_InabaBuf7 battleUnitBuf = model.bufListDetail.GetReadyBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_InabaBuf7) as BattleUnitBuf_InabaBuf7;
			if (battleUnitBuf == null)
			{
				battleUnitBuf = new BattleUnitBuf_InabaBuf7(model);
				battleUnitBuf.stack = value;
				model.bufListDetail.AddReadyBuf(battleUnitBuf);
				return;
			}
			battleUnitBuf.Add(value);
		}
		public override void OnRoundEnd()
		{
			stack--;
			if (this.stack <= 0)
			{
				this.Destroy();
			}
		}
	}
}
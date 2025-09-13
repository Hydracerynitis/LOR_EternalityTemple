using EternalityTemple.Kaguya;
using EternalityTemple.Yagokoro;
using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace EternalityTemple.Inaba
{
	public class BattleUnitBuf_InabaBuf1 : BattleUnitBuf //狂气
	{
		public override string keywordId => "InabaBuf1";
		public BattleUnitBuf_InabaBuf1(BattleUnitModel model)
		{
			this._owner = model;
			_bufIcon = EternalityInitializer.ArtWorks["Reisen_Buf狂气"];
			_iconInit = true;
		}
		public void Add(int add)
		{
			this.stack += add;
			if (this.stack < 0)
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
				battleUnitBuf.stack = value; //Debug 测试
                model.bufListDetail.AddBuf(battleUnitBuf);
				return;
			}
			battleUnitBuf.Add(value);
		}

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

	public class BattleUnitBuf_InabaBuf2 : InabaBufAbility //疯狂
	{
		public static Dictionary<BattleDiceCardModel, targetSetter> frenzyTargets = new Dictionary<BattleDiceCardModel, targetSetter>();
		public override string keywordId
		{
			get {
				if (_owner.passiveDetail.HasPassive<PassiveAbility_226769000>() // 蓬莱
					|| _owner.passiveDetail.HasPassive<PassiveAbility_226769005>() // 八意
						|| _owner.passiveDetail.HasPassive<PassiveAbility_226769010>()) // 铃仙
					return "InabaBuf2_ally";
				else
					return "InabaBuf2";
            }
		}

		public BattleUnitBuf_InabaBuf2(BattleUnitModel model)
		{
			this._owner = model;
			_bufIcon = EternalityInitializer.ArtWorks["Reisen_Buf疯狂"];
			_iconInit = true;
		}
		private void Add(int add)
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
		public static bool CheckFrenzy(BattleUnitModel owner,int idx)
		{
            int unavailable = owner.speedDiceResult.FindAll(x => x.breaked).Count;
            return idx < GetStack(owner) + unavailable;
		}
		public override void OnAfterRollSpeedDice()
		{
			if(frenzyTargets.Count > 0)
				frenzyTargets.Clear();
            if (isEnemy(_owner) || _owner.faction == Faction.Enemy)
                return;
            base.OnAfterRollSpeedDice();
			int count = this._owner.speedDiceResult.Count;
			int num = Mathf.Min(this.stack, count);
            int unavailable = _owner.speedDiceResult.FindAll(x => x.breaked).Count;
            for (int i = unavailable; i < num+unavailable; i++)
			{
				if (i < 0 || i >= _owner.speedDiceCount)
					continue;
                if (_owner.faction == Faction.Player)
				{
					this._owner.SetCurrentOrder(i);
					this._owner.speedDiceResult[i].isControlable = false;
				}
				List<BattleDiceCardModel> list = _owner.allyCardDetail.GetHand()
					.FindAll(x => x.GetSpec().Ranged != CardRange.Instance).FindAll(x => _owner.IsCardChoosable(x))
					.FindAll(x => x.GetSpec().Cost <= _owner.cardSlotDetail.PlayPoint)
					.FindAll(x => x.GetSpec().Ranged != CardRange.FarArea).FindAll(x => x.GetSpec().Ranged != CardRange.FarAreaEach);
				if (list.Count <= 0)
                    continue;
                BattleDiceCardModel card = RandomUtil.SelectOne(list);
				//_owner.allyCardDetail.ExhaustACardAnywhere(card);
				card.AddBuf(new InabaFrenzyActivate());
				BattleUnitModel target = GetTarget_player(_owner);
				if (target != null)
				{
					int targetSlot = UnityEngine.Random.Range(0, target.speedDiceResult.Count);
					_owner.cardSlotDetail.AddCard(card, target, targetSlot, false);
					if (target.cardSlotDetail.cardAry[targetSlot] != null && target.faction != _owner.faction)
					{
						target.cardSlotDetail.cardAry[targetSlot].target = _owner;
						target.cardSlotDetail.cardAry[targetSlot].targetSlotOrder = i;
					}
				}
			}
			SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
		}
		public override void OnStartBattle()
		{
			base.OnStartBattle();
            int count = this._owner.speedDiceResult.Count;
			int num = Mathf.Min(stack, count);
            int unavailable = _owner.speedDiceResult.FindAll(x => x.breaked).Count;
            for (int i = unavailable; i < num+unavailable; i++)
			{
				if (i >= _owner.speedDiceResult.Count)
					return;
                BattlePlayingCardDataInUnitModel card = _owner.cardSlotDetail.cardAry[i];
				if (card == null)
					continue;
				card.card.AddBuf(new InabaFrenzyActivate());
			}
		}
		public override void OnRoundEndTheLast()
		{
			EternalityInitializer.ResetSpeedDiceColor();
			this.Destroy();
		}
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int currentSlot)
        {
			BattleUnitModel result = null;
			List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(false);
			aliveList.Remove(this._owner);
			aliveList.RemoveAll((BattleUnitModel x) => !x.IsTargetable(this._owner));
			if (aliveList.Count > 0 && currentSlot < stack && !isEnemy(_owner) && _owner.faction == Faction.Enemy)
			{
				result = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
			}
			return result;
        }
        public static bool isEnemy(BattleUnitModel owner)
		{
			return owner.passiveDetail.HasPassive<PassiveAbility_226769010>() || owner.passiveDetail.HasPassive<PassiveAbility_226769000>() || owner.passiveDetail.HasPassive<PassiveAbility_226769005>();
		}
		public static BattleUnitModel GetTarget_player(BattleUnitModel owner)
		{
			BattleUnitModel result = null;
			List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(false);
			aliveList.Remove(owner);
			aliveList.RemoveAll((BattleUnitModel x) => !x.IsTargetable(owner));
			if (aliveList.Count > 0)
                result = RandomUtil.SelectOne(aliveList);
            return result;
		}
        public static BattleUnitModel GetTarget_enemy(BattleUnitModel owner)
		{
			BattleUnitModel result = null;
			List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(owner.faction);
			aliveList.RemoveAll((BattleUnitModel x) => !x.IsTargetable(owner));
			if (aliveList.Count > 0)
                result = RandomUtil.SelectOne(aliveList);
            return result;
		}
        public override bool DirectAttack()
        {
			return _owner.faction == Faction.Enemy;
        }
		public class InabaFrenzyActivate : BattleDiceCardBuf
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
		public class targetSetter
		{
			public BattleUnitModel target;
			public int targetSlot;
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
        public override bool IsImmuneDmg()
        {
			return true;
        }
        public override bool IsImmuneBreakDmg(DamageType type)
        {
            return true;
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
		public override string keywordIconId => "Reisen_Buf国士无双";
		public override string keywordId => "InabaBuf5_Txt" + stack.ToString();
		public BattleUnitBuf_InabaBuf5(BattleUnitModel model)
		{
			this._owner = model;
			_bufIcon = EternalityInitializer.ArtWorks["Reisen_Buf国士无双"];
			_iconInit = true;
		}
		public void Add(int add)
		{
			this.stack += add;
			if (stack >= 3)
				stack = 3;
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
	public class BattleUnitBuf_InabaBuf6 : BattleUnitBuf
	{
		public BattleUnitBuf_InabaBuf6()
        {
			_bufIcon = EternalityInitializer.ArtWorks["Reisen_Buf怠惰"];
			_iconInit = true;
		}
		public override string keywordIconId => "Reisen_Buf怠惰";
		public override string keywordId => "InabaBuf6";
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
		public override string keywordIconId => "Reisen_Buf狂视";
		public override string keywordId => "InabaBuf7";
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
			_bufIcon = EternalityInitializer.ArtWorks["Reisen_Buf狂视"];
			_iconInit = true;
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
		public static int GetStack(BattleUnitModel model)
		{
			BattleUnitBuf_InabaBuf7 battleUnitBuf = model.bufListDetail.GetActivatedBufList().Find((BattleUnitBuf x) => x is BattleUnitBuf_InabaBuf7) as BattleUnitBuf_InabaBuf7;
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
		public override void OnRoundEnd()
		{
			stack--;
			if (this.stack <= 0)
			{
				_owner.view.ChangeSkin("Reisen");
				this.Destroy();
			}
		}
	}
}
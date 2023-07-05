using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;

namespace EternalityTemple.Inaba
{
    public class BattleUnitBuf_InabaBuf1 : BattleUnitBuf
    {
        public override string keywordIconId => "QueenOfHatred_Hatred";
        public override string keywordId => "InabaBuf1";
    }

    public class BattleUnitBuf_InabaBuf2 : BattleUnitBuf
    {
        public override string keywordIconId => "RedHoodFinal_Berserk";
        public override string keywordId => "InabaBuf2";

		public override void OnAfterRollSpeedDice()
		{
			base.OnAfterRollSpeedDice();
			int count = this._owner.speedDiceResult.Count;
			int max = -1;
			int idx = -1;
			int num = Mathf.Min(this.stack, count);
			for (int i = 0; i < num; i++)
			{
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
				BattleDiceCardModel card = RandomUtil.SelectOne<BattleDiceCardModel>(list);
				_owner.allyCardDetail.GetHand().Remove(card);
				_owner.allyCardDetail.AddNewCardToDeck(card.GetID());
				BattleUnitModel target = this.GetTarget_player();
				if (_owner.passiveDetail.HasPassive<PassiveAbility_226769010>())
				{
					target = this.GetTarget_self();
				}
				if (target != null)
				{
					int targetSlot = UnityEngine.Random.Range(0, target.speedDiceResult.Count);
					this._owner.cardSlotDetail.AddCard(card, target, targetSlot, false);
				}
			}
			SingletonBehavior<BattleManagerUI>.Instance.ui_TargetArrow.UpdateTargetList();
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

		private BattleUnitModel GetTarget_self()
		{
			BattleUnitModel result = null;
			List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(this._owner.faction);
			aliveList.RemoveAll((BattleUnitModel x) => !x.IsTargetable(this._owner));
			if (aliveList.Count > 0)
			{
				result = RandomUtil.SelectOne<BattleUnitModel>(aliveList);
			}
			return result;
		}
	}
}
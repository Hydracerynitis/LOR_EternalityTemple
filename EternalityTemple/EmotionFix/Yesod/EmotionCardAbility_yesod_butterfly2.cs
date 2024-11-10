using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_yesod_butterfly2 : EmotionCardAbilityBase
    {
		private int Str
		{
			get
			{
				return RandomUtil.Range(1, 2);
			}
		}
		public override void OnRoundStart()
		{
			base.OnRoundStart();
			foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(false))
			{
				battleUnitModel.bufListDetail.AddBuf(new EmotionCardAbility_butterfly2.BattleUnitBuf_Emotion_Butterfly_DmgByDebuf());
			}
			if (base._owner.bufListDetail.GetNegativeBufTypeCount() <= 0)
			{
				if (base._owner.bufListDetail.GetReadyBufList().Find((BattleUnitBuf x) => x.positiveType == BufPositiveType.Negative) == null)
				{
					return;
				}
			}
			SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("2_Y/FX_IllusionCard_2_Y_Fly", 1f, base._owner.view, base._owner.view, 2f);
			base._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this.Str, base._owner);
		}
		private const int _strMin = 1;
		private const int _strMax = 2;
		public class BattleUnitBuf_Emotion_Butterfly_DmgByDebuf : BattleUnitBuf
		{
			private int DmgAdd
			{
				get
				{
					return RandomUtil.Range(2, 4);
				}
			}
			public override bool Hide
			{
				get
				{
					return true;
				}
			}
			public override int GetDamageReduction(BattleDiceBehavior behavior)
			{
				if (this._owner.bufListDetail.GetNegativeBufTypeCount() > 0)
				{
					return -this.DmgAdd;
				}
				return base.GetDamageReduction(behavior);
			}
			public override void BeforeTakeDamage(BattleUnitModel attacker, int dmg)
			{
				base.BeforeTakeDamage(attacker, dmg);
				if (this._owner.bufListDetail.GetNegativeBufTypeCount() > 0)
				{
					if (Singleton<StageController>.Instance.IsLogState())
					{
						BattleCardTotalResult battleCardResultLog = this._owner.battleCardResultLog;
						if (battleCardResultLog != null)
						{
							battleCardResultLog.SetCreatureAbilityEffect("2/ButterflyEffect_White", 1f);
						}
						BattleCardTotalResult battleCardResultLog2 = this._owner.battleCardResultLog;
						if (battleCardResultLog2 == null)
						{
							return;
						}
						battleCardResultLog2.SetCreatureEffectSound("Creature/ButterFlyMan_Atk_White");
						return;
					}
					else
					{
						SingletonBehavior<DiceEffectManager>.Instance.CreateCreatureEffect("2/ButterflyEffect_White", 1f, this._owner.view, this._owner.view, 1f);
						SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/ButterFlyMan_Atk_White", false, 1f, null);
					}
				}
			}
			public override void OnRoundEnd()
			{
				base.OnRoundEnd();
				this.Destroy();
			}
			private const int _dmgAddMin = 2;
			private const int _dmgAddMax = 4;
		}
	}
}

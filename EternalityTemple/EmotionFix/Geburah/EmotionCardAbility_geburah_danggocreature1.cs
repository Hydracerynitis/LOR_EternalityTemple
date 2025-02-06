using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_geburah_danggocreature1 : EmotionCardAbilityBase
    {
		public override void OnWaveStart()
		{
			base.OnWaveStart();
			this._nextWave = true;
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x0014093D File Offset: 0x0013EB3D
		public override void OnSelectEmotion()
		{
			base.OnSelectEmotion();
			base._owner.bufListDetail.AddBuf(new EmotionCardAbility_danggocreature1.BattleUnitBuf_Emotion_DanggoCreature_Healed());
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x0014095C File Offset: 0x0013EB5C
		public override void OnRoundStart()
		{
			base.OnRoundStart();
			if (this._nextWave)
			{
				return;
			}
			float num = (float)base._owner.MaxHp * 0.1f;
			int num2 = (int)((float)base._owner.UnitData.historyInWave.healed / num);
			num2 = Math.Min(3, num2);
			if (num2 > 0)
			{
				base._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, num2, base._owner);
			}
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x001409CC File Offset: 0x0013EBCC
		public override void OnKill(BattleUnitModel target)
		{
			base.OnKill(target);
			if (this._nextWave)
			{
				return;
			}
			if (target.faction != base._owner.faction)
			{
				float num = Mathf.Min(20f, 0.2f * (float)base._owner.MaxHp);
				base._owner.RecoverHP((int)num);
				BattleCardTotalResult battleCardResultLog = target.battleCardResultLog;
				if (battleCardResultLog != null)
				{
					battleCardResultLog.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_Meet", 2f);
				}
				BattleCardTotalResult battleCardResultLog2 = base._owner.battleCardResultLog;
				if (battleCardResultLog2 == null)
				{
					return;
				}
				battleCardResultLog2.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(this.KillEffect));
			}
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x00140A64 File Offset: 0x0013EC64
		public void KillEffect()
		{
			CameraFilterUtil.EarthQuake(0.18f, 0.16f, 90f, 0.45f);
			CreatureEffect creatureEffect = Resources.Load<CreatureEffect>("Prefabs/Battle/CreatureEffect/6/Dango_Emotion_Effect");
			if (creatureEffect != null)
			{
				CreatureEffect creatureEffect2 = UnityEngine.Object.Instantiate<CreatureEffect>(creatureEffect, SingletonBehavior<BattleSceneRoot>.Instance.transform);
				if (((creatureEffect2 != null) ? creatureEffect2.gameObject.GetComponent<AutoDestruct>() : null) == null)
				{
					AutoDestruct autoDestruct = (creatureEffect2 != null) ? creatureEffect2.gameObject.AddComponent<AutoDestruct>() : null;
					if (autoDestruct != null)
					{
						autoDestruct.time = 3f;
						autoDestruct.DestroyWhenDisable();
					}
				}
			}
			CreatureEffect creatureEffect3 = Resources.Load<CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
			if (creatureEffect3 != null)
			{
				CreatureEffect creatureEffect4 = UnityEngine.Object.Instantiate<CreatureEffect>(creatureEffect3, SingletonBehavior<BattleSceneRoot>.Instance.transform);
				if (((creatureEffect4 != null) ? creatureEffect4.gameObject.GetComponent<AutoDestruct>() : null) == null)
				{
					AutoDestruct autoDestruct2 = (creatureEffect4 != null) ? creatureEffect4.gameObject.AddComponent<AutoDestruct>() : null;
					if (autoDestruct2 != null)
					{
						autoDestruct2.time = 3f;
						autoDestruct2.DestroyWhenDisable();
					}
				}
			}
		}
		private bool _nextWave;
		public class BattleUnitBuf_Emotion_DanggoCreature_Healed : BattleUnitBuf
		{
			public override string keywordId
			{
				get
				{
					return "DangoCreature_Emotion_Healed";
				}
			}
			public override void Init(BattleUnitModel owner)
			{
				base.Init(owner);
				this.SetStack();
			}
			public override void OnRoundStart()
			{
				base.OnRoundStart();
				this.SetStack();
			}
			public override void OnRoundStartAfter()
			{
				base.OnRoundStartAfter();
				this.SetStack();
			}
			public override void OnRoundEndTheLast()
			{
				base.OnRoundEndTheLast();
				this.SetStack();
			}
			private void SetStack()
			{
				this.stack = this._owner.UnitData.historyInWave.healed;
			}
		}
	}
}

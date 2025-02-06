using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_geburah_danggocreature3 : EmotionCardAbilityBase
    {
		public override void OnSelectEmotion()
		{
			base.OnSelectEmotion();
			List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(base._owner.faction);
			aliveList.Remove(base._owner);
			this.stack = aliveList.Count;
			if (this.stack > 0)
			{
				foreach (BattleUnitModel battleUnitModel in aliveList)
				{
					battleUnitModel.Die(null, true);
				}
				base._owner.cardSlotDetail.SetRecoverPoint(base._owner.cardSlotDetail.GetRecoverPlayPoint() + this.stack);
			}
			Singleton<StageController>.Instance.GetStageModel().danggoUsed = true;
		}
		public override void OnWaveStart()
		{
			base.OnWaveStart();
			base._owner.cardSlotDetail.SetRecoverPoint(base._owner.cardSlotDetail.GetRecoverPlayPoint() + this.stack);
			this.MakeEffect("6/Dango_Emotion_Spread", 1f, base._owner, -1f, true);
		}
		public override int MaxPlayPointAdder()
		{
			return this.stack;
		}
		public override void OnRoundStart()
		{
			base.OnRoundStart();
			if (!this._effect)
			{
				this._effect = true;
				CameraFilterUtil.EarthQuake(0.18f, 0.16f, 90f, 0.45f);
				CreatureEffect creatureEffect = Resources.Load<CreatureEffect>("Prefabs/Battle/CreatureEffect/6/Dango_Emotion_Effect");
				if (creatureEffect != null)
				{
					CreatureEffect creatureEffect2 = UnityEngine.Object.Instantiate<CreatureEffect>(creatureEffect, SingletonBehavior<BattleSceneRoot>.Instance.transform);
					CreatureEffect creatureEffect3 = UnityEngine.Object.Instantiate<CreatureEffect>(creatureEffect, SingletonBehavior<BattleSceneRoot>.Instance.transform);
					CreatureEffect creatureEffect4 = UnityEngine.Object.Instantiate<CreatureEffect>(creatureEffect, SingletonBehavior<BattleSceneRoot>.Instance.transform);
					if (((creatureEffect2 != null) ? creatureEffect2.gameObject.GetComponent<AutoDestruct>() : null) == null)
					{
						AutoDestruct autoDestruct = (creatureEffect2 != null) ? creatureEffect2.gameObject.AddComponent<AutoDestruct>() : null;
						if (autoDestruct != null)
						{
							autoDestruct.time = 3f;
							autoDestruct.DestroyWhenDisable();
						}
					}
					if (((creatureEffect3 != null) ? creatureEffect3.gameObject.GetComponent<AutoDestruct>() : null) == null)
					{
						AutoDestruct autoDestruct2 = (creatureEffect3 != null) ? creatureEffect3.gameObject.AddComponent<AutoDestruct>() : null;
						if (autoDestruct2 != null)
						{
							autoDestruct2.time = 3f;
							autoDestruct2.DestroyWhenDisable();
						}
					}
					if (((creatureEffect4 != null) ? creatureEffect4.gameObject.GetComponent<AutoDestruct>() : null) == null)
					{
						AutoDestruct autoDestruct3 = (creatureEffect4 != null) ? creatureEffect4.gameObject.AddComponent<AutoDestruct>() : null;
						if (autoDestruct3 != null)
						{
							autoDestruct3.time = 3f;
							autoDestruct3.DestroyWhenDisable();
						}
					}
				}
				CreatureEffect creatureEffect5 = Resources.Load<CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
				if (creatureEffect5 != null)
				{
					CreatureEffect creatureEffect6 = UnityEngine.Object.Instantiate<CreatureEffect>(creatureEffect5, SingletonBehavior<BattleSceneRoot>.Instance.transform);
					CreatureEffect creatureEffect7 = UnityEngine.Object.Instantiate<CreatureEffect>(creatureEffect5, SingletonBehavior<BattleSceneRoot>.Instance.transform);
					CreatureEffect creatureEffect8 = UnityEngine.Object.Instantiate<CreatureEffect>(creatureEffect5, SingletonBehavior<BattleSceneRoot>.Instance.transform);
					if (((creatureEffect6 != null) ? creatureEffect6.gameObject.GetComponent<AutoDestruct>() : null) == null)
					{
						AutoDestruct autoDestruct4 = (creatureEffect6 != null) ? creatureEffect6.gameObject.AddComponent<AutoDestruct>() : null;
						if (autoDestruct4 != null)
						{
							autoDestruct4.time = 3f;
							autoDestruct4.DestroyWhenDisable();
						}
					}
					if (((creatureEffect7 != null) ? creatureEffect7.gameObject.GetComponent<AutoDestruct>() : null) == null)
					{
						AutoDestruct autoDestruct5 = (creatureEffect7 != null) ? creatureEffect7.gameObject.AddComponent<AutoDestruct>() : null;
						if (autoDestruct5 != null)
						{
							autoDestruct5.time = 3f;
							autoDestruct5.DestroyWhenDisable();
						}
					}
					if (((creatureEffect8 != null) ? creatureEffect8.gameObject.GetComponent<AutoDestruct>() : null) == null)
					{
						AutoDestruct autoDestruct6 = (creatureEffect8 != null) ? creatureEffect8.gameObject.AddComponent<AutoDestruct>() : null;
						if (autoDestruct6 != null)
						{
							autoDestruct6.time = 3f;
							autoDestruct6.DestroyWhenDisable();
						}
					}
				}
				this.MakeEffect("6/Dango_Emotion_Spread", 1f, base._owner, -1f, true);
				SoundEffectPlayer.PlaySound("Creature/Danggo_LvUp");
				SoundEffectPlayer.PlaySound("Creature/Danggo_Birth");
			}
			if (this.stack > 0)
			{
				base._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, this.stack, base._owner);
				base._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, this.stack, base._owner);
				base._owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, this.stack, base._owner);
			}
		}
		private int stack;
		private bool _effect;
	}
}

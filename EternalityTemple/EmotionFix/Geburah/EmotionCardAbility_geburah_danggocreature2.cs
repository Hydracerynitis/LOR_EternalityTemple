using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_geburah_danggocreature2 : EmotionCardAbilityBase
    {
		public override void OnRoundStart()
		{
			base.OnRoundStart();
			this.cnt = 0;
		}
		public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
		{
			base.OnTakeDamageByAttack(atkDice, dmg);
			if (this.cnt < 3 && this.CheckHP())
			{
				this.cnt++;
				foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList((base._owner.faction == Faction.Player) ? Faction.Enemy : Faction.Player))
				{
					if (!battleUnitModel.IsExtinction())
					{
						battleUnitModel.TakeBreakDamage(EmotionCardAbility_danggocreature2.BDmg, DamageType.Emotion, base._owner, AtkResist.Normal, KeywordBuf.None);
						battleUnitModel.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1, base._owner);
					}
				}
				BattleCardTotalResult battleCardResultLog = base._owner.battleCardResultLog;
				if (battleCardResultLog != null)
				{
					battleCardResultLog.SetNewCreatureAbilityEffect("6_G/FX_IllusionCard_6_G_Shout", 3f);
				}
				BattleCardTotalResult battleCardResultLog2 = base._owner.battleCardResultLog;
				if (battleCardResultLog2 == null)
				{
					return;
				}
				battleCardResultLog2.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(this.Damaged));
			}
		}
		public void Damaged()
		{
			CameraFilterUtil.EarthQuake(0.08f, 0.02f, 50f, 0.3f);
			SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Danggo_Lv2_Shout", false, 1f, null);
		}
		private bool CheckHP()
		{
			return base._owner.hp <= (float)base._owner.MaxHp * 0.5f;
		}
		private int cnt;
	}
}

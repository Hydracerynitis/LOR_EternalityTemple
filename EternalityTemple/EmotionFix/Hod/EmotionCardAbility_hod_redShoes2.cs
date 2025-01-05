using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_hod_redShoes2 : EmotionCardAbilityBase
    {
		public override void OnSelectEmotionOnce()
		{
			base.OnSelectEmotionOnce();
			SoundEffectPlayer.PlaySound("Creature/RedShoes_On");
		}
		public override int OnGiveKeywordBufByCard(BattleUnitBuf buf, int stack, BattleUnitModel target)
		{
			if (buf.bufType == KeywordBuf.Bleeding)
			{
				return stack;
			}
			return 0;
		}
		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			base.BeforeRollDice(behavior);
			base._owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend));
		}
	}
}

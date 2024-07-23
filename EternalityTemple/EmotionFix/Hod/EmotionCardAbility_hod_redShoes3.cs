using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using System.IO;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Hod
{
    public class EmotionCardAbility_hod_redShoes3 : EmotionCardAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = RandomUtil.Range(1, 2)
            });
        }

        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            base.OnLoseParrying(behavior);
            _owner.breakDetail.TakeBreakDamage(RandomUtil.Range(2, 5), DamageType.Passive, _owner);
        }

        public override void OnDrawParrying(BattleDiceBehavior behavior)
        {
            base.OnDrawParrying(behavior);
            if (behavior.Detail != BehaviourDetail.Slash)
                return;
            _owner.breakDetail.TakeBreakDamage(RandomUtil.Range(2, 5), DamageType.Emotion, _owner);
        }

        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            base.OnRollDice(behavior);
            if (behavior.Detail != BehaviourDetail.Slash)
                return;
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/RedShoes_SlashHit")?.SetGlobalPosition(_owner.view.WorldPosition);
            _owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Red));
        }
    }
}

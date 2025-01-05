using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_hod_spiderBud3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Spidermom_Down")?.SetGlobalPosition(_owner.view.WorldPosition);
            MakeEffect("3/Spider_RedEye", target: _owner, apply: false);
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (!behavior.IsParrying())
                return;
            BattleDiceBehavior targetDice = behavior?.TargetDice;
            if (targetDice == null)
                return;
            targetDice.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = -1
            });
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            if (RandomUtil.valueForProb > 0.5)
                return;
            behavior.card?.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Binding, 1, _owner);
            behavior.card?.target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Vulnerable, 1, _owner);
        }
    }
}

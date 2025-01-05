using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_yesod_murderer2 : EmotionCardAbilityBase
    {
        public override int GetSpeedDiceAdder() => -1000;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (!IsAttackDice(behavior.Detail))
                return;
            int enemy = RandomUtil.Range(1, 2);
            _owner.battleCardResultLog?.SetEmotionAbility(true, _emotionCard, 0, ResultOption.Sign, enemy);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = enemy
            });
        }
        public override void OnSelectEmotion()
        {
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Abandoned_Breathe")?.SetGlobalPosition(_owner.view.WorldPosition);
            _owner.view.charAppearance.SetTemporaryGift("Gift_AbandonedMurder", GiftPosition.Mouth);
        }
    }
}

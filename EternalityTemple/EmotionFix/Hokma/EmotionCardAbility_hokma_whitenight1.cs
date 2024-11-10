using Sound;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_whitenight1 : EmotionCardAbilityBase
    {
        private bool _effect;

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _owner.cardSlotDetail.SetRecoverPoint(2);
        }

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_effect)
                return;
            _effect = true;
            DiceEffectManager.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Wave", 1f, _owner.view, _owner.view, 5f);
            SoundEffectPlayer.PlaySound("Creature/WhiteNight_Call");
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.cardSlotDetail.SetRecoverPoint(2);
        }
    }
}

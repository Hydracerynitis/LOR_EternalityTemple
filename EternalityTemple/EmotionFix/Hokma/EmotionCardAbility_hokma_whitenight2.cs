using Sound;
using System;
using System.Collections.Generic;
using UnityEngine;
using Guard = EmotionCardAbility_whitenight2.BattleUnitBuf_Emotion_WhiteNight_Guard;

namespace EternalityEmotion
{
    public class EmotionCardAbility_hokma_whitenight2 : EmotionCardAbilityBase
    {
        private bool _effect;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _owner.bufListDetail.AddBuf(new Guard());
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_effect)
                return;
            _effect = true;
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/WhiteNight_Filter", false, 2f);
            SoundEffectPlayer.PlaySound("Creature/WhiteNight_Changing");
            SoundEffectPlayer.PlaySound("Creature/WhiteNight_Apostle_Whisper2");
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.bufListDetail.AddBuf(new Guard());
        }
    }
}

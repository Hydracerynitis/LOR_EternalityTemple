using System;
using Bell = EmotionCardAbility_silence2.BattleUnitBuf_Emotion_Silence_Bell;

namespace EmotionalFix.Hokma
{
    public class EmotionCardAbility_hokma_silence2 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            GiveBuf();
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            GiveBuf();
        }

        private void GiveBuf()
        {
            if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is Bell) != null)
                return;
            _owner.bufListDetail.AddBuf(new Bell());
        }
    }
}

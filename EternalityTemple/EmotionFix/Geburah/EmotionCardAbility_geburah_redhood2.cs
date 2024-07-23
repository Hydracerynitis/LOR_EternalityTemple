using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix.Geburah
{
    public class EmotionCardAbility_geburah_redhood2 : EmotionCardAbilityBase
    {
        private int DamageTaken;
        private int EnemyThreshold => (int)(_owner.MaxHp * 0.1);
        public override void OnWaveStart()
        {
            DamageTaken=0;
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            DamageTaken = 0;
            new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/RedHood_Filter", false, 2f);
            SoundEffectManager.Instance.PlayClip("Creature/RedHood_Change_mad");
        }
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEnd();
            int threshold;
            int strcount = 0;
            DamageTaken += _owner.history.takeDamageAtOneRound;
            threshold = EnemyThreshold;
            for (; DamageTaken > threshold; DamageTaken -= threshold)
                strcount += 1;
            _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, Math.Min(strcount,4), _owner);
        }
    }
}

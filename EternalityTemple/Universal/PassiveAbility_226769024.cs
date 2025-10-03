using System.Collections.Generic;
using UnityEngine;

namespace EternalityTemple
{
    public class PassiveAbility_226769024 : PassiveAbility_226769023
    {
        public int teamEmotionLevel = 0;
        public List<EmotionCardXmlInfo> emotionFloor = new List<EmotionCardXmlInfo>();
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            AbnormalityAttribution.ResetEmotionDeck();
        }
        public override void OnRoundEndTheLast_ignoreDead()
        {
            while (Singleton<StageController>.Instance.GetCurrentWaveModel().team.emotionLevel > teamEmotionLevel)
            {
                teamEmotionLevel++;
                AbnormalityAttribution.PickEmotionCard();
            }
        }
    }
}

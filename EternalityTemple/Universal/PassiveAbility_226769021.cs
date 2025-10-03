using System;
using System.Collections.Generic;
using UnityEngine;

namespace EternalityTemple
{
    public class PassiveAbility_226769021 : PassiveAbility_226769019
    {
        public int teamEmotionLevel = 0;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            AbnormalityAttribution.ResetEmotionDeck();
        }
        public override void OnRoundStart()
        {
            if (Singleton<StageController>.Instance.CurrentFloor == SephirahType.Netzach)
            {
                foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(owner.faction))
                {
                    foreach (BattleDiceCardModel battleDiceCardModel in battleUnitModel.allyCardDetail.GetAllDeck())
                    {
                        if (battleDiceCardModel.GetID() == 407007)
                            battleDiceCardModel.XmlData.Script = "ally2strength1thisRound";
                    }
                }
            }
        }
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
        {
            return BattleObjectManager.instance.GetAliveList(Faction.Player).Find((BattleUnitModel x) => x.bufListDetail.HasBuf<EmotionCardAbility_alriune3.BattleUnitBuf_Emotion_Alriune>());
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

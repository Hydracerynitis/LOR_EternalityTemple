using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using Sin = EmotionCardAbility_longbird2.BattleUnitBuf_LongBird_Emotion_Sin;

namespace EmotionalFix.Binah
{
    public class EmotionCardAbility_binah_longbird2 : EmotionCardAbilityBase
    {
        private bool Prob => RandomUtil.valueForProb <= 0.5;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (!Prob)
                    continue;
                BattleUnitBuf buf = alive.bufListDetail.GetActivatedBufList().Find(x => x is Sin);
                if (buf == null)
                {
                    buf = new Sin();
                    alive.bufListDetail.AddBuf(buf);
                }
                ++buf.stack;
            }
        }
    }
}

using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EternalityEmotion
{
    public class EmotionCardAbility_keter_snowqueen2 : EmotionCardAbilityBase
    {
        private int cnt;

        public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
        {
            base.OnParryingStart(card);
            cnt = 0;
        }

        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null)
                return;
            ++cnt;
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_IceUnATK", 2f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/SnowQueen_Guard");
            if (cnt != 2)
                return;
            EmotionCardAbility_snowqueen2.BattleUnitBuf_Emotion_SnowQueen_Shard buf =
                target.bufListDetail.GetActivatedBufList().Find(x => x.GetType()==typeof(EmotionCardAbility_snowqueen2.BattleUnitBuf_Emotion_SnowQueen_Shard)) 
                    as EmotionCardAbility_snowqueen2.BattleUnitBuf_Emotion_SnowQueen_Shard;
            if (buf == null)
            {
                buf = new EmotionCardAbility_snowqueen2.BattleUnitBuf_Emotion_SnowQueen_Shard(_owner);
                target.bufListDetail.AddBuf(buf);
            }
            buf.Add();
        }

        public override void OnEndParrying(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnEndParrying(curCard);
            cnt = 0;
        }
    }
}

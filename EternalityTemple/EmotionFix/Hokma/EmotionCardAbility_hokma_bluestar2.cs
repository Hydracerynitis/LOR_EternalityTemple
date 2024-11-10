using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_hokma_bluestar2 : EmotionCardAbilityBase
    {
        private bool triggered;
        private int Dmg => RandomUtil.Range(3, 7);
        public override void OnRoundEndTheLast()
        {
            base.OnRoundEndTheLast();
            triggered = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive.IsBreakLifeZero() && !triggered)
                {
                    triggered = true;
                    _owner.TakeDamage(Dmg, DamageType.Emotion, _owner);
                    SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_JudgementExplo", 1f, alive.view, alive.view, 2f);
                    SoundEffectPlayer.PlaySound("Creature/BlueStar_Suicide");
                    foreach (BattleUnitModel ally in BattleObjectManager.instance.GetAliveList(Faction.Player))
                        ally.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, _owner);
                }
            }
            if (!triggered)
                return;
        }
    }
}

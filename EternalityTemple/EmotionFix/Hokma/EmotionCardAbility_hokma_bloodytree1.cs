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
    public class EmotionCardAbility_hokma_bloodytree1 : EmotionCardAbilityBase
    {
        public override void OnGiveDeflect(BattleDiceBehavior behavior)
        {
            int num = (int)(behavior.DiceResultValue * 0.5);
            behavior.card?.target?.TakeDamage(num, DamageType.Emotion, _owner);
            _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("9_H/FX_IllusionCard_9_H_StickPiercing", 2f);
        }
    }
}

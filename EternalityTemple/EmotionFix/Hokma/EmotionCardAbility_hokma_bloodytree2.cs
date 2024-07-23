using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix.Hokma
{
    public class EmotionCardAbility_hokma_bloodytree2 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            DiceEffectManager.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Eye", 1f, _owner.view, _owner.view);
            SoundEffectPlayer.PlaySound("Creature/MustSee_Wake_Storng");
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            DiceEffectManager.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Eye", 1f, _owner.view, _owner.view);
        }

        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (!behavior.IsParrying())
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = RandomUtil.Range(2,4)
                });
            }
            else
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    dmg = RandomUtil.Range(3, 6)
                });
                if (behavior.Detail != BehaviourDetail.Guard)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = RandomUtil.Range(1, 3)
                });
            }
        }
    }
}

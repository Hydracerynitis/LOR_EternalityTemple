using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_chesed_scarecrow1 : EmotionCardAbilityBase
    {
        List<BattleDiceBehavior> usedDice=new List<BattleDiceBehavior>();
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            SetFilter();
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = -RandomUtil.Range(1, 3) });
        }
        public override void AfterDiceAction(BattleDiceBehavior behavior)
        {
            if (usedDice.Contains(behavior))
                return;
            usedDice.Add(behavior);
            if (RandomUtil.valueForProb <= 0.25)
            {
                behavior.isBonusAttack = true;
                _owner.battleCardResultLog?.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(PrintSound));
            }      
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            usedDice.Clear();
        }
        private void PrintSound() => SoundEffectManager.Instance.PlayClip("Creature/Scarecrow_Special");
        private void SetFilter()
        {
            GameObject gameObject = BattleCamManager.Instance.EffectCam.gameObject;
            if (!(gameObject != null))
                return;
            CameraFilterPack_Distortion_ShockWave distortionShockWave = gameObject.AddComponent<CameraFilterPack_Distortion_ShockWave>();
            distortionShockWave.PosX = 0.5f;
            distortionShockWave.PosY = 0.5f;
            distortionShockWave.Speed = 1.2f;
            AutoScriptDestruct autoScriptDestruct = BattleCamManager.Instance?.EffectCam.gameObject.AddComponent<AutoScriptDestruct>() ?? null;
            if (!(autoScriptDestruct != null))
                return;
            autoScriptDestruct.targetScript = distortionShockWave;
            autoScriptDestruct.time = 1.5f;
        }
    }
}

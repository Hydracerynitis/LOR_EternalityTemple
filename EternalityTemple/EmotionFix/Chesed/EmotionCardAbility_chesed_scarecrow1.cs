using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EternalityEmotion
{
    public class EmotionCardAbility_chesed_scarecrow1 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            SetFilter();
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            _owner.allyCardDetail.DisCardACardRandom();
            _owner.cardSlotDetail.RecoverPlayPoint(2);
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

using System;
using LOR_DiceSystem;
using System.Collections;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_keter_heart1 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            BattleCamManager.Instance?.StartCoroutine(Pinpong(BattleCamManager.Instance?.AddCameraFilter<CameraFilterPack_Blur_Radial>()));
        }

        public override void OnSelectEmotionOnce()
        {
            base.OnSelectEmotionOnce();
            SoundEffectPlayer.PlaySound("Creature/Heartbeat");
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, RandomUtil.Range(1,2));

        }
        private IEnumerator Pinpong(CameraFilterPack_Blur_Radial r)
        {
            float elapsedTime = 0.0f;
            while ((double)elapsedTime < 1.0)
            {
                elapsedTime += Time.deltaTime;
                r.Intensity = Mathf.PingPong(Time.time, 0.05f);
                yield return new WaitForEndOfFrame();
            }
            BattleCamManager.Instance?.RemoveCameraFilter<CameraFilterPack_Blur_Radial>();
        }
        public override void OnRoundEnd()
        {
            if (_owner.history.damageAtOneRound > 0)
                return;
            _owner.LoseHp((int)(_owner.MaxHp * 0.05));
        }
    }
}

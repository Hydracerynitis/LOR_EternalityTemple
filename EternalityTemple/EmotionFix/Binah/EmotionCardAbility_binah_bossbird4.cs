using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Reflection;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using EternalityTemple;

namespace EternalityEmotion
{
    public class EmotionCardAbility_binah_bossbird4 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            this.GiveBuf();
            GameObject gameObject = Util.LoadPrefab("Battle/CreatureEffect/FinalBattle/BinahFinalBattle_ImageFilter");
            if (gameObject == null)
                return;
            Creature_Final_Binah_ImageFilter component = gameObject?.GetComponent<Creature_Final_Binah_ImageFilter>();
            if (component != null)
                component.Init(4);
            gameObject.AddComponent<AutoDestruct>().time = 10f;
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            GiveBuf();
        }

        private void GiveBuf()
        {
            if (!CheckCondition())
                return;
            PlatformManager.Instance.UnlockAchievement(AchievementEnum.ONCE_FLOOR8);
            _owner.bufListDetail.AddBuf(new EmotionCardAbility_bossbird4.BattleUnitBuf_Emotion_BossBird_Apocalypse());
        }

        private bool CheckCondition()
        {
            if(Helper.SearchEmotion(_owner, "ApocalypseBird_LongArm_Enemy")==null)
                return false;
            if (Helper.SearchEmotion(_owner, "ApocalypseBird_BigEye_Enemy") == null)
                return false;
            if (Helper.SearchEmotion(_owner, "ApocalypseBird_SmallPeak_Enemy") == null)
                return false;
            return true;
        }          
    }
}

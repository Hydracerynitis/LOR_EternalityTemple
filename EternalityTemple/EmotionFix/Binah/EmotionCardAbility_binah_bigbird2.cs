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
    public class EmotionCardAbility_binah_bigbird2 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            DiceEffectManager.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_Lamp", 1f, _owner.view, _owner.view, 3f);
            SoundEffectPlayer.PlaySound("Creature/Bigbird_Attract");
            _owner.bufListDetail.AddBuf(new Charm());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.bufListDetail.AddBuf(new Charm());
        }
        public class Charm: BattleUnitBuf
        {
            public override bool IsTauntable()
            {
                return false;
            }
        }
    }
}

using System;
using Sound;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_chesed_ozma3 : EmotionCardAbilityBase
    {
        private bool _effect;
        private bool _activated;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _effect = false;
        }
        public override void OnRoundStart()
        {
            if (!_effect)
            {
                _effect = true;
                DiceEffectManager.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Particle", 1f, _owner.view, _owner.view, 3f);
            }
            if (_activated)
                _owner.bufListDetail.GetActivatedBufList().Find(x => x is ReviveIndicator)?.Destroy();
            else
            {
                if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is ReviveIndicator) != null)
                    return;
                _owner.bufListDetail.AddBuf(new ReviveIndicator());
            }
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (_activated || (double)_owner.hp > dmg)
                return false;
            _activated = true;
            _owner.RecoverHP(_owner.MaxHp/2);
            _owner.breakDetail.RecoverBreakLife(_owner.MaxBreakLife);
            _owner.breakDetail.nextTurnBreak = false;
            _owner.breakDetail.RecoverBreak(_owner.breakDetail.GetDefaultBreakGauge());
            if (StageController.Instance.IsLogState())
            {
                _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Particle", 3f);
                _owner.battleCardResultLog?.SetCreatureEffectSound("CreatureOzma_FarAtk");
            }
            else
            {
                DiceEffectManager.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Particle", 1f, _owner.view, _owner.view, 3f);
                SoundEffectPlayer.PlaySound("CreatureOzma_FarAtk");
            }
            return true;
        }
        public class ReviveIndicator : BattleUnitBuf
        {
            public override string keywordId => "EF_OzmaRevive";
            public override string keywordIconId => "Ozma_AwakenPumpkin";
            public ReviveIndicator() => stack = 0;
        }
    }
}

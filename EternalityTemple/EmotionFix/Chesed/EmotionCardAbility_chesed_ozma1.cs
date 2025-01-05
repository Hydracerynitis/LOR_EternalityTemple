using System;
using Sound;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityEmotion
{
    public class EmotionCardAbility_chesed_ozma1 : EmotionCardAbilityBase
    {
        private bool _effect;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_effect)
                return;
            _effect = true;
            DiceEffectManager.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Jack_Start", 1f, _owner.view, _owner.view, 2f);
            SoundEffectPlayer.PlaySound("Creature/Ozma_RealPumkin_GetCard");
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            int num = RandomUtil.Range(3, 4);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = num
            });
            if (IsAttackDice(behavior.Detail))
                _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Jack_P", 2f);
            else
                _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Jack_G", 2f);
        }
        public override int GetCardCostAdder(BattleDiceCardModel card) => 1;
    }
}

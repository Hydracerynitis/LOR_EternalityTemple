using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_malkuth_fairy3 : EmotionCardAbilityBase
    {
        private int Threshold => (int)(_owner.MaxHp * 0.05);
        private int HealThreshold => (int)(_owner.MaxHp * 0.2);
        private int _healing;
        private bool _hungry;
        public override void OnSelectEmotion()
        {
            _healing = 0;
        }
        public override void CheckDmg(int dmg, BattleUnitModel target)
        {
            int heal = dmg / 2;
            if (_healing >= HealThreshold)
                return;
            if (heal + _healing >= HealThreshold)
                heal = heal + _healing - HealThreshold;
            _healing += heal;
            _owner.RecoverHP(heal);
        }
        public override void OnRoundStart()
        {
            _hungry = false;
            if (_healing < Threshold)
            {
                int dmg = (int)(_owner.MaxHp * 0.05);
                _owner.TakeDamage(Mathf.Min(dmg, 12));
                SoundEffectPlayer.PlaySound("Creature/Fairy_QueenEat");
                SoundEffectPlayer.PlaySound("Creature/Fairy_QueenChange");
                SetFilter("1/Fairy_Filter");
                _hungry = true;
            }
            _healing = 0;
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            if (!_hungry)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmgRate = 30
            });
        }
    }
}

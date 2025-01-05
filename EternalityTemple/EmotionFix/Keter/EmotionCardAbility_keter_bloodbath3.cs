using System;
using Battle.CreatureEffect;
using LOR_DiceSystem;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EternalityTemple;

namespace EternalityEmotion
{
    public class EmotionCardAbility_keter_bloodbath3 : EmotionCardAbilityBase
    {
        private BattleUnitModel _target;
        private int _stack;
        public override int GetCounter() => _stack;
        public override void OnRollDice(BattleDiceBehavior behavior)
        {
            base.OnRollDice(behavior);
            if (!IsAttackDice(behavior.Detail) || _target == null || _target == behavior.card.target)
                return;
            if (_target.bufListDetail.GetActivatedBufList().Find(x => x is BloodBathHand) is BloodBathHand bloodBathHandDebuf)
                _target.bufListDetail.RemoveBuf(bloodBathHandDebuf);
            _target = null;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (_target != behavior.card.target)
            {
                _target = behavior.card.target;
                _stack = 1;
                BloodBathHand bloodBathHandDebuf = Helper.AddBuf<BloodBathHand>(_target, 0);
                bloodBathHandDebuf.OnHit();
            }
            else
            {
                ++_stack;
                if (_target.bufListDetail.GetActivatedBufList().Find(x => x is BloodBathHand) is BloodBathHand bloodBathHandDebuf)
                    bloodBathHandDebuf.OnHit();
                if (_stack < 3)
                    return;
                Ability();
            }
        }

        private void Ability()
        {
            if (_target == null)
                return;
            _target.TakeBreakDamage(RandomUtil.Range(3, 12), DamageType.Emotion, _owner);
            if (_target.bufListDetail.GetActivatedBufList().Find((x => x is BloodBathHand)) is BloodBathHand bloodBathHandDebuf)
                _target.bufListDetail.RemoveBuf(bloodBathHandDebuf);
            _target.battleCardResultLog?.SetCreatureAbilityEffect("0/BloodyBath_PaleHand_Hit", 3f);
            _target = null;
            _stack = 0;
        }
        public void Destroy()
        {
            if (_target == null)
                return;
            if (_target.bufListDetail.GetActivatedBufList().Find((x => x is BloodBathHand)) is BloodBathHand bloodBathHandDebuf)
                _target.bufListDetail.RemoveBuf(bloodBathHandDebuf);
            _target = null;
            _stack = 0;
        }

        public class BloodBathHand : BattleUnitBuf
        {
            public override string keywordIconId => "BloodBath_Hand";
            public override string keywordId => "EF_BloodbathHands";
            public BloodBathHand() => stack = 0;
            public void OnHit() => ++stack;
        }
    }
}

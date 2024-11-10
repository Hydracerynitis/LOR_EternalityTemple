using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_malkuth_fairy1 : EmotionCardAbilityBase
    {
        private int _takendmg;
        private int DmgThreshold => (int)(_owner.MaxHp * 0.25);
        private CreatureEffect_Anim _effect;
        private bool _destroy;
        private int _count = 3;
        private int _hitcount;
        public override void OnSelectEmotion()
        {
            try
            {
                _effect = MakeEffect("1/Fairy_Gluttony") as CreatureEffect_Anim;
                _effect?.SetLayer("Character");
                SoundEffectManager.Instance.PlayClip("Creature/Fariy_Special")?.SetGlobalPosition(_owner.view.WorldPosition);
            }
            catch 
            {

            }
        }
        public override void OnRoundEnd()
        {

            if (_destroy)
                return;
            _owner.RecoverHP((int)(_owner.MaxHp * 0.15));
            _count -= 1;
            if (_count > 0)
                return;
            _destroy = true;
            if (_effect != null)
                _effect.SetTrigger("Disappear");
            _effect = null;
        }
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if (_takendmg >= DmgThreshold || _destroy)
                return false;
            _takendmg+=dmg;
            _hitcount = 5;
            if (_takendmg >= DmgThreshold)
            {
                int dmg1 = _owner.LoseHp((int)(_owner.MaxHp * 0.3));
                _owner.battleCardResultLog?.SetDamageTaken(dmg1, dmg, BehaviourDetail.Slash);
                _owner.battleCardResultLog?.SetEmotionAbility(true, _emotionCard, 1, ResultOption.Default, dmg1);
                _destroy = true;
            }
            if (_effect != null)
                ApplyCreatureEffect(_effect);
            _owner?.battleCardResultLog?.SetCreatureEffectSound("Creature/Fairy_Dead");
            return false;
        }

        public override void OnPrintEffect(BattleDiceBehavior behavior)
        {
            if (_hitcount > 0)
            {
                if (_effect != null)
                    _effect.SetTrigger("Hit");
                --_hitcount;
            }
            if (!_destroy)
                return;
            if (_effect != null)
                _effect.SetTrigger("Disappear");
            _effect = null;
        }

        public override void OnLayerChanged(string layerName)
        {
            try
            {
                if (_effect == null)
                    return;
                _effect.SetLayer(layerName);
            }
            catch
            {

            }
        }
    }
}

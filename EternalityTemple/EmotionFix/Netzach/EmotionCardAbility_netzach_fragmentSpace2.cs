using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix.Netzach
{
    public class EmotionCardAbility_netzach_fragmentSpace2 : EmotionCardAbilityBase
    {
        private List<BattleUnitModel> victim = new List<BattleUnitModel>();
        private Battle.CreatureEffect.CreatureEffect _hitEffect;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior.card.card.XmlData.Spec.Ranged == CardRange.FarArea || behavior.card.card.XmlData.Spec.Ranged == CardRange.FarAreaEach)
                return;
            if (victim.Contains(behavior.card.target))
                return;
            _hitEffect = MakeEffect("4/Fragment_Hit", destroyTime: 1f,target: behavior.card.target);
            _hitEffect?.gameObject.SetActive(false);
            if (behavior.card.target == null)
                return;
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Cosmos_Hit")?.SetGlobalPosition(behavior.card.target.view.WorldPosition);
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (victim.Contains(behavior.card.target))
                return;
            behavior.card.target.TakeBreakDamage(behavior.card.target.breakDetail.GetDefaultBreakGauge()/10);
            victim.Add(behavior.card.target);
        }
        public override void OnRoundEnd()
        {
            victim.Clear();
        }
    }
}

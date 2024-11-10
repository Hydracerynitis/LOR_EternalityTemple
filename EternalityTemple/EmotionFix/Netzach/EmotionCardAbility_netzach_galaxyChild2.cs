using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_netzach_galaxyChild2 : EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect _effect;
        private List<Battle.CreatureEffect.CreatureEffect> _damagedEffects = new List<Battle.CreatureEffect.CreatureEffect>();
        public override void OnBreakState()
        {
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive == _owner || alive.IsBreakLifeZero())
                    continue;
                int num=(int) (alive.breakDetail.GetDefaultBreakGauge()*0.4);
                alive.TakeBreakDamage(num);
                alive.view.BreakDamaged(num, BehaviourDetail.Penetrate, _owner, AtkResist.Normal);
                Battle.CreatureEffect.CreatureEffect creatureEffect = DiceEffectManager.Instance.CreateCreatureEffect("4/GalaxyBoy_Damaged", 1f, alive.view, (BattleUnitView)null, 2f);
                creatureEffect.SetLayer("Character");
                _damagedEffects.Add(creatureEffect);
                creatureEffect.gameObject.SetActive(false);
                creatureEffect.SetLayer("Effect");
            }
            _effect = MakeEffect("4/GalaxyBoy_Dust", destroyTime: 3f);
            _effect?.gameObject.SetActive(false);
            _effect?.AttachEffectLayer();
            SoundEffectPlayer.PlaySound("Creature/GalaxyBoy_Cry");
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 1 });
        }
        public override void OnPrintEffect(BattleDiceBehavior behavior)
        {
            if (_effect==null)
                return;
            _effect.gameObject.SetActive(true);
            _effect = null;
            foreach (Component damagedEffect in _damagedEffects)
                damagedEffect.gameObject.SetActive(true);
            _damagedEffects.Clear();
        }
    }
}

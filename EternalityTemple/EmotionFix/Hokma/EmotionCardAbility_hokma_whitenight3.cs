using System;
using UnityEngine;

namespace EmotionalFix.Hokma
{
    public class EmotionCardAbility_hokma_whitenight3 : EmotionCardAbilityBase
    {
        private bool _effect;
        private GameObject _aura;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _effect = false;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!_effect)
            {
                _effect = true;
                if (_aura == null)
                    _aura = DiceEffectManager.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Power", 1f, _owner.view, _owner.view)?.gameObject;
            }
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
                if (alive != _owner)
                    alive.bufListDetail.AddBuf(new Mighty(_owner));
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            DestroyAura();
            _aura = DiceEffectManager.Instance.CreateNewFXCreatureEffect("9_H/FX_IllusionCard_9_H_Power", 1f, _owner.view, _owner.view)?.gameObject;
        }

        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        public override void OnEndBattlePhase()
        {
            base.OnEndBattlePhase();
            DestroyAura();
        }
        public void DestroyAura()
        {
            if (_aura == null)
                return;
            UnityEngine.Object.Destroy(_aura);
            _aura = null;
        }
        public class Mighty : BattleUnitBuf
        {
            private BattleUnitModel _god;
            public override bool Hide => true;
            public Mighty(BattleUnitModel god) => _god = god;
            public override void OnDieOtherUnit(BattleUnitModel unit)
            {
                base.OnDieOtherUnit(unit);
                if (unit == null || unit != _god)
                    return;
                Destroy();
            }
            public override double ChangeDamage(BattleUnitModel attacker, double dmg)
            {
                if (dmg > 3)
                    return base.ChangeDamage(attacker, dmg);
                _owner.RecoverHP((int)dmg);
                return 0.0;
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}

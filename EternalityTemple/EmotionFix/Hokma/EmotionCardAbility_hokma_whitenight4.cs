using System.Collections.Generic;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_hokma_whitenight4 : EmotionCardAbilityBase
    {
        private bool _effect;
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
                GameObject gameObject = Util.LoadPrefab("Battle/CreatureEffect/FinalBattle/WhiteNight_DeadApostleImageFilter");
                if (gameObject != null)
                {
                    WhiteNightApostleDeadFilter component = gameObject?.GetComponent<WhiteNightApostleDeadFilter>();
                    if (component != null)
                        component.Init(11, 12, null,null, null);
                    AutoDestruct autoDestruct = gameObject.AddComponent<AutoDestruct>();
                    if (autoDestruct != null)
                    {
                        autoDestruct.time = 2.5f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
            }
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
            {
                if (alive != _owner)
                {
                    Apostal whiteNightApostle = new Apostal(_owner);
                    alive.bufListDetail.AddBuf(whiteNightApostle);
                }
            }
        }
        public class Apostal : BattleUnitBuf
        {
            private BattleUnitModel _god;
            public override bool Hide => true;
            public Apostal(BattleUnitModel god) => _god = god;
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (_god == null || _god.IsDead())
                    return;
                if (behavior != null)
                    behavior.ApplyDiceStatBonus(new DiceStatBonus()
                    {
                        dmg = RandomUtil.Range(2, 7)
                    });
                if (EmotionCardAbility_hokma_plaguedoctor1.WhiteNightClock.ContainsKey(_god.UnitData) || EmotionCardAbility_hokma_plaguedoctor1.WhiteNightClock[_god.UnitData]<12)
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = 1
                });
            }
            public override void OnDieOtherUnit(BattleUnitModel unit)
            {
                base.OnDieOtherUnit(unit);
                if (unit == null || unit != _god)
                    return;
                Destroy();
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}

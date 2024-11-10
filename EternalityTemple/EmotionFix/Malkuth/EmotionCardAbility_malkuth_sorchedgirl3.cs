using System;
using System.Collections.Generic;
using UnityEngine;
using LOR_DiceSystem;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_malkuth_sorchedgirl3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            _owner.bufListDetail.AddBuf(new Ember());
        }
        public override void OnWaveStart()
        {
            _owner.bufListDetail.AddBuf(new Ember());
        }
        public class Ember : BattleUnitBuf
        {
            private bool IsFirstDice(BattleDiceBehavior behavior) => behavior != null && behavior.Index == 0;
            public override string keywordId => "EF_Ember_Eternal";
            public override string keywordIconId => "Burning_Match";
            public Ember() => stack = 0;
            public override void OnRoundStart()
            {
                stack += 1;
                if (stack >= 4)
                {
                    _owner.LoseHp((int)(0.05 * _owner.MaxHp));
                    DiceEffectManager.Instance.CreateBufEffect("BufEffect_Burn", _owner.view);
                }
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                base.BeforeRollDice(behavior);
                if (!IsFirstDice(behavior))
                    return;
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    max = stack
                });
            }
        }
    }
}

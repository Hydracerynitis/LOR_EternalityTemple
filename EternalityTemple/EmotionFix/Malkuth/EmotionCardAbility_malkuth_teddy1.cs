using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_malkuth_teddy1: EmotionCardAbilityBase
    {
        private BattleUnitModel _lastTarget;
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            if (RandomUtil.valueForProb > 0.4 && _owner.faction == Faction.Enemy)
                return;
            BattleUnitModel target = behavior?.card?.target;
            if (target == null)
                return;
            int diceResultValue = behavior.DiceResultValue;
            _owner.battleCardResultLog?.SetEmotionAbility(true, _emotionCard, 0, ResultOption.Default, diceResultValue);
            target.TakeBreakDamage(diceResultValue,DamageType.Emotion ,_owner);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Teddy_Atk");
            target.battleCardResultLog?.SetCreatureAbilityEffect("1/HappyTeddy_Hug");
        }
        public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
        {
            if (card.target != _lastTarget)
                _lastTarget = card?.target;
        }
    }
}

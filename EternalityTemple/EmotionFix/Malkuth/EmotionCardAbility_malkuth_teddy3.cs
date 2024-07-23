using System;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix.Malkuth
{
    public class EmotionCardAbility_malkuth_teddy3 : EmotionCardAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior.IsParrying())
            {
                int num = RandomUtil.Range(1, 2);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = num
                });
                _owner.battleCardResultLog?.SetCreatureAbilityEffect("1/Teddy_Heart");
                behavior.card.target?.battleCardResultLog?.SetCreatureEffectSound("Creature/Teddy_Guard");
            }
            else
            {
                int num = RandomUtil.Range(1, 2);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = -num
                });
            }
        }
    }
}

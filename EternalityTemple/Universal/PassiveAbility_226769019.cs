using System;

namespace EternalityTemple
{
    public class PassiveAbility_226769019 : PassiveAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                min = 1
            });
            if (base.IsAttackDice(behavior.Detail) && this.owner.emotionDetail.EmotionLevel >= 3)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    power = 1
                });
            }
            if (Singleton<StageController>.Instance.RoundTurn >= 4)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus
                {
                    power = 1
                });
            }
        }
    }
}


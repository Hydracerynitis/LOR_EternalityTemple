using System;
using UnityEngine;

namespace EternalityTemple
{
    public class PassiveAbility_226769020 : PassiveAbility_226769019
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            owner.RecoverHP(2);
            owner.breakDetail.RecoverBreak(2);
        }
    }
    public class PassiveAbility_226769023 : PassiveAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            owner.RecoverHP(2);
            owner.breakDetail.RecoverBreak(2);
        }
        public override int GetDamageReductionAll()
        {
            return 1;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus
            {
                min = 1,
                max = 1
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

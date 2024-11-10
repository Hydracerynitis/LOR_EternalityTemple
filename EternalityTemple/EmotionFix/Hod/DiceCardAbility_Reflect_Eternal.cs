using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class DiceCardAbility_Reflect_Eternal : DiceCardAbilityBase
    {
        public override void OnWinParryingDefense()
        {
            if (behavior.TargetDice == null)
                return;
            card.target.battleCardResultLog?.SetCreatureAbilityEffect("3/BlackSwan_Barrier", 0.8f);
            card.target.TakeDamage(behavior.TargetDice.DiceResultValue, DamageType.Rebound,owner);
            card.target.TakeBreakDamage(behavior.TargetDice.DiceResultValue, DamageType.Rebound,owner);
        }
    }
}

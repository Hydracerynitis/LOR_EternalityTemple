using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix.Yesod
{
    public class DiceCardAbility_Execute_Eternal : DiceCardAbilityBase
    {
        public override void BeforeRollDice() => behavior.ApplyDiceStatBonus(new DiceStatBonus()
        {
            dmg = -10000,
            breakDmg = -10000
        });

        public override void AfterAction()
        {
            behavior.card.target.TakeDamage(behavior.DiceResultValue);
            behavior.card.target.TakeBreakDamage(behavior.DiceResultValue);
            behavior?.card?.target?.battleCardResultLog?.SetNewCreatureAbilityEffect("2_Y/FX_IllusionCard_2_Y_Seven", 3f);
            behavior.card.target.battleCardResultLog?.SetCreatureEffectSound("Creature/Matan_FinalShot");
        }
    }
}

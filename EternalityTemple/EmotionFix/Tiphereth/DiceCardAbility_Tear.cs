using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class DiceCardAbility_Tear: DiceCardAbilityBase
    {
        public override void OnSucceedAttack(BattleUnitModel target)
        {
            if (card.cardBehaviorQueue.Count != 0)
            {
                foreach (BattleDiceBehavior battleDice in card.cardBehaviorQueue)
                {
                    if (IsAttackDice(battleDice.Detail))
                    {
                        card.ApplyDiceAbility(DiceMatch.NextAttackDice, new DiceCardAbility_Tear());
                        return;
                    }
                }
            }
            owner.battleCardResultLog?.SetCreatureEffectSound("Creature/KnightOfDespair_Atk_Strong");
            owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Despair));
            card.target.TakeDamage(Math.Min((int)(card.target.MaxHp * 0.1), 12));
        }
    }

}

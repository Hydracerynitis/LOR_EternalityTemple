using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;

namespace EmotionalFix.Yesod
{
    public class EmotionCardAbility_yesod_helper3 : EmotionCardAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior?.card?.target == null)
                return;
            int a = behavior.card.speedDiceResultValue - behavior.card.target.speedDiceResult[behavior.card.targetSlotOrder].value;
            if (a <= 0 || !IsAttackDice(behavior.Detail))
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = RandomUtil.Range(1, 2)
            }) ;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            BattleUnitModel target = behavior.card?.target;
            int? speedDiceResultValue = behavior.card?.speedDiceResultValue;
            int? nullable = target?.speedDiceResult[behavior.card.targetSlotOrder].value;
            if (speedDiceResultValue.GetValueOrDefault() > nullable.GetValueOrDefault() & (speedDiceResultValue.HasValue & nullable.HasValue))
                target.battleCardResultLog?.SetCreatureAbilityEffect("2/Helper_Hit", 1.5f);
            if (behavior.Detail != BehaviourDetail.Slash || target == null)
                return;
            target.battleCardResultLog?.SetCreatureEffectSound("Creature / Helper_Atk");
        }
    }
}

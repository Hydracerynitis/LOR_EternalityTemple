using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityEmotion
{
    public class EmotionCardAbility_keter_quietKid3: EmotionCardAbilityBase
    {
        public override int GetBreakDamageReduction(BattleDiceBehavior behavior) => RandomUtil.Range(1, 3);

        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            if (atkDice.owner == null)
                return;
            atkDice.owner.battleCardResultLog?.SetNewCreatureAbilityEffect("0_K/FX_IllusionCard_0_K_RedEye", 1f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Slientgirl_Guilty");
        }
        public override double ChangeDamage(BattleUnitModel attacker, double dmg)
        {
            return dmg-attacker.history.damageToEnemyAtRound/5;
        }
    }
}

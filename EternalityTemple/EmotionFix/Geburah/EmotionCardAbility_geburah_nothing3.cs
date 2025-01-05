using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityEmotion
{
    public class EmotionCardAbility_geburah_nothing3 : EmotionCardAbilityBase
    {
        public override int GetBreakDamageReduction(BattleDiceBehavior behavior) => 9999;

        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            base.OnTakeDamageByAttack(atkDice, dmg);
            _owner.battleCardResultLog?.SetCreatureAbilityEffect("6/Nothing_Guard", 0.8f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/NothingThere_Guard");
        }
    }
}

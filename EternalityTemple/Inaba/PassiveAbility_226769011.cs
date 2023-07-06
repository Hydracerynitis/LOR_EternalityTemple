using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Inaba
{
    public class PassiveAbility_226769011 : PassiveAbilityBase
    {
        public override bool BeforeTakeDamage(BattleUnitModel attacker, int dmg)
        {
            if(attacker.currentDiceAction.card.HasBuf<BattleUnitBuf_InabaBuf2.BattleDiceCardBuf_checkInaba>())
            {
                attacker.TakeDamage(dmg / 2,DamageType.Passive);
                return true;
            }
            return false;
        }
    }
}
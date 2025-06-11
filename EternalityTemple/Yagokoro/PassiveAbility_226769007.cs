using EternalityTemple.Kaguya;
using HyperCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Yagokoro
{
    public class PassiveAbility_226769007: PassiveAbilityBase
    {
        public override void OnBattleEnd()
        {
            double ratio = 0.1;
            if (owner.IsDead())
                ratio = 0.2;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetList(owner.faction))
            {
                unit.RecoverHP((int)(ratio * (unit.MaxHp - (int)unit.hp)));
                unit.UpdateUnitData();
            }
                
        }
    }
}

using EternalityTemple.Kaguya;
using HyperCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EternalityTemple.Yagokoro
{
    public class PassiveAbility_226769008: PassiveAbilityBase
    {
        public override void OnBattleEnd()
        {
            double ratio = 0.1;
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetList(owner.faction))
            {
                unit.hp = Mathf.Min(unit.hp + (int)(ratio * (unit.MaxHp - (int)unit.hp)), unit.MaxHp);
                unit.UpdateUnitData();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Inaba
{
    public class PassiveAbility_226769010 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            BattleUnitBuf_InabaBuf2.AddStack(owner, 1);
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(false))
            {
                battleUnitModel.bufListDetail.AddBuf(new BattleUnitBuf_InabaDmgCheck());
            }
        }
        public class BattleUnitBuf_InabaDmgCheck : BattleUnitBuf
        {
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetAliveList(false).Find((BattleUnitModel x) => x.passiveDetail.HasPassive<PassiveAbility_226769010>());
                if (battleUnitModel == null)
                {
                    Destroy();
                    return;
                }
                BattleUnitBuf_InabaBuf1.AddStack(battleUnitModel, dmg);
            }
        }
    }
}
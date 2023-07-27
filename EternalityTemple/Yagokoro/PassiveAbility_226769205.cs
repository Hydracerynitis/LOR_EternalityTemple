using EternalityTemple.Kaguya;
using HyperCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Yagokoro
{
    public class PassiveAbility_226769205 : PassiveAbilityBase
    {
        public override void OnRoundEnd()
        {
            MoonBuf moonBuf = owner.bufListDetail.GetActivatedBufList().Find(x => x is MoonBuf) as MoonBuf;
            if (moonBuf != null)
                moonBuf.Update();
            else
                owner.bufListDetail.AddBuf(new BattleUnitBuf_Moon1());
        }
    }
}

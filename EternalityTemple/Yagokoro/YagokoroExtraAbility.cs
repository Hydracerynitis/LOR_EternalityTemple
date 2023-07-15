using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Yagokoro
{
    public class MoonCardAbility: DiceCardSelfAbilityBase
    {
        public int ActivateMoonAbility = 0;
        //用来判断这个书页可以用在哪几个月相骰上 e.g. 月相1月相2月相3的情况下 slot==1 slot==2 slot==3的时候 return true
        public virtual bool CanActivateMoon(int slot)
        {
            return false;
        }
        //触发月相1的效果
        public virtual void OnFirstMoon()
        {

        }
        //触发月相2的效果
        public virtual void OnSecondMoon()
        {

        }
        //触发月相3的效果
        public virtual void OnThirdMoon()
        {

        }
        //触发月相4的效果
        public virtual void OnForthMoon()
        {

        }
        //触发月相5的效果
        public virtual void OnFifthMoon()
        {

        }
    }
}

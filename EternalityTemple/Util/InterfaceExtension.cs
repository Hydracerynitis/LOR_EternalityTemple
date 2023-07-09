using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Util
{
    internal interface IsBufImmune
    {
        bool IsImmune(BattleUnitBuf buf);
    }
    internal interface OnGiveOtherBuf
    {
        void OnGiveBuf(BattleUnitBuf buf,int stack);
    }
    internal interface OnRecoverHP
    {
        void OnHeal(int num);
    }
    internal interface OnAddOtherBuf
    {
        void OnAddBuf(BattleUnitBuf buf, int stack);
    }
}

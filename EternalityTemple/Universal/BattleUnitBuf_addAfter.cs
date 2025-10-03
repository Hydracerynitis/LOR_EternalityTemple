using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple
{
    public class BattleUnitBuf_addAfter : BattleUnitBuf
    {
        public BattleUnitBuf_addAfter(LorId cardId, int turnCount)
        {
            this._cardId = cardId;
            this._count = turnCount;
        }
        public override void OnRoundStart()
        {
            this._count--;
            if (this._count <= 0)
            {
                _owner.personalEgoDetail.AddCard(_cardId);
                this.Destroy();
            }
        }
        private int _count;
        private LorId _cardId = LorId.None;
    }
}

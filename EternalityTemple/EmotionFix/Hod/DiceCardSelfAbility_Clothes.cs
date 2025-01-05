using System;
using LOR_DiceSystem;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityEmotion
{
    public class DiceCardSelfAbility_Clothes: DiceCardSelfAbilityBase
    {
        public override void OnUseCard()
        {
            this.owner.allyCardDetail.DrawCards(1);
            foreach(BattleUnitModel ally in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                if (!(ally.bufListDetail.GetActivatedBufList().Find(x => x is Nettle) is Nettle nettle))
                {
                    Nettle Nettle = new Nettle(2);
                    ally.bufListDetail.AddBuf(Nettle);
                }
                else
                {
                    nettle.stack += 2;
                }
            }
        }
        public class Nettle : BattleUnitBuf
        {
            public override string keywordId => "BlackSwan_Nettle";

            public Nettle(int value) => stack = value;

            public override int GetDamageReduction(BattleDiceBehavior behavior)
            {
                return stack;
            }
            public override int GetBreakDamageReduction(BehaviourDetail behaviourDetail)
            {
                return stack;
            }
        }
    }
}

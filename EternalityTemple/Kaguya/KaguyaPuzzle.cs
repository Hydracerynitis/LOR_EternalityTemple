using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Kaguya
{
    public class BattleUnitBuf_Puzzle : BattleUnitBuf
    {
        public override string keywordIconId => "KeterFinal_Cogito";
        public override string keywordId => "KaguyaBuf8";
    }
    public class DiceCardSelfAbility_KaguyaPuzzle2 : DiceCardSelfAbilityBase
    {
        public override void OnUseInstance(BattleUnitModel unit, BattleDiceCardModel self, BattleUnitModel targetUnit)
        {
            
        }
    }
    public class KaguyaPuzzle1: BattleUnitBuf
    {
        public override string keywordIconId => "PlutoUnfairAtk";
    }
}

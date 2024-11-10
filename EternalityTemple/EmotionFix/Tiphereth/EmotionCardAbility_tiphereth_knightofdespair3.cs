using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_tiphereth_knightofdespair3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _owner.bufListDetail.AddBuf(new Tear());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.bufListDetail.AddBuf(new Tear());
        }
        public class Tear : BattleUnitBuf
        {
            public override string keywordIconId => "KnightOfDespair_Blessing";
            public override string keywordId => "EF_Tear";
            public override void OnStartParrying(BattlePlayingCardDataInUnitModel card)
            {
                if (card.isKeepedCard)
                    return;
                foreach (BattleDiceBehavior battleDice in card.cardBehaviorQueue)
                {
                    if (IsAttackDice(battleDice.Detail))
                    {
                        card.ApplyDiceAbility(DiceMatch.NextAttackDice, new DiceCardAbility_Tear());
                        return;
                    }
                }
            }
        }
    }
}

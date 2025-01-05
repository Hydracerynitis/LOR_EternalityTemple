using System;
using System.Collections.Generic;
using Sound;

namespace EternalityEmotion
{
    public class EmotionCardAbility_malkuth_teddy2 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion() => SoundEffectPlayer.PlaySound("Creature/Teddy_On");
        public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
        {
            if (card.target == null || card.target.currentDiceAction is BattleKeepedCardDataInUnitModel)
                return;
            BattleUnitBuf activatedBuf = card.target.bufListDetail.GetActivatedBuf(KeywordBuf.TeddyLove);
            if (activatedBuf != null && activatedBuf.stack < 5)
            {
                card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus() { power = activatedBuf.stack });
                if(activatedBuf.stack<5)
                    ++activatedBuf.stack;
            }
            else
                card.target.bufListDetail.AddBuf(new TeddyMemory());
        }
        public class TeddyMemory : BattleUnitBuf
        {
            public override KeywordBuf bufType => KeywordBuf.TeddyLove;
            public override string keywordIconId => "TeddyLove";
            public override string keywordId => "EF_Memory_Eternal";
            public override void OnRoundEnd()
            {
                stack -= 1;
                if (stack <= 0)
                {
                    Destroy();
                }
            }
        }
    }
}

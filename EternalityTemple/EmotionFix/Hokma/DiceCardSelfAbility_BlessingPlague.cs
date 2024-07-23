using System;
using LOR_DiceSystem;
using System.IO;
using UnityEngine;
using Sound;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix.Hokma
{
    public class DiceCardSelfAbility_BlessingPlague : DiceCardSelfAbilityBase
    {
        public override void OnStartBattle()
        {
            DiceEffectManager.Instance.CreateBehaviourEffect("WhiteNight_Blessing", 1f, card.target.view, card.target.view);
            SoundEffectPlayer.PlaySound("Creature/WhiteNight_Bless");
            card.target.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Endurance, 1);
            card.target.RecoverHP(6);
            EmotionCardAbility_hokma_plaguedoctor1.WhiteNightClock[owner.UnitData] += 1;
        }
        public override bool IsTargetChangable(BattleUnitModel attacker)
        {
            return false;
        }
    }
}

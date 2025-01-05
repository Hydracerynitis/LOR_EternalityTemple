using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityEmotion
{
    public class EmotionCardAbility_netzach_alriune2 : EmotionCardAbilityBase
    {
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            base.OnTakeDamageByAttack(atkDice, dmg);
            BattleUnitModel owner = atkDice?.owner;
            if (owner == null)
                return;
            owner.bufListDetail.AddKeywordBufByCard(KeywordBuf.Alriune_Debuf, 1, _owner);
            owner.battleCardResultLog?.SetCreatureAbilityEffect("0/Alriune_Stun_Effect", 1f);
            owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Ali_Guard");
        }
    }
}

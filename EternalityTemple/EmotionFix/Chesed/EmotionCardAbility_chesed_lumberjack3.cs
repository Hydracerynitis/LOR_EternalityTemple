using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_chesed_lumberjack3 : EmotionCardAbilityBase
    {
        private int count;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            count = 0;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            behavior.card.target.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Bloodmeet", 2f);
            behavior.card.target.battleCardResultLog?.SetCreatureEffectSound("Creature/WoodMachine_Kill");
            if(count>=2 || behavior.card.target.cardSlotDetail.PlayPoint - behavior.card.target.cardSlotDetail.ReservedPlayPoint <= 0)
            {
                count++;
                _owner.cardSlotDetail.RecoverPlayPoint(1);
                behavior.card.target.cardSlotDetail.LosePlayPoint(1);
            }
           
        }
    }
}

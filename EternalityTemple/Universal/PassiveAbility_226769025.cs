using System;
using System.Collections.Generic;
using LOR_DiceSystem;
using LOR_XML;
using UnityEngine;
namespace EternalityTemple
{
    public class PassiveAbility_226769025 : PassiveAbilityBase
    {
        public bool HasEgoPassive()
        {
            return this.owner.passiveDetail.HasPassive<PassiveAbility_250422>();
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.allyCardDetail.AddNewCard(607002, false);
            owner.allyCardDetail.SetMaxHand(20);
            owner.allyCardDetail.SetMaxDrawHand(14);
        }
        public override void OnRoundStart()
        {
            if (owner.emotionDetail.EmotionLevel >= 4 && !this.HasEgoPassive())
            {
                owner.allyCardDetail.AddNewCard(607001);
                this.owner.RecoverBreakLife(1, false);
                this.owner.ResetBreakGauge();
                this.owner.breakDetail.nextTurnBreak = false;
                this.owner.passiveDetail.AddPassive(new PassiveAbility_250422());
            }
        }
    }
}
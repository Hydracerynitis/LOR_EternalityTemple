using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EternalityEmotion
{
    public class EmotionCardAbility_chesed_scarecrow3 : EmotionCardAbilityBase
    {
        private bool Trigger;
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            Trigger = false;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior.card.target==null || Trigger)
                return;
            Trigger = true;
            List<BattleDiceBehavior> diceinhand = new List<BattleDiceBehavior>();
            foreach (BattleDiceCardModel card in behavior.card.target.allyCardDetail.GetHand())
                diceinhand.AddRange(card.CreateDiceCardBehaviorList().FindAll(x => x.Type == BehaviourType.Atk));
            if (diceinhand.Count == 0)
                return;
            behavior.card.AddDice(RandomUtil.SelectOne(diceinhand));
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Scarecrow_Dead");
        }
    }
}

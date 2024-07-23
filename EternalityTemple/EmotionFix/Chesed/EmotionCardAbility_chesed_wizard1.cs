using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Chesed
{
    public class EmotionCardAbility_chesed_wizard1: EmotionCardAbilityBase
    {
        private int round;
        private bool Trigger;
        public override void OnSelectEmotion()
        {
            if (_owner.faction == Faction.Enemy)
            {
                round = 4;
                Trigger = false;
            }
        }
        public override void OnRoundStart()
        {
            if (round!=5)
                round += 1;
            if (round == 5)
            {
                if (_owner.cardSlotDetail.PlayPoint >= 3)
                {
                    _owner.cardSlotDetail.SpendCost(3);
                    DiceEffectManager.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Magic", 1f, _owner.view, _owner.view, 3f);
                    SoundEffectPlayer.PlaySound("Creature/Oz_Atk_Up");
                    Trigger = true;
                }
            }
        }
        public override void OnStartBattle()
        {
            if (!Trigger)
                return;
            List<BattlePlayingCardDataInUnitModel> cards = StageController.Instance._allCardList.FindAll(x => x.owner.faction!=_owner.faction && GetParry(x)==null);
            if (cards.Count == 0)
                cards.AddRange(StageController.Instance._allCardList.FindAll(x => x.owner.faction != _owner.faction));
            if (cards.Count == 0)
                return;
            List <BattlePlayingCardDataInUnitModel> victims=new List<BattlePlayingCardDataInUnitModel>();
            for (;cards.Count>0 && victims.Count < 2;)
            {
                BattlePlayingCardDataInUnitModel victim = RandomUtil.SelectOne(cards);
                victims.Add(victim);
                cards.Remove(victim);
            }
            victims.ForEach(x => x.DestroyPlayingCard()); 
            Trigger = false;
            round = 0;
        }
        public BattlePlayingCardDataInUnitModel GetParry(BattlePlayingCardDataInUnitModel card)
        {
            BattlePlayingCardDataInUnitModel oppoist = card.target.cardSlotDetail.cardAry[card.targetSlotOrder];
            if (oppoist.owner.DirectAttack() || card.owner.DirectAttack())
                return null;
            if (oppoist.target == card.owner && oppoist.targetSlotOrder == card.slotOrder)
                return oppoist;
            else
                return null;
        }
    }
}

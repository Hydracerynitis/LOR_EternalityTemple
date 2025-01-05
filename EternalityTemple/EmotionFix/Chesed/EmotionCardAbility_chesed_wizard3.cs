using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_chesed_wizard3: EmotionCardAbilityBase
    {
        private List<BattleDiceCardModel> zero_cards= new List<BattleDiceCardModel>();
        private int round;
        public override void OnSelectEmotion()
        {
            round = 3;
        }
        public override void OnDrawCard()
        {
            zero_cards.Clear();
            if (round != 4)
                round += 1;
            if (round == 4)
            {
                if (_owner.cardSlotDetail.PlayPoint >= 4)
                {
                    _owner.cardSlotDetail.SpendCost(4);
                    foreach (BattleDiceCardModel battleDiceCardModel in _owner.allyCardDetail.GetHand())
                    {
                        if(battleDiceCardModel.GetOriginCost()<=0)
                            zero_cards.Add(battleDiceCardModel);
                        battleDiceCardModel.AddBuf(new DiceCardSelfAbility_oz_control.BattleDiceCardBuf_costZero1Round());
                    }
                    round = 0;
                    DiceEffectManager.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Magic", 1f, _owner.view, _owner.view, 3f);
                    SoundEffectPlayer.PlaySound("Creature/Oz_CardMagic");
                }
            }
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (zero_cards.Contains(curCard.card))
            {
                curCard.ApplyDiceStatBonus(DiceMatch.AllDice,new DiceStatBonus() { power = RandomUtil.Range(1, 2) });
            }
        }
    }
}

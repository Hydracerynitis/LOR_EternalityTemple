using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Keter
{
    public class EmotionCardAbility_keter_pinocchio1 : EmotionCardAbilityBase
    {
        private List<LorId> copyiedList = new List<LorId>();
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            SoundEffectPlayer.PlaySound("Creature/Pino_On"); 
            BattleDiceCardModel copy = _owner.allyCardDetail.AddNewCard(RandomUtil.SelectOne(copyiedList));
            copy.temporary = true;
            copy.SetPriorityAdder(100);
            copyiedList.Clear();
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            if (_owner.faction != Faction.Enemy)
                return;
            foreach (BattleUnitModel player in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                List<BattlePlayingCardDataInUnitModel> cardAry = player.cardSlotDetail.cardAry;
                if (cardAry == null)
                    continue;
                foreach (BattlePlayingCardDataInUnitModel cardDataInUnitModel in cardAry)
                {
                    if (cardDataInUnitModel?.card != null)
                    {
                        if (cardDataInUnitModel.card.XmlData.optionList.Contains(CardOption.Personal))
                            continue;
                        LorId num = cardDataInUnitModel.card.GetID();
                        copyiedList.Add(num);
                    }
                }
            }
        }
    }
}

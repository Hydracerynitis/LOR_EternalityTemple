using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;
using LowCost = EmotionCardAbility_lumberjack2.BattleDiceCardBuf_Lumberjack_Emotion;

namespace EmotionalFix
{
    public class EmotionCardAbility_chesed_lumberjack2 : EmotionCardAbilityBase
    {
        private int accumulatedDmg;
        private bool dmged;
        private List<BattleDiceCardModel> LowCostCards=new List<BattleDiceCardModel>();

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (dmged)
                Ability();
            accumulatedDmg = 0;
            dmged = false;
            LowCostCards.Clear();
        }
        public override void OnDrawCard()
        {
            base.OnDrawCard();
            LowCostCards.AddRange(_owner.allyCardDetail.GetAllDeck().FindAll(x => x.GetCost() < 2));
        }

        public override void AfterGiveDamage(BattleUnitModel target, int dmg)
        {
            base.AfterGiveDamage(target, dmg);
            if (dmged)
                return;
            accumulatedDmg += dmg;
            if (accumulatedDmg < 15)
                return;
            dmged = true;
            Effect();
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            if (LowCostCards.Contains(curCard.card))
                curCard.ApplyDiceStatBonus(DiceMatch.AllAttackDice, new DiceStatBonus() { dmgRate = 25 });
        }
        public void Effect()
        {
            if (Singleton<StageController>.Instance.IsLogState())
            {
                _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("7_C/FX_IllusionCard_7_C_Heart", 1.5f);
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/WoodMachine_AtkStrong");
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Heart_Guard");
            }
            else
            {
                DiceEffectManager.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Heart", 1f, _owner.view, _owner.view, 1.5f);
                SoundEffectPlayer.PlaySound("Creature/WoodMachine_AtkStrong");
                SoundEffectPlayer.PlaySound("Creature/Heart_Guard");
            }
        }

        private void Ability()
        {
            List<BattleDiceCardModel> battleDiceCardModelList = new List<BattleDiceCardModel>();
            battleDiceCardModelList.AddRange(_owner.allyCardDetail.GetHand());
            if (battleDiceCardModelList.Count <= 0)
                return;
            battleDiceCardModelList.Sort((x, y) => y.GetCost() - x.GetCost());
            for (int i = 0; i < 2; i++)
            {
                int targetCost = battleDiceCardModelList[0].GetCost();
                BattleDiceCardModel targetCard = RandomUtil.SelectOne(battleDiceCardModelList.FindAll(x => x.GetCost() == targetCost));
                battleDiceCardModelList.Remove(targetCard);
                targetCard.AddBuf(new LowCost());
            }
        }
    }
}

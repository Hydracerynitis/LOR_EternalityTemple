using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_binah_bigbird3 : EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect _aura;

        public override void OnParryingStart(BattlePlayingCardDataInUnitModel card)
        {
            base.OnParryingStart(card);
            DestroyAura();
            BattleUnitModel target = card?.target;
            if (target == null || target.speedDiceResult[card.targetSlotOrder].value <= card.speedDiceResultValue)
                return;
            _aura = DiceEffectManager.Instance.CreateNewFXCreatureEffect("8_B/FX_IllusionCard_8_B_See_Red", 1f, _owner.view, _owner.view);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Bigbird_MouseOpen");
            _owner.battleCardResultLog?.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(DestroyAura));
            card.ApplyDiceStatBonus(DiceMatch.AllDice, new DiceStatBonus()
            {
                power = 1
            });
        }

        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || target.hp > target.MaxHp * 0.25)
                return;
            target.currentDiceAction?.DestroyDice(DiceMatch.AllDice);
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_DiceBroken", 3f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Bigbird_HeadCut");
        }

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            DestroyAura();
        }

        public void DestroyAura()
        {
            if (_aura == null)
                return;
            UnityEngine.Object.Destroy(_aura.gameObject);
            _aura = null;
        }
    }
}

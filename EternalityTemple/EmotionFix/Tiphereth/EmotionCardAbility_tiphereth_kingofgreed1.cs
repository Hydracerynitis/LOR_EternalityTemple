using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix.Tiphereth
{
    public class EmotionCardAbility_tiphereth_kingofgreed1 : EmotionCardAbilityBase
    {

        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || behavior == null || behavior.Index != 0 || RandomUtil.valueForProb > 0.25)
                return;
            target.currentDiceAction?.DestroyDice(DiceMatch.AllDice);
            _owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(Filter));
        }

        public void Filter()
        {
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KingOfGreed_Yellow", false);
            SoundEffectPlayer.PlaySound("Creature/Greed_StrongAtk_Defensed");
        }
    }
}

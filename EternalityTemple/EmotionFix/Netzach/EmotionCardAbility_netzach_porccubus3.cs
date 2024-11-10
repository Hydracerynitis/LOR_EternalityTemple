using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_netzach_porccubus3 : EmotionCardAbilityBase
    {
        private static int RecoverBP => RandomUtil.Range(2, 4);
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            _owner.battleCardResultLog?.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(Filter));
            if (_owner.IsBreakLifeZero())
                return;
            int recoverBp = RecoverBP;
            _owner.battleCardResultLog?.SetEmotionAbility(false, _emotionCard, 0, ResultOption.Default, Array.Empty<int>());
            _owner.breakDetail.RecoverBreak(recoverBp);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Hit");
        }
        public void Filter() => new GameObject().AddComponent<SpriteFilter_Porccubus>().Init("EmotionCardFilter/Porccubus_Filter", false);
    }
}

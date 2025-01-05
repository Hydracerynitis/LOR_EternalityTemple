using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_hod_spiderBud2 : EmotionCardAbilityBase
    {
        private bool breaked;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            if (behavior !=null && behavior.card.target!=null && behavior.card.target.IsBreakLifeZero())
                breaked = true;
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            if (behavior?.card?.target == null || !behavior.card.target.IsBreakLifeZero() || breaked)
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
                alive.RecoverHP(RandomUtil.Range(3, 7));
            string resPath = "";
            switch (behavior.behaviourInCard.MotionDetail)
            {
                case MotionDetail.H:
                    resPath = "3/SpiderBud_Spiders_H";
                    break;
                case MotionDetail.J:
                    resPath = "3/SpiderBud_Spiders_J";
                    break;
                case MotionDetail.Z:
                    resPath = "3/SpiderBud_Spiders_Z";
                    break;
            }
            if (!string.IsNullOrEmpty(resPath))
                _owner.battleCardResultLog.SetCreatureAbilityEffect(resPath, 1f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Spider_gochiDown");
        }
    }
}

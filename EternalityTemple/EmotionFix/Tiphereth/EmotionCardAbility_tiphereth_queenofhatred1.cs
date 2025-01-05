using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityEmotion
{
    public class EmotionCardAbility_tiphereth_queenofhatred1 : EmotionCardAbilityBase
    {
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MagicalGirl_kiss");
            _owner.battleCardResultLog?.SetEmotionAbilityEffect("5/MagicalGirl_Heart");
            if (IsAttackDice(behavior.Detail))
            {
                foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList_random(_owner.faction, 1))
                    battleUnitModel.RecoverHP(RandomUtil.Range(3, 5));
            }
            else
            {
                if (!IsDefenseDice(behavior.Detail))
                    return;
                List<BattleUnitModel> list = new List<BattleUnitModel>();
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
                {
                    if (!alive.IsBreakLifeZero())
                        list.Add(alive);
                }
                RandomUtil.SelectOne(list).breakDetail.RecoverBreak(RandomUtil.Range(2, 4));
            }
        }
        public override void OnPrintEffect(BattleDiceBehavior behavior) => base.OnPrintEffect(behavior);
    }
}

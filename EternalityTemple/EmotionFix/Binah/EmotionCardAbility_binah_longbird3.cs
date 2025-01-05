using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_binah_longbird3 : EmotionCardAbilityBase
    {
        private int cnt;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            cnt = 0;
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || cnt >= 3)
                return;
            float maxHp = GetMaxHP();
            if (target.hp < maxHp)
                return;
            ++cnt;
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Scale", 2f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/LongBird_On");
            BattleUnitModel benifitor = GetHealTarget();
            if(benifitor==null)
                return;
            benifitor.RecoverHP((int)(benifitor.MaxHp*0.05));
        }

        private BattleUnitModel GetHealTarget()
        {
            BattleUnitModel healTarget = null;
            List<BattleUnitModel> list = new List<BattleUnitModel>();
            int num = -100;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
            {
                if (num == -100)
                {
                    list.Add(alive);
                    num = (int)alive.hp;
                }
                else if ((int)alive.hp < num)
                {
                    list.Clear();
                    list.Add(alive);
                    num = (int)alive.hp;
                }
                else if ((int)alive.hp == num)
                    list.Add(alive);
            }
            if (list.Count > 0)
                healTarget = RandomUtil.SelectOne(list);
            return healTarget;
        }

        private float GetMaxHP()
        {
            float maxHp = 0.0f;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
            {
                if (alive.hp > maxHp)
                    maxHp = alive.hp;
            }
            return maxHp;
        }
    }
}

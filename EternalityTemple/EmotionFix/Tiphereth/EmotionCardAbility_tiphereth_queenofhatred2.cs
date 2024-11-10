using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_tiphereth_queenofhatred2 : EmotionCardAbilityBase
    {
        public BattleUnitModel target;
        public Battle.CreatureEffect.CreatureEffect effect;
        private int max;
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            BattleUnitModel battleUnitModel = null;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
            {
                int damageToEnemyAtRound = alive.history.damageToEnemyAtRound;
                if (damageToEnemyAtRound > max)
                {
                    battleUnitModel = alive;
                    max = damageToEnemyAtRound;
                }
            }
            target = battleUnitModel;
            max = 0;
            if (effect == null)
                return;
            UnityEngine.Object.Destroy(effect.gameObject);
            effect = null;
        }

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (target == null || target.IsDead() || effect!=null)
                return;
            effect=MakeEffect("5/MagicalGirl_Villain", target: target);
            target.bufListDetail.AddBuf(new Villain());
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            base.BeforeGiveDamage(behavior);
            if (target != behavior.card?.target)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = RandomUtil.Range(3, 5)
            });
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MagicalGirl_Gun");
            _owner.battleCardResultLog.SetAttackEffectFilter(typeof(ImageFilter_ColorBlend_Pink));
        }
        public class Villain: BattleUnitBuf
        {
            public override bool Hide => true;
            public override void OnRoundEndTheLast()
            {
                base.OnRoundEndTheLast();
                Destroy();
            }
            public override void OnDie()
            {
                base.OnDie();
                Destroy();
            }
        }
    }
}

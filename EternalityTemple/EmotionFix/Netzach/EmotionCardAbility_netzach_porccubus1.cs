using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EternalityEmotion
{
    public class EmotionCardAbility_netzach_porccubus1 : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior.card?.target;
            if (target == null)
                return;
            if (target.bufListDetail.GetActivatedBufList().Find(x => x is Thrill) is Thrill happy)
                happy.Add();
            else
            {
                happy = new Thrill();
                target.bufListDetail.AddBuf(happy);
                happy.Add();
            }
        }
        public class Thrill : BattleUnitBuf
        {
            public override string keywordId => "EF_Thrill";
            public override string keywordIconId => "Porccubus_Happy";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public void Add()
            {
                ++stack;
                _owner.battleCardResultLog?.SetCreatureAbilityEffect("3/Porccubuss_Delight", 1f);
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Porccu_Penetrate");
                if (stack < 3)
                    return;
                int dmg= RandomUtil.Range(2, 7);
                _owner.TakeDamage(dmg);
                _owner.TakeBreakDamage(dmg);
                _owner.battleCardResultLog?.SetTakeDamagedEvent(new BattleCardBehaviourResult.BehaviourEvent(BloodFilter));
                stack = 0;
                Destroy();
            }
            public void BloodFilter()
            {
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
                if (original != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                    if (creatureEffect?.gameObject.GetComponent<AutoDestruct>() == null)
                    {
                        AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
                        if (autoDestruct != null)
                        {
                            autoDestruct.time = 3f;
                            autoDestruct.DestroyWhenDisable();
                        }
                    }
                }
                SoundEffectPlayer.PlaySound("Creature/Porccu_Special");
            }
        }
    }
}

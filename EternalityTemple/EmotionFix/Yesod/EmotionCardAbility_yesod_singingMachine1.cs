using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_yesod_singingMachine1 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/Singing_Atk");
            Util.LoadPrefab("Battle/CreatureCard/SingingMachineCard_play_particle", SingletonBehavior<BattleSceneRoot>.Instance.transform);
        }
        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            _owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(Filter));
        }
        public override void BeforeGiveDamage(BattleDiceBehavior behavior)
        {
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = RandomUtil.Range(4, 8)
            });
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            _owner.breakDetail.TakeBreakDamage(RandomUtil.Range(4, 8));
        }
        public void Filter() => new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/SingingMachine_Filter_Special", false, 2f);
    }
}

using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;
using BaseMod;

namespace EmotionalFix.Yesod
{
    public class EmotionCardAbility_yesod_freischutz1 : EmotionCardAbilityBase
    {
        private Transform lastkillPos;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || GetAliveTarget() == null || target != GetAliveTarget())
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                dmg = RandomUtil.Range(3,5)
            });
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || GetAliveTarget() != null)
                return;
            target.bufListDetail.AddBuf(new EmotionCardAbility_freischutz1.BattleUnitBuf_Emotion_Freischutz_Target());
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("2_Y/FX_IllusionCard_2_Y_Marker", 3f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Matan_Flame");
        }

        private BattleUnitModel GetAliveTarget() => BattleObjectManager.instance.GetAliveList_opponent(_owner.faction).Find(x => x.bufListDetail.GetActivatedBufList().Find(y => y is EmotionCardAbility_freischutz1.BattleUnitBuf_Emotion_Freischutz_Target) != null);

        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            if (target.bufListDetail.GetActivatedBufList().Find(y => y is EmotionCardAbility_freischutz1.BattleUnitBuf_Emotion_Freischutz_Target) == null)
                return;
            lastkillPos = target.view.camRotationFollower;
            _owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(PrintEffect));
            _owner.breakDetail.RecoverBreak(_owner.breakDetail.GetDefaultBreakGauge());
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Matan_Flame");
            int emotionLevel = target.emotionDetail.EmotionLevel;
        }

        public void PrintEffect()
        {
            GameObject gameObject = Util.LoadPrefab("Battle/CreatureEffect/New_IllusionCardFX/2_Y/FX_IllusionCard_2_Y_MarkerDie");
            if (gameObject != null && lastkillPos != null)
            {
                gameObject.transform.parent = lastkillPos;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localRotation = Quaternion.identity;
            }
            lastkillPos = null;
        }
    }
}

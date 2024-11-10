using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using System.Data.SqlTypes;

namespace EmotionalFix
{
    public class EmotionCardAbility_tiphereth_kingofgreed3 : EmotionCardAbilityBase
    {

        private static int Dmg => RandomUtil.Range(3, 7);

        private static int Heal => RandomUtil.Range(2, 5);

        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || behavior.TargetDice == null)
                return;
            if (behavior.DiceResultValue - behavior.TargetDice.DiceResultValue < 4)
                return;
            target.TakeDamage(Dmg, DamageType.Emotion, _owner);
            _owner.RecoverHP(Heal);
            _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Greed_Vert_Change");
        }

        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            base.OnLoseParrying(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || behavior.TargetDice == null)
                return;
            if (Helper.SearchEmotion(_owner, "NihilClown_fusion_Enemy") != null)
                return;
            if (behavior.TargetDice.DiceResultValue - behavior.DiceResultValue < 4)
                return;
            _owner.TakeDamage(Dmg, DamageType.Emotion, _owner);
            target.RecoverHP(Heal);
            _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_GoldCrash", 1.5f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Greed_Vert_Change");
        }
    }
}

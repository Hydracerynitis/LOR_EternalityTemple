using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using Creature = EmotionCardAbility_bossbird6.BattleUnitBuf_Emotion_BossBird_Creature;

namespace EmotionalFix.Binah
{
    public class EmotionCardAbility_binah_bossbird6 : EmotionCardAbilityBase
    {
        private BattleUnitModel _target;
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _target = null;
        }
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || GetAliveCreature() != null)
                return;
            _target = target;
            target.bufListDetail.AddBuf(new Creature());
            target.battleCardResultLog?.SetNewCreatureAbilityEffect("8_B/FX_IllusionCard_8_B_Terrable_Start", 2f);
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Bossbird_Bossbird_Stab");
            target.battleCardResultLog?.SetCreatureEffectSound("Creature/Bossbird_StoryFilter_Dead");
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_target != null && !_target.IsDead())
            {
                _target.bufListDetail.AddBuf(new Creature());
                return;
            }
            _target =null;
        }
        private BattleUnitModel GetAliveCreature() => BattleObjectManager.instance.GetAliveList(Faction.Player).Find(x => x.bufListDetail.GetActivatedBufList().Find(y => y is Creature) != null) ?? _target;
    }
}

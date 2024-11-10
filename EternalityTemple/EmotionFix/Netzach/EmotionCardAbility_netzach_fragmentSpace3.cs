using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_netzach_fragmentSpace3 : EmotionCardAbilityBase
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
            if (target == null || behavior == null)
                return;
            if (RandomUtil.valueForProb <= 0.5 && cnt < 3)
            {
                ++cnt;
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Weak, 1, _owner);
                target.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Disarm, 1, _owner);
                _owner.battleCardResultLog.SetEndCardActionEvent(new BattleCardBehaviourResult.BehaviourEvent(Effect));
            }
        }
        private void Effect()
        {
            try
            {
                SoundEffectManager.Instance.PlayClip("Creature/Cosmos_Sing")?.SetGlobalPosition(_owner.view.WorldPosition);
                BattleCamManager.Instance?.AddCameraFilter<CameraFilterCustom_universe>(true);
            }
            catch 
            {

            }
        }
    }
}

using System;
using LOR_DiceSystem;
using UI;
using Battle.CameraFilter;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix.Netzach
{
    public class EmotionCardAbility_netzach_fragmentSpace1 : EmotionCardAbilityBase
    {
        public override void OnStartTargetedOneSide(BattlePlayingCardDataInUnitModel curCard)
        {
            if(RandomUtil.valueForProb <= 0.5)
            {
                curCard.owner.TakeBreakDamage(RandomUtil.Range(3, 6));
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

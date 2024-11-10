using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_tiphereth_servantofwrath3 : EmotionCardAbilityBase
    {
        private bool _effect;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _effect = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
                alive.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Decay, 5, _owner);
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!_effect)
            {
                _effect = true;
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/5/Servant_Emotion_Effect");
                if (original != null)
                {
                    Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
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
                SoundEffectPlayer.PlaySound("Creature/Angry_StrongFinish");
            }
        }
    }
}

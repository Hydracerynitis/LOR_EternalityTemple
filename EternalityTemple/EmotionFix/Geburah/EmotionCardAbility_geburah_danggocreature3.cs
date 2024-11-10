using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_geburah_danggocreature3 : EmotionCardAbilityBase
    {
        private int stack;
        private bool _effect;

        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            stack += 1;
            _effect = true;
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.cardSlotDetail.SetRecoverPoint(_owner.cardSlotDetail.GetRecoverPlayPoint() + stack);
            MakeEffect("6/Dango_Emotion_Spread", target: _owner);
        }

        public override int MaxPlayPointAdder() => stack;
        
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_effect)
            {
                _effect = false;
                Effect();
            }
            if (stack <= 0)
                return;
            _owner.cardSlotDetail.SetRecoverPoint(_owner.cardSlotDetail.GetRecoverPlayPoint() + stack);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, stack, _owner);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, stack, _owner);
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Quickness, stack, _owner);
        }

        public void Effect()
        {
            CameraFilterUtil.EarthQuake(0.18f, 0.16f, 90f, 0.45f);
            Battle.CreatureEffect.CreatureEffect original1 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/6/Dango_Emotion_Effect");
            if (original1 != null)
            {
                Battle.CreatureEffect.CreatureEffect creatureEffect1 = UnityEngine.Object.Instantiate(original1, BattleSceneRoot.Instance.transform);
                Battle.CreatureEffect.CreatureEffect creatureEffect2 = UnityEngine.Object.Instantiate(original1, BattleSceneRoot.Instance.transform);
                Battle.CreatureEffect.CreatureEffect creatureEffect3 = UnityEngine.Object.Instantiate(original1, BattleSceneRoot.Instance.transform);
                if (creatureEffect1?.gameObject.GetComponent<AutoDestruct>() == null)
                {
                    AutoDestruct autoDestruct = creatureEffect1?.gameObject.AddComponent<AutoDestruct>();
                    if (autoDestruct != null)
                    {
                        autoDestruct.time = 3f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
                if (creatureEffect2?.gameObject.GetComponent<AutoDestruct>() == null)
                {
                    AutoDestruct autoDestruct = creatureEffect2?.gameObject.AddComponent<AutoDestruct>();
                    if (autoDestruct != null)
                    {
                        autoDestruct.time = 3f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
                if (creatureEffect3?.gameObject.GetComponent<AutoDestruct>() == null)
                {
                    AutoDestruct autoDestruct = creatureEffect3?.gameObject.AddComponent<AutoDestruct>();
                    if (autoDestruct != null)
                    {
                        autoDestruct.time = 3f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
            }
            Battle.CreatureEffect.CreatureEffect original2 = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/7/Lumberjack_final_blood_1st");
            if (original2 != null)
            {
                Battle.CreatureEffect.CreatureEffect creatureEffect4 = UnityEngine.Object.Instantiate(original2, BattleSceneRoot.Instance.transform);
                Battle.CreatureEffect.CreatureEffect creatureEffect5 = UnityEngine.Object.Instantiate(original2, BattleSceneRoot.Instance.transform);
                Battle.CreatureEffect.CreatureEffect creatureEffect6 = UnityEngine.Object.Instantiate(original2, BattleSceneRoot.Instance.transform);
                if (creatureEffect4?.gameObject.GetComponent<AutoDestruct>() == null)
                {
                    AutoDestruct autoDestruct = creatureEffect4?.gameObject.AddComponent<AutoDestruct>();
                    if (autoDestruct != null)
                    {
                        autoDestruct.time = 3f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
                if (creatureEffect5?.gameObject.GetComponent<AutoDestruct>() == null)
                {
                    AutoDestruct autoDestruct = creatureEffect5?.gameObject.AddComponent<AutoDestruct>();
                    if (autoDestruct != null)
                    {
                        autoDestruct.time = 3f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
                if (creatureEffect6?.gameObject.GetComponent<AutoDestruct>() == null)
                {
                    AutoDestruct autoDestruct = creatureEffect6?.gameObject.AddComponent<AutoDestruct>();
                    if (autoDestruct != null)
                    {
                        autoDestruct.time = 3f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
            }
            MakeEffect("6/Dango_Emotion_Spread", target: _owner);
            SoundEffectPlayer.PlaySound("Creature/Danggo_LvUp");
            SoundEffectPlayer.PlaySound("Creature/Danggo_Birth");
        }
    }
}

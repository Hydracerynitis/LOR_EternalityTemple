using System;
using UnityEngine;
using LOR_DiceSystem;
using HarmonyLib;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_keter_snowqueen3 : EmotionCardAbilityBase
    {
        private bool _effect;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_effect)
                return;
            _effect = true;
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/0_K/FX_IllusionCard_0_K_Blizzard");
            if (original == null)
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
            if (creatureEffect?.gameObject.GetComponent<AutoDestruct>() != null)
                return;
            AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
            if (autoDestruct == null)
                return;
            autoDestruct.time = 3f;
            autoDestruct.DestroyWhenDisable();
            SoundEffectPlayer.PlaySound("Creature/SnowQueen_StrongAtk2");
        }
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _effect = false;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList())
            {
                if (alive != _owner)
                    alive.bufListDetail.AddBuf(new SnowQueen_Stun());
            }
        }

        public class SnowQueen_Stun : BattleUnitBuf
        {
            private Battle.CreatureEffect.CreatureEffect _aura;
            public override string keywordId => "Stun";
            public override string keywordIconId => "SnowQueen_Stun";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Stun, 1);
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (_owner.bufListDetail.GetActivatedBuf(KeywordBuf.Stun) == null || _owner.IsImmune(KeywordBuf.Stun) || _owner.bufListDetail.IsImmune(BufPositiveType.Negative))
                    return;
                _owner.view.charAppearance.ChangeMotion(ActionDetail.Damaged);
                _aura = DiceEffectManager.Instance.CreateCreatureEffect("0/SnowQueen_Emotion_Frozen", 1f, _owner.view, _owner.view);
            }

            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (_aura != null)
                {
                    UnityEngine.Object.Destroy(_aura.gameObject);
                    _aura = (Battle.CreatureEffect.CreatureEffect)null;
                    _owner.view.charAppearance.ChangeMotion(ActionDetail.Default);
                }
                Destroy();
            }
        }
    }
}

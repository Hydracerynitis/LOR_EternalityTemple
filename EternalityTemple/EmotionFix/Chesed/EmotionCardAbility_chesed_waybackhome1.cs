using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using HarmonyLib;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_chesed_waybackhome1 : EmotionCardAbilityBase
    {
        private bool _sound;
        private GameObject _aura;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            SoundEffectPlayer.PlaySound("Creature/House_Lion_Poison");
        }

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            int stack = BattleObjectManager.instance.GetAliveList(_owner.faction).Count - 2;
            if (stack > 0)
            {
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, stack, _owner);
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Protection, stack, _owner);
                _aura = DiceEffectManager.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Aura_Lion", 1f, _owner.view, _owner.view)?.gameObject;
            }
            else if (stack < 0)
            {
                if (!_sound)
                {
                    _sound = true;
                    SoundEffectPlayer.PlaySound("Creature/House_Lion_Change");
                }
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Weak, -stack, _owner);
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Vulnerable, -stack, _owner);
                _aura = DiceEffectManager.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Aura_Cat", 1f, _owner.view, _owner.view)?.gameObject;
            }
            else
                _aura = DiceEffectManager.Instance.CreateNewFXCreatureEffect("7_C/FX_IllusionCard_7_C_Aura_Lion", 1f, _owner.view, _owner.view)?.gameObject;
        }

        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            DestroyAura();
        }

        public void DestroyAura()
        {
            if (_aura == null)
                return;
            UnityEngine.Object.Destroy(_aura);
            _aura = null;
        }
    }
}

using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix.Malkuth
{
    public class EmotionCardAbility_malkuth_sorchedgirl2 : EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect _effect;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Binding, 3);
        }
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            if (dmg < _owner.MaxHp * 0.25)
                return;
            atkDice.owner.TakeDamage(dmg);
            SoundEffectManager.Instance.PlayClip("Creature/MachGirl_Explosion")?.SetGlobalPosition(_owner.view.WorldPosition);
            BattleManagerUI.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(atkDice.owner, atkDice.owner.faction, atkDice.owner.hp, atkDice.owner.breakDetail.breakGauge);
            _effect = MakeEffect("1/MatchGirl_Footfall", destroyTime: 2f, apply: false);
            _effect.AttachEffectLayer(); 
        }
        public override void OnPrintEffect(BattleDiceBehavior behavior) => _effect = null;
        public override void OnSelectEmotion() => SoundEffectPlayer.PlaySound("Creature/MatchGirl_Cry");
    }
}

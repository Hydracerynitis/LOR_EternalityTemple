using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Claw = EmotionCardAbility_bigbadwolf2.BattleUnitBuf_Emotion_Wolf_Claw;

namespace EternalityEmotion
{
    public class EmotionCardAbility_geburah_bigbadwolf2 : EmotionCardAbilityBase
    {
        private bool trigger;
        private Battle.CreatureEffect.CreatureEffect aura;
        private string path = "6/BigBadWolf_Emotion_Aura";
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (aura == null)
                return;
            DestroyAura();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!trigger)
                return;
            trigger = false;
            _owner.bufListDetail.AddBuf(new Claw());
            if (aura != null)
                return;
            aura = MakeEffect(path, target: _owner, apply: false);
        }
        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            DestroyAura();
            if (_owner.history.takeDamageAtOneRound >= _owner.MaxHp * 0.3)
                trigger = true;
        }
        public override void OnDie(BattleUnitModel killer)
        {
            base.OnDie(killer);
            DestroyAura();
        }
        private void DestroyAura()
        {
            if (aura != null && aura.gameObject != null)
                UnityEngine.Object.Destroy(aura.gameObject);
            aura = null;
        }
    }
}

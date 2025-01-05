using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityEmotion
{
    public class EmotionCardAbility_keter_quietKid2: EmotionCardAbilityBase
    {
        private Battle.CreatureEffect.CreatureEffect _aura;

        public override bool IsGiveDamageDouble() => true;

        public override bool IsTakeDamageDouble() => true;

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _aura = DiceEffectManager.Instance.CreateCreatureEffect("0/SilentGirl_Aura", 1f, _owner.view, null);
            SoundEffectPlayer.PlaySound("Creature/Slientgirl_Strong_Start");
        }

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            if (_aura != null)
                return;
            _aura = DiceEffectManager.Instance.CreateCreatureEffect("0/SilentGirl_Aura", 1f, _owner.view, null);
        }
    }
}

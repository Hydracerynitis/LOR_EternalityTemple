using System;
using LOR_DiceSystem;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_netzach_orchestra1 : EmotionCardAbilityBase
    {
        private int savedHp = 100;
        private int savedBp = 100;
        private bool effect;
        private bool trigger;
        public override void OnWaveStart()
        {
            effect = false;
            if (_owner.faction == Faction.Enemy)
                trigger = false;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!effect)
                return;
            effect = false;
            trigger = true;
            _owner.SetHp(savedHp);
            _owner.breakDetail.breakGauge = savedBp;
            _owner.cardSlotDetail.RecoverPlayPoint(_owner.cardSlotDetail.GetMaxPlayPoint());
            BattleManagerUI.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(_owner, _owner.faction, _owner.hp, _owner.breakDetail.breakGauge);
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/4_N/FX_IllusionCard_4_N_Orchestra_Start");
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
            SoundEffectPlayer.PlaySound("Creature/Sym_movment_0_clap");
        }
        public override void OnRoundEnd()
        {
            if (trigger || _owner.history.takeDamageAtOneRound < _owner.MaxHp * 0.25)
                return;
            effect = true;
        }
        public override void OnSelectEmotion()
        {
            savedHp = (int)_owner.hp;
            savedBp = _owner.breakDetail.breakGauge;
            effect = false;
            trigger = false;
        }
    }
}

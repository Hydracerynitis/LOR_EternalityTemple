using System;
using LOR_DiceSystem;
using UI;
using System.IO;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityEmotion
{
    public class EmotionCardAbility_chesed_ozma2 : EmotionCardAbilityBase
    {
        private bool trigger;
        public override void OnSelectEmotion()
        {
            trigger= false;
            _owner.bufListDetail.AddBuf(new Forgotten());
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!trigger)
            {
                Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/7_C/FX_IllusionCard_7_C_Mist");
                if (original == null)
                    return;
                Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate<Battle.CreatureEffect.CreatureEffect>(original);
                creatureEffect.gameObject.transform.SetParent(SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
                creatureEffect.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                creatureEffect.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                SoundEffectPlayer.PlaySound("Creature/Ozma_StrongAtk_Start");
                trigger = true;
            }
        }
        public class Forgotten : BattleUnitBuf
        {
            public override int SpeedDiceBreakedAdder() => 10;

            public override void OnRoundEnd()
            {
                _owner.RecoverHP((int)(_owner.MaxHp * 0.4));
                _owner.cardSlotDetail.RecoverPlayPointByCard(_owner.MaxHp);
                Destroy();
            }

            public override bool IsTargetable() => false;
        }
    }
}

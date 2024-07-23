using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix.Hokma
{
    public class EmotionCardAbility_hokma_onebadmanygood1 : EmotionCardAbilityBase
    {
        private bool _effect;

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _owner.breakDetail.RecoverBreakLife(_owner.MaxBreakLife);
            _owner.breakDetail.nextTurnBreak = false;
            _owner.turnState = BattleUnitTurnState.WAIT_CARD;
            _owner.breakDetail.RecoverBreak(_owner.breakDetail.GetDefaultBreakGauge());
            DestroyNegativeBuf(_owner.bufListDetail.GetActivatedBufList());
            DestroyNegativeBuf(_owner.bufListDetail.GetReadyBufList());
            DestroyNegativeBuf(_owner.bufListDetail.GetReadyReadyBufList());
        }

        public override void OnRoundStartOnce()
        {
            base.OnRoundStartOnce();
            if (_effect)
                return;
            _effect = true;
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/9_H/FX_IllusionCard_9_H_SkyLight");
            if (original != null)
            {
                Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate(original, BattleSceneRoot.Instance.transform);
                creatureEffect.transform.localPosition = Vector3.zero;
                creatureEffect.transform.localScale = Vector3.one;
                if (StageController.Instance.AllyFormationDirection == Direction.RIGHT)
                    creatureEffect.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            SoundEffectPlayer.PlaySound("Creature/WhiteNight_OneBad_Enter");
        }

        public override int GetBreakDamageReduction(BattleDiceBehavior behavior) => 2;

        private void DestroyNegativeBuf(List<BattleUnitBuf> bufList)
        {
            foreach (BattleUnitBuf buf in bufList)
            {
                if (buf.positiveType == BufPositiveType.Negative)
                    buf.Destroy();
            }
        }
    }
}

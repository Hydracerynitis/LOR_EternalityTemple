using System;
using LOR_DiceSystem;
using System.Collections;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix.Keter
{
    public class EmotionCardAbility_keter_heart2 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            StageController.Instance.onChangePhase += new StageController.OnChangePhaseDelegate(OnChangeStagePhase);
            SoundEffectPlayer.PlaySound("Creature/Heartbeat");
            DiceEffectManager.Instance.CreateNewFXCreatureEffect("0_K/FX_IllusionCard_0_K_Heart", 1f, _owner.view, _owner.view, 3f);
            _owner.bufListDetail.AddBuf(new Aspiration());
            _owner.view.unitBottomStatUI.SetBufs();
        }
        public override StatBonus GetStatBonus() => new StatBonus()
        {
            hpRate = 15
        };

        public override int GetSpeedDiceAdder()
        {
            int speedDiceAdder = RandomUtil.Range(1, 2);
            _owner.ShowTypoTemporary(_emotionCard, 0, ResultOption.Sign, speedDiceAdder);
            return speedDiceAdder;
        }

        public class Aspiration : BattleUnitBuf
        {
            public override string keywordId => "HeartofAspiration_Heart";

            public override BufPositiveType positiveType => BufPositiveType.Positive;

            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
        }
    }
}

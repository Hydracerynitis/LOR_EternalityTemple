using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix
{
    public class EmotionCardAbility_netzach_galaxyChild1 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _owner.bufListDetail.AddBuf(new Pebble());
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.bufListDetail.AddBuf(new Pebble());
        }
        public class Pebble : BattleUnitBuf
        {
            private BehaviourDetail resonate;
            public override string keywordId => "EF_Pebble";
            public override string keywordIconId => "GalaxyBoy_Stone";
            public override string bufActivatedText => BattleEffectTextsXmlList.Instance.GetEffectTextDesc(keywordId,GetText());
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                Resonate();
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                Resonate();
            }
            private void Resonate()
            {
                resonate= RandomUtil.SelectOne(BehaviourDetail.Penetrate, BehaviourDetail.Slash, BehaviourDetail.Hit);
            }
            private string GetText()
            {
                string prompt = "";
                switch(resonate)
                {
                    case BehaviourDetail.Slash:
                        prompt = "ui_dice_slash";
                        break;
                    case BehaviourDetail.Hit:
                        prompt = "ui_dice_hit";
                        break;
                    case BehaviourDetail.Penetrate:
                        prompt = "ui_dice_penetrate";
                        break;
                }
                return TextDataModel.GetText(prompt);
            }
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                if (atkDice.Detail == resonate)
                {
                    _owner.RecoverHP(RandomUtil.Range(4, 7));
                    if (Singleton<StageController>.Instance.IsLogState())
                    {
                        _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_O", 2f);
                        _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/GalaxyBoy_Heal");
                    }
                    else
                    {
                        SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_O", 1f, _owner.view, _owner.view, 2f);
                        SoundEffectPlayer.PlaySound("Creature/GalaxyBoy_Heal");
                    }
                }
                else
                {
                    _owner.LoseHp(RandomUtil.Range(1, 3));
                    if (Singleton<StageController>.Instance.IsLogState())
                    {
                        _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_X", 2f);
                        _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/GalaxyBoy_Deal");
                    }
                    else
                    {
                        SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("4_N/FX_IllusionCard_4_N_GalaxyCard_X", 1f, _owner.view, _owner.view, 2f);
                        SoundEffectPlayer.PlaySound("Creature/GalaxyBoy_Deal");
                    }
                }
            }
        }
    }
}

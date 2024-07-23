using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix.Binah
{
    public class EmotionCardAbility_binah_bigbird1 : EmotionCardAbilityBase
    {
        private bool _effect;

        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.cardSlotDetail.SetRecoverPoint(1 + _owner.cardSlotDetail.GetRecoverPlayPoint());
        }

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _owner.cardSlotDetail.RecoverPlayPoint(_owner.cardSlotDetail.GetMaxPlayPoint());
            _owner.cardSlotDetail.SetRecoverPoint(1 + _owner.cardSlotDetail.GetRecoverPlayPoint());
        }

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_effect)
                return;
            _effect = true;
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/BigBird_Filter_Bg", false, 2f);
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/BigBird_Filter_Fg", false, 2f);
            SoundEffectPlayer.PlaySound("Creature/Bigbird_Eyes");
        }

        public override void OnRoundEnd()
        {
            base.OnRoundEnd();
            List<BattleUnitModel> list = new List<BattleUnitModel>();
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
            {
                if (alive != _owner)
                {
                    alive.cardSlotDetail.SetRecoverPointDefault();
                    list.Add(alive);
                }
            }
            if (list.Count <= 0)
                return;
            RandomUtil.SelectOne(list)?.cardSlotDetail.SetRecoverPoint(0);
        }
    }
}

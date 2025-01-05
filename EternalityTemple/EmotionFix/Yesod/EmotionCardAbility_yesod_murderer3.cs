using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_yesod_murderer3 : EmotionCardAbilityBase
    {
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (behavior == null)
                return;
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                min = -1,
                max = 3
            });
        }
        public override void OnSelectEmotion()
        {
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Murderer_Filter", false, 2f);
            SoundEffectPlayer.PlaySound("Creature/Abandoned_Angry");
        }
    }
}

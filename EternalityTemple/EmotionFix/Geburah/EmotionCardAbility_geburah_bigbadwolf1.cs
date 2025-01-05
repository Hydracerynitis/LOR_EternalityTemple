using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityEmotion
{
    public class EmotionCardAbility_geburah_bigbadwolf1 : EmotionCardAbilityBase
    {
        private BattleDiceBehavior last;
        private bool win;
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Wolf_Filter_Sheep", false, 2f);
        }
        public override void OnUseCard(BattlePlayingCardDataInUnitModel curCard)
        {
            base.OnUseCard(curCard);
            win = false;
            if (curCard == null)
                return;
            BattleDiceBehavior[] array = curCard.cardBehaviorQueue?.ToArray();
            if (array == null || array.Length == 0)
                return;
            last = array[array.Length - 1];
        }
        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            BattlePlayingCardDataInUnitModel card = behavior?.card;
            if (card == null || behavior == last)
                return;
            win=true;
            card.ApplyDiceStatBonus(DiceMatch.LastDice, new DiceStatBonus()
            {
                power = RandomUtil.Range(1, 2)
            });
        }

        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            if (behavior != last)
                return;
            SoundEffectManager.Instance.PlayClip("Creature/Wolf_Bite");
            if(win)
                _owner.RecoverHP(RandomUtil.Range(3, 7));
        }
    }
}

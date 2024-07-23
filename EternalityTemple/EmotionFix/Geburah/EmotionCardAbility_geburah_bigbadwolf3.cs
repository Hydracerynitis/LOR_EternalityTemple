using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseMod;

namespace EmotionalFix.Geburah
{
    public class EmotionCardAbility_geburah_bigbadwolf3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Wolf_Filter_Eye", false, 2f);
        }
        public override void OnStartBattle()
        {
            DiceCardXmlInfo cardItem = ItemXmlDataList.instance.GetCardItem(Tools.MakeLorId(1106201));
            List<BattleDiceBehavior> behaviourList = new List<BattleDiceBehavior>();
            int num = 0;
            foreach (DiceBehaviour diceBehaviour2 in cardItem.DiceBehaviourList)
            {
                BattleDiceBehavior battleDiceBehavior = new BattleDiceBehavior
                {
                    behaviourInCard = diceBehaviour2
                };
                battleDiceBehavior.SetIndex(num++);
                behaviourList.Add(battleDiceBehavior);
            }
            _owner.cardSlotDetail.keepCard.AddBehaviours(BattleDiceCardModel.CreatePlayingCard(cardItem), behaviourList);
        }
    }
}

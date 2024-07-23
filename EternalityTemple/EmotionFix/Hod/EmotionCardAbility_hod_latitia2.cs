using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using BaseMod;
using UnityEngine;
using HyperCard;

namespace EmotionalFix.Hod
{
    public class EmotionCardAbility_hod_latitia2 : EmotionCardAbilityBase
    {
        private bool GiftCriteria(BattleUnitModel unit) => unit.allyCardDetail.GetHand().Count > 0 && unit.allyCardDetail.GetHand().FindAll(x => !x.GetBufList().Exists(y => y is Gift)).Count > 0;
        public override void OnRoundStart()
        {
            BattleUnitModel giftee=RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(_owner.faction).FindAll(x => GiftCriteria(x)));
            if (giftee == null)
                return;
            BattleDiceCardModel randomCardInHand = RandomUtil.SelectOne(giftee.allyCardDetail.GetHand().FindAll(x => !x.GetBufList().Exists(y => y is Gift)));
            randomCardInHand.AddBuf(new Gift());
            randomCardInHand.SetAddedIcon("Latitia_Heart");
            GiftIndicator GI = giftee.bufListDetail.FindBuf<GiftIndicator>();
            if (GI == null)
                GI = giftee.bufListDetail.AddBufByEtc<GiftIndicator>(1);
            GI.CheckGiftCount();
        }
        public class GiftIndicator: BattleUnitBuf
        {
            public override string keywordId => "EF_Gift_indicator_Eternal";
            public override string keywordIconId => "SpiderBud_Cocoon";
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                CheckGiftCount();
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                CheckGiftCount() ;
            }
            public void CheckGiftCount()
            {
                stack= _owner.allyCardDetail.GetAllDeck().FindAll(x => x._bufList.Exists(y => y is Gift)).Count;
                if (stack <= 0)
                    Destroy();
            }
        }
        public class Gift: BattleDiceCardBuf
        {
            private int turn;
            private bool used;
            public override void OnUseCard(BattleUnitModel owner)
            {
                base.OnUseCard(owner);
                used = true;
            }
            public override void OnUseCard(BattleUnitModel owner, BattlePlayingCardDataInUnitModel playingCard)
            {
                base.OnUseCard(owner, playingCard);
                playingCard.ApplyDiceStatBonus(DiceMatch.AllDice,new DiceStatBonus() { power = 1 });
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (used)
                {
                    _card.RemoveAddedIcon("Latitia_Heart");
                    Destroy();
                }
                else
                {
                    ++turn;
                    if (turn < 2)
                        return;
                    BattleUnitModel owner = _card?.owner;
                    owner?.TakeDamage(RandomUtil.Range(3, 8), DamageType.Card_Ability, owner);
                    new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/Latitia_Filter_Grey", false, 2f);
                    _card.temporary = true;
                    Destroy();
                }
            }
        }
    }
}

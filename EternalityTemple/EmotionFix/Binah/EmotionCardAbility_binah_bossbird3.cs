using System;
using LOR_DiceSystem;
using UI;
using UnityEngine;
using Sound;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace EmotionalFix
{
    public class EmotionCardAbility_binah_bossbird3 : EmotionCardAbilityBase
    {
        public override void OnSelectEmotion()
        {
            _owner.bufListDetail.AddBuf(new Smallbird());
        }
        public override void OnWaveStart()
        {
            _owner.bufListDetail.AddBuf(new Smallbird());
        }
        public void Destroy()
        {
            BattleUnitBuf Buff = _owner.bufListDetail.GetActivatedBufList().Find(x => x is Smallbird);
            if (Buff != null)
                Buff.Destroy();
            BattleManagerUI.Instance.ui_unitListInfoSummary.UpdateCharacterProfile(_owner, _owner.faction, _owner.hp, _owner.breakDetail.breakGauge);
        }
        public class Smallbird : BattleUnitBuf
        {
            public override string keywordIconId => "ApocalypseBird_SmallBeak";
            public override string keywordId => "EF_Smallbird";
            private List<BattleDiceCardModel> _cards = new List<BattleDiceCardModel>();
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                stack = 0;
            }
            public override void OnRoundStart()
            {
                ResetCardsCost();
                BattleUnitModel alive = RandomUtil.SelectOne(BattleObjectManager.instance.GetAliveList_opponent(_owner.faction));
                List<BattleDiceCardModel> hand = alive.allyCardDetail.GetHand();
                int num = 0;
                while (num < 2)
                {
                    ++num;
                    if (hand.Count > 0)
                    {
                        BattleDiceCardModel battleDiceCardModel = RandomUtil.SelectOne(hand);
                        battleDiceCardModel.AddCost(1);
                        _cards.Add(battleDiceCardModel);
                        hand.Remove(battleDiceCardModel);
                    }
                }
            }
            public override void OnRoundEnd() => ResetCardsCost();
            public override void Destroy()
            {
                base.Destroy();
                ResetCardsCost();
            }
            private void ResetCardsCost()
            {
                if (_cards.Count <= 0)
                    return;
                foreach (BattleDiceCardModel card in _cards)
                    card.AddCost(-1);
                _cards.Clear();
            }
        }
    }
}

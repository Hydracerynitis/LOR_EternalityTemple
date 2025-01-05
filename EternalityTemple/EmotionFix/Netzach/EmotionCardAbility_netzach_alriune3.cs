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
    public class EmotionCardAbility_netzach_alriune3 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
            if (aliveList.Count <= 0)
                return;
            RandomUtil.SelectOne(aliveList).bufListDetail.AddBuf(new EmotionCardAbility_alriune3.BattleUnitBuf_Emotion_Alriune(_owner));
        }
    }
}

using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_tiphereth_queenofhatred3 : EmotionCardAbilityBase
    {
        private int cnt;

        public override void OnRoundStart()
        {
            base.OnRoundStart();
            cnt = 0;
        }
        public override void OnLoseParrying(BattleDiceBehavior behavior)
        {
            base.OnLoseParrying(behavior);
            if (behavior?.card?.target == null)
                return;
            ++cnt;
            _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_HeartBroken", 2f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Oz_Atk_Boom");
            if (Helper.SearchEmotion(_owner, "NihilClown_fusion_Enemy") == null)
                _owner.TakeBreakDamage(RandomUtil.Range(2,4), DamageType.Emotion, _owner);
            if (cnt <= 2)
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, 1, _owner);
            if (cnt == 3)
                ReusePage();
        }

        private void ReusePage()
        {
            if (_owner.currentDiceAction == null)
                return;
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList_opponent(_owner.faction);
            if (aliveList.Count <= 0)
                return;
            StageController.Instance.AddAllCardListInBattle(_owner.currentDiceAction, RandomUtil.SelectOne(aliveList));
            _owner.breakDetail.LoseBreakGauge((int)(_owner.breakDetail.GetDefaultBreakGauge() * 0.1));
        }
    }
}

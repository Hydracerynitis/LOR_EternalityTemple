using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;

namespace EmotionalFix
{
    public class EmotionCardAbility_tiphereth_knightofdespair1 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            BattleUnitBuf Gaho = new BattleUnitBuf_Gaho();
            Weakest(BattleObjectManager.instance.GetAliveList(_owner.faction)).bufListDetail.AddBuf(Gaho);
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/KnightOfDespair_Gaho", false, 2f);
            SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/KnightOfDespair_Gaho")?.SetGlobalPosition(_owner.view.WorldPosition);
        }
        private BattleUnitModel Weakest(List<BattleUnitModel> list)
        {
            if (list.Count == 0)
                return null;
            int num = 10000;
            List<BattleUnitModel> battleUnitModelList = new List<BattleUnitModel>();
            foreach (BattleUnitModel alive in list)
            {
                int hp = (int)alive.hp;
                if (hp < num)
                {
                    battleUnitModelList.Clear();
                    battleUnitModelList.Add(alive);
                    num = hp;
                }
                else if (hp == num)
                    battleUnitModelList.Add(alive);
            }
            if (battleUnitModelList.Count == 0)
                return null;
            return RandomUtil.SelectOne(battleUnitModelList);
        }
        public class BattleUnitBuf_Gaho : BattleUnitBuf
        {
            public override string keywordId => "EF_Gaho";
            public override string keywordIconId => "Gaho";
            public override int GetDamageIncreaseRate() => -30;
            public override int GetBreakDamageIncreaseRate() => -30;
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}

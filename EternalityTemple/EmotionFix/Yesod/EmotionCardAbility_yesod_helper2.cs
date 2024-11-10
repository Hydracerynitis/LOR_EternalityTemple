using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;
using Sound;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_yesod_helper2: EmotionCardAbilityBase
    {
        private int cnt;
        private int quick;
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (quick <= 0)
                return;
            SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("2_Y/FX_IllusionCard_2_Y_Scan_Start", 1f, _owner.view, _owner.view, 3f);
            SoundEffectPlayer.PlaySound("Creature/Helper_On");
            quick = 0;
        }

        public override void OnWinParrying(BattleDiceBehavior behavior)
        {
            base.OnWinParrying(behavior);
            ++cnt;
            if (cnt < 3)
                return;
            cnt %= 3;
            _owner.cardSlotDetail.RecoverPlayPoint(1);
            ++quick;
            _owner.battleCardResultLog?.SetNewCreatureAbilityEffect("2_Y/FX_IllusionCard_2_Y_Scan", 3f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Helper_On");
        }

        public override void OnRoundEnd()
        {
            if (quick <= 0)
                return;
            for (int index = 0; index < quick; ++index)
                _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Quickness, RandomUtil.Range(1,2),_owner);
        }
    }
}

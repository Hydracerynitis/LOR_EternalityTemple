using System;
using UnityEngine;
using Sound;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_malkuth_queenbee3 : EmotionCardAbilityBase
    {
        private int _dmg;
        public override void OnRoundStart()
        {
            int stack = _dmg / (int)(0.1 * _owner.MaxHp);
            if (stack > 0)
            {
                new GameObject().AddComponent<SpriteFilter_Queenbee_Spore>().Init("EmotionCardFilter/QueenBee_Filter_Spore", false, 2f);
                SingletonBehavior<SoundEffectManager>.Instance.PlayClip("Creature/QueenBee_Funga")?.SetGlobalPosition(_owner.view.WorldPosition);
                foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction))
                    alive.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, stack);
            }
            _dmg = 0;
        }

        public override void OnRoundEnd()
        {
            _dmg = _owner.history.takeDamageAtOneRound;
        }
    }
}

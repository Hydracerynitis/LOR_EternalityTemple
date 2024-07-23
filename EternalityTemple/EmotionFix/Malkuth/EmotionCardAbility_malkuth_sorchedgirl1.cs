using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix.Malkuth
{
    public class EmotionCardAbility_malkuth_sorchedgirl1 : EmotionCardAbilityBase
    {
        public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
        {
            if (atkDice.owner == null)
                return;
            if (dmg < _owner.MaxHp * 0.01)
                return;
            atkDice?.owner?.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Burn, RandomUtil.Range(1,3), _owner);
            _owner.battleCardResultLog?.SetCreatureAbilityEffect("1/MatchGirl_Ash", 1f);
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/MatchGirl_Barrier");

        }
    }
}

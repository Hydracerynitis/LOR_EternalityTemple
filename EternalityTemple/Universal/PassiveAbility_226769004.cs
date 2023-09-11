using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Universal
{
    public class PassiveAbility_226769004: PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (EternalityParam.PickedEmotionCard < 2)
                EternalityParam.PickedEmotionCard = 2;
        }
    }
}

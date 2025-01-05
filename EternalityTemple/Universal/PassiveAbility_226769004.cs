using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EternalityTemple
{
    public class PassiveAbility_226769004: PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            EternalityParam.PickedEmotionCard = -5;
        }
    }
}

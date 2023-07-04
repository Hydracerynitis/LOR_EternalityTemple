using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Kaguya
{
    public class PassiveAbility_226769001: PassiveAbilityBase
    {
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            owner.bufListDetail.AddBuf(new BattleUnitBuf_Puzzle());
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769101));
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769102));
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769103));
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769104));
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769105));
        }
    }
}

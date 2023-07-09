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
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
                unit.bufListDetail.AddBuf(new BattleUnitBuf_PuzzleBuf());
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769011));
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769012));
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769013));
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769014));
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769015));
        }
        public override void OnRoundEndTheLast_ignoreDead()
        {
            EternalityInitializer.ResetSpeedDiceColor();
        }
    }
}

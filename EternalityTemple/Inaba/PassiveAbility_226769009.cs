using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EternalityTemple.Yagokoro;

namespace EternalityTemple.Inaba
{
    public class PassiveAbility_226769009 : PassiveAbilityBase
    {
        public override void OnRoundEndTheLast()
        {
            if (BattleObjectManager.instance.GetAliveList(base.owner.faction).Find((BattleUnitModel x) => x.bufListDetail.HasBuf<BattleUnitBuf_Moon3>()) != null)
            {
                if (EternalityInitializer.InabaBufGainNum >= 900)
                {
                    BattleUnitBuf_InabaBuf2.AddReadyStack(RandomUtil.SelectOne<BattleUnitModel>(BattleObjectManager.instance.GetAliveList_opponent(owner.faction)), 1);
                }
            }
        }
        public override void OnWaveStart()
        {
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769034));
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769035));
            owner.personalEgoDetail.AddCard(new LorId(EternalityInitializer.packageId, 226769036));
        }
    }
}
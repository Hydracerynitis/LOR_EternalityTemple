using EternalityTemple.Universal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Inaba
{
    public class PassiveAbility_226769010 : PassiveAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            BattleUnitBuf_InabaBuf2.AddStack(owner, 1);
            target_226769135 = null;
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            foreach (BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList(false))
                battleUnitModel.bufListDetail.AddBuf(new BattleUnitBuf_InabaDmgCheck());
            BattleUnitBuf_InabaBuf1.AddStack(owner, EternalityParam.GetFaction(owner.faction).InabaBufGainNum);
        }
        public class BattleUnitBuf_InabaDmgCheck : BattleUnitBuf
        {
            public override void OnTakeDamageByAttack(BattleDiceBehavior atkDice, int dmg)
            {
                base.OnTakeDamageByAttack(atkDice, dmg);
                BattleUnitModel battleUnitModel = BattleObjectManager.instance.GetAliveList(false).Find((BattleUnitModel x) => x.passiveDetail.HasPassive<PassiveAbility_226769010>());
                if (battleUnitModel == null)
                {
                    Destroy();
                    return;
                }
                BattleUnitBuf_InabaBuf1.AddStack(battleUnitModel, dmg);
            }
        }
		private void AddNewCard(int id)
		{
			BattleDiceCardModel battleDiceCardModel = this.owner.allyCardDetail.AddTempCard(new LorId(EternalityInitializer.packageId,id));
			if (battleDiceCardModel != null)
			{
				battleDiceCardModel.SetPriorityAdder(9999);
			}
		}
		public override void OnRollSpeedDice()
		{
            specialCardColdDown--;
            if (owner.faction == Faction.Player)
            {
                return;
            }
            if (BattleUnitBuf_InabaBuf1.GetStack(owner)>=150 && BattleUnitBuf_InabaBuf7.GetStack(owner) <= 0)
            {
                this.AddNewCard(226769134);
            }
            List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList(base.owner.faction);
            if(aliveList.Count<=1)
            {
                return;
            }
            if (aliveList.Find((BattleUnitModel x) => x.breakDetail.breakGauge <= 130) != null && RandomUtil.valueForProb <= 0.4f)
            {
                aliveList.Remove(owner);
                aliveList.Sort((BattleUnitModel x, BattleUnitModel y) => (int)(x.breakDetail.breakGauge - y.breakDetail.breakGauge));
                this.target_226769135 = aliveList[0];
                this.AddNewCard(226769136);
            }
            else if(RandomUtil.valueForProb <= 0.3f && specialCardColdDown <= 0)
            {
                this.AddNewCard(226769135);
                specialCardColdDown = 2;
            }
		}
        public override BattleUnitModel ChangeAttackTarget(BattleDiceCardModel card, int idx)
        {
            BattleUnitModel result = null;
            if(card.GetID().packageId != EternalityInitializer.packageId || owner.faction == Faction.Player)
            {
                return result;
            }
            if (card.GetID().id==226769136)
            {
                result = target_226769135;
            }
            if (card.GetID().id==226769135)
            {
                List<BattleUnitModel> aliveList2 = BattleObjectManager.instance.GetAliveList(owner.faction);
                aliveList2.Remove(owner);
                if (aliveList2.Count > 0)
                    result = RandomUtil.SelectOne(aliveList2);
            }
            return result;
        }
        private BattleUnitModel target_226769135;
        private int specialCardColdDown;
    }
}
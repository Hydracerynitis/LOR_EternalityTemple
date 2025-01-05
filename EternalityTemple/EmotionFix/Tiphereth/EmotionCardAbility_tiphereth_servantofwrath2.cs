using System;
using UnityEngine;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Friend = EmotionCardAbility_servantofwrath2.BattleUnitBuf_Emotion_Wrath_Friend;

namespace EternalityEmotion
{
    public class EmotionCardAbility_tiphereth_servantofwrath2 : EmotionCardAbilityBase
    {
        public override void OnSucceedAttack(BattleDiceBehavior behavior)
        {
            base.OnSucceedAttack(behavior);
            BattleUnitModel target = behavior?.card?.target;
            if (target == null || target.faction==_owner.faction)
                return;
            if(GetAliveFriend() == null)
                target.bufListDetail.AddBuf(new Friend());
            else
            {
                if (GetAliveFriend() != target)
                    return;
                target.battleCardResultLog?.SetNewCreatureAbilityEffect("5_T/FX_IllusionCard_5_T_ATKMarker", 1.5f);
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Angry_R_StrongAtk");
            }
        }
        public override void OnKill(BattleUnitModel target)
        {
            base.OnKill(target);
            if (target.bufListDetail.GetActivatedBufList().Find(y => y is Friend) == null)
                return;
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(Faction.Player))
            {
                alive.cardSlotDetail.RecoverPlayPoint(3);
                alive.breakDetail.RecoverBreak((int)(_owner.MaxHp*0.1));
                alive.RecoverHP((int)(_owner.breakDetail.GetDefaultBreakGauge() * 0.1));
            }
            _owner.battleCardResultLog?.SetAfterActionEvent(new BattleCardBehaviourResult.BehaviourEvent(KillEffect));
            _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Angry_Vert2");
        }
        public void KillEffect()
        {
            CameraFilterUtil.EarthQuake(0.08f, 0.02f, 50f, 0.6f);
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/5_T/FX_IllusionCard_5_T_SmokeWater");
            if (original == null)
                return;
            Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate(original, BattleSceneRoot.Instance.transform);
            if (creatureEffect?.gameObject.GetComponent<AutoDestruct>() != null)
                return;
            AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
            if (autoDestruct == null)
                return;
            autoDestruct.time = 3f;
            autoDestruct.DestroyWhenDisable();
        }
        public void Destroy()
        {
            if (GetAliveFriend().bufListDetail.GetActivatedBufList().Find(x => x is Friend) is Friend friend)
                friend.Destroy();
        }
        private BattleUnitModel GetAliveFriend() => BattleObjectManager.instance.GetAliveList(_owner.faction==Faction.Player?Faction.Enemy:Faction.Player).Find(x => x.bufListDetail.GetActivatedBufList().Find(y => y is Friend) != null);
    }
}

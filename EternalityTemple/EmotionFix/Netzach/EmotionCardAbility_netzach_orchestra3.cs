using System;
using LOR_DiceSystem;
using UI;
using Sound;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class EmotionCardAbility_netzach_orchestra3 : EmotionCardAbilityBase
    {
        private bool trigger;
        public override void OnMakeBreakState(BattleUnitModel target)
        {
            base.OnMakeBreakState(target);
            trigger = true;
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (!trigger)
                return;
            trigger = false;
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/4_N/FX_IllusionCard_4_N_Orchestra_Light");
            if (original != null)
            {
                Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate(original, SingletonBehavior<BattleSceneRoot>.Instance.transform);
                if (creatureEffect?.gameObject.GetComponent<AutoDestruct>() == null)
                {
                    AutoDestruct autoDestruct = creatureEffect?.gameObject.AddComponent<AutoDestruct>();
                    if (autoDestruct != null)
                    {
                        autoDestruct.time = 3f;
                        autoDestruct.DestroyWhenDisable();
                    }
                }
            }
            foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(_owner.faction == Faction.Player ? Faction.Enemy : Faction.Player))
                alive.cardSlotDetail.LosePlayPoint(RandomUtil.Range(1, 6));
            SoundEffectPlayer.PlaySound("Creature/Sym_movment_5_finale");
        }
    }
}

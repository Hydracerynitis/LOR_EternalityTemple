using System;
using LOR_DiceSystem;
using Battle.CameraFilter;
using Battle.CreatureEffect;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using System.Collections;

namespace EmotionalFix.Netzach
{
    public class EmotionCardAbility_netzach_galaxyChild3 : EmotionCardAbilityBase
    {
        private int _roundCount;
        private List<Battle.CreatureEffect.CreatureEffect> _recoverEffects = new List<Battle.CreatureEffect.CreatureEffect>();
        public override void OnRoundStart()
        {
            if (_roundCount >= 3)
                return;
            ++_roundCount;
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(_owner.faction))
            {
                int num = (int)(unit.MaxHp * 0.1);
                unit.bufListDetail.AddBuf(new EmotionCardAbility_galaxyChild3.BattleUnitBuf_galaxyChild_Friend());
                unit.RecoverHP(num);
                unit.ShowTypoTemporary(_emotionCard, 0, ResultOption.Default, num);
                Battle.CreatureEffect.CreatureEffect creatureEffect = MakeEffect("4/GalaxyBoy_Recover", target: unit);
                _recoverEffects.Add(creatureEffect);
                (creatureEffect as CreatureEffect_Hit).SetPerm();
            }
        }
        public override void OnSelectEmotion() => _owner.view.unitBottomStatUI.SetBufs();
        public override void OnDrawCard()
        {
            foreach (Battle.CreatureEffect.CreatureEffect recoverEffect in _recoverEffects)
            {
                if (recoverEffect!=null)
                    recoverEffect.ManualDestroy();
            }
            _recoverEffects.Clear();
        }
        public override Sprite GetAbilityBufIcon() => Resources.Load<Sprite>("Sprites/BufIcon/Ability/GalaxyBoy_Stone");
    }
}

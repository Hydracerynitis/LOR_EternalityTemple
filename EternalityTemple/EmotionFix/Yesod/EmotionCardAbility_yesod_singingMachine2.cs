using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_yesod_singingMachine2 : EmotionCardAbilityBase
    {
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            GetBuf();
        }
        private void GetBuf()
        {
            if (_owner.bufListDetail.GetActivatedBufList().Find(x => x is Rhythm) is Rhythm singingMachineRhythm)
            {
                ++singingMachineRhythm.stack;
                _owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, _owner);
            }
            else
            {
                _owner.bufListDetail.AddBuf(new Rhythm(1));
            }
        }
        public class Rhythm : BattleUnitBuf
        {
            private Battle.CreatureEffect.CreatureEffect _effect;
            private int reserve;
            public override string keywordIconId => "SingingMachine_Rhythm";
            public override string keywordId => "EF_Rhythm_Eternal";
            private int _count;
            public Rhythm(int value = 0)
            {
                stack = value;
                _count = 0;
                reserve = value == 0 ? 1 : 0;
            }
            public override void Init(BattleUnitModel owner)
            {
                base.Init(owner);
                if (stack <= 0)
                    return;
                _owner.bufListDetail.AddKeywordBufThisRoundByEtc(KeywordBuf.Strength, 1, _owner);
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                if (stack > 0)
                    stack = 0;
                stack += reserve;
                reserve = 0;
                if (stack <= 0)
                    Destroy();
                else
                    _owner.bufListDetail.AddKeywordBufByEtc(KeywordBuf.Strength, stack, _owner);
            }
            public override void OnRoundStart()
            {
                base.OnRoundStart();
                if (_effect != null)
                    return;
                _effect = DiceEffectManager.Instance.CreateCreatureEffect("4/SingingMachine_NoteAura", 1f, _owner.view, null);
                _effect?.SetLayer("Character");
                SoundEffectManager.Instance.PlayClip("Creature/Singing_Rhythm")?.SetGlobalPosition(_owner.view.WorldPosition);
            }
            public override void Destroy()
            {
                if (_effect != null)
                {
                    UnityEngine.Object.Destroy(_effect.gameObject);
                    _effect = null;
                }
                base.Destroy();
            }
            public override void OnLayerChanged(string layerName)
            {
                if (_effect != null)
                    return;
                _effect.SetLayer(layerName);
            }
            public void Reserve(int value = 1) => reserve += value;
            public override void OnSuccessAttack(BattleDiceBehavior behavior)
            {
                base.OnSuccessAttack(behavior);
                if (stack == 0)
                    return;
                _count += 1;
                if (_count < 3)
                    return;
                _count = 0;
                _owner.battleCardResultLog?.SetCreatureEffectSound("Creature/Singing_Rhythm");
                Ability();
            }
            private void Ability()
            {
                List<BattleUnitModel> aliveList = BattleObjectManager.instance.GetAliveList();
                if (aliveList.Count == 0)
                    return;
                BattleUnitModel victim = RandomUtil.SelectOne(aliveList);
                if (!(victim.bufListDetail.GetActivatedBufList().Find(x => x is Rhythm) is Rhythm singingMachineRhythm))
                    victim.bufListDetail.AddBuf(new Rhythm());
                else
                    singingMachineRhythm.Reserve();
            }
        }
    }
}

using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_geburah_danggocreature2 : EmotionCardAbilityBase
    {
        private bool _effect;
        List<BattleUnitModel> dead=new List<BattleUnitModel>();
        List<BattleDiceCardModel> remain = new List<BattleDiceCardModel>();
        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            _effect = false;
        }
        public override void OnDieOtherUnit(BattleUnitModel unit)
        {
            if (unit == _owner || _owner.faction==unit.faction)
                return;
            _effect = true;
            dead.Add(unit);
        }
        public override void OnDrawCard()
        {
            foreach(BattleUnitModel corpse in dead)
            {
                List<BattleDiceCardModel> deadman = corpse.allyCardDetail.GetAllDeck();
                for (int i = 0; i < 3; i++)
                {
                    BattleDiceCardModel deadcard = RandomUtil.SelectOne(deadman);
                    deadman.Remove(deadcard);
                    _owner.allyCardDetail.AddCardToHand(deadcard);
                    remain.Add(deadcard);
                }
            }
            dead.Clear();
        }
        public override void OnRoundStart()
        {
            base.OnRoundStart();
            if (_effect)
            {
                _effect = false;
                _owner.bufListDetail.AddBuf(new SpeedDiceBonus(dead.Count));
                DiceEffectManager.Instance.CreateNewFXCreatureEffect("6_G/FX_IllusionCard_6_G_Shout", 1f, _owner.view, _owner.view, 3f);
                CameraFilterUtil.EarthQuake(0.08f, 0.02f, 50f, 0.3f);
                SoundEffectManager.Instance.PlayClip("Creature/Danggo_Lv2_Shout");
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            _owner.allyCardDetail.AddCardToDeck(remain);
            _owner.allyCardDetail.Shuffle();
        }
        public class SpeedDiceBonus : BattleUnitBuf
        {
            private int count;
            public SpeedDiceBonus(int count)
            {
                this.count= count;
            }
            public override int SpeedDiceNumAdder()
            {
                return count;
            }
            public override void OnRoundEnd()
            {
                base.OnRoundEnd();
                Destroy();
            }
        }
    }
}

using System;
using LOR_DiceSystem;
using System.Collections.Generic;
using Sound;
using Battle.CreatureEffect;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_hod_shyLook1 : EmotionCardAbilityBase
    {
        private int _currentFace = 1;
        private float elap;
        private float freq = 1f;
        private CreatureEffect_Emotion_Face face;

        private int CurrentFace
        {
            get => _currentFace;
            set
            {
                _currentFace = value;
                SetFace();
            }
        }

        private void SetFace()
        {
            if (face == null)
                return;
            face.SetFace(CurrentFace);
        }

        private void GenFace()
        {
            if (face != null)
                UnityEngine.Object.Destroy(face.gameObject);
            face = SingletonBehavior<DiceEffectManager>.Instance.CreateNewFXCreatureEffect("3_H/FX_IllusionCard_3_H_Look", 1f, _owner.view, _owner.view)?.GetComponent<CreatureEffect_Emotion_Face>();
            SetFace();
        }

        public override void OnWaveStart()
        {
            CurrentFace = RandomUtil.Range(0, 4);
            GenFace();
        }

        public override void OnSelectEmotion()
        {
            CurrentFace = RandomUtil.Range(0, 4);
            GenFace();
            Debug.Log("aaaaaaaaaaaaaaaa");
            SoundEffectPlayer.PlaySound("Creature/Shy_Smile");
        }
        
        
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            int num = 0;
            Debug.Log("bbbbbbbbbbbb");
            switch (CurrentFace)
            {
                case 0:
                    num = -2;
                    break;
                case 1:
                    num = -1;
                    break;
                case 2:
                    num = 0;
                    break;
                case 3:
                    num = 1;
                    break;
                case 4:
                    num = 2;
                    break;
            }
            if (num == 0)
                return;
            _owner.battleCardResultLog?.SetEmotionAbility(false, _emotionCard, 0, ResultOption.Sign, num);
            behavior.ApplyDiceStatBonus(new DiceStatBonus()
            {
                power = num
            });
        }
        /*
         public override void OnStartBattle()
        {
            base.OnStartBattle();
            foreach(BattleUnitModel battleUnitModel in BattleObjectManager.instance.GetAliveList())
            {
                battleUnitModel.bufListDetail.AddBuf(new PowerDecide(CurrentFace, _emotionCard));
            }
        }
        public class PowerDecide : BattleUnitBuf
        {
            public BattleEmotionCardModel cardModel;
            public PowerDecide(int s, BattleEmotionCardModel model)
            {
                stack = s;
                cardModel = model;
            }
            public override void BeforeRollDice(BattleDiceBehavior behavior)
            {
                int num = 0;
                switch (stack)
                {
                    case 0:
                        num = -2;
                        break;
                    case 1:
                        num = -1;
                        break;
                    case 2:
                        num = 0;
                        break;
                    case 3:
                        num = 1;
                        break;
                    case 4:
                        num = 2;
                        break;
                }
                if (num == 0)
                    return;
                _owner.battleCardResultLog?.SetEmotionAbility(false, cardModel, 0, ResultOption.Sign, num);
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = num
                });
            }
        }
        */
        public override void OnFixedUpdateInWaitPhase(float delta)
        {
            elap += delta;
            if (elap <= freq)
                return;
            CurrentFace = RandomUtil.Range(0, 4);
            elap = 0.0f;
        }
    }
}

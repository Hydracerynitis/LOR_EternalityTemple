using Sound;
using UnityEngine;

namespace EternalityEmotion
{
    public class EmotionCardAbility_hokma_silence1 : EmotionCardAbilityBase
    {
        private bool damaged;
        private bool rolled;
        private float _elapsed;
        private bool _bTimeLimitOvered;
        private bool Trigger;
        private Silence_Emotion_Clock _clock;
        private Silence_Emotion_Clock Clock
        {
            get
            {
                if (_clock == null)
                    _clock = BattleManagerUI.Instance.EffectLayer.GetComponentInChildren<Silence_Emotion_Clock>();
                if (_clock == null)
                {
                    Silence_Emotion_Clock original = Resources.Load<Silence_Emotion_Clock>("Prefabs/Battle/CreatureEffect/8/Silence_Emotion_Clock");
                    if (original != null)
                    {
                        Silence_Emotion_Clock silenceEmotionClock = UnityEngine.Object.Instantiate(original);
                        silenceEmotionClock.gameObject.transform.SetParent(BattleManagerUI.Instance.EffectLayer);
                        silenceEmotionClock.gameObject.transform.localPosition = new Vector3(0.0f, 800f, 0.0f);
                        silenceEmotionClock.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    _clock = original;
                }
                return _clock;
            }
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            Init();
        }
        private void Init()
        {
            _elapsed = 0.0f;
            _bTimeLimitOvered = false;
            rolled = false;
        }
        public override void OnFixedUpdateInWaitPhase(float delta)
        {
            base.OnFixedUpdateInWaitPhase(delta);
            if (!rolled || _bTimeLimitOvered)
                return;
            Clock?.Run(_elapsed);
            _elapsed += delta;
            if (_elapsed < 30.0 || BattleCamManager.Instance.IsCamIsReturning)
                return;
            Trigger = true;
            _bTimeLimitOvered = true;
            Clock?.OnStartUnitMoving();
            if (!damaged)
            {
                SoundEffectManager.Instance.PlayClip("Creature/Clock_StopCard");
                damaged = true;
            }
        }
        public override void OnAfterRollSpeedDice()
        {
            base.OnAfterRollSpeedDice();
            Init();
            rolled = true;
            Trigger = false;
            damaged = false;
            Clock?.OnStartRollSpeedDice();
            _elapsed = 0.0f;
        }
        public override void OnStartBattle()
        {
            base.OnStartBattle();
            Clock?.OnStartUnitMoving();
        }
        public override void OnRoundEnd()
        {
            rolled = false;
        }
        public override void BeforeRollDice(BattleDiceBehavior behavior)
        {
            base.BeforeRollDice(behavior);
            if (Trigger)
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus()
                {
                    power = RandomUtil.Range(1,2)
                });
            }
        }
    }
}

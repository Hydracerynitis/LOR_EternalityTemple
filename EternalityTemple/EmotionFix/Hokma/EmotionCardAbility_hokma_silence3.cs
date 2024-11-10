using Sound;
using System.Collections.Generic;
using UnityEngine;

namespace EmotionalFix
{
    public class EmotionCardAbility_hokma_silence3 : EmotionCardAbilityBase
    {
        private int count;

        public override void OnSelectEmotion()
        {
            base.OnSelectEmotion();
            count = 2;
        }
        public override void OnRoundEnd()
        {
            base.OnRoundStart();
            count++;
            if (count < 3)
                return;
            Silence();
            count = 0;
        }
        public void Silence()
        {
            List<BattleUnitModel> nonstun = new List<BattleUnitModel>();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList_opponent(_owner.faction))
            {
                if (unit.bufListDetail.GetActivatedBufList().Find(x => x is BattleUnitBuf_stun) == null)
                    nonstun.Add(unit);
            }
            if (nonstun.Count <= 0)
                return;
            BattleUnitModel victim = RandomUtil.SelectOne(nonstun);
            victim.bufListDetail.AddBuf(new BattleUnitBuf_stun());
            _owner.bufListDetail.AddBuf(new BattleUnitBuf_stun());
            Effect();
        }
        public void Effect()
        {
            new GameObject().AddComponent<SpriteFilter_Gaho>().Init("EmotionCardFilter/ThePriceOfSilence_Filter", false, 2f);
            SoundEffectPlayer.PlaySound("Creature/Clock_StopCard");
            Battle.CreatureEffect.CreatureEffect original = Resources.Load<Battle.CreatureEffect.CreatureEffect>("Prefabs/Battle/CreatureEffect/New_IllusionCardFX/9_H/FX_IllusionCard_9_H_Silence");
            if (original != null)
            {
                Battle.CreatureEffect.CreatureEffect creatureEffect = UnityEngine.Object.Instantiate(original);
                creatureEffect.gameObject.transform.SetParent(SingletonBehavior<BattleManagerUI>.Instance.EffectLayer);
                creatureEffect.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                creatureEffect.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}

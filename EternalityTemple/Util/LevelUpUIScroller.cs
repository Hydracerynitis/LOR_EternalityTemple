using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EternalityTemple.Util
{
    public class LevelUpUIScroller: MonoBehaviour
    {
        private LevelUpUI upUI;
        private int pointer = 0;
        private List<EmotionCardXmlInfo> cardList;
        public void Init(LevelUpUI upUI, List<EmotionCardXmlInfo> cardList)
        {
            this.upUI = upUI;
            this.cardList = cardList;
        }
        public void Update()
        {
            if (cardList == null)
                return;
            float scrolling = Input.GetAxisRaw("Mouse ScrollWheel");
            if (scrolling > 0)
                pointer = Math.Min(pointer + 1, cardList.Count - 3);
            else if (scrolling < 0)
                pointer = Math.Max(pointer - 1, 0);
            UpdateEntry();
        }
        public void UpdateEntry()
        {
            if (upUI == null)
                return;
            int index = pointer;
            foreach(EmotionPassiveCardUI cardUI in upUI.candidates)
            {
                EmotionCardXmlInfo info = cardList[index];
                index++;
                if (cardUI.Card.id == info.id && cardUI.Card.Sephirah == info.Sephirah)
                    continue;
                if (!cardUI.isActiveAndEnabled)
                    cardUI.gameObject.SetActive(true);
                cardUI.Init(info);
            }
        }
    }
}

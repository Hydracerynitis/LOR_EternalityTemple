using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using EI = EternalityTemple.EternalityInitializer;

namespace EternalityTemple
{
	public static class Helper
	{
		public static T AddBuf<T>(BattleUnitModel model, int stack, BufReadyType bufReadyType = BufReadyType.ThisRound) where T:BattleUnitBuf
		{
			if(model==null)
				return null;
            List<BattleUnitBuf> list=null;
            switch (bufReadyType)
            {
                case BufReadyType.ThisRound:
                    list = model.bufListDetail.GetActivatedBufList();
                    break;
                case BufReadyType.NextRound:
                    list = model.bufListDetail.GetReadyBufList();
                    break;
                case BufReadyType.NextNextRound:
                    list = model.bufListDetail.GetReadyReadyBufList();
                    break;
            }
			if (list == null)
				return null;
            T buf = list.Find(x => x.GetType() == typeof(T)) as T;
            if (buf == null)
            {
                buf = Activator.CreateInstance<T>();
                buf.Init(model);
                buf.stack = stack;
                list.Add(buf);
            }
            else
                buf.stack += stack;
            return buf;
        }
        public static BattleEmotionCardModel SearchEmotion(BattleUnitModel owner, string Name)
		{
			foreach (BattleEmotionCardModel battleEmotionCardModel in owner.emotionDetail.PassiveList)
			{
				if (battleEmotionCardModel.XmlInfo.Name == Name)
				{
					return battleEmotionCardModel;
				}
			}
			return null;
		}
		public static bool CheckOtherMod(string DLLname)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				if (assemblies[i].GetName().Name == DLLname)
				{
					return true;
				}
			}
			return false;
		}
        public static BattleDiceCardModel DrawCardSpecified(
        BattleUnitModel model,
      Predicate<BattleDiceCardModel> match)
        {
            if (model == null)
                return null;
            BattleAllyCardDetail cardDetail = model.allyCardDetail;
            BattleDiceCardModel battleDiceCardModel;
            if (cardDetail.GetHand().Count >= cardDetail._maxDrawHand)
                battleDiceCardModel = null;
            else
            {
                try
                {
                    List<BattleDiceCardModel> cardInDeck = cardDetail._cardInDeck;
                    List<BattleDiceCardModel> cardInDiscarded = cardDetail._cardInDiscarded;
                    cardInDeck.AddRange(cardInDiscarded);
                    cardInDiscarded.Clear();
                    BattleDiceCardModel card = cardInDeck.Find(match);
                    if (card != null)
                    {
                        cardDetail.AddCardToHand(card);
                        cardInDeck.Remove(card);
                        return card;
                    }
                }
                catch (Exception ex)
                {
                    File.WriteAllText(EI.ModPath + "/DrawCardSpecifiederror.log", ex.Message + Environment.NewLine + ex.StackTrace);
                }
                battleDiceCardModel = null;
            }
            return battleDiceCardModel;
        }
    }
}

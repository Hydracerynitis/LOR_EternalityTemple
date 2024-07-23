using System;
using System.Reflection;

namespace EmotionalFix
{
	public static class Helper
	{
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
	}
}

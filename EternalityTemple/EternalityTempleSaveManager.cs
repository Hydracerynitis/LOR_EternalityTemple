using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GameSave;

namespace EternalityTemple
{
	public class EternalityTempleSaveManager : Singleton<EternalityTempleSaveManager>
	{
		public void RemoveData(string savename)
		{
			if (!Directory.Exists(EternalityTempleSaveManager.Saveroot))
			{
				Directory.CreateDirectory(EternalityTempleSaveManager.Saveroot);
			}
			if (File.Exists(EternalityTempleSaveManager.Saveroot + "/" + savename))
			{
				File.Delete(EternalityTempleSaveManager.Saveroot + "/" + savename);
			}
		}

		public void SaveData(SaveData data, string savename)
		{
			if (!Directory.Exists(EternalityTempleSaveManager.Saveroot))
			{
				Directory.CreateDirectory(EternalityTempleSaveManager.Saveroot);
			}
			object serializedData = data.GetSerializedData();
			using (FileStream fileStream = File.Create(EternalityTempleSaveManager.Saveroot + "/" + savename))
			{
				new BinaryFormatter().Serialize(fileStream, serializedData);
			}
		}

		public SaveData LoadData(string savename)
		{
			SaveData result;
			if (!Directory.Exists(EternalityTempleSaveManager.Saveroot))
			{
				Directory.CreateDirectory(EternalityTempleSaveManager.Saveroot);
				result = null;
			}
			else
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				if (File.Exists(EternalityTempleSaveManager.Saveroot + "/" + savename))
				{
					object obj;
					using (FileStream fileStream = File.Open(EternalityTempleSaveManager.Saveroot + "/" + savename, FileMode.Open))
					{
						obj = binaryFormatter.Deserialize(fileStream);
					}
					if (obj != null)
					{
						SaveData saveData = new SaveData();
						saveData.LoadFromSerializedData(obj);
						return saveData;
					}
				}
				result = null;
			}
			return result;
		}

		public static string Saveroot
		{
			get
			{
				return SaveManager.savePath + "/EternalityTempleSave";
			}
		}
	}
}
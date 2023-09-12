using EternalityTemple.Inaba;
using EternalityTemple.Kaguya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.UI.GridLayoutGroup;

namespace EternalityTemple
{
    public class EternalityParam
    {
        //这个是司书敌人都会有的跨幕数据 （时辰，狂气，谜题）
        public int InabaBufGainNum = 0;
        public int KaguyaStack = 1;
        public List<(UnitBattleDataModel,int)> PuzzleLog=new List<(UnitBattleDataModel, int)>();
        public List<PuzzleQuestData> QuestLog =new List<PuzzleQuestData> ();
        public Faction faction;
        //这个是战斗机制需要的跨幕数据，所以并不需要创建多个个体
        public static int PickedEmotionCard = 0;
        public static bool EgoCoolDown = false;
        //这些是跨幕数据的个体，方便其他类访问
        public static EternalityParam Enemy =new EternalityParam() { faction=Faction.Enemy };
        public static EternalityParam Librarian = new EternalityParam() { faction= Faction.Player };
        
        //这个是被动和buf需要调用拥有者对应阵营的跨幕数据时要引用的
        public static EternalityParam GetFaction(Faction f)
        {
            if(f==Faction.Enemy) 
                return Enemy;
            else
                return Librarian;
        }

        public void Reset()
        {
            KaguyaStack = 1;
            InabaBufGainNum = 0;
            PuzzleLog.Clear();
            QuestLog.Clear();
        }
        public void EndBattleRecord()
        {
            RecordKaguyaStack();
            RecordInabaGain();
            RecordProgress();
        }
        private void RecordKaguyaStack()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(faction))
            {
                int stack = BattleUnitBuf_KaguyaBuf.GetStack(unit);
                if (stack != -1)
                {
                    KaguyaStack = stack;
                    return;
                }
            }
        }
        private void RecordInabaGain()
        {
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(faction))
            {
                int stack = BattleUnitBuf_InabaBuf1.GetStack(unit);
                if (stack != 0)
                {
                    InabaBufGainNum = stack;
                    return;
                }
            }
        }
        private void RecordProgress()
        {
            List<PuzzleQuestData> removal= new List<PuzzleQuestData>();
            foreach (BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(faction))
            {
                foreach (PuzzleQuestData PQD in QuestLog.FindAll(x => x.questGiver == unit.UnitData))
                {
                    KaguyaPuzzle KP = unit.bufListDetail.GetActivatedBufList().Find(x => x is KaguyaPuzzle && (x as KaguyaPuzzle).getPuzzleId() == PQD.QuestId) as KaguyaPuzzle;
                    if (KP == null)
                        removal.Add(PQD);
                    else
                        PQD.QuestProgress = KP.stack;
                }
            }
            QuestLog.RemoveAll(x => removal.Contains(x));
        }
    }
    public class PuzzleQuestData
    {
        public int QuestId;
        public int QuestProgress;
        public UnitBattleDataModel questGiver;
    }
}

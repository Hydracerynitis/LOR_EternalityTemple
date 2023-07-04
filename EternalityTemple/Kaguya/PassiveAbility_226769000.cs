using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalityTemple.Kaguya
{
    public class PassiveAbility_226769000: PassiveAbilityBase
    {
        private static int[] EnemySet = new int[] { 226769000, 226769001, 226769002 };
        private static int[] LibrarySet = new int[] { 226769003, 226769004, 226769006 };
        public override void OnWaveStart()
        {
            List<BattleUnitModel> trio=new List<BattleUnitModel>();
            foreach(BattleUnitModel unit in BattleObjectManager.instance.GetAliveList(owner.faction))
            {
                LorId bookId = unit.Book.ClassInfo.id;
                if (bookId.packageId == EternalityInitializer.packageId)
                    if (((owner.faction == Faction.Enemy && EnemySet.Contains(bookId.id)) || (owner.faction == Faction.Player && LibrarySet.Contains(bookId.id))) && !trio.Exists(x => x.Book.ClassInfo.id == bookId))
                        trio.Add(unit);
            }
            trio.ForEach(x => x.bufListDetail.AddBuf(new BattleUnitBuf_KaguyaBuf1()));
        }
    }
}

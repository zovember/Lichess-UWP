using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationApp.Bitboards.Zobrist.RepetitionHistory
{
    public class RepetitionHistory
    {
        private List<ZobristHash> Hashes = new List<ZobristHash>();

        public RepetitionHistory() { }

        public void AddPosition(ZobristHash hash) => Hashes.Add(hash);

        public void Clear() => Hashes.Clear();

        public byte GetRepetitionNumber(ZobristHash hash) // По-русски: получить число повторений
        {
            byte ctr = 0;
            foreach (ZobristHash _hash in Hashes)
            {
                if (_hash == hash) ctr++;
            }

            return ctr;
        }
    }
}

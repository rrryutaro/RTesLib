using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesList<T> : List<T>, ITesBase where T : ITesBase
    {
        public TesList()
        {
        }
        public TesList(IEnumerable<T> collection)
        {
            this.AddRange(collection);
        }

        public TesBytes ToBytes()
        {
            TesBytes result = new TesBytes();
            foreach (var x in this)
            {
                result.AddRange(x.ToBytes());
            }
            return result;
        }

        public uint Recalc()
        {
            uint result = 0;
            foreach (var x in this)
            {
                result += x.Recalc();
            }
            return result;
        }

        public uint ItemCount()
        {
            uint result = 0;
            foreach (var x in this)
            {
                result += x.ItemCount();
            }
            return result;
        }

        public void Write(BinaryWriter bw)
        {
            foreach (var x in this)
            {
                x.Write(bw);
            }
        }
    }
}

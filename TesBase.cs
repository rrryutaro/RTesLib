using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesBase : ITesBase
    {
        public TesList<ITesBase> OutputItems { get; } = new TesList<ITesBase>();

        public TesBase()
        {
        }

        public virtual TesBytes ToBytes()
        {
            return OutputItems.ToBytes();
        }
        public virtual uint Recalc()
        {
            return OutputItems.Recalc();
        }
        public virtual uint ItemCount()
        {
            return OutputItems.ItemCount();
        }

        public virtual void Write(BinaryWriter bw)
        {
            OutputItems.Write(bw);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RTesLib
{
    public interface ITesBase
    {
        TesBytes ToBytes();
        uint Recalc();
        uint ItemCount();
        void Write(BinaryWriter bw);
    }
}

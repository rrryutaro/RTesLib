using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RTesLib
{
    [System.Diagnostics.DebuggerDisplay("{Value} ({System.BitConverter.ToString(ToBytes().ToArray()).Replace(\"-\", \" \")})")]
    public class TesUInt16 : ITesBase
    {
        public ushort Value { get; set; }

        public TesUInt16(ushort value)
        {
            this.Value = value;
        }
        public TesUInt16(TesFileReader fr)
        {
            this.Value = fr.GetUInt16();
        }

        public TesBytes ToBytes()
        {
            TesBytes result = new TesBytes(Value);
            return result;
        }
        public uint Recalc()
        {
            uint result = ToBytes().Recalc();
            return result;
        }
        public uint ItemCount()
        {
            uint result = 0;
            return result;
        }
        public void Write(BinaryWriter bw)
        {
            bw.Write(ToBytes().ToArray());
        }
    }
}

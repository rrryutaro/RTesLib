using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RTesLib
{
    [System.Diagnostics.DebuggerDisplay("{Value} ({System.BitConverter.ToString(ToBytes().ToArray()).Replace(\"-\", \" \")})")]
    public class TesInt16 : ITesBase
    {
        public short Value { get; set; }

        public TesInt16(short value)
        {
            this.Value = value;
        }
        public TesInt16(TesFileReader fr)
        {
            this.Value = fr.GetInt16();
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

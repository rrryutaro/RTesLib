using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RTesLib
{
    [System.Diagnostics.DebuggerDisplay("{Value} ({System.BitConverter.ToString(ToBytes().ToArray()).Replace(\"-\", \" \")})")]
    public class TesUInt32 : ITesBase
    {
        public uint Value { get; set; }

        public TesUInt32(uint value)
        {
            this.Value = value;
        }
        public TesUInt32(TesFileReader fr)
        {
            this.Value = fr.GetUInt32();
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

        /// <summary>
        /// uintへの代入時に暗黙の型変換
        /// </summary>
        /// <param name="obj"></param>
        public static implicit operator uint(TesUInt32 obj)
        {
            return obj.Value;
        }
    }
}

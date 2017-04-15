using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RTesLib
{
    [System.Diagnostics.DebuggerDisplay("{Value} ({System.BitConverter.ToString(ToBytes().ToArray()).Replace(\"-\", \" \")})")]
    public class TesFloat : ITesBase
    {
        public float Value { get; set; }

        public TesFloat(float value)
        {
            this.Value = value;
        }
        public TesFloat(TesFileReader fr)
        {
            this.Value = fr.GetFloat();
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
        /// floatへの代入時に暗黙の型変換
        /// </summary>
        /// <param name="obj"></param>
        public static implicit operator float(TesFloat obj)
        {
            return obj.Value;
        }
    }
}

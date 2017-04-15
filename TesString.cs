using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RTesLib
{
    [System.Diagnostics.DebuggerDisplay("{Value}")]
    public class TesString : ITesBase
    {
        public string Value { get; set; }
        public bool isNullTerminated;

        public TesString(TesBytes value)
        {
            if (value[value.Count() - 1] == 0x00)
                isNullTerminated = true;

            this.Value = value.ToString();
        }
        public TesString(string value, bool isNullTerminated = false)
        {
            this.Value = value;
            this.isNullTerminated = isNullTerminated;
        }
        public TesString(TesFileReader fr)
        {
            this.Value = fr.GetString(4);
            this.isNullTerminated = false;
        }

        public TesBytes ToBytes()
        {
            TesBytes result = new TesBytes(Value, isNullTerminated);
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
        /// stringへの代入時に暗黙の型変換
        /// </summary>
        /// <param name="obj"></param>
        public static implicit operator string(TesString obj)
        {
            return obj.Value;
        }
    }
}

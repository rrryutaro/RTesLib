using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RTesLib
{
    public class TesBytes : List<byte>, ITesBase
    {
        public TesBytes()
        {
        }
        public TesBytes(TesBytes value)
        {
            this.AddRange(value);
        }
        public TesBytes(byte[] value)
        {
            this.AddRange(value);
        }
        public TesBytes(string value, bool isNullTerminated)
        {
            this.AddRange(LibraryPropertys.TesEncoding.GetBytes(value));
            if (isNullTerminated)
                this.Add(0x00);
        }
        public TesBytes(short value)
        {
            this.AddRange(BitConverter.GetBytes(value));
        }
        public TesBytes(ushort value)
        {
            this.AddRange(BitConverter.GetBytes(value));
        }
        public TesBytes(uint value)
        {
            this.AddRange(BitConverter.GetBytes(value));
        }
        public TesBytes(float value)
        {
            this.AddRange(BitConverter.GetBytes(value));
        }

        public TesBytes GetOffset(long offset, long count)
        {
            if (Int32.MaxValue < offset + count)
                throw new Exception();

            TesBytes result = new TesBytes(this.GetRange((int)offset, (int)count).ToArray());
            return result;
        }

        public TesBytes ToBytes()
        {
            return this;
        }
        public uint Recalc()
        {
            return (uint)this.Count;
        }
        public uint ItemCount()
        {
            return 0;
        }
        public void Write(BinaryWriter bw)
        {
            bw.Write(ToBytes().ToArray());
        }

        public override string ToString()
        {
            //終端がNULLの場合、除外する
            int count = this.Count;
            if (this[count - 1].Equals(0x00))
                --count;

            string result = LibraryPropertys.TesEncoding.GetString(this.GetOffset(0, count).ToArray());
            return result;
        }
        public short ToInt16()
        {
            short result = BitConverter.ToInt16(this.ToArray(), 0);
            return result;
        }
        public ushort ToUInt16()
        {
            ushort result = BitConverter.ToUInt16(this.ToArray(), 0);
            return result;
        }
        public uint ToUInt32()
        {
            uint result = BitConverter.ToUInt32(this.ToArray(), 0);
            return result;
        }
        public float ToFloat()
        {
            float result = BitConverter.ToSingle(this.ToArray(), 0);
            return result;
        }

        public void Overwrite(ITesBase bytes, int pos = 0)
        {
            TesBytes b = bytes.ToBytes();
            this.RemoveRange(pos, b.Count);
            this.InsertRange(pos, b);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RTesLib
{
    public class TesFileReader
    {
        private BinaryReader br;
        private long pos;
        private long len;
        private bool cp;

        public TesFileReader(string path)
        {
            br = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read));
            pos = 0;
            len = br.BaseStream.Length;
            cp = false;
        }
        private TesFileReader(BinaryReader br, long pos, long len)
        {
            this.br = br;
            this.pos = pos;
            this.len = len;
            this.cp = true;
        }
        ~TesFileReader()
        {
            if (!cp)
                br.Close();
        }

        public long Position
        {
            get
            {
                long result = pos;
                return result;
            }
        }
        public long Length
        {
            get
            {
                long result = len;
                return result;
            }
        }
        public bool EOF
        {
            get
            {
                bool result = len <= pos;
                return result;
            }
        }

        public void Seek(long count)
        {
            pos += count;
        }
        private TesBytes Read(long count, bool next)
        {
            if (pos != br.BaseStream.Position)
                br.BaseStream.Seek(pos, SeekOrigin.Begin);

            if (Int32.MaxValue < count)
                throw new Exception();

            TesBytes result = new TesBytes(br.ReadBytes((int)count));

            if (!next)
                br.BaseStream.Seek(-count, SeekOrigin.Current);

            return result;
        }

        public TesBytes GetBytes(long count, long offset = 0, bool next = true)
        {
            if (len < pos + count + offset)
                throw new Exception();

            TesBytes result = Read(offset + count, next);
            if (0 < offset)
                result = result.GetOffset(offset, count);

            if (next)
                pos += count;

            return result;
        }

        public string GetString(long count, long offset = 0, bool next = true)
        {
            TesBytes b = GetBytes(count, offset, next);
            string result = b.ToString();
            return result;
        }
        public short GetInt16(long offset = 0, bool next = true)
        {
            long count = 2;
            TesBytes b = GetBytes(count, offset, next);
            short result = b.ToInt16();
            return result;
        }
        public ushort GetUInt16(long offset = 0, bool next = true)
        {
            long count = 2;
            TesBytes b = GetBytes(count, offset, next);
            ushort result = b.ToUInt16();
            return result;
        }
        public uint GetUInt32(long offset = 0, bool next = true)
        {
            long count = 4;
            TesBytes b = GetBytes(count, offset, next);
            uint result = b.ToUInt32();
            return result;
        }
        public float GetFloat(long offset = 0, bool next = true)
        {
            long count = 4;
            TesBytes b = GetBytes(count, offset, next);
            float result = b.ToFloat();
            return result;
        }

        public string GetTypeID(long offset = 0)
        {
            string result = GetString(4, offset, false);
            return result;
        }

        public TesFileReader GetGroup(bool next = true)
        {
            long count = GetUInt32(4, false);
            TesFileReader result = new TesFileReader(br, pos, pos + count);
            if (next)
                pos += count;

            return result;
        }
        public TesFileReader GetRecord(bool next = true)
        {
            long count = GetUInt32(4, false) + 24;
            TesFileReader result = new TesFileReader(br, pos, pos + count);
            if (next)
                pos += count;

            return result;
        }
        public TesFileReader GetField(bool next = true)
        {
            long count = GetUInt16(4, false) + 6;
            TesFileReader result = new TesFileReader(br, pos, pos + count);
            if (next)
                pos += count;

            return result;
        }

        /// <summary>
        /// Cellは CELL レコードと、その直後のGRUPを範囲とする
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public TesFileReader GetCell(bool next = true)
        {
            //CELLレコードのサイズ
            long count = GetUInt32(4, false) + 24;

            //読込みサイズがファイルサイズと一致する場合、GRUPなし
            if (pos + count < len && GetTypeID(count).Equals("GRUP"))
            {
                //CELLレコード後のGROUPのサイズを加算
                count += GetUInt32(count + 4, false);
            }

            TesFileReader result = new TesFileReader(br, pos, pos + count);
            if (next)
                pos += count;

            return result;
        }

        public TesFileReader GetCopyReader(uint count, bool next = true)
        {
            TesFileReader result = new TesFileReader(br, pos, pos + count);
            if (next)
                pos += count;
            return result;
        }
        public string GetNullTerminatedString(long pos)
        {
            this.pos = pos;
            if (pos != br.BaseStream.Position)
                br.BaseStream.Seek(pos, SeekOrigin.Begin);

            TesBytes bytes = new TesBytes();
            byte b;
            do
            {
                b = br.ReadByte();
                bytes.Add(b);
                ++pos;
            }
            while (b != 0x00 && pos < len);

            string result = bytes.ToString();
            return result;
        }
    }
}

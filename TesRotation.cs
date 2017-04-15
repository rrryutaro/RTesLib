using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    /// <summary>
    /// Rotationは特殊な型っぽい？？（0.0000 - 359.9998）
    /// </summary>
    public class TesRotation : TesBase
    {
        public TesBytes X { get; set; }
        public TesBytes Y { get; set; }
        public TesBytes Z { get; set; }
        private void Initialize()
        {
            OutputItems.Add(X);
            OutputItems.Add(Y);
            OutputItems.Add(Z);
        }
        public TesRotation() : this(0, 0, 0)
        {
        }
        public TesRotation(TesBytes x, TesBytes y, TesBytes z)
        {
            X = x;
            Y = y;
            Z = z;
            Initialize();
        }
        public TesRotation(int x, int y, int z)
        {
            X = IntToByte(x);
            Y = IntToByte(y);
            Z = IntToByte(z);
            Initialize();
        }
        public TesRotation(TesFileReader fr)
        {
            X = fr.GetBytes(4);
            Y = fr.GetBytes(4);
            Z = fr.GetBytes(4);
        }

        public TesRotation Clone()
        {
            TesRotation result = new TesRotation(X.ToBytes(), Y.ToBytes(), Z.ToBytes());
            return result;
        }

        public static TesBytes IntToByte(int value)
        {
            TesBytes result = null;
            switch (value)
            {
                case 90:
                    result = new TesBytes(new byte[] { 0xdb, 0x0f, 0xc9, 0x3f });
                    break;

                case 180:
                    result = new TesBytes(new byte[] { 0xdb, 0x0f, 0x49, 0x40 });
                    break;

                case 270:
                    result = new TesBytes(new byte[] { 0xdb, 0x0f, 0xc9, 0xbf });
                    break;

                default:
                    result = new TesBytes(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                    break;
            }
            return result;
        }

        public static int ByteToInt(TesBytes bytes)
        {
            int result = 0;
            if (bytes.SequenceEqual(new byte[] { 0xdb, 0x0f, 0xc9, 0x3f }))
                result = 90;
            else if (bytes.SequenceEqual(new byte[] { 0xdb, 0x0f, 0x49, 0x40 }))
                result = 180;
            else if (bytes.SequenceEqual(new byte[] { 0xdb, 0x0f, 0xc9, 0xbf }))
                result = 270;
            return result;
        }
    }
}

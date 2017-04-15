using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    [System.Diagnostics.DebuggerDisplay("X:{X.Value}, Y:{Y.Value}, Z:{Z.Value}")]
    public class Tes3DPos : TesBase
    {
        public TesFloat X { get; set; }
        public TesFloat Y { get; set; }
        public TesFloat Z { get; set; }
        private void Initialize()
        {
            OutputItems.Add(X);
            OutputItems.Add(Y);
            OutputItems.Add(Z);
        }
        public Tes3DPos() : this(0, 0, 0)
        {
        }
        public Tes3DPos(TesFloat x, TesFloat y, TesFloat z)
        {
            X = x;
            Y = y;
            Z = z;
            Initialize();
        }
        public Tes3DPos(float x, float y, float z)
        {
            X = new TesFloat(x);
            Y = new TesFloat(y);
            Z = new TesFloat(z);
            Initialize();
        }
        public Tes3DPos(TesFileReader fr)
        {
            X = new TesFloat(fr);
            Y = new TesFloat(fr);
            Z = new TesFloat(fr);
            Initialize();
        }

        public Tes3DPos Offset(float x, float y, float z)
        {
            return new Tes3DPos(X.Value + x, Y.Value + y, Z.Value + z);
        }

        public Tes3DPos Clone()
        {
            Tes3DPos result = new Tes3DPos(X.Value, Y.Value, Z.Value);
            return result;
        }
    }
}

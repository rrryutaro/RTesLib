using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesXTEL : TesField
    {
        public TesUInt32 Door { get; }
        public Tes3DPos Position { get; }
        public TesRotation Rotation { get; }
        public TesUInt32 Flags { get; }

        public TesXTEL() : base("XTEL")
        {
            Door = new TesUInt32(0);
            Position = new Tes3DPos(0, 0, 0);
            Rotation = new TesRotation(0, 0, 0);
            Flags = new TesUInt32(0);

            OutputItems.Add(Door);
            OutputItems.Add(Position);
            OutputItems.Add(Rotation);
            OutputItems.Add(Flags);
        }
    }
}

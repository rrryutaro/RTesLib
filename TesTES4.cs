using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesTES4 : TesRecord
    {
        public class TES4_HEDR : TesField
        {
            public TesFloat Version { get; }
            public TesUInt32 NumberOfRecords { get; }
            public TesUInt32 NextObjectID { get; }
            public TES4_HEDR(TesFileReader fr) : base(fr, false)
            {
                Version = new TesFloat(fr);
                NumberOfRecords = new TesUInt32(fr);
                NextObjectID = new TesUInt32(fr);

                Values.Add(Version);
                Values.Add(NumberOfRecords);
                Values.Add(NextObjectID);
            }
        }
        public TES4_HEDR HEDR { get; set; }

        public TesTES4(TesFileReader fr) : base(fr)
        {
        }

        public override TesField ReadField(TesFileReader fr)
        {
            TesField result = base.ReadField(fr);

            if (result == null)
            {
                string id = fr.GetTypeID();
                switch (id)
                {
                    case "HEDR":
                        HEDR = new TES4_HEDR(fr.GetField());
                        result = HEDR;
                        break;
                }
            }

            return result;
        }

        public override uint ItemCount()
        {
            return 0;
        }
    }
}

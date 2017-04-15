using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesNPC : TesGroup
    {
        public TesNPC(TesFileReader fr) : base(fr, false)
        {
            while (!fr.EOF)
            {
                Records.Add(new TesRecordNPC(fr.GetRecord()));
            }
        }
    }

    public class TesRecordNPC : TesRecord
    {
        public TesRecordNPC(TesFileReader fr) : base(fr, false)
        {
            OutputItems.Add(fr.GetBytes(Header.DataSize));
        }
    }
}

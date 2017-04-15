using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesWorldspace : TesGroup
    {
        public new TesList<TesRecordWorldspace> Records = new TesList<TesRecordWorldspace>();

        public TesWorldspace(TesFileReader fr) : base(fr, false)
        {
            while (!fr.EOF)
            {
                Records.Add(new TesRecordWorldspace(fr.GetCell()));
            }
            OutputItems.Add(Records);
        }
    }

    public class TesRecordWorldspace : TesBase
    {
        public Dictionary<string, TesList<TesField>> Fields { get; } = new Dictionary<string, TesList<TesField>>();

        public TesRecord WRLD { get; }
        public TesRecordWorldspaceMain Main { get; }

        public TesRecordWorldspace(TesFileReader fr)
        {
            WRLD = new TesRecord(fr.GetRecord());
            Main = new TesRecordWorldspaceMain(fr.GetGroup());
            OutputItems.Add(WRLD);
            OutputItems.Add(Main);
        }
    }

    public class TesRecordWorldspaceMain : TesGroup
    {
        public TesRecordCell Cell { get; }
        public TesList<TesCellBlock> Blocks { get; }

        public TesRecordWorldspaceMain(TesFileReader fr) : base(fr, false)
        {
            while (!fr.EOF)
            {
                string id = fr.GetTypeID();
                if (id.Equals("CELL"))
                {
                    Cell = new TesRecordCell(fr.GetCell());
                    OutputItems.Add(Cell);
                }
                else if (id.Equals("GRUP"))
                {
                    if (Blocks == null)
                    {
                        Blocks = new TesList<TesCellBlock>();
                        OutputItems.Add(Blocks);
                    }
                    Blocks.Add(new TesCellBlock(fr.GetGroup()));
                }
                else
                    throw new Exception();
            }
        }
    }
}

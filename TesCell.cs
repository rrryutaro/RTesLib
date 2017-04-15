using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesCell : TesGroup
    {
        public TesList<TesCellBlock> Blocks = new TesList<TesCellBlock>();

        public TesCell(TesFileReader fr) : base(fr, false)
        {
            while (!fr.EOF)
            {
                Blocks.Add(new TesCellBlock(fr.GetGroup()));
            }
            OutputItems.Add(Blocks);
        }
    }

    public class TesCellBlock : TesGroup
    {
        public TesList<TesCellSubBlock> SubBlocks = new TesList<TesCellSubBlock>();

        public TesCellBlock(TesFileReader fr) : base(fr, false)
        {
            while (!fr.EOF)
            {
                SubBlocks.Add(new TesCellSubBlock(fr.GetGroup()));
            }
            OutputItems.Add(SubBlocks);
        }
    }

    public class TesCellSubBlock : TesGroup
    {
        public TesList<TesRecordCell> Cells = new TesList<TesRecordCell>();

        public TesCellSubBlock(TesFileReader fr) : base(fr, false)
        {
            while (!fr.EOF)
            {
                Cells.Add(new TesRecordCell(fr.GetCell()));
            }
            OutputItems.Add(Cells);
        }
    }

    public class TesRecordCell : TesRecord
    {
        public TesBytes CellInfo { get; }
        public TesCellMain CellMain { get; }

        public TesRecordCell(TesFileReader fr) : base(fr, false)
        {
            CellInfo = new TesBytes(fr.GetBytes(Header.DataSize));
            OutputItems.Add(CellInfo);

            if (!fr.EOF)
            {
                CellMain = new TesCellMain(fr.GetGroup());
                OutputItems.Add(CellMain);
            }
        }
        public override uint Recalc()
        {
            uint result = OutputItems.Recalc();
            Header.DataSize.Value = (uint)CellInfo.Count;
            return result;
        }
        public override uint ItemCount()
        {
            uint result = 1;
            if (CellMain != null)
                result += CellMain.ItemCount();
            return result;
        }
    }
    public class TesCellMain : TesGroup
    {
        public TesList<TesCellMainSub> Subs = new TesList<TesCellMainSub>();

        public TesCellMain(TesFileReader fr) : base(fr, false)
        {
            while (!fr.EOF)
            {
                Subs.Add(new TesCellMainSub(fr.GetGroup()));
            }
            OutputItems.Add(Subs);
        }
    }
    public class TesCellMainSub : TesGroup
    {
        public Dictionary<string, TesList<TesRecord>> DicRecords { get; } = new Dictionary<string, TesList<TesRecord>>();

        public TesCellMainSub(TesFileReader fr) : base(fr, false)
        {
            while (!fr.EOF)
            {
                AddRecord(new TesRecord(fr.GetRecord()));
            }
        }
        public void AddRecord(TesRecord record)
        {
            Records.Add(record);
            if (!DicRecords.ContainsKey(record.Header.Signature))
                DicRecords.Add(record.Header.Signature, new TesList<TesRecord>());

            DicRecords[record.Header.Signature].Add(record);
        }
        public override uint ItemCount()
        {
            uint result = (uint)Records.Count + 1;
            return result;
        }
    }
}

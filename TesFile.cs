using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RTesLib
{
    public class TesFile : TesBase
    {
        public TesTES4 TES4 { get; }
        public Dictionary<string, TesGroup> Groups { get; set; } = new Dictionary<string, TesGroup>();

        public TesFile(string path, List<string> idList = null)
        {
            TesFileReader fr = new TesFileReader(path);

            TES4 = new TesTES4(fr.GetRecord());
            OutputItems.Add(TES4);

            string id = null;
            while (!fr.EOF)
            {
                id = fr.GetTypeID(8);

                if (idList != null && !idList.Contains(id))
                {
                    fr.Seek(fr.GetUInt32(4, false));
                    continue;
                }

                switch (id)
                {
                    case "NPC_":
                        TesNPC npc_ = new TesNPC(fr.GetGroup());
                        OutputItems.Add(npc_);
                        Groups.Add(id, npc_);
                        break;
                    case "CELL":
                        TesCell cell = new TesCell(fr.GetGroup());
                        OutputItems.Add(cell);
                        Groups.Add(id, cell);
                        break;
                    case "WRLD":
                        TesWorldspace wrld = new TesWorldspace(fr.GetGroup());
                        OutputItems.Add(wrld);
                        Groups.Add(id, wrld);
                        break;
                    case "DIAL":
                        OutputItems.Add(new TesBytes(fr.GetBytes(fr.GetUInt32(4, false))));
                        break;
                    case "HAZD":
                        string id2 = fr.GetTypeID(24);
                        if (id2.Equals("HAZD"))
                            OutputItems.Add(new TesBytes(fr.GetBytes(fr.GetUInt32(4, false))));
                        else if (id2.Equals("GRUP"))
                            OutputItems.Add(new TesBytes(fr.GetBytes(fr.GetUInt32(4, false))));
                        break;
                    default:
                        TesGroup grup = new TesGroup(fr.GetGroup());
                        OutputItems.Add(grup);
                        Groups.Add(id, grup);
                        break;
                }
            }
        }

        public void Recalc(uint countPlus = 0)
        {
            uint count = 0;
            foreach (var x in OutputItems)
            {
                x.Recalc();
                count += x.ItemCount();
            }

            TES4.HEDR.NumberOfRecords.Value = count + countPlus;
        }

        public void Save(string path, uint countPlus = 0)
        {
            Recalc(countPlus);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                OutputItems.Write(bw);
            }
        }

        public bool Contains(string signature)
        {
            bool result = Groups.ContainsKey(signature);
            return result;
        }
        public TesGroup this[string signature]
        {
            get
            {
                TesGroup result = Groups[signature];
                return result;
            }
        }
        public TesCell Cell
        {
            get
            {
                TesCell result = null;
                if (Contains("CELL"))
                    result = (TesCell)this["CELL"];
                return result;
            }
        }
    }
}

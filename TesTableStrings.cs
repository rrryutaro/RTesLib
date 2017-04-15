using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesTableStrings
    {
        private TesFileReader fr;
        private uint count;
        private uint dataSize;
        private Dictionary<uint, uint> de = new Dictionary<uint, uint>();
        private long pos;
        private Dictionary<uint, string> dic = new Dictionary<uint, string>();

        public TesTableStrings(string path)
        {
            fr = new TesFileReader(path);
            count = fr.GetUInt32();
            dataSize = fr.GetUInt32();
            for (int i = 0; i < count; i++)
            {
                de.Add(fr.GetUInt32(), fr.GetUInt32());
            }
            pos = fr.Position;
        }

        public string this[uint id]
        {
            get
            {
                if (!de.ContainsKey(id))
                    return "";

                if (!dic.ContainsKey(id))
                {
                    dic.Add(id, fr.GetNullTerminatedString(pos + de[id]));
                }
                string result = dic[id];
                return result;
            }
        }

        public Dictionary<uint, uint> DicStringID
        {
            get
            {
                return de;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesRecord : TesBase
    {
        public Dictionary<string, TesList<TesField>> Fields { get; } = new Dictionary<string, TesList<TesField>>();

        public TesHeader Header { get; }

        /// <summary>
        /// FormID
        /// </summary>
        public uint FormID
        {
            get
            {
                uint result = Header.FormID;
                return result;
            }
            set
            {
                Header.FormID.Value = value;
            }
        }

        /// <summary>
        /// EditorID
        /// </summary>
        public string EditorID
        {
            get
            {
                string result = string.Empty;
                if (Fields.ContainsKey("EDID"))
                {
                    result = this["EDID"][0].ToString();
                }
                return result;
            }
        }

        public string Name
        {
            get
            {
                string result = string.Empty;
                if (Fields.ContainsKey("FULL"))
                {
                    result = this["FULL"][0].ToString();
                }
                return result;
            }
        }
        public uint NameID
        {
            get
            {
                uint result = 0;
                if (Fields.ContainsKey("FULL"))
                {
                    result = this["FULL"][0].ToUInt32();
                }
                return result;
            }
        }

        public TesRecord(string signature, uint fromID)
        {
            Header = new TesHeader(signature, fromID);
            OutputItems.Add(Header);
        }

        public TesRecord(TesFileReader fr, bool readFiled = true)
        {
            Header = new TesHeader(fr);
            OutputItems.Add(Header);

            if (readFiled)
            {
                if (Header.Signature == "NAVM" || Header.Signature == "LAND")
                {
                    OutputItems.Add(new TesBytes(fr.GetBytes(Header.DataSize)));
                }
                else
                {
                    while (!fr.EOF)
                    {
                        TesField field = ReadField(fr) ?? new TesField(fr.GetField());
                        AddField(field);
                    }
                }
            }
        }

        public void AddField(TesField field)
        {
            OutputItems.Add(field);
            if (!Fields.ContainsKey(field.Signature))
                Fields.Add(field.Signature, new TesList<TesField>());

            Fields[field.Signature].Add(field);
        }

        public virtual TesField ReadField(TesFileReader fr)
        {
            TesField result = null;

            string id = fr.GetTypeID();
            switch (id)
            {
                //ワールドデータ内にある"OFST"で、1箇所DataSizeが0になっており、以降のデータが書き出されている場所がある
                case "OFST":
                    if (fr.GetInt16(4, false) == 0)
                    {
                        result = new TesField(fr, false);
                        result.Values.Add(new TesBytes(fr.GetBytes(fr.Length - fr.Position)));
                    }
                    break;
            }
            return result;
        }

        public override uint Recalc()
        {
            uint result = OutputItems.Recalc();
            Header.DataSize.Value = result - 24;
            return result;
        }
        public override uint ItemCount()
        {
            return 1;
        }

        public bool Contains(string signature)
        {
            bool result = Fields.ContainsKey(signature);
            return result;
        }
        public TesField this[string signature, int index = 0]
        {
            get
            {
                TesField result = Fields[signature][index];
                return result;
            }
        }
    }
}

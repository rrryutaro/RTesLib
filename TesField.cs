using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesField : TesBase
    {
        public TesString Signature { get; }
        public TesUInt16 DataSize { get; }
        public TesList<ITesBase> Values { get; } = new TesList<ITesBase>();

        public TesField(string signature, ITesBase value = null)
        {
            Signature = new TesString(signature);
            if (value != null)
                DataSize = new TesUInt16((ushort)value.ToBytes().Count());
            else
                DataSize = new TesUInt16(0);

            OutputItems.Add(Signature);
            OutputItems.Add(DataSize);

            if (value != null)
            {
                Values.Add(value);
            }
            OutputItems.Add(Values);
        }
        public TesField(string signature, IEnumerable<ITesBase> collection)
        {
            Signature = new TesString(signature);
            DataSize = new TesUInt16(0);
            OutputItems.Add(Signature);
            OutputItems.Add(DataSize);

            Values.AddRange(collection);
            OutputItems.Add(Values);
        }


        public TesField(TesFileReader fr, bool readValue = true)
        {
            Signature = new TesString(fr.GetString(4));
            DataSize = new TesUInt16(fr);
            OutputItems.Add(Signature);
            OutputItems.Add(DataSize);

            if (readValue)
            {
                Values.Add(fr.GetBytes(DataSize.Value));
            }
            OutputItems.Add(Values);
        }

        public override uint Recalc()
        {
            uint result = OutputItems.Recalc();
            
            //"OFST"でDataSizeが0のものがあるので、この場合DataSizeの再設定を行わない
            if (!Signature.Value.Equals("OFST") || DataSize.Value != 0)
                DataSize.Value = (ushort)(result - 6);
            return result;
        }

        public TesBytes this[int index]
        {
            get
            {
                TesBytes result = (TesBytes)Values[index];
                return result;
            }
        }
    }
}

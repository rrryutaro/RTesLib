using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesVMAD : TesField
    {
        public TesUInt16 Version { get; }
        public TesUInt16 ObjectFormat { get; }
        public TesUInt16 ScriptCount { get; }
        public class Script : TesBase
        {
            public TesUInt16 ScriptNameDataSize { get; }
            public TesString ScriptName { get; }
            public TesBytes Flags { get; }
            public TesUInt16 PropertyCount { get; }
            public class Property : TesBase
            {
                public TesUInt16 PropertyNameDataSize { get; }
                public TesString PropertyName { get; }
                public TesBytes Type { get; }
                public TesBytes PropertyValue { get; }

                public Property()
                {
                    PropertyNameDataSize = new TesUInt16(0);
                    PropertyName = new TesString("");
                    Type = new TesBytes(new byte[] { 0x01, 0x01, 0x00, 0x00, 0xff, 0xff });
                    PropertyValue = new TesBytes();

                    OutputItems.Add(PropertyNameDataSize);
                    OutputItems.Add(PropertyName);
                    OutputItems.Add(Type);
                    OutputItems.Add(PropertyValue);
                }
            }
            public TesList<Property> Propertys { get; } = new TesList<Property>();

            public Script()
            {
                ScriptNameDataSize = new TesUInt16(0);
                ScriptName = new TesString("");
                Flags = new TesBytes(new byte[] { 0x01 });
                PropertyCount = new TesUInt16(0);

                OutputItems.Add(ScriptNameDataSize);
                OutputItems.Add(ScriptName);
                OutputItems.Add(Flags);
                OutputItems.Add(PropertyCount);
                OutputItems.Add(Propertys);
            }
        }
        public TesList<Script> Scripts { get; } = new TesList<Script>();

        public TesVMAD(string scriptName, string propertyName, uint propertyValue) : base("VMAD")
        {
            Version = new TesUInt16(5);
            ObjectFormat = new TesUInt16(2);
            ScriptCount = new TesUInt16(1);

            Script script = new Script();
            script.ScriptNameDataSize.Value = (ushort)scriptName.Length;
            script.ScriptName.Value = scriptName;
            script.PropertyCount.Value = 1;

            Script.Property property = new Script.Property();
            property.PropertyNameDataSize.Value = (ushort)propertyName.Length;
            property.PropertyName.Value = propertyName;
            property.PropertyValue.AddRange(new TesBytes(propertyValue));

            script.Propertys.Add(property);
            Scripts.Add(script);

            OutputItems.Add(Version);
            OutputItems.Add(ObjectFormat);
            OutputItems.Add(ScriptCount);
            OutputItems.Add(Scripts);
        }

        public TesVMAD(List<Tuple<string, List<Tuple<string, uint>>>> list) : base("VMAD")
        {
            Version = new TesUInt16(5);
            ObjectFormat = new TesUInt16(2);
            ScriptCount = new TesUInt16((ushort)list.Count);

            foreach (var x in list)
            {
                Script script = new Script();
                script.ScriptNameDataSize.Value = (ushort)x.Item1.Length;
                script.ScriptName.Value = x.Item1;
                script.PropertyCount.Value = (ushort)x.Item2.Count;

                foreach (var x2 in x.Item2)
                {
                    Script.Property property = new Script.Property();
                    property.PropertyNameDataSize.Value = (ushort)x2.Item1.Length;
                    property.PropertyName.Value = x2.Item1;
                    property.PropertyValue.AddRange(new TesBytes(x2.Item2));

                    script.Propertys.Add(property);
                }
                Scripts.Add(script);
            }

            OutputItems.Add(Version);
            OutputItems.Add(ObjectFormat);
            OutputItems.Add(ScriptCount);
            OutputItems.Add(Scripts);
        }
    }
}

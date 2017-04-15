using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesHeader : TesBase
    {
        public TesString Signature { get; }
        public TesUInt32 DataSize { get; set; }
        public TesUInt32 RecordFlags { get; }
        public TesUInt32 FormID { get; set; }
        public TesUInt32 VersionControlInfo1 { get; }
        public TesUInt16 FormVersion { get; }
        public TesUInt16 VersionControlInfo2 { get; }

        private void Initialize()
        {
            OutputItems.Add(Signature);
            OutputItems.Add(DataSize);
            OutputItems.Add(RecordFlags);
            OutputItems.Add(FormID);
            OutputItems.Add(VersionControlInfo1);
            OutputItems.Add(FormVersion);
            OutputItems.Add(VersionControlInfo2);
        }

        public TesHeader(string signature, uint formID)
        {
            Signature = new TesString(signature);
            DataSize = new TesUInt32(0);
            RecordFlags = new TesUInt32(0);
            FormID = new TesUInt32(formID);
            VersionControlInfo1 = new TesUInt32(0);
            FormVersion = new TesUInt16(0x2c);
            VersionControlInfo2 = new TesUInt16(0);
            Initialize();
        }

        public TesHeader(TesFileReader fr)
        {
            Signature = new TesString(fr);
            DataSize = new TesUInt32(fr);
            RecordFlags = new TesUInt32(fr);
            FormID = new TesUInt32(fr);
            VersionControlInfo1 = new TesUInt32(fr);
            FormVersion = new TesUInt16(fr);
            VersionControlInfo2 = new TesUInt16(fr);
            Initialize();
        }

        public override uint ItemCount()
        {
            return 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTesLib
{
    public class TesGroup : TesBase
    {
        public TesList<TesRecord> Records = new TesList<TesRecord>();

        public TesString GRUP { get; }
        public TesUInt32 DataSize { get; }

        /// <summary>
        /// GroupTypeが以下の場合に設定
        /// 0 : Top 
        /// </summary>
        public TesString Signature { get; }

        /// <summary>
        /// GroupType以下の場合に設定
        /// 1 : World Children
        /// 6 : Cell Children
        /// 8 : Cell Persistent Childen
        /// 9 : Cell Temporary Children
        /// </summary>
        public TesUInt32 FormID { get; }

        /// <summary>
        /// GroupType以下の場合に設定
        /// 2 : Interior Cell Block
        /// 3 : Interior Cell Sub-Block
        /// </summary>
        public TesUInt32 Index { get; }

        /// <summary>
        /// GroupType以下の場合に設定
        /// 4 : Exterior Cell Block
        /// 5 : Exterior Cell Sub-Block
        /// </summary>
        public TesCellGrid Grid { get; }
        public class TesCellGrid : TesBase
        {
            public TesInt16 X { get; }
            public TesInt16 Y { get; }
            public TesCellGrid(TesFileReader fr) : base()
            {
                Y = new TesInt16(fr);
                X = new TesInt16(fr);
                OutputItems.Add(Y);
                OutputItems.Add(X);
            }
        }

        /// <summary>
        /// このグループのタイプの設定
        /// 0 : Top
        /// 1 : World Children
        /// 2 : Interior Cell Block
        /// 3 : Interior Cell Sub-Block
        /// 4 : Exterior Cell Block
        /// 5 : Exterior Cell Sub-Block
        /// 6 : Cell Children
        /// 8 : Cell Persistent Childen
        /// 9 : Cell Temporary Children
        /// </summary>
        public TesUInt32 GroupType { get; }

        public TesBytes Other { get; }

        public TesGroup(TesFileReader fr, bool readRecord = true)
        {
            GRUP = new TesString(fr);
            DataSize = new TesUInt32(fr);
            OutputItems.Add(GRUP);
            OutputItems.Add(DataSize);

            //グループタイプ別
            uint type = fr.GetUInt32(4, false);
            switch (type)
            {
                case 0:
                    Signature = new TesString(fr);
                    OutputItems.Add(Signature);
                    break;

                case 1:
                case 6:
                case 8:
                case 9:
                    FormID = new TesUInt32(fr);
                    OutputItems.Add(FormID);
                    break;

                case 2:
                case 3:
                    Index = new TesUInt32(fr);
                    OutputItems.Add(Index);
                    break;

                case 4:
                case 5:
                    Grid = new TesCellGrid(fr);
                    OutputItems.Add(Grid);
                    break;

                default:
                    throw new Exception();

            }
            GroupType = new TesUInt32(fr);
            Other = new TesBytes(fr.GetBytes(8));
            OutputItems.Add(GroupType);
            OutputItems.Add(Other);

            if (readRecord)
            {
                while (!fr.EOF)
                {
                    Records.Add(new TesRecord(fr.GetRecord()));
                }
            }
            OutputItems.Add(Records);
        }

        public override uint Recalc()
        {
            uint result = OutputItems.Recalc();
            DataSize.Value = result;
            return result;
        }

        public override uint ItemCount()
        {
            uint result = OutputItems.ItemCount() + 1;
            return result;
        }
    }
}

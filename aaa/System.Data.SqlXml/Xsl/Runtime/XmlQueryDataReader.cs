using System;
using System.IO;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000BA RID: 186
	internal class XmlQueryDataReader : BinaryReader
	{
		// Token: 0x0600091C RID: 2332 RVA: 0x0002BC00 File Offset: 0x0002AC00
		public XmlQueryDataReader(Stream input)
			: base(input)
		{
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0002BC09 File Offset: 0x0002AC09
		public int ReadInt32Encoded()
		{
			return base.Read7BitEncodedInt();
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0002BC11 File Offset: 0x0002AC11
		public string ReadStringQ()
		{
			if (!this.ReadBoolean())
			{
				return null;
			}
			return this.ReadString();
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0002BC24 File Offset: 0x0002AC24
		public sbyte ReadSByte(sbyte minValue, sbyte maxValue)
		{
			sbyte b = this.ReadSByte();
			if (b < minValue || maxValue < b)
			{
				throw new ArgumentOutOfRangeException();
			}
			return b;
		}
	}
}

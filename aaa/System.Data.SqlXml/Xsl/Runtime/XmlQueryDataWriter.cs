using System;
using System.IO;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000BB RID: 187
	internal class XmlQueryDataWriter : BinaryWriter
	{
		// Token: 0x06000920 RID: 2336 RVA: 0x0002BC47 File Offset: 0x0002AC47
		public XmlQueryDataWriter(Stream output)
			: base(output)
		{
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0002BC50 File Offset: 0x0002AC50
		public void WriteInt32Encoded(int value)
		{
			base.Write7BitEncodedInt(value);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0002BC59 File Offset: 0x0002AC59
		public void WriteStringQ(string value)
		{
			this.Write(value != null);
			if (value != null)
			{
				this.Write(value);
			}
		}
	}
}

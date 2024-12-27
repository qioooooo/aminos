using System;
using System.Xml;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000A1 RID: 161
	internal abstract class TraceRecord
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0003C798 File Offset: 0x0003BB98
		internal virtual string EventId
		{
			get
			{
				return "http://schemas.microsoft.com/2004/03/Transactions/EmptyTraceRecord";
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0003C7AC File Offset: 0x0003BBAC
		public override string ToString()
		{
			PlainXmlWriter plainXmlWriter = new PlainXmlWriter();
			this.WriteTo(plainXmlWriter);
			return plainXmlWriter.ToString();
		}

		// Token: 0x0600049B RID: 1179
		internal abstract void WriteTo(XmlWriter xml);

		// Token: 0x04000272 RID: 626
		protected internal const string EventIdBase = "http://schemas.microsoft.com/2004/03/Transactions/";

		// Token: 0x04000273 RID: 627
		protected internal const string NamespaceSuffix = "TraceRecord";
	}
}

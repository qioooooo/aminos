using System;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000B2 RID: 178
	internal class XmlILQueryEventArgs : XsltMessageEncounteredEventArgs
	{
		// Token: 0x06000835 RID: 2101 RVA: 0x00028EEB File Offset: 0x00027EEB
		public XmlILQueryEventArgs(string message)
		{
			this.message = message;
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x00028EFA File Offset: 0x00027EFA
		public override string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x0400057B RID: 1403
		private string message;
	}
}

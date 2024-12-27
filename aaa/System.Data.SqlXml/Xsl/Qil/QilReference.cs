using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200004C RID: 76
	internal class QilReference : QilNode
	{
		// Token: 0x06000537 RID: 1335 RVA: 0x00020E80 File Offset: 0x0001FE80
		public QilReference(QilNodeType nodeType)
			: base(nodeType)
		{
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00020E89 File Offset: 0x0001FE89
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x00020E91 File Offset: 0x0001FE91
		public string DebugName
		{
			get
			{
				return this.debugName;
			}
			set
			{
				if (value.Length > 1023)
				{
					value = value.Substring(0, 1023);
				}
				this.debugName = value;
			}
		}

		// Token: 0x0400038D RID: 909
		private const int MaxDebugNameLength = 1023;

		// Token: 0x0400038E RID: 910
		private string debugName;
	}
}

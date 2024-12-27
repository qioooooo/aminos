using System;

namespace System.Xml.Serialization
{
	// Token: 0x0200033C RID: 828
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public class XmlTextAttribute : Attribute
	{
		// Token: 0x06002890 RID: 10384 RVA: 0x000D1C53 File Offset: 0x000D0C53
		public XmlTextAttribute()
		{
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000D1C5B File Offset: 0x000D0C5B
		public XmlTextAttribute(Type type)
		{
			this.type = type;
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06002892 RID: 10386 RVA: 0x000D1C6A File Offset: 0x000D0C6A
		// (set) Token: 0x06002893 RID: 10387 RVA: 0x000D1C72 File Offset: 0x000D0C72
		public Type Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06002894 RID: 10388 RVA: 0x000D1C7B File Offset: 0x000D0C7B
		// (set) Token: 0x06002895 RID: 10389 RVA: 0x000D1C91 File Offset: 0x000D0C91
		public string DataType
		{
			get
			{
				if (this.dataType != null)
				{
					return this.dataType;
				}
				return string.Empty;
			}
			set
			{
				this.dataType = value;
			}
		}

		// Token: 0x04001686 RID: 5766
		private Type type;

		// Token: 0x04001687 RID: 5767
		private string dataType;
	}
}

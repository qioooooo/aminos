using System;

namespace System.Xml.Serialization
{
	// Token: 0x0200033D RID: 829
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
	public class XmlTypeAttribute : Attribute
	{
		// Token: 0x06002896 RID: 10390 RVA: 0x000D1C9A File Offset: 0x000D0C9A
		public XmlTypeAttribute()
		{
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000D1CA9 File Offset: 0x000D0CA9
		public XmlTypeAttribute(string typeName)
		{
			this.typeName = typeName;
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06002898 RID: 10392 RVA: 0x000D1CBF File Offset: 0x000D0CBF
		// (set) Token: 0x06002899 RID: 10393 RVA: 0x000D1CC7 File Offset: 0x000D0CC7
		public bool AnonymousType
		{
			get
			{
				return this.anonymousType;
			}
			set
			{
				this.anonymousType = value;
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x0600289A RID: 10394 RVA: 0x000D1CD0 File Offset: 0x000D0CD0
		// (set) Token: 0x0600289B RID: 10395 RVA: 0x000D1CD8 File Offset: 0x000D0CD8
		public bool IncludeInSchema
		{
			get
			{
				return this.includeInSchema;
			}
			set
			{
				this.includeInSchema = value;
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x0600289C RID: 10396 RVA: 0x000D1CE1 File Offset: 0x000D0CE1
		// (set) Token: 0x0600289D RID: 10397 RVA: 0x000D1CF7 File Offset: 0x000D0CF7
		public string TypeName
		{
			get
			{
				if (this.typeName != null)
				{
					return this.typeName;
				}
				return string.Empty;
			}
			set
			{
				this.typeName = value;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x000D1D00 File Offset: 0x000D0D00
		// (set) Token: 0x0600289F RID: 10399 RVA: 0x000D1D08 File Offset: 0x000D0D08
		public string Namespace
		{
			get
			{
				return this.ns;
			}
			set
			{
				this.ns = value;
			}
		}

		// Token: 0x04001688 RID: 5768
		private bool includeInSchema = true;

		// Token: 0x04001689 RID: 5769
		private bool anonymousType;

		// Token: 0x0400168A RID: 5770
		private string ns;

		// Token: 0x0400168B RID: 5771
		private string typeName;
	}
}

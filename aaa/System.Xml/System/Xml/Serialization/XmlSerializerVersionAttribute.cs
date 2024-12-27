using System;

namespace System.Xml.Serialization
{
	// Token: 0x0200033B RID: 827
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class XmlSerializerVersionAttribute : Attribute
	{
		// Token: 0x06002886 RID: 10374 RVA: 0x000D1BF8 File Offset: 0x000D0BF8
		public XmlSerializerVersionAttribute()
		{
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x000D1C00 File Offset: 0x000D0C00
		public XmlSerializerVersionAttribute(Type type)
		{
			this.type = type;
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06002888 RID: 10376 RVA: 0x000D1C0F File Offset: 0x000D0C0F
		// (set) Token: 0x06002889 RID: 10377 RVA: 0x000D1C17 File Offset: 0x000D0C17
		public string ParentAssemblyId
		{
			get
			{
				return this.mvid;
			}
			set
			{
				this.mvid = value;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x0600288A RID: 10378 RVA: 0x000D1C20 File Offset: 0x000D0C20
		// (set) Token: 0x0600288B RID: 10379 RVA: 0x000D1C28 File Offset: 0x000D0C28
		public string Version
		{
			get
			{
				return this.serializerVersion;
			}
			set
			{
				this.serializerVersion = value;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x0600288C RID: 10380 RVA: 0x000D1C31 File Offset: 0x000D0C31
		// (set) Token: 0x0600288D RID: 10381 RVA: 0x000D1C39 File Offset: 0x000D0C39
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

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x0600288E RID: 10382 RVA: 0x000D1C42 File Offset: 0x000D0C42
		// (set) Token: 0x0600288F RID: 10383 RVA: 0x000D1C4A File Offset: 0x000D0C4A
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

		// Token: 0x04001682 RID: 5762
		private string mvid;

		// Token: 0x04001683 RID: 5763
		private string serializerVersion;

		// Token: 0x04001684 RID: 5764
		private string ns;

		// Token: 0x04001685 RID: 5765
		private Type type;
	}
}

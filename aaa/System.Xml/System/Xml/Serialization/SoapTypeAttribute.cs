using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002F3 RID: 755
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
	public class SoapTypeAttribute : Attribute
	{
		// Token: 0x06002361 RID: 9057 RVA: 0x000A817C File Offset: 0x000A717C
		public SoapTypeAttribute()
		{
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x000A818B File Offset: 0x000A718B
		public SoapTypeAttribute(string typeName)
		{
			this.typeName = typeName;
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x000A81A1 File Offset: 0x000A71A1
		public SoapTypeAttribute(string typeName, string ns)
		{
			this.typeName = typeName;
			this.ns = ns;
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002364 RID: 9060 RVA: 0x000A81BE File Offset: 0x000A71BE
		// (set) Token: 0x06002365 RID: 9061 RVA: 0x000A81C6 File Offset: 0x000A71C6
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

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002366 RID: 9062 RVA: 0x000A81CF File Offset: 0x000A71CF
		// (set) Token: 0x06002367 RID: 9063 RVA: 0x000A81E5 File Offset: 0x000A71E5
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

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002368 RID: 9064 RVA: 0x000A81EE File Offset: 0x000A71EE
		// (set) Token: 0x06002369 RID: 9065 RVA: 0x000A81F6 File Offset: 0x000A71F6
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

		// Token: 0x040014F5 RID: 5365
		private string ns;

		// Token: 0x040014F6 RID: 5366
		private string typeName;

		// Token: 0x040014F7 RID: 5367
		private bool includeInSchema = true;
	}
}

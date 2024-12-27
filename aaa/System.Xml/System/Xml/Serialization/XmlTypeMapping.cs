using System;

namespace System.Xml.Serialization
{
	// Token: 0x0200033E RID: 830
	public class XmlTypeMapping : XmlMapping
	{
		// Token: 0x060028A0 RID: 10400 RVA: 0x000D1D11 File Offset: 0x000D0D11
		internal XmlTypeMapping(TypeScope scope, ElementAccessor accessor)
			: base(scope, accessor)
		{
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x060028A1 RID: 10401 RVA: 0x000D1D1B File Offset: 0x000D0D1B
		internal TypeMapping Mapping
		{
			get
			{
				return base.Accessor.Mapping;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x000D1D28 File Offset: 0x000D0D28
		public string TypeName
		{
			get
			{
				return this.Mapping.TypeDesc.Name;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x060028A3 RID: 10403 RVA: 0x000D1D3A File Offset: 0x000D0D3A
		public string TypeFullName
		{
			get
			{
				return this.Mapping.TypeDesc.FullName;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x060028A4 RID: 10404 RVA: 0x000D1D4C File Offset: 0x000D0D4C
		public string XsdTypeName
		{
			get
			{
				return this.Mapping.TypeName;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x060028A5 RID: 10405 RVA: 0x000D1D59 File Offset: 0x000D0D59
		public string XsdTypeNamespace
		{
			get
			{
				return this.Mapping.Namespace;
			}
		}
	}
}

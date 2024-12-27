using System;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000B4 RID: 180
	public sealed class XmlSchemaSearchPattern : DiscoverySearchPattern
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x00017D90 File Offset: 0x00016D90
		public override string Pattern
		{
			get
			{
				return "*.xsd";
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00017D97 File Offset: 0x00016D97
		public override DiscoveryReference GetDiscoveryReference(string filename)
		{
			return new SchemaReference(filename);
		}
	}
}

using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002C6 RID: 710
	internal class PrimitiveMapping : TypeMapping
	{
		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x060021B6 RID: 8630 RVA: 0x0009F254 File Offset: 0x0009E254
		// (set) Token: 0x060021B7 RID: 8631 RVA: 0x0009F25C File Offset: 0x0009E25C
		internal override bool IsList
		{
			get
			{
				return this.isList;
			}
			set
			{
				this.isList = value;
			}
		}

		// Token: 0x04001475 RID: 5237
		private bool isList;
	}
}

using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002C7 RID: 711
	internal class NullableMapping : TypeMapping
	{
		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x060021B9 RID: 8633 RVA: 0x0009F26D File Offset: 0x0009E26D
		// (set) Token: 0x060021BA RID: 8634 RVA: 0x0009F275 File Offset: 0x0009E275
		internal TypeMapping BaseMapping
		{
			get
			{
				return this.baseMapping;
			}
			set
			{
				this.baseMapping = value;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x060021BB RID: 8635 RVA: 0x0009F27E File Offset: 0x0009E27E
		internal override string DefaultElementName
		{
			get
			{
				return this.BaseMapping.DefaultElementName;
			}
		}

		// Token: 0x04001476 RID: 5238
		private TypeMapping baseMapping;
	}
}

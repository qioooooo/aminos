using System;

namespace System.Xml.Serialization
{
	// Token: 0x0200030C RID: 780
	[AttributeUsage(AttributeTargets.Field)]
	public class XmlEnumAttribute : Attribute
	{
		// Token: 0x06002504 RID: 9476 RVA: 0x000ADF01 File Offset: 0x000ACF01
		public XmlEnumAttribute()
		{
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x000ADF09 File Offset: 0x000ACF09
		public XmlEnumAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06002506 RID: 9478 RVA: 0x000ADF18 File Offset: 0x000ACF18
		// (set) Token: 0x06002507 RID: 9479 RVA: 0x000ADF20 File Offset: 0x000ACF20
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x0400157E RID: 5502
		private string name;
	}
}

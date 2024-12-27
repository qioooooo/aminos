using System;

namespace System.Xml.Serialization
{
	// Token: 0x0200031A RID: 794
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.ReturnValue)]
	public class XmlRootAttribute : Attribute
	{
		// Token: 0x06002598 RID: 9624 RVA: 0x000B35DC File Offset: 0x000B25DC
		public XmlRootAttribute()
		{
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000B35EB File Offset: 0x000B25EB
		public XmlRootAttribute(string elementName)
		{
			this.elementName = elementName;
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x0600259A RID: 9626 RVA: 0x000B3601 File Offset: 0x000B2601
		// (set) Token: 0x0600259B RID: 9627 RVA: 0x000B3617 File Offset: 0x000B2617
		public string ElementName
		{
			get
			{
				if (this.elementName != null)
				{
					return this.elementName;
				}
				return string.Empty;
			}
			set
			{
				this.elementName = value;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x0600259C RID: 9628 RVA: 0x000B3620 File Offset: 0x000B2620
		// (set) Token: 0x0600259D RID: 9629 RVA: 0x000B3628 File Offset: 0x000B2628
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

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x000B3631 File Offset: 0x000B2631
		// (set) Token: 0x0600259F RID: 9631 RVA: 0x000B3647 File Offset: 0x000B2647
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

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x000B3650 File Offset: 0x000B2650
		// (set) Token: 0x060025A1 RID: 9633 RVA: 0x000B3658 File Offset: 0x000B2658
		public bool IsNullable
		{
			get
			{
				return this.nullable;
			}
			set
			{
				this.nullable = value;
				this.nullableSpecified = true;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x000B3668 File Offset: 0x000B2668
		internal bool IsNullableSpecified
		{
			get
			{
				return this.nullableSpecified;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060025A3 RID: 9635 RVA: 0x000B3670 File Offset: 0x000B2670
		internal string Key
		{
			get
			{
				return string.Concat(new string[]
				{
					(this.ns == null) ? string.Empty : this.ns,
					":",
					this.ElementName,
					":",
					this.nullable.ToString()
				});
			}
		}

		// Token: 0x040015AE RID: 5550
		private string elementName;

		// Token: 0x040015AF RID: 5551
		private string ns;

		// Token: 0x040015B0 RID: 5552
		private string dataType;

		// Token: 0x040015B1 RID: 5553
		private bool nullable = true;

		// Token: 0x040015B2 RID: 5554
		private bool nullableSpecified;
	}
}

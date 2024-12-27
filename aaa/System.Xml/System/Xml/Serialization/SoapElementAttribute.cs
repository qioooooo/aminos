using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002EB RID: 747
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public class SoapElementAttribute : Attribute
	{
		// Token: 0x060022F0 RID: 8944 RVA: 0x000A4501 File Offset: 0x000A3501
		public SoapElementAttribute()
		{
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x000A4509 File Offset: 0x000A3509
		public SoapElementAttribute(string elementName)
		{
			this.elementName = elementName;
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060022F2 RID: 8946 RVA: 0x000A4518 File Offset: 0x000A3518
		// (set) Token: 0x060022F3 RID: 8947 RVA: 0x000A452E File Offset: 0x000A352E
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

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x060022F4 RID: 8948 RVA: 0x000A4537 File Offset: 0x000A3537
		// (set) Token: 0x060022F5 RID: 8949 RVA: 0x000A454D File Offset: 0x000A354D
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

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x060022F6 RID: 8950 RVA: 0x000A4556 File Offset: 0x000A3556
		// (set) Token: 0x060022F7 RID: 8951 RVA: 0x000A455E File Offset: 0x000A355E
		public bool IsNullable
		{
			get
			{
				return this.nullable;
			}
			set
			{
				this.nullable = value;
			}
		}

		// Token: 0x040014DF RID: 5343
		private string elementName;

		// Token: 0x040014E0 RID: 5344
		private string dataType;

		// Token: 0x040014E1 RID: 5345
		private bool nullable;
	}
}

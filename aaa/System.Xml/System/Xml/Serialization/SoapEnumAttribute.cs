using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002EC RID: 748
	[AttributeUsage(AttributeTargets.Field)]
	public class SoapEnumAttribute : Attribute
	{
		// Token: 0x060022F8 RID: 8952 RVA: 0x000A4567 File Offset: 0x000A3567
		public SoapEnumAttribute()
		{
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x000A456F File Offset: 0x000A356F
		public SoapEnumAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x000A457E File Offset: 0x000A357E
		// (set) Token: 0x060022FB RID: 8955 RVA: 0x000A4594 File Offset: 0x000A3594
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x040014E2 RID: 5346
		private string name;
	}
}

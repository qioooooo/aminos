using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002EE RID: 750
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true)]
	public class SoapIncludeAttribute : Attribute
	{
		// Token: 0x060022FD RID: 8957 RVA: 0x000A45A5 File Offset: 0x000A35A5
		public SoapIncludeAttribute(Type type)
		{
			this.type = type;
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x060022FE RID: 8958 RVA: 0x000A45B4 File Offset: 0x000A35B4
		// (set) Token: 0x060022FF RID: 8959 RVA: 0x000A45BC File Offset: 0x000A35BC
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

		// Token: 0x040014E3 RID: 5347
		private Type type;
	}
}

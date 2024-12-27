using System;

namespace System.Xml.Serialization
{
	// Token: 0x0200030E RID: 782
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = true)]
	public class XmlIncludeAttribute : Attribute
	{
		// Token: 0x06002509 RID: 9481 RVA: 0x000ADF31 File Offset: 0x000ACF31
		public XmlIncludeAttribute(Type type)
		{
			this.type = type;
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x0600250A RID: 9482 RVA: 0x000ADF40 File Offset: 0x000ACF40
		// (set) Token: 0x0600250B RID: 9483 RVA: 0x000ADF48 File Offset: 0x000ACF48
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

		// Token: 0x0400157F RID: 5503
		private Type type;
	}
}

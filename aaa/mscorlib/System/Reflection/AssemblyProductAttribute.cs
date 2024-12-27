using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002CB RID: 715
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyProductAttribute : Attribute
	{
		// Token: 0x06001CAC RID: 7340 RVA: 0x00049932 File Offset: 0x00048932
		public AssemblyProductAttribute(string product)
		{
			this.m_product = product;
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001CAD RID: 7341 RVA: 0x00049941 File Offset: 0x00048941
		public string Product
		{
			get
			{
				return this.m_product;
			}
		}

		// Token: 0x04000A7C RID: 2684
		private string m_product;
	}
}

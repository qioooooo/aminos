using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002C9 RID: 713
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCopyrightAttribute : Attribute
	{
		// Token: 0x06001CA8 RID: 7336 RVA: 0x00049904 File Offset: 0x00048904
		public AssemblyCopyrightAttribute(string copyright)
		{
			this.m_copyright = copyright;
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001CA9 RID: 7337 RVA: 0x00049913 File Offset: 0x00048913
		public string Copyright
		{
			get
			{
				return this.m_copyright;
			}
		}

		// Token: 0x04000A7A RID: 2682
		private string m_copyright;
	}
}

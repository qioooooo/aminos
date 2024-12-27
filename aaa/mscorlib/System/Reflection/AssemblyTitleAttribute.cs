using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002CE RID: 718
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyTitleAttribute : Attribute
	{
		// Token: 0x06001CB2 RID: 7346 RVA: 0x00049977 File Offset: 0x00048977
		public AssemblyTitleAttribute(string title)
		{
			this.m_title = title;
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001CB3 RID: 7347 RVA: 0x00049986 File Offset: 0x00048986
		public string Title
		{
			get
			{
				return this.m_title;
			}
		}

		// Token: 0x04000A7F RID: 2687
		private string m_title;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002D5 RID: 725
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyKeyFileAttribute : Attribute
	{
		// Token: 0x06001CC0 RID: 7360 RVA: 0x00049A26 File Offset: 0x00048A26
		public AssemblyKeyFileAttribute(string keyFile)
		{
			this.m_keyFile = keyFile;
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x00049A35 File Offset: 0x00048A35
		public string KeyFile
		{
			get
			{
				return this.m_keyFile;
			}
		}

		// Token: 0x04000A86 RID: 2694
		private string m_keyFile;
	}
}

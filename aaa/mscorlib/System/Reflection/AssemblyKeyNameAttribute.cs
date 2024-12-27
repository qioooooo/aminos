using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002D9 RID: 729
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyKeyNameAttribute : Attribute
	{
		// Token: 0x06001CCC RID: 7372 RVA: 0x00049AB7 File Offset: 0x00048AB7
		public AssemblyKeyNameAttribute(string keyName)
		{
			this.m_keyName = keyName;
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001CCD RID: 7373 RVA: 0x00049AC6 File Offset: 0x00048AC6
		public string KeyName
		{
			get
			{
				return this.m_keyName;
			}
		}

		// Token: 0x04000A8A RID: 2698
		private string m_keyName;
	}
}

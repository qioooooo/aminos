using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000355 RID: 853
	internal class TypeLoadExceptionHolder
	{
		// Token: 0x0600222C RID: 8748 RVA: 0x00056D33 File Offset: 0x00055D33
		internal TypeLoadExceptionHolder(string typeName)
		{
			this.m_typeName = typeName;
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x0600222D RID: 8749 RVA: 0x00056D42 File Offset: 0x00055D42
		internal string TypeName
		{
			get
			{
				return this.m_typeName;
			}
		}

		// Token: 0x04000E36 RID: 3638
		private string m_typeName;
	}
}

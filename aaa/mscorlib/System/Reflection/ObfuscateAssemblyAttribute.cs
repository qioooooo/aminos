using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200031D RID: 797
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	public sealed class ObfuscateAssemblyAttribute : Attribute
	{
		// Token: 0x06001F3B RID: 7995 RVA: 0x0004F28B File Offset: 0x0004E28B
		public ObfuscateAssemblyAttribute(bool assemblyIsPrivate)
		{
			this.m_assemblyIsPrivate = assemblyIsPrivate;
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x0004F2A1 File Offset: 0x0004E2A1
		public bool AssemblyIsPrivate
		{
			get
			{
				return this.m_assemblyIsPrivate;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001F3D RID: 7997 RVA: 0x0004F2A9 File Offset: 0x0004E2A9
		// (set) Token: 0x06001F3E RID: 7998 RVA: 0x0004F2B1 File Offset: 0x0004E2B1
		public bool StripAfterObfuscation
		{
			get
			{
				return this.m_strip;
			}
			set
			{
				this.m_strip = value;
			}
		}

		// Token: 0x04000D33 RID: 3379
		private bool m_assemblyIsPrivate;

		// Token: 0x04000D34 RID: 3380
		private bool m_strip = true;
	}
}

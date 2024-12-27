using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002D2 RID: 722
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyFileVersionAttribute : Attribute
	{
		// Token: 0x06001CBA RID: 7354 RVA: 0x000499D3 File Offset: 0x000489D3
		public AssemblyFileVersionAttribute(string version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this._version = version;
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001CBB RID: 7355 RVA: 0x000499F0 File Offset: 0x000489F0
		public string Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x04000A83 RID: 2691
		private string _version;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Resources
{
	// Token: 0x02000429 RID: 1065
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	[ComVisible(true)]
	public sealed class SatelliteContractVersionAttribute : Attribute
	{
		// Token: 0x06002C0D RID: 11277 RVA: 0x000968E0 File Offset: 0x000958E0
		public SatelliteContractVersionAttribute(string version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this._version = version;
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002C0E RID: 11278 RVA: 0x000968FD File Offset: 0x000958FD
		public string Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x04001567 RID: 5479
		private string _version;
	}
}

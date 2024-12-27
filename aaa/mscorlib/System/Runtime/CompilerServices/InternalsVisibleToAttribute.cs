using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005D3 RID: 1491
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public sealed class InternalsVisibleToAttribute : Attribute
	{
		// Token: 0x06003799 RID: 14233 RVA: 0x000BB89D File Offset: 0x000BA89D
		public InternalsVisibleToAttribute(string assemblyName)
		{
			this._assemblyName = assemblyName;
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x0600379A RID: 14234 RVA: 0x000BB8B3 File Offset: 0x000BA8B3
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600379B RID: 14235 RVA: 0x000BB8BB File Offset: 0x000BA8BB
		// (set) Token: 0x0600379C RID: 14236 RVA: 0x000BB8C3 File Offset: 0x000BA8C3
		public bool AllInternalsVisible
		{
			get
			{
				return this._allInternalsVisible;
			}
			set
			{
				this._allInternalsVisible = value;
			}
		}

		// Token: 0x04001C9D RID: 7325
		private string _assemblyName;

		// Token: 0x04001C9E RID: 7326
		private bool _allInternalsVisible = true;
	}
}

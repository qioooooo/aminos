using System;

namespace System.Deployment.Application
{
	// Token: 0x02000078 RID: 120
	internal class AssemblyModule
	{
		// Token: 0x0600039D RID: 925 RVA: 0x0001519A File Offset: 0x0001419A
		public AssemblyModule(string name, byte[] hash)
		{
			this._name = name;
			this._hash = hash;
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600039E RID: 926 RVA: 0x000151B0 File Offset: 0x000141B0
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600039F RID: 927 RVA: 0x000151B8 File Offset: 0x000141B8
		public byte[] Hash
		{
			get
			{
				return this._hash;
			}
		}

		// Token: 0x040002A2 RID: 674
		private string _name;

		// Token: 0x040002A3 RID: 675
		private byte[] _hash;
	}
}

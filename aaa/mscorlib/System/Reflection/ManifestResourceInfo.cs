using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002FC RID: 764
	[ComVisible(true)]
	public class ManifestResourceInfo
	{
		// Token: 0x06001E52 RID: 7762 RVA: 0x0004D1F2 File Offset: 0x0004C1F2
		internal ManifestResourceInfo(Assembly containingAssembly, string containingFileName, ResourceLocation resourceLocation)
		{
			this._containingAssembly = containingAssembly;
			this._containingFileName = containingFileName;
			this._resourceLocation = resourceLocation;
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001E53 RID: 7763 RVA: 0x0004D20F File Offset: 0x0004C20F
		public virtual Assembly ReferencedAssembly
		{
			get
			{
				return this._containingAssembly;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x0004D217 File Offset: 0x0004C217
		public virtual string FileName
		{
			get
			{
				return this._containingFileName;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x0004D21F File Offset: 0x0004C21F
		public virtual ResourceLocation ResourceLocation
		{
			get
			{
				return this._resourceLocation;
			}
		}

		// Token: 0x04000B1E RID: 2846
		private Assembly _containingAssembly;

		// Token: 0x04000B1F RID: 2847
		private string _containingFileName;

		// Token: 0x04000B20 RID: 2848
		private ResourceLocation _resourceLocation;
	}
}

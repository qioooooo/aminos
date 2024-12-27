using System;
using System.Resources;

namespace System.Web.Compilation
{
	// Token: 0x02000178 RID: 376
	internal class GlobalResXResourceProvider : BaseResXResourceProvider
	{
		// Token: 0x0600107F RID: 4223 RVA: 0x00049240 File Offset: 0x00048240
		internal GlobalResXResourceProvider(string classKey)
		{
			this._classKey = classKey;
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00049250 File Offset: 0x00048250
		protected override ResourceManager CreateResourceManager()
		{
			string text = "Resources." + this._classKey;
			if (BuildManager.AppResourcesAssembly == null)
			{
				return null;
			}
			return new ResourceManager(text, BuildManager.AppResourcesAssembly)
			{
				IgnoreCase = true
			};
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x0004928B File Offset: 0x0004828B
		public override IResourceReader ResourceReader
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x04001659 RID: 5721
		private string _classKey;
	}
}

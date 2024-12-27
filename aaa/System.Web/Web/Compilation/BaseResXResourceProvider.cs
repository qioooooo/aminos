using System;
using System.Globalization;
using System.Resources;

namespace System.Web.Compilation
{
	// Token: 0x02000177 RID: 375
	internal abstract class BaseResXResourceProvider : IResourceProvider
	{
		// Token: 0x0600107A RID: 4218 RVA: 0x000491F5 File Offset: 0x000481F5
		public virtual object GetObject(string resourceKey, CultureInfo culture)
		{
			this.EnsureResourceManager();
			if (this._resourceManager == null)
			{
				return null;
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentUICulture;
			}
			return this._resourceManager.GetObject(resourceKey, culture);
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x0004921E File Offset: 0x0004821E
		public virtual IResourceReader ResourceReader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600107C RID: 4220
		protected abstract ResourceManager CreateResourceManager();

		// Token: 0x0600107D RID: 4221 RVA: 0x00049221 File Offset: 0x00048221
		private void EnsureResourceManager()
		{
			if (this._resourceManager != null)
			{
				return;
			}
			this._resourceManager = this.CreateResourceManager();
		}

		// Token: 0x04001658 RID: 5720
		private ResourceManager _resourceManager;
	}
}

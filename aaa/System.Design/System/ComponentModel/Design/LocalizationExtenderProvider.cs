using System;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.ComponentModel.Design
{
	// Token: 0x02000132 RID: 306
	[ProvideProperty("LoadLanguage", typeof(object))]
	[ProvideProperty("Language", typeof(object))]
	[ProvideProperty("Localizable", typeof(object))]
	[Obsolete("This class has been deprecated. Use CodeDomLocalizationProvider instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
	public class LocalizationExtenderProvider : IExtenderProvider, IDisposable
	{
		// Token: 0x06000BEB RID: 3051 RVA: 0x0002EA40 File Offset: 0x0002DA40
		public LocalizationExtenderProvider(ISite serviceProvider, IComponent baseComponent)
		{
			this.serviceProvider = serviceProvider;
			this.baseComponent = baseComponent;
			if (serviceProvider != null)
			{
				IExtenderProviderService extenderProviderService = (IExtenderProviderService)serviceProvider.GetService(typeof(IExtenderProviderService));
				if (extenderProviderService != null)
				{
					extenderProviderService.AddExtenderProvider(this);
				}
			}
			this.language = CultureInfo.InvariantCulture;
			ResourceManager resourceManager = new ResourceManager(baseComponent.GetType());
			if (resourceManager != null)
			{
				ResourceSet resourceSet = resourceManager.GetResourceSet(this.language, true, false);
				if (resourceSet != null)
				{
					object @object = resourceSet.GetObject("$this.Localizable");
					if (@object is bool)
					{
						this.defaultLocalizable = (bool)@object;
						this.localizable = this.defaultLocalizable;
					}
				}
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x0002EADC File Offset: 0x0002DADC
		private CultureInfo ThreadDefaultLanguage
		{
			get
			{
				lock (LocalizationExtenderProvider.localizationLock)
				{
					if (this.defaultLanguage != null)
					{
						return this.defaultLanguage;
					}
					LocalDataStoreSlot namedDataSlot = Thread.GetNamedDataSlot("_Thread_Default_Language");
					if (namedDataSlot == null)
					{
						return null;
					}
					this.defaultLanguage = (CultureInfo)Thread.GetData(namedDataSlot);
					if (this.defaultLanguage == null)
					{
						this.defaultLanguage = Application.CurrentCulture;
						Thread.SetData(namedDataSlot, this.defaultLanguage);
					}
				}
				return this.defaultLanguage;
			}
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x0002EB6C File Offset: 0x0002DB6C
		[DesignOnly(true)]
		[Localizable(true)]
		[SRDescription("ParentControlDesignerLanguageDescr")]
		public CultureInfo GetLanguage(object o)
		{
			return this.language;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0002EB74 File Offset: 0x0002DB74
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DesignOnly(true)]
		[Browsable(false)]
		public CultureInfo GetLoadLanguage(object o)
		{
			if (this.loadLanguage == null)
			{
				this.loadLanguage = CultureInfo.InvariantCulture;
			}
			return this.loadLanguage;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0002EB8F File Offset: 0x0002DB8F
		[SRDescription("ParentControlDesignerLocalizableDescr")]
		[DesignOnly(true)]
		[Localizable(true)]
		public bool GetLocalizable(object o)
		{
			return this.localizable;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0002EB98 File Offset: 0x0002DB98
		public void SetLanguage(object o, CultureInfo language)
		{
			if (language == null)
			{
				language = CultureInfo.InvariantCulture;
			}
			if (this.language.Equals(language))
			{
				return;
			}
			bool flag = language.Equals(CultureInfo.InvariantCulture);
			CultureInfo threadDefaultLanguage = this.ThreadDefaultLanguage;
			this.language = language;
			if (!flag)
			{
				this.SetLocalizable(null, true);
			}
			if (this.serviceProvider != null)
			{
				IDesignerLoaderService designerLoaderService = (IDesignerLoaderService)this.serviceProvider.GetService(typeof(IDesignerLoaderService));
				IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					if (designerHost.Loading)
					{
						this.loadLanguage = language;
						return;
					}
					bool flag2 = false;
					if (designerLoaderService != null)
					{
						flag2 = designerLoaderService.Reload();
					}
					if (!flag2)
					{
						IUIService iuiservice = (IUIService)this.serviceProvider.GetService(typeof(IUIService));
						if (iuiservice != null)
						{
							iuiservice.ShowMessage(SR.GetString("LocalizerManualReload"));
						}
					}
				}
			}
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0002EC77 File Offset: 0x0002DC77
		public void SetLocalizable(object o, bool localizable)
		{
			this.localizable = localizable;
			if (!localizable)
			{
				this.SetLanguage(null, CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0002EC8F File Offset: 0x0002DC8F
		public bool ShouldSerializeLanguage(object o)
		{
			return this.language != null && this.language != CultureInfo.InvariantCulture;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0002ECAB File Offset: 0x0002DCAB
		private bool ShouldSerializeLocalizable(object o)
		{
			return this.localizable != this.defaultLocalizable;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0002ECBE File Offset: 0x0002DCBE
		private void ResetLocalizable(object o)
		{
			this.SetLocalizable(null, this.defaultLocalizable);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0002ECCD File Offset: 0x0002DCCD
		public void ResetLanguage(object o)
		{
			this.SetLanguage(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0002ECDB File Offset: 0x0002DCDB
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0002ECE4 File Offset: 0x0002DCE4
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.serviceProvider != null)
			{
				IExtenderProviderService extenderProviderService = (IExtenderProviderService)this.serviceProvider.GetService(typeof(IExtenderProviderService));
				if (extenderProviderService != null)
				{
					extenderProviderService.RemoveExtenderProvider(this);
				}
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0002ED21 File Offset: 0x0002DD21
		public bool CanExtend(object o)
		{
			return o.Equals(this.baseComponent);
		}

		// Token: 0x04000E6A RID: 3690
		private const string KeyThreadDefaultLanguage = "_Thread_Default_Language";

		// Token: 0x04000E6B RID: 3691
		private IServiceProvider serviceProvider;

		// Token: 0x04000E6C RID: 3692
		private IComponent baseComponent;

		// Token: 0x04000E6D RID: 3693
		private bool localizable;

		// Token: 0x04000E6E RID: 3694
		private bool defaultLocalizable;

		// Token: 0x04000E6F RID: 3695
		private CultureInfo language;

		// Token: 0x04000E70 RID: 3696
		private CultureInfo loadLanguage;

		// Token: 0x04000E71 RID: 3697
		private CultureInfo defaultLanguage;

		// Token: 0x04000E72 RID: 3698
		private static object localizationLock = new object();
	}
}

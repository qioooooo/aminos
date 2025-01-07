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
	[ProvideProperty("LoadLanguage", typeof(object))]
	[ProvideProperty("Language", typeof(object))]
	[ProvideProperty("Localizable", typeof(object))]
	[Obsolete("This class has been deprecated. Use CodeDomLocalizationProvider instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
	public class LocalizationExtenderProvider : IExtenderProvider, IDisposable
	{
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

		[DesignOnly(true)]
		[Localizable(true)]
		[SRDescription("ParentControlDesignerLanguageDescr")]
		public CultureInfo GetLanguage(object o)
		{
			return this.language;
		}

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

		[SRDescription("ParentControlDesignerLocalizableDescr")]
		[DesignOnly(true)]
		[Localizable(true)]
		public bool GetLocalizable(object o)
		{
			return this.localizable;
		}

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

		public void SetLocalizable(object o, bool localizable)
		{
			this.localizable = localizable;
			if (!localizable)
			{
				this.SetLanguage(null, CultureInfo.InvariantCulture);
			}
		}

		public bool ShouldSerializeLanguage(object o)
		{
			return this.language != null && this.language != CultureInfo.InvariantCulture;
		}

		private bool ShouldSerializeLocalizable(object o)
		{
			return this.localizable != this.defaultLocalizable;
		}

		private void ResetLocalizable(object o)
		{
			this.SetLocalizable(null, this.defaultLocalizable);
		}

		public void ResetLanguage(object o)
		{
			this.SetLanguage(null, CultureInfo.InvariantCulture);
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

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

		public bool CanExtend(object o)
		{
			return o.Equals(this.baseComponent);
		}

		private const string KeyThreadDefaultLanguage = "_Thread_Default_Language";

		private IServiceProvider serviceProvider;

		private IComponent baseComponent;

		private bool localizable;

		private bool defaultLocalizable;

		private CultureInfo language;

		private CultureInfo loadLanguage;

		private CultureInfo defaultLanguage;

		private static object localizationLock = new object();
	}
}

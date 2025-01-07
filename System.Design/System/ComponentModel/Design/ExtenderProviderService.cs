using System;
using System.Collections;
using System.Design;

namespace System.ComponentModel.Design
{
	internal sealed class ExtenderProviderService : IExtenderProviderService, IExtenderListService
	{
		internal ExtenderProviderService()
		{
		}

		IExtenderProvider[] IExtenderListService.GetExtenderProviders()
		{
			if (this._providers != null)
			{
				IExtenderProvider[] array = new IExtenderProvider[this._providers.Count];
				this._providers.CopyTo(array, 0);
				return array;
			}
			return new IExtenderProvider[0];
		}

		void IExtenderProviderService.AddExtenderProvider(IExtenderProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (this._providers == null)
			{
				this._providers = new ArrayList(4);
			}
			if (this._providers.Contains(provider))
			{
				throw new ArgumentException(SR.GetString("ExtenderProviderServiceDuplicateProvider", new object[] { provider }));
			}
			this._providers.Add(provider);
		}

		void IExtenderProviderService.RemoveExtenderProvider(IExtenderProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (this._providers != null)
			{
				this._providers.Remove(provider);
			}
		}

		private ArrayList _providers;
	}
}

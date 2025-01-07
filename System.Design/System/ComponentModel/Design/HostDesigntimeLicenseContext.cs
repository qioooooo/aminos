using System;

namespace System.ComponentModel.Design
{
	internal class HostDesigntimeLicenseContext : DesigntimeLicenseContext
	{
		public HostDesigntimeLicenseContext(IServiceProvider provider)
		{
			this.provider = provider;
		}

		public override object GetService(Type serviceClass)
		{
			return this.provider.GetService(serviceClass);
		}

		private IServiceProvider provider;
	}
}

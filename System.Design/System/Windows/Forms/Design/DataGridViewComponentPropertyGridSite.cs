using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal class DataGridViewComponentPropertyGridSite : ISite, IServiceProvider
	{
		public DataGridViewComponentPropertyGridSite(IServiceProvider sp, IComponent comp)
		{
			this.sp = sp;
			this.comp = comp;
		}

		public IComponent Component
		{
			get
			{
				return this.comp;
			}
		}

		public IContainer Container
		{
			get
			{
				return null;
			}
		}

		public bool DesignMode
		{
			get
			{
				return false;
			}
		}

		public string Name
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public object GetService(Type t)
		{
			if (!this.inGetService && this.sp != null)
			{
				try
				{
					this.inGetService = true;
					return this.sp.GetService(t);
				}
				finally
				{
					this.inGetService = false;
				}
			}
			return null;
		}

		private IServiceProvider sp;

		private IComponent comp;

		private bool inGetService;
	}
}

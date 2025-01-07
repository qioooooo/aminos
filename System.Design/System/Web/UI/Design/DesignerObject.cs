using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Web.UI.Design
{
	public abstract class DesignerObject : IServiceProvider
	{
		protected DesignerObject(ControlDesigner designer, string name)
		{
			if (designer == null)
			{
				throw new ArgumentNullException("designer");
			}
			if (name == null || name.Length == 0)
			{
				throw new ArgumentNullException("name");
			}
			this._designer = designer;
			this._name = name;
		}

		public ControlDesigner Designer
		{
			get
			{
				return this._designer;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
		}

		public IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					this._properties = new HybridDictionary();
				}
				return this._properties;
			}
		}

		protected object GetService(Type serviceType)
		{
			IServiceProvider site = this._designer.Component.Site;
			if (site != null)
			{
				return site.GetService(serviceType);
			}
			return null;
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			return this.GetService(serviceType);
		}

		private ControlDesigner _designer;

		private string _name;

		private IDictionary _properties;
	}
}

using System;
using System.ComponentModel.Design;

namespace System.Web.UI.Design
{
	public abstract class DesignerAutoFormat
	{
		protected DesignerAutoFormat(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentNullException("name");
			}
			this._name = name;
		}

		public string Name
		{
			get
			{
				return this._name;
			}
		}

		public DesignerAutoFormatStyle Style
		{
			get
			{
				if (this._style == null)
				{
					this._style = new DesignerAutoFormatStyle();
				}
				return this._style;
			}
		}

		public abstract void Apply(Control control);

		public virtual Control GetPreviewControl(Control runtimeControl)
		{
			IDesignerHost designerHost = (IDesignerHost)runtimeControl.Site.GetService(typeof(IDesignerHost));
			ControlDesigner controlDesigner = designerHost.GetDesigner(runtimeControl) as ControlDesigner;
			if (controlDesigner != null)
			{
				return controlDesigner.CreateClonedControl(designerHost, true);
			}
			return null;
		}

		public override string ToString()
		{
			return this.Name;
		}

		private string _name;

		private DesignerAutoFormatStyle _style;
	}
}

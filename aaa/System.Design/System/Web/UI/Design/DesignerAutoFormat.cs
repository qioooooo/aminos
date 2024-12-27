using System;
using System.ComponentModel.Design;

namespace System.Web.UI.Design
{
	// Token: 0x02000358 RID: 856
	public abstract class DesignerAutoFormat
	{
		// Token: 0x0600201B RID: 8219 RVA: 0x000B6366 File Offset: 0x000B5366
		protected DesignerAutoFormat(string name)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentNullException("name");
			}
			this._name = name;
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x000B638B File Offset: 0x000B538B
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x000B6393 File Offset: 0x000B5393
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

		// Token: 0x0600201E RID: 8222
		public abstract void Apply(Control control);

		// Token: 0x0600201F RID: 8223 RVA: 0x000B63B0 File Offset: 0x000B53B0
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

		// Token: 0x06002020 RID: 8224 RVA: 0x000B63F2 File Offset: 0x000B53F2
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x040017C9 RID: 6089
		private string _name;

		// Token: 0x040017CA RID: 6090
		private DesignerAutoFormatStyle _style;
	}
}

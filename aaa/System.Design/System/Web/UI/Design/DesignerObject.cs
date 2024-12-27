using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Web.UI.Design
{
	// Token: 0x0200035D RID: 861
	public abstract class DesignerObject : IServiceProvider
	{
		// Token: 0x0600204E RID: 8270 RVA: 0x000B66CC File Offset: 0x000B56CC
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

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x0600204F RID: 8271 RVA: 0x000B6706 File Offset: 0x000B5706
		public ControlDesigner Designer
		{
			get
			{
				return this._designer;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002050 RID: 8272 RVA: 0x000B670E File Offset: 0x000B570E
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06002051 RID: 8273 RVA: 0x000B6716 File Offset: 0x000B5716
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

		// Token: 0x06002052 RID: 8274 RVA: 0x000B6734 File Offset: 0x000B5734
		protected object GetService(Type serviceType)
		{
			IServiceProvider site = this._designer.Component.Site;
			if (site != null)
			{
				return site.GetService(serviceType);
			}
			return null;
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000B675E File Offset: 0x000B575E
		object IServiceProvider.GetService(Type serviceType)
		{
			return this.GetService(serviceType);
		}

		// Token: 0x040017D1 RID: 6097
		private ControlDesigner _designer;

		// Token: 0x040017D2 RID: 6098
		private string _name;

		// Token: 0x040017D3 RID: 6099
		private IDictionary _properties;
	}
}

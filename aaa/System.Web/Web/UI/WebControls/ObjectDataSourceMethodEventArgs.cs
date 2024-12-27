using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005F8 RID: 1528
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ObjectDataSourceMethodEventArgs : CancelEventArgs
	{
		// Token: 0x06004B9F RID: 19359 RVA: 0x001337BA File Offset: 0x001327BA
		public ObjectDataSourceMethodEventArgs(IOrderedDictionary inputParameters)
		{
			this._inputParameters = inputParameters;
		}

		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x06004BA0 RID: 19360 RVA: 0x001337C9 File Offset: 0x001327C9
		public IOrderedDictionary InputParameters
		{
			get
			{
				return this._inputParameters;
			}
		}

		// Token: 0x04002BAE RID: 11182
		private IOrderedDictionary _inputParameters;
	}
}

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000560 RID: 1376
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DetailsViewInsertEventArgs : CancelEventArgs
	{
		// Token: 0x0600441D RID: 17437 RVA: 0x00119857 File Offset: 0x00118857
		public DetailsViewInsertEventArgs(object commandArgument)
			: base(false)
		{
			this._commandArgument = commandArgument;
		}

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x0600441E RID: 17438 RVA: 0x00119867 File Offset: 0x00118867
		public object CommandArgument
		{
			get
			{
				return this._commandArgument;
			}
		}

		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x0600441F RID: 17439 RVA: 0x0011986F File Offset: 0x0011886F
		public IOrderedDictionary Values
		{
			get
			{
				if (this._values == null)
				{
					this._values = new OrderedDictionary();
				}
				return this._values;
			}
		}

		// Token: 0x0400299A RID: 10650
		private object _commandArgument;

		// Token: 0x0400299B RID: 10651
		private OrderedDictionary _values;
	}
}

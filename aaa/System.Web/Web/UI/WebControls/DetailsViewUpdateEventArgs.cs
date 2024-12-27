using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200056C RID: 1388
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DetailsViewUpdateEventArgs : CancelEventArgs
	{
		// Token: 0x06004453 RID: 17491 RVA: 0x00119A8B File Offset: 0x00118A8B
		public DetailsViewUpdateEventArgs(object commandArgument)
			: base(false)
		{
			this._commandArgument = commandArgument;
		}

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x06004454 RID: 17492 RVA: 0x00119A9B File Offset: 0x00118A9B
		public object CommandArgument
		{
			get
			{
				return this._commandArgument;
			}
		}

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x06004455 RID: 17493 RVA: 0x00119AA3 File Offset: 0x00118AA3
		public IOrderedDictionary Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new OrderedDictionary();
				}
				return this._keys;
			}
		}

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x06004456 RID: 17494 RVA: 0x00119ABE File Offset: 0x00118ABE
		public IOrderedDictionary NewValues
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

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x06004457 RID: 17495 RVA: 0x00119AD9 File Offset: 0x00118AD9
		public IOrderedDictionary OldValues
		{
			get
			{
				if (this._oldValues == null)
				{
					this._oldValues = new OrderedDictionary();
				}
				return this._oldValues;
			}
		}

		// Token: 0x040029AE RID: 10670
		private object _commandArgument;

		// Token: 0x040029AF RID: 10671
		private OrderedDictionary _values;

		// Token: 0x040029B0 RID: 10672
		private OrderedDictionary _keys;

		// Token: 0x040029B1 RID: 10673
		private OrderedDictionary _oldValues;
	}
}

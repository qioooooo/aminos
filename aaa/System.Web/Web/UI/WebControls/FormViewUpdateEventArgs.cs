using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000595 RID: 1429
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormViewUpdateEventArgs : CancelEventArgs
	{
		// Token: 0x06004607 RID: 17927 RVA: 0x0011EBDA File Offset: 0x0011DBDA
		public FormViewUpdateEventArgs(object commandArgument)
			: base(false)
		{
			this._commandArgument = commandArgument;
		}

		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x06004608 RID: 17928 RVA: 0x0011EBEA File Offset: 0x0011DBEA
		public object CommandArgument
		{
			get
			{
				return this._commandArgument;
			}
		}

		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x06004609 RID: 17929 RVA: 0x0011EBF2 File Offset: 0x0011DBF2
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

		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x0600460A RID: 17930 RVA: 0x0011EC0D File Offset: 0x0011DC0D
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

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x0600460B RID: 17931 RVA: 0x0011EC28 File Offset: 0x0011DC28
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

		// Token: 0x04002A2E RID: 10798
		private object _commandArgument;

		// Token: 0x04002A2F RID: 10799
		private OrderedDictionary _values;

		// Token: 0x04002A30 RID: 10800
		private OrderedDictionary _keys;

		// Token: 0x04002A31 RID: 10801
		private OrderedDictionary _oldValues;
	}
}

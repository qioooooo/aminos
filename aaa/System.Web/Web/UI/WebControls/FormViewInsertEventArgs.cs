using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200058A RID: 1418
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormViewInsertEventArgs : CancelEventArgs
	{
		// Token: 0x060045DA RID: 17882 RVA: 0x0011EA27 File Offset: 0x0011DA27
		public FormViewInsertEventArgs(object commandArgument)
			: base(false)
		{
			this._commandArgument = commandArgument;
		}

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x060045DB RID: 17883 RVA: 0x0011EA37 File Offset: 0x0011DA37
		public object CommandArgument
		{
			get
			{
				return this._commandArgument;
			}
		}

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x060045DC RID: 17884 RVA: 0x0011EA3F File Offset: 0x0011DA3F
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

		// Token: 0x04002A1B RID: 10779
		private object _commandArgument;

		// Token: 0x04002A1C RID: 10780
		private OrderedDictionary _values;
	}
}

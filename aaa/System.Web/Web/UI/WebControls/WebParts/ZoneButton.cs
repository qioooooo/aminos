using System;
using System.ComponentModel;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200074D RID: 1869
	[SupportsEventValidation]
	internal sealed class ZoneButton : Button
	{
		// Token: 0x06005AD2 RID: 23250 RVA: 0x0016E79F File Offset: 0x0016D79F
		public ZoneButton(WebZone owner, string eventArgument)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this._owner = owner;
			this._eventArgument = eventArgument;
		}

		// Token: 0x1700178B RID: 6027
		// (get) Token: 0x06005AD3 RID: 23251 RVA: 0x0016E7C3 File Offset: 0x0016D7C3
		// (set) Token: 0x06005AD4 RID: 23252 RVA: 0x0016E7C6 File Offset: 0x0016D7C6
		[DefaultValue(false)]
		public override bool UseSubmitBehavior
		{
			get
			{
				return false;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06005AD5 RID: 23253 RVA: 0x0016E7D0 File Offset: 0x0016D7D0
		protected override PostBackOptions GetPostBackOptions()
		{
			if (!string.IsNullOrEmpty(this._eventArgument) && this._owner.Page != null)
			{
				return new PostBackOptions(this._owner, this._eventArgument)
				{
					ClientSubmit = true
				};
			}
			return base.GetPostBackOptions();
		}

		// Token: 0x040030CA RID: 12490
		private WebZone _owner;

		// Token: 0x040030CB RID: 12491
		private string _eventArgument;
	}
}

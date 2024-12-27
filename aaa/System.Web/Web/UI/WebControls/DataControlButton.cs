using System;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200052B RID: 1323
	[SupportsEventValidation]
	internal sealed class DataControlButton : Button
	{
		// Token: 0x0600412C RID: 16684 RVA: 0x0010EC5C File Offset: 0x0010DC5C
		internal DataControlButton(IPostBackContainer container)
		{
			this._container = container;
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x0600412D RID: 16685 RVA: 0x0010EC6B File Offset: 0x0010DC6B
		// (set) Token: 0x0600412E RID: 16686 RVA: 0x0010EC6E File Offset: 0x0010DC6E
		public override bool CausesValidation
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("CannotSetValidationOnDataControlButtons"));
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x0600412F RID: 16687 RVA: 0x0010EC7F File Offset: 0x0010DC7F
		// (set) Token: 0x06004130 RID: 16688 RVA: 0x0010EC82 File Offset: 0x0010DC82
		public override bool UseSubmitBehavior
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x0010EC8C File Offset: 0x0010DC8C
		protected sealed override PostBackOptions GetPostBackOptions()
		{
			PostBackOptions postBackOptions;
			if (this._container != null)
			{
				postBackOptions = this._container.GetPostBackOptions(this);
				if (this.Page != null)
				{
					postBackOptions.ClientSubmit = true;
				}
			}
			else
			{
				postBackOptions = base.GetPostBackOptions();
			}
			return postBackOptions;
		}

		// Token: 0x0400289A RID: 10394
		private IPostBackContainer _container;
	}
}

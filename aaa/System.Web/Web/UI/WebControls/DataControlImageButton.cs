using System;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000533 RID: 1331
	[SupportsEventValidation]
	internal sealed class DataControlImageButton : ImageButton
	{
		// Token: 0x0600418D RID: 16781 RVA: 0x0010F999 File Offset: 0x0010E999
		internal DataControlImageButton(IPostBackContainer container)
		{
			this._container = container;
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x0600418E RID: 16782 RVA: 0x0010F9A8 File Offset: 0x0010E9A8
		// (set) Token: 0x0600418F RID: 16783 RVA: 0x0010F9AB File Offset: 0x0010E9AB
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

		// Token: 0x06004190 RID: 16784 RVA: 0x0010F9BC File Offset: 0x0010E9BC
		internal void EnableCallback(string argument)
		{
			this._enableCallback = true;
			this._callbackArgument = argument;
		}

		// Token: 0x06004191 RID: 16785 RVA: 0x0010F9CC File Offset: 0x0010E9CC
		protected sealed override PostBackOptions GetPostBackOptions()
		{
			if (this._container != null)
			{
				return this._container.GetPostBackOptions(this);
			}
			return base.GetPostBackOptions();
		}

		// Token: 0x06004192 RID: 16786 RVA: 0x0010F9E9 File Offset: 0x0010E9E9
		protected internal override void Render(HtmlTextWriter writer)
		{
			this.SetCallbackProperties();
			base.Render(writer);
		}

		// Token: 0x06004193 RID: 16787 RVA: 0x0010F9F8 File Offset: 0x0010E9F8
		private void SetCallbackProperties()
		{
			if (this._enableCallback)
			{
				ICallbackContainer callbackContainer = this._container as ICallbackContainer;
				if (callbackContainer != null)
				{
					string callbackScript = callbackContainer.GetCallbackScript(this, this._callbackArgument);
					if (!string.IsNullOrEmpty(callbackScript))
					{
						this.OnClientClick = callbackScript;
					}
				}
			}
		}

		// Token: 0x040028B5 RID: 10421
		private IPostBackContainer _container;

		// Token: 0x040028B6 RID: 10422
		private string _callbackArgument;

		// Token: 0x040028B7 RID: 10423
		private bool _enableCallback;
	}
}

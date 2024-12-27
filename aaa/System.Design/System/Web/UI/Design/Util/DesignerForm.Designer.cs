namespace System.Web.UI.Design.Util
{
	// Token: 0x02000327 RID: 807
	internal abstract partial class DesignerForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06001E44 RID: 7748 RVA: 0x000AC03D File Offset: 0x000AB03D
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._serviceProvider = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x04001738 RID: 5944
		private global::System.IServiceProvider _serviceProvider;
	}
}

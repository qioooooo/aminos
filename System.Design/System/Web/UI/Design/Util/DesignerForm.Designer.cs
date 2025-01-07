namespace System.Web.UI.Design.Util
{
	internal abstract partial class DesignerForm : global::System.Windows.Forms.Form
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._serviceProvider = null;
			}
			base.Dispose(disposing);
		}

		private global::System.IServiceProvider _serviceProvider;
	}
}

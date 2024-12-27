using System;
using System.Windows.Forms;

namespace System.Web.UI.Design
{
	// Token: 0x02000386 RID: 902
	public interface IWebFormsBuilderUIService
	{
		// Token: 0x0600215A RID: 8538
		string BuildColor(Control owner, string initialColor);

		// Token: 0x0600215B RID: 8539
		string BuildUrl(Control owner, string initialUrl, string baseUrl, string caption, string filter, UrlBuilderOptions options);
	}
}

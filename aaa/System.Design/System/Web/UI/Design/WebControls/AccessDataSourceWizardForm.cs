using System;
using System.ComponentModel.Design.Data;
using System.Drawing;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003DA RID: 986
	internal partial class AccessDataSourceWizardForm : SqlDataSourceWizardForm
	{
		// Token: 0x06002473 RID: 9331 RVA: 0x000C2ED2 File Offset: 0x000C1ED2
		public AccessDataSourceWizardForm(IServiceProvider serviceProvider, AccessDataSourceDesigner accessDataSourceDesigner, IDataEnvironment dataEnvironment)
			: base(serviceProvider, accessDataSourceDesigner, dataEnvironment)
		{
			base.Glyph = new Bitmap(typeof(AccessDataSourceWizardForm), "datasourcewizard.bmp");
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x000C2EF7 File Offset: 0x000C1EF7
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.AccessDataSource.ConfigureDataSource";
			}
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000C2F00 File Offset: 0x000C1F00
		protected override SqlDataSourceConnectionPanel CreateConnectionPanel()
		{
			AccessDataSourceDesigner accessDataSourceDesigner = (AccessDataSourceDesigner)base.SqlDataSourceDesigner;
			AccessDataSource accessDataSource = (AccessDataSource)accessDataSourceDesigner.Component;
			return new AccessDataSourceConnectionChooserPanel(accessDataSourceDesigner, accessDataSource);
		}
	}
}

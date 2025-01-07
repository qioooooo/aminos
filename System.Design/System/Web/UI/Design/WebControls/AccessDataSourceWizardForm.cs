using System;
using System.ComponentModel.Design.Data;
using System.Drawing;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal partial class AccessDataSourceWizardForm : SqlDataSourceWizardForm
	{
		public AccessDataSourceWizardForm(IServiceProvider serviceProvider, AccessDataSourceDesigner accessDataSourceDesigner, IDataEnvironment dataEnvironment)
			: base(serviceProvider, accessDataSourceDesigner, dataEnvironment)
		{
			base.Glyph = new Bitmap(typeof(AccessDataSourceWizardForm), "datasourcewizard.bmp");
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.AccessDataSource.ConfigureDataSource";
			}
		}

		protected override SqlDataSourceConnectionPanel CreateConnectionPanel()
		{
			AccessDataSourceDesigner accessDataSourceDesigner = (AccessDataSourceDesigner)base.SqlDataSourceDesigner;
			AccessDataSource accessDataSource = (AccessDataSource)accessDataSourceDesigner.Component;
			return new AccessDataSourceConnectionChooserPanel(accessDataSourceDesigner, accessDataSource);
		}
	}
}

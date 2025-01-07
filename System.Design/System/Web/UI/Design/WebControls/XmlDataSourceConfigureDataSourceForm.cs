using System;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal sealed partial class XmlDataSourceConfigureDataSourceForm : DesignerForm
	{
		public XmlDataSourceConfigureDataSourceForm(IServiceProvider serviceProvider, XmlDataSource xmlDataSource)
			: base(serviceProvider)
		{
			this._xmlDataSource = xmlDataSource;
			this.InitializeComponent();
			this.InitializeUI();
			this.DataFile = this._xmlDataSource.DataFile;
			this.TransformFile = this._xmlDataSource.TransformFile;
			this.XPath = this._xmlDataSource.XPath;
		}

		private string DataFile
		{
			get
			{
				return this._dataFileTextBox.Text;
			}
			set
			{
				this._dataFileTextBox.Text = value;
				this._dataFileTextBox.Select(0, 0);
			}
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.XmlDataSource.ConfigureDataSource";
			}
		}

		private string TransformFile
		{
			get
			{
				return this._transformFileTextBox.Text;
			}
			set
			{
				this._transformFileTextBox.Text = value;
				this._transformFileTextBox.Select(0, 0);
			}
		}

		private string XPath
		{
			get
			{
				return this._xpathExpressionTextBox.Text;
			}
			set
			{
				this._xpathExpressionTextBox.Text = value;
				this._xpathExpressionTextBox.Select(0, 0);
			}
		}

		private void InitializeUI()
		{
			this._dataFileLabel.Text = SR.GetString("XmlDataSourceConfigureDataSourceForm_DataFileLabel");
			this._transformFileLabel.Text = SR.GetString("XmlDataSourceConfigureDataSourceForm_TransformFileLabel");
			this._xpathExpressionLabel.Text = SR.GetString("XmlDataSourceConfigureDataSourceForm_XPathExpressionLabel");
			this._transformFileHelpLabel.Text = SR.GetString("XmlDataSourceConfigureDataSourceForm_TransformFileHelpLabel");
			this._xpathExpressionHelpLabel.Text = SR.GetString("XmlDataSourceConfigureDataSourceForm_XPathExpressionHelpLabel");
			this._chooseDataFileButton.Text = SR.GetString("XmlDataSourceConfigureDataSourceForm_Browse");
			this._chooseTransformFileButton.Text = SR.GetString("XmlDataSourceConfigureDataSourceForm_Browse");
			this._helpLabel.Text = SR.GetString("XmlDataSourceConfigureDataSourceForm_HelpLabel");
			this._okButton.Text = SR.GetString("OK");
			this._cancelButton.Text = SR.GetString("Cancel");
			this._chooseDataFileButton.AccessibleDescription = SR.GetString("XmlDataFileEditor_Ellipses");
			this._chooseTransformFileButton.AccessibleDescription = SR.GetString("XslTransformFileEditor_Ellipses");
			this.Text = SR.GetString("ConfigureDataSource_Title", new object[] { this._xmlDataSource.ID });
		}

		private void OnChooseDataFileButtonClick(object sender, EventArgs e)
		{
			string text = UrlBuilder.BuildUrl(this._xmlDataSource, this, this.DataFile, SR.GetString("XmlDataFileEditor_Caption"), SR.GetString("XmlDataFileEditor_Filter"));
			if (text != null)
			{
				this.DataFile = text;
			}
		}

		private void OnChooseTransformFileButtonClick(object sender, EventArgs e)
		{
			string text = UrlBuilder.BuildUrl(this._xmlDataSource, this, this.TransformFile, SR.GetString("XslTransformFileEditor_Caption"), SR.GetString("XslTransformFileEditor_Filter"));
			if (text != null)
			{
				this.TransformFile = text;
			}
		}

		private void OnOkButtonClick(object sender, EventArgs e)
		{
			if (this._xmlDataSource.DataFile != this.DataFile)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._xmlDataSource)["DataFile"];
				propertyDescriptor.ResetValue(this._xmlDataSource);
				propertyDescriptor.SetValue(this._xmlDataSource, this.DataFile);
			}
			if (this._xmlDataSource.TransformFile != this.TransformFile)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._xmlDataSource)["TransformFile"];
				propertyDescriptor.ResetValue(this._xmlDataSource);
				propertyDescriptor.SetValue(this._xmlDataSource, this.TransformFile);
			}
			if (this._xmlDataSource.XPath != this.XPath)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._xmlDataSource)["XPath"];
				propertyDescriptor.ResetValue(this._xmlDataSource);
				propertyDescriptor.SetValue(this._xmlDataSource, this.XPath);
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		private XmlDataSource _xmlDataSource;
	}
}

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006CA RID: 1738
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ImportCatalogPart : CatalogPart
	{
		// Token: 0x17001600 RID: 5632
		// (get) Token: 0x06005581 RID: 21889 RVA: 0x0015A57C File Offset: 0x0015957C
		// (set) Token: 0x06005582 RID: 21890 RVA: 0x0015A5AE File Offset: 0x001595AE
		[WebSysDefaultValue("ImportCatalogPart_Browse")]
		[WebSysDescription("ImportCatalogPart_BrowseHelpText")]
		[WebCategory("Appearance")]
		public string BrowseHelpText
		{
			get
			{
				object obj = this.ViewState["BrowseHelpText"];
				if (obj == null)
				{
					return SR.GetString("ImportCatalogPart_Browse");
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["BrowseHelpText"] = value;
			}
		}

		// Token: 0x17001601 RID: 5633
		// (get) Token: 0x06005583 RID: 21891 RVA: 0x0015A5C1 File Offset: 0x001595C1
		// (set) Token: 0x06005584 RID: 21892 RVA: 0x0015A5C9 File Offset: 0x001595C9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Themeable(false)]
		public override string DefaultButton
		{
			get
			{
				return base.DefaultButton;
			}
			set
			{
				base.DefaultButton = value;
			}
		}

		// Token: 0x17001602 RID: 5634
		// (get) Token: 0x06005585 RID: 21893 RVA: 0x0015A5D4 File Offset: 0x001595D4
		// (set) Token: 0x06005586 RID: 21894 RVA: 0x0015A606 File Offset: 0x00159606
		[WebSysDefaultValue("ImportCatalogPart_ImportedPartLabel")]
		[WebCategory("Appearance")]
		[WebSysDescription("ImportCatalogPart_ImportedPartLabelText")]
		public string ImportedPartLabelText
		{
			get
			{
				object obj = this.ViewState["ImportedPartLabelText"];
				if (obj == null)
				{
					return SR.GetString("ImportCatalogPart_ImportedPartLabel");
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["ImportedPartLabelText"] = value;
			}
		}

		// Token: 0x17001603 RID: 5635
		// (get) Token: 0x06005587 RID: 21895 RVA: 0x0015A61C File Offset: 0x0015961C
		// (set) Token: 0x06005588 RID: 21896 RVA: 0x0015A64E File Offset: 0x0015964E
		[WebCategory("Appearance")]
		[WebSysDefaultValue("ImportCatalogPart_ImportedPartErrorLabel")]
		[WebSysDescription("ImportCatalogPart_PartImportErrorLabelText")]
		public string PartImportErrorLabelText
		{
			get
			{
				object obj = this.ViewState["PartImportErrorLabelText"];
				if (obj == null)
				{
					return SR.GetString("ImportCatalogPart_ImportedPartErrorLabel");
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["PartImportErrorLabelText"] = value;
			}
		}

		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x06005589 RID: 21897 RVA: 0x0015A664 File Offset: 0x00159664
		// (set) Token: 0x0600558A RID: 21898 RVA: 0x0015A696 File Offset: 0x00159696
		[WebSysDefaultValue("ImportCatalogPart_PartTitle")]
		public override string Title
		{
			get
			{
				string text = (string)this.ViewState["Title"];
				if (text == null)
				{
					return SR.GetString("ImportCatalogPart_PartTitle");
				}
				return text;
			}
			set
			{
				this.ViewState["Title"] = value;
			}
		}

		// Token: 0x17001605 RID: 5637
		// (get) Token: 0x0600558B RID: 21899 RVA: 0x0015A6AC File Offset: 0x001596AC
		// (set) Token: 0x0600558C RID: 21900 RVA: 0x0015A6DE File Offset: 0x001596DE
		[WebCategory("Appearance")]
		[WebSysDescription("ImportCatalogPart_UploadButtonText")]
		[WebSysDefaultValue("ImportCatalogPart_UploadButton")]
		public string UploadButtonText
		{
			get
			{
				object obj = this.ViewState["UploadButtonText"];
				if (obj == null)
				{
					return SR.GetString("ImportCatalogPart_UploadButton");
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["UploadButtonText"] = value;
			}
		}

		// Token: 0x17001606 RID: 5638
		// (get) Token: 0x0600558D RID: 21901 RVA: 0x0015A6F4 File Offset: 0x001596F4
		// (set) Token: 0x0600558E RID: 21902 RVA: 0x0015A726 File Offset: 0x00159726
		[WebSysDescription("ImportCatalogPart_UploadHelpText")]
		[WebSysDefaultValue("ImportCatalogPart_Upload")]
		[WebCategory("Appearance")]
		public string UploadHelpText
		{
			get
			{
				object obj = this.ViewState["UploadHelpText"];
				if (obj == null)
				{
					return SR.GetString("ImportCatalogPart_Upload");
				}
				return (string)obj;
			}
			set
			{
				this.ViewState["UploadHelpText"] = value;
			}
		}

		// Token: 0x0600558F RID: 21903 RVA: 0x0015A73C File Offset: 0x0015973C
		protected internal override void CreateChildControls()
		{
			this.Controls.Clear();
			this._upload = new FileUpload();
			this.Controls.Add(this._upload);
			this._uploadButton = new Button();
			this._uploadButton.ID = "Upload";
			this._uploadButton.CommandName = "upload";
			this._uploadButton.Click += this.OnUpload;
			this.Controls.Add(this._uploadButton);
			if (!base.DesignMode && this.Page != null)
			{
				IScriptManager scriptManager = this.Page.ScriptManager;
				if (scriptManager != null)
				{
					scriptManager.RegisterPostBackControl(this._uploadButton);
				}
			}
		}

		// Token: 0x06005590 RID: 21904 RVA: 0x0015A7EE File Offset: 0x001597EE
		public override WebPartDescriptionCollection GetAvailableWebPartDescriptions()
		{
			if (base.DesignMode)
			{
				return ImportCatalogPart.DesignModeAvailableWebPart;
			}
			this.CreateAvailableWebPartDescriptions();
			return this._availableWebPartDescriptions;
		}

		// Token: 0x06005591 RID: 21905 RVA: 0x0015A80C File Offset: 0x0015980C
		private void CreateAvailableWebPartDescriptions()
		{
			if (this._availableWebPartDescriptions != null)
			{
				return;
			}
			if (base.WebPartManager == null || string.IsNullOrEmpty(this._importedPartDescription))
			{
				this._availableWebPartDescriptions = new WebPartDescriptionCollection();
				return;
			}
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			permissionSet.AddPermission(new AspNetHostingPermission(AspNetHostingPermissionLevel.Minimal));
			permissionSet.PermitOnly();
			bool flag = true;
			string text = null;
			string text2 = null;
			string text3 = null;
			try
			{
				try
				{
					using (StringReader stringReader = new StringReader(this._importedPartDescription))
					{
						using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
						{
							if (xmlTextReader != null)
							{
								xmlTextReader.MoveToContent();
								xmlTextReader.MoveToContent();
								xmlTextReader.ReadStartElement("webParts");
								xmlTextReader.ReadStartElement("webPart");
								xmlTextReader.ReadStartElement("metaData");
								string text4 = null;
								string text5 = null;
								while (xmlTextReader.Name != "type")
								{
									xmlTextReader.Skip();
									if (xmlTextReader.EOF)
									{
										throw new EndOfStreamException();
									}
								}
								if (xmlTextReader.Name == "type")
								{
									text4 = xmlTextReader.GetAttribute("name");
									text5 = xmlTextReader.GetAttribute("src");
								}
								bool flag2 = base.WebPartManager.Personalization.Scope == PersonalizationScope.Shared;
								if (!string.IsNullOrEmpty(text4))
								{
									PermissionSet permissionSet2 = new PermissionSet(PermissionState.None);
									permissionSet2.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
									permissionSet2.AddPermission(new AspNetHostingPermission(AspNetHostingPermissionLevel.Medium));
									CodeAccessPermission.RevertPermitOnly();
									flag = false;
									permissionSet2.PermitOnly();
									flag = true;
									Type type = WebPartUtil.DeserializeType(text4, true);
									CodeAccessPermission.RevertPermitOnly();
									flag = false;
									permissionSet.PermitOnly();
									flag = true;
									if (!base.WebPartManager.IsAuthorized(type, null, null, flag2))
									{
										this._importErrorMessage = SR.GetString("WebPartManager_ForbiddenType");
										return;
									}
									if (!type.IsSubclassOf(typeof(WebPart)) && !type.IsSubclassOf(typeof(Control)))
									{
										this._importErrorMessage = SR.GetString("WebPartManager_TypeMustDeriveFromControl");
										return;
									}
								}
								else if (!base.WebPartManager.IsAuthorized(typeof(UserControl), text5, null, flag2))
								{
									this._importErrorMessage = SR.GetString("WebPartManager_ForbiddenType");
									return;
								}
								while (!xmlTextReader.EOF)
								{
									while (!xmlTextReader.EOF && (xmlTextReader.NodeType != XmlNodeType.Element || !(xmlTextReader.Name == "property")))
									{
										xmlTextReader.Read();
									}
									if (xmlTextReader.EOF)
									{
										break;
									}
									string attribute = xmlTextReader.GetAttribute("name");
									if (attribute == "Title")
									{
										text = xmlTextReader.ReadElementString();
									}
									else if (attribute == "Description")
									{
										text2 = xmlTextReader.ReadElementString();
									}
									else
									{
										if (!(attribute == "CatalogIconImageUrl"))
										{
											xmlTextReader.Read();
											continue;
										}
										string text6 = xmlTextReader.ReadElementString().Trim();
										if (!CrossSiteScriptingValidation.IsDangerousUrl(text6))
										{
											text3 = text6;
										}
									}
									if (text != null && text2 != null && text3 != null)
									{
										break;
									}
									xmlTextReader.Read();
								}
							}
						}
						if (string.IsNullOrEmpty(text))
						{
							text = SR.GetString("Part_Untitled");
						}
						this._availableWebPartDescriptions = new WebPartDescriptionCollection(new WebPartDescription[]
						{
							new WebPartDescription("ImportedWebPart", text, text2, text3)
						});
					}
				}
				catch (XmlException)
				{
					this._importErrorMessage = SR.GetString("WebPartManager_ImportInvalidFormat");
				}
				catch
				{
					this._importErrorMessage = ((!string.IsNullOrEmpty(this._importErrorMessage)) ? this._importErrorMessage : SR.GetString("WebPart_DefaultImportErrorMessage"));
				}
				finally
				{
					if (flag)
					{
						CodeAccessPermission.RevertPermitOnly();
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x06005592 RID: 21906 RVA: 0x0015AC3C File Offset: 0x00159C3C
		public override WebPart GetWebPart(WebPartDescription description)
		{
			if (description == null)
			{
				throw new ArgumentNullException("description");
			}
			WebPartDescriptionCollection availableWebPartDescriptions = this.GetAvailableWebPartDescriptions();
			if (!availableWebPartDescriptions.Contains(description))
			{
				throw new ArgumentException(SR.GetString("CatalogPart_UnknownDescription"), "description");
			}
			if (this._availableWebPart != null)
			{
				return this._availableWebPart;
			}
			using (XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this._importedPartDescription)))
			{
				if (xmlTextReader != null && base.WebPartManager != null)
				{
					this._availableWebPart = base.WebPartManager.ImportWebPart(xmlTextReader, out this._importErrorMessage);
				}
			}
			if (this._availableWebPart == null)
			{
				this._importedPartDescription = null;
				this._availableWebPartDescriptions = null;
			}
			return this._availableWebPart;
		}

		// Token: 0x06005593 RID: 21907 RVA: 0x0015ACF8 File Offset: 0x00159CF8
		protected internal override void LoadControlState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadControlState(null);
				return;
			}
			object[] array = (object[])savedState;
			if (array.Length != 2)
			{
				throw new ArgumentException(SR.GetString("Invalid_ControlState"));
			}
			base.LoadControlState(array[0]);
			if (array[1] != null)
			{
				this._importedPartDescription = (string)array[1];
				this.GetAvailableWebPartDescriptions();
			}
		}

		// Token: 0x06005594 RID: 21908 RVA: 0x0015AD50 File Offset: 0x00159D50
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Page.RegisterRequiresControlState(this);
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x0015AD68 File Offset: 0x00159D68
		internal void OnUpload(object sender, EventArgs e)
		{
			string fileName = this._upload.FileName;
			Stream fileContent = this._upload.FileContent;
			if (!string.IsNullOrEmpty(fileName) && fileContent != null)
			{
				using (StreamReader streamReader = new StreamReader(fileContent, true))
				{
					this._importedPartDescription = streamReader.ReadToEnd();
					this._availableWebPart = null;
					this._availableWebPartDescriptions = null;
					this._importErrorMessage = null;
					if (string.IsNullOrEmpty(this._importedPartDescription))
					{
						this._importErrorMessage = SR.GetString("ImportCatalogPart_NoFileName");
					}
					else
					{
						this.GetAvailableWebPartDescriptions();
					}
					return;
				}
			}
			this._importErrorMessage = SR.GetString("ImportCatalogPart_NoFileName");
		}

		// Token: 0x06005596 RID: 21910 RVA: 0x0015AE14 File Offset: 0x00159E14
		protected internal override object SaveControlState()
		{
			object[] array = new object[]
			{
				base.SaveControlState(),
				this._importedPartDescription
			};
			for (int i = 0; i < 2; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x0015AE4E File Offset: 0x00159E4E
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			base.Render(writer);
		}

		// Token: 0x06005598 RID: 21912 RVA: 0x0015AE6C File Offset: 0x00159E6C
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			this.EnsureChildControls();
			CatalogZoneBase zone = base.Zone;
			if (zone != null && !zone.LabelStyle.IsEmpty)
			{
				zone.LabelStyle.AddAttributesToRender(writer, this);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.For, this._upload.ClientID);
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			writer.Write(this.BrowseHelpText);
			writer.RenderEndTag();
			writer.WriteBreak();
			if (zone != null && !zone.EditUIStyle.IsEmpty)
			{
				this._upload.ApplyStyle(zone.EditUIStyle);
			}
			this._upload.RenderControl(writer);
			writer.WriteBreak();
			if (zone != null && !zone.LabelStyle.IsEmpty)
			{
				zone.LabelStyle.AddAttributesToRender(writer, this);
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			writer.Write(this.UploadHelpText);
			writer.RenderEndTag();
			writer.WriteBreak();
			if (zone != null && !zone.EditUIStyle.IsEmpty)
			{
				this._uploadButton.ApplyStyle(zone.EditUIStyle);
			}
			this._uploadButton.Text = this.UploadButtonText;
			this._uploadButton.RenderControl(writer);
			if (this._importedPartDescription != null || this._importErrorMessage != null || base.DesignMode)
			{
				writer.WriteBreak();
				if (this._importErrorMessage != null)
				{
					if (zone != null && !zone.ErrorStyle.IsEmpty)
					{
						zone.ErrorStyle.AddAttributesToRender(writer, this);
					}
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
					writer.Write(this.PartImportErrorLabelText);
					writer.RenderEndTag();
					writer.RenderBeginTag(HtmlTextWriterTag.Hr);
					writer.RenderEndTag();
					if (zone != null && !zone.ErrorStyle.IsEmpty)
					{
						zone.ErrorStyle.AddAttributesToRender(writer, this);
					}
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
					writer.WriteEncodedText(this._importErrorMessage);
					writer.RenderEndTag();
					return;
				}
				if (zone != null && !zone.LabelStyle.IsEmpty)
				{
					zone.LabelStyle.AddAttributesToRender(writer, this);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Span);
				writer.Write(this.ImportedPartLabelText);
				writer.RenderEndTag();
				writer.RenderBeginTag(HtmlTextWriterTag.Hr);
				writer.RenderEndTag();
			}
		}

		// Token: 0x04002F1F RID: 12063
		private const int baseIndex = 0;

		// Token: 0x04002F20 RID: 12064
		private const int importedPartDescriptionIndex = 1;

		// Token: 0x04002F21 RID: 12065
		private const int controlStateArrayLength = 2;

		// Token: 0x04002F22 RID: 12066
		private const string TitlePropertyName = "Title";

		// Token: 0x04002F23 RID: 12067
		private const string DescriptionPropertyName = "Description";

		// Token: 0x04002F24 RID: 12068
		private const string IconPropertyName = "CatalogIconImageUrl";

		// Token: 0x04002F25 RID: 12069
		private const string ImportedWebPartID = "ImportedWebPart";

		// Token: 0x04002F26 RID: 12070
		private WebPart _availableWebPart;

		// Token: 0x04002F27 RID: 12071
		private string _importedPartDescription;

		// Token: 0x04002F28 RID: 12072
		private WebPartDescriptionCollection _availableWebPartDescriptions;

		// Token: 0x04002F29 RID: 12073
		private FileUpload _upload;

		// Token: 0x04002F2A RID: 12074
		private Button _uploadButton;

		// Token: 0x04002F2B RID: 12075
		private string _importErrorMessage;

		// Token: 0x04002F2C RID: 12076
		private static readonly WebPartDescriptionCollection DesignModeAvailableWebPart = new WebPartDescriptionCollection(new WebPartDescription[]
		{
			new WebPartDescription("webpart1", string.Format(CultureInfo.CurrentCulture, SR.GetString("CatalogPart_SampleWebPartTitle"), new object[] { "1" }), null, null)
		});
	}
}

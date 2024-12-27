using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006E9 RID: 1769
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PropertyGridEditorPart : EditorPart
	{
		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x060056A7 RID: 22183 RVA: 0x0015DA4D File Offset: 0x0015CA4D
		// (set) Token: 0x060056A8 RID: 22184 RVA: 0x0015DA55 File Offset: 0x0015CA55
		[Themeable(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x060056A9 RID: 22185 RVA: 0x0015DA60 File Offset: 0x0015CA60
		public override bool Display
		{
			get
			{
				if (!base.Display)
				{
					return false;
				}
				object editableObject = this.GetEditableObject();
				return editableObject != null && this.GetEditableProperties(editableObject, false).Count > 0;
			}
		}

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x060056AA RID: 22186 RVA: 0x0015DA94 File Offset: 0x0015CA94
		private ArrayList EditorControls
		{
			get
			{
				if (this._editorControls == null)
				{
					this._editorControls = new ArrayList();
				}
				return this._editorControls;
			}
		}

		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x060056AB RID: 22187 RVA: 0x0015DAB0 File Offset: 0x0015CAB0
		private bool HasError
		{
			get
			{
				foreach (string text in this._errorMessages)
				{
					if (text != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x060056AC RID: 22188 RVA: 0x0015DAE0 File Offset: 0x0015CAE0
		// (set) Token: 0x060056AD RID: 22189 RVA: 0x0015DB12 File Offset: 0x0015CB12
		[WebSysDefaultValue("PropertyGridEditorPart_PartTitle")]
		public override string Title
		{
			get
			{
				string text = (string)this.ViewState["Title"];
				if (text == null)
				{
					return SR.GetString("PropertyGridEditorPart_PartTitle");
				}
				return text;
			}
			set
			{
				this.ViewState["Title"] = value;
			}
		}

		// Token: 0x060056AE RID: 22190 RVA: 0x0015DB28 File Offset: 0x0015CB28
		public override bool ApplyChanges()
		{
			object editableObject = this.GetEditableObject();
			if (editableObject == null)
			{
				return true;
			}
			this.EnsureChildControls();
			if (this.Controls.Count == 0)
			{
				return true;
			}
			PropertyDescriptorCollection editableProperties = this.GetEditableProperties(editableObject, true);
			for (int i = 0; i < editableProperties.Count; i++)
			{
				PropertyDescriptor propertyDescriptor = editableProperties[i];
				Control control = (Control)this.EditorControls[i];
				try
				{
					object editorControlValue = this.GetEditorControlValue(control, propertyDescriptor);
					if (propertyDescriptor.Attributes.Matches(PropertyGridEditorPart.urlPropertyAttribute) && CrossSiteScriptingValidation.IsDangerousUrl(editorControlValue.ToString()))
					{
						this._errorMessages[i] = SR.GetString("EditorPart_ErrorBadUrl");
					}
					else
					{
						try
						{
							propertyDescriptor.SetValue(editableObject, editorControlValue);
						}
						catch (Exception ex)
						{
							this._errorMessages[i] = base.CreateErrorMessage(ex.Message);
						}
					}
				}
				catch
				{
					if (this.Context != null && this.Context.IsCustomErrorEnabled)
					{
						this._errorMessages[i] = SR.GetString("EditorPart_ErrorConvertingProperty");
					}
					else
					{
						this._errorMessages[i] = SR.GetString("EditorPart_ErrorConvertingPropertyWithType", new object[] { propertyDescriptor.PropertyType.FullName });
					}
				}
			}
			return !this.HasError;
		}

		// Token: 0x060056AF RID: 22191 RVA: 0x0015DC7C File Offset: 0x0015CC7C
		private bool CanEditProperty(PropertyDescriptor property)
		{
			if (property.IsReadOnly)
			{
				return false;
			}
			if (base.WebPartManager != null && base.WebPartManager.Personalization != null && base.WebPartManager.Personalization.Scope == PersonalizationScope.User)
			{
				AttributeCollection attributes = property.Attributes;
				if (attributes.Contains(PersonalizableAttribute.SharedPersonalizable))
				{
					return false;
				}
			}
			return Util.CanConvertToFrom(property.Converter, typeof(string));
		}

		// Token: 0x060056B0 RID: 22192 RVA: 0x0015DCE8 File Offset: 0x0015CCE8
		protected internal override void CreateChildControls()
		{
			ControlCollection controls = this.Controls;
			controls.Clear();
			this.EditorControls.Clear();
			object editableObject = this.GetEditableObject();
			if (editableObject != null)
			{
				foreach (object obj in this.GetEditableProperties(editableObject, true))
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
					Control control = this.CreateEditorControl(propertyDescriptor);
					this.EditorControls.Add(control);
					this.Controls.Add(control);
				}
				this._errorMessages = new string[this.EditorControls.Count];
			}
			foreach (object obj2 in controls)
			{
				Control control2 = (Control)obj2;
				control2.EnableViewState = false;
			}
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x0015DDE8 File Offset: 0x0015CDE8
		private Control CreateEditorControl(PropertyDescriptor pd)
		{
			Type propertyType = pd.PropertyType;
			if (propertyType == typeof(bool))
			{
				return new CheckBox();
			}
			if (typeof(Enum).IsAssignableFrom(propertyType))
			{
				DropDownList dropDownList = new DropDownList();
				ICollection standardValues = pd.Converter.GetStandardValues();
				foreach (object obj in standardValues)
				{
					string text = pd.Converter.ConvertToString(obj);
					dropDownList.Items.Add(new ListItem(text));
				}
				return dropDownList;
			}
			return new TextBox
			{
				Columns = 30
			};
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x0015DEAC File Offset: 0x0015CEAC
		private string GetDescription(PropertyDescriptor pd)
		{
			WebDescriptionAttribute webDescriptionAttribute = (WebDescriptionAttribute)pd.Attributes[typeof(WebDescriptionAttribute)];
			if (webDescriptionAttribute != null)
			{
				return webDescriptionAttribute.Description;
			}
			return null;
		}

		// Token: 0x060056B3 RID: 22195 RVA: 0x0015DEE0 File Offset: 0x0015CEE0
		private string GetDisplayName(PropertyDescriptor pd)
		{
			WebDisplayNameAttribute webDisplayNameAttribute = (WebDisplayNameAttribute)pd.Attributes[typeof(WebDisplayNameAttribute)];
			if (webDisplayNameAttribute != null && !string.IsNullOrEmpty(webDisplayNameAttribute.DisplayName))
			{
				return webDisplayNameAttribute.DisplayName;
			}
			return pd.Name;
		}

		// Token: 0x060056B4 RID: 22196 RVA: 0x0015DF28 File Offset: 0x0015CF28
		private object GetEditableObject()
		{
			if (base.DesignMode)
			{
				return PropertyGridEditorPart.designModeWebPart;
			}
			WebPart webPartToEdit = base.WebPartToEdit;
			IWebEditable webEditable = webPartToEdit;
			if (webEditable != null)
			{
				return webEditable.WebBrowsableObject;
			}
			return webPartToEdit;
		}

		// Token: 0x060056B5 RID: 22197 RVA: 0x0015DF58 File Offset: 0x0015CF58
		private PropertyDescriptorCollection GetEditableProperties(object editableObject, bool sort)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(editableObject, PropertyGridEditorPart.FilterAttributes);
			if (sort)
			{
				propertyDescriptorCollection = propertyDescriptorCollection.Sort();
			}
			PropertyDescriptorCollection propertyDescriptorCollection2 = new PropertyDescriptorCollection(null);
			foreach (object obj in propertyDescriptorCollection)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				if (this.CanEditProperty(propertyDescriptor))
				{
					propertyDescriptorCollection2.Add(propertyDescriptor);
				}
			}
			return propertyDescriptorCollection2;
		}

		// Token: 0x060056B6 RID: 22198 RVA: 0x0015DFD8 File Offset: 0x0015CFD8
		private object GetEditorControlValue(Control editorControl, PropertyDescriptor pd)
		{
			CheckBox checkBox = editorControl as CheckBox;
			if (checkBox != null)
			{
				return checkBox.Checked;
			}
			DropDownList dropDownList = editorControl as DropDownList;
			if (dropDownList != null)
			{
				string selectedValue = dropDownList.SelectedValue;
				return pd.Converter.ConvertFromString(selectedValue);
			}
			TextBox textBox = (TextBox)editorControl;
			return pd.Converter.ConvertFromString(textBox.Text);
		}

		// Token: 0x060056B7 RID: 22199 RVA: 0x0015E031 File Offset: 0x0015D031
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Display && this.Visible && !this.HasError)
			{
				this.SyncChanges();
			}
		}

		// Token: 0x060056B8 RID: 22200 RVA: 0x0015E058 File Offset: 0x0015D058
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			this.EnsureChildControls();
			string[] array = null;
			string[] array2 = null;
			object editableObject = this.GetEditableObject();
			if (editableObject != null)
			{
				PropertyDescriptorCollection editableProperties = this.GetEditableProperties(editableObject, true);
				array = new string[editableProperties.Count];
				array2 = new string[editableProperties.Count];
				for (int i = 0; i < editableProperties.Count; i++)
				{
					array[i] = this.GetDisplayName(editableProperties[i]);
					array2[i] = this.GetDescription(editableProperties[i]);
				}
			}
			if (array != null)
			{
				WebControl[] array3 = (WebControl[])this.EditorControls.ToArray(typeof(WebControl));
				base.RenderPropertyEditors(writer, array, array2, array3, this._errorMessages);
			}
		}

		// Token: 0x060056B9 RID: 22201 RVA: 0x0015E118 File Offset: 0x0015D118
		public override void SyncChanges()
		{
			object editableObject = this.GetEditableObject();
			if (editableObject != null)
			{
				this.EnsureChildControls();
				int num = 0;
				foreach (object obj in this.GetEditableProperties(editableObject, true))
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
					if (this.CanEditProperty(propertyDescriptor))
					{
						Control control = (Control)this.EditorControls[num];
						this.SyncChanges(control, propertyDescriptor, editableObject);
						num++;
					}
				}
			}
		}

		// Token: 0x060056BA RID: 22202 RVA: 0x0015E1B0 File Offset: 0x0015D1B0
		private void SyncChanges(Control control, PropertyDescriptor pd, object instance)
		{
			Type propertyType = pd.PropertyType;
			if (propertyType == typeof(bool))
			{
				CheckBox checkBox = (CheckBox)control;
				checkBox.Checked = (bool)pd.GetValue(instance);
				return;
			}
			if (typeof(Enum).IsAssignableFrom(propertyType))
			{
				DropDownList dropDownList = (DropDownList)control;
				dropDownList.SelectedValue = pd.Converter.ConvertToString(pd.GetValue(instance));
				return;
			}
			TextBox textBox = (TextBox)control;
			textBox.Text = pd.Converter.ConvertToString(pd.GetValue(instance));
		}

		// Token: 0x04002F6C RID: 12140
		private const int TextBoxColumns = 30;

		// Token: 0x04002F6D RID: 12141
		private ArrayList _editorControls;

		// Token: 0x04002F6E RID: 12142
		private string[] _errorMessages;

		// Token: 0x04002F6F RID: 12143
		private static readonly Attribute[] FilterAttributes = new Attribute[] { WebBrowsableAttribute.Yes };

		// Token: 0x04002F70 RID: 12144
		private static readonly WebPart designModeWebPart = new PropertyGridEditorPart.DesignModeWebPart();

		// Token: 0x04002F71 RID: 12145
		private static readonly UrlPropertyAttribute urlPropertyAttribute = new UrlPropertyAttribute();

		// Token: 0x020006EA RID: 1770
		private sealed class DesignModeWebPart : WebPart
		{
			// Token: 0x17001663 RID: 5731
			// (get) Token: 0x060056BD RID: 22205 RVA: 0x0015E27A File Offset: 0x0015D27A
			// (set) Token: 0x060056BE RID: 22206 RVA: 0x0015E27D File Offset: 0x0015D27D
			[PropertyGridEditorPart.DesignModeWebPart.WebSysWebDisplayNameAttribute("PropertyGridEditorPart_DesignModeWebPart_BoolProperty")]
			[WebBrowsable]
			public bool BoolProperty
			{
				get
				{
					return false;
				}
				set
				{
				}
			}

			// Token: 0x17001664 RID: 5732
			// (get) Token: 0x060056BF RID: 22207 RVA: 0x0015E27F File Offset: 0x0015D27F
			// (set) Token: 0x060056C0 RID: 22208 RVA: 0x0015E282 File Offset: 0x0015D282
			[WebBrowsable]
			[PropertyGridEditorPart.DesignModeWebPart.WebSysWebDisplayNameAttribute("PropertyGridEditorPart_DesignModeWebPart_EnumProperty")]
			public PropertyGridEditorPart.DesignModeWebPart.SampleEnum EnumProperty
			{
				get
				{
					return PropertyGridEditorPart.DesignModeWebPart.SampleEnum.EnumValue;
				}
				set
				{
				}
			}

			// Token: 0x17001665 RID: 5733
			// (get) Token: 0x060056C1 RID: 22209 RVA: 0x0015E284 File Offset: 0x0015D284
			// (set) Token: 0x060056C2 RID: 22210 RVA: 0x0015E28B File Offset: 0x0015D28B
			[WebBrowsable]
			[PropertyGridEditorPart.DesignModeWebPart.WebSysWebDisplayNameAttribute("PropertyGridEditorPart_DesignModeWebPart_StringProperty")]
			public string StringProperty
			{
				get
				{
					return string.Empty;
				}
				set
				{
				}
			}

			// Token: 0x020006EB RID: 1771
			public enum SampleEnum
			{
				// Token: 0x04002F73 RID: 12147
				EnumValue
			}

			// Token: 0x020006ED RID: 1773
			private sealed class WebSysWebDisplayNameAttribute : WebDisplayNameAttribute
			{
				// Token: 0x060056CD RID: 22221 RVA: 0x0015E322 File Offset: 0x0015D322
				internal WebSysWebDisplayNameAttribute(string DisplayName)
					: base(DisplayName)
				{
				}

				// Token: 0x17001668 RID: 5736
				// (get) Token: 0x060056CE RID: 22222 RVA: 0x0015E32B File Offset: 0x0015D32B
				public override string DisplayName
				{
					get
					{
						if (!this.replaced)
						{
							this.replaced = true;
							base.DisplayNameValue = SR.GetString(base.DisplayName);
						}
						return base.DisplayName;
					}
				}

				// Token: 0x17001669 RID: 5737
				// (get) Token: 0x060056CF RID: 22223 RVA: 0x0015E353 File Offset: 0x0015D353
				public override object TypeId
				{
					get
					{
						return typeof(WebDisplayNameAttribute);
					}
				}

				// Token: 0x04002F76 RID: 12150
				private bool replaced;
			}
		}
	}
}

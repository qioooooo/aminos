using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200046D RID: 1133
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class LoginViewDesigner : ControlDesigner
	{
		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x0600291E RID: 10526 RVA: 0x000E162C File Offset: 0x000E062C
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new LoginViewDesigner.LoginViewDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x0600291F RID: 10527 RVA: 0x000E165C File Offset: 0x000E065C
		private object CurrentObject
		{
			get
			{
				if (this.CurrentView == 0)
				{
					return base.Component;
				}
				if (this.CurrentView == 1)
				{
					return base.Component;
				}
				return this._loginView.RoleGroups[this.CurrentView - 2];
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06002920 RID: 10528 RVA: 0x000E16A4 File Offset: 0x000E06A4
		private ITemplate CurrentTemplate
		{
			get
			{
				if (this.CurrentView == 0)
				{
					return this._loginView.AnonymousTemplate;
				}
				if (this.CurrentView == 1)
				{
					return this._loginView.LoggedInTemplate;
				}
				RoleGroup roleGroup = this._loginView.RoleGroups[this.CurrentView - 2];
				return roleGroup.ContentTemplate;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06002921 RID: 10529 RVA: 0x000E16FC File Offset: 0x000E06FC
		private PropertyDescriptor CurrentTemplateDescriptor
		{
			get
			{
				if (this.CurrentView == 0)
				{
					return TypeDescriptor.GetProperties(base.Component)["AnonymousTemplate"];
				}
				if (this.CurrentView == 1)
				{
					return TypeDescriptor.GetProperties(base.Component)["LoggedInTemplate"];
				}
				RoleGroup roleGroup = this._loginView.RoleGroups[this.CurrentView - 2];
				return TypeDescriptor.GetProperties(roleGroup)["ContentTemplate"];
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x000E1770 File Offset: 0x000E0770
		// (set) Token: 0x06002923 RID: 10531 RVA: 0x000E17B5 File Offset: 0x000E07B5
		private int CurrentView
		{
			get
			{
				object obj = base.DesignerState["CurrentView"];
				int num = ((obj == null) ? 0 : ((int)obj));
				if (num <= 2 + this._loginView.RoleGroups.Count - 1)
				{
					return num;
				}
				return 0;
			}
			set
			{
				base.DesignerState["CurrentView"] = value;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06002924 RID: 10532 RVA: 0x000E17D0 File Offset: 0x000E07D0
		private ITemplate CurrentViewControlTemplate
		{
			get
			{
				if (this.CurrentView == 0)
				{
					return ((LoginView)base.ViewControl).AnonymousTemplate;
				}
				if (this.CurrentView == 1)
				{
					return ((LoginView)base.ViewControl).LoggedInTemplate;
				}
				RoleGroup roleGroup = ((LoginView)base.ViewControl).RoleGroups[this.CurrentView - 2];
				return roleGroup.ContentTemplate;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06002925 RID: 10533 RVA: 0x000E1834 File Offset: 0x000E0834
		private TemplateDefinition TemplateDefinition
		{
			get
			{
				int currentView = this.CurrentView;
				if (currentView == 0)
				{
					return new TemplateDefinition(this, "AnonymousTemplate", this._loginView, "AnonymousTemplate");
				}
				if (this.CurrentView == 1)
				{
					return new TemplateDefinition(this, "LoggedInTemplate", this._loginView, "LoggedInTemplate");
				}
				return new TemplateDefinition(this, "ContentTemplate", this._loginView.RoleGroups[currentView - 2], "ContentTemplate");
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06002926 RID: 10534 RVA: 0x000E18A8 File Offset: 0x000E08A8
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				if (this._templateGroups == null)
				{
					this._templateGroups = new TemplateGroupCollection();
					TemplateGroup templateGroup = new TemplateGroup("AnonymousTemplate");
					templateGroup.AddTemplateDefinition(new TemplateDefinition(this, "AnonymousTemplate", this._loginView, "AnonymousTemplate"));
					this._templateGroups.Add(templateGroup);
					templateGroup = new TemplateGroup("LoggedInTemplate");
					templateGroup.AddTemplateDefinition(new TemplateDefinition(this, "LoggedInTemplate", this._loginView, "LoggedInTemplate"));
					this._templateGroups.Add(templateGroup);
					RoleGroupCollection roleGroups = this._loginView.RoleGroups;
					for (int i = 0; i < roleGroups.Count; i++)
					{
						string text = LoginViewDesigner.CreateRoleGroupCaption(i, roleGroups);
						templateGroup = new TemplateGroup(text);
						templateGroup.AddTemplateDefinition(new TemplateDefinition(this, text, this._loginView.RoleGroups[i], "ContentTemplate"));
						this._templateGroups.Add(templateGroup);
					}
				}
				templateGroups.AddRange(this._templateGroups);
				return templateGroups;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002927 RID: 10535 RVA: 0x000E19A6 File Offset: 0x000E09A6
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x000E19AC File Offset: 0x000E09AC
		private EditableDesignerRegion BuildRegion()
		{
			return new LoginViewDesigner.LoginViewDesignerRegion(this, this.CurrentObject, this.CurrentTemplate, this.CurrentTemplateDescriptor, this.TemplateDefinition)
			{
				Description = SR.GetString("ContainerControlDesigner_RegionWatermark")
			};
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x000E19EC File Offset: 0x000E09EC
		private static string CreateRoleGroupCaption(int roleGroupIndex, RoleGroupCollection roleGroups)
		{
			string text = roleGroups[roleGroupIndex].ToString();
			string text2 = "RoleGroup[" + roleGroupIndex.ToString(CultureInfo.InvariantCulture) + "]";
			if (text != null && text.Length > 0)
			{
				text2 = text2 + " - " + text;
			}
			return text2;
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000E1A3C File Offset: 0x000E0A3C
		private void EditRoleGroups()
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["RoleGroups"];
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.EditRoleGroupsChangeCallback), propertyDescriptor, SR.GetString("LoginView_EditRoleGroupsTransactionDescription"), propertyDescriptor);
			int num = this._loginView.RoleGroups.Count + 2;
			if (this.CurrentView >= num)
			{
				this.CurrentView = num - 1;
			}
			if (this.CurrentView < 0)
			{
				this.CurrentView = 0;
			}
			this._templateGroups = null;
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000E1AC0 File Offset: 0x000E0AC0
		private bool EditRoleGroupsChangeCallback(object context)
		{
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)context;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			UITypeEditor uitypeEditor = (UITypeEditor)propertyDescriptor.GetEditor(typeof(UITypeEditor));
			object obj = uitypeEditor.EditValue(new TypeDescriptorContext(designerHost, propertyDescriptor, base.Component), new WindowsFormsEditorServiceHelper(this), propertyDescriptor.GetValue(base.Component));
			return obj != null;
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x000E1B30 File Offset: 0x000E0B30
		public override string GetDesignTimeHtml()
		{
			string text = string.Empty;
			if (this.CurrentViewControlTemplate != null)
			{
				LoginView loginView = (LoginView)base.ViewControl;
				IDictionary dictionary = new HybridDictionary(1);
				dictionary["TemplateIndex"] = this.CurrentView;
				((IControlDesignerAccessor)loginView).SetDesignModeState(dictionary);
				loginView.DataBind();
				text = base.GetDesignTimeHtml();
			}
			return text;
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x000E1B8C File Offset: 0x000E0B8C
		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			string text = string.Empty;
			bool flag = base.UseRegions(regions, this.CurrentTemplate, this.CurrentViewControlTemplate);
			if (flag)
			{
				regions.Add(this.BuildRegion());
			}
			else
			{
				text = this.GetDesignTimeHtml();
			}
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<table cellspacing=0 cellpadding=0 border=0 style=\"display:inline-block\">\r\n                <tr>\r\n                    <td nowrap align=center valign=middle style=\"color:{0}; background-color:{1}; \">{2}</td>\r\n                </tr>\r\n                <tr>\r\n                    <td style=\"vertical-align:top;\" {3}='0'>{4}</td>\r\n                </tr>\r\n          </table>", new object[]
			{
				ColorTranslator.ToHtml(SystemColors.ControlText),
				ColorTranslator.ToHtml(SystemColors.Control),
				this._loginView.ID,
				DesignerRegion.DesignerRegionAttributeName,
				text
			}));
			return stringBuilder.ToString();
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000E1C30 File Offset: 0x000E0C30
		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			if (region is LoginViewDesigner.LoginViewDesignerRegion)
			{
				ITemplate template = ((LoginViewDesigner.LoginViewDesignerRegion)region).Template;
				if (template != null)
				{
					IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
					return ControlPersister.PersistTemplate(template, designerHost);
				}
			}
			return base.GetEditableDesignerRegionContent(region);
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x000E1C84 File Offset: 0x000E0C84
		protected override string GetEmptyDesignTimeHtml()
		{
			string text = string.Empty;
			switch (this.CurrentView)
			{
			case 0:
				text = SR.GetString("LoginView_AnonymousTemplateEmpty");
				break;
			case 1:
				text = SR.GetString("LoginView_LoggedInTemplateEmpty");
				break;
			default:
			{
				int num = this.CurrentView - 2;
				string text2 = LoginViewDesigner.CreateRoleGroupCaption(num, this._loginView.RoleGroups);
				text = SR.GetString("LoginView_RoleGroupTemplateEmpty", new object[] { text2 });
				break;
			}
			}
			return base.CreatePlaceHolderDesignTimeHtml(text + "<br>" + SR.GetString("LoginView_NoTemplateInst"));
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x000E1D18 File Offset: 0x000E0D18
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("LoginView_ErrorRendering") + "<br />" + e.Message);
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x000E1D3A File Offset: 0x000E0D3A
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(LoginView));
			this._loginView = (LoginView)component;
			base.Initialize(component);
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000E1D60 File Offset: 0x000E0D60
		private void LaunchWebAdmin()
		{
			if (base.Component.Site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					IWebAdministrationService webAdministrationService = (IWebAdministrationService)designerHost.GetService(typeof(IWebAdministrationService));
					if (webAdministrationService != null)
					{
						webAdministrationService.Start(null);
					}
				}
			}
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x000E1DC0 File Offset: 0x000E0DC0
		public override void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (e.Member == null || e.Member.Name.Equals("RoleGroups"))
			{
				int num = this._loginView.RoleGroups.Count + 2;
				if (this.CurrentView >= num)
				{
					this.CurrentView = num - 1;
				}
				this._templateGroups = null;
			}
			base.OnComponentChanged(sender, e);
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000E1E20 File Offset: 0x000E0E20
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			if (base.InTemplateMode)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["RoleGroups"];
				properties["RoleGroups"] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
			}
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x000E1E74 File Offset: 0x000E0E74
		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			LoginViewDesigner.LoginViewDesignerRegion loginViewDesignerRegion = region as LoginViewDesigner.LoginViewDesignerRegion;
			if (loginViewDesignerRegion == null)
			{
				return;
			}
			IDesignerHost designerHost = (IDesignerHost)base.Component.Site.GetService(typeof(IDesignerHost));
			ITemplate template = ControlParser.ParseTemplate(designerHost, content);
			using (DesignerTransaction designerTransaction = designerHost.CreateTransaction("SetEditableDesignerRegionContent"))
			{
				loginViewDesignerRegion.PropertyDescriptor.SetValue(loginViewDesignerRegion.Object, template);
				designerTransaction.Commit();
			}
			loginViewDesignerRegion.Template = template;
		}

		// Token: 0x04001C5B RID: 7259
		private const string _designtimeHTML = "<table cellspacing=0 cellpadding=0 border=0 style=\"display:inline-block\">\r\n                <tr>\r\n                    <td nowrap align=center valign=middle style=\"color:{0}; background-color:{1}; \">{2}</td>\r\n                </tr>\r\n                <tr>\r\n                    <td style=\"vertical-align:top;\" {3}='0'>{4}</td>\r\n                </tr>\r\n          </table>";

		// Token: 0x04001C5C RID: 7260
		private const int _anonymousTemplateIndex = 0;

		// Token: 0x04001C5D RID: 7261
		private const int _loggedInTemplateIndex = 1;

		// Token: 0x04001C5E RID: 7262
		private const int _roleGroupStartingIndex = 2;

		// Token: 0x04001C5F RID: 7263
		private const string _anonymousTemplateName = "AnonymousTemplate";

		// Token: 0x04001C60 RID: 7264
		private const string _loggedInTemplateName = "LoggedInTemplate";

		// Token: 0x04001C61 RID: 7265
		private const string _contentTemplateName = "ContentTemplate";

		// Token: 0x04001C62 RID: 7266
		private const string _roleGroupsPropertyName = "RoleGroups";

		// Token: 0x04001C63 RID: 7267
		private LoginView _loginView;

		// Token: 0x04001C64 RID: 7268
		private TemplateGroupCollection _templateGroups;

		// Token: 0x04001C65 RID: 7269
		private static readonly string[] _templateNames = new string[] { "AnonymousTemplate", "LoggedInTemplate" };

		// Token: 0x0200046E RID: 1134
		private class LoginViewDesignerRegion : TemplatedEditableDesignerRegion
		{
			// Token: 0x170007A8 RID: 1960
			// (get) Token: 0x06002938 RID: 10552 RVA: 0x000E1F2E File Offset: 0x000E0F2E
			// (set) Token: 0x06002939 RID: 10553 RVA: 0x000E1F36 File Offset: 0x000E0F36
			public ITemplate Template
			{
				get
				{
					return this._template;
				}
				set
				{
					this._template = value;
				}
			}

			// Token: 0x170007A9 RID: 1961
			// (get) Token: 0x0600293A RID: 10554 RVA: 0x000E1F3F File Offset: 0x000E0F3F
			public object Object
			{
				get
				{
					return this._object;
				}
			}

			// Token: 0x170007AA RID: 1962
			// (get) Token: 0x0600293B RID: 10555 RVA: 0x000E1F47 File Offset: 0x000E0F47
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return this._prop;
				}
			}

			// Token: 0x0600293C RID: 10556 RVA: 0x000E1F4F File Offset: 0x000E0F4F
			public LoginViewDesignerRegion(ControlDesigner owner, object obj, ITemplate template, PropertyDescriptor descriptor, TemplateDefinition definition)
				: base(definition)
			{
				this._template = template;
				this._object = obj;
				this._prop = descriptor;
				base.EnsureSize = true;
			}

			// Token: 0x04001C66 RID: 7270
			private ITemplate _template;

			// Token: 0x04001C67 RID: 7271
			private object _object;

			// Token: 0x04001C68 RID: 7272
			private PropertyDescriptor _prop;
		}

		// Token: 0x0200046F RID: 1135
		private class LoginViewDesignerActionList : DesignerActionList
		{
			// Token: 0x0600293D RID: 10557 RVA: 0x000E1F76 File Offset: 0x000E0F76
			public LoginViewDesignerActionList(LoginViewDesigner designer)
				: base(designer.Component)
			{
				this._designer = designer;
			}

			// Token: 0x170007AB RID: 1963
			// (get) Token: 0x0600293E RID: 10558 RVA: 0x000E1F8B File Offset: 0x000E0F8B
			// (set) Token: 0x0600293F RID: 10559 RVA: 0x000E1F8E File Offset: 0x000E0F8E
			public override bool AutoShow
			{
				get
				{
					return true;
				}
				set
				{
				}
			}

			// Token: 0x170007AC RID: 1964
			// (get) Token: 0x06002940 RID: 10560 RVA: 0x000E1F90 File Offset: 0x000E0F90
			// (set) Token: 0x06002941 RID: 10561 RVA: 0x000E2014 File Offset: 0x000E1014
			[TypeConverter(typeof(LoginViewDesigner.LoginViewDesignerActionList.LoginViewViewTypeConverter))]
			public string View
			{
				get
				{
					int num = this._designer.CurrentView;
					if (num - 2 >= this._designer._loginView.RoleGroups.Count)
					{
						num = this._designer._loginView.RoleGroups.Count + 1;
						this._designer.CurrentView = num;
					}
					if (num == 0)
					{
						return "AnonymousTemplate";
					}
					if (num == 1)
					{
						return "LoggedInTemplate";
					}
					return LoginViewDesigner.CreateRoleGroupCaption(num - 2, this._designer._loginView.RoleGroups);
				}
				set
				{
					if (string.Compare(value, "AnonymousTemplate", StringComparison.Ordinal) == 0)
					{
						this._designer.CurrentView = 0;
					}
					else if (string.Compare(value, "LoggedInTemplate", StringComparison.Ordinal) == 0)
					{
						this._designer.CurrentView = 1;
					}
					else
					{
						RoleGroupCollection roleGroups = this._designer._loginView.RoleGroups;
						for (int i = 0; i < roleGroups.Count; i++)
						{
							string text = LoginViewDesigner.CreateRoleGroupCaption(i, roleGroups);
							if (string.Compare(value, text, StringComparison.Ordinal) == 0)
							{
								this._designer.CurrentView = i + 2;
							}
						}
					}
					this._designer.UpdateDesignTimeHtml();
				}
			}

			// Token: 0x06002942 RID: 10562 RVA: 0x000E20A6 File Offset: 0x000E10A6
			public void EditRoleGroups()
			{
				this._designer.EditRoleGroups();
			}

			// Token: 0x06002943 RID: 10563 RVA: 0x000E20B3 File Offset: 0x000E10B3
			public void LaunchWebAdmin()
			{
				this._designer.LaunchWebAdmin();
			}

			// Token: 0x06002944 RID: 10564 RVA: 0x000E20C0 File Offset: 0x000E10C0
			public override DesignerActionItemCollection GetSortedActionItems()
			{
				return new DesignerActionItemCollection
				{
					new DesignerActionMethodItem(this, "EditRoleGroups", SR.GetString("LoginView_EditRoleGroups"), string.Empty, SR.GetString("LoginView_EditRoleGroupsDescription"), true),
					new DesignerActionPropertyItem("View", SR.GetString("WebControls_Views"), string.Empty, SR.GetString("WebControls_ViewsDescription")),
					new DesignerActionMethodItem(this, "LaunchWebAdmin", SR.GetString("Login_LaunchWebAdmin"), string.Empty, SR.GetString("Login_LaunchWebAdminDescription"), true)
				};
			}

			// Token: 0x04001C69 RID: 7273
			private LoginViewDesigner _designer;

			// Token: 0x02000470 RID: 1136
			private class LoginViewViewTypeConverter : TypeConverter
			{
				// Token: 0x06002945 RID: 10565 RVA: 0x000E2158 File Offset: 0x000E1158
				public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
				{
					LoginViewDesigner.LoginViewDesignerActionList loginViewDesignerActionList = (LoginViewDesigner.LoginViewDesignerActionList)context.Instance;
					LoginView loginView = loginViewDesignerActionList._designer._loginView;
					RoleGroupCollection roleGroups = loginView.RoleGroups;
					string[] array = new string[roleGroups.Count + 2];
					array[0] = "AnonymousTemplate";
					array[1] = "LoggedInTemplate";
					for (int i = 0; i < roleGroups.Count; i++)
					{
						array[i + 2] = LoginViewDesigner.CreateRoleGroupCaption(i, roleGroups);
					}
					return new TypeConverter.StandardValuesCollection(array);
				}

				// Token: 0x06002946 RID: 10566 RVA: 0x000E21CB File Offset: 0x000E11CB
				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return true;
				}

				// Token: 0x06002947 RID: 10567 RVA: 0x000E21CE File Offset: 0x000E11CE
				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
			}
		}
	}
}

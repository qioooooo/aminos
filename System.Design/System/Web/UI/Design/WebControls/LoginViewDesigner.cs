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
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class LoginViewDesigner : ControlDesigner
	{
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

		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		private EditableDesignerRegion BuildRegion()
		{
			return new LoginViewDesigner.LoginViewDesignerRegion(this, this.CurrentObject, this.CurrentTemplate, this.CurrentTemplateDescriptor, this.TemplateDefinition)
			{
				Description = SR.GetString("ContainerControlDesigner_RegionWatermark")
			};
		}

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

		private bool EditRoleGroupsChangeCallback(object context)
		{
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)context;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			UITypeEditor uitypeEditor = (UITypeEditor)propertyDescriptor.GetEditor(typeof(UITypeEditor));
			object obj = uitypeEditor.EditValue(new TypeDescriptorContext(designerHost, propertyDescriptor, base.Component), new WindowsFormsEditorServiceHelper(this), propertyDescriptor.GetValue(base.Component));
			return obj != null;
		}

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

		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("LoginView_ErrorRendering") + "<br />" + e.Message);
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(LoginView));
			this._loginView = (LoginView)component;
			base.Initialize(component);
		}

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

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			if (base.InTemplateMode)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["RoleGroups"];
				properties["RoleGroups"] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
			}
		}

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

		private const string _designtimeHTML = "<table cellspacing=0 cellpadding=0 border=0 style=\"display:inline-block\">\r\n                <tr>\r\n                    <td nowrap align=center valign=middle style=\"color:{0}; background-color:{1}; \">{2}</td>\r\n                </tr>\r\n                <tr>\r\n                    <td style=\"vertical-align:top;\" {3}='0'>{4}</td>\r\n                </tr>\r\n          </table>";

		private const int _anonymousTemplateIndex = 0;

		private const int _loggedInTemplateIndex = 1;

		private const int _roleGroupStartingIndex = 2;

		private const string _anonymousTemplateName = "AnonymousTemplate";

		private const string _loggedInTemplateName = "LoggedInTemplate";

		private const string _contentTemplateName = "ContentTemplate";

		private const string _roleGroupsPropertyName = "RoleGroups";

		private LoginView _loginView;

		private TemplateGroupCollection _templateGroups;

		private static readonly string[] _templateNames = new string[] { "AnonymousTemplate", "LoggedInTemplate" };

		private class LoginViewDesignerRegion : TemplatedEditableDesignerRegion
		{
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

			public object Object
			{
				get
				{
					return this._object;
				}
			}

			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return this._prop;
				}
			}

			public LoginViewDesignerRegion(ControlDesigner owner, object obj, ITemplate template, PropertyDescriptor descriptor, TemplateDefinition definition)
				: base(definition)
			{
				this._template = template;
				this._object = obj;
				this._prop = descriptor;
				base.EnsureSize = true;
			}

			private ITemplate _template;

			private object _object;

			private PropertyDescriptor _prop;
		}

		private class LoginViewDesignerActionList : DesignerActionList
		{
			public LoginViewDesignerActionList(LoginViewDesigner designer)
				: base(designer.Component)
			{
				this._designer = designer;
			}

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

			public void EditRoleGroups()
			{
				this._designer.EditRoleGroups();
			}

			public void LaunchWebAdmin()
			{
				this._designer.LaunchWebAdmin();
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				return new DesignerActionItemCollection
				{
					new DesignerActionMethodItem(this, "EditRoleGroups", SR.GetString("LoginView_EditRoleGroups"), string.Empty, SR.GetString("LoginView_EditRoleGroupsDescription"), true),
					new DesignerActionPropertyItem("View", SR.GetString("WebControls_Views"), string.Empty, SR.GetString("WebControls_ViewsDescription")),
					new DesignerActionMethodItem(this, "LaunchWebAdmin", SR.GetString("Login_LaunchWebAdmin"), string.Empty, SR.GetString("Login_LaunchWebAdminDescription"), true)
				};
			}

			private LoginViewDesigner _designer;

			private class LoginViewViewTypeConverter : TypeConverter
			{
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

				public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
				{
					return true;
				}

				public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
				{
					return true;
				}
			}
		}
	}
}

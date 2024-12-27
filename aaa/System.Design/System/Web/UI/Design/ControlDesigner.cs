using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;

namespace System.Web.UI.Design
{
	// Token: 0x0200032E RID: 814
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ControlDesigner : HtmlControlDesigner
	{
		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001E81 RID: 7809 RVA: 0x000ACBC8 File Offset: 0x000ABBC8
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
				designerActionListCollection.AddRange(base.ActionLists);
				designerActionListCollection.Add(new ControlDesigner.ControlDesignerActionList(this));
				return designerActionListCollection;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x000ACBF5 File Offset: 0x000ABBF5
		public virtual bool AllowResize
		{
			get
			{
				return this.IsWebControl;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001E83 RID: 7811 RVA: 0x000ACBFD File Offset: 0x000ABBFD
		public virtual DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				return new DesignerAutoFormatCollection();
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x000ACC04 File Offset: 0x000ABC04
		protected virtual bool DataBindingsEnabled
		{
			get
			{
				ControlDesigner designer;
				for (IControlDesignerView controlDesignerView = this.View; controlDesignerView != null; controlDesignerView = designer.View)
				{
					EditableDesignerRegion editableDesignerRegion = (EditableDesignerRegion)controlDesignerView.ContainingRegion;
					if (editableDesignerRegion == null)
					{
						return false;
					}
					if (editableDesignerRegion.SupportsDataBinding)
					{
						return true;
					}
					designer = editableDesignerRegion.Designer;
					if (designer == null)
					{
						return false;
					}
				}
				return false;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001E85 RID: 7813 RVA: 0x000ACC4E File Offset: 0x000ABC4E
		protected ControlDesignerState DesignerState
		{
			get
			{
				if (this._designerState == null)
				{
					this._designerState = new ControlDesignerState(base.Component);
				}
				return this._designerState;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x000ACC6F File Offset: 0x000ABC6F
		[Obsolete("The recommended alternative is SetViewFlags(ViewFlags.DesignTimeHtmlRequiresLoadComplete, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual bool DesignTimeHtmlRequiresLoadComplete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x000ACC72 File Offset: 0x000ABC72
		protected internal virtual bool HidePropertiesInTemplateMode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x000ACC75 File Offset: 0x000ABC75
		// (set) Token: 0x06001E89 RID: 7817 RVA: 0x000ACC87 File Offset: 0x000ABC87
		public virtual string ID
		{
			get
			{
				return ((Control)base.Component).ID;
			}
			set
			{
				if (this.RootDesigner != null)
				{
					this.RootDesigner.SetControlID((Control)base.Component, value);
				}
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x000ACCA8 File Offset: 0x000ABCA8
		protected bool InTemplateMode
		{
			get
			{
				return this._inTemplateMode;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x000ACCB0 File Offset: 0x000ABCB0
		// (set) Token: 0x06001E8C RID: 7820 RVA: 0x000ACCB8 File Offset: 0x000ABCB8
		[Obsolete("The recommended alternative is to use Tag.SetDirty() and Tag.IsDirty. http://go.microsoft.com/fwlink/?linkid=14202")]
		public bool IsDirty
		{
			get
			{
				return this.IsDirtyInternal;
			}
			set
			{
				this.IsDirtyInternal = value;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x000ACCC1 File Offset: 0x000ABCC1
		// (set) Token: 0x06001E8E RID: 7822 RVA: 0x000ACCDD File Offset: 0x000ABCDD
		internal bool IsDirtyInternal
		{
			get
			{
				if (this.Tag != null)
				{
					return this.Tag.IsDirty;
				}
				return this.fDirty;
			}
			set
			{
				if (this.Tag != null)
				{
					this.Tag.SetDirty(value);
					return;
				}
				this.fDirty = value;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x000ACCFB File Offset: 0x000ABCFB
		internal bool IsIgnoringComponentChanges
		{
			get
			{
				return this._ignoreComponentChangesCount > 0;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x000ACD06 File Offset: 0x000ABD06
		internal bool IsWebControl
		{
			get
			{
				return this.isWebControl;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x000ACD0E File Offset: 0x000ABD0E
		internal string LocalizedInnerContent
		{
			get
			{
				return this._localizedInnerContent;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001E92 RID: 7826 RVA: 0x000ACD16 File Offset: 0x000ABD16
		// (set) Token: 0x06001E93 RID: 7827 RVA: 0x000ACD1E File Offset: 0x000ABD1E
		public virtual bool ViewControlCreated
		{
			get
			{
				return this._viewControlCreated;
			}
			set
			{
				this._viewControlCreated = value;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001E94 RID: 7828 RVA: 0x000ACD27 File Offset: 0x000ABD27
		// (set) Token: 0x06001E95 RID: 7829 RVA: 0x000ACD2F File Offset: 0x000ABD2F
		[Obsolete("The recommended alternative is to inherit from ContainerControlDesigner instead and to use an EditableDesignerRegion. Regions allow for better control of the content in the designer. http://go.microsoft.com/fwlink/?linkid=14202")]
		public bool ReadOnly
		{
			get
			{
				return this.ReadOnlyInternal;
			}
			set
			{
				this.ReadOnlyInternal = value;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001E96 RID: 7830 RVA: 0x000ACD38 File Offset: 0x000ABD38
		// (set) Token: 0x06001E97 RID: 7831 RVA: 0x000ACD40 File Offset: 0x000ABD40
		internal bool ReadOnlyInternal
		{
			get
			{
				return this.readOnly;
			}
			set
			{
				this.readOnly = value;
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001E98 RID: 7832 RVA: 0x000ACD4C File Offset: 0x000ABD4C
		protected WebFormsRootDesigner RootDesigner
		{
			get
			{
				WebFormsRootDesigner webFormsRootDesigner = null;
				ISite site = base.Component.Site;
				if (site != null)
				{
					IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
					if (designerHost != null && designerHost.RootComponent != null)
					{
						webFormsRootDesigner = designerHost.GetDesigner(designerHost.RootComponent) as WebFormsRootDesigner;
					}
				}
				return webFormsRootDesigner;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001E99 RID: 7833 RVA: 0x000ACDA0 File Offset: 0x000ABDA0
		private bool SupportsDataBindings
		{
			get
			{
				BindableAttribute bindableAttribute = (BindableAttribute)TypeDescriptor.GetAttributes(base.Component)[typeof(BindableAttribute)];
				return bindableAttribute != null && bindableAttribute.Bindable;
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001E9A RID: 7834 RVA: 0x000ACDD8 File Offset: 0x000ABDD8
		protected IControlDesignerTag Tag
		{
			get
			{
				return this._tag;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001E9B RID: 7835 RVA: 0x000ACDE0 File Offset: 0x000ABDE0
		public virtual TemplateGroupCollection TemplateGroups
		{
			get
			{
				return new TemplateGroupCollection();
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001E9C RID: 7836 RVA: 0x000ACDE8 File Offset: 0x000ABDE8
		protected virtual bool UsePreviewControl
		{
			get
			{
				object[] customAttributes = base.GetType().GetCustomAttributes(typeof(SupportsPreviewControlAttribute), false);
				if (customAttributes.Length > 0)
				{
					SupportsPreviewControlAttribute supportsPreviewControlAttribute = (SupportsPreviewControlAttribute)customAttributes[0];
					return supportsPreviewControlAttribute.SupportsPreviewControl;
				}
				return false;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001E9D RID: 7837 RVA: 0x000ACE23 File Offset: 0x000ABE23
		internal IControlDesignerView View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001E9E RID: 7838 RVA: 0x000ACE2B File Offset: 0x000ABE2B
		// (set) Token: 0x06001E9F RID: 7839 RVA: 0x000ACE63 File Offset: 0x000ABE63
		public Control ViewControl
		{
			get
			{
				if (!this.ViewControlCreated)
				{
					this._viewControl = (this.UsePreviewControl ? this.CreateViewControlInternal() : ((Control)base.Component));
					this.ViewControlCreated = true;
				}
				return this._viewControl;
			}
			set
			{
				this._viewControl = value;
				this.ViewControlCreated = true;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x000ACE73 File Offset: 0x000ABE73
		protected virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001EA1 RID: 7841 RVA: 0x000ACE78 File Offset: 0x000ABE78
		[Obsolete("Error: This property can no longer be referenced, and is included to support existing compiled applications. The design-time element view architecture is no longer used. http://go.microsoft.com/fwlink/?linkid=14202", true)]
		protected object DesignTimeElementView
		{
			get
			{
				IHtmlControlDesignerBehavior behaviorInternal = this.BehaviorInternal;
				if (behaviorInternal != null)
				{
					return ((IControlDesignerBehavior)behaviorInternal).DesignTimeElementView;
				}
				return null;
			}
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000ACE9C File Offset: 0x000ABE9C
		internal static DesignerAutoFormatCollection CreateAutoFormats(string schemes, ControlDesigner.CreateAutoFormatDelegate createAutoFormatDelegate)
		{
			DesignerAutoFormatCollection designerAutoFormatCollection = new DesignerAutoFormatCollection();
			try
			{
				DataSet dataSet = new DataSet();
				dataSet.Locale = CultureInfo.InvariantCulture;
				dataSet.ReadXml(new XmlTextReader(new StringReader(schemes)));
				DataTable dataTable = dataSet.Tables[0];
				dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["SchemeName"] };
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					designerAutoFormatCollection.Add(createAutoFormatDelegate(dataTable.Rows[i]));
				}
			}
			catch (Exception)
			{
			}
			return designerAutoFormatCollection;
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x000ACF48 File Offset: 0x000ABF48
		internal Control CreateClonedControl(IDesignerHost parseTimeDesignerHost, bool applyTheme)
		{
			string text = null;
			if (this.Tag != null)
			{
				text = this.Tag.GetOuterContent();
			}
			if (string.IsNullOrEmpty(text))
			{
				text = ControlPersister.PersistControl((Control)base.Component);
			}
			return ControlParser.ParseControl(parseTimeDesignerHost, text, applyTheme);
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x000ACF8E File Offset: 0x000ABF8E
		protected string CreatePlaceHolderDesignTimeHtml()
		{
			return this.CreatePlaceHolderDesignTimeHtml(null);
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x000ACF98 File Offset: 0x000ABF98
		protected string CreatePlaceHolderDesignTimeHtml(string instruction)
		{
			string name = base.Component.GetType().Name;
			string name2 = base.Component.Site.Name;
			if (instruction == null)
			{
				instruction = string.Empty;
			}
			return string.Format(CultureInfo.InvariantCulture, ControlDesigner.PlaceHolderDesignTimeHtmlTemplate, new object[] { name, name2, instruction });
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000ACFF4 File Offset: 0x000ABFF4
		protected string CreateErrorDesignTimeHtml(string errorMessage)
		{
			return this.CreateErrorDesignTimeHtml(errorMessage, null);
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000ACFFE File Offset: 0x000ABFFE
		protected string CreateErrorDesignTimeHtml(string errorMessage, Exception e)
		{
			return ControlDesigner.CreateErrorDesignTimeHtml(errorMessage, e, base.Component);
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x000AD010 File Offset: 0x000AC010
		internal static string CreateErrorDesignTimeHtml(string errorMessage, Exception e, IComponent component)
		{
			string name = component.Site.Name;
			if (errorMessage == null)
			{
				errorMessage = string.Empty;
			}
			else
			{
				errorMessage = HttpUtility.HtmlEncode(errorMessage);
			}
			if (e != null)
			{
				errorMessage = errorMessage + "<br />" + HttpUtility.HtmlEncode(e.Message);
			}
			return string.Format(CultureInfo.InvariantCulture, ControlDesigner.ErrorDesignTimeHtmlTemplate, new object[]
			{
				SR.GetString("ControlDesigner_DesignTimeHtmlError"),
				HttpUtility.HtmlEncode(name),
				errorMessage
			});
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x000AD08C File Offset: 0x000AC08C
		internal string CreateInvalidParentDesignTimeHtml(Type controlType, Type requiredParentType)
		{
			return this.CreateErrorDesignTimeHtml(SR.GetString("Control_CanOnlyBePlacedInside", new object[] { controlType.Name, requiredParentType.Name }));
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x000AD0C4 File Offset: 0x000AC0C4
		private Control CreateViewControlInternal()
		{
			Control control = (Control)base.Component;
			Control control2 = this.CreateViewControl();
			((IControlDesignerAccessor)control2).SetOwnerControl(control);
			this.UpdateExpressionValues(control2);
			return control2;
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x000AD0F3 File Offset: 0x000AC0F3
		protected virtual Control CreateViewControl()
		{
			return this.CreateClonedControl((IDesignerHost)this.GetService(typeof(IDesignerHost)), true);
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x000AD114 File Offset: 0x000AC114
		private object EnsureParsedExpression(TemplateControl templateControl, ExpressionBinding eb, object parsedData)
		{
			if (parsedData == null && templateControl != null)
			{
				string text;
				Type expressionBuilderType = ExpressionEditor.GetExpressionBuilderType(eb.ExpressionPrefix, base.Component.Site, out text);
				if (expressionBuilderType != null)
				{
					try
					{
						global::System.Web.Compilation.ExpressionBuilder expressionBuilder = (global::System.Web.Compilation.ExpressionBuilder)Activator.CreateInstance(expressionBuilderType);
						ExpressionBuilderContext expressionBuilderContext = new ExpressionBuilderContext(templateControl);
						parsedData = expressionBuilder.ParseExpression(eb.Expression, eb.PropertyType, expressionBuilderContext);
					}
					catch (Exception ex)
					{
						IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)this.GetService(typeof(IComponentDesignerDebugService));
						if (componentDesignerDebugService != null)
						{
							componentDesignerDebugService.Fail(SR.GetString("ControlDesigner_CouldNotGetExpressionBuilder", new object[] { eb.ExpressionPrefix, ex.Message }));
						}
					}
				}
			}
			return parsedData;
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x000AD1D4 File Offset: 0x000AC1D4
		public Rectangle GetBounds()
		{
			if (this.View != null)
			{
				return this.View.GetBounds(null);
			}
			return Rectangle.Empty;
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x000AD1F0 File Offset: 0x000AC1F0
		internal static PropertyDescriptor GetComplexProperty(object target, string propName, out object realTarget)
		{
			realTarget = null;
			string[] array = propName.Split(new char[] { '.' });
			PropertyDescriptor propertyDescriptor = null;
			string[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				string text = array2[i];
				PropertyDescriptor propertyDescriptor2;
				if (string.IsNullOrEmpty(text))
				{
					propertyDescriptor2 = null;
				}
				else
				{
					propertyDescriptor = TypeDescriptor.GetProperties(target)[text];
					if (propertyDescriptor != null)
					{
						realTarget = target;
						target = propertyDescriptor.GetValue(target);
						i++;
						continue;
					}
					propertyDescriptor2 = null;
				}
				return propertyDescriptor2;
			}
			return propertyDescriptor;
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000AD264 File Offset: 0x000AC264
		public virtual string GetDesignTimeHtml()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			DesignTimeHtmlTextWriter designTimeHtmlTextWriter = new DesignTimeHtmlTextWriter(stringWriter);
			string text = null;
			bool flag = false;
			bool flag2 = true;
			Control control = null;
			try
			{
				control = this.ViewControl;
				flag2 = control.Visible;
				if (!flag2)
				{
					control.Visible = true;
					flag = !this.UsePreviewControl;
				}
				control.RenderControl(designTimeHtmlTextWriter);
				text = stringWriter.ToString();
			}
			catch (Exception ex)
			{
				text = this.GetErrorDesignTimeHtml(ex);
			}
			finally
			{
				if (flag)
				{
					control.Visible = flag2;
				}
			}
			if (text == null || text.Length == 0)
			{
				text = this.GetEmptyDesignTimeHtml();
			}
			return text;
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x000AD310 File Offset: 0x000AC310
		public virtual string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			return this.GetDesignTimeHtml();
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x000AD318 File Offset: 0x000AC318
		public static DesignTimeResourceProviderFactory GetDesignTimeResourceProviderFactory(IServiceProvider serviceProvider)
		{
			DesignTimeResourceProviderFactory designTimeResourceProviderFactory = null;
			IWebApplication webApplication = (IWebApplication)serviceProvider.GetService(typeof(IWebApplication));
			if (webApplication != null)
			{
				Configuration configuration = webApplication.OpenWebConfiguration(true);
				if (configuration != null)
				{
					GlobalizationSection globalizationSection = configuration.GetSection("system.web/globalization") as GlobalizationSection;
					if (globalizationSection != null)
					{
						string resourceProviderFactoryType = globalizationSection.ResourceProviderFactoryType;
						if (!string.IsNullOrEmpty(resourceProviderFactoryType))
						{
							ITypeResolutionService typeResolutionService = (ITypeResolutionService)serviceProvider.GetService(typeof(ITypeResolutionService));
							if (typeResolutionService != null)
							{
								Type type = typeResolutionService.GetType(resourceProviderFactoryType, true, true);
								if (type != null)
								{
									object[] customAttributes = type.GetCustomAttributes(typeof(DesignTimeResourceProviderFactoryAttribute), true);
									if (customAttributes != null && customAttributes.Length > 0)
									{
										DesignTimeResourceProviderFactoryAttribute designTimeResourceProviderFactoryAttribute = customAttributes[0] as DesignTimeResourceProviderFactoryAttribute;
										string factoryTypeName = designTimeResourceProviderFactoryAttribute.FactoryTypeName;
										if (!string.IsNullOrEmpty(factoryTypeName))
										{
											Type type2 = typeResolutionService.GetType(factoryTypeName, true, true);
											if (type2 != null && typeof(DesignTimeResourceProviderFactory).IsAssignableFrom(type2))
											{
												try
												{
													designTimeResourceProviderFactory = (DesignTimeResourceProviderFactory)Activator.CreateInstance(type2);
												}
												catch (Exception ex)
												{
													if (serviceProvider != null)
													{
														IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)serviceProvider.GetService(typeof(IComponentDesignerDebugService));
														if (componentDesignerDebugService != null)
														{
															componentDesignerDebugService.Fail(SR.GetString("ControlDesigner_CouldNotGetDesignTimeResourceProviderFactory", new object[] { factoryTypeName, ex.Message }));
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			if (designTimeResourceProviderFactory == null)
			{
				IDesignTimeResourceProviderFactoryService designTimeResourceProviderFactoryService = (IDesignTimeResourceProviderFactoryService)serviceProvider.GetService(typeof(IDesignTimeResourceProviderFactoryService));
				if (designTimeResourceProviderFactoryService != null)
				{
					designTimeResourceProviderFactory = designTimeResourceProviderFactoryService.GetFactory();
				}
			}
			return designTimeResourceProviderFactory;
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x000AD4B0 File Offset: 0x000AC4B0
		public virtual string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			return string.Empty;
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x000AD4B8 File Offset: 0x000AC4B8
		protected virtual string GetEmptyDesignTimeHtml()
		{
			string name = base.Component.GetType().Name;
			string name2 = base.Component.Site.Name;
			if (name2 != null && name2.Length > 0)
			{
				return string.Concat(new string[] { "[ ", name, " \"", name2, "\" ]" });
			}
			return "[ " + name + " ]";
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x000AD531 File Offset: 0x000AC531
		protected virtual string GetErrorDesignTimeHtml(Exception e)
		{
			return this.CreateErrorDesignTimeHtml(SR.GetString("ControlDesigner_UnhandledException"), e);
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x000AD544 File Offset: 0x000AC544
		[Obsolete("The recommended alternative is GetPersistenceContent(). http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual string GetPersistInnerHtml()
		{
			return this.GetPersistInnerHtmlInternal();
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x000AD54C File Offset: 0x000AC54C
		internal virtual string GetPersistInnerHtmlInternal()
		{
			if (this._localizedInnerContent != null)
			{
				return this._localizedInnerContent;
			}
			if (!this.IsDirtyInternal)
			{
				return null;
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			this.IsDirtyInternal = false;
			return ControlSerializer.SerializeInnerContents((Control)base.Component, designerHost);
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x000AD5A0 File Offset: 0x000AC5A0
		public virtual string GetPersistenceContent()
		{
			return this.GetPersistInnerHtml();
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x000AD5A8 File Offset: 0x000AC5A8
		internal void HideAllPropertiesExceptID(IDictionary properties)
		{
			ICollection values = properties.Values;
			if (values != null)
			{
				object[] array = new object[values.Count];
				values.CopyTo(array, 0);
				foreach (PropertyDescriptor propertyDescriptor in array)
				{
					if (propertyDescriptor != null && !string.Equals(propertyDescriptor.Name, "ID", StringComparison.OrdinalIgnoreCase))
					{
						properties[propertyDescriptor.Name] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
					}
				}
			}
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x000AD62C File Offset: 0x000AC62C
		public void Localize(IDesignTimeResourceWriter resourceWriter)
		{
			this.OnComponentChanging(base.Component, new ComponentChangingEventArgs(base.Component, null));
			string text2;
			string text = ControlLocalizer.LocalizeControl((Control)base.Component, resourceWriter, out text2);
			if (!string.IsNullOrEmpty(text))
			{
				this.SetTagAttribute("meta:resourcekey", text, true);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				this._localizedInnerContent = text2;
			}
			this.OnComponentChanged(base.Component, new ComponentChangedEventArgs(base.Component, null, null, null));
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x000AD6A4 File Offset: 0x000AC6A4
		public static ViewRendering GetViewRendering(Control control)
		{
			ControlDesigner controlDesigner = null;
			ISite site = control.Site;
			if (site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
				controlDesigner = designerHost.GetDesigner(control) as ControlDesigner;
			}
			return ControlDesigner.GetViewRendering(controlDesigner);
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x000AD6E8 File Offset: 0x000AC6E8
		public static ViewRendering GetViewRendering(ControlDesigner designer)
		{
			string text = string.Empty;
			DesignerRegionCollection designerRegionCollection = new DesignerRegionCollection();
			bool flag = true;
			if (designer != null)
			{
				bool flag2 = false;
				if (designer.View != null)
				{
					flag2 = designer.View.SupportsRegions;
				}
				try
				{
					designer.ViewControlCreated = false;
					if (flag2)
					{
						text = designer.GetDesignTimeHtml(designerRegionCollection);
					}
					else
					{
						text = designer.GetDesignTimeHtml();
					}
					flag = designer.Visible;
				}
				catch (Exception ex)
				{
					designerRegionCollection.Clear();
					try
					{
						text = designer.GetErrorDesignTimeHtml(ex);
					}
					catch (Exception ex2)
					{
						text = designer.CreateErrorDesignTimeHtml(ex2.Message);
					}
					flag = true;
				}
			}
			return new ViewRendering(text, designerRegionCollection, flag);
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x000AD790 File Offset: 0x000AC790
		public ViewRendering GetViewRendering()
		{
			EditableDesignerRegion editableDesignerRegion = null;
			if (this.View != null)
			{
				editableDesignerRegion = this.View.ContainingRegion as EditableDesignerRegion;
			}
			ViewRendering viewRendering;
			if (editableDesignerRegion != null)
			{
				viewRendering = editableDesignerRegion.GetChildViewRendering((Control)base.Component);
			}
			else
			{
				viewRendering = ControlDesigner.GetViewRendering(this);
			}
			return viewRendering;
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x000AD7D9 File Offset: 0x000AC7D9
		private void IgnoreComponentChanges(bool ignore)
		{
			this._ignoreComponentChangesCount += (ignore ? 1 : (-1));
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x000AD7F0 File Offset: 0x000AC7F0
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(Control));
			base.Initialize(component);
			if (this.RootDesigner != null)
			{
				this.RootDesigner.GetControlViewAndTag((Control)base.Component, out this._view, out this._tag);
				if (this._view != null)
				{
					this._view.ViewEvent += this.OnViewEvent;
				}
			}
			base.Expressions.Changed += this.OnExpressionsChanged;
			this.isWebControl = component is WebControl;
			this.UpdateExpressionValues(component);
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x000AD88A File Offset: 0x000AC88A
		public void Invalidate()
		{
			if (this.View != null)
			{
				this.Invalidate(this.View.GetBounds(null));
			}
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x000AD8A6 File Offset: 0x000AC8A6
		public void Invalidate(Rectangle rectangle)
		{
			if (this.View != null)
			{
				this.View.Invalidate(rectangle);
			}
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000AD8BC File Offset: 0x000AC8BC
		public static void InvokeTransactedChange(IComponent component, TransactedChangeCallback callback, object context, string description)
		{
			ControlDesigner.InvokeTransactedChange(component, callback, context, description, null);
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000AD8C8 File Offset: 0x000AC8C8
		public static void InvokeTransactedChange(IComponent component, TransactedChangeCallback callback, object context, string description, MemberDescriptor member)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			ControlDesigner.InvokeTransactedChange(component.Site, component, callback, context, description, member);
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x000AD8EC File Offset: 0x000AC8EC
		public static void InvokeTransactedChange(IServiceProvider serviceProvider, IComponent component, TransactedChangeCallback callback, object context, string description, MemberDescriptor member)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			if (serviceProvider == null)
			{
				throw new ArgumentException(SR.GetString("ControlDesigner_TransactedChangeRequiresServiceProvider"), "serviceProvider");
			}
			IDesignerHost designerHost = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
			using (DesignerTransaction designerTransaction = designerHost.CreateTransaction(description))
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)serviceProvider.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					try
					{
						componentChangeService.OnComponentChanging(component, member);
					}
					catch (CheckoutException ex)
					{
						if (ex == CheckoutException.Canceled)
						{
							return;
						}
						throw ex;
					}
				}
				ControlDesigner controlDesigner = designerHost.GetDesigner(component) as ControlDesigner;
				bool flag = false;
				try
				{
					if (controlDesigner != null)
					{
						controlDesigner.IgnoreComponentChanges(true);
					}
					if (callback(context))
					{
						if (controlDesigner != null)
						{
							flag = true;
							controlDesigner.IgnoreComponentChanges(false);
						}
						if (componentChangeService != null)
						{
							componentChangeService.OnComponentChanged(component, member, null, null);
						}
						TypeDescriptor.Refresh(component);
						designerTransaction.Commit();
					}
				}
				finally
				{
					if (controlDesigner != null && !flag)
					{
						controlDesigner.IgnoreComponentChanges(false);
					}
				}
			}
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000ADA14 File Offset: 0x000ACA14
		[Obsolete("The recommended alternative is DataBindings.Contains(string). The DataBindings collection allows more control of the databindings associated with the control. http://go.microsoft.com/fwlink/?linkid=14202")]
		public bool IsPropertyBound(string propName)
		{
			return base.DataBindings[propName] != null;
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000ADA28 File Offset: 0x000ACA28
		public virtual void OnAutoFormatApplied(DesignerAutoFormat appliedAutoFormat)
		{
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x000ADA2C File Offset: 0x000ACA2C
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["ID"];
			if (propertyDescriptor != null)
			{
				properties["ID"] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, ControlDesigner.emptyAttrs);
			}
			propertyDescriptor = (PropertyDescriptor)properties["SkinID"];
			if (propertyDescriptor != null)
			{
				properties["SkinID"] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[]
				{
					new TypeConverterAttribute(typeof(SkinIDTypeConverter))
				});
			}
			if (this.InTemplateMode)
			{
				if (this.HidePropertiesInTemplateMode)
				{
					this.HideAllPropertiesExceptID(properties);
				}
				propertyDescriptor = (PropertyDescriptor)properties["ID"];
				if (propertyDescriptor != null)
				{
					properties[propertyDescriptor.Name] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { ReadOnlyAttribute.Yes });
				}
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000ADB08 File Offset: 0x000ACB08
		[Obsolete("The recommended alternative is to handle the Changed event on the DataBindings collection. The DataBindings collection allows more control of the databindings associated with the control. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected override void OnBindingsCollectionChanged(string propName)
		{
			if (this.Tag == null)
			{
				return;
			}
			DataBindingCollection dataBindings = base.DataBindings;
			if (propName != null)
			{
				DataBinding dataBinding = dataBindings[propName];
				string text = propName.Replace('.', '-');
				if (dataBinding == null)
				{
					this.Tag.RemoveAttribute(text);
					return;
				}
				string text2 = "<%# " + dataBinding.Expression + " %>";
				this.Tag.SetAttribute(text, text2);
				if (text.IndexOf('-') < 0)
				{
					this.ResetPropertyValue(text, false);
					return;
				}
			}
			else
			{
				string[] removedBindings = dataBindings.RemovedBindings;
				foreach (string text3 in removedBindings)
				{
					string text4 = text3.Replace('.', '-');
					this.Tag.RemoveAttribute(text4);
				}
				foreach (object obj in dataBindings)
				{
					DataBinding dataBinding2 = (DataBinding)obj;
					string text5 = "<%# " + dataBinding2.Expression + " %>";
					string text6 = dataBinding2.PropertyName.Replace('.', '-');
					this.Tag.SetAttribute(text6, text5);
					if (text6.IndexOf('-') < 0)
					{
						this.ResetPropertyValue(text6, false);
					}
				}
			}
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x000ADC5C File Offset: 0x000ACC5C
		protected virtual void OnClick(DesignerRegionMouseEventArgs e)
		{
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x000ADC60 File Offset: 0x000ACC60
		public virtual void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
		{
			if (this.IsIgnoringComponentChanges)
			{
				return;
			}
			IComponent component = base.Component;
			if (base.DesignTimeElementInternal == null)
			{
				return;
			}
			MemberDescriptor member = ce.Member;
			if (member != null)
			{
				Type type = Type.GetType("System.ComponentModel.ReflectPropertyDescriptor, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
				if (member.GetType() != type || (ce.NewValue != null && ce.NewValue == ce.OldValue))
				{
					return;
				}
				if (((PropertyDescriptor)member).SerializationVisibility != DesignerSerializationVisibility.Hidden)
				{
					this.IsDirtyInternal = true;
					PersistenceModeAttribute persistenceModeAttribute = (PersistenceModeAttribute)member.Attributes[typeof(PersistenceModeAttribute)];
					PersistenceMode mode = persistenceModeAttribute.Mode;
					if (mode == PersistenceMode.Attribute || mode == PersistenceMode.InnerDefaultProperty || mode == PersistenceMode.EncodedInnerDefaultProperty)
					{
						string name = member.Name;
						if (ce.Component == base.Component)
						{
							if (base.DataBindings.Contains(name))
							{
								base.DataBindings.Remove(name, false);
								this.RemoveTagAttribute(name, true);
							}
							if (base.Expressions.Contains(name))
							{
								ExpressionBinding expressionBinding = base.Expressions[name];
								if (!expressionBinding.Generated)
								{
									base.Expressions.Remove(name, false);
									this.RemoveTagAttribute(name, true);
								}
								this._expressionsChanged = true;
							}
						}
						Control control = (Control)ce.Component;
						IDesignerHost designerHost = null;
						if (control.Site != null)
						{
							designerHost = (IDesignerHost)control.Site.GetService(typeof(IDesignerHost));
						}
						if (designerHost != null)
						{
							ArrayList controlPersistedAttribute = ControlSerializer.GetControlPersistedAttribute(control, (PropertyDescriptor)member, designerHost);
							this.PersistAttributes(controlPersistedAttribute);
						}
					}
				}
			}
			else
			{
				this.IsDirtyInternal = true;
				Control control2 = (Control)ce.Component;
				IDesignerHost designerHost2 = null;
				if (control2.Site != null)
				{
					designerHost2 = (IDesignerHost)control2.Site.GetService(typeof(IDesignerHost));
				}
				foreach (object obj in base.Expressions.RemovedBindings)
				{
					string text = (string)obj;
					object obj2;
					PropertyDescriptor complexProperty = ControlDesigner.GetComplexProperty(base.Component, text, out obj2);
					if (complexProperty != null)
					{
						this.IgnoreComponentChanges(true);
						try
						{
							complexProperty.ResetValue(obj2);
						}
						finally
						{
							this.IgnoreComponentChanges(false);
						}
					}
				}
				if (designerHost2 != null)
				{
					ArrayList controlPersistedAttributes = ControlSerializer.GetControlPersistedAttributes(control2, designerHost2);
					this.PersistAttributes(controlPersistedAttributes);
				}
				foreach (object obj3 in base.DataBindings)
				{
					DataBinding dataBinding = (DataBinding)obj3;
					if (dataBinding.PropertyName.IndexOf('.') < 0)
					{
						this.ResetPropertyValue(dataBinding.PropertyName, false);
					}
				}
				base.OnBindingsCollectionChangedInternal(null);
				this._expressionsChanged = true;
			}
			if (this._expressionsChanged)
			{
				this.UpdateExpressionValues(base.Component);
			}
			this.UpdateDesignTimeHtml();
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x000ADF54 File Offset: 0x000ACF54
		public virtual void OnComponentChanging(object sender, ComponentChangingEventArgs ce)
		{
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000ADF56 File Offset: 0x000ACF56
		[Obsolete("The recommended alternative is OnComponentChanged(). OnComponentChanged is called when any property of the control is changed. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void OnControlResize()
		{
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000ADF58 File Offset: 0x000ACF58
		private void OnExpressionsChanged(object sender, EventArgs e)
		{
			this._expressionsChanged = true;
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x000ADF61 File Offset: 0x000ACF61
		protected virtual void OnPaint(PaintEventArgs e)
		{
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x000ADF64 File Offset: 0x000ACF64
		private void OnViewEvent(object sender, ViewEventArgs e)
		{
			if (e.EventType == ViewEvent.Click)
			{
				this.OnClick((DesignerRegionMouseEventArgs)e.EventArgs);
				return;
			}
			if (e.EventType == ViewEvent.Paint)
			{
				this.OnPaint((PaintEventArgs)e.EventArgs);
				return;
			}
			if (e.EventType == ViewEvent.TemplateModeChanged)
			{
				TemplateModeChangedEventArgs templateModeChangedEventArgs = (TemplateModeChangedEventArgs)e.EventArgs;
				this._inTemplateMode = templateModeChangedEventArgs.NewTemplateGroup != null;
				TypeDescriptor.Refresh(base.Component);
			}
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x000ADFE8 File Offset: 0x000ACFE8
		private void PersistAttributes(ArrayList attributes)
		{
			foreach (object obj in attributes)
			{
				Triplet triplet = (Triplet)obj;
				string text = Convert.ToString(triplet.Second, CultureInfo.InvariantCulture);
				string text2 = triplet.First.ToString();
				if (text2 == null || text2.Length > 0)
				{
					text = text2 + ':' + text;
				}
				if (triplet.Third == null)
				{
					this.RemoveTagAttribute(text, true);
				}
				else
				{
					string text3 = Convert.ToString(triplet.Third, CultureInfo.InvariantCulture);
					this.SetTagAttribute(text, text3, true);
				}
			}
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x000AE0A0 File Offset: 0x000AD0A0
		[Obsolete("Use of this method is not recommended because resizing is handled by the OnComponentChanged() method. http://go.microsoft.com/fwlink/?linkid=14202")]
		public void RaiseResizeEvent()
		{
			this.OnControlResize();
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000AE0A8 File Offset: 0x000AD0A8
		public void RegisterClone(object original, object clone)
		{
			if (original == null)
			{
				throw new ArgumentNullException("original");
			}
			if (clone == null)
			{
				throw new ArgumentNullException("clone");
			}
			ControlBuilder controlBuilder = ((IControlBuilderAccessor)base.Component).ControlBuilder;
			if (controlBuilder != null)
			{
				ObjectPersistData objectPersistData = controlBuilder.GetObjectPersistData();
				objectPersistData.BuiltObjects[clone] = objectPersistData.BuiltObjects[original];
			}
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000AE104 File Offset: 0x000AD104
		private void ResetPropertyValue(string property, bool useInstance)
		{
			PropertyDescriptor propertyDescriptor;
			if (useInstance)
			{
				propertyDescriptor = TypeDescriptor.GetProperties(base.Component)[property];
			}
			else
			{
				propertyDescriptor = TypeDescriptor.GetProperties(base.Component.GetType())[property];
			}
			if (propertyDescriptor != null)
			{
				this.IgnoreComponentChanges(true);
				try
				{
					propertyDescriptor.ResetValue(base.Component);
				}
				finally
				{
					this.IgnoreComponentChanges(false);
				}
			}
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000AE174 File Offset: 0x000AD174
		private void RemoveTagAttribute(string name, bool ignoreCase)
		{
			if (this.Tag != null)
			{
				this.Tag.RemoveAttribute(name);
				return;
			}
			this.BehaviorInternal.RemoveAttribute(name, ignoreCase);
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000AE198 File Offset: 0x000AD198
		public virtual void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000AE19A File Offset: 0x000AD19A
		protected void SetRegionContent(EditableDesignerRegion region, string content)
		{
			if (this.View != null)
			{
				this.View.SetRegionContent(region, content);
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000AE1B1 File Offset: 0x000AD1B1
		private void SetTagAttribute(string name, object value, bool ignoreCase)
		{
			if (this.Tag != null)
			{
				this.Tag.SetAttribute(name, value.ToString());
				return;
			}
			this.BehaviorInternal.SetAttribute(name, value, ignoreCase);
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x000AE1DC File Offset: 0x000AD1DC
		protected void SetViewFlags(ViewFlags viewFlags, bool setFlag)
		{
			if (this.View != null)
			{
				this.View.SetFlags(viewFlags, setFlag);
			}
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x000AE1F4 File Offset: 0x000AD1F4
		public virtual void UpdateDesignTimeHtml()
		{
			if (this.View != null)
			{
				this.View.Update();
				return;
			}
			if (this.ReadOnlyInternal)
			{
				IHtmlControlDesignerBehavior behaviorInternal = this.BehaviorInternal;
				if (behaviorInternal != null)
				{
					((IControlDesignerBehavior)behaviorInternal).DesignTimeHtml = this.GetDesignTimeHtml();
				}
			}
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x000AE238 File Offset: 0x000AD238
		private void UpdateExpressionValues(IComponent target)
		{
			IExpressionsAccessor expressionsAccessor = target as IExpressionsAccessor;
			TemplateControl templateControl = null;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				templateControl = designerHost.RootComponent as TemplateControl;
			}
			foreach (object obj in expressionsAccessor.Expressions)
			{
				ExpressionBinding expressionBinding = (ExpressionBinding)obj;
				if (!expressionBinding.Generated)
				{
					string propertyName = expressionBinding.PropertyName;
					object obj2;
					PropertyDescriptor complexProperty = ControlDesigner.GetComplexProperty(target, propertyName, out obj2);
					if (complexProperty != null)
					{
						this.IgnoreComponentChanges(true);
						try
						{
							ExpressionEditor expressionEditor = ExpressionEditor.GetExpressionEditor(expressionBinding.ExpressionPrefix, target.Site);
							if (expressionEditor != null)
							{
								object obj3 = this.EnsureParsedExpression(templateControl, expressionBinding, expressionBinding.ParsedExpressionData);
								object obj4 = expressionEditor.EvaluateExpression(expressionBinding.Expression, obj3, complexProperty.PropertyType, target.Site);
								if (obj4 != null)
								{
									if (obj4 is string)
									{
										TypeConverter converter = complexProperty.Converter;
										if (converter != null && converter.CanConvertFrom(typeof(string)))
										{
											obj4 = converter.ConvertFromInvariantString((string)obj4);
										}
									}
									complexProperty.SetValue(obj2, obj4);
								}
								else
								{
									complexProperty.SetValue(obj2, SR.GetString("ExpressionEditor_ExpressionBound", new object[] { expressionBinding.Expression }));
								}
							}
							else
							{
								complexProperty.SetValue(obj2, SR.GetString("ExpressionEditor_ExpressionBound", new object[] { expressionBinding.Expression }));
							}
						}
						catch
						{
						}
						finally
						{
							this.IgnoreComponentChanges(false);
						}
					}
				}
			}
			this._expressionsChanged = false;
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x000AE428 File Offset: 0x000AD428
		internal bool UseRegions(DesignerRegionCollection regions, ITemplate componentTemplate)
		{
			return this.UseRegionsCore(regions) && componentTemplate != null;
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x000AE44C File Offset: 0x000AD44C
		internal bool UseRegions(DesignerRegionCollection regions, ITemplate componentTemplate, ITemplate viewControlTemplate)
		{
			bool flag = componentTemplate == null && viewControlTemplate != null;
			return this.UseRegionsCore(regions) && !flag;
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x000AE47C File Offset: 0x000AD47C
		private bool UseRegionsCore(DesignerRegionCollection regions)
		{
			return regions != null && this.View != null && this.View.SupportsRegions;
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x000AE4A4 File Offset: 0x000AD4A4
		internal static void VerifyInitializeArgument(IComponent component, Type expectedType)
		{
			if (!expectedType.IsInstanceOfType(component))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("ControlDesigner_ArgumentMustBeOfType"), new object[] { expectedType.FullName }), "component");
			}
		}

		// Token: 0x04001749 RID: 5961
		internal static readonly string ErrorDesignTimeHtmlTemplate = "<table cellpadding=\"4\" cellspacing=\"0\" style=\"font: messagebox; color: buttontext; background-color: buttonface; border: solid 1px; border-top-color: buttonhighlight; border-left-color: buttonhighlight; border-bottom-color: buttonshadow; border-right-color: buttonshadow\">\r\n                <tr><td nowrap><span style=\"font-weight: bold; color: red\">{0}</span> - {1}</td></tr>\r\n                <tr><td>{2}</td></tr>\r\n              </table>";

		// Token: 0x0400174A RID: 5962
		private static readonly string PlaceHolderDesignTimeHtmlTemplate = "<table cellpadding=4 cellspacing=0 style=\"font:messagebox;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n              <tr><td nowrap><span style=\"font-weight:bold\">{0}</span> - {1}</td></tr>\r\n              <tr><td>{2}</td></tr>\r\n            </table>";

		// Token: 0x0400174B RID: 5963
		private bool isWebControl;

		// Token: 0x0400174C RID: 5964
		private bool readOnly = true;

		// Token: 0x0400174D RID: 5965
		private bool fDirty;

		// Token: 0x0400174E RID: 5966
		private int _ignoreComponentChangesCount;

		// Token: 0x0400174F RID: 5967
		private bool _inTemplateMode;

		// Token: 0x04001750 RID: 5968
		private Control _viewControl;

		// Token: 0x04001751 RID: 5969
		private bool _viewControlCreated;

		// Token: 0x04001752 RID: 5970
		private IControlDesignerTag _tag;

		// Token: 0x04001753 RID: 5971
		private IControlDesignerView _view;

		// Token: 0x04001754 RID: 5972
		private ControlDesignerState _designerState;

		// Token: 0x04001755 RID: 5973
		private bool _expressionsChanged;

		// Token: 0x04001756 RID: 5974
		private string _localizedInnerContent;

		// Token: 0x04001757 RID: 5975
		private static readonly Attribute[] emptyAttrs = new Attribute[0];

		// Token: 0x04001758 RID: 5976
		private static readonly Attribute[] nonBrowsableAttrs = new Attribute[] { BrowsableAttribute.No };

		// Token: 0x0200032F RID: 815
		internal class ControlDesignerActionList : DesignerActionList
		{
			// Token: 0x06001EE0 RID: 7904 RVA: 0x000AE53C File Offset: 0x000AD53C
			public ControlDesignerActionList(ControlDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
			}

			// Token: 0x1700056D RID: 1389
			// (get) Token: 0x06001EE1 RID: 7905 RVA: 0x000AE551 File Offset: 0x000AD551
			// (set) Token: 0x06001EE2 RID: 7906 RVA: 0x000AE554 File Offset: 0x000AD554
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

			// Token: 0x06001EE3 RID: 7907 RVA: 0x000AE558 File Offset: 0x000AD558
			private bool DataBindingsCallback(object context)
			{
				Control control = (Control)this._parent.Component;
				ISite site = control.Site;
				DataBindingsDialog dataBindingsDialog = new DataBindingsDialog(site, control);
				DialogResult dialogResult = UIServiceHelper.ShowDialog(site, dataBindingsDialog);
				return dialogResult == DialogResult.OK;
			}

			// Token: 0x06001EE4 RID: 7908 RVA: 0x000AE591 File Offset: 0x000AD591
			public void EditDataBindings()
			{
				ControlDesigner.InvokeTransactedChange(this._parent.Component, new TransactedChangeCallback(this.DataBindingsCallback), null, SR.GetString("Designer_DataBindingsVerb"));
				this._parent.UpdateDesignTimeHtml();
			}

			// Token: 0x06001EE5 RID: 7909 RVA: 0x000AE5C8 File Offset: 0x000AD5C8
			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				if (this._parent.SupportsDataBindings && this._parent.DataBindingsEnabled)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "EditDataBindings", SR.GetString("Designer_DataBindingsVerb"), string.Empty, SR.GetString("Designer_DataBindingsVerbDesc"), true));
				}
				return designerActionItemCollection;
			}

			// Token: 0x04001759 RID: 5977
			private ControlDesigner _parent;
		}

		// Token: 0x02000330 RID: 816
		// (Invoke) Token: 0x06001EE7 RID: 7911
		internal delegate DesignerAutoFormat CreateAutoFormatDelegate(DataRow schemeData);
	}
}

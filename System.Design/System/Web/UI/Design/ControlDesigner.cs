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
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ControlDesigner : HtmlControlDesigner
	{
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

		public virtual bool AllowResize
		{
			get
			{
				return this.IsWebControl;
			}
		}

		public virtual DesignerAutoFormatCollection AutoFormats
		{
			get
			{
				return new DesignerAutoFormatCollection();
			}
		}

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

		[Obsolete("The recommended alternative is SetViewFlags(ViewFlags.DesignTimeHtmlRequiresLoadComplete, true). http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual bool DesignTimeHtmlRequiresLoadComplete
		{
			get
			{
				return false;
			}
		}

		protected internal virtual bool HidePropertiesInTemplateMode
		{
			get
			{
				return true;
			}
		}

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

		protected bool InTemplateMode
		{
			get
			{
				return this._inTemplateMode;
			}
		}

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

		internal bool IsIgnoringComponentChanges
		{
			get
			{
				return this._ignoreComponentChangesCount > 0;
			}
		}

		internal bool IsWebControl
		{
			get
			{
				return this.isWebControl;
			}
		}

		internal string LocalizedInnerContent
		{
			get
			{
				return this._localizedInnerContent;
			}
		}

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

		private bool SupportsDataBindings
		{
			get
			{
				BindableAttribute bindableAttribute = (BindableAttribute)TypeDescriptor.GetAttributes(base.Component)[typeof(BindableAttribute)];
				return bindableAttribute != null && bindableAttribute.Bindable;
			}
		}

		protected IControlDesignerTag Tag
		{
			get
			{
				return this._tag;
			}
		}

		public virtual TemplateGroupCollection TemplateGroups
		{
			get
			{
				return new TemplateGroupCollection();
			}
		}

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

		internal IControlDesignerView View
		{
			get
			{
				return this._view;
			}
		}

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

		protected virtual bool Visible
		{
			get
			{
				return true;
			}
		}

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

		protected string CreatePlaceHolderDesignTimeHtml()
		{
			return this.CreatePlaceHolderDesignTimeHtml(null);
		}

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

		protected string CreateErrorDesignTimeHtml(string errorMessage)
		{
			return this.CreateErrorDesignTimeHtml(errorMessage, null);
		}

		protected string CreateErrorDesignTimeHtml(string errorMessage, Exception e)
		{
			return ControlDesigner.CreateErrorDesignTimeHtml(errorMessage, e, base.Component);
		}

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

		internal string CreateInvalidParentDesignTimeHtml(Type controlType, Type requiredParentType)
		{
			return this.CreateErrorDesignTimeHtml(SR.GetString("Control_CanOnlyBePlacedInside", new object[] { controlType.Name, requiredParentType.Name }));
		}

		private Control CreateViewControlInternal()
		{
			Control control = (Control)base.Component;
			Control control2 = this.CreateViewControl();
			((IControlDesignerAccessor)control2).SetOwnerControl(control);
			this.UpdateExpressionValues(control2);
			return control2;
		}

		protected virtual Control CreateViewControl()
		{
			return this.CreateClonedControl((IDesignerHost)this.GetService(typeof(IDesignerHost)), true);
		}

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

		public Rectangle GetBounds()
		{
			if (this.View != null)
			{
				return this.View.GetBounds(null);
			}
			return Rectangle.Empty;
		}

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

		public virtual string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			return this.GetDesignTimeHtml();
		}

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

		public virtual string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			return string.Empty;
		}

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

		protected virtual string GetErrorDesignTimeHtml(Exception e)
		{
			return this.CreateErrorDesignTimeHtml(SR.GetString("ControlDesigner_UnhandledException"), e);
		}

		[Obsolete("The recommended alternative is GetPersistenceContent(). http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual string GetPersistInnerHtml()
		{
			return this.GetPersistInnerHtmlInternal();
		}

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

		public virtual string GetPersistenceContent()
		{
			return this.GetPersistInnerHtml();
		}

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

		private void IgnoreComponentChanges(bool ignore)
		{
			this._ignoreComponentChangesCount += (ignore ? 1 : (-1));
		}

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

		public void Invalidate()
		{
			if (this.View != null)
			{
				this.Invalidate(this.View.GetBounds(null));
			}
		}

		public void Invalidate(Rectangle rectangle)
		{
			if (this.View != null)
			{
				this.View.Invalidate(rectangle);
			}
		}

		public static void InvokeTransactedChange(IComponent component, TransactedChangeCallback callback, object context, string description)
		{
			ControlDesigner.InvokeTransactedChange(component, callback, context, description, null);
		}

		public static void InvokeTransactedChange(IComponent component, TransactedChangeCallback callback, object context, string description, MemberDescriptor member)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			ControlDesigner.InvokeTransactedChange(component.Site, component, callback, context, description, member);
		}

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

		[Obsolete("The recommended alternative is DataBindings.Contains(string). The DataBindings collection allows more control of the databindings associated with the control. http://go.microsoft.com/fwlink/?linkid=14202")]
		public bool IsPropertyBound(string propName)
		{
			return base.DataBindings[propName] != null;
		}

		public virtual void OnAutoFormatApplied(DesignerAutoFormat appliedAutoFormat)
		{
		}

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

		protected virtual void OnClick(DesignerRegionMouseEventArgs e)
		{
		}

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

		public virtual void OnComponentChanging(object sender, ComponentChangingEventArgs ce)
		{
		}

		[Obsolete("The recommended alternative is OnComponentChanged(). OnComponentChanged is called when any property of the control is changed. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void OnControlResize()
		{
		}

		private void OnExpressionsChanged(object sender, EventArgs e)
		{
			this._expressionsChanged = true;
		}

		protected virtual void OnPaint(PaintEventArgs e)
		{
		}

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

		[Obsolete("Use of this method is not recommended because resizing is handled by the OnComponentChanged() method. http://go.microsoft.com/fwlink/?linkid=14202")]
		public void RaiseResizeEvent()
		{
			this.OnControlResize();
		}

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

		private void RemoveTagAttribute(string name, bool ignoreCase)
		{
			if (this.Tag != null)
			{
				this.Tag.RemoveAttribute(name);
				return;
			}
			this.BehaviorInternal.RemoveAttribute(name, ignoreCase);
		}

		public virtual void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
		}

		protected void SetRegionContent(EditableDesignerRegion region, string content)
		{
			if (this.View != null)
			{
				this.View.SetRegionContent(region, content);
			}
		}

		private void SetTagAttribute(string name, object value, bool ignoreCase)
		{
			if (this.Tag != null)
			{
				this.Tag.SetAttribute(name, value.ToString());
				return;
			}
			this.BehaviorInternal.SetAttribute(name, value, ignoreCase);
		}

		protected void SetViewFlags(ViewFlags viewFlags, bool setFlag)
		{
			if (this.View != null)
			{
				this.View.SetFlags(viewFlags, setFlag);
			}
		}

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

		internal bool UseRegions(DesignerRegionCollection regions, ITemplate componentTemplate)
		{
			return this.UseRegionsCore(regions) && componentTemplate != null;
		}

		internal bool UseRegions(DesignerRegionCollection regions, ITemplate componentTemplate, ITemplate viewControlTemplate)
		{
			bool flag = componentTemplate == null && viewControlTemplate != null;
			return this.UseRegionsCore(regions) && !flag;
		}

		private bool UseRegionsCore(DesignerRegionCollection regions)
		{
			return regions != null && this.View != null && this.View.SupportsRegions;
		}

		internal static void VerifyInitializeArgument(IComponent component, Type expectedType)
		{
			if (!expectedType.IsInstanceOfType(component))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("ControlDesigner_ArgumentMustBeOfType"), new object[] { expectedType.FullName }), "component");
			}
		}

		internal static readonly string ErrorDesignTimeHtmlTemplate = "<table cellpadding=\"4\" cellspacing=\"0\" style=\"font: messagebox; color: buttontext; background-color: buttonface; border: solid 1px; border-top-color: buttonhighlight; border-left-color: buttonhighlight; border-bottom-color: buttonshadow; border-right-color: buttonshadow\">\r\n                <tr><td nowrap><span style=\"font-weight: bold; color: red\">{0}</span> - {1}</td></tr>\r\n                <tr><td>{2}</td></tr>\r\n              </table>";

		private static readonly string PlaceHolderDesignTimeHtmlTemplate = "<table cellpadding=4 cellspacing=0 style=\"font:messagebox;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n              <tr><td nowrap><span style=\"font-weight:bold\">{0}</span> - {1}</td></tr>\r\n              <tr><td>{2}</td></tr>\r\n            </table>";

		private bool isWebControl;

		private bool readOnly = true;

		private bool fDirty;

		private int _ignoreComponentChangesCount;

		private bool _inTemplateMode;

		private Control _viewControl;

		private bool _viewControlCreated;

		private IControlDesignerTag _tag;

		private IControlDesignerView _view;

		private ControlDesignerState _designerState;

		private bool _expressionsChanged;

		private string _localizedInnerContent;

		private static readonly Attribute[] emptyAttrs = new Attribute[0];

		private static readonly Attribute[] nonBrowsableAttrs = new Attribute[] { BrowsableAttribute.No };

		internal class ControlDesignerActionList : DesignerActionList
		{
			public ControlDesignerActionList(ControlDesigner parent)
				: base(parent.Component)
			{
				this._parent = parent;
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

			private bool DataBindingsCallback(object context)
			{
				Control control = (Control)this._parent.Component;
				ISite site = control.Site;
				DataBindingsDialog dataBindingsDialog = new DataBindingsDialog(site, control);
				DialogResult dialogResult = UIServiceHelper.ShowDialog(site, dataBindingsDialog);
				return dialogResult == DialogResult.OK;
			}

			public void EditDataBindings()
			{
				ControlDesigner.InvokeTransactedChange(this._parent.Component, new TransactedChangeCallback(this.DataBindingsCallback), null, SR.GetString("Designer_DataBindingsVerb"));
				this._parent.UpdateDesignTimeHtml();
			}

			public override DesignerActionItemCollection GetSortedActionItems()
			{
				DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
				if (this._parent.SupportsDataBindings && this._parent.DataBindingsEnabled)
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "EditDataBindings", SR.GetString("Designer_DataBindingsVerb"), string.Empty, SR.GetString("Designer_DataBindingsVerbDesc"), true));
				}
				return designerActionItemCollection;
			}

			private ControlDesigner _parent;
		}

		internal delegate DesignerAutoFormat CreateAutoFormatDelegate(DataRow schemeData);
	}
}

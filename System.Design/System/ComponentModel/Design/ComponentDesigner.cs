using System;
using System.Collections;
using System.Configuration;
using System.Design;
using System.Globalization;
using System.Windows.Forms.Design;
using Microsoft.Internal.Performance;

namespace System.ComponentModel.Design
{
	public class ComponentDesigner : ITreeDesigner, IDesigner, IDisposable, IDesignerFilter, IComponentInitializer
	{
		public virtual DesignerActionListCollection ActionLists
		{
			get
			{
				if (this.actionLists == null)
				{
					this.actionLists = new DesignerActionListCollection();
				}
				return this.actionLists;
			}
		}

		public virtual ICollection AssociatedComponents
		{
			get
			{
				return new IComponent[0];
			}
		}

		internal virtual bool CanBeAssociatedWith(IDesigner parentDesigner)
		{
			return true;
		}

		public IComponent Component
		{
			get
			{
				return this.component;
			}
		}

		protected bool Inherited
		{
			get
			{
				return !this.InheritanceAttribute.Equals(InheritanceAttribute.NotInherited);
			}
		}

		protected virtual IComponent ParentComponent
		{
			get
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				IComponent rootComponent = designerHost.RootComponent;
				if (rootComponent == this.Component)
				{
					return null;
				}
				return rootComponent;
			}
		}

		protected InheritanceAttribute InvokeGetInheritanceAttribute(ComponentDesigner toInvoke)
		{
			return toInvoke.InheritanceAttribute;
		}

		protected virtual InheritanceAttribute InheritanceAttribute
		{
			get
			{
				if (this.inheritanceAttribute == null)
				{
					IInheritanceService inheritanceService = (IInheritanceService)this.GetService(typeof(IInheritanceService));
					if (inheritanceService != null)
					{
						this.inheritanceAttribute = inheritanceService.GetInheritanceAttribute(this.Component);
					}
					else
					{
						this.inheritanceAttribute = InheritanceAttribute.Default;
					}
				}
				return this.inheritanceAttribute;
			}
		}

		private string SettingsKey
		{
			get
			{
				if (string.IsNullOrEmpty((string)this.ShadowProperties["SettingsKey"]))
				{
					IPersistComponentSettings persistComponentSettings = this.Component as IPersistComponentSettings;
					IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
					IComponent component = ((designerHost != null) ? designerHost.RootComponent : null);
					if (persistComponentSettings != null && component != null)
					{
						if (string.IsNullOrEmpty(persistComponentSettings.SettingsKey))
						{
							if (component != null && component != persistComponentSettings)
							{
								this.ShadowProperties["SettingsKey"] = string.Format(CultureInfo.CurrentCulture, "{0}.{1}", new object[]
								{
									component.Site.Name,
									this.Component.Site.Name
								});
							}
							else
							{
								this.ShadowProperties["SettingsKey"] = this.Component.Site.Name;
							}
						}
						persistComponentSettings.SettingsKey = this.ShadowProperties["SettingsKey"] as string;
						return persistComponentSettings.SettingsKey;
					}
				}
				return this.ShadowProperties["SettingsKey"] as string;
			}
			set
			{
				this.ShadowProperties["SettingsKey"] = value;
				this.settingsKeyExplicitlySet = true;
				IPersistComponentSettings persistComponentSettings = this.Component as IPersistComponentSettings;
				if (persistComponentSettings != null)
				{
					persistComponentSettings.SettingsKey = value;
				}
			}
		}

		protected ComponentDesigner.ShadowPropertyCollection ShadowProperties
		{
			get
			{
				if (this.shadowProperties == null)
				{
					this.shadowProperties = new ComponentDesigner.ShadowPropertyCollection(this);
				}
				return this.shadowProperties;
			}
		}

		public virtual DesignerVerbCollection Verbs
		{
			get
			{
				if (this.verbs == null)
				{
					this.verbs = new DesignerVerbCollection();
				}
				return this.verbs;
			}
		}

		private void OnComponentRename(object sender, ComponentRenameEventArgs e)
		{
			if (this.Component is IPersistComponentSettings)
			{
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				IComponent component = ((designerHost != null) ? designerHost.RootComponent : null);
				if (!this.settingsKeyExplicitlySet && (e.Component == this.Component || e.Component == component))
				{
					this.ResetSettingsKey();
				}
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ComponentDesigner()
		{
			this.Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRename -= this.OnComponentRename;
				}
				this.component = null;
			}
		}

		public virtual void DoDefaultAction()
		{
			IEventBindingService eventBindingService = (IEventBindingService)this.GetService(typeof(IEventBindingService));
			if (eventBindingService == null)
			{
				return;
			}
			ISelectionService selectionService = (ISelectionService)this.GetService(typeof(ISelectionService));
			if (selectionService == null)
			{
				return;
			}
			ICollection selectedComponents = selectionService.GetSelectedComponents();
			EventDescriptor eventDescriptor = null;
			string text = null;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			DesignerTransaction designerTransaction = null;
			try
			{
				foreach (object obj in selectedComponents)
				{
					if (obj is IComponent)
					{
						EventDescriptor defaultEvent = TypeDescriptor.GetDefaultEvent(obj);
						PropertyDescriptor propertyDescriptor = null;
						string text2 = null;
						bool flag = false;
						if (defaultEvent != null)
						{
							propertyDescriptor = eventBindingService.GetEventProperty(defaultEvent);
						}
						if (propertyDescriptor != null && !propertyDescriptor.IsReadOnly)
						{
							try
							{
								if (designerHost != null && designerTransaction == null)
								{
									designerTransaction = designerHost.CreateTransaction(SR.GetString("ComponentDesignerAddEvent", new object[] { defaultEvent.Name }));
								}
							}
							catch (CheckoutException ex)
							{
								if (ex == CheckoutException.Canceled)
								{
									return;
								}
								throw ex;
							}
							text2 = (string)propertyDescriptor.GetValue(obj);
							if (text2 == null)
							{
								flag = true;
								text2 = eventBindingService.CreateUniqueMethodName((IComponent)obj, defaultEvent);
							}
							else
							{
								flag = true;
								foreach (object obj2 in eventBindingService.GetCompatibleMethods(defaultEvent))
								{
									string text3 = (string)obj2;
									if (text2 == text3)
									{
										flag = false;
										break;
									}
								}
							}
							ComponentDesigner.codemarkers.CodeMarker(CodeMarkerEvent.perfFXBindEventDesignToCode);
							if (flag && propertyDescriptor != null)
							{
								propertyDescriptor.SetValue(obj, text2);
							}
							if (this.component == obj)
							{
								eventDescriptor = defaultEvent;
								text = text2;
							}
						}
					}
				}
			}
			catch (InvalidOperationException)
			{
				if (designerTransaction != null)
				{
					designerTransaction.Cancel();
					designerTransaction = null;
				}
			}
			finally
			{
				if (designerTransaction != null)
				{
					designerTransaction.Commit();
				}
			}
			if (text != null && eventDescriptor != null)
			{
				eventBindingService.ShowCode(this.component, eventDescriptor);
			}
		}

		internal bool IsRootDesigner
		{
			get
			{
				bool flag = false;
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null && this.component == designerHost.RootComponent)
				{
					flag = true;
				}
				return flag;
			}
		}

		public virtual void Initialize(IComponent component)
		{
			this.component = component;
			bool flag = false;
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null && component == designerHost.RootComponent)
			{
				flag = true;
			}
			IServiceContainer serviceContainer = component.Site as IServiceContainer;
			if (serviceContainer != null && this.GetService(typeof(DesignerCommandSet)) == null)
			{
				serviceContainer.AddService(typeof(DesignerCommandSet), new ComponentDesigner.CDDesignerCommandSet(this));
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRename += this.OnComponentRename;
			}
			if (flag || !this.InheritanceAttribute.Equals(InheritanceAttribute.NotInherited))
			{
				this.InitializeInheritedProperties(flag);
			}
		}

		public virtual void InitializeExistingComponent(IDictionary defaultValues)
		{
			this.InitializeNonDefault();
		}

		public virtual void InitializeNewComponent(IDictionary defaultValues)
		{
			DesignerActionUIService designerActionUIService = (DesignerActionUIService)this.GetService(typeof(DesignerActionUIService));
			if (designerActionUIService != null && designerActionUIService.ShouldAutoShow(this.Component))
			{
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (designerHost != null && designerHost.InTransaction)
				{
					designerHost.TransactionClosed += this.ShowDesignerActionUI;
				}
				else
				{
					designerActionUIService.ShowUI(this.Component);
				}
			}
			this.OnSetComponentDefaults();
		}

		private void ShowDesignerActionUI(object sender, DesignerTransactionCloseEventArgs e)
		{
			DesignerActionUIService designerActionUIService = (DesignerActionUIService)this.GetService(typeof(DesignerActionUIService));
			if (designerActionUIService != null)
			{
				designerActionUIService.ShowUI(this.Component);
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				designerHost.TransactionClosed -= this.ShowDesignerActionUI;
			}
		}

		private void InitializeInheritedProperties(bool rootComponent)
		{
			Hashtable hashtable = new Hashtable();
			if (!this.InheritanceAttribute.Equals(InheritanceAttribute.InheritedReadOnly))
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.Component);
				PropertyDescriptor[] array = new PropertyDescriptor[properties.Count];
				properties.CopyTo(array, 0);
				foreach (PropertyDescriptor propertyDescriptor in array)
				{
					if (!object.Equals(propertyDescriptor.Attributes[typeof(DesignOnlyAttribute)], DesignOnlyAttribute.Yes) && (propertyDescriptor.SerializationVisibility != DesignerSerializationVisibility.Hidden || propertyDescriptor.IsBrowsable) && (PropertyDescriptor)hashtable[propertyDescriptor.Name] == null)
					{
						hashtable[propertyDescriptor.Name] = new InheritedPropertyDescriptor(propertyDescriptor, this.component, rootComponent);
					}
				}
			}
			this.inheritedProps = hashtable;
			TypeDescriptor.Refresh(this.Component);
		}

		[Obsolete("This method has been deprecated. Use InitializeExistingComponent instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual void InitializeNonDefault()
		{
		}

		protected virtual object GetService(Type serviceType)
		{
			if (this.component != null)
			{
				ISite site = this.component.Site;
				if (site != null)
				{
					return site.GetService(serviceType);
				}
			}
			return null;
		}

		private Attribute[] NonBrowsableAttributes(EventDescriptor e)
		{
			Attribute[] array = new Attribute[e.Attributes.Count];
			e.Attributes.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && typeof(BrowsableAttribute).IsInstanceOfType(array[i]) && ((BrowsableAttribute)array[i]).Browsable)
				{
					array[i] = BrowsableAttribute.No;
					return array;
				}
			}
			Attribute[] array2 = new Attribute[array.Length + 1];
			Array.Copy(array, 0, array2, 0, array.Length);
			array2[array.Length] = BrowsableAttribute.No;
			return array2;
		}

		[Obsolete("This method has been deprecated. Use InitializeNewComponent instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual void OnSetComponentDefaults()
		{
			ISite site = this.Component.Site;
			if (site != null)
			{
				IComponent component = this.Component;
				PropertyDescriptor defaultProperty = TypeDescriptor.GetDefaultProperty(component);
				if (defaultProperty != null && defaultProperty.PropertyType.Equals(typeof(string)))
				{
					string text = (string)defaultProperty.GetValue(component);
					if (text == null || text.Length == 0)
					{
						defaultProperty.SetValue(component, site.Name);
					}
				}
			}
		}

		internal virtual void ShowContextMenu(int x, int y)
		{
			IMenuCommandService menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
			if (menuCommandService != null)
			{
				menuCommandService.ShowContextMenu(MenuCommands.SelectionMenu, x, y);
			}
		}

		protected virtual void PostFilterAttributes(IDictionary attributes)
		{
			if (attributes.Contains(typeof(InheritanceAttribute)))
			{
				this.inheritanceAttribute = attributes[typeof(InheritanceAttribute)] as InheritanceAttribute;
				return;
			}
			if (!this.InheritanceAttribute.Equals(InheritanceAttribute.NotInherited))
			{
				attributes[typeof(InheritanceAttribute)] = this.InheritanceAttribute;
			}
		}

		protected virtual void PostFilterEvents(IDictionary events)
		{
			if (this.InheritanceAttribute.Equals(InheritanceAttribute.InheritedReadOnly))
			{
				EventDescriptor[] array = new EventDescriptor[events.Values.Count];
				events.Values.CopyTo(array, 0);
				foreach (EventDescriptor eventDescriptor in array)
				{
					events[eventDescriptor.Name] = TypeDescriptor.CreateEvent(eventDescriptor.ComponentType, eventDescriptor, new Attribute[] { ReadOnlyAttribute.Yes });
				}
			}
		}

		protected virtual void PostFilterProperties(IDictionary properties)
		{
			if (this.inheritedProps != null)
			{
				bool flag = this.InheritanceAttribute.Equals(InheritanceAttribute.InheritedReadOnly);
				if (flag)
				{
					PropertyDescriptor[] array = new PropertyDescriptor[properties.Values.Count];
					properties.Values.CopyTo(array, 0);
					foreach (PropertyDescriptor propertyDescriptor in array)
					{
						properties[propertyDescriptor.Name] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { ReadOnlyAttribute.Yes });
					}
					return;
				}
				foreach (object obj in this.inheritedProps)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					InheritedPropertyDescriptor inheritedPropertyDescriptor = dictionaryEntry.Value as InheritedPropertyDescriptor;
					if (inheritedPropertyDescriptor != null)
					{
						PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)properties[dictionaryEntry.Key];
						if (propertyDescriptor2 != null)
						{
							inheritedPropertyDescriptor.PropertyDescriptor = propertyDescriptor2;
							properties[dictionaryEntry.Key] = inheritedPropertyDescriptor;
						}
					}
				}
			}
		}

		protected virtual void PreFilterAttributes(IDictionary attributes)
		{
		}

		protected virtual void PreFilterEvents(IDictionary events)
		{
		}

		protected virtual void PreFilterProperties(IDictionary properties)
		{
			if (this.Component is IPersistComponentSettings)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["SettingsKey"];
				if (propertyDescriptor != null)
				{
					properties["SettingsKey"] = TypeDescriptor.CreateProperty(typeof(ComponentDesigner), propertyDescriptor, new Attribute[0]);
				}
			}
		}

		protected void RaiseComponentChanged(MemberDescriptor member, object oldValue, object newValue)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanged(this.Component, member, oldValue, newValue);
			}
		}

		protected void RaiseComponentChanging(MemberDescriptor member)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanging(this.Component, member);
			}
		}

		private void ResetSettingsKey()
		{
			if (this.Component is IPersistComponentSettings)
			{
				this.SettingsKey = null;
				this.settingsKeyExplicitlySet = false;
			}
		}

		private bool ShouldSerializeSettingsKey()
		{
			IPersistComponentSettings persistComponentSettings = this.Component as IPersistComponentSettings;
			return persistComponentSettings != null && (this.settingsKeyExplicitlySet || (persistComponentSettings.SaveSettings && !string.IsNullOrEmpty(this.SettingsKey)));
		}

		void IDesignerFilter.PostFilterAttributes(IDictionary attributes)
		{
			this.PostFilterAttributes(attributes);
		}

		void IDesignerFilter.PostFilterEvents(IDictionary events)
		{
			this.PostFilterEvents(events);
		}

		void IDesignerFilter.PostFilterProperties(IDictionary properties)
		{
			this.PostFilterProperties(properties);
		}

		void IDesignerFilter.PreFilterAttributes(IDictionary attributes)
		{
			this.PreFilterAttributes(attributes);
		}

		void IDesignerFilter.PreFilterEvents(IDictionary events)
		{
			this.PreFilterEvents(events);
		}

		void IDesignerFilter.PreFilterProperties(IDictionary properties)
		{
			this.PreFilterProperties(properties);
		}

		ICollection ITreeDesigner.Children
		{
			get
			{
				ICollection associatedComponents = this.AssociatedComponents;
				IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				if (associatedComponents.Count > 0 && designerHost != null)
				{
					IDesigner[] array = new IDesigner[associatedComponents.Count];
					int num = 0;
					foreach (object obj in associatedComponents)
					{
						IComponent component = (IComponent)obj;
						array[num] = designerHost.GetDesigner(component);
						if (array[num] != null)
						{
							num++;
						}
					}
					if (num < array.Length)
					{
						IDesigner[] array2 = new IDesigner[num];
						Array.Copy(array, 0, array2, 0, num);
						array = array2;
					}
					return array;
				}
				return new object[0];
			}
		}

		IDesigner ITreeDesigner.Parent
		{
			get
			{
				IComponent parentComponent = this.ParentComponent;
				if (parentComponent != null)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						return designerHost.GetDesigner(parentComponent);
					}
				}
				return null;
			}
		}

		private IComponent component;

		private InheritanceAttribute inheritanceAttribute;

		private Hashtable inheritedProps;

		private DesignerVerbCollection verbs;

		private DesignerActionListCollection actionLists;

		private ComponentDesigner.ShadowPropertyCollection shadowProperties;

		private bool settingsKeyExplicitlySet;

		private static CodeMarkers codemarkers = CodeMarkers.Instance;

		private class CDDesignerCommandSet : DesignerCommandSet
		{
			public CDDesignerCommandSet(ComponentDesigner componentDesigner)
			{
				this.componentDesigner = componentDesigner;
			}

			public override ICollection GetCommands(string name)
			{
				if (name.Equals("Verbs"))
				{
					return this.componentDesigner.Verbs;
				}
				if (name.Equals("ActionLists"))
				{
					return this.componentDesigner.ActionLists;
				}
				return base.GetCommands(name);
			}

			private ComponentDesigner componentDesigner;
		}

		protected sealed class ShadowPropertyCollection
		{
			internal ShadowPropertyCollection(ComponentDesigner designer)
			{
				this.designer = designer;
			}

			public object this[string propertyName]
			{
				get
				{
					if (propertyName == null)
					{
						throw new ArgumentNullException("propertyName");
					}
					if (this.properties != null && this.properties.ContainsKey(propertyName))
					{
						return this.properties[propertyName];
					}
					PropertyDescriptor shadowedPropertyDescriptor = this.GetShadowedPropertyDescriptor(propertyName);
					return shadowedPropertyDescriptor.GetValue(this.designer.Component);
				}
				set
				{
					if (this.properties == null)
					{
						this.properties = new Hashtable();
					}
					this.properties[propertyName] = value;
				}
			}

			public bool Contains(string propertyName)
			{
				return this.properties != null && this.properties.ContainsKey(propertyName);
			}

			private PropertyDescriptor GetShadowedPropertyDescriptor(string propertyName)
			{
				if (this.descriptors == null)
				{
					this.descriptors = new Hashtable();
				}
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)this.descriptors[propertyName];
				if (propertyDescriptor == null)
				{
					propertyDescriptor = TypeDescriptor.GetProperties(this.designer.Component.GetType())[propertyName];
					if (propertyDescriptor == null)
					{
						throw new ArgumentException(SR.GetString("DesignerPropNotFound", new object[]
						{
							propertyName,
							this.designer.Component.GetType().FullName
						}));
					}
					this.descriptors[propertyName] = propertyDescriptor;
				}
				return propertyDescriptor;
			}

			internal bool ShouldSerializeValue(string propertyName, object defaultValue)
			{
				if (propertyName == null)
				{
					throw new ArgumentNullException("propertyName");
				}
				if (this.Contains(propertyName))
				{
					return !object.Equals(this[propertyName], defaultValue);
				}
				return this.GetShadowedPropertyDescriptor(propertyName).ShouldSerializeValue(this.designer.Component);
			}

			private ComponentDesigner designer;

			private Hashtable properties;

			private Hashtable descriptors;
		}
	}
}

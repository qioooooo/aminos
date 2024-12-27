using System;
using System.Collections;
using System.Configuration;
using System.Design;
using System.Globalization;
using System.Windows.Forms.Design;
using Microsoft.Internal.Performance;

namespace System.ComponentModel.Design
{
	// Token: 0x020000F7 RID: 247
	public class ComponentDesigner : ITreeDesigner, IDesigner, IDisposable, IDesignerFilter, IComponentInitializer
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x00027D7A File Offset: 0x00026D7A
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

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00027D95 File Offset: 0x00026D95
		public virtual ICollection AssociatedComponents
		{
			get
			{
				return new IComponent[0];
			}
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00027D9D File Offset: 0x00026D9D
		internal virtual bool CanBeAssociatedWith(IDesigner parentDesigner)
		{
			return true;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00027DA0 File Offset: 0x00026DA0
		public IComponent Component
		{
			get
			{
				return this.component;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00027DA8 File Offset: 0x00026DA8
		protected bool Inherited
		{
			get
			{
				return !this.InheritanceAttribute.Equals(InheritanceAttribute.NotInherited);
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00027DC0 File Offset: 0x00026DC0
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

		// Token: 0x06000A3C RID: 2620 RVA: 0x00027DF6 File Offset: 0x00026DF6
		protected InheritanceAttribute InvokeGetInheritanceAttribute(ComponentDesigner toInvoke)
		{
			return toInvoke.InheritanceAttribute;
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x00027E00 File Offset: 0x00026E00
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

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x00027E54 File Offset: 0x00026E54
		// (set) Token: 0x06000A3F RID: 2623 RVA: 0x00027F70 File Offset: 0x00026F70
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

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x00027FAB File Offset: 0x00026FAB
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

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x00027FC7 File Offset: 0x00026FC7
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

		// Token: 0x06000A42 RID: 2626 RVA: 0x00027FE4 File Offset: 0x00026FE4
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

		// Token: 0x06000A43 RID: 2627 RVA: 0x00028046 File Offset: 0x00027046
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00028058 File Offset: 0x00027058
		~ComponentDesigner()
		{
			this.Dispose(false);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00028088 File Offset: 0x00027088
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

		// Token: 0x06000A46 RID: 2630 RVA: 0x000280CC File Offset: 0x000270CC
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

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00028354 File Offset: 0x00027354
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

		// Token: 0x06000A48 RID: 2632 RVA: 0x00028390 File Offset: 0x00027390
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

		// Token: 0x06000A49 RID: 2633 RVA: 0x00028448 File Offset: 0x00027448
		public virtual void InitializeExistingComponent(IDictionary defaultValues)
		{
			this.InitializeNonDefault();
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00028450 File Offset: 0x00027450
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

		// Token: 0x06000A4B RID: 2635 RVA: 0x000284CC File Offset: 0x000274CC
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

		// Token: 0x06000A4C RID: 2636 RVA: 0x0002852C File Offset: 0x0002752C
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

		// Token: 0x06000A4D RID: 2637 RVA: 0x00028607 File Offset: 0x00027607
		[Obsolete("This method has been deprecated. Use InitializeExistingComponent instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual void InitializeNonDefault()
		{
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0002860C File Offset: 0x0002760C
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

		// Token: 0x06000A4F RID: 2639 RVA: 0x0002863C File Offset: 0x0002763C
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

		// Token: 0x06000A50 RID: 2640 RVA: 0x000286CC File Offset: 0x000276CC
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

		// Token: 0x06000A51 RID: 2641 RVA: 0x00028738 File Offset: 0x00027738
		internal virtual void ShowContextMenu(int x, int y)
		{
			IMenuCommandService menuCommandService = (IMenuCommandService)this.GetService(typeof(IMenuCommandService));
			if (menuCommandService != null)
			{
				menuCommandService.ShowContextMenu(MenuCommands.SelectionMenu, x, y);
			}
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0002876C File Offset: 0x0002776C
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

		// Token: 0x06000A53 RID: 2643 RVA: 0x000287D0 File Offset: 0x000277D0
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

		// Token: 0x06000A54 RID: 2644 RVA: 0x00028848 File Offset: 0x00027848
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

		// Token: 0x06000A55 RID: 2645 RVA: 0x00028960 File Offset: 0x00027960
		protected virtual void PreFilterAttributes(IDictionary attributes)
		{
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00028962 File Offset: 0x00027962
		protected virtual void PreFilterEvents(IDictionary events)
		{
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00028964 File Offset: 0x00027964
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

		// Token: 0x06000A58 RID: 2648 RVA: 0x000289B4 File Offset: 0x000279B4
		protected void RaiseComponentChanged(MemberDescriptor member, object oldValue, object newValue)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanged(this.Component, member, oldValue, newValue);
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x000289EC File Offset: 0x000279EC
		protected void RaiseComponentChanging(MemberDescriptor member)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.OnComponentChanging(this.Component, member);
			}
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00028A1F File Offset: 0x00027A1F
		private void ResetSettingsKey()
		{
			if (this.Component is IPersistComponentSettings)
			{
				this.SettingsKey = null;
				this.settingsKeyExplicitlySet = false;
			}
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00028A3C File Offset: 0x00027A3C
		private bool ShouldSerializeSettingsKey()
		{
			IPersistComponentSettings persistComponentSettings = this.Component as IPersistComponentSettings;
			return persistComponentSettings != null && (this.settingsKeyExplicitlySet || (persistComponentSettings.SaveSettings && !string.IsNullOrEmpty(this.SettingsKey)));
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00028A7C File Offset: 0x00027A7C
		void IDesignerFilter.PostFilterAttributes(IDictionary attributes)
		{
			this.PostFilterAttributes(attributes);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00028A85 File Offset: 0x00027A85
		void IDesignerFilter.PostFilterEvents(IDictionary events)
		{
			this.PostFilterEvents(events);
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00028A8E File Offset: 0x00027A8E
		void IDesignerFilter.PostFilterProperties(IDictionary properties)
		{
			this.PostFilterProperties(properties);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00028A97 File Offset: 0x00027A97
		void IDesignerFilter.PreFilterAttributes(IDictionary attributes)
		{
			this.PreFilterAttributes(attributes);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00028AA0 File Offset: 0x00027AA0
		void IDesignerFilter.PreFilterEvents(IDictionary events)
		{
			this.PreFilterEvents(events);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00028AA9 File Offset: 0x00027AA9
		void IDesignerFilter.PreFilterProperties(IDictionary properties)
		{
			this.PreFilterProperties(properties);
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00028AB4 File Offset: 0x00027AB4
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

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x00028B78 File Offset: 0x00027B78
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

		// Token: 0x04000D81 RID: 3457
		private IComponent component;

		// Token: 0x04000D82 RID: 3458
		private InheritanceAttribute inheritanceAttribute;

		// Token: 0x04000D83 RID: 3459
		private Hashtable inheritedProps;

		// Token: 0x04000D84 RID: 3460
		private DesignerVerbCollection verbs;

		// Token: 0x04000D85 RID: 3461
		private DesignerActionListCollection actionLists;

		// Token: 0x04000D86 RID: 3462
		private ComponentDesigner.ShadowPropertyCollection shadowProperties;

		// Token: 0x04000D87 RID: 3463
		private bool settingsKeyExplicitlySet;

		// Token: 0x04000D88 RID: 3464
		private static CodeMarkers codemarkers = CodeMarkers.Instance;

		// Token: 0x020000F9 RID: 249
		private class CDDesignerCommandSet : DesignerCommandSet
		{
			// Token: 0x06000A6A RID: 2666 RVA: 0x00028BF4 File Offset: 0x00027BF4
			public CDDesignerCommandSet(ComponentDesigner componentDesigner)
			{
				this.componentDesigner = componentDesigner;
			}

			// Token: 0x06000A6B RID: 2667 RVA: 0x00028C03 File Offset: 0x00027C03
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

			// Token: 0x04000D89 RID: 3465
			private ComponentDesigner componentDesigner;
		}

		// Token: 0x020000FA RID: 250
		protected sealed class ShadowPropertyCollection
		{
			// Token: 0x06000A6C RID: 2668 RVA: 0x00028C3E File Offset: 0x00027C3E
			internal ShadowPropertyCollection(ComponentDesigner designer)
			{
				this.designer = designer;
			}

			// Token: 0x1700016E RID: 366
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

			// Token: 0x06000A6F RID: 2671 RVA: 0x00028CC9 File Offset: 0x00027CC9
			public bool Contains(string propertyName)
			{
				return this.properties != null && this.properties.ContainsKey(propertyName);
			}

			// Token: 0x06000A70 RID: 2672 RVA: 0x00028CE4 File Offset: 0x00027CE4
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

			// Token: 0x06000A71 RID: 2673 RVA: 0x00028D7C File Offset: 0x00027D7C
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

			// Token: 0x04000D8A RID: 3466
			private ComponentDesigner designer;

			// Token: 0x04000D8B RID: 3467
			private Hashtable properties;

			// Token: 0x04000D8C RID: 3468
			private Hashtable descriptors;
		}
	}
}

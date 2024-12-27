using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x02000124 RID: 292
	public class DesignerActionService : IDisposable
	{
		// Token: 0x06000B98 RID: 2968 RVA: 0x0002D51C File Offset: 0x0002C51C
		public DesignerActionService(IServiceProvider serviceProvider)
		{
			if (serviceProvider != null)
			{
				this.serviceProvider = serviceProvider;
				IDesignerHost designerHost = (IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost));
				designerHost.AddService(typeof(DesignerActionService), this);
				IComponentChangeService componentChangeService = (IComponentChangeService)serviceProvider.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRemoved += this.OnComponentRemoved;
				}
				this.selSvc = (ISelectionService)serviceProvider.GetService(typeof(ISelectionService));
				ISelectionService selectionService = this.selSvc;
			}
			this.designerActionLists = new Hashtable();
			this.componentToVerbsEventHookedUp = new Hashtable();
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000B99 RID: 2969 RVA: 0x0002D5C3 File Offset: 0x0002C5C3
		// (remove) Token: 0x06000B9A RID: 2970 RVA: 0x0002D5DC File Offset: 0x0002C5DC
		public event DesignerActionListsChangedEventHandler DesignerActionListsChanged
		{
			add
			{
				this.designerActionListsChanged = (DesignerActionListsChangedEventHandler)Delegate.Combine(this.designerActionListsChanged, value);
			}
			remove
			{
				this.designerActionListsChanged = (DesignerActionListsChangedEventHandler)Delegate.Remove(this.designerActionListsChanged, value);
			}
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0002D5F8 File Offset: 0x0002C5F8
		public void Add(IComponent comp, DesignerActionListCollection designerActionListCollection)
		{
			if (comp == null)
			{
				throw new ArgumentNullException("comp");
			}
			if (designerActionListCollection == null)
			{
				throw new ArgumentNullException("designerActionListCollection");
			}
			DesignerActionListCollection designerActionListCollection2 = (DesignerActionListCollection)this.designerActionLists[comp];
			if (designerActionListCollection2 != null)
			{
				designerActionListCollection2.AddRange(designerActionListCollection);
			}
			else
			{
				this.designerActionLists.Add(comp, designerActionListCollection);
			}
			this.OnDesignerActionListsChanged(new DesignerActionListsChangedEventArgs(comp, DesignerActionListsChangedType.ActionListsAdded, this.GetComponentActions(comp)));
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0002D660 File Offset: 0x0002C660
		public void Add(IComponent comp, DesignerActionList actionList)
		{
			this.Add(comp, new DesignerActionListCollection(actionList));
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0002D670 File Offset: 0x0002C670
		public void Clear()
		{
			if (this.designerActionLists.Count == 0)
			{
				return;
			}
			ArrayList arrayList = new ArrayList(this.designerActionLists.Count);
			foreach (object obj in this.designerActionLists)
			{
				arrayList.Add(((DictionaryEntry)obj).Key);
			}
			this.designerActionLists.Clear();
			foreach (object obj2 in arrayList)
			{
				Component component = (Component)obj2;
				this.OnDesignerActionListsChanged(new DesignerActionListsChangedEventArgs(component, DesignerActionListsChangedType.ActionListsRemoved, this.GetComponentActions(component)));
			}
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0002D754 File Offset: 0x0002C754
		public bool Contains(IComponent comp)
		{
			if (comp == null)
			{
				throw new ArgumentNullException("comp");
			}
			return this.designerActionLists.Contains(comp);
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0002D770 File Offset: 0x0002C770
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0002D77C File Offset: 0x0002C77C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.serviceProvider != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.serviceProvider.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					designerHost.RemoveService(typeof(DesignerActionService));
				}
				IComponentChangeService componentChangeService = (IComponentChangeService)this.serviceProvider.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
				}
			}
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0002D7F2 File Offset: 0x0002C7F2
		public DesignerActionListCollection GetComponentActions(IComponent component)
		{
			return this.GetComponentActions(component, ComponentActionsType.All);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0002D7FC File Offset: 0x0002C7FC
		public virtual DesignerActionListCollection GetComponentActions(IComponent component, ComponentActionsType type)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
			switch (type)
			{
			case ComponentActionsType.All:
				this.GetComponentDesignerActions(component, designerActionListCollection);
				this.GetComponentServiceActions(component, designerActionListCollection);
				break;
			case ComponentActionsType.Component:
				this.GetComponentDesignerActions(component, designerActionListCollection);
				break;
			case ComponentActionsType.Service:
				this.GetComponentServiceActions(component, designerActionListCollection);
				break;
			}
			return designerActionListCollection;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0002D858 File Offset: 0x0002C858
		protected virtual void GetComponentDesignerActions(IComponent component, DesignerActionListCollection actionLists)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (actionLists == null)
			{
				throw new ArgumentNullException("actionLists");
			}
			IServiceContainer serviceContainer = component.Site as IServiceContainer;
			if (serviceContainer != null)
			{
				DesignerCommandSet designerCommandSet = (DesignerCommandSet)serviceContainer.GetService(typeof(DesignerCommandSet));
				if (designerCommandSet != null)
				{
					DesignerActionListCollection actionLists2 = designerCommandSet.ActionLists;
					if (actionLists2 != null)
					{
						actionLists.AddRange(actionLists2);
					}
					if (actionLists.Count == 0)
					{
						DesignerVerbCollection verbs = designerCommandSet.Verbs;
						if (verbs != null && verbs.Count != 0)
						{
							ArrayList arrayList = new ArrayList();
							bool flag = this.componentToVerbsEventHookedUp[component] == null;
							if (flag)
							{
								this.componentToVerbsEventHookedUp[component] = true;
							}
							foreach (object obj in verbs)
							{
								DesignerVerb designerVerb = (DesignerVerb)obj;
								if (flag)
								{
									designerVerb.CommandChanged += this.OnVerbStatusChanged;
								}
								if (designerVerb.Enabled && designerVerb.Visible)
								{
									arrayList.Add(designerVerb);
								}
							}
							if (arrayList.Count != 0)
							{
								DesignerActionVerbList designerActionVerbList = new DesignerActionVerbList((DesignerVerb[])arrayList.ToArray(typeof(DesignerVerb)));
								actionLists.Add(designerActionVerbList);
							}
						}
					}
					if (actionLists2 != null)
					{
						foreach (object obj2 in actionLists2)
						{
							DesignerActionList designerActionList = (DesignerActionList)obj2;
							DesignerActionItemCollection sortedActionItems = designerActionList.GetSortedActionItems();
							if (sortedActionItems == null || sortedActionItems.Count == 0)
							{
								actionLists.Remove(designerActionList);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0002DA20 File Offset: 0x0002CA20
		private void OnVerbStatusChanged(object sender, EventArgs args)
		{
			if (!this.reEntrantCode)
			{
				try
				{
					this.reEntrantCode = true;
					IComponent component = this.selSvc.PrimarySelection as IComponent;
					if (component != null)
					{
						IServiceContainer serviceContainer = component.Site as IServiceContainer;
						if (serviceContainer != null)
						{
							DesignerCommandSet designerCommandSet = (DesignerCommandSet)serviceContainer.GetService(typeof(DesignerCommandSet));
							foreach (object obj in designerCommandSet.Verbs)
							{
								DesignerVerb designerVerb = (DesignerVerb)obj;
								if (designerVerb == sender)
								{
									DesignerActionUIService designerActionUIService = (DesignerActionUIService)serviceContainer.GetService(typeof(DesignerActionUIService));
									if (designerActionUIService != null)
									{
										designerActionUIService.Refresh(component);
									}
								}
							}
						}
					}
				}
				finally
				{
					this.reEntrantCode = false;
				}
			}
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002DB04 File Offset: 0x0002CB04
		protected virtual void GetComponentServiceActions(IComponent component, DesignerActionListCollection actionLists)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (actionLists == null)
			{
				throw new ArgumentNullException("actionLists");
			}
			DesignerActionListCollection designerActionListCollection = (DesignerActionListCollection)this.designerActionLists[component];
			if (designerActionListCollection != null)
			{
				actionLists.AddRange(designerActionListCollection);
				foreach (object obj in designerActionListCollection)
				{
					DesignerActionList designerActionList = (DesignerActionList)obj;
					DesignerActionItemCollection sortedActionItems = designerActionList.GetSortedActionItems();
					if (sortedActionItems == null || sortedActionItems.Count == 0)
					{
						actionLists.Remove(designerActionList);
					}
				}
			}
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0002DBA8 File Offset: 0x0002CBA8
		private void OnComponentRemoved(object source, ComponentEventArgs ce)
		{
			this.Remove(ce.Component);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0002DBB6 File Offset: 0x0002CBB6
		private void OnDesignerActionListsChanged(DesignerActionListsChangedEventArgs e)
		{
			if (this.designerActionListsChanged != null)
			{
				this.designerActionListsChanged(this, e);
			}
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0002DBCD File Offset: 0x0002CBCD
		public void Remove(IComponent comp)
		{
			if (comp == null)
			{
				throw new ArgumentNullException("comp");
			}
			if (!this.designerActionLists.Contains(comp))
			{
				return;
			}
			this.designerActionLists.Remove(comp);
			this.OnDesignerActionListsChanged(new DesignerActionListsChangedEventArgs(comp, DesignerActionListsChangedType.ActionListsRemoved, this.GetComponentActions(comp)));
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0002DC0C File Offset: 0x0002CC0C
		public void Remove(DesignerActionList actionList)
		{
			if (actionList == null)
			{
				throw new ArgumentNullException("actionList");
			}
			foreach (object obj in this.designerActionLists.Keys)
			{
				IComponent component = (IComponent)obj;
				if (((DesignerActionListCollection)this.designerActionLists[component]).Contains(actionList))
				{
					this.Remove(component, actionList);
					break;
				}
			}
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0002DC94 File Offset: 0x0002CC94
		public void Remove(IComponent comp, DesignerActionList actionList)
		{
			if (comp == null)
			{
				throw new ArgumentNullException("comp");
			}
			if (actionList == null)
			{
				throw new ArgumentNullException("actionList");
			}
			if (!this.designerActionLists.Contains(comp))
			{
				return;
			}
			DesignerActionListCollection designerActionListCollection = (DesignerActionListCollection)this.designerActionLists[comp];
			if (!designerActionListCollection.Contains(actionList))
			{
				return;
			}
			if (designerActionListCollection.Count == 1)
			{
				this.Remove(comp);
				return;
			}
			ArrayList arrayList = new ArrayList(1);
			foreach (object obj in designerActionListCollection)
			{
				DesignerActionList designerActionList = (DesignerActionList)obj;
				if (actionList.Equals(designerActionList))
				{
					arrayList.Add(designerActionList);
				}
			}
			foreach (object obj2 in arrayList)
			{
				DesignerActionList designerActionList2 = (DesignerActionList)obj2;
				designerActionListCollection.Remove(designerActionList2);
			}
			this.OnDesignerActionListsChanged(new DesignerActionListsChangedEventArgs(comp, DesignerActionListsChangedType.ActionListsRemoved, this.GetComponentActions(comp)));
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000BAB RID: 2987 RVA: 0x0002DDB8 File Offset: 0x0002CDB8
		// (remove) Token: 0x06000BAC RID: 2988 RVA: 0x0002DDEC File Offset: 0x0002CDEC
		internal event DesignerActionUIStateChangeEventHandler DesignerActionUIStateChange
		{
			add
			{
				DesignerActionUIService designerActionUIService = (DesignerActionUIService)this.serviceProvider.GetService(typeof(DesignerActionUIService));
				if (designerActionUIService != null)
				{
					designerActionUIService.DesignerActionUIStateChange += value;
				}
			}
			remove
			{
				DesignerActionUIService designerActionUIService = (DesignerActionUIService)this.serviceProvider.GetService(typeof(DesignerActionUIService));
				if (designerActionUIService != null)
				{
					designerActionUIService.DesignerActionUIStateChange -= value;
				}
			}
		}

		// Token: 0x04000E47 RID: 3655
		private Hashtable designerActionLists;

		// Token: 0x04000E48 RID: 3656
		private DesignerActionListsChangedEventHandler designerActionListsChanged;

		// Token: 0x04000E49 RID: 3657
		private IServiceProvider serviceProvider;

		// Token: 0x04000E4A RID: 3658
		private ISelectionService selSvc;

		// Token: 0x04000E4B RID: 3659
		private Hashtable componentToVerbsEventHookedUp;

		// Token: 0x04000E4C RID: 3660
		private bool reEntrantCode;
	}
}

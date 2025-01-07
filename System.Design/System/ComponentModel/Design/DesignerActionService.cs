using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	public class DesignerActionService : IDisposable
	{
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

		public void Add(IComponent comp, DesignerActionList actionList)
		{
			this.Add(comp, new DesignerActionListCollection(actionList));
		}

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

		public bool Contains(IComponent comp)
		{
			if (comp == null)
			{
				throw new ArgumentNullException("comp");
			}
			return this.designerActionLists.Contains(comp);
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

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

		public DesignerActionListCollection GetComponentActions(IComponent component)
		{
			return this.GetComponentActions(component, ComponentActionsType.All);
		}

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

		private void OnComponentRemoved(object source, ComponentEventArgs ce)
		{
			this.Remove(ce.Component);
		}

		private void OnDesignerActionListsChanged(DesignerActionListsChangedEventArgs e)
		{
			if (this.designerActionListsChanged != null)
			{
				this.designerActionListsChanged(this, e);
			}
		}

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

		private Hashtable designerActionLists;

		private DesignerActionListsChangedEventHandler designerActionListsChanged;

		private IServiceProvider serviceProvider;

		private ISelectionService selSvc;

		private Hashtable componentToVerbsEventHookedUp;

		private bool reEntrantCode;
	}
}

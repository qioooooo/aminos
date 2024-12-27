using System;
using System.Collections;
using System.Design;
using System.Diagnostics;

namespace System.ComponentModel.Design
{
	// Token: 0x02000136 RID: 310
	public class MenuCommandService : IMenuCommandService, IDisposable
	{
		// Token: 0x06000C01 RID: 3073 RVA: 0x0002ED61 File Offset: 0x0002DD61
		public MenuCommandService(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider;
			this._commandGroups = new Hashtable();
			this._commandChangedHandler = new EventHandler(this.OnCommandChanged);
			TypeDescriptor.Refreshed += this.OnTypeRefreshed;
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000C02 RID: 3074 RVA: 0x0002ED9E File Offset: 0x0002DD9E
		// (remove) Token: 0x06000C03 RID: 3075 RVA: 0x0002EDB7 File Offset: 0x0002DDB7
		public event MenuCommandsChangedEventHandler MenuCommandsChanged
		{
			add
			{
				this._commandsChangedHandler = (MenuCommandsChangedEventHandler)Delegate.Combine(this._commandsChangedHandler, value);
			}
			remove
			{
				this._commandsChangedHandler = (MenuCommandsChangedEventHandler)Delegate.Remove(this._commandsChangedHandler, value);
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x0002EDD0 File Offset: 0x0002DDD0
		public virtual DesignerVerbCollection Verbs
		{
			get
			{
				this.EnsureVerbs();
				return this._currentVerbs;
			}
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0002EDE0 File Offset: 0x0002DDE0
		public virtual void AddCommand(MenuCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			if (((IMenuCommandService)this).FindCommand(command.CommandID) != null)
			{
				throw new ArgumentException(SR.GetString("MenuCommandService_DuplicateCommand", new object[] { command.CommandID.ToString() }));
			}
			ArrayList arrayList = this._commandGroups[command.CommandID.Guid] as ArrayList;
			if (arrayList == null)
			{
				arrayList = new ArrayList();
				arrayList.Add(command);
				this._commandGroups.Add(command.CommandID.Guid, arrayList);
			}
			else
			{
				arrayList.Add(command);
			}
			command.CommandChanged += this._commandChangedHandler;
			this.OnCommandsChanged(new MenuCommandsChangedEventArgs(MenuCommandsChangedType.CommandAdded, command));
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0002EEA0 File Offset: 0x0002DEA0
		public virtual void AddVerb(DesignerVerb verb)
		{
			if (verb == null)
			{
				throw new ArgumentNullException("verb");
			}
			if (this._globalVerbs == null)
			{
				this._globalVerbs = new ArrayList();
			}
			this._globalVerbs.Add(verb);
			this.OnCommandsChanged(new MenuCommandsChangedEventArgs(MenuCommandsChangedType.CommandAdded, verb));
			this.EnsureVerbs();
			if (!((IMenuCommandService)this).Verbs.Contains(verb))
			{
				((IMenuCommandService)this).Verbs.Add(verb);
			}
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0002EF09 File Offset: 0x0002DF09
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0002EF14 File Offset: 0x0002DF14
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._selectionService != null)
				{
					this._selectionService.SelectionChanging -= this.OnSelectionChanging;
					this._selectionService = null;
				}
				if (this._serviceProvider != null)
				{
					this._serviceProvider = null;
					TypeDescriptor.Refreshed -= this.OnTypeRefreshed;
				}
				IDictionaryEnumerator enumerator = this._commandGroups.GetEnumerator();
				while (enumerator.MoveNext())
				{
					ArrayList arrayList = (ArrayList)enumerator.Value;
					foreach (object obj in arrayList)
					{
						MenuCommand menuCommand = (MenuCommand)obj;
						menuCommand.CommandChanged -= this._commandChangedHandler;
					}
					arrayList.Clear();
				}
			}
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0002EFE4 File Offset: 0x0002DFE4
		protected void EnsureVerbs()
		{
			bool flag = false;
			if (this._currentVerbs == null && this._serviceProvider != null)
			{
				if (this._selectionService == null)
				{
					this._selectionService = this.GetService(typeof(ISelectionService)) as ISelectionService;
					if (this._selectionService != null)
					{
						this._selectionService.SelectionChanging += this.OnSelectionChanging;
					}
				}
				int num = 0;
				DesignerVerbCollection designerVerbCollection = null;
				DesignerVerbCollection designerVerbCollection2 = new DesignerVerbCollection();
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (this._selectionService != null && designerHost != null && this._selectionService.SelectionCount == 1)
				{
					object primarySelection = this._selectionService.PrimarySelection;
					if (primarySelection is IComponent && !TypeDescriptor.GetAttributes(primarySelection).Contains(InheritanceAttribute.InheritedReadOnly))
					{
						flag = primarySelection == designerHost.RootComponent;
						IDesigner designer = designerHost.GetDesigner((IComponent)primarySelection);
						if (designer != null)
						{
							designerVerbCollection = designer.Verbs;
							if (designerVerbCollection != null)
							{
								num += designerVerbCollection.Count;
								this._verbSourceType = primarySelection.GetType();
							}
							else
							{
								this._verbSourceType = null;
							}
						}
						DesignerActionService designerActionService = this.GetService(typeof(DesignerActionService)) as DesignerActionService;
						if (designerActionService != null)
						{
							DesignerActionListCollection componentActions = designerActionService.GetComponentActions(primarySelection as IComponent);
							if (componentActions != null)
							{
								foreach (object obj in componentActions)
								{
									DesignerActionList designerActionList = (DesignerActionList)obj;
									DesignerActionItemCollection sortedActionItems = designerActionList.GetSortedActionItems();
									if (sortedActionItems != null)
									{
										for (int i = 0; i < sortedActionItems.Count; i++)
										{
											DesignerActionMethodItem designerActionMethodItem = sortedActionItems[i] as DesignerActionMethodItem;
											if (designerActionMethodItem != null && designerActionMethodItem.IncludeAsDesignerVerb)
											{
												EventHandler eventHandler = new EventHandler(designerActionMethodItem.Invoke);
												DesignerVerb designerVerb = new DesignerVerb(designerActionMethodItem.DisplayName, eventHandler);
												designerVerbCollection2.Add(designerVerb);
												num++;
											}
										}
									}
								}
							}
						}
					}
				}
				if (flag && this._globalVerbs == null)
				{
					flag = false;
				}
				if (flag)
				{
					num += this._globalVerbs.Count;
				}
				Hashtable hashtable = new Hashtable(num, StringComparer.OrdinalIgnoreCase);
				ArrayList arrayList = new ArrayList(num);
				if (flag)
				{
					for (int j = 0; j < this._globalVerbs.Count; j++)
					{
						string text = ((DesignerVerb)this._globalVerbs[j]).Text;
						hashtable[text] = arrayList.Add(this._globalVerbs[j]);
					}
				}
				if (designerVerbCollection2.Count > 0)
				{
					for (int k = 0; k < designerVerbCollection2.Count; k++)
					{
						string text2 = designerVerbCollection2[k].Text;
						hashtable[text2] = arrayList.Add(designerVerbCollection2[k]);
					}
				}
				if (designerVerbCollection != null && designerVerbCollection.Count > 0)
				{
					for (int l = 0; l < designerVerbCollection.Count; l++)
					{
						string text3 = designerVerbCollection[l].Text;
						hashtable[text3] = arrayList.Add(designerVerbCollection[l]);
					}
				}
				DesignerVerb[] array = new DesignerVerb[hashtable.Count];
				int num2 = 0;
				for (int m = 0; m < arrayList.Count; m++)
				{
					DesignerVerb designerVerb2 = (DesignerVerb)arrayList[m];
					string text4 = designerVerb2.Text;
					if ((int)hashtable[text4] == m)
					{
						array[num2] = designerVerb2;
						num2++;
					}
				}
				this._currentVerbs = new DesignerVerbCollection(array);
			}
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0002F388 File Offset: 0x0002E388
		public MenuCommand FindCommand(CommandID commandID)
		{
			return this.FindCommand(commandID.Guid, commandID.ID);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0002F39C File Offset: 0x0002E39C
		protected MenuCommand FindCommand(Guid guid, int id)
		{
			ArrayList arrayList = this._commandGroups[guid] as ArrayList;
			if (arrayList != null)
			{
				foreach (object obj in arrayList)
				{
					MenuCommand menuCommand = (MenuCommand)obj;
					if (menuCommand.CommandID.ID == id)
					{
						return menuCommand;
					}
				}
			}
			this.EnsureVerbs();
			if (this._currentVerbs != null)
			{
				int num = StandardCommands.VerbFirst.ID;
				foreach (object obj2 in this._currentVerbs)
				{
					DesignerVerb designerVerb = (DesignerVerb)obj2;
					CommandID commandID = designerVerb.CommandID;
					if (commandID.ID == id && commandID.Guid.Equals(guid))
					{
						return designerVerb;
					}
					if (num == id && commandID.Guid.Equals(guid))
					{
						return designerVerb;
					}
					if (commandID.Equals(StandardCommands.VerbFirst))
					{
						num++;
					}
				}
			}
			return null;
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0002F4E0 File Offset: 0x0002E4E0
		protected ICollection GetCommandList(Guid guid)
		{
			return this._commandGroups[guid] as ArrayList;
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0002F4F8 File Offset: 0x0002E4F8
		protected object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (this._serviceProvider != null)
			{
				return this._serviceProvider.GetService(serviceType);
			}
			return null;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0002F520 File Offset: 0x0002E520
		public virtual bool GlobalInvoke(CommandID commandID)
		{
			MenuCommand menuCommand = ((IMenuCommandService)this).FindCommand(commandID);
			if (menuCommand != null)
			{
				menuCommand.Invoke();
				return true;
			}
			return false;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0002F544 File Offset: 0x0002E544
		public virtual bool GlobalInvoke(CommandID commandId, object arg)
		{
			MenuCommand menuCommand = ((IMenuCommandService)this).FindCommand(commandId);
			if (menuCommand != null)
			{
				menuCommand.Invoke(arg);
				return true;
			}
			return false;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0002F566 File Offset: 0x0002E566
		private void OnCommandChanged(object sender, EventArgs e)
		{
			this.OnCommandsChanged(new MenuCommandsChangedEventArgs(MenuCommandsChangedType.CommandChanged, (MenuCommand)sender));
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0002F57A File Offset: 0x0002E57A
		protected virtual void OnCommandsChanged(MenuCommandsChangedEventArgs e)
		{
			if (this._commandsChangedHandler != null)
			{
				this._commandsChangedHandler(this, e);
			}
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0002F591 File Offset: 0x0002E591
		private void OnTypeRefreshed(RefreshEventArgs e)
		{
			if (this._verbSourceType != null && this._verbSourceType.IsAssignableFrom(e.TypeChanged))
			{
				this._currentVerbs = null;
			}
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0002F5B5 File Offset: 0x0002E5B5
		private void OnSelectionChanging(object sender, EventArgs e)
		{
			if (this._currentVerbs != null)
			{
				this._currentVerbs = null;
				this.OnCommandsChanged(new MenuCommandsChangedEventArgs(MenuCommandsChangedType.CommandChanged, null));
			}
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0002F5D4 File Offset: 0x0002E5D4
		public virtual void RemoveCommand(MenuCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			ArrayList arrayList = this._commandGroups[command.CommandID.Guid] as ArrayList;
			if (arrayList != null)
			{
				int num = arrayList.IndexOf(command);
				if (-1 != num)
				{
					arrayList.RemoveAt(num);
					if (arrayList.Count == 0)
					{
						this._commandGroups.Remove(command.CommandID.Guid);
					}
					command.CommandChanged -= this._commandChangedHandler;
					this.OnCommandsChanged(new MenuCommandsChangedEventArgs(MenuCommandsChangedType.CommandRemoved, command));
				}
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0002F664 File Offset: 0x0002E664
		public virtual void RemoveVerb(DesignerVerb verb)
		{
			if (verb == null)
			{
				throw new ArgumentNullException("verb");
			}
			if (this._globalVerbs != null)
			{
				int num = this._globalVerbs.IndexOf(verb);
				if (num != -1)
				{
					this._globalVerbs.RemoveAt(num);
					this.EnsureVerbs();
					if (((IMenuCommandService)this).Verbs.Contains(verb))
					{
						((IMenuCommandService)this).Verbs.Remove(verb);
					}
					this.OnCommandsChanged(new MenuCommandsChangedEventArgs(MenuCommandsChangedType.CommandRemoved, verb));
				}
			}
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0002F6D1 File Offset: 0x0002E6D1
		public virtual void ShowContextMenu(CommandID menuID, int x, int y)
		{
		}

		// Token: 0x04000E79 RID: 3705
		private IServiceProvider _serviceProvider;

		// Token: 0x04000E7A RID: 3706
		private Hashtable _commandGroups;

		// Token: 0x04000E7B RID: 3707
		private EventHandler _commandChangedHandler;

		// Token: 0x04000E7C RID: 3708
		private MenuCommandsChangedEventHandler _commandsChangedHandler;

		// Token: 0x04000E7D RID: 3709
		private ArrayList _globalVerbs;

		// Token: 0x04000E7E RID: 3710
		private ISelectionService _selectionService;

		// Token: 0x04000E7F RID: 3711
		internal static TraceSwitch MENUSERVICE = new TraceSwitch("MENUSERVICE", "MenuCommandService: Track menu command routing");

		// Token: 0x04000E80 RID: 3712
		private DesignerVerbCollection _currentVerbs;

		// Token: 0x04000E81 RID: 3713
		private Type _verbSourceType;
	}
}

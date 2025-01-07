using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Design;
using System.Diagnostics;
using System.Reflection;

namespace System.ComponentModel.Design
{
	public abstract class UndoEngine : IDisposable
	{
		protected UndoEngine(IServiceProvider provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this._provider = provider;
			this._unitStack = new Stack();
			this._enabled = true;
			this._host = this.GetRequiredService(typeof(IDesignerHost)) as IDesignerHost;
			this._componentChangeService = this.GetRequiredService(typeof(IComponentChangeService)) as IComponentChangeService;
			this._serializationService = this.GetRequiredService(typeof(ComponentSerializationService)) as ComponentSerializationService;
			this._host.TransactionOpening += this.OnTransactionOpening;
			this._host.TransactionClosed += this.OnTransactionClosed;
			this._componentChangeService.ComponentAdding += this.OnComponentAdding;
			this._componentChangeService.ComponentChanging += this.OnComponentChanging;
			this._componentChangeService.ComponentRemoving += this.OnComponentRemoving;
			this._componentChangeService.ComponentAdded += this.OnComponentAdded;
			this._componentChangeService.ComponentChanged += this.OnComponentChanged;
			this._componentChangeService.ComponentRemoved += this.OnComponentRemoved;
			this._componentChangeService.ComponentRename += this.OnComponentRename;
		}

		private UndoEngine.UndoUnit CurrentUnit
		{
			get
			{
				if (this._unitStack.Count > 0)
				{
					return (UndoEngine.UndoUnit)this._unitStack.Peek();
				}
				return null;
			}
		}

		public bool UndoInProgress
		{
			get
			{
				return this._executingUnit != null;
			}
		}

		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				this._enabled = value;
			}
		}

		public event EventHandler Undoing
		{
			add
			{
				this._undoingEvent = (EventHandler)Delegate.Combine(this._undoingEvent, value);
			}
			remove
			{
				this._undoingEvent = (EventHandler)Delegate.Remove(this._undoingEvent, value);
			}
		}

		public event EventHandler Undone
		{
			add
			{
				this._undoneEvent = (EventHandler)Delegate.Combine(this._undoneEvent, value);
			}
			remove
			{
				this._undoneEvent = (EventHandler)Delegate.Remove(this._undoneEvent, value);
			}
		}

		protected abstract void AddUndoUnit(UndoEngine.UndoUnit unit);

		private void CheckPopUnit(UndoEngine.PopUnitReason reason)
		{
			if (reason != UndoEngine.PopUnitReason.Normal || !this._host.InTransaction)
			{
				UndoEngine.UndoUnit undoUnit = (UndoEngine.UndoUnit)this._unitStack.Pop();
				if (!undoUnit.IsEmpty)
				{
					undoUnit.Close();
					if (reason == UndoEngine.PopUnitReason.TransactionCancel)
					{
						undoUnit.Undo();
						if (this._unitStack.Count == 0)
						{
							this.DiscardUndoUnit(undoUnit);
							return;
						}
					}
					else if (this._unitStack.Count == 0)
					{
						this.AddUndoUnit(undoUnit);
						return;
					}
				}
				else if (this._unitStack.Count == 0)
				{
					this.DiscardUndoUnit(undoUnit);
				}
			}
		}

		protected virtual UndoEngine.UndoUnit CreateUndoUnit(string name, bool primary)
		{
			return new UndoEngine.UndoUnit(this, name);
		}

		internal IComponentChangeService ComponentChangeService
		{
			get
			{
				return this._componentChangeService;
			}
		}

		protected virtual void DiscardUndoUnit(UndoEngine.UndoUnit unit)
		{
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._host != null)
				{
					this._host.TransactionOpening -= this.OnTransactionOpening;
					this._host.TransactionClosed -= this.OnTransactionClosed;
				}
				if (this._componentChangeService != null)
				{
					this._componentChangeService.ComponentAdding -= this.OnComponentAdding;
					this._componentChangeService.ComponentChanging -= this.OnComponentChanging;
					this._componentChangeService.ComponentRemoving -= this.OnComponentRemoving;
					this._componentChangeService.ComponentAdded -= this.OnComponentAdded;
					this._componentChangeService.ComponentChanged -= this.OnComponentChanged;
					this._componentChangeService.ComponentRemoved -= this.OnComponentRemoved;
					this._componentChangeService.ComponentRename -= this.OnComponentRename;
				}
				this._provider = null;
			}
		}

		internal string GetName(object obj, bool generateNew)
		{
			string text = null;
			if (obj != null)
			{
				IReferenceService referenceService = this.GetService(typeof(IReferenceService)) as IReferenceService;
				if (referenceService != null)
				{
					text = referenceService.GetName(obj);
				}
				else
				{
					IComponent component = obj as IComponent;
					if (component != null)
					{
						ISite site = component.Site;
						if (site != null)
						{
							text = site.Name;
						}
					}
				}
			}
			if (text == null && generateNew)
			{
				if (obj == null)
				{
					text = "(null)";
				}
				else
				{
					text = obj.GetType().Name;
				}
			}
			return text;
		}

		protected object GetRequiredService(Type serviceType)
		{
			object service = this.GetService(serviceType);
			if (service == null)
			{
				throw new InvalidOperationException(SR.GetString("UndoEngineMissingService", new object[] { serviceType.Name }))
				{
					HelpLink = "UndoEngineMissingService"
				};
			}
			return service;
		}

		protected object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (this._provider != null)
			{
				return this._provider.GetService(serviceType);
			}
			return null;
		}

		private void OnComponentAdded(object sender, ComponentEventArgs e)
		{
			foreach (object obj in this._unitStack)
			{
				UndoEngine.UndoUnit undoUnit = (UndoEngine.UndoUnit)obj;
				undoUnit.ComponentAdded(e);
			}
			if (this.CurrentUnit != null)
			{
				this.CheckPopUnit(UndoEngine.PopUnitReason.Normal);
			}
		}

		private void OnComponentAdding(object sender, ComponentEventArgs e)
		{
			if (this._enabled && this._executingUnit == null && this._unitStack.Count == 0)
			{
				string text;
				if (e.Component != null)
				{
					text = SR.GetString("UndoEngineComponentAdd1", new object[] { this.GetName(e.Component, true) });
				}
				else
				{
					text = SR.GetString("UndoEngineComponentAdd0");
				}
				this._unitStack.Push(this.CreateUndoUnit(text, true));
			}
			foreach (object obj in this._unitStack)
			{
				UndoEngine.UndoUnit undoUnit = (UndoEngine.UndoUnit)obj;
				undoUnit.ComponentAdding(e);
			}
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			foreach (object obj in this._unitStack)
			{
				UndoEngine.UndoUnit undoUnit = (UndoEngine.UndoUnit)obj;
				undoUnit.ComponentChanged(e);
			}
			if (this.CurrentUnit != null)
			{
				this.CheckPopUnit(UndoEngine.PopUnitReason.Normal);
			}
		}

		private void OnComponentChanging(object sender, ComponentChangingEventArgs e)
		{
			if (this._enabled && this._executingUnit == null && this._unitStack.Count == 0)
			{
				string text;
				if (e.Member != null && e.Component != null)
				{
					text = SR.GetString("UndoEngineComponentChange2", new object[]
					{
						this.GetName(e.Component, true),
						e.Member.Name
					});
				}
				else if (e.Component != null)
				{
					text = SR.GetString("UndoEngineComponentChange1", new object[] { this.GetName(e.Component, true) });
				}
				else
				{
					text = SR.GetString("UndoEngineComponentChange0");
				}
				this._unitStack.Push(this.CreateUndoUnit(text, true));
			}
			foreach (object obj in this._unitStack)
			{
				UndoEngine.UndoUnit undoUnit = (UndoEngine.UndoUnit)obj;
				undoUnit.ComponentChanging(e);
			}
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			foreach (object obj in this._unitStack)
			{
				UndoEngine.UndoUnit undoUnit = (UndoEngine.UndoUnit)obj;
				undoUnit.ComponentRemoved(e);
			}
			if (this.CurrentUnit != null)
			{
				this.CheckPopUnit(UndoEngine.PopUnitReason.Normal);
			}
			List<UndoEngine.ReferencingComponent> list = null;
			if (this._refToRemovedComponent != null && this._refToRemovedComponent.TryGetValue(e.Component, out list) && list != null && this._componentChangeService != null)
			{
				foreach (UndoEngine.ReferencingComponent referencingComponent in list)
				{
					this._componentChangeService.OnComponentChanged(referencingComponent.component, referencingComponent.member, null, null);
				}
				this._refToRemovedComponent.Remove(e.Component);
			}
		}

		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			if (this._enabled && this._executingUnit == null && this._unitStack.Count == 0)
			{
				string text;
				if (e.Component != null)
				{
					text = SR.GetString("UndoEngineComponentRemove1", new object[] { this.GetName(e.Component, true) });
				}
				else
				{
					text = SR.GetString("UndoEngineComponentRemove0");
				}
				this._unitStack.Push(this.CreateUndoUnit(text, true));
			}
			if (this._enabled && this._host != null && this._host.Container != null && this._componentChangeService != null)
			{
				List<UndoEngine.ReferencingComponent> list = null;
				foreach (object obj in this._host.Container.Components)
				{
					IComponent component = (IComponent)obj;
					if (component != e.Component)
					{
						PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component);
						foreach (object obj2 in properties)
						{
							PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj2;
							if (propertyDescriptor.PropertyType.IsAssignableFrom(e.Component.GetType()) && !propertyDescriptor.Attributes.Contains(DesignerSerializationVisibilityAttribute.Hidden) && !propertyDescriptor.IsReadOnly)
							{
								object obj3 = null;
								try
								{
									obj3 = propertyDescriptor.GetValue(component);
								}
								catch (TargetInvocationException)
								{
									continue;
								}
								if (obj3 != null && object.ReferenceEquals(obj3, e.Component))
								{
									if (list == null)
									{
										list = new List<UndoEngine.ReferencingComponent>();
										if (this._refToRemovedComponent == null)
										{
											this._refToRemovedComponent = new Dictionary<IComponent, List<UndoEngine.ReferencingComponent>>();
										}
										this._refToRemovedComponent[e.Component] = list;
									}
									this._componentChangeService.OnComponentChanging(component, propertyDescriptor);
									list.Add(new UndoEngine.ReferencingComponent(component, propertyDescriptor));
								}
							}
						}
					}
				}
			}
			foreach (object obj4 in this._unitStack)
			{
				UndoEngine.UndoUnit undoUnit = (UndoEngine.UndoUnit)obj4;
				undoUnit.ComponentRemoving(e);
			}
		}

		private void OnComponentRename(object sender, ComponentRenameEventArgs e)
		{
			if (this._enabled && this._executingUnit == null && this._unitStack.Count == 0)
			{
				string @string = SR.GetString("UndoEngineComponentRename", new object[] { e.OldName, e.NewName });
				this._unitStack.Push(this.CreateUndoUnit(@string, true));
			}
			foreach (object obj in this._unitStack)
			{
				UndoEngine.UndoUnit undoUnit = (UndoEngine.UndoUnit)obj;
				undoUnit.ComponentRename(e);
			}
		}

		private void OnTransactionClosed(object sender, DesignerTransactionCloseEventArgs e)
		{
			if (this._executingUnit == null && this.CurrentUnit != null)
			{
				UndoEngine.PopUnitReason popUnitReason = (e.TransactionCommitted ? UndoEngine.PopUnitReason.TransactionCommit : UndoEngine.PopUnitReason.TransactionCancel);
				this.CheckPopUnit(popUnitReason);
			}
		}

		private void OnTransactionOpening(object sender, EventArgs e)
		{
			if (this._enabled && this._executingUnit == null)
			{
				this._unitStack.Push(this.CreateUndoUnit(this._host.TransactionDescription, this._unitStack.Count == 0));
			}
		}

		protected virtual void OnUndoing(EventArgs e)
		{
			if (this._undoingEvent != null)
			{
				this._undoingEvent(this, e);
			}
		}

		protected virtual void OnUndone(EventArgs e)
		{
			if (this._undoneEvent != null)
			{
				this._undoneEvent(this, e);
			}
		}

		[Conditional("DEBUG")]
		private static void Trace(string text, params object[] values)
		{
		}

		private static TraceSwitch traceUndo = new TraceSwitch("UndoEngine", "Trace UndoRedo");

		private IServiceProvider _provider;

		private Stack _unitStack;

		private UndoEngine.UndoUnit _executingUnit;

		private IDesignerHost _host;

		private ComponentSerializationService _serializationService;

		private EventHandler _undoingEvent;

		private EventHandler _undoneEvent;

		private IComponentChangeService _componentChangeService;

		private Dictionary<IComponent, List<UndoEngine.ReferencingComponent>> _refToRemovedComponent;

		private bool _enabled;

		private enum PopUnitReason
		{
			Normal,
			TransactionCommit,
			TransactionCancel
		}

		private struct ReferencingComponent
		{
			public ReferencingComponent(IComponent component, MemberDescriptor member)
			{
				this.component = component;
				this.member = member;
			}

			public IComponent component;

			public MemberDescriptor member;
		}

		protected class UndoUnit
		{
			public UndoUnit(UndoEngine engine, string name)
			{
				if (engine == null)
				{
					throw new ArgumentNullException("engine");
				}
				if (name == null)
				{
					name = string.Empty;
				}
				this._name = name;
				this._engine = engine;
				this._reverse = true;
				ISelectionService selectionService = this._engine.GetService(typeof(ISelectionService)) as ISelectionService;
				if (selectionService != null)
				{
					ICollection selectedComponents = selectionService.GetSelectedComponents();
					Hashtable hashtable = new Hashtable();
					foreach (object obj in selectedComponents)
					{
						IComponent component = obj as IComponent;
						if (component != null && component.Site != null)
						{
							hashtable[component.Site.Name] = component.Site.Container;
						}
					}
					this._lastSelection = hashtable;
				}
			}

			public string Name
			{
				get
				{
					return this._name;
				}
			}

			public virtual bool IsEmpty
			{
				get
				{
					return this._events == null || this._events.Count == 0;
				}
			}

			protected UndoEngine UndoEngine
			{
				get
				{
					return this._engine;
				}
			}

			private void AddEvent(UndoEngine.UndoUnit.UndoEvent e)
			{
				if (this._events == null)
				{
					this._events = new ArrayList();
				}
				this._events.Add(e);
			}

			public virtual void Close()
			{
				if (this._changeEvents != null)
				{
					foreach (object obj in this._changeEvents)
					{
						UndoEngine.UndoUnit.ChangeUndoEvent changeUndoEvent = (UndoEngine.UndoUnit.ChangeUndoEvent)obj;
						changeUndoEvent.Commit(this._engine);
					}
				}
				if (this._removeEvents != null)
				{
					foreach (object obj2 in this._removeEvents)
					{
						UndoEngine.UndoUnit.AddRemoveUndoEvent addRemoveUndoEvent = (UndoEngine.UndoUnit.AddRemoveUndoEvent)obj2;
						addRemoveUndoEvent.Commit(this._engine);
					}
				}
				this._changeEvents = null;
				this._removeEvents = null;
				this._ignoreAddingList = null;
				this._ignoreAddedList = null;
			}

			public virtual void ComponentAdded(ComponentEventArgs e)
			{
				if (e.Component.Site == null || !(e.Component.Site.Container is INestedContainer))
				{
					this.AddEvent(new UndoEngine.UndoUnit.AddRemoveUndoEvent(this._engine, e.Component, true));
				}
				if (this._ignoreAddingList != null)
				{
					this._ignoreAddingList.Remove(e.Component);
				}
				if (this._ignoreAddedList == null)
				{
					this._ignoreAddedList = new ArrayList();
				}
				this._ignoreAddedList.Add(e.Component);
			}

			public virtual void ComponentAdding(ComponentEventArgs e)
			{
				if (this._ignoreAddingList == null)
				{
					this._ignoreAddingList = new ArrayList();
				}
				this._ignoreAddingList.Add(e.Component);
			}

			private static bool ChangeEventsSymmetric(ComponentChangingEventArgs changing, ComponentChangedEventArgs changed)
			{
				return changing != null && changed != null && changing.Component == changed.Component && changing.Member == changed.Member;
			}

			private bool CanRepositionEvent(int startIndex, ComponentChangedEventArgs e)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				for (int i = startIndex + 1; i < this._events.Count; i++)
				{
					UndoEngine.UndoUnit.AddRemoveUndoEvent addRemoveUndoEvent = this._events[i] as UndoEngine.UndoUnit.AddRemoveUndoEvent;
					UndoEngine.UndoUnit.RenameUndoEvent renameUndoEvent = this._events[i] as UndoEngine.UndoUnit.RenameUndoEvent;
					UndoEngine.UndoUnit.ChangeUndoEvent changeUndoEvent = this._events[i] as UndoEngine.UndoUnit.ChangeUndoEvent;
					if (addRemoveUndoEvent != null && !addRemoveUndoEvent.NextUndoAdds)
					{
						flag = true;
					}
					else if (changeUndoEvent != null && UndoEngine.UndoUnit.ChangeEventsSymmetric(changeUndoEvent.ComponentChangingEventArgs, e))
					{
						flag3 = true;
					}
					else if (renameUndoEvent != null)
					{
						flag2 = true;
					}
				}
				return flag && !flag2 && !flag3;
			}

			public virtual void ComponentChanged(ComponentChangedEventArgs e)
			{
				if (this._events != null && e != null)
				{
					for (int i = 0; i < this._events.Count; i++)
					{
						UndoEngine.UndoUnit.ChangeUndoEvent changeUndoEvent = this._events[i] as UndoEngine.UndoUnit.ChangeUndoEvent;
						if (changeUndoEvent != null && UndoEngine.UndoUnit.ChangeEventsSymmetric(changeUndoEvent.ComponentChangingEventArgs, e) && i != this._events.Count - 1 && e.Member != null && e.Member.Attributes.Contains(DesignerSerializationVisibilityAttribute.Content) && this.CanRepositionEvent(i, e))
						{
							this._events.RemoveAt(i);
							this._events.Add(changeUndoEvent);
						}
					}
				}
			}

			public virtual void ComponentChanging(ComponentChangingEventArgs e)
			{
				if (this._ignoreAddingList != null && this._ignoreAddingList.Contains(e.Component))
				{
					return;
				}
				if (this._changeEvents == null)
				{
					this._changeEvents = new ArrayList();
				}
				if (this._engine != null && this._engine.GetName(e.Component, false) != null)
				{
					IComponent component = e.Component as IComponent;
					bool flag = false;
					for (int i = 0; i < this._changeEvents.Count; i++)
					{
						UndoEngine.UndoUnit.ChangeUndoEvent changeUndoEvent = (UndoEngine.UndoUnit.ChangeUndoEvent)this._changeEvents[i];
						if (changeUndoEvent.OpenComponent == e.Component && changeUndoEvent.ContainsChange(e.Member))
						{
							flag = true;
							break;
						}
					}
					if (!flag || (e.Member != null && e.Member.Attributes != null && e.Member.Attributes.Contains(DesignerSerializationVisibilityAttribute.Content)))
					{
						UndoEngine.UndoUnit.ChangeUndoEvent changeUndoEvent2 = null;
						bool flag2 = true;
						if (this._ignoreAddedList != null && this._ignoreAddedList.Contains(e.Component))
						{
							flag2 = false;
						}
						if (component != null && component.Site != null)
						{
							changeUndoEvent2 = new UndoEngine.UndoUnit.ChangeUndoEvent(this._engine, e, flag2);
						}
						else if (e.Component != null)
						{
							IReferenceService referenceService = this.GetService(typeof(IReferenceService)) as IReferenceService;
							if (referenceService != null)
							{
								IComponent component2 = referenceService.GetComponent(e.Component);
								if (component2 != null)
								{
									changeUndoEvent2 = new UndoEngine.UndoUnit.ChangeUndoEvent(this._engine, new ComponentChangingEventArgs(component2, null), flag2);
								}
							}
						}
						if (changeUndoEvent2 != null)
						{
							this.AddEvent(changeUndoEvent2);
							this._changeEvents.Add(changeUndoEvent2);
						}
					}
				}
			}

			public virtual void ComponentRemoved(ComponentEventArgs e)
			{
				if (this._events != null)
				{
					UndoEngine.UndoUnit.ChangeUndoEvent changeUndoEvent = null;
					int num = -1;
					int i = this._events.Count - 1;
					while (i >= 0)
					{
						UndoEngine.UndoUnit.AddRemoveUndoEvent addRemoveUndoEvent = this._events[i] as UndoEngine.UndoUnit.AddRemoveUndoEvent;
						if (changeUndoEvent == null)
						{
							changeUndoEvent = this._events[i] as UndoEngine.UndoUnit.ChangeUndoEvent;
							num = i;
						}
						if (addRemoveUndoEvent != null && addRemoveUndoEvent.OpenComponent == e.Component)
						{
							addRemoveUndoEvent.Commit(this._engine);
							if (i == this._events.Count - 1 || changeUndoEvent == null)
							{
								break;
							}
							bool flag = true;
							for (int j = i + 1; j < num; j++)
							{
								if (!(this._events[j] is UndoEngine.UndoUnit.ChangeUndoEvent))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								this._events.RemoveAt(i);
								this._events.Insert(num, addRemoveUndoEvent);
								return;
							}
							break;
						}
						else
						{
							i--;
						}
					}
				}
			}

			public virtual void ComponentRemoving(ComponentEventArgs e)
			{
				if (e.Component.Site != null && e.Component.Site is INestedContainer)
				{
					return;
				}
				if (this._removeEvents == null)
				{
					this._removeEvents = new ArrayList();
				}
				try
				{
					UndoEngine.UndoUnit.AddRemoveUndoEvent addRemoveUndoEvent = new UndoEngine.UndoUnit.AddRemoveUndoEvent(this._engine, e.Component, false);
					this.AddEvent(addRemoveUndoEvent);
					this._removeEvents.Add(addRemoveUndoEvent);
				}
				catch (TargetInvocationException)
				{
				}
			}

			public virtual void ComponentRename(ComponentRenameEventArgs e)
			{
				this.AddEvent(new UndoEngine.UndoUnit.RenameUndoEvent(e.OldName, e.NewName));
			}

			protected object GetService(Type serviceType)
			{
				return this._engine.GetService(serviceType);
			}

			public override string ToString()
			{
				return this.Name;
			}

			public void Undo()
			{
				UndoEngine.UndoUnit executingUnit = this._engine._executingUnit;
				this._engine._executingUnit = this;
				DesignerTransaction designerTransaction = null;
				try
				{
					if (executingUnit == null)
					{
						this._engine.OnUndoing(EventArgs.Empty);
					}
					designerTransaction = this._engine._host.CreateTransaction();
					this.UndoCore();
				}
				catch (CheckoutException)
				{
					designerTransaction.Cancel();
					designerTransaction = null;
					throw;
				}
				finally
				{
					if (designerTransaction != null)
					{
						designerTransaction.Commit();
					}
					this._engine._executingUnit = executingUnit;
					if (executingUnit == null)
					{
						this._engine.OnUndone(EventArgs.Empty);
					}
				}
			}

			protected virtual void UndoCore()
			{
				if (this._events != null)
				{
					if (this._reverse)
					{
						for (int i = this._events.Count - 1; i >= 0; i--)
						{
							int num = i;
							int num2 = i;
							while (num2 >= 0 && ((UndoEngine.UndoUnit.UndoEvent)this._events[num2]).CausesSideEffects)
							{
								num = num2;
								num2--;
							}
							for (int j = i; j >= num; j--)
							{
								((UndoEngine.UndoUnit.UndoEvent)this._events[j]).BeforeUndo(this._engine);
							}
							for (int k = i; k >= num; k--)
							{
								((UndoEngine.UndoUnit.UndoEvent)this._events[k]).Undo(this._engine);
							}
							i = num;
						}
						if (this._lastSelection != null)
						{
							ISelectionService selectionService = this._engine.GetService(typeof(ISelectionService)) as ISelectionService;
							if (selectionService != null)
							{
								string[] array = new string[this._lastSelection.Keys.Count];
								this._lastSelection.Keys.CopyTo(array, 0);
								ArrayList arrayList = new ArrayList(array.Length);
								foreach (string text in array)
								{
									if (text != null)
									{
										object obj = ((Container)this._lastSelection[text]).Components[text];
										if (obj != null)
										{
											arrayList.Add(obj);
										}
									}
								}
								selectionService.SetSelectedComponents(arrayList, SelectionTypes.Replace);
							}
						}
					}
					else
					{
						int count = this._events.Count;
						for (int m = 0; m < count; m++)
						{
							int num3 = m;
							int num4 = m;
							while (num4 < count && ((UndoEngine.UndoUnit.UndoEvent)this._events[num4]).CausesSideEffects)
							{
								num3 = num4;
								num4++;
							}
							for (int n = m; n <= num3; n++)
							{
								((UndoEngine.UndoUnit.UndoEvent)this._events[n]).BeforeUndo(this._engine);
							}
							for (int num5 = m; num5 <= num3; num5++)
							{
								((UndoEngine.UndoUnit.UndoEvent)this._events[num5]).Undo(this._engine);
							}
							m = num3;
						}
					}
				}
				this._reverse = !this._reverse;
			}

			private string _name;

			private UndoEngine _engine;

			private ArrayList _events;

			private ArrayList _changeEvents;

			private ArrayList _removeEvents;

			private ArrayList _ignoreAddingList;

			private ArrayList _ignoreAddedList;

			private bool _reverse;

			private Hashtable _lastSelection;

			private abstract class UndoEvent
			{
				public virtual bool CausesSideEffects
				{
					get
					{
						return false;
					}
				}

				public virtual void BeforeUndo(UndoEngine engine)
				{
				}

				public abstract void Undo(UndoEngine engine);
			}

			private sealed class AddRemoveUndoEvent : UndoEngine.UndoUnit.UndoEvent
			{
				public AddRemoveUndoEvent(UndoEngine engine, IComponent component, bool add)
				{
					this._componentName = component.Site.Name;
					this._nextUndoAdds = !add;
					this._openComponent = component;
					using (this._serializedData = engine._serializationService.CreateStore())
					{
						engine._serializationService.Serialize(this._serializedData, component);
					}
					this._committed = add;
				}

				internal bool Committed
				{
					get
					{
						return this._committed;
					}
				}

				internal IComponent OpenComponent
				{
					get
					{
						return this._openComponent;
					}
				}

				internal bool NextUndoAdds
				{
					get
					{
						return this._nextUndoAdds;
					}
				}

				internal void Commit(UndoEngine engine)
				{
					if (!this.Committed)
					{
						this._committed = true;
					}
				}

				public override void Undo(UndoEngine engine)
				{
					if (this._nextUndoAdds)
					{
						IDesignerHost designerHost = engine.GetRequiredService(typeof(IDesignerHost)) as IDesignerHost;
						if (designerHost != null)
						{
							engine._serializationService.DeserializeTo(this._serializedData, designerHost.Container);
						}
					}
					else
					{
						IDesignerHost designerHost2 = engine.GetRequiredService(typeof(IDesignerHost)) as IDesignerHost;
						IComponent component = designerHost2.Container.Components[this._componentName];
						if (component != null)
						{
							designerHost2.DestroyComponent(component);
						}
					}
					this._nextUndoAdds = !this._nextUndoAdds;
				}

				private SerializationStore _serializedData;

				private string _componentName;

				private bool _nextUndoAdds;

				private bool _committed;

				private IComponent _openComponent;
			}

			private sealed class ChangeUndoEvent : UndoEngine.UndoUnit.UndoEvent
			{
				public ChangeUndoEvent(UndoEngine engine, ComponentChangingEventArgs e, bool serializeBeforeState)
				{
					this._componentName = engine.GetName(e.Component, true);
					this._openComponent = e.Component;
					this._member = e.Member;
					if (serializeBeforeState)
					{
						this._before = this.Serialize(engine, this._openComponent, this._member);
					}
				}

				public ComponentChangingEventArgs ComponentChangingEventArgs
				{
					get
					{
						return new ComponentChangingEventArgs(this._openComponent, this._member);
					}
				}

				public override bool CausesSideEffects
				{
					get
					{
						return true;
					}
				}

				public bool Committed
				{
					get
					{
						return this._openComponent == null;
					}
				}

				public object OpenComponent
				{
					get
					{
						return this._openComponent;
					}
				}

				public override void BeforeUndo(UndoEngine engine)
				{
					if (!this._savedAfterState)
					{
						this._savedAfterState = true;
						this.SaveAfterState(engine);
					}
				}

				public bool ContainsChange(MemberDescriptor desc)
				{
					return this._member == null || (desc != null && desc.Equals(this._member));
				}

				public void Commit(UndoEngine engine)
				{
					if (!this.Committed)
					{
						this._openComponent = null;
					}
				}

				private void SaveAfterState(UndoEngine engine)
				{
					object obj = null;
					IReferenceService referenceService = engine.GetService(typeof(IReferenceService)) as IReferenceService;
					if (referenceService != null)
					{
						obj = referenceService.GetReference(this._componentName);
					}
					else
					{
						IDesignerHost designerHost = engine.GetService(typeof(IDesignerHost)) as IDesignerHost;
						if (designerHost != null)
						{
							obj = designerHost.Container.Components[this._componentName];
						}
					}
					if (obj != null)
					{
						this._after = this.Serialize(engine, obj, this._member);
					}
				}

				private SerializationStore Serialize(UndoEngine engine, object component, MemberDescriptor member)
				{
					SerializationStore serializationStore2;
					SerializationStore serializationStore = (serializationStore2 = engine._serializationService.CreateStore());
					try
					{
						if (member != null && !member.Attributes.Contains(DesignerSerializationVisibilityAttribute.Hidden))
						{
							engine._serializationService.SerializeMemberAbsolute(serializationStore, component, member);
						}
						else
						{
							engine._serializationService.SerializeAbsolute(serializationStore, component);
						}
					}
					finally
					{
						if (serializationStore2 != null)
						{
							((IDisposable)serializationStore2).Dispose();
						}
					}
					return serializationStore;
				}

				public override void Undo(UndoEngine engine)
				{
					if (this._before != null)
					{
						IDesignerHost designerHost = engine.GetService(typeof(IDesignerHost)) as IDesignerHost;
						if (designerHost != null)
						{
							engine._serializationService.DeserializeTo(this._before, designerHost.Container);
						}
					}
					SerializationStore after = this._after;
					this._after = this._before;
					this._before = after;
				}

				private object _openComponent;

				private string _componentName;

				private MemberDescriptor _member;

				private SerializationStore _before;

				private SerializationStore _after;

				private bool _savedAfterState;
			}

			private sealed class RenameUndoEvent : UndoEngine.UndoUnit.UndoEvent
			{
				public RenameUndoEvent(string before, string after)
				{
					this._before = before;
					this._after = after;
				}

				public override void Undo(UndoEngine engine)
				{
					IComponent component = engine._host.Container.Components[this._after];
					if (component != null)
					{
						engine.ComponentChangeService.OnComponentChanging(component, null);
						component.Site.Name = this._before;
						string after = this._after;
						this._after = this._before;
						this._before = after;
					}
				}

				private string _before;

				private string _after;
			}
		}
	}
}

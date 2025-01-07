using System;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.ComponentModel.Design
{
	internal sealed class SelectionService : ISelectionService, IDisposable
	{
		internal SelectionService(IServiceProvider provider)
		{
			this._provider = provider;
			this._state = default(BitVector32);
			this._events = new EventHandlerList();
			this._statusCommandUI = new StatusCommandUI(provider);
		}

		internal void AddSelection(object sel)
		{
			if (this._selection == null)
			{
				this._selection = new ArrayList();
				IComponentChangeService componentChangeService = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRemoved += this.OnComponentRemove;
				}
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null)
				{
					designerHost.TransactionOpened += this.OnTransactionOpened;
					designerHost.TransactionClosed += this.OnTransactionClosed;
					if (designerHost.InTransaction)
					{
						this.OnTransactionOpened(designerHost, EventArgs.Empty);
					}
				}
			}
			if (!this._selection.Contains(sel))
			{
				this._selection.Add(sel);
			}
		}

		private void FlushSelectionChanges()
		{
			if (!this._state[SelectionService.StateTransaction] && this._state[SelectionService.StateTransactionChange])
			{
				this._state[SelectionService.StateTransactionChange] = false;
				this.OnSelectionChanged();
			}
		}

		private object GetService(Type serviceType)
		{
			if (this._provider != null)
			{
				return this._provider.GetService(serviceType);
			}
			return null;
		}

		private void OnComponentRemove(object sender, ComponentEventArgs ce)
		{
			if (this._selection != null && this._selection.Contains(ce.Component))
			{
				this.RemoveSelection(ce.Component);
				this.OnSelectionChanged();
			}
		}

		private void OnSelectionChanged()
		{
			if (this._state[SelectionService.StateTransaction])
			{
				this._state[SelectionService.StateTransactionChange] = true;
				return;
			}
			EventHandler eventHandler = this._events[SelectionService.EventSelectionChanging] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
			this.UpdateHelpKeyword(true);
			eventHandler = this._events[SelectionService.EventSelectionChanged] as EventHandler;
			if (eventHandler != null)
			{
				try
				{
					eventHandler(this, EventArgs.Empty);
				}
				catch
				{
				}
			}
		}

		private void OnTransactionClosed(object sender, DesignerTransactionCloseEventArgs e)
		{
			if (e.LastTransaction)
			{
				this._state[SelectionService.StateTransaction] = false;
				this.FlushSelectionChanges();
			}
		}

		private void OnTransactionOpened(object sender, EventArgs e)
		{
			this._state[SelectionService.StateTransaction] = true;
		}

		internal void RemoveSelection(object sel)
		{
			if (this._selection != null)
			{
				this._selection.Remove(sel);
			}
		}

		private void ApplicationIdle(object source, EventArgs args)
		{
			this.UpdateHelpKeyword(false);
			Application.Idle -= this.ApplicationIdle;
		}

		private void UpdateHelpKeyword(bool tryLater)
		{
			IHelpService helpService = this.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService == null)
			{
				if (tryLater)
				{
					Application.Idle += this.ApplicationIdle;
				}
				return;
			}
			if (this._contextAttributes != null)
			{
				foreach (string text in this._contextAttributes)
				{
					helpService.RemoveContextAttribute("Keyword", text);
				}
				this._contextAttributes = null;
			}
			helpService.RemoveContextAttribute("Selection", SelectionService.SelectionKeywords[(int)this._contextKeyword]);
			bool flag = false;
			if (this._selection.Count == 0)
			{
				flag = true;
			}
			else if (this._selection.Count == 1)
			{
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null && this._selection.Contains(designerHost.RootComponent))
				{
					flag = true;
				}
			}
			this._contextAttributes = new string[this._selection.Count];
			for (int j = 0; j < this._selection.Count; j++)
			{
				object obj = this._selection[j];
				string text2 = TypeDescriptor.GetClassName(obj);
				HelpKeywordAttribute helpKeywordAttribute = (HelpKeywordAttribute)TypeDescriptor.GetAttributes(obj)[typeof(HelpKeywordAttribute)];
				if (helpKeywordAttribute != null && !helpKeywordAttribute.IsDefaultAttribute())
				{
					text2 = helpKeywordAttribute.HelpKeyword;
				}
				this._contextAttributes[j] = text2;
			}
			HelpKeywordType helpKeywordType = (flag ? HelpKeywordType.GeneralKeyword : HelpKeywordType.F1Keyword);
			foreach (string text3 in this._contextAttributes)
			{
				helpService.AddContextAttribute("Keyword", text3, helpKeywordType);
			}
			int num = this._selection.Count;
			if (num == 1 && flag)
			{
				num--;
			}
			this._contextKeyword = (short)Math.Min(num, SelectionService.SelectionKeywords.Length - 1);
			helpService.AddContextAttribute("Selection", SelectionService.SelectionKeywords[(int)this._contextKeyword], HelpKeywordType.FilterKeyword);
		}

		void IDisposable.Dispose()
		{
			if (this._selection != null)
			{
				IDesignerHost designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null)
				{
					designerHost.TransactionOpened -= this.OnTransactionOpened;
					designerHost.TransactionClosed -= this.OnTransactionClosed;
					if (designerHost.InTransaction)
					{
						this.OnTransactionClosed(designerHost, new DesignerTransactionCloseEventArgs(true, true));
					}
				}
				IComponentChangeService componentChangeService = this.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				if (componentChangeService != null)
				{
					componentChangeService.ComponentRemoved -= this.OnComponentRemove;
				}
				this._selection.Clear();
			}
			this._statusCommandUI = null;
			this._provider = null;
		}

		object ISelectionService.PrimarySelection
		{
			get
			{
				if (this._selection != null && this._selection.Count > 0)
				{
					return this._selection[0];
				}
				return null;
			}
		}

		int ISelectionService.SelectionCount
		{
			get
			{
				if (this._selection != null)
				{
					return this._selection.Count;
				}
				return 0;
			}
		}

		event EventHandler ISelectionService.SelectionChanged
		{
			add
			{
				this._events.AddHandler(SelectionService.EventSelectionChanged, value);
			}
			remove
			{
				this._events.RemoveHandler(SelectionService.EventSelectionChanged, value);
			}
		}

		event EventHandler ISelectionService.SelectionChanging
		{
			add
			{
				this._events.AddHandler(SelectionService.EventSelectionChanging, value);
			}
			remove
			{
				this._events.RemoveHandler(SelectionService.EventSelectionChanging, value);
			}
		}

		bool ISelectionService.GetComponentSelected(object component)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			return this._selection != null && this._selection.Contains(component);
		}

		ICollection ISelectionService.GetSelectedComponents()
		{
			if (this._selection != null)
			{
				object[] array = new object[this._selection.Count];
				this._selection.CopyTo(array, 0);
				return array;
			}
			return new object[0];
		}

		void ISelectionService.SetSelectedComponents(ICollection components)
		{
			((ISelectionService)this).SetSelectedComponents(components, SelectionTypes.Auto);
		}

		void ISelectionService.SetSelectedComponents(ICollection components, SelectionTypes selectionType)
		{
			bool flag = (selectionType & SelectionTypes.Toggle) == SelectionTypes.Toggle;
			bool flag2 = (selectionType & SelectionTypes.Click) == SelectionTypes.Click;
			bool flag3 = (selectionType & SelectionTypes.Add) == SelectionTypes.Add;
			bool flag4 = (selectionType & SelectionTypes.Remove) == SelectionTypes.Remove;
			bool flag5 = (selectionType & SelectionTypes.Replace) == SelectionTypes.Replace;
			bool flag6 = !flag && !flag3 && !flag4 && !flag5;
			if (components == null)
			{
				components = new object[0];
			}
			if (flag6)
			{
				flag = (Control.ModifierKeys & (Keys.Shift | Keys.Control)) > Keys.None;
				flag3 |= Control.ModifierKeys == Keys.Shift;
				if (flag || flag3)
				{
					flag2 = false;
				}
			}
			bool flag7 = false;
			object obj = null;
			if (flag2 && 1 == components.Count)
			{
				using (IEnumerator enumerator = components.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						obj = obj2;
						if (obj2 == null)
						{
							throw new ArgumentNullException("components");
						}
					}
				}
			}
			int num;
			if (obj != null && this._selection != null && (num = this._selection.IndexOf(obj)) != -1)
			{
				if (num != 0)
				{
					object obj3 = this._selection[0];
					this._selection[0] = this._selection[num];
					this._selection[num] = obj3;
					flag7 = true;
				}
			}
			else
			{
				if (!flag && !flag3 && !flag4 && this._selection != null)
				{
					object[] array = new object[this._selection.Count];
					this._selection.CopyTo(array, 0);
					foreach (object obj4 in array)
					{
						bool flag8 = true;
						foreach (object obj5 in components)
						{
							if (obj5 == null)
							{
								throw new ArgumentNullException("components");
							}
							if (object.ReferenceEquals(obj5, obj4))
							{
								flag8 = false;
								break;
							}
						}
						if (flag8)
						{
							this.RemoveSelection(obj4);
							flag7 = true;
						}
					}
				}
				foreach (object obj6 in components)
				{
					if (obj6 == null)
					{
						throw new ArgumentNullException("components");
					}
					if (this._selection != null && this._selection.Contains(obj6))
					{
						if (flag || flag4)
						{
							this.RemoveSelection(obj6);
							flag7 = true;
						}
					}
					else if (!flag4)
					{
						this.AddSelection(obj6);
						flag7 = true;
					}
				}
			}
			if (flag7)
			{
				if (this._selection.Count > 0)
				{
					this._statusCommandUI.SetStatusInformation(this._selection[0] as Component);
				}
				else
				{
					this._statusCommandUI.SetStatusInformation(Rectangle.Empty);
				}
				this.OnSelectionChanged();
			}
		}

		private static readonly string[] SelectionKeywords = new string[] { "None", "Single", "Multiple" };

		private static readonly int StateTransaction = BitVector32.CreateMask();

		private static readonly int StateTransactionChange = BitVector32.CreateMask(SelectionService.StateTransaction);

		private static readonly object EventSelectionChanging = new object();

		private static readonly object EventSelectionChanged = new object();

		private IServiceProvider _provider;

		private BitVector32 _state;

		private EventHandlerList _events;

		private ArrayList _selection;

		private string[] _contextAttributes;

		private short _contextKeyword;

		private StatusCommandUI _statusCommandUI;
	}
}

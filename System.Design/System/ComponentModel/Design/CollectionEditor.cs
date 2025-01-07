using System;
using System.Collections;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

namespace System.ComponentModel.Design
{
	public class CollectionEditor : UITypeEditor
	{
		protected virtual void CancelChanges()
		{
		}

		public CollectionEditor(Type type)
		{
			this.type = type;
		}

		protected Type CollectionItemType
		{
			get
			{
				if (this.collectionItemType == null)
				{
					this.collectionItemType = this.CreateCollectionItemType();
				}
				return this.collectionItemType;
			}
		}

		protected Type CollectionType
		{
			get
			{
				return this.type;
			}
		}

		protected ITypeDescriptorContext Context
		{
			get
			{
				return this.currentContext;
			}
		}

		protected Type[] NewItemTypes
		{
			get
			{
				if (this.newItemTypes == null)
				{
					this.newItemTypes = this.CreateNewItemTypes();
				}
				return this.newItemTypes;
			}
		}

		protected virtual string HelpTopic
		{
			get
			{
				return "net.ComponentModel.CollectionEditor";
			}
		}

		protected virtual bool CanRemoveInstance(object value)
		{
			IComponent component = value as IComponent;
			if (component != null)
			{
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(component)[typeof(InheritanceAttribute)];
				if (inheritanceAttribute != null && inheritanceAttribute.InheritanceLevel != InheritanceLevel.NotInherited)
				{
					return false;
				}
			}
			return true;
		}

		protected virtual bool CanSelectMultipleInstances()
		{
			return true;
		}

		protected virtual CollectionEditor.CollectionForm CreateCollectionForm()
		{
			return new CollectionEditor.CollectionEditorCollectionForm(this);
		}

		protected virtual object CreateInstance(Type itemType)
		{
			return CollectionEditor.CreateInstance(itemType, (IDesignerHost)this.GetService(typeof(IDesignerHost)), null);
		}

		protected virtual IList GetObjectsFromInstance(object instance)
		{
			return new ArrayList { instance };
		}

		internal static object CreateInstance(Type itemType, IDesignerHost host, string name)
		{
			object obj = null;
			if (typeof(IComponent).IsAssignableFrom(itemType) && host != null)
			{
				obj = host.CreateComponent(itemType, name);
				if (host != null)
				{
					IComponentInitializer componentInitializer = host.GetDesigner((IComponent)obj) as IComponentInitializer;
					if (componentInitializer != null)
					{
						componentInitializer.InitializeNewComponent(null);
					}
				}
			}
			if (obj == null)
			{
				obj = TypeDescriptor.CreateInstance(host, itemType, null, null);
			}
			return obj;
		}

		protected virtual string GetDisplayText(object value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(value)["Name"];
			string text;
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string))
			{
				text = (string)propertyDescriptor.GetValue(value);
				if (text != null && text.Length > 0)
				{
					return text;
				}
			}
			propertyDescriptor = TypeDescriptor.GetDefaultProperty(this.CollectionType);
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(string))
			{
				text = (string)propertyDescriptor.GetValue(value);
				if (text != null && text.Length > 0)
				{
					return text;
				}
			}
			text = TypeDescriptor.GetConverter(value).ConvertToString(value);
			if (text == null || text.Length == 0)
			{
				text = value.GetType().Name;
			}
			return text;
		}

		protected virtual Type CreateCollectionItemType()
		{
			PropertyInfo[] properties = TypeDescriptor.GetReflectionType(this.CollectionType).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i].Name.Equals("Item") || properties[i].Name.Equals("Items"))
				{
					return properties[i].PropertyType;
				}
			}
			return typeof(object);
		}

		protected virtual Type[] CreateNewItemTypes()
		{
			return new Type[] { this.CollectionItemType };
		}

		protected virtual void DestroyInstance(object instance)
		{
			IComponent component = instance as IComponent;
			if (component == null)
			{
				IDisposable disposable = instance as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				return;
			}
			IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				designerHost.DestroyComponent(component);
				return;
			}
			component.Dispose();
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					this.currentContext = context;
					CollectionEditor.CollectionForm collectionForm = this.CreateCollectionForm();
					ITypeDescriptorContext typeDescriptorContext = this.currentContext;
					collectionForm.EditValue = value;
					this.ignoreChangingEvents = false;
					this.ignoreChangedEvents = false;
					DesignerTransaction designerTransaction = null;
					bool flag = true;
					IComponentChangeService componentChangeService = null;
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					try
					{
						try
						{
							if (designerHost != null)
							{
								designerTransaction = designerHost.CreateTransaction(SR.GetString("CollectionEditorUndoBatchDesc", new object[] { this.CollectionItemType.Name }));
							}
						}
						catch (CheckoutException ex)
						{
							if (ex == CheckoutException.Canceled)
							{
								return value;
							}
							throw ex;
						}
						componentChangeService = ((designerHost != null) ? ((IComponentChangeService)designerHost.GetService(typeof(IComponentChangeService))) : null);
						if (componentChangeService != null)
						{
							componentChangeService.ComponentChanged += this.OnComponentChanged;
							componentChangeService.ComponentChanging += this.OnComponentChanging;
						}
						if (collectionForm.ShowEditorDialog(windowsFormsEditorService) == DialogResult.OK)
						{
							value = collectionForm.EditValue;
						}
						else
						{
							flag = false;
						}
					}
					finally
					{
						collectionForm.EditValue = null;
						this.currentContext = typeDescriptorContext;
						if (designerTransaction != null)
						{
							if (flag)
							{
								designerTransaction.Commit();
							}
							else
							{
								designerTransaction.Cancel();
							}
						}
						if (componentChangeService != null)
						{
							componentChangeService.ComponentChanged -= this.OnComponentChanged;
							componentChangeService.ComponentChanging -= this.OnComponentChanging;
						}
						collectionForm.Dispose();
					}
					return value;
				}
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		private bool IsAnyObjectInheritedReadOnly(object[] items)
		{
			IInheritanceService inheritanceService = null;
			bool flag = false;
			foreach (object obj in items)
			{
				IComponent component = obj as IComponent;
				if (component != null && component.Site == null)
				{
					if (!flag)
					{
						flag = true;
						if (this.Context != null)
						{
							inheritanceService = (IInheritanceService)this.Context.GetService(typeof(IInheritanceService));
						}
					}
					if (inheritanceService != null && inheritanceService.GetInheritanceAttribute(component).Equals(InheritanceAttribute.InheritedReadOnly))
					{
						return true;
					}
				}
			}
			return false;
		}

		protected virtual object[] GetItems(object editValue)
		{
			if (editValue != null && editValue is ICollection)
			{
				ArrayList arrayList = new ArrayList();
				ICollection collection = (ICollection)editValue;
				foreach (object obj in collection)
				{
					arrayList.Add(obj);
				}
				object[] array = new object[arrayList.Count];
				arrayList.CopyTo(array, 0);
				return array;
			}
			return new object[0];
		}

		protected object GetService(Type serviceType)
		{
			if (this.Context != null)
			{
				return this.Context.GetService(serviceType);
			}
			return null;
		}

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (!this.ignoreChangedEvents && sender != this.Context.Instance)
			{
				this.ignoreChangedEvents = true;
				this.Context.OnComponentChanged();
			}
		}

		private void OnComponentChanging(object sender, ComponentChangingEventArgs e)
		{
			if (!this.ignoreChangingEvents && sender != this.Context.Instance)
			{
				this.ignoreChangingEvents = true;
				this.Context.OnComponentChanging();
			}
		}

		internal virtual void OnItemRemoving(object item)
		{
		}

		protected virtual object SetItems(object editValue, object[] value)
		{
			if (editValue != null)
			{
				Array items = this.GetItems(editValue);
				int length = items.Length;
				int num = value.Length;
				if (editValue is IList)
				{
					IList list = (IList)editValue;
					list.Clear();
					for (int i = 0; i < value.Length; i++)
					{
						list.Add(value[i]);
					}
				}
			}
			return editValue;
		}

		protected virtual void ShowHelp()
		{
			IHelpService helpService = this.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword(this.HelpTopic);
			}
		}

		private Type type;

		private Type collectionItemType;

		private Type[] newItemTypes;

		private ITypeDescriptorContext currentContext;

		private bool ignoreChangedEvents;

		private bool ignoreChangingEvents;

		internal class SplitButton : Button
		{
			public bool ShowSplit
			{
				set
				{
					if (value != this.showSplit)
					{
						this.showSplit = value;
						base.Invalidate();
					}
				}
			}

			private PushButtonState State
			{
				get
				{
					return this._state;
				}
				set
				{
					if (!this._state.Equals(value))
					{
						this._state = value;
						base.Invalidate();
					}
				}
			}

			public override Size GetPreferredSize(Size proposedSize)
			{
				Size preferredSize = base.GetPreferredSize(proposedSize);
				if (this.showSplit && !string.IsNullOrEmpty(this.Text) && TextRenderer.MeasureText(this.Text, this.Font).Width + 14 > preferredSize.Width)
				{
					return preferredSize + new Size(14, 0);
				}
				return preferredSize;
			}

			protected override bool IsInputKey(Keys keyData)
			{
				return (keyData.Equals(Keys.Down) && this.showSplit) || base.IsInputKey(keyData);
			}

			protected override void OnGotFocus(EventArgs e)
			{
				if (!this.showSplit)
				{
					base.OnGotFocus(e);
					return;
				}
				if (!this.State.Equals(PushButtonState.Pressed) && !this.State.Equals(PushButtonState.Disabled))
				{
					this.State = PushButtonState.Default;
				}
			}

			protected override void OnKeyDown(KeyEventArgs kevent)
			{
				if (kevent.KeyCode.Equals(Keys.Down) && this.showSplit)
				{
					this.ShowContextMenuStrip();
				}
			}

			protected override void OnLostFocus(EventArgs e)
			{
				if (!this.showSplit)
				{
					base.OnLostFocus(e);
					return;
				}
				if (!this.State.Equals(PushButtonState.Pressed) && !this.State.Equals(PushButtonState.Disabled))
				{
					this.State = PushButtonState.Normal;
				}
			}

			protected override void OnMouseDown(MouseEventArgs e)
			{
				if (!this.showSplit)
				{
					base.OnMouseDown(e);
					return;
				}
				if (this.dropDownRectangle.Contains(e.Location))
				{
					this.ShowContextMenuStrip();
					return;
				}
				this.State = PushButtonState.Pressed;
			}

			protected override void OnMouseEnter(EventArgs e)
			{
				if (!this.showSplit)
				{
					base.OnMouseEnter(e);
					return;
				}
				if (!this.State.Equals(PushButtonState.Pressed) && !this.State.Equals(PushButtonState.Disabled))
				{
					this.State = PushButtonState.Hot;
				}
			}

			protected override void OnMouseLeave(EventArgs e)
			{
				if (!this.showSplit)
				{
					base.OnMouseLeave(e);
					return;
				}
				if (!this.State.Equals(PushButtonState.Pressed) && !this.State.Equals(PushButtonState.Disabled))
				{
					if (this.Focused)
					{
						this.State = PushButtonState.Default;
						return;
					}
					this.State = PushButtonState.Normal;
				}
			}

			protected override void OnMouseUp(MouseEventArgs mevent)
			{
				if (!this.showSplit)
				{
					base.OnMouseUp(mevent);
					return;
				}
				if (this.ContextMenuStrip == null || !this.ContextMenuStrip.Visible)
				{
					this.SetButtonDrawState();
					if (base.Bounds.Contains(base.Parent.PointToClient(Cursor.Position)) && !this.dropDownRectangle.Contains(mevent.Location))
					{
						this.OnClick(new EventArgs());
					}
				}
			}

			protected override void OnPaint(PaintEventArgs pevent)
			{
				base.OnPaint(pevent);
				if (!this.showSplit)
				{
					return;
				}
				Graphics graphics = pevent.Graphics;
				Rectangle rectangle = new Rectangle(0, 0, base.Width, base.Height);
				TextFormatFlags textFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
				ButtonRenderer.DrawButton(graphics, rectangle, this.State);
				this.dropDownRectangle = new Rectangle(rectangle.Right - 14 - 1, 4, 14, rectangle.Height - 8);
				if (this.RightToLeft == RightToLeft.Yes)
				{
					this.dropDownRectangle.X = rectangle.Left + 1;
					graphics.DrawLine(SystemPens.ButtonHighlight, rectangle.Left + 14, 4, rectangle.Left + 14, rectangle.Bottom - 4);
					graphics.DrawLine(SystemPens.ButtonHighlight, rectangle.Left + 14 + 1, 4, rectangle.Left + 14 + 1, rectangle.Bottom - 4);
					rectangle.Offset(14, 0);
					rectangle.Width -= 14;
				}
				else
				{
					graphics.DrawLine(SystemPens.ButtonHighlight, rectangle.Right - 14, 4, rectangle.Right - 14, rectangle.Bottom - 4);
					graphics.DrawLine(SystemPens.ButtonHighlight, rectangle.Right - 14 - 1, 4, rectangle.Right - 14 - 1, rectangle.Bottom - 4);
					rectangle.Width -= 14;
				}
				this.PaintArrow(graphics, this.dropDownRectangle);
				if (!base.UseMnemonic)
				{
					textFormatFlags |= TextFormatFlags.NoPrefix;
				}
				else if (!this.ShowKeyboardCues)
				{
					textFormatFlags |= TextFormatFlags.HidePrefix;
				}
				if (!string.IsNullOrEmpty(this.Text))
				{
					TextRenderer.DrawText(graphics, this.Text, this.Font, rectangle, SystemColors.ControlText, textFormatFlags);
				}
				if (this.Focused)
				{
					rectangle.Inflate(-4, -4);
				}
			}

			private void PaintArrow(Graphics g, Rectangle dropDownRect)
			{
				Point point = new Point(Convert.ToInt32(dropDownRect.Left + dropDownRect.Width / 2), Convert.ToInt32(dropDownRect.Top + dropDownRect.Height / 2));
				point.X += dropDownRect.Width % 2;
				Point[] array = new Point[]
				{
					new Point(point.X - 2, point.Y - 1),
					new Point(point.X + 3, point.Y - 1),
					new Point(point.X, point.Y + 2)
				};
				g.FillPolygon(SystemBrushes.ControlText, array);
			}

			private void ShowContextMenuStrip()
			{
				this.State = PushButtonState.Pressed;
				if (this.ContextMenuStrip != null)
				{
					this.ContextMenuStrip.Closed += this.ContextMenuStrip_Closed;
					this.ContextMenuStrip.Show(this, 0, base.Height);
				}
			}

			private void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
			{
				ContextMenuStrip contextMenuStrip = sender as ContextMenuStrip;
				if (contextMenuStrip != null)
				{
					contextMenuStrip.Closed -= this.ContextMenuStrip_Closed;
				}
				this.SetButtonDrawState();
			}

			private void SetButtonDrawState()
			{
				if (base.Bounds.Contains(base.Parent.PointToClient(Cursor.Position)))
				{
					this.State = PushButtonState.Hot;
					return;
				}
				if (this.Focused)
				{
					this.State = PushButtonState.Default;
					return;
				}
				this.State = PushButtonState.Normal;
			}

			private const int pushButtonWidth = 14;

			private PushButtonState _state;

			private Rectangle dropDownRectangle = default(Rectangle);

			private bool showSplit;
		}

		protected abstract class CollectionForm : Form
		{
			public CollectionForm(CollectionEditor editor)
			{
				this.editor = editor;
			}

			protected Type CollectionItemType
			{
				get
				{
					return this.editor.CollectionItemType;
				}
			}

			protected Type CollectionType
			{
				get
				{
					return this.editor.CollectionType;
				}
			}

			internal virtual bool CollectionEditable
			{
				get
				{
					if (this.editableState != 0)
					{
						return this.editableState == 1;
					}
					bool flag = typeof(IList).IsAssignableFrom(this.editor.CollectionType);
					if (flag)
					{
						IList list = this.EditValue as IList;
						if (list != null)
						{
							return !list.IsReadOnly;
						}
					}
					return flag;
				}
				set
				{
					if (value)
					{
						this.editableState = 1;
						return;
					}
					this.editableState = 2;
				}
			}

			protected ITypeDescriptorContext Context
			{
				get
				{
					return this.editor.Context;
				}
			}

			public object EditValue
			{
				get
				{
					return this.value;
				}
				set
				{
					this.value = value;
					this.OnEditValueChanged();
				}
			}

			protected object[] Items
			{
				get
				{
					return this.editor.GetItems(this.EditValue);
				}
				set
				{
					bool flag = false;
					try
					{
						flag = this.Context.OnComponentChanging();
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
						this.DisplayError(ex);
					}
					if (flag)
					{
						object obj = this.editor.SetItems(this.EditValue, value);
						if (obj != this.EditValue)
						{
							this.EditValue = obj;
						}
						this.Context.OnComponentChanged();
					}
				}
			}

			protected Type[] NewItemTypes
			{
				get
				{
					return this.editor.NewItemTypes;
				}
			}

			protected bool CanRemoveInstance(object value)
			{
				return this.editor.CanRemoveInstance(value);
			}

			protected virtual bool CanSelectMultipleInstances()
			{
				return this.editor.CanSelectMultipleInstances();
			}

			protected object CreateInstance(Type itemType)
			{
				return this.editor.CreateInstance(itemType);
			}

			protected void DestroyInstance(object instance)
			{
				this.editor.DestroyInstance(instance);
			}

			protected virtual void DisplayError(Exception e)
			{
				IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					iuiservice.ShowError(e);
					return;
				}
				string text = e.Message;
				if (text == null || text.Length == 0)
				{
					text = e.ToString();
				}
				RTLAwareMessageBox.Show(null, text, null, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
			}

			protected override object GetService(Type serviceType)
			{
				return this.editor.GetService(serviceType);
			}

			protected internal virtual DialogResult ShowEditorDialog(IWindowsFormsEditorService edSvc)
			{
				return edSvc.ShowDialog(this);
			}

			protected abstract void OnEditValueChanged();

			private const short EditableDynamic = 0;

			private const short EditableYes = 1;

			private const short EditableNo = 2;

			private CollectionEditor editor;

			private object value;

			private short editableState;
		}

		private class CollectionEditorCollectionForm : CollectionEditor.CollectionForm
		{
			public CollectionEditorCollectionForm(CollectionEditor editor)
				: base(editor)
			{
				this.editor = editor;
				this.InitializeComponent();
				this.Text = SR.GetString("CollectionEditorCaption", new object[] { base.CollectionItemType.Name });
				this.HookEvents();
				Type[] newItemTypes = base.NewItemTypes;
				if (newItemTypes.Length > 1)
				{
					EventHandler eventHandler = new EventHandler(this.AddDownMenu_click);
					this.addButton.ShowSplit = true;
					this.addDownMenu = new ContextMenuStrip();
					this.addButton.ContextMenuStrip = this.addDownMenu;
					for (int i = 0; i < newItemTypes.Length; i++)
					{
						this.addDownMenu.Items.Add(new CollectionEditor.CollectionEditorCollectionForm.TypeMenuItem(newItemTypes[i], eventHandler));
					}
				}
				this.AdjustListBoxItemHeight();
			}

			private bool IsImmutable
			{
				get
				{
					bool flag = true;
					if (!TypeDescriptor.GetConverter(base.CollectionItemType).GetCreateInstanceSupported())
					{
						foreach (object obj in TypeDescriptor.GetProperties(base.CollectionItemType))
						{
							PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
							if (!propertyDescriptor.IsReadOnly)
							{
								flag = false;
								break;
							}
						}
					}
					return flag;
				}
			}

			private void AddButton_click(object sender, EventArgs e)
			{
				this.PerformAdd();
			}

			private void AddDownMenu_click(object sender, EventArgs e)
			{
				if (sender is CollectionEditor.CollectionEditorCollectionForm.TypeMenuItem)
				{
					CollectionEditor.CollectionEditorCollectionForm.TypeMenuItem typeMenuItem = (CollectionEditor.CollectionEditorCollectionForm.TypeMenuItem)sender;
					this.CreateAndAddInstance(typeMenuItem.ItemType);
				}
			}

			private void AddItems(IList instances)
			{
				if (this.createdItems == null)
				{
					this.createdItems = new ArrayList();
				}
				this.listbox.BeginUpdate();
				try
				{
					foreach (object obj in instances)
					{
						if (obj != null)
						{
							this.dirty = true;
							this.createdItems.Add(obj);
							CollectionEditor.CollectionEditorCollectionForm.ListItem listItem = new CollectionEditor.CollectionEditorCollectionForm.ListItem(this.editor, obj);
							this.listbox.Items.Add(listItem);
						}
					}
				}
				finally
				{
					this.listbox.EndUpdate();
				}
				if (instances.Count == 1)
				{
					this.UpdateItemWidths(this.listbox.Items[this.listbox.Items.Count - 1] as CollectionEditor.CollectionEditorCollectionForm.ListItem);
				}
				else
				{
					this.UpdateItemWidths(null);
				}
				this.SuspendEnabledUpdates();
				try
				{
					this.listbox.ClearSelected();
					this.listbox.SelectedIndex = this.listbox.Items.Count - 1;
					object[] array = new object[this.listbox.Items.Count];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = ((CollectionEditor.CollectionEditorCollectionForm.ListItem)this.listbox.Items[i]).Value;
					}
					base.Items = array;
					if (this.listbox.Items.Count > 0 && this.listbox.SelectedIndex != this.listbox.Items.Count - 1)
					{
						this.listbox.ClearSelected();
						this.listbox.SelectedIndex = this.listbox.Items.Count - 1;
					}
				}
				finally
				{
					this.ResumeEnabledUpdates(true);
				}
			}

			private void AdjustListBoxItemHeight()
			{
				this.listbox.ItemHeight = this.Font.Height + SystemInformation.BorderSize.Width * 2;
			}

			private bool AllowRemoveInstance(object value)
			{
				return (this.createdItems != null && this.createdItems.Contains(value)) || base.CanRemoveInstance(value);
			}

			private int CalcItemWidth(Graphics g, CollectionEditor.CollectionEditorCollectionForm.ListItem item)
			{
				int num = this.listbox.Items.Count;
				if (num < 2)
				{
					num = 2;
				}
				SizeF sizeF = g.MeasureString(num.ToString(CultureInfo.CurrentCulture), this.listbox.Font);
				int num2 = (int)(Math.Log((double)(num - 1)) / CollectionEditor.CollectionEditorCollectionForm.LOG10) + 1;
				int num3 = 4 + num2 * (this.Font.Height / 2);
				num3 = Math.Max(num3, (int)Math.Ceiling((double)sizeF.Width));
				num3 += SystemInformation.BorderSize.Width * 4;
				SizeF sizeF2 = g.MeasureString(this.GetDisplayText(item), this.listbox.Font);
				int num4 = 0;
				if (item.Editor != null && item.Editor.GetPaintValueSupported())
				{
					num4 = 21;
				}
				return (int)Math.Ceiling((double)sizeF2.Width) + num3 + num4 + SystemInformation.BorderSize.Width * 4;
			}

			private void CancelButton_click(object sender, EventArgs e)
			{
				try
				{
					this.editor.CancelChanges();
					if (this.CollectionEditable && this.dirty)
					{
						this.dirty = false;
						this.listbox.Items.Clear();
						if (this.createdItems != null)
						{
							object[] array = this.createdItems.ToArray();
							if (array.Length > 0 && array[0] is IComponent && ((IComponent)array[0]).Site != null)
							{
								return;
							}
							for (int i = 0; i < array.Length; i++)
							{
								base.DestroyInstance(array[i]);
							}
							this.createdItems.Clear();
						}
						if (this.removedItems != null)
						{
							this.removedItems.Clear();
						}
						if (this.originalItems != null && this.originalItems.Count > 0)
						{
							object[] array2 = new object[this.originalItems.Count];
							for (int j = 0; j < this.originalItems.Count; j++)
							{
								array2[j] = this.originalItems[j];
							}
							base.Items = array2;
							this.originalItems.Clear();
						}
						else
						{
							base.Items = new object[0];
						}
					}
				}
				catch (Exception ex)
				{
					base.DialogResult = DialogResult.None;
					this.DisplayError(ex);
				}
			}

			private void CreateAndAddInstance(Type type)
			{
				try
				{
					object obj = base.CreateInstance(type);
					IList objectsFromInstance = this.editor.GetObjectsFromInstance(obj);
					if (objectsFromInstance != null)
					{
						this.AddItems(objectsFromInstance);
					}
				}
				catch (Exception ex)
				{
					this.DisplayError(ex);
				}
			}

			private void DownButton_click(object sender, EventArgs e)
			{
				try
				{
					this.SuspendEnabledUpdates();
					this.dirty = true;
					int selectedIndex = this.listbox.SelectedIndex;
					if (selectedIndex != this.listbox.Items.Count - 1)
					{
						int topIndex = this.listbox.TopIndex;
						object obj = this.listbox.Items[selectedIndex];
						this.listbox.Items[selectedIndex] = this.listbox.Items[selectedIndex + 1];
						this.listbox.Items[selectedIndex + 1] = obj;
						if (topIndex < this.listbox.Items.Count - 1)
						{
							this.listbox.TopIndex = topIndex + 1;
						}
						this.listbox.ClearSelected();
						this.listbox.SelectedIndex = selectedIndex + 1;
						Control control = (Control)sender;
						if (control.Enabled)
						{
							control.Focus();
						}
					}
				}
				finally
				{
					this.ResumeEnabledUpdates(true);
				}
			}

			private void CollectionEditor_HelpButtonClicked(object sender, CancelEventArgs e)
			{
				e.Cancel = true;
				this.editor.ShowHelp();
			}

			private void Form_HelpRequested(object sender, HelpEventArgs e)
			{
				this.editor.ShowHelp();
			}

			private string GetDisplayText(CollectionEditor.CollectionEditorCollectionForm.ListItem item)
			{
				if (item != null)
				{
					return item.ToString();
				}
				return string.Empty;
			}

			private void HookEvents()
			{
				this.listbox.KeyDown += this.Listbox_keyDown;
				this.listbox.DrawItem += this.Listbox_drawItem;
				this.listbox.SelectedIndexChanged += this.Listbox_selectedIndexChanged;
				this.listbox.HandleCreated += this.Listbox_handleCreated;
				this.upButton.Click += this.UpButton_click;
				this.downButton.Click += this.DownButton_click;
				this.propertyBrowser.PropertyValueChanged += this.PropertyGrid_propertyValueChanged;
				this.addButton.Click += this.AddButton_click;
				this.removeButton.Click += this.RemoveButton_click;
				this.okButton.Click += this.OKButton_click;
				this.cancelButton.Click += this.CancelButton_click;
				base.HelpButtonClicked += this.CollectionEditor_HelpButtonClicked;
				base.HelpRequested += this.Form_HelpRequested;
			}

			private void InitializeComponent()
			{
				ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CollectionEditor));
				this.membersLabel = new Label();
				this.listbox = new CollectionEditor.FilterListBox();
				this.upButton = new Button();
				this.downButton = new Button();
				this.propertiesLabel = new Label();
				this.propertyBrowser = new VsPropertyGrid(base.Context);
				this.addButton = new CollectionEditor.SplitButton();
				this.removeButton = new Button();
				this.okButton = new Button();
				this.cancelButton = new Button();
				this.okCancelTableLayoutPanel = new TableLayoutPanel();
				this.overArchingTableLayoutPanel = new TableLayoutPanel();
				this.addRemoveTableLayoutPanel = new TableLayoutPanel();
				this.okCancelTableLayoutPanel.SuspendLayout();
				this.overArchingTableLayoutPanel.SuspendLayout();
				this.addRemoveTableLayoutPanel.SuspendLayout();
				base.SuspendLayout();
				componentResourceManager.ApplyResources(this.membersLabel, "membersLabel");
				this.membersLabel.Name = "membersLabel";
				componentResourceManager.ApplyResources(this.listbox, "listbox");
				this.listbox.SelectionMode = (this.CanSelectMultipleInstances() ? SelectionMode.MultiExtended : SelectionMode.One);
				this.listbox.DrawMode = DrawMode.OwnerDrawFixed;
				this.listbox.FormattingEnabled = true;
				this.listbox.Name = "listbox";
				this.overArchingTableLayoutPanel.SetRowSpan(this.listbox, 2);
				componentResourceManager.ApplyResources(this.upButton, "upButton");
				this.upButton.Name = "upButton";
				componentResourceManager.ApplyResources(this.downButton, "downButton");
				this.downButton.Name = "downButton";
				componentResourceManager.ApplyResources(this.propertiesLabel, "propertiesLabel");
				this.propertiesLabel.AutoEllipsis = true;
				this.propertiesLabel.Name = "propertiesLabel";
				componentResourceManager.ApplyResources(this.propertyBrowser, "propertyBrowser");
				this.propertyBrowser.CommandsVisibleIfAvailable = false;
				this.propertyBrowser.Name = "propertyBrowser";
				this.overArchingTableLayoutPanel.SetRowSpan(this.propertyBrowser, 3);
				componentResourceManager.ApplyResources(this.addButton, "addButton");
				this.addButton.Name = "addButton";
				componentResourceManager.ApplyResources(this.removeButton, "removeButton");
				this.removeButton.Name = "removeButton";
				componentResourceManager.ApplyResources(this.okButton, "okButton");
				this.okButton.DialogResult = DialogResult.OK;
				this.okButton.Name = "okButton";
				componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
				this.cancelButton.DialogResult = DialogResult.Cancel;
				this.cancelButton.Name = "cancelButton";
				componentResourceManager.ApplyResources(this.okCancelTableLayoutPanel, "okCancelTableLayoutPanel");
				this.overArchingTableLayoutPanel.SetColumnSpan(this.okCancelTableLayoutPanel, 3);
				this.okCancelTableLayoutPanel.Controls.Add(this.okButton, 0, 0);
				this.okCancelTableLayoutPanel.Controls.Add(this.cancelButton, 1, 0);
				this.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel";
				componentResourceManager.ApplyResources(this.overArchingTableLayoutPanel, "overArchingTableLayoutPanel");
				this.overArchingTableLayoutPanel.Controls.Add(this.downButton, 1, 2);
				this.overArchingTableLayoutPanel.Controls.Add(this.addRemoveTableLayoutPanel, 0, 3);
				this.overArchingTableLayoutPanel.Controls.Add(this.propertiesLabel, 2, 0);
				this.overArchingTableLayoutPanel.Controls.Add(this.membersLabel, 0, 0);
				this.overArchingTableLayoutPanel.Controls.Add(this.listbox, 0, 1);
				this.overArchingTableLayoutPanel.Controls.Add(this.propertyBrowser, 2, 1);
				this.overArchingTableLayoutPanel.Controls.Add(this.okCancelTableLayoutPanel, 0, 4);
				this.overArchingTableLayoutPanel.Controls.Add(this.upButton, 1, 1);
				this.overArchingTableLayoutPanel.Name = "overArchingTableLayoutPanel";
				componentResourceManager.ApplyResources(this.addRemoveTableLayoutPanel, "addRemoveTableLayoutPanel");
				this.addRemoveTableLayoutPanel.Controls.Add(this.addButton, 0, 0);
				this.addRemoveTableLayoutPanel.Controls.Add(this.removeButton, 2, 0);
				this.addRemoveTableLayoutPanel.Margin = new Padding(0, 3, 3, 3);
				this.addRemoveTableLayoutPanel.Name = "addRemoveTableLayoutPanel";
				base.AcceptButton = this.okButton;
				componentResourceManager.ApplyResources(this, "$this");
				base.AutoScaleMode = AutoScaleMode.Font;
				base.CancelButton = this.cancelButton;
				base.Controls.Add(this.overArchingTableLayoutPanel);
				base.HelpButton = true;
				base.MaximizeBox = false;
				base.MinimizeBox = false;
				base.Name = "CollectionEditor";
				base.ShowIcon = false;
				base.ShowInTaskbar = false;
				this.okCancelTableLayoutPanel.ResumeLayout(false);
				this.okCancelTableLayoutPanel.PerformLayout();
				this.overArchingTableLayoutPanel.ResumeLayout(false);
				this.overArchingTableLayoutPanel.PerformLayout();
				this.addRemoveTableLayoutPanel.ResumeLayout(false);
				this.addRemoveTableLayoutPanel.PerformLayout();
				base.ResumeLayout(false);
			}

			private void UpdateItemWidths(CollectionEditor.CollectionEditorCollectionForm.ListItem item)
			{
				if (!this.listbox.IsHandleCreated)
				{
					return;
				}
				using (Graphics graphics = this.listbox.CreateGraphics())
				{
					int horizontalExtent = this.listbox.HorizontalExtent;
					if (item != null)
					{
						int num = this.CalcItemWidth(graphics, item);
						if (num > horizontalExtent)
						{
							this.listbox.HorizontalExtent = num;
						}
					}
					else
					{
						int num2 = 0;
						foreach (object obj in this.listbox.Items)
						{
							CollectionEditor.CollectionEditorCollectionForm.ListItem listItem = (CollectionEditor.CollectionEditorCollectionForm.ListItem)obj;
							int num3 = this.CalcItemWidth(graphics, listItem);
							if (num3 > num2)
							{
								num2 = num3;
							}
						}
						this.listbox.HorizontalExtent = num2;
					}
				}
			}

			private void Listbox_drawItem(object sender, DrawItemEventArgs e)
			{
				if (e.Index != -1)
				{
					CollectionEditor.CollectionEditorCollectionForm.ListItem listItem = (CollectionEditor.CollectionEditorCollectionForm.ListItem)this.listbox.Items[e.Index];
					Graphics graphics = e.Graphics;
					int count = this.listbox.Items.Count;
					int num = ((count > 1) ? (count - 1) : count);
					SizeF sizeF = graphics.MeasureString(num.ToString(CultureInfo.CurrentCulture), this.listbox.Font);
					int num2 = (int)(Math.Log((double)num) / CollectionEditor.CollectionEditorCollectionForm.LOG10) + 1;
					int num3 = 4 + num2 * (this.Font.Height / 2);
					num3 = Math.Max(num3, (int)Math.Ceiling((double)sizeF.Width));
					num3 += SystemInformation.BorderSize.Width * 4;
					Rectangle rectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, num3, e.Bounds.Height);
					ControlPaint.DrawButton(graphics, rectangle, ButtonState.Normal);
					rectangle.Inflate(-SystemInformation.BorderSize.Width * 2, -SystemInformation.BorderSize.Height * 2);
					int num4 = num3;
					Color color = SystemColors.Window;
					Color color2 = SystemColors.WindowText;
					if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
					{
						color = SystemColors.Highlight;
						color2 = SystemColors.HighlightText;
					}
					Rectangle rectangle2 = new Rectangle(e.Bounds.X + num4, e.Bounds.Y, e.Bounds.Width - num4, e.Bounds.Height);
					graphics.FillRectangle(new SolidBrush(color), rectangle2);
					if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
					{
						ControlPaint.DrawFocusRectangle(graphics, rectangle2);
					}
					num4 += 2;
					if (listItem.Editor != null && listItem.Editor.GetPaintValueSupported())
					{
						Rectangle rectangle3 = new Rectangle(e.Bounds.X + num4, e.Bounds.Y + 1, 20, e.Bounds.Height - 3);
						graphics.DrawRectangle(SystemPens.ControlText, rectangle3.X, rectangle3.Y, rectangle3.Width - 1, rectangle3.Height - 1);
						rectangle3.Inflate(-1, -1);
						listItem.Editor.PaintValue(listItem.Value, graphics, rectangle3);
						num4 += 27;
					}
					using (StringFormat stringFormat = new StringFormat())
					{
						stringFormat.Alignment = StringAlignment.Center;
						graphics.DrawString(e.Index.ToString(CultureInfo.CurrentCulture), this.Font, SystemBrushes.ControlText, new Rectangle(e.Bounds.X, e.Bounds.Y, num3, e.Bounds.Height), stringFormat);
					}
					Brush brush = new SolidBrush(color2);
					string displayText = this.GetDisplayText(listItem);
					try
					{
						graphics.DrawString(displayText, this.Font, brush, new Rectangle(e.Bounds.X + num4, e.Bounds.Y, e.Bounds.Width - num4, e.Bounds.Height));
					}
					finally
					{
						if (brush != null)
						{
							brush.Dispose();
						}
					}
					int num5 = num4 + (int)graphics.MeasureString(displayText, this.Font).Width;
					if (num5 > e.Bounds.Width && this.listbox.HorizontalExtent < num5)
					{
						this.listbox.HorizontalExtent = num5;
					}
				}
			}

			private void Listbox_keyDown(object sender, KeyEventArgs kevent)
			{
				switch (kevent.KeyData)
				{
				case Keys.Insert:
					this.PerformAdd();
					return;
				case Keys.Delete:
					this.PerformRemove();
					return;
				default:
					return;
				}
			}

			private void Listbox_selectedIndexChanged(object sender, EventArgs e)
			{
				this.UpdateEnabled();
			}

			private void Listbox_handleCreated(object sender, EventArgs e)
			{
				this.UpdateItemWidths(null);
			}

			private void OKButton_click(object sender, EventArgs e)
			{
				try
				{
					if (!this.dirty || !this.CollectionEditable)
					{
						this.dirty = false;
						base.DialogResult = DialogResult.Cancel;
					}
					else
					{
						if (this.dirty)
						{
							object[] array = new object[this.listbox.Items.Count];
							for (int i = 0; i < array.Length; i++)
							{
								array[i] = ((CollectionEditor.CollectionEditorCollectionForm.ListItem)this.listbox.Items[i]).Value;
							}
							base.Items = array;
						}
						if (this.removedItems != null && this.dirty)
						{
							object[] array2 = this.removedItems.ToArray();
							for (int j = 0; j < array2.Length; j++)
							{
								base.DestroyInstance(array2[j]);
							}
							this.removedItems.Clear();
						}
						if (this.createdItems != null)
						{
							this.createdItems.Clear();
						}
						if (this.originalItems != null)
						{
							this.originalItems.Clear();
						}
						this.listbox.Items.Clear();
						this.dirty = false;
					}
				}
				catch (Exception ex)
				{
					base.DialogResult = DialogResult.None;
					this.DisplayError(ex);
				}
			}

			private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
			{
				if (!this.dirty)
				{
					foreach (object obj in this.originalItems)
					{
						if (obj == e.Component)
						{
							this.dirty = true;
							break;
						}
					}
				}
			}

			protected override void OnEditValueChanged()
			{
				if (this.originalItems == null)
				{
					this.originalItems = new ArrayList();
				}
				this.originalItems.Clear();
				this.listbox.Items.Clear();
				this.propertyBrowser.Site = new CollectionEditor.PropertyGridSite(base.Context, this.propertyBrowser);
				if (base.EditValue != null)
				{
					this.SuspendEnabledUpdates();
					try
					{
						object[] items = base.Items;
						for (int i = 0; i < items.Length; i++)
						{
							this.listbox.Items.Add(new CollectionEditor.CollectionEditorCollectionForm.ListItem(this.editor, items[i]));
							this.originalItems.Add(items[i]);
						}
						if (this.listbox.Items.Count > 0)
						{
							this.listbox.SelectedIndex = 0;
						}
						goto IL_00CA;
					}
					finally
					{
						this.ResumeEnabledUpdates(true);
					}
				}
				this.UpdateEnabled();
				IL_00CA:
				this.AdjustListBoxItemHeight();
				this.UpdateItemWidths(null);
			}

			protected override void OnFontChanged(EventArgs e)
			{
				base.OnFontChanged(e);
				this.AdjustListBoxItemHeight();
			}

			private void PerformAdd()
			{
				this.CreateAndAddInstance(base.NewItemTypes[0]);
			}

			private void PerformRemove()
			{
				int selectedIndex = this.listbox.SelectedIndex;
				if (selectedIndex != -1)
				{
					this.SuspendEnabledUpdates();
					try
					{
						if (this.listbox.SelectedItems.Count > 1)
						{
							ArrayList arrayList = new ArrayList(this.listbox.SelectedItems);
							using (IEnumerator enumerator = arrayList.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									object obj = enumerator.Current;
									CollectionEditor.CollectionEditorCollectionForm.ListItem listItem = (CollectionEditor.CollectionEditorCollectionForm.ListItem)obj;
									this.RemoveInternal(listItem);
								}
								goto IL_008D;
							}
						}
						this.RemoveInternal((CollectionEditor.CollectionEditorCollectionForm.ListItem)this.listbox.SelectedItem);
						IL_008D:
						if (selectedIndex < this.listbox.Items.Count)
						{
							this.listbox.SelectedIndex = selectedIndex;
						}
						else if (this.listbox.Items.Count > 0)
						{
							this.listbox.SelectedIndex = this.listbox.Items.Count - 1;
						}
					}
					finally
					{
						this.ResumeEnabledUpdates(true);
					}
				}
			}

			private void PropertyGrid_propertyValueChanged(object sender, PropertyValueChangedEventArgs e)
			{
				this.dirty = true;
				this.SuspendEnabledUpdates();
				try
				{
					this.listbox.RefreshItem(this.listbox.SelectedIndex);
				}
				finally
				{
					this.ResumeEnabledUpdates(false);
				}
				this.UpdateItemWidths(null);
				this.listbox.Invalidate();
				this.propertiesLabel.Text = SR.GetString("CollectionEditorProperties", new object[] { this.GetDisplayText((CollectionEditor.CollectionEditorCollectionForm.ListItem)this.listbox.SelectedItem) });
			}

			private void RemoveInternal(CollectionEditor.CollectionEditorCollectionForm.ListItem item)
			{
				if (item != null)
				{
					this.editor.OnItemRemoving(item.Value);
					this.dirty = true;
					if (this.createdItems != null && this.createdItems.Contains(item.Value))
					{
						base.DestroyInstance(item.Value);
						this.createdItems.Remove(item.Value);
						this.listbox.Items.Remove(item);
					}
					else
					{
						try
						{
							if (!base.CanRemoveInstance(item.Value))
							{
								throw new Exception(SR.GetString("CollectionEditorCantRemoveItem", new object[] { this.GetDisplayText(item) }));
							}
							if (this.removedItems == null)
							{
								this.removedItems = new ArrayList();
							}
							this.removedItems.Add(item.Value);
							this.listbox.Items.Remove(item);
						}
						catch (Exception ex)
						{
							this.DisplayError(ex);
						}
					}
					this.UpdateItemWidths(null);
				}
			}

			private void RemoveButton_click(object sender, EventArgs e)
			{
				this.PerformRemove();
				Control control = (Control)sender;
				if (control.Enabled)
				{
					control.Focus();
				}
			}

			private void ResumeEnabledUpdates(bool updateNow)
			{
				this.suspendEnabledCount--;
				if (updateNow)
				{
					this.UpdateEnabled();
					return;
				}
				base.BeginInvoke(new MethodInvoker(this.UpdateEnabled));
			}

			private void SuspendEnabledUpdates()
			{
				this.suspendEnabledCount++;
			}

			protected internal override DialogResult ShowEditorDialog(IWindowsFormsEditorService edSvc)
			{
				IComponentChangeService componentChangeService = null;
				DialogResult dialogResult = DialogResult.OK;
				try
				{
					componentChangeService = (IComponentChangeService)this.editor.Context.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.ComponentChanged += this.OnComponentChanged;
					}
					base.ActiveControl = this.listbox;
					dialogResult = base.ShowEditorDialog(edSvc);
				}
				finally
				{
					if (componentChangeService != null)
					{
						componentChangeService.ComponentChanged -= this.OnComponentChanged;
					}
				}
				return dialogResult;
			}

			private void UpButton_click(object sender, EventArgs e)
			{
				int selectedIndex = this.listbox.SelectedIndex;
				if (selectedIndex == 0)
				{
					return;
				}
				this.dirty = true;
				try
				{
					this.SuspendEnabledUpdates();
					int topIndex = this.listbox.TopIndex;
					object obj = this.listbox.Items[selectedIndex];
					this.listbox.Items[selectedIndex] = this.listbox.Items[selectedIndex - 1];
					this.listbox.Items[selectedIndex - 1] = obj;
					if (topIndex > 0)
					{
						this.listbox.TopIndex = topIndex - 1;
					}
					this.listbox.ClearSelected();
					this.listbox.SelectedIndex = selectedIndex - 1;
					Control control = (Control)sender;
					if (control.Enabled)
					{
						control.Focus();
					}
				}
				finally
				{
					this.ResumeEnabledUpdates(true);
				}
			}

			private void UpdateEnabled()
			{
				if (this.suspendEnabledCount > 0)
				{
					return;
				}
				bool flag = this.listbox.SelectedItem != null && this.CollectionEditable;
				this.removeButton.Enabled = flag && this.AllowRemoveInstance(((CollectionEditor.CollectionEditorCollectionForm.ListItem)this.listbox.SelectedItem).Value);
				this.upButton.Enabled = flag && this.listbox.Items.Count > 1;
				this.downButton.Enabled = flag && this.listbox.Items.Count > 1;
				this.propertyBrowser.Enabled = flag;
				this.addButton.Enabled = this.CollectionEditable;
				if (this.listbox.SelectedItem == null)
				{
					this.propertiesLabel.Text = SR.GetString("CollectionEditorPropertiesNone");
					this.propertyBrowser.SelectedObject = null;
					return;
				}
				object[] array;
				if (this.IsImmutable)
				{
					array = new object[]
					{
						new CollectionEditor.CollectionEditorCollectionForm.SelectionWrapper(base.CollectionType, base.CollectionItemType, this.listbox, this.listbox.SelectedItems)
					};
				}
				else
				{
					array = new object[this.listbox.SelectedItems.Count];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = ((CollectionEditor.CollectionEditorCollectionForm.ListItem)this.listbox.SelectedItems[i]).Value;
					}
				}
				int count = this.listbox.SelectedItems.Count;
				if (count == 1 || count == -1)
				{
					this.propertiesLabel.Text = SR.GetString("CollectionEditorProperties", new object[] { this.GetDisplayText((CollectionEditor.CollectionEditorCollectionForm.ListItem)this.listbox.SelectedItem) });
				}
				else
				{
					this.propertiesLabel.Text = SR.GetString("CollectionEditorPropertiesMultiSelect");
				}
				if (this.editor.IsAnyObjectInheritedReadOnly(array))
				{
					this.propertyBrowser.SelectedObjects = null;
					this.propertyBrowser.Enabled = false;
					this.removeButton.Enabled = false;
					this.upButton.Enabled = false;
					this.downButton.Enabled = false;
					this.propertiesLabel.Text = SR.GetString("CollectionEditorInheritedReadOnlySelection");
					return;
				}
				this.propertyBrowser.Enabled = true;
				this.propertyBrowser.SelectedObjects = array;
			}

			private const int TEXT_INDENT = 1;

			private const int PAINT_WIDTH = 20;

			private const int PAINT_INDENT = 26;

			private static readonly double LOG10 = Math.Log(10.0);

			private ArrayList createdItems;

			private ArrayList removedItems;

			private ArrayList originalItems;

			private CollectionEditor editor;

			private CollectionEditor.FilterListBox listbox;

			private CollectionEditor.SplitButton addButton;

			private Button removeButton;

			private Button cancelButton;

			private Button okButton;

			private Button downButton;

			private Button upButton;

			private VsPropertyGrid propertyBrowser;

			private Label membersLabel;

			private Label propertiesLabel;

			private ContextMenuStrip addDownMenu;

			private TableLayoutPanel okCancelTableLayoutPanel;

			private TableLayoutPanel overArchingTableLayoutPanel;

			private TableLayoutPanel addRemoveTableLayoutPanel;

			private int suspendEnabledCount;

			private bool dirty;

			private class SelectionWrapper : PropertyDescriptor, ICustomTypeDescriptor
			{
				public SelectionWrapper(Type collectionType, Type collectionItemType, Control control, ICollection collection)
					: base("Value", new Attribute[]
					{
						new CategoryAttribute(collectionItemType.Name)
					})
				{
					this.collectionType = collectionType;
					this.collectionItemType = collectionItemType;
					this.control = control;
					this.collection = collection;
					this.properties = new PropertyDescriptorCollection(new PropertyDescriptor[] { this });
					this.value = this;
					foreach (object obj in collection)
					{
						CollectionEditor.CollectionEditorCollectionForm.ListItem listItem = (CollectionEditor.CollectionEditorCollectionForm.ListItem)obj;
						if (this.value == this)
						{
							this.value = listItem.Value;
						}
						else
						{
							object obj2 = listItem.Value;
							if (this.value != null)
							{
								if (obj2 == null)
								{
									this.value = null;
									break;
								}
								if (!this.value.Equals(obj2))
								{
									this.value = null;
									break;
								}
							}
							else if (obj2 != null)
							{
								this.value = null;
								break;
							}
						}
					}
				}

				public override Type ComponentType
				{
					get
					{
						return this.collectionType;
					}
				}

				public override bool IsReadOnly
				{
					get
					{
						return false;
					}
				}

				public override Type PropertyType
				{
					get
					{
						return this.collectionItemType;
					}
				}

				public override bool CanResetValue(object component)
				{
					return false;
				}

				public override object GetValue(object component)
				{
					return this.value;
				}

				public override void ResetValue(object component)
				{
				}

				public override void SetValue(object component, object value)
				{
					this.value = value;
					foreach (object obj in this.collection)
					{
						CollectionEditor.CollectionEditorCollectionForm.ListItem listItem = (CollectionEditor.CollectionEditorCollectionForm.ListItem)obj;
						listItem.Value = value;
					}
					this.control.Invalidate();
					this.OnValueChanged(component, EventArgs.Empty);
				}

				public override bool ShouldSerializeValue(object component)
				{
					return false;
				}

				AttributeCollection ICustomTypeDescriptor.GetAttributes()
				{
					return TypeDescriptor.GetAttributes(this.collectionItemType);
				}

				string ICustomTypeDescriptor.GetClassName()
				{
					return this.collectionItemType.Name;
				}

				string ICustomTypeDescriptor.GetComponentName()
				{
					return null;
				}

				TypeConverter ICustomTypeDescriptor.GetConverter()
				{
					return null;
				}

				EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
				{
					return null;
				}

				PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
				{
					return this;
				}

				object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
				{
					return null;
				}

				EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
				{
					return EventDescriptorCollection.Empty;
				}

				EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
				{
					return EventDescriptorCollection.Empty;
				}

				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
				{
					return this.properties;
				}

				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
				{
					return this.properties;
				}

				object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
				{
					return this;
				}

				private Type collectionType;

				private Type collectionItemType;

				private Control control;

				private ICollection collection;

				private PropertyDescriptorCollection properties;

				private object value;
			}

			private class ListItem
			{
				public ListItem(CollectionEditor parentCollectionEditor, object value)
				{
					this.value = value;
					this.parentCollectionEditor = parentCollectionEditor;
				}

				public override string ToString()
				{
					return this.parentCollectionEditor.GetDisplayText(this.value);
				}

				public UITypeEditor Editor
				{
					get
					{
						if (this.uiTypeEditor == null)
						{
							this.uiTypeEditor = TypeDescriptor.GetEditor(this.value, typeof(UITypeEditor));
							if (this.uiTypeEditor == null)
							{
								this.uiTypeEditor = this;
							}
						}
						if (this.uiTypeEditor != this)
						{
							return (UITypeEditor)this.uiTypeEditor;
						}
						return null;
					}
				}

				public object Value
				{
					get
					{
						return this.value;
					}
					set
					{
						this.uiTypeEditor = null;
						this.value = value;
					}
				}

				private object value;

				private object uiTypeEditor;

				private CollectionEditor parentCollectionEditor;
			}

			private class TypeMenuItem : ToolStripMenuItem
			{
				public TypeMenuItem(Type itemType, EventHandler handler)
					: base(itemType.Name, null, handler)
				{
					this.itemType = itemType;
				}

				public Type ItemType
				{
					get
					{
						return this.itemType;
					}
				}

				private Type itemType;
			}
		}

		internal class FilterListBox : ListBox
		{
			private PropertyGrid PropertyGrid
			{
				get
				{
					if (this.grid == null)
					{
						foreach (object obj in base.Parent.Controls)
						{
							Control control = (Control)obj;
							if (control is PropertyGrid)
							{
								this.grid = (PropertyGrid)control;
								break;
							}
						}
					}
					return this.grid;
				}
			}

			public new void RefreshItem(int index)
			{
				base.RefreshItem(index);
			}

			protected override void WndProc(ref Message m)
			{
				switch (m.Msg)
				{
				case 256:
					this.lastKeyDown = m;
					if ((int)m.WParam == 229 && this.PropertyGrid != null)
					{
						this.PropertyGrid.Focus();
						UnsafeNativeMethods.SetFocus(new HandleRef(this.PropertyGrid, this.PropertyGrid.Handle));
						Application.DoEvents();
						if (this.PropertyGrid.Focused || this.PropertyGrid.ContainsFocus)
						{
							NativeMethods.SendMessage(UnsafeNativeMethods.GetFocus(), 256, this.lastKeyDown.WParam, this.lastKeyDown.LParam);
						}
					}
					break;
				case 258:
					if ((Control.ModifierKeys & (Keys.Control | Keys.Alt)) == Keys.None && this.PropertyGrid != null)
					{
						this.PropertyGrid.Focus();
						UnsafeNativeMethods.SetFocus(new HandleRef(this.PropertyGrid, this.PropertyGrid.Handle));
						Application.DoEvents();
						if (this.PropertyGrid.Focused || this.PropertyGrid.ContainsFocus)
						{
							IntPtr focus = UnsafeNativeMethods.GetFocus();
							NativeMethods.SendMessage(focus, 256, this.lastKeyDown.WParam, this.lastKeyDown.LParam);
							NativeMethods.SendMessage(focus, 258, m.WParam, m.LParam);
							return;
						}
					}
					break;
				}
				base.WndProc(ref m);
			}

			private PropertyGrid grid;

			private Message lastKeyDown;
		}

		internal class PropertyGridSite : ISite, IServiceProvider
		{
			public PropertyGridSite(IServiceProvider sp, IComponent comp)
			{
				this.sp = sp;
				this.comp = comp;
			}

			public IComponent Component
			{
				get
				{
					return this.comp;
				}
			}

			public IContainer Container
			{
				get
				{
					return null;
				}
			}

			public bool DesignMode
			{
				get
				{
					return false;
				}
			}

			public string Name
			{
				get
				{
					return null;
				}
				set
				{
				}
			}

			public object GetService(Type t)
			{
				if (!this.inGetService && this.sp != null)
				{
					try
					{
						this.inGetService = true;
						return this.sp.GetService(t);
					}
					finally
					{
						this.inGetService = false;
					}
				}
				return null;
			}

			private IServiceProvider sp;

			private IComponent comp;

			private bool inGetService;
		}
	}
}

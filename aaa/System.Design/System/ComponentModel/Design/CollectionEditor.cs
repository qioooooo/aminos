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
	// Token: 0x020000E6 RID: 230
	public class CollectionEditor : UITypeEditor
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x00022E60 File Offset: 0x00021E60
		protected virtual void CancelChanges()
		{
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00022E62 File Offset: 0x00021E62
		public CollectionEditor(Type type)
		{
			this.type = type;
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x00022E71 File Offset: 0x00021E71
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

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x00022E8D File Offset: 0x00021E8D
		protected Type CollectionType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x00022E95 File Offset: 0x00021E95
		protected ITypeDescriptorContext Context
		{
			get
			{
				return this.currentContext;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x00022E9D File Offset: 0x00021E9D
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

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x00022EB9 File Offset: 0x00021EB9
		protected virtual string HelpTopic
		{
			get
			{
				return "net.ComponentModel.CollectionEditor";
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00022EC0 File Offset: 0x00021EC0
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

		// Token: 0x0600096C RID: 2412 RVA: 0x00022F01 File Offset: 0x00021F01
		protected virtual bool CanSelectMultipleInstances()
		{
			return true;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00022F04 File Offset: 0x00021F04
		protected virtual CollectionEditor.CollectionForm CreateCollectionForm()
		{
			return new CollectionEditor.CollectionEditorCollectionForm(this);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00022F0C File Offset: 0x00021F0C
		protected virtual object CreateInstance(Type itemType)
		{
			return CollectionEditor.CreateInstance(itemType, (IDesignerHost)this.GetService(typeof(IDesignerHost)), null);
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00022F2C File Offset: 0x00021F2C
		protected virtual IList GetObjectsFromInstance(object instance)
		{
			return new ArrayList { instance };
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00022F48 File Offset: 0x00021F48
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

		// Token: 0x06000971 RID: 2417 RVA: 0x00022FA4 File Offset: 0x00021FA4
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

		// Token: 0x06000972 RID: 2418 RVA: 0x0002305C File Offset: 0x0002205C
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

		// Token: 0x06000973 RID: 2419 RVA: 0x000230C8 File Offset: 0x000220C8
		protected virtual Type[] CreateNewItemTypes()
		{
			return new Type[] { this.CollectionItemType };
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x000230E8 File Offset: 0x000220E8
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

		// Token: 0x06000975 RID: 2421 RVA: 0x00023138 File Offset: 0x00022138
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

		// Token: 0x06000976 RID: 2422 RVA: 0x000232D0 File Offset: 0x000222D0
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x000232D4 File Offset: 0x000222D4
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

		// Token: 0x06000978 RID: 2424 RVA: 0x0002335C File Offset: 0x0002235C
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

		// Token: 0x06000979 RID: 2425 RVA: 0x000233E8 File Offset: 0x000223E8
		protected object GetService(Type serviceType)
		{
			if (this.Context != null)
			{
				return this.Context.GetService(serviceType);
			}
			return null;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00023400 File Offset: 0x00022400
		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (!this.ignoreChangedEvents && sender != this.Context.Instance)
			{
				this.ignoreChangedEvents = true;
				this.Context.OnComponentChanged();
			}
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0002342A File Offset: 0x0002242A
		private void OnComponentChanging(object sender, ComponentChangingEventArgs e)
		{
			if (!this.ignoreChangingEvents && sender != this.Context.Instance)
			{
				this.ignoreChangingEvents = true;
				this.Context.OnComponentChanging();
			}
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00023455 File Offset: 0x00022455
		internal virtual void OnItemRemoving(object item)
		{
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x00023458 File Offset: 0x00022458
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

		// Token: 0x0600097E RID: 2430 RVA: 0x000234AC File Offset: 0x000224AC
		protected virtual void ShowHelp()
		{
			IHelpService helpService = this.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword(this.HelpTopic);
			}
		}

		// Token: 0x04000D15 RID: 3349
		private Type type;

		// Token: 0x04000D16 RID: 3350
		private Type collectionItemType;

		// Token: 0x04000D17 RID: 3351
		private Type[] newItemTypes;

		// Token: 0x04000D18 RID: 3352
		private ITypeDescriptorContext currentContext;

		// Token: 0x04000D19 RID: 3353
		private bool ignoreChangedEvents;

		// Token: 0x04000D1A RID: 3354
		private bool ignoreChangingEvents;

		// Token: 0x020000E7 RID: 231
		internal class SplitButton : Button
		{
			// Token: 0x17000146 RID: 326
			// (set) Token: 0x0600097F RID: 2431 RVA: 0x000234DE File Offset: 0x000224DE
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

			// Token: 0x17000147 RID: 327
			// (get) Token: 0x06000980 RID: 2432 RVA: 0x000234F6 File Offset: 0x000224F6
			// (set) Token: 0x06000981 RID: 2433 RVA: 0x000234FE File Offset: 0x000224FE
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

			// Token: 0x06000982 RID: 2434 RVA: 0x00023528 File Offset: 0x00022528
			public override Size GetPreferredSize(Size proposedSize)
			{
				Size preferredSize = base.GetPreferredSize(proposedSize);
				if (this.showSplit && !string.IsNullOrEmpty(this.Text) && TextRenderer.MeasureText(this.Text, this.Font).Width + 14 > preferredSize.Width)
				{
					return preferredSize + new Size(14, 0);
				}
				return preferredSize;
			}

			// Token: 0x06000983 RID: 2435 RVA: 0x00023587 File Offset: 0x00022587
			protected override bool IsInputKey(Keys keyData)
			{
				return (keyData.Equals(Keys.Down) && this.showSplit) || base.IsInputKey(keyData);
			}

			// Token: 0x06000984 RID: 2436 RVA: 0x000235B0 File Offset: 0x000225B0
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

			// Token: 0x06000985 RID: 2437 RVA: 0x00023604 File Offset: 0x00022604
			protected override void OnKeyDown(KeyEventArgs kevent)
			{
				if (kevent.KeyCode.Equals(Keys.Down) && this.showSplit)
				{
					this.ShowContextMenuStrip();
				}
			}

			// Token: 0x06000986 RID: 2438 RVA: 0x00023630 File Offset: 0x00022630
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

			// Token: 0x06000987 RID: 2439 RVA: 0x00023684 File Offset: 0x00022684
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

			// Token: 0x06000988 RID: 2440 RVA: 0x000236B8 File Offset: 0x000226B8
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

			// Token: 0x06000989 RID: 2441 RVA: 0x0002370C File Offset: 0x0002270C
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

			// Token: 0x0600098A RID: 2442 RVA: 0x00023770 File Offset: 0x00022770
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

			// Token: 0x0600098B RID: 2443 RVA: 0x000237E8 File Offset: 0x000227E8
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

			// Token: 0x0600098C RID: 2444 RVA: 0x000239B8 File Offset: 0x000229B8
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

			// Token: 0x0600098D RID: 2445 RVA: 0x00023A8B File Offset: 0x00022A8B
			private void ShowContextMenuStrip()
			{
				this.State = PushButtonState.Pressed;
				if (this.ContextMenuStrip != null)
				{
					this.ContextMenuStrip.Closed += this.ContextMenuStrip_Closed;
					this.ContextMenuStrip.Show(this, 0, base.Height);
				}
			}

			// Token: 0x0600098E RID: 2446 RVA: 0x00023AC8 File Offset: 0x00022AC8
			private void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
			{
				ContextMenuStrip contextMenuStrip = sender as ContextMenuStrip;
				if (contextMenuStrip != null)
				{
					contextMenuStrip.Closed -= this.ContextMenuStrip_Closed;
				}
				this.SetButtonDrawState();
			}

			// Token: 0x0600098F RID: 2447 RVA: 0x00023AF8 File Offset: 0x00022AF8
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

			// Token: 0x04000D1B RID: 3355
			private const int pushButtonWidth = 14;

			// Token: 0x04000D1C RID: 3356
			private PushButtonState _state;

			// Token: 0x04000D1D RID: 3357
			private Rectangle dropDownRectangle = default(Rectangle);

			// Token: 0x04000D1E RID: 3358
			private bool showSplit;
		}

		// Token: 0x020000E8 RID: 232
		protected abstract class CollectionForm : Form
		{
			// Token: 0x06000991 RID: 2449 RVA: 0x00023B58 File Offset: 0x00022B58
			public CollectionForm(CollectionEditor editor)
			{
				this.editor = editor;
			}

			// Token: 0x17000148 RID: 328
			// (get) Token: 0x06000992 RID: 2450 RVA: 0x00023B67 File Offset: 0x00022B67
			protected Type CollectionItemType
			{
				get
				{
					return this.editor.CollectionItemType;
				}
			}

			// Token: 0x17000149 RID: 329
			// (get) Token: 0x06000993 RID: 2451 RVA: 0x00023B74 File Offset: 0x00022B74
			protected Type CollectionType
			{
				get
				{
					return this.editor.CollectionType;
				}
			}

			// Token: 0x1700014A RID: 330
			// (get) Token: 0x06000994 RID: 2452 RVA: 0x00023B84 File Offset: 0x00022B84
			// (set) Token: 0x06000995 RID: 2453 RVA: 0x00023BDB File Offset: 0x00022BDB
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

			// Token: 0x1700014B RID: 331
			// (get) Token: 0x06000996 RID: 2454 RVA: 0x00023BEF File Offset: 0x00022BEF
			protected ITypeDescriptorContext Context
			{
				get
				{
					return this.editor.Context;
				}
			}

			// Token: 0x1700014C RID: 332
			// (get) Token: 0x06000997 RID: 2455 RVA: 0x00023BFC File Offset: 0x00022BFC
			// (set) Token: 0x06000998 RID: 2456 RVA: 0x00023C04 File Offset: 0x00022C04
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

			// Token: 0x1700014D RID: 333
			// (get) Token: 0x06000999 RID: 2457 RVA: 0x00023C13 File Offset: 0x00022C13
			// (set) Token: 0x0600099A RID: 2458 RVA: 0x00023C28 File Offset: 0x00022C28
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

			// Token: 0x1700014E RID: 334
			// (get) Token: 0x0600099B RID: 2459 RVA: 0x00023C9C File Offset: 0x00022C9C
			protected Type[] NewItemTypes
			{
				get
				{
					return this.editor.NewItemTypes;
				}
			}

			// Token: 0x0600099C RID: 2460 RVA: 0x00023CA9 File Offset: 0x00022CA9
			protected bool CanRemoveInstance(object value)
			{
				return this.editor.CanRemoveInstance(value);
			}

			// Token: 0x0600099D RID: 2461 RVA: 0x00023CB7 File Offset: 0x00022CB7
			protected virtual bool CanSelectMultipleInstances()
			{
				return this.editor.CanSelectMultipleInstances();
			}

			// Token: 0x0600099E RID: 2462 RVA: 0x00023CC4 File Offset: 0x00022CC4
			protected object CreateInstance(Type itemType)
			{
				return this.editor.CreateInstance(itemType);
			}

			// Token: 0x0600099F RID: 2463 RVA: 0x00023CD2 File Offset: 0x00022CD2
			protected void DestroyInstance(object instance)
			{
				this.editor.DestroyInstance(instance);
			}

			// Token: 0x060009A0 RID: 2464 RVA: 0x00023CE0 File Offset: 0x00022CE0
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

			// Token: 0x060009A1 RID: 2465 RVA: 0x00023D35 File Offset: 0x00022D35
			protected override object GetService(Type serviceType)
			{
				return this.editor.GetService(serviceType);
			}

			// Token: 0x060009A2 RID: 2466 RVA: 0x00023D43 File Offset: 0x00022D43
			protected internal virtual DialogResult ShowEditorDialog(IWindowsFormsEditorService edSvc)
			{
				return edSvc.ShowDialog(this);
			}

			// Token: 0x060009A3 RID: 2467
			protected abstract void OnEditValueChanged();

			// Token: 0x04000D1F RID: 3359
			private const short EditableDynamic = 0;

			// Token: 0x04000D20 RID: 3360
			private const short EditableYes = 1;

			// Token: 0x04000D21 RID: 3361
			private const short EditableNo = 2;

			// Token: 0x04000D22 RID: 3362
			private CollectionEditor editor;

			// Token: 0x04000D23 RID: 3363
			private object value;

			// Token: 0x04000D24 RID: 3364
			private short editableState;
		}

		// Token: 0x020000E9 RID: 233
		private class CollectionEditorCollectionForm : CollectionEditor.CollectionForm
		{
			// Token: 0x060009A4 RID: 2468 RVA: 0x00023D4C File Offset: 0x00022D4C
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

			// Token: 0x1700014F RID: 335
			// (get) Token: 0x060009A5 RID: 2469 RVA: 0x00023E0C File Offset: 0x00022E0C
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

			// Token: 0x060009A6 RID: 2470 RVA: 0x00023E84 File Offset: 0x00022E84
			private void AddButton_click(object sender, EventArgs e)
			{
				this.PerformAdd();
			}

			// Token: 0x060009A7 RID: 2471 RVA: 0x00023E8C File Offset: 0x00022E8C
			private void AddDownMenu_click(object sender, EventArgs e)
			{
				if (sender is CollectionEditor.CollectionEditorCollectionForm.TypeMenuItem)
				{
					CollectionEditor.CollectionEditorCollectionForm.TypeMenuItem typeMenuItem = (CollectionEditor.CollectionEditorCollectionForm.TypeMenuItem)sender;
					this.CreateAndAddInstance(typeMenuItem.ItemType);
				}
			}

			// Token: 0x060009A8 RID: 2472 RVA: 0x00023EB4 File Offset: 0x00022EB4
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

			// Token: 0x060009A9 RID: 2473 RVA: 0x00024098 File Offset: 0x00023098
			private void AdjustListBoxItemHeight()
			{
				this.listbox.ItemHeight = this.Font.Height + SystemInformation.BorderSize.Width * 2;
			}

			// Token: 0x060009AA RID: 2474 RVA: 0x000240CB File Offset: 0x000230CB
			private bool AllowRemoveInstance(object value)
			{
				return (this.createdItems != null && this.createdItems.Contains(value)) || base.CanRemoveInstance(value);
			}

			// Token: 0x060009AB RID: 2475 RVA: 0x000240EC File Offset: 0x000230EC
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

			// Token: 0x060009AC RID: 2476 RVA: 0x000241D8 File Offset: 0x000231D8
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

			// Token: 0x060009AD RID: 2477 RVA: 0x00024328 File Offset: 0x00023328
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

			// Token: 0x060009AE RID: 2478 RVA: 0x00024370 File Offset: 0x00023370
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

			// Token: 0x060009AF RID: 2479 RVA: 0x00024474 File Offset: 0x00023474
			private void CollectionEditor_HelpButtonClicked(object sender, CancelEventArgs e)
			{
				e.Cancel = true;
				this.editor.ShowHelp();
			}

			// Token: 0x060009B0 RID: 2480 RVA: 0x00024488 File Offset: 0x00023488
			private void Form_HelpRequested(object sender, HelpEventArgs e)
			{
				this.editor.ShowHelp();
			}

			// Token: 0x060009B1 RID: 2481 RVA: 0x00024495 File Offset: 0x00023495
			private string GetDisplayText(CollectionEditor.CollectionEditorCollectionForm.ListItem item)
			{
				if (item != null)
				{
					return item.ToString();
				}
				return string.Empty;
			}

			// Token: 0x060009B2 RID: 2482 RVA: 0x000244A8 File Offset: 0x000234A8
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

			// Token: 0x060009B3 RID: 2483 RVA: 0x000245D8 File Offset: 0x000235D8
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

			// Token: 0x060009B4 RID: 2484 RVA: 0x00024AE0 File Offset: 0x00023AE0
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

			// Token: 0x060009B5 RID: 2485 RVA: 0x00024BBC File Offset: 0x00023BBC
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

			// Token: 0x060009B6 RID: 2486 RVA: 0x00024F94 File Offset: 0x00023F94
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

			// Token: 0x060009B7 RID: 2487 RVA: 0x00024FC7 File Offset: 0x00023FC7
			private void Listbox_selectedIndexChanged(object sender, EventArgs e)
			{
				this.UpdateEnabled();
			}

			// Token: 0x060009B8 RID: 2488 RVA: 0x00024FCF File Offset: 0x00023FCF
			private void Listbox_handleCreated(object sender, EventArgs e)
			{
				this.UpdateItemWidths(null);
			}

			// Token: 0x060009B9 RID: 2489 RVA: 0x00024FD8 File Offset: 0x00023FD8
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

			// Token: 0x060009BA RID: 2490 RVA: 0x000250FC File Offset: 0x000240FC
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

			// Token: 0x060009BB RID: 2491 RVA: 0x00025164 File Offset: 0x00024164
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

			// Token: 0x060009BC RID: 2492 RVA: 0x00025258 File Offset: 0x00024258
			protected override void OnFontChanged(EventArgs e)
			{
				base.OnFontChanged(e);
				this.AdjustListBoxItemHeight();
			}

			// Token: 0x060009BD RID: 2493 RVA: 0x00025267 File Offset: 0x00024267
			private void PerformAdd()
			{
				this.CreateAndAddInstance(base.NewItemTypes[0]);
			}

			// Token: 0x060009BE RID: 2494 RVA: 0x00025278 File Offset: 0x00024278
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

			// Token: 0x060009BF RID: 2495 RVA: 0x0002538C File Offset: 0x0002438C
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

			// Token: 0x060009C0 RID: 2496 RVA: 0x00025420 File Offset: 0x00024420
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

			// Token: 0x060009C1 RID: 2497 RVA: 0x00025524 File Offset: 0x00024524
			private void RemoveButton_click(object sender, EventArgs e)
			{
				this.PerformRemove();
				Control control = (Control)sender;
				if (control.Enabled)
				{
					control.Focus();
				}
			}

			// Token: 0x060009C2 RID: 2498 RVA: 0x0002554D File Offset: 0x0002454D
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

			// Token: 0x060009C3 RID: 2499 RVA: 0x0002557A File Offset: 0x0002457A
			private void SuspendEnabledUpdates()
			{
				this.suspendEnabledCount++;
			}

			// Token: 0x060009C4 RID: 2500 RVA: 0x0002558C File Offset: 0x0002458C
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

			// Token: 0x060009C5 RID: 2501 RVA: 0x00025610 File Offset: 0x00024610
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

			// Token: 0x060009C6 RID: 2502 RVA: 0x000256EC File Offset: 0x000246EC
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

			// Token: 0x04000D25 RID: 3365
			private const int TEXT_INDENT = 1;

			// Token: 0x04000D26 RID: 3366
			private const int PAINT_WIDTH = 20;

			// Token: 0x04000D27 RID: 3367
			private const int PAINT_INDENT = 26;

			// Token: 0x04000D28 RID: 3368
			private static readonly double LOG10 = Math.Log(10.0);

			// Token: 0x04000D29 RID: 3369
			private ArrayList createdItems;

			// Token: 0x04000D2A RID: 3370
			private ArrayList removedItems;

			// Token: 0x04000D2B RID: 3371
			private ArrayList originalItems;

			// Token: 0x04000D2C RID: 3372
			private CollectionEditor editor;

			// Token: 0x04000D2D RID: 3373
			private CollectionEditor.FilterListBox listbox;

			// Token: 0x04000D2E RID: 3374
			private CollectionEditor.SplitButton addButton;

			// Token: 0x04000D2F RID: 3375
			private Button removeButton;

			// Token: 0x04000D30 RID: 3376
			private Button cancelButton;

			// Token: 0x04000D31 RID: 3377
			private Button okButton;

			// Token: 0x04000D32 RID: 3378
			private Button downButton;

			// Token: 0x04000D33 RID: 3379
			private Button upButton;

			// Token: 0x04000D34 RID: 3380
			private VsPropertyGrid propertyBrowser;

			// Token: 0x04000D35 RID: 3381
			private Label membersLabel;

			// Token: 0x04000D36 RID: 3382
			private Label propertiesLabel;

			// Token: 0x04000D37 RID: 3383
			private ContextMenuStrip addDownMenu;

			// Token: 0x04000D38 RID: 3384
			private TableLayoutPanel okCancelTableLayoutPanel;

			// Token: 0x04000D39 RID: 3385
			private TableLayoutPanel overArchingTableLayoutPanel;

			// Token: 0x04000D3A RID: 3386
			private TableLayoutPanel addRemoveTableLayoutPanel;

			// Token: 0x04000D3B RID: 3387
			private int suspendEnabledCount;

			// Token: 0x04000D3C RID: 3388
			private bool dirty;

			// Token: 0x020000EA RID: 234
			private class SelectionWrapper : PropertyDescriptor, ICustomTypeDescriptor
			{
				// Token: 0x060009C8 RID: 2504 RVA: 0x00025950 File Offset: 0x00024950
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

				// Token: 0x17000150 RID: 336
				// (get) Token: 0x060009C9 RID: 2505 RVA: 0x00025A58 File Offset: 0x00024A58
				public override Type ComponentType
				{
					get
					{
						return this.collectionType;
					}
				}

				// Token: 0x17000151 RID: 337
				// (get) Token: 0x060009CA RID: 2506 RVA: 0x00025A60 File Offset: 0x00024A60
				public override bool IsReadOnly
				{
					get
					{
						return false;
					}
				}

				// Token: 0x17000152 RID: 338
				// (get) Token: 0x060009CB RID: 2507 RVA: 0x00025A63 File Offset: 0x00024A63
				public override Type PropertyType
				{
					get
					{
						return this.collectionItemType;
					}
				}

				// Token: 0x060009CC RID: 2508 RVA: 0x00025A6B File Offset: 0x00024A6B
				public override bool CanResetValue(object component)
				{
					return false;
				}

				// Token: 0x060009CD RID: 2509 RVA: 0x00025A6E File Offset: 0x00024A6E
				public override object GetValue(object component)
				{
					return this.value;
				}

				// Token: 0x060009CE RID: 2510 RVA: 0x00025A76 File Offset: 0x00024A76
				public override void ResetValue(object component)
				{
				}

				// Token: 0x060009CF RID: 2511 RVA: 0x00025A78 File Offset: 0x00024A78
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

				// Token: 0x060009D0 RID: 2512 RVA: 0x00025AF0 File Offset: 0x00024AF0
				public override bool ShouldSerializeValue(object component)
				{
					return false;
				}

				// Token: 0x060009D1 RID: 2513 RVA: 0x00025AF3 File Offset: 0x00024AF3
				AttributeCollection ICustomTypeDescriptor.GetAttributes()
				{
					return TypeDescriptor.GetAttributes(this.collectionItemType);
				}

				// Token: 0x060009D2 RID: 2514 RVA: 0x00025B00 File Offset: 0x00024B00
				string ICustomTypeDescriptor.GetClassName()
				{
					return this.collectionItemType.Name;
				}

				// Token: 0x060009D3 RID: 2515 RVA: 0x00025B0D File Offset: 0x00024B0D
				string ICustomTypeDescriptor.GetComponentName()
				{
					return null;
				}

				// Token: 0x060009D4 RID: 2516 RVA: 0x00025B10 File Offset: 0x00024B10
				TypeConverter ICustomTypeDescriptor.GetConverter()
				{
					return null;
				}

				// Token: 0x060009D5 RID: 2517 RVA: 0x00025B13 File Offset: 0x00024B13
				EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
				{
					return null;
				}

				// Token: 0x060009D6 RID: 2518 RVA: 0x00025B16 File Offset: 0x00024B16
				PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
				{
					return this;
				}

				// Token: 0x060009D7 RID: 2519 RVA: 0x00025B19 File Offset: 0x00024B19
				object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
				{
					return null;
				}

				// Token: 0x060009D8 RID: 2520 RVA: 0x00025B1C File Offset: 0x00024B1C
				EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
				{
					return EventDescriptorCollection.Empty;
				}

				// Token: 0x060009D9 RID: 2521 RVA: 0x00025B23 File Offset: 0x00024B23
				EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
				{
					return EventDescriptorCollection.Empty;
				}

				// Token: 0x060009DA RID: 2522 RVA: 0x00025B2A File Offset: 0x00024B2A
				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
				{
					return this.properties;
				}

				// Token: 0x060009DB RID: 2523 RVA: 0x00025B32 File Offset: 0x00024B32
				PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
				{
					return this.properties;
				}

				// Token: 0x060009DC RID: 2524 RVA: 0x00025B3A File Offset: 0x00024B3A
				object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
				{
					return this;
				}

				// Token: 0x04000D3D RID: 3389
				private Type collectionType;

				// Token: 0x04000D3E RID: 3390
				private Type collectionItemType;

				// Token: 0x04000D3F RID: 3391
				private Control control;

				// Token: 0x04000D40 RID: 3392
				private ICollection collection;

				// Token: 0x04000D41 RID: 3393
				private PropertyDescriptorCollection properties;

				// Token: 0x04000D42 RID: 3394
				private object value;
			}

			// Token: 0x020000EB RID: 235
			private class ListItem
			{
				// Token: 0x060009DD RID: 2525 RVA: 0x00025B3D File Offset: 0x00024B3D
				public ListItem(CollectionEditor parentCollectionEditor, object value)
				{
					this.value = value;
					this.parentCollectionEditor = parentCollectionEditor;
				}

				// Token: 0x060009DE RID: 2526 RVA: 0x00025B53 File Offset: 0x00024B53
				public override string ToString()
				{
					return this.parentCollectionEditor.GetDisplayText(this.value);
				}

				// Token: 0x17000153 RID: 339
				// (get) Token: 0x060009DF RID: 2527 RVA: 0x00025B68 File Offset: 0x00024B68
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

				// Token: 0x17000154 RID: 340
				// (get) Token: 0x060009E0 RID: 2528 RVA: 0x00025BBD File Offset: 0x00024BBD
				// (set) Token: 0x060009E1 RID: 2529 RVA: 0x00025BC5 File Offset: 0x00024BC5
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

				// Token: 0x04000D43 RID: 3395
				private object value;

				// Token: 0x04000D44 RID: 3396
				private object uiTypeEditor;

				// Token: 0x04000D45 RID: 3397
				private CollectionEditor parentCollectionEditor;
			}

			// Token: 0x020000EC RID: 236
			private class TypeMenuItem : ToolStripMenuItem
			{
				// Token: 0x060009E2 RID: 2530 RVA: 0x00025BD5 File Offset: 0x00024BD5
				public TypeMenuItem(Type itemType, EventHandler handler)
					: base(itemType.Name, null, handler)
				{
					this.itemType = itemType;
				}

				// Token: 0x17000155 RID: 341
				// (get) Token: 0x060009E3 RID: 2531 RVA: 0x00025BEC File Offset: 0x00024BEC
				public Type ItemType
				{
					get
					{
						return this.itemType;
					}
				}

				// Token: 0x04000D46 RID: 3398
				private Type itemType;
			}
		}

		// Token: 0x020000ED RID: 237
		internal class FilterListBox : ListBox
		{
			// Token: 0x17000156 RID: 342
			// (get) Token: 0x060009E4 RID: 2532 RVA: 0x00025BF4 File Offset: 0x00024BF4
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

			// Token: 0x060009E5 RID: 2533 RVA: 0x00025C70 File Offset: 0x00024C70
			public new void RefreshItem(int index)
			{
				base.RefreshItem(index);
			}

			// Token: 0x060009E6 RID: 2534 RVA: 0x00025C7C File Offset: 0x00024C7C
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

			// Token: 0x04000D47 RID: 3399
			private PropertyGrid grid;

			// Token: 0x04000D48 RID: 3400
			private Message lastKeyDown;
		}

		// Token: 0x020000EE RID: 238
		internal class PropertyGridSite : ISite, IServiceProvider
		{
			// Token: 0x060009E8 RID: 2536 RVA: 0x00025E00 File Offset: 0x00024E00
			public PropertyGridSite(IServiceProvider sp, IComponent comp)
			{
				this.sp = sp;
				this.comp = comp;
			}

			// Token: 0x17000157 RID: 343
			// (get) Token: 0x060009E9 RID: 2537 RVA: 0x00025E16 File Offset: 0x00024E16
			public IComponent Component
			{
				get
				{
					return this.comp;
				}
			}

			// Token: 0x17000158 RID: 344
			// (get) Token: 0x060009EA RID: 2538 RVA: 0x00025E1E File Offset: 0x00024E1E
			public IContainer Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17000159 RID: 345
			// (get) Token: 0x060009EB RID: 2539 RVA: 0x00025E21 File Offset: 0x00024E21
			public bool DesignMode
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700015A RID: 346
			// (get) Token: 0x060009EC RID: 2540 RVA: 0x00025E24 File Offset: 0x00024E24
			// (set) Token: 0x060009ED RID: 2541 RVA: 0x00025E27 File Offset: 0x00024E27
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

			// Token: 0x060009EE RID: 2542 RVA: 0x00025E2C File Offset: 0x00024E2C
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

			// Token: 0x04000D49 RID: 3401
			private IServiceProvider sp;

			// Token: 0x04000D4A RID: 3402
			private IComponent comp;

			// Token: 0x04000D4B RID: 3403
			private bool inGetService;
		}
	}
}

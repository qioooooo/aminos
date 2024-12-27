using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms
{
	// Token: 0x020003DC RID: 988
	[ProvideProperty("Error", typeof(Control))]
	[ToolboxItemFilter("System.Windows.Forms")]
	[ComplexBindingProperties("DataSource", "DataMember")]
	[ProvideProperty("IconAlignment", typeof(Control))]
	[ProvideProperty("IconPadding", typeof(Control))]
	[SRDescription("DescriptionErrorProvider")]
	public class ErrorProvider : Component, IExtenderProvider, ISupportInitialize
	{
		// Token: 0x06003B10 RID: 15120 RVA: 0x000D5EEC File Offset: 0x000D4EEC
		public ErrorProvider()
		{
			this.icon = ErrorProvider.DefaultIcon;
			this.blinkRate = 250;
			this.blinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
			this.currentChanged = new EventHandler(this.ErrorManager_CurrentChanged);
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x000D5F56 File Offset: 0x000D4F56
		public ErrorProvider(ContainerControl parentControl)
			: this()
		{
			this.parentControl = parentControl;
			this.propChangedEvent = new EventHandler(this.ParentControl_BindingContextChanged);
			parentControl.BindingContextChanged += this.propChangedEvent;
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x000D5F83 File Offset: 0x000D4F83
		public ErrorProvider(IContainer container)
			: this()
		{
			container.Add(this);
		}

		// Token: 0x17000B3D RID: 2877
		// (set) Token: 0x06003B13 RID: 15123 RVA: 0x000D5F94 File Offset: 0x000D4F94
		public override ISite Site
		{
			set
			{
				base.Site = value;
				if (value == null)
				{
					return;
				}
				IDesignerHost designerHost = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null)
				{
					IComponent rootComponent = designerHost.RootComponent;
					if (rootComponent is ContainerControl)
					{
						this.ContainerControl = (ContainerControl)rootComponent;
					}
				}
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06003B14 RID: 15124 RVA: 0x000D5FE0 File Offset: 0x000D4FE0
		// (set) Token: 0x06003B15 RID: 15125 RVA: 0x000D5FF4 File Offset: 0x000D4FF4
		[DefaultValue(ErrorBlinkStyle.BlinkIfDifferentError)]
		[SRCategory("CatBehavior")]
		[SRDescription("ErrorProviderBlinkStyleDescr")]
		public ErrorBlinkStyle BlinkStyle
		{
			get
			{
				if (this.blinkRate == 0)
				{
					return ErrorBlinkStyle.NeverBlink;
				}
				return this.blinkStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ErrorBlinkStyle));
				}
				if (this.blinkRate == 0)
				{
					value = ErrorBlinkStyle.NeverBlink;
				}
				if (this.blinkStyle == value)
				{
					return;
				}
				if (value == ErrorBlinkStyle.AlwaysBlink)
				{
					this.showIcon = true;
					this.blinkStyle = ErrorBlinkStyle.AlwaysBlink;
					using (IEnumerator enumerator = this.windows.Values.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							ErrorProvider.ErrorWindow errorWindow = (ErrorProvider.ErrorWindow)obj;
							errorWindow.StartBlinking();
						}
						return;
					}
				}
				if (this.blinkStyle == ErrorBlinkStyle.AlwaysBlink)
				{
					this.blinkStyle = value;
					using (IEnumerator enumerator2 = this.windows.Values.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							ErrorProvider.ErrorWindow errorWindow2 = (ErrorProvider.ErrorWindow)obj2;
							errorWindow2.StopBlinking();
						}
						return;
					}
				}
				this.blinkStyle = value;
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06003B16 RID: 15126 RVA: 0x000D6108 File Offset: 0x000D5108
		// (set) Token: 0x06003B17 RID: 15127 RVA: 0x000D6110 File Offset: 0x000D5110
		[DefaultValue(null)]
		[SRDescription("ErrorProviderContainerControlDescr")]
		[SRCategory("CatData")]
		public ContainerControl ContainerControl
		{
			[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
			[UIPermission(SecurityAction.InheritanceDemand, Window = UIPermissionWindow.AllWindows)]
			get
			{
				return this.parentControl;
			}
			set
			{
				if (this.parentControl != value)
				{
					if (this.parentControl != null)
					{
						this.parentControl.BindingContextChanged -= this.propChangedEvent;
					}
					this.parentControl = value;
					if (this.parentControl != null)
					{
						this.parentControl.BindingContextChanged += this.propChangedEvent;
					}
					this.Set_ErrorManager(this.DataSource, this.DataMember, true);
				}
			}
		}

		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06003B18 RID: 15128 RVA: 0x000D6172 File Offset: 0x000D5172
		// (set) Token: 0x06003B19 RID: 15129 RVA: 0x000D617A File Offset: 0x000D517A
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("ControlRightToLeftDescr")]
		public virtual bool RightToLeft
		{
			get
			{
				return this.rightToLeft;
			}
			set
			{
				if (value != this.rightToLeft)
				{
					this.rightToLeft = value;
					this.OnRightToLeftChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140001EF RID: 495
		// (add) Token: 0x06003B1A RID: 15130 RVA: 0x000D6197 File Offset: 0x000D5197
		// (remove) Token: 0x06003B1B RID: 15131 RVA: 0x000D61B0 File Offset: 0x000D51B0
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnRightToLeftChangedDescr")]
		public event EventHandler RightToLeftChanged
		{
			add
			{
				this.onRightToLeftChanged = (EventHandler)Delegate.Combine(this.onRightToLeftChanged, value);
			}
			remove
			{
				this.onRightToLeftChanged = (EventHandler)Delegate.Remove(this.onRightToLeftChanged, value);
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x06003B1C RID: 15132 RVA: 0x000D61C9 File Offset: 0x000D51C9
		// (set) Token: 0x06003B1D RID: 15133 RVA: 0x000D61D1 File Offset: 0x000D51D1
		[DefaultValue(null)]
		[Bindable(true)]
		[TypeConverter(typeof(StringConverter))]
		[SRCategory("CatData")]
		[Localizable(false)]
		[SRDescription("ControlTagDescr")]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x000D61DC File Offset: 0x000D51DC
		private void Set_ErrorManager(object newDataSource, string newDataMember, bool force)
		{
			if (this.inSetErrorManager)
			{
				return;
			}
			this.inSetErrorManager = true;
			try
			{
				bool flag = this.DataSource != newDataSource;
				bool flag2 = this.DataMember != newDataMember;
				if (flag || flag2 || force)
				{
					this.dataSource = newDataSource;
					this.dataMember = newDataMember;
					if (this.initializing)
					{
						this.setErrorManagerOnEndInit = true;
					}
					else
					{
						this.UnwireEvents(this.errorManager);
						if (this.parentControl != null && this.dataSource != null && this.parentControl.BindingContext != null)
						{
							this.errorManager = this.parentControl.BindingContext[this.dataSource, this.dataMember];
						}
						else
						{
							this.errorManager = null;
						}
						this.WireEvents(this.errorManager);
						if (this.errorManager != null)
						{
							this.UpdateBinding();
						}
					}
				}
			}
			finally
			{
				this.inSetErrorManager = false;
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06003B1F RID: 15135 RVA: 0x000D62C8 File Offset: 0x000D52C8
		// (set) Token: 0x06003B20 RID: 15136 RVA: 0x000D62D0 File Offset: 0x000D52D0
		[SRDescription("ErrorProviderDataSourceDescr")]
		[DefaultValue(null)]
		[SRCategory("CatData")]
		[AttributeProvider(typeof(IListSource))]
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				if (this.parentControl != null && value != null && !string.IsNullOrEmpty(this.dataMember))
				{
					try
					{
						this.errorManager = this.parentControl.BindingContext[value, this.dataMember];
					}
					catch (ArgumentException)
					{
						this.dataMember = "";
					}
				}
				this.Set_ErrorManager(value, this.DataMember, false);
			}
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x000D6340 File Offset: 0x000D5340
		private bool ShouldSerializeDataSource()
		{
			return this.dataSource != null;
		}

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06003B22 RID: 15138 RVA: 0x000D634E File Offset: 0x000D534E
		// (set) Token: 0x06003B23 RID: 15139 RVA: 0x000D6356 File Offset: 0x000D5356
		[DefaultValue(null)]
		[Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ErrorProviderDataMemberDescr")]
		[SRCategory("CatData")]
		public string DataMember
		{
			get
			{
				return this.dataMember;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				this.Set_ErrorManager(this.DataSource, value, false);
			}
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x000D6370 File Offset: 0x000D5370
		private bool ShouldSerializeDataMember()
		{
			return this.dataMember != null && this.dataMember.Length != 0;
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x000D638D File Offset: 0x000D538D
		public void BindToDataAndErrors(object newDataSource, string newDataMember)
		{
			this.Set_ErrorManager(newDataSource, newDataMember, false);
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x000D6398 File Offset: 0x000D5398
		private void WireEvents(BindingManagerBase listManager)
		{
			if (listManager != null)
			{
				listManager.CurrentChanged += this.currentChanged;
				listManager.BindingComplete += this.ErrorManager_BindingComplete;
				CurrencyManager currencyManager = listManager as CurrencyManager;
				if (currencyManager != null)
				{
					currencyManager.ItemChanged += this.ErrorManager_ItemChanged;
					currencyManager.Bindings.CollectionChanged += this.ErrorManager_BindingsChanged;
				}
			}
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x000D63FC File Offset: 0x000D53FC
		private void UnwireEvents(BindingManagerBase listManager)
		{
			if (listManager != null)
			{
				listManager.CurrentChanged -= this.currentChanged;
				listManager.BindingComplete -= this.ErrorManager_BindingComplete;
				CurrencyManager currencyManager = listManager as CurrencyManager;
				if (currencyManager != null)
				{
					currencyManager.ItemChanged -= this.ErrorManager_ItemChanged;
					currencyManager.Bindings.CollectionChanged -= this.ErrorManager_BindingsChanged;
				}
			}
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x000D6460 File Offset: 0x000D5460
		private void ErrorManager_BindingComplete(object sender, BindingCompleteEventArgs e)
		{
			Binding binding = e.Binding;
			if (binding != null && binding.Control != null)
			{
				this.SetError(binding.Control, (e.ErrorText == null) ? string.Empty : e.ErrorText);
			}
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x000D64A0 File Offset: 0x000D54A0
		private void ErrorManager_BindingsChanged(object sender, CollectionChangeEventArgs e)
		{
			this.ErrorManager_CurrentChanged(this.errorManager, e);
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x000D64AF File Offset: 0x000D54AF
		private void ParentControl_BindingContextChanged(object sender, EventArgs e)
		{
			this.Set_ErrorManager(this.DataSource, this.DataMember, true);
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x000D64C4 File Offset: 0x000D54C4
		public void UpdateBinding()
		{
			this.ErrorManager_CurrentChanged(this.errorManager, EventArgs.Empty);
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x000D64D8 File Offset: 0x000D54D8
		private void ErrorManager_ItemChanged(object sender, ItemChangedEventArgs e)
		{
			BindingsCollection bindings = this.errorManager.Bindings;
			int count = bindings.Count;
			if (e.Index == -1 && this.errorManager.Count == 0)
			{
				for (int i = 0; i < count; i++)
				{
					if (bindings[i].Control != null)
					{
						this.SetError(bindings[i].Control, "");
					}
				}
				return;
			}
			this.ErrorManager_CurrentChanged(sender, e);
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x000D6548 File Offset: 0x000D5548
		private void ErrorManager_CurrentChanged(object sender, EventArgs e)
		{
			if (this.errorManager.Count == 0)
			{
				return;
			}
			object obj = this.errorManager.Current;
			if (!(obj is IDataErrorInfo))
			{
				return;
			}
			BindingsCollection bindings = this.errorManager.Bindings;
			int count = bindings.Count;
			foreach (object obj2 in this.items.Values)
			{
				ErrorProvider.ControlItem controlItem = (ErrorProvider.ControlItem)obj2;
				controlItem.BlinkPhase = 0;
			}
			Hashtable hashtable = new Hashtable(count);
			for (int i = 0; i < count; i++)
			{
				if (bindings[i].Control != null)
				{
					BindToObject bindToObject = bindings[i].BindToObject;
					string text = ((IDataErrorInfo)obj)[bindToObject.BindingMemberInfo.BindingField];
					if (text == null)
					{
						text = "";
					}
					string text2 = "";
					if (hashtable.Contains(bindings[i].Control))
					{
						text2 = (string)hashtable[bindings[i].Control];
					}
					if (string.IsNullOrEmpty(text2))
					{
						text2 = text;
					}
					else
					{
						text2 = text2 + "\r\n" + text;
					}
					hashtable[bindings[i].Control] = text2;
				}
			}
			foreach (object obj3 in hashtable)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
				this.SetError((Control)dictionaryEntry.Key, (string)dictionaryEntry.Value);
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06003B2E RID: 15150 RVA: 0x000D66F8 File Offset: 0x000D56F8
		// (set) Token: 0x06003B2F RID: 15151 RVA: 0x000D6700 File Offset: 0x000D5700
		[SRDescription("ErrorProviderBlinkRateDescr")]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(250)]
		public int BlinkRate
		{
			get
			{
				return this.blinkRate;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("BlinkRate", value, SR.GetString("BlinkRateMustBeZeroOrMore"));
				}
				this.blinkRate = value;
				if (this.blinkRate == 0)
				{
					this.BlinkStyle = ErrorBlinkStyle.NeverBlink;
				}
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06003B30 RID: 15152 RVA: 0x000D6738 File Offset: 0x000D5738
		private static Icon DefaultIcon
		{
			get
			{
				if (ErrorProvider.defaultIcon == null)
				{
					lock (typeof(ErrorProvider))
					{
						if (ErrorProvider.defaultIcon == null)
						{
							ErrorProvider.defaultIcon = new Icon(typeof(ErrorProvider), "Error.ico");
						}
					}
				}
				return ErrorProvider.defaultIcon;
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06003B31 RID: 15153 RVA: 0x000D679C File Offset: 0x000D579C
		// (set) Token: 0x06003B32 RID: 15154 RVA: 0x000D67A4 File Offset: 0x000D57A4
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ErrorProviderIconDescr")]
		public Icon Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.icon = value;
				this.DisposeRegion();
				ErrorProvider.ErrorWindow[] array = new ErrorProvider.ErrorWindow[this.windows.Values.Count];
				this.windows.Values.CopyTo(array, 0);
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Update(false);
				}
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06003B33 RID: 15155 RVA: 0x000D680B File Offset: 0x000D580B
		internal ErrorProvider.IconRegion Region
		{
			get
			{
				if (this.region == null)
				{
					this.region = new ErrorProvider.IconRegion(this.Icon);
				}
				return this.region;
			}
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x000D682C File Offset: 0x000D582C
		void ISupportInitialize.BeginInit()
		{
			this.initializing = true;
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x000D6835 File Offset: 0x000D5835
		private void EndInitCore()
		{
			this.initializing = false;
			if (this.setErrorManagerOnEndInit)
			{
				this.setErrorManagerOnEndInit = false;
				this.Set_ErrorManager(this.DataSource, this.DataMember, true);
			}
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x000D6860 File Offset: 0x000D5860
		void ISupportInitialize.EndInit()
		{
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null && !supportInitializeNotification.IsInitialized)
			{
				supportInitializeNotification.Initialized += this.DataSource_Initialized;
				return;
			}
			this.EndInitCore();
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x000D68A0 File Offset: 0x000D58A0
		private void DataSource_Initialized(object sender, EventArgs e)
		{
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null)
			{
				supportInitializeNotification.Initialized -= this.DataSource_Initialized;
			}
			this.EndInitCore();
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x000D68D4 File Offset: 0x000D58D4
		public void Clear()
		{
			ErrorProvider.ErrorWindow[] array = new ErrorProvider.ErrorWindow[this.windows.Values.Count];
			this.windows.Values.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Dispose();
			}
			this.windows.Clear();
			foreach (object obj in this.items.Values)
			{
				ErrorProvider.ControlItem controlItem = (ErrorProvider.ControlItem)obj;
				if (controlItem != null)
				{
					controlItem.Dispose();
				}
			}
			this.items.Clear();
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x000D698C File Offset: 0x000D598C
		public bool CanExtend(object extendee)
		{
			return extendee is Control && !(extendee is Form) && !(extendee is ToolBar);
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x000D69AC File Offset: 0x000D59AC
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Clear();
				this.DisposeRegion();
				this.UnwireEvents(this.errorManager);
			}
			base.Dispose(disposing);
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x000D69D0 File Offset: 0x000D59D0
		private void DisposeRegion()
		{
			if (this.region != null)
			{
				this.region.Dispose();
				this.region = null;
			}
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x000D69EC File Offset: 0x000D59EC
		private ErrorProvider.ControlItem EnsureControlItem(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			ErrorProvider.ControlItem controlItem = (ErrorProvider.ControlItem)this.items[control];
			if (controlItem == null)
			{
				controlItem = new ErrorProvider.ControlItem(this, control, (IntPtr)(++this.itemIdCounter));
				this.items[control] = controlItem;
			}
			return controlItem;
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x000D6A48 File Offset: 0x000D5A48
		internal ErrorProvider.ErrorWindow EnsureErrorWindow(Control parent)
		{
			ErrorProvider.ErrorWindow errorWindow = (ErrorProvider.ErrorWindow)this.windows[parent];
			if (errorWindow == null)
			{
				errorWindow = new ErrorProvider.ErrorWindow(this, parent);
				this.windows[parent] = errorWindow;
			}
			return errorWindow;
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x000D6A80 File Offset: 0x000D5A80
		[DefaultValue("")]
		[SRDescription("ErrorProviderErrorDescr")]
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		public string GetError(Control control)
		{
			return this.EnsureControlItem(control).Error;
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x000D6A8E File Offset: 0x000D5A8E
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(ErrorIconAlignment.MiddleRight)]
		[SRDescription("ErrorProviderIconAlignmentDescr")]
		public ErrorIconAlignment GetIconAlignment(Control control)
		{
			return this.EnsureControlItem(control).IconAlignment;
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x000D6A9C File Offset: 0x000D5A9C
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ErrorProviderIconPaddingDescr")]
		[DefaultValue(0)]
		public int GetIconPadding(Control control)
		{
			return this.EnsureControlItem(control).IconPadding;
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x000D6AAA File Offset: 0x000D5AAA
		private void ResetIcon()
		{
			this.Icon = ErrorProvider.DefaultIcon;
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x000D6AB8 File Offset: 0x000D5AB8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftChanged(EventArgs e)
		{
			foreach (object obj in this.windows.Values)
			{
				ErrorProvider.ErrorWindow errorWindow = (ErrorProvider.ErrorWindow)obj;
				errorWindow.Update(false);
			}
			if (this.onRightToLeftChanged != null)
			{
				this.onRightToLeftChanged(this, e);
			}
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x000D6B2C File Offset: 0x000D5B2C
		public void SetError(Control control, string value)
		{
			this.EnsureControlItem(control).Error = value;
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x000D6B3B File Offset: 0x000D5B3B
		public void SetIconAlignment(Control control, ErrorIconAlignment value)
		{
			this.EnsureControlItem(control).IconAlignment = value;
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x000D6B4A File Offset: 0x000D5B4A
		public void SetIconPadding(Control control, int padding)
		{
			this.EnsureControlItem(control).IconPadding = padding;
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x000D6B59 File Offset: 0x000D5B59
		private bool ShouldSerializeIcon()
		{
			return this.Icon != ErrorProvider.DefaultIcon;
		}

		// Token: 0x04001D8C RID: 7564
		private const int defaultBlinkRate = 250;

		// Token: 0x04001D8D RID: 7565
		private const ErrorBlinkStyle defaultBlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;

		// Token: 0x04001D8E RID: 7566
		private const ErrorIconAlignment defaultIconAlignment = ErrorIconAlignment.MiddleRight;

		// Token: 0x04001D8F RID: 7567
		private Hashtable items = new Hashtable();

		// Token: 0x04001D90 RID: 7568
		private Hashtable windows = new Hashtable();

		// Token: 0x04001D91 RID: 7569
		private Icon icon = ErrorProvider.DefaultIcon;

		// Token: 0x04001D92 RID: 7570
		private ErrorProvider.IconRegion region;

		// Token: 0x04001D93 RID: 7571
		private int itemIdCounter;

		// Token: 0x04001D94 RID: 7572
		private int blinkRate;

		// Token: 0x04001D95 RID: 7573
		private ErrorBlinkStyle blinkStyle;

		// Token: 0x04001D96 RID: 7574
		private bool showIcon = true;

		// Token: 0x04001D97 RID: 7575
		private bool inSetErrorManager;

		// Token: 0x04001D98 RID: 7576
		private bool setErrorManagerOnEndInit;

		// Token: 0x04001D99 RID: 7577
		private bool initializing;

		// Token: 0x04001D9A RID: 7578
		[ThreadStatic]
		private static Icon defaultIcon;

		// Token: 0x04001D9B RID: 7579
		private ContainerControl parentControl;

		// Token: 0x04001D9C RID: 7580
		private object dataSource;

		// Token: 0x04001D9D RID: 7581
		private string dataMember;

		// Token: 0x04001D9E RID: 7582
		private BindingManagerBase errorManager;

		// Token: 0x04001D9F RID: 7583
		private EventHandler currentChanged;

		// Token: 0x04001DA0 RID: 7584
		private EventHandler propChangedEvent;

		// Token: 0x04001DA1 RID: 7585
		private EventHandler onRightToLeftChanged;

		// Token: 0x04001DA2 RID: 7586
		private bool rightToLeft;

		// Token: 0x04001DA3 RID: 7587
		private object userData;

		// Token: 0x020003DD RID: 989
		internal class ErrorWindow : NativeWindow
		{
			// Token: 0x06003B47 RID: 15175 RVA: 0x000D6B6C File Offset: 0x000D5B6C
			public ErrorWindow(ErrorProvider provider, Control parent)
			{
				this.provider = provider;
				this.parent = parent;
			}

			// Token: 0x06003B48 RID: 15176 RVA: 0x000D6BC0 File Offset: 0x000D5BC0
			public void Add(ErrorProvider.ControlItem item)
			{
				this.items.Add(item);
				if (!this.EnsureCreated())
				{
					return;
				}
				NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
				toolinfo_T.cbSize = Marshal.SizeOf(toolinfo_T);
				toolinfo_T.hwnd = base.Handle;
				toolinfo_T.uId = item.Id;
				toolinfo_T.lpszText = item.Error;
				toolinfo_T.uFlags = 16;
				UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.TTM_ADDTOOL, 0, toolinfo_T);
				this.Update(false);
			}

			// Token: 0x06003B49 RID: 15177 RVA: 0x000D6C4B File Offset: 0x000D5C4B
			public void Dispose()
			{
				this.EnsureDestroyed();
			}

			// Token: 0x06003B4A RID: 15178 RVA: 0x000D6C54 File Offset: 0x000D5C54
			private bool EnsureCreated()
			{
				if (base.Handle == IntPtr.Zero)
				{
					if (!this.parent.IsHandleCreated)
					{
						return false;
					}
					this.CreateHandle(new CreateParams
					{
						Caption = string.Empty,
						Style = 1342177280,
						ClassStyle = 8,
						X = 0,
						Y = 0,
						Width = 0,
						Height = 0,
						Parent = this.parent.Handle
					});
					NativeMethods.INITCOMMONCONTROLSEX initcommoncontrolsex = new NativeMethods.INITCOMMONCONTROLSEX();
					initcommoncontrolsex.dwICC = 8;
					initcommoncontrolsex.dwSize = Marshal.SizeOf(initcommoncontrolsex);
					SafeNativeMethods.InitCommonControlsEx(initcommoncontrolsex);
					CreateParams createParams = new CreateParams();
					createParams.Parent = base.Handle;
					createParams.ClassName = "tooltips_class32";
					createParams.Style = 1;
					this.tipWindow = new NativeWindow();
					this.tipWindow.CreateHandle(createParams);
					UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
					SafeNativeMethods.SetWindowPos(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.HWND_TOP, 0, 0, 0, 0, 19);
					UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), 1027, 3, 0);
				}
				return true;
			}

			// Token: 0x06003B4B RID: 15179 RVA: 0x000D6DB4 File Offset: 0x000D5DB4
			private void EnsureDestroyed()
			{
				if (this.timer != null)
				{
					this.timer.Dispose();
					this.timer = null;
				}
				if (this.tipWindow != null)
				{
					this.tipWindow.DestroyHandle();
					this.tipWindow = null;
				}
				SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.HWND_TOP, this.windowBounds.X, this.windowBounds.Y, this.windowBounds.Width, this.windowBounds.Height, 131);
				if (this.parent != null)
				{
					this.parent.Invalidate(true);
				}
				this.DestroyHandle();
				if (this.mirrordc != null)
				{
					this.mirrordc.Dispose();
				}
			}

			// Token: 0x06003B4C RID: 15180 RVA: 0x000D6E6C File Offset: 0x000D5E6C
			private void CreateMirrorDC(IntPtr hdc, int originOffset)
			{
				this.mirrordc = DeviceContext.FromHdc(hdc);
				if (this.parent.IsMirrored && this.mirrordc != null)
				{
					this.mirrordc.SaveHdc();
					this.mirrordcExtent = this.mirrordc.ViewportExtent;
					this.mirrordcOrigin = this.mirrordc.ViewportOrigin;
					this.mirrordcMode = this.mirrordc.SetMapMode(DeviceContextMapMode.Anisotropic);
					this.mirrordc.ViewportExtent = new Size(-this.mirrordcExtent.Width, this.mirrordcExtent.Height);
					this.mirrordc.ViewportOrigin = new Point(this.mirrordcOrigin.X + originOffset, this.mirrordcOrigin.Y);
				}
			}

			// Token: 0x06003B4D RID: 15181 RVA: 0x000D6F30 File Offset: 0x000D5F30
			private void RestoreMirrorDC()
			{
				if (this.parent.IsMirrored && this.mirrordc != null)
				{
					this.mirrordc.ViewportExtent = this.mirrordcExtent;
					this.mirrordc.ViewportOrigin = this.mirrordcOrigin;
					this.mirrordc.SetMapMode(this.mirrordcMode);
					this.mirrordc.RestoreHdc();
					this.mirrordc.Dispose();
				}
				this.mirrordc = null;
				this.mirrordcExtent = Size.Empty;
				this.mirrordcOrigin = Point.Empty;
				this.mirrordcMode = DeviceContextMapMode.Text;
			}

			// Token: 0x06003B4E RID: 15182 RVA: 0x000D6FC0 File Offset: 0x000D5FC0
			private void OnPaint(ref Message m)
			{
				NativeMethods.PAINTSTRUCT paintstruct = default(NativeMethods.PAINTSTRUCT);
				IntPtr intPtr = UnsafeNativeMethods.BeginPaint(new HandleRef(this, base.Handle), ref paintstruct);
				try
				{
					this.CreateMirrorDC(intPtr, this.windowBounds.Width - 1);
					try
					{
						for (int i = 0; i < this.items.Count; i++)
						{
							ErrorProvider.ControlItem controlItem = (ErrorProvider.ControlItem)this.items[i];
							Rectangle iconBounds = controlItem.GetIconBounds(this.provider.Region.Size);
							SafeNativeMethods.DrawIconEx(new HandleRef(this, this.mirrordc.Hdc), iconBounds.X - this.windowBounds.X, iconBounds.Y - this.windowBounds.Y, new HandleRef(this.provider.Region, this.provider.Region.IconHandle), iconBounds.Width, iconBounds.Height, 0, NativeMethods.NullHandleRef, 3);
						}
					}
					finally
					{
						this.RestoreMirrorDC();
					}
				}
				finally
				{
					UnsafeNativeMethods.EndPaint(new HandleRef(this, base.Handle), ref paintstruct);
				}
			}

			// Token: 0x06003B4F RID: 15183 RVA: 0x000D70F4 File Offset: 0x000D60F4
			protected override void OnThreadException(Exception e)
			{
				Application.OnThreadException(e);
			}

			// Token: 0x06003B50 RID: 15184 RVA: 0x000D70FC File Offset: 0x000D60FC
			private void OnTimer(object sender, EventArgs e)
			{
				int num = 0;
				for (int i = 0; i < this.items.Count; i++)
				{
					num += ((ErrorProvider.ControlItem)this.items[i]).BlinkPhase;
				}
				if (num == 0 && this.provider.BlinkStyle != ErrorBlinkStyle.AlwaysBlink)
				{
					this.timer.Stop();
				}
				this.Update(true);
			}

			// Token: 0x06003B51 RID: 15185 RVA: 0x000D7160 File Offset: 0x000D6160
			private void OnToolTipVisibilityChanging(IntPtr id, bool toolTipShown)
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					if (((ErrorProvider.ControlItem)this.items[i]).Id == id)
					{
						((ErrorProvider.ControlItem)this.items[i]).ToolTipShown = toolTipShown;
					}
				}
			}

			// Token: 0x06003B52 RID: 15186 RVA: 0x000D71B8 File Offset: 0x000D61B8
			public void Remove(ErrorProvider.ControlItem item)
			{
				this.items.Remove(item);
				if (this.tipWindow != null)
				{
					NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
					toolinfo_T.cbSize = Marshal.SizeOf(toolinfo_T);
					toolinfo_T.hwnd = base.Handle;
					toolinfo_T.uId = item.Id;
					UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.TTM_DELTOOL, 0, toolinfo_T);
				}
				if (this.items.Count == 0)
				{
					this.EnsureDestroyed();
					return;
				}
				this.Update(false);
			}

			// Token: 0x06003B53 RID: 15187 RVA: 0x000D7244 File Offset: 0x000D6244
			internal void StartBlinking()
			{
				if (this.timer == null)
				{
					this.timer = new Timer();
					this.timer.Tick += this.OnTimer;
				}
				this.timer.Interval = this.provider.BlinkRate;
				this.timer.Start();
				this.Update(false);
			}

			// Token: 0x06003B54 RID: 15188 RVA: 0x000D72A3 File Offset: 0x000D62A3
			internal void StopBlinking()
			{
				if (this.timer != null)
				{
					this.timer.Stop();
				}
				this.Update(false);
			}

			// Token: 0x06003B55 RID: 15189 RVA: 0x000D72C0 File Offset: 0x000D62C0
			public void Update(bool timerCaused)
			{
				ErrorProvider.IconRegion region = this.provider.Region;
				Size size = region.Size;
				this.windowBounds = Rectangle.Empty;
				for (int i = 0; i < this.items.Count; i++)
				{
					ErrorProvider.ControlItem controlItem = (ErrorProvider.ControlItem)this.items[i];
					Rectangle iconBounds = controlItem.GetIconBounds(size);
					if (this.windowBounds.IsEmpty)
					{
						this.windowBounds = iconBounds;
					}
					else
					{
						this.windowBounds = Rectangle.Union(this.windowBounds, iconBounds);
					}
				}
				Region region2 = new Region(new Rectangle(0, 0, 0, 0));
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					for (int j = 0; j < this.items.Count; j++)
					{
						ErrorProvider.ControlItem controlItem2 = (ErrorProvider.ControlItem)this.items[j];
						Rectangle iconBounds2 = controlItem2.GetIconBounds(size);
						iconBounds2.X -= this.windowBounds.X;
						iconBounds2.Y -= this.windowBounds.Y;
						bool flag = true;
						if (!controlItem2.ToolTipShown)
						{
							switch (this.provider.BlinkStyle)
							{
							case ErrorBlinkStyle.BlinkIfDifferentError:
								flag = controlItem2.BlinkPhase == 0 || (controlItem2.BlinkPhase > 0 && (controlItem2.BlinkPhase & 1) == (j & 1));
								break;
							case ErrorBlinkStyle.AlwaysBlink:
								flag = (j & 1) == 0 == this.provider.showIcon;
								break;
							}
						}
						if (flag)
						{
							region.Region.Translate(iconBounds2.X, iconBounds2.Y);
							region2.Union(region.Region);
							region.Region.Translate(-iconBounds2.X, -iconBounds2.Y);
						}
						if (this.tipWindow != null)
						{
							NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
							toolinfo_T.cbSize = Marshal.SizeOf(toolinfo_T);
							toolinfo_T.hwnd = base.Handle;
							toolinfo_T.uId = controlItem2.Id;
							toolinfo_T.lpszText = controlItem2.Error;
							toolinfo_T.rect = NativeMethods.RECT.FromXYWH(iconBounds2.X, iconBounds2.Y, iconBounds2.Width, iconBounds2.Height);
							toolinfo_T.uFlags = 16;
							if (this.provider.RightToLeft)
							{
								toolinfo_T.uFlags |= 4;
							}
							UnsafeNativeMethods.SendMessage(new HandleRef(this.tipWindow, this.tipWindow.Handle), NativeMethods.TTM_SETTOOLINFO, 0, toolinfo_T);
						}
						if (timerCaused && controlItem2.BlinkPhase > 0)
						{
							controlItem2.BlinkPhase--;
						}
					}
					if (timerCaused)
					{
						this.provider.showIcon = !this.provider.showIcon;
					}
					DeviceContext deviceContext = null;
					using (DeviceContext deviceContext = DeviceContext.FromHwnd(base.Handle))
					{
						this.CreateMirrorDC(deviceContext.Hdc, this.windowBounds.Width);
						Graphics graphics = Graphics.FromHdcInternal(this.mirrordc.Hdc);
						try
						{
							intPtr = region2.GetHrgn(graphics);
							global::System.Internal.HandleCollector.Add(intPtr, NativeMethods.CommonHandles.GDI);
						}
						finally
						{
							graphics.Dispose();
							this.RestoreMirrorDC();
						}
						if (UnsafeNativeMethods.SetWindowRgn(new HandleRef(this, base.Handle), new HandleRef(region2, intPtr), true) != 0)
						{
							intPtr = IntPtr.Zero;
						}
					}
				}
				finally
				{
					region2.Dispose();
					if (intPtr != IntPtr.Zero)
					{
						SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
					}
				}
				SafeNativeMethods.SetWindowPos(new HandleRef(this, base.Handle), NativeMethods.HWND_TOP, this.windowBounds.X, this.windowBounds.Y, this.windowBounds.Width, this.windowBounds.Height, 16);
				SafeNativeMethods.InvalidateRect(new HandleRef(this, base.Handle), null, false);
			}

			// Token: 0x06003B56 RID: 15190 RVA: 0x000D76D4 File Offset: 0x000D66D4
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg != 15)
				{
					if (msg != 20)
					{
						if (msg == 78)
						{
							NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
							if (nmhdr.code == -521 || nmhdr.code == -522)
							{
								this.OnToolTipVisibilityChanging(nmhdr.idFrom, nmhdr.code == -521);
								return;
							}
						}
						else
						{
							base.WndProc(ref m);
						}
					}
					return;
				}
				this.OnPaint(ref m);
			}

			// Token: 0x04001DA4 RID: 7588
			private ArrayList items = new ArrayList();

			// Token: 0x04001DA5 RID: 7589
			private Control parent;

			// Token: 0x04001DA6 RID: 7590
			private ErrorProvider provider;

			// Token: 0x04001DA7 RID: 7591
			private Rectangle windowBounds = Rectangle.Empty;

			// Token: 0x04001DA8 RID: 7592
			private Timer timer;

			// Token: 0x04001DA9 RID: 7593
			private NativeWindow tipWindow;

			// Token: 0x04001DAA RID: 7594
			private DeviceContext mirrordc;

			// Token: 0x04001DAB RID: 7595
			private Size mirrordcExtent = Size.Empty;

			// Token: 0x04001DAC RID: 7596
			private Point mirrordcOrigin = Point.Empty;

			// Token: 0x04001DAD RID: 7597
			private DeviceContextMapMode mirrordcMode = DeviceContextMapMode.Text;
		}

		// Token: 0x020003DE RID: 990
		internal class ControlItem
		{
			// Token: 0x06003B57 RID: 15191 RVA: 0x000D7754 File Offset: 0x000D6754
			public ControlItem(ErrorProvider provider, Control control, IntPtr id)
			{
				this.toolTipShown = false;
				this.iconAlignment = ErrorIconAlignment.MiddleRight;
				this.error = string.Empty;
				this.id = id;
				this.control = control;
				this.provider = provider;
				this.control.HandleCreated += this.OnCreateHandle;
				this.control.HandleDestroyed += this.OnDestroyHandle;
				this.control.LocationChanged += this.OnBoundsChanged;
				this.control.SizeChanged += this.OnBoundsChanged;
				this.control.VisibleChanged += this.OnParentVisibleChanged;
				this.control.ParentChanged += this.OnParentVisibleChanged;
			}

			// Token: 0x06003B58 RID: 15192 RVA: 0x000D7820 File Offset: 0x000D6820
			public void Dispose()
			{
				if (this.control != null)
				{
					this.control.HandleCreated -= this.OnCreateHandle;
					this.control.HandleDestroyed -= this.OnDestroyHandle;
					this.control.LocationChanged -= this.OnBoundsChanged;
					this.control.SizeChanged -= this.OnBoundsChanged;
					this.control.VisibleChanged -= this.OnParentVisibleChanged;
					this.control.ParentChanged -= this.OnParentVisibleChanged;
				}
				this.error = string.Empty;
			}

			// Token: 0x17000B48 RID: 2888
			// (get) Token: 0x06003B59 RID: 15193 RVA: 0x000D78CD File Offset: 0x000D68CD
			public IntPtr Id
			{
				get
				{
					return this.id;
				}
			}

			// Token: 0x17000B49 RID: 2889
			// (get) Token: 0x06003B5A RID: 15194 RVA: 0x000D78D5 File Offset: 0x000D68D5
			// (set) Token: 0x06003B5B RID: 15195 RVA: 0x000D78DD File Offset: 0x000D68DD
			public int BlinkPhase
			{
				get
				{
					return this.blinkPhase;
				}
				set
				{
					this.blinkPhase = value;
				}
			}

			// Token: 0x17000B4A RID: 2890
			// (get) Token: 0x06003B5C RID: 15196 RVA: 0x000D78E6 File Offset: 0x000D68E6
			// (set) Token: 0x06003B5D RID: 15197 RVA: 0x000D78EE File Offset: 0x000D68EE
			public int IconPadding
			{
				get
				{
					return this.iconPadding;
				}
				set
				{
					if (this.iconPadding != value)
					{
						this.iconPadding = value;
						this.UpdateWindow();
					}
				}
			}

			// Token: 0x17000B4B RID: 2891
			// (get) Token: 0x06003B5E RID: 15198 RVA: 0x000D7906 File Offset: 0x000D6906
			// (set) Token: 0x06003B5F RID: 15199 RVA: 0x000D7910 File Offset: 0x000D6910
			public string Error
			{
				get
				{
					return this.error;
				}
				set
				{
					if (value == null)
					{
						value = "";
					}
					if (this.error.Equals(value) && this.provider.BlinkStyle != ErrorBlinkStyle.AlwaysBlink)
					{
						return;
					}
					bool flag = this.error.Length == 0;
					this.error = value;
					if (value.Length == 0)
					{
						this.RemoveFromWindow();
						return;
					}
					if (flag)
					{
						this.AddToWindow();
						return;
					}
					if (this.provider.BlinkStyle != ErrorBlinkStyle.NeverBlink)
					{
						this.StartBlinking();
						return;
					}
					this.UpdateWindow();
				}
			}

			// Token: 0x17000B4C RID: 2892
			// (get) Token: 0x06003B60 RID: 15200 RVA: 0x000D798E File Offset: 0x000D698E
			// (set) Token: 0x06003B61 RID: 15201 RVA: 0x000D7996 File Offset: 0x000D6996
			public ErrorIconAlignment IconAlignment
			{
				get
				{
					return this.iconAlignment;
				}
				set
				{
					if (this.iconAlignment != value)
					{
						if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
						{
							throw new InvalidEnumArgumentException("value", (int)value, typeof(ErrorIconAlignment));
						}
						this.iconAlignment = value;
						this.UpdateWindow();
					}
				}
			}

			// Token: 0x17000B4D RID: 2893
			// (get) Token: 0x06003B62 RID: 15202 RVA: 0x000D79D4 File Offset: 0x000D69D4
			// (set) Token: 0x06003B63 RID: 15203 RVA: 0x000D79DC File Offset: 0x000D69DC
			public bool ToolTipShown
			{
				get
				{
					return this.toolTipShown;
				}
				set
				{
					this.toolTipShown = value;
				}
			}

			// Token: 0x06003B64 RID: 15204 RVA: 0x000D79E8 File Offset: 0x000D69E8
			internal ErrorIconAlignment RTLTranslateIconAlignment(ErrorIconAlignment align)
			{
				if (!this.provider.RightToLeft)
				{
					return align;
				}
				switch (align)
				{
				case ErrorIconAlignment.TopLeft:
					return ErrorIconAlignment.TopRight;
				case ErrorIconAlignment.TopRight:
					return ErrorIconAlignment.TopLeft;
				case ErrorIconAlignment.MiddleLeft:
					return ErrorIconAlignment.MiddleRight;
				case ErrorIconAlignment.MiddleRight:
					return ErrorIconAlignment.MiddleLeft;
				case ErrorIconAlignment.BottomLeft:
					return ErrorIconAlignment.BottomRight;
				case ErrorIconAlignment.BottomRight:
					return ErrorIconAlignment.BottomLeft;
				default:
					return align;
				}
			}

			// Token: 0x06003B65 RID: 15205 RVA: 0x000D7A34 File Offset: 0x000D6A34
			internal Rectangle GetIconBounds(Size size)
			{
				int num = 0;
				int num2 = 0;
				switch (this.RTLTranslateIconAlignment(this.IconAlignment))
				{
				case ErrorIconAlignment.TopLeft:
				case ErrorIconAlignment.MiddleLeft:
				case ErrorIconAlignment.BottomLeft:
					num = this.control.Left - size.Width - this.iconPadding;
					break;
				case ErrorIconAlignment.TopRight:
				case ErrorIconAlignment.MiddleRight:
				case ErrorIconAlignment.BottomRight:
					num = this.control.Right + this.iconPadding;
					break;
				}
				switch (this.IconAlignment)
				{
				case ErrorIconAlignment.TopLeft:
				case ErrorIconAlignment.TopRight:
					num2 = this.control.Top;
					break;
				case ErrorIconAlignment.MiddleLeft:
				case ErrorIconAlignment.MiddleRight:
					num2 = this.control.Top + (this.control.Height - size.Height) / 2;
					break;
				case ErrorIconAlignment.BottomLeft:
				case ErrorIconAlignment.BottomRight:
					num2 = this.control.Bottom - size.Height;
					break;
				}
				return new Rectangle(num, num2, size.Width, size.Height);
			}

			// Token: 0x06003B66 RID: 15206 RVA: 0x000D7B24 File Offset: 0x000D6B24
			private void UpdateWindow()
			{
				if (this.window != null)
				{
					this.window.Update(false);
				}
			}

			// Token: 0x06003B67 RID: 15207 RVA: 0x000D7B3A File Offset: 0x000D6B3A
			private void StartBlinking()
			{
				if (this.window != null)
				{
					this.BlinkPhase = 10;
					this.window.StartBlinking();
				}
			}

			// Token: 0x06003B68 RID: 15208 RVA: 0x000D7B58 File Offset: 0x000D6B58
			private void AddToWindow()
			{
				if (this.window == null && (this.control.Created || this.control.RecreatingHandle) && this.control.Visible && this.control.ParentInternal != null && this.error.Length > 0)
				{
					this.window = this.provider.EnsureErrorWindow(this.control.ParentInternal);
					this.window.Add(this);
					if (this.provider.BlinkStyle != ErrorBlinkStyle.NeverBlink)
					{
						this.StartBlinking();
					}
				}
			}

			// Token: 0x06003B69 RID: 15209 RVA: 0x000D7BEB File Offset: 0x000D6BEB
			private void RemoveFromWindow()
			{
				if (this.window != null)
				{
					this.window.Remove(this);
					this.window = null;
				}
			}

			// Token: 0x06003B6A RID: 15210 RVA: 0x000D7C08 File Offset: 0x000D6C08
			private void OnBoundsChanged(object sender, EventArgs e)
			{
				this.UpdateWindow();
			}

			// Token: 0x06003B6B RID: 15211 RVA: 0x000D7C10 File Offset: 0x000D6C10
			private void OnParentVisibleChanged(object sender, EventArgs e)
			{
				this.BlinkPhase = 0;
				this.RemoveFromWindow();
				this.AddToWindow();
			}

			// Token: 0x06003B6C RID: 15212 RVA: 0x000D7C25 File Offset: 0x000D6C25
			private void OnCreateHandle(object sender, EventArgs e)
			{
				this.AddToWindow();
			}

			// Token: 0x06003B6D RID: 15213 RVA: 0x000D7C2D File Offset: 0x000D6C2D
			private void OnDestroyHandle(object sender, EventArgs e)
			{
				this.RemoveFromWindow();
			}

			// Token: 0x04001DAE RID: 7598
			private const int startingBlinkPhase = 10;

			// Token: 0x04001DAF RID: 7599
			private string error;

			// Token: 0x04001DB0 RID: 7600
			private Control control;

			// Token: 0x04001DB1 RID: 7601
			private ErrorProvider.ErrorWindow window;

			// Token: 0x04001DB2 RID: 7602
			private ErrorProvider provider;

			// Token: 0x04001DB3 RID: 7603
			private int blinkPhase;

			// Token: 0x04001DB4 RID: 7604
			private IntPtr id;

			// Token: 0x04001DB5 RID: 7605
			private int iconPadding;

			// Token: 0x04001DB6 RID: 7606
			private bool toolTipShown;

			// Token: 0x04001DB7 RID: 7607
			private ErrorIconAlignment iconAlignment;
		}

		// Token: 0x020003DF RID: 991
		internal class IconRegion
		{
			// Token: 0x06003B6E RID: 15214 RVA: 0x000D7C35 File Offset: 0x000D6C35
			public IconRegion(Icon icon)
			{
				this.icon = new Icon(icon, 16, 16);
			}

			// Token: 0x17000B4E RID: 2894
			// (get) Token: 0x06003B6F RID: 15215 RVA: 0x000D7C4D File Offset: 0x000D6C4D
			public IntPtr IconHandle
			{
				get
				{
					return this.icon.Handle;
				}
			}

			// Token: 0x17000B4F RID: 2895
			// (get) Token: 0x06003B70 RID: 15216 RVA: 0x000D7C5C File Offset: 0x000D6C5C
			public Region Region
			{
				get
				{
					if (this.region == null)
					{
						this.region = new Region(new Rectangle(0, 0, 0, 0));
						IntPtr intPtr = IntPtr.Zero;
						try
						{
							Size size = this.icon.Size;
							Bitmap bitmap = this.icon.ToBitmap();
							bitmap.MakeTransparent();
							intPtr = ControlPaint.CreateHBitmapTransparencyMask(bitmap);
							bitmap.Dispose();
							int num = size.Width / 8;
							byte[] array = new byte[num * size.Height];
							SafeNativeMethods.GetBitmapBits(new HandleRef(null, intPtr), array.Length, array);
							for (int i = 0; i < size.Height; i++)
							{
								for (int j = 0; j < size.Width; j++)
								{
									if (((int)array[i * num + j / 8] & (1 << 7 - j % 8)) == 0)
									{
										this.region.Union(new Rectangle(j, i, 1, 1));
									}
								}
							}
							this.region.Intersect(new Rectangle(0, 0, size.Width, size.Height));
						}
						finally
						{
							if (intPtr != IntPtr.Zero)
							{
								SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
							}
						}
					}
					return this.region;
				}
			}

			// Token: 0x17000B50 RID: 2896
			// (get) Token: 0x06003B71 RID: 15217 RVA: 0x000D7D98 File Offset: 0x000D6D98
			public Size Size
			{
				get
				{
					return this.icon.Size;
				}
			}

			// Token: 0x06003B72 RID: 15218 RVA: 0x000D7DA5 File Offset: 0x000D6DA5
			public void Dispose()
			{
				if (this.region != null)
				{
					this.region.Dispose();
					this.region = null;
				}
				this.icon.Dispose();
			}

			// Token: 0x04001DB8 RID: 7608
			private Region region;

			// Token: 0x04001DB9 RID: 7609
			private Icon icon;
		}
	}
}

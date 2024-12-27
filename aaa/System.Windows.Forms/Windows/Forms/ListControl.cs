using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000265 RID: 613
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")]
	public abstract class ListControl : Control
	{
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06002012 RID: 8210 RVA: 0x00043EBB File Offset: 0x00042EBB
		// (set) Token: 0x06002013 RID: 8211 RVA: 0x00043EC4 File Offset: 0x00042EC4
		[AttributeProvider(typeof(IListSource))]
		[SRDescription("ListControlDataSourceDescr")]
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				if (value != null && !(value is IList) && !(value is IListSource))
				{
					throw new ArgumentException(SR.GetString("BadDataSourceForComplexBinding"));
				}
				if (this.dataSource == value)
				{
					return;
				}
				try
				{
					this.SetDataConnection(value, this.displayMember, false);
				}
				catch
				{
					this.DisplayMember = "";
				}
				if (value == null)
				{
					this.DisplayMember = "";
				}
			}
		}

		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x06002014 RID: 8212 RVA: 0x00043F3C File Offset: 0x00042F3C
		// (remove) Token: 0x06002015 RID: 8213 RVA: 0x00043F4F File Offset: 0x00042F4F
		[SRDescription("ListControlOnDataSourceChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler DataSourceChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_DATASOURCECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_DATASOURCECHANGED, value);
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06002016 RID: 8214 RVA: 0x00043F62 File Offset: 0x00042F62
		protected CurrencyManager DataManager
		{
			get
			{
				return this.dataManager;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06002017 RID: 8215 RVA: 0x00043F6A File Offset: 0x00042F6A
		// (set) Token: 0x06002018 RID: 8216 RVA: 0x00043F78 File Offset: 0x00042F78
		[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue("")]
		[SRDescription("ListControlDisplayMemberDescr")]
		[SRCategory("CatData")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string DisplayMember
		{
			get
			{
				return this.displayMember.BindingMember;
			}
			set
			{
				BindingMemberInfo bindingMemberInfo = this.displayMember;
				try
				{
					this.SetDataConnection(this.dataSource, new BindingMemberInfo(value), false);
				}
				catch
				{
					this.displayMember = bindingMemberInfo;
				}
			}
		}

		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x06002019 RID: 8217 RVA: 0x00043FBC File Offset: 0x00042FBC
		// (remove) Token: 0x0600201A RID: 8218 RVA: 0x00043FCF File Offset: 0x00042FCF
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlOnDisplayMemberChangedDescr")]
		public event EventHandler DisplayMemberChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_DISPLAYMEMBERCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_DISPLAYMEMBERCHANGED, value);
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600201B RID: 8219 RVA: 0x00043FE4 File Offset: 0x00042FE4
		private TypeConverter DisplayMemberConverter
		{
			get
			{
				if (this.displayMemberConverter == null && this.DataManager != null)
				{
					PropertyDescriptorCollection itemProperties = this.DataManager.GetItemProperties();
					if (itemProperties != null)
					{
						PropertyDescriptor propertyDescriptor = itemProperties.Find(this.displayMember.BindingField, true);
						if (propertyDescriptor != null)
						{
							this.displayMemberConverter = propertyDescriptor.Converter;
						}
					}
				}
				return this.displayMemberConverter;
			}
		}

		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x0600201C RID: 8220 RVA: 0x00044038 File Offset: 0x00043038
		// (remove) Token: 0x0600201D RID: 8221 RVA: 0x00044051 File Offset: 0x00043051
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlFormatDescr")]
		public event ListControlConvertEventHandler Format
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_FORMAT, value);
				this.RefreshItems();
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_FORMAT, value);
				this.RefreshItems();
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x0004406A File Offset: 0x0004306A
		// (set) Token: 0x0600201F RID: 8223 RVA: 0x00044072 File Offset: 0x00043072
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[DefaultValue(null)]
		public IFormatProvider FormatInfo
		{
			get
			{
				return this.formatInfo;
			}
			set
			{
				if (value != this.formatInfo)
				{
					this.formatInfo = value;
					this.RefreshItems();
					this.OnFormatInfoChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06002020 RID: 8224 RVA: 0x00044095 File Offset: 0x00043095
		// (remove) Token: 0x06002021 RID: 8225 RVA: 0x000440A8 File Offset: 0x000430A8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlFormatInfoChangedDescr")]
		public event EventHandler FormatInfoChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_FORMATINFOCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_FORMATINFOCHANGED, value);
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x000440BB File Offset: 0x000430BB
		// (set) Token: 0x06002023 RID: 8227 RVA: 0x000440C3 File Offset: 0x000430C3
		[MergableProperty(false)]
		[Editor("System.Windows.Forms.Design.FormatStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[SRDescription("ListControlFormatStringDescr")]
		public string FormatString
		{
			get
			{
				return this.formatString;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (!value.Equals(this.formatString))
				{
					this.formatString = value;
					this.RefreshItems();
					this.OnFormatStringChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x06002024 RID: 8228 RVA: 0x000440F5 File Offset: 0x000430F5
		// (remove) Token: 0x06002025 RID: 8229 RVA: 0x00044108 File Offset: 0x00043108
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlFormatStringChangedDescr")]
		public event EventHandler FormatStringChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_FORMATSTRINGCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_FORMATSTRINGCHANGED, value);
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x0004411B File Offset: 0x0004311B
		// (set) Token: 0x06002027 RID: 8231 RVA: 0x00044123 File Offset: 0x00043123
		[DefaultValue(false)]
		[SRDescription("ListControlFormattingEnabledDescr")]
		public bool FormattingEnabled
		{
			get
			{
				return this.formattingEnabled;
			}
			set
			{
				if (value != this.formattingEnabled)
				{
					this.formattingEnabled = value;
					this.RefreshItems();
					this.OnFormattingEnabledChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140000C6 RID: 198
		// (add) Token: 0x06002028 RID: 8232 RVA: 0x00044146 File Offset: 0x00043146
		// (remove) Token: 0x06002029 RID: 8233 RVA: 0x00044159 File Offset: 0x00043159
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlFormattingEnabledChangedDescr")]
		public event EventHandler FormattingEnabledChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_FORMATTINGENABLEDCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_FORMATTINGENABLEDCHANGED, value);
			}
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x0004416C File Offset: 0x0004316C
		private bool BindingMemberInfoInDataManager(BindingMemberInfo bindingMemberInfo)
		{
			if (this.dataManager == null)
			{
				return false;
			}
			PropertyDescriptorCollection itemProperties = this.dataManager.GetItemProperties();
			int count = itemProperties.Count;
			for (int i = 0; i < count; i++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[i].PropertyType) && itemProperties[i].Name.Equals(bindingMemberInfo.BindingField))
				{
					return true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[j].PropertyType) && string.Compare(itemProperties[j].Name, bindingMemberInfo.BindingField, true, CultureInfo.CurrentCulture) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x00044227 File Offset: 0x00043227
		// (set) Token: 0x0600202C RID: 8236 RVA: 0x00044234 File Offset: 0x00043234
		[SRCategory("CatData")]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ListControlValueMemberDescr")]
		public string ValueMember
		{
			get
			{
				return this.valueMember.BindingMember;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				BindingMemberInfo bindingMemberInfo = new BindingMemberInfo(value);
				if (!bindingMemberInfo.Equals(this.valueMember))
				{
					if (this.DisplayMember.Length == 0)
					{
						this.SetDataConnection(this.DataSource, bindingMemberInfo, false);
					}
					if (this.dataManager != null && value != null && value.Length != 0 && !this.BindingMemberInfoInDataManager(bindingMemberInfo))
					{
						throw new ArgumentException(SR.GetString("ListControlWrongValueMember"), "value");
					}
					this.valueMember = bindingMemberInfo;
					this.OnValueMemberChanged(EventArgs.Empty);
					this.OnSelectedValueChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x140000C7 RID: 199
		// (add) Token: 0x0600202D RID: 8237 RVA: 0x000442D6 File Offset: 0x000432D6
		// (remove) Token: 0x0600202E RID: 8238 RVA: 0x000442E9 File Offset: 0x000432E9
		[SRDescription("ListControlOnValueMemberChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler ValueMemberChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_VALUEMEMBERCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_VALUEMEMBERCHANGED, value);
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x000442FC File Offset: 0x000432FC
		protected virtual bool AllowSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06002030 RID: 8240
		// (set) Token: 0x06002031 RID: 8241
		public abstract int SelectedIndex { get; set; }

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x00044300 File Offset: 0x00043300
		// (set) Token: 0x06002033 RID: 8243 RVA: 0x00044348 File Offset: 0x00043348
		[Browsable(false)]
		[DefaultValue(null)]
		[Bindable(true)]
		[SRCategory("CatData")]
		[SRDescription("ListControlSelectedValueDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedValue
		{
			get
			{
				if (this.SelectedIndex != -1 && this.dataManager != null)
				{
					object obj = this.dataManager[this.SelectedIndex];
					return this.FilterItemOnProperty(obj, this.valueMember.BindingField);
				}
				return null;
			}
			set
			{
				if (this.dataManager != null)
				{
					string bindingField = this.valueMember.BindingField;
					if (string.IsNullOrEmpty(bindingField))
					{
						throw new InvalidOperationException(SR.GetString("ListControlEmptyValueMemberInSettingSelectedValue"));
					}
					PropertyDescriptorCollection itemProperties = this.dataManager.GetItemProperties();
					PropertyDescriptor propertyDescriptor = itemProperties.Find(bindingField, true);
					int num = this.dataManager.Find(propertyDescriptor, value, true);
					this.SelectedIndex = num;
				}
			}
		}

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x06002034 RID: 8244 RVA: 0x000443AC File Offset: 0x000433AC
		// (remove) Token: 0x06002035 RID: 8245 RVA: 0x000443BF File Offset: 0x000433BF
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ListControlOnSelectedValueChangedDescr")]
		public event EventHandler SelectedValueChanged
		{
			add
			{
				base.Events.AddHandler(ListControl.EVENT_SELECTEDVALUECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListControl.EVENT_SELECTEDVALUECHANGED, value);
			}
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x000443D2 File Offset: 0x000433D2
		private void DataManager_PositionChanged(object sender, EventArgs e)
		{
			if (this.dataManager != null && this.AllowSelection)
			{
				this.SelectedIndex = this.dataManager.Position;
			}
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000443F8 File Offset: 0x000433F8
		private void DataManager_ItemChanged(object sender, ItemChangedEventArgs e)
		{
			if (this.dataManager != null)
			{
				if (e.Index == -1)
				{
					this.SetItemsCore(this.dataManager.List);
					if (this.AllowSelection)
					{
						this.SelectedIndex = this.dataManager.Position;
						return;
					}
				}
				else
				{
					this.SetItemCore(e.Index, this.dataManager[e.Index]);
				}
			}
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x0004445E File Offset: 0x0004345E
		protected object FilterItemOnProperty(object item)
		{
			return this.FilterItemOnProperty(item, this.displayMember.BindingField);
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x00044474 File Offset: 0x00043474
		protected object FilterItemOnProperty(object item, string field)
		{
			if (item != null && field.Length > 0)
			{
				try
				{
					PropertyDescriptor propertyDescriptor;
					if (this.dataManager != null)
					{
						propertyDescriptor = this.dataManager.GetItemProperties().Find(field, true);
					}
					else
					{
						propertyDescriptor = TypeDescriptor.GetProperties(item).Find(field, true);
					}
					if (propertyDescriptor != null)
					{
						item = propertyDescriptor.GetValue(item);
					}
				}
				catch
				{
				}
			}
			return item;
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600203A RID: 8250 RVA: 0x000444DC File Offset: 0x000434DC
		internal bool BindingFieldEmpty
		{
			get
			{
				return this.displayMember.BindingField.Length <= 0;
			}
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x000444F4 File Offset: 0x000434F4
		internal int FindStringInternal(string str, IList items, int startIndex, bool exact)
		{
			return this.FindStringInternal(str, items, startIndex, exact, true);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00044504 File Offset: 0x00043504
		internal int FindStringInternal(string str, IList items, int startIndex, bool exact, bool ignorecase)
		{
			if (str == null || items == null)
			{
				return -1;
			}
			if (startIndex < -1 || startIndex >= items.Count)
			{
				return -1;
			}
			int length = str.Length;
			int i = 0;
			int num = (startIndex + 1) % items.Count;
			while (i < items.Count)
			{
				i++;
				bool flag;
				if (exact)
				{
					flag = string.Compare(str, this.GetItemText(items[num]), ignorecase, CultureInfo.CurrentCulture) == 0;
				}
				else
				{
					flag = string.Compare(str, 0, this.GetItemText(items[num]), 0, length, ignorecase, CultureInfo.CurrentCulture) == 0;
				}
				if (flag)
				{
					return num;
				}
				num = (num + 1) % items.Count;
			}
			return -1;
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x000445A4 File Offset: 0x000435A4
		public string GetItemText(object item)
		{
			if (!this.formattingEnabled)
			{
				if (item == null)
				{
					return string.Empty;
				}
				item = this.FilterItemOnProperty(item, this.displayMember.BindingField);
				if (item == null)
				{
					return "";
				}
				return Convert.ToString(item, CultureInfo.CurrentCulture);
			}
			else
			{
				object obj = this.FilterItemOnProperty(item, this.displayMember.BindingField);
				ListControlConvertEventArgs listControlConvertEventArgs = new ListControlConvertEventArgs(obj, typeof(string), item);
				this.OnFormat(listControlConvertEventArgs);
				if (listControlConvertEventArgs.Value != item && listControlConvertEventArgs.Value is string)
				{
					return (string)listControlConvertEventArgs.Value;
				}
				if (ListControl.stringTypeConverter == null)
				{
					ListControl.stringTypeConverter = TypeDescriptor.GetConverter(typeof(string));
				}
				string text;
				try
				{
					text = (string)Formatter.FormatObject(obj, typeof(string), this.DisplayMemberConverter, ListControl.stringTypeConverter, this.formatString, this.formatInfo, null, DBNull.Value);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					text = ((obj != null) ? Convert.ToString(item, CultureInfo.CurrentCulture) : "");
				}
				return text;
			}
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x000446C0 File Offset: 0x000436C0
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			switch (keyData & Keys.KeyCode)
			{
			case Keys.Prior:
			case Keys.Next:
			case Keys.End:
			case Keys.Home:
				return true;
			default:
				return base.IsInputKey(keyData);
			}
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x00044709 File Offset: 0x00043709
		protected override void OnBindingContextChanged(EventArgs e)
		{
			this.SetDataConnection(this.dataSource, this.displayMember, true);
			base.OnBindingContextChanged(e);
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x00044728 File Offset: 0x00043728
		protected virtual void OnDataSourceChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_DATASOURCECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x00044758 File Offset: 0x00043758
		protected virtual void OnDisplayMemberChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_DISPLAYMEMBERCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x00044788 File Offset: 0x00043788
		protected virtual void OnFormat(ListControlConvertEventArgs e)
		{
			ListControlConvertEventHandler listControlConvertEventHandler = base.Events[ListControl.EVENT_FORMAT] as ListControlConvertEventHandler;
			if (listControlConvertEventHandler != null)
			{
				listControlConvertEventHandler(this, e);
			}
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x000447B8 File Offset: 0x000437B8
		protected virtual void OnFormatInfoChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_FORMATINFOCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x000447E8 File Offset: 0x000437E8
		protected virtual void OnFormatStringChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_FORMATSTRINGCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x00044818 File Offset: 0x00043818
		protected virtual void OnFormattingEnabledChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_FORMATTINGENABLEDCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x00044846 File Offset: 0x00043846
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			this.OnSelectedValueChanged(EventArgs.Empty);
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x00044854 File Offset: 0x00043854
		protected virtual void OnValueMemberChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_VALUEMEMBERCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x00044884 File Offset: 0x00043884
		protected virtual void OnSelectedValueChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[ListControl.EVENT_SELECTEDVALUECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002049 RID: 8265
		protected abstract void RefreshItem(int index);

		// Token: 0x0600204A RID: 8266 RVA: 0x000448B2 File Offset: 0x000438B2
		protected virtual void RefreshItems()
		{
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000448B4 File Offset: 0x000438B4
		private void DataSourceDisposed(object sender, EventArgs e)
		{
			this.SetDataConnection(null, new BindingMemberInfo(""), true);
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000448C8 File Offset: 0x000438C8
		private void DataSourceInitialized(object sender, EventArgs e)
		{
			this.SetDataConnection(this.dataSource, this.displayMember, true);
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000448E0 File Offset: 0x000438E0
		private void SetDataConnection(object newDataSource, BindingMemberInfo newDisplayMember, bool force)
		{
			bool flag = this.dataSource != newDataSource;
			bool flag2 = !this.displayMember.Equals(newDisplayMember);
			if (this.inSetDataConnection)
			{
				return;
			}
			try
			{
				if (force || flag || flag2)
				{
					this.inSetDataConnection = true;
					IList list = ((this.DataManager != null) ? this.DataManager.List : null);
					bool flag3 = this.DataManager == null;
					this.UnwireDataSource();
					this.dataSource = newDataSource;
					this.displayMember = newDisplayMember;
					this.WireDataSource();
					if (this.isDataSourceInitialized)
					{
						CurrencyManager currencyManager = null;
						if (newDataSource != null && this.BindingContext != null && newDataSource != Convert.DBNull)
						{
							currencyManager = (CurrencyManager)this.BindingContext[newDataSource, newDisplayMember.BindingPath];
						}
						if (this.dataManager != currencyManager)
						{
							if (this.dataManager != null)
							{
								this.dataManager.ItemChanged -= this.DataManager_ItemChanged;
								this.dataManager.PositionChanged -= this.DataManager_PositionChanged;
							}
							this.dataManager = currencyManager;
							if (this.dataManager != null)
							{
								this.dataManager.ItemChanged += this.DataManager_ItemChanged;
								this.dataManager.PositionChanged += this.DataManager_PositionChanged;
							}
						}
						if (this.dataManager != null && (flag2 || flag) && this.displayMember.BindingMember != null && this.displayMember.BindingMember.Length != 0 && !this.BindingMemberInfoInDataManager(this.displayMember))
						{
							throw new ArgumentException(SR.GetString("ListControlWrongDisplayMember"), "newDisplayMember");
						}
						if (this.dataManager != null && (flag || flag2 || force) && (flag2 || (force && (list != this.dataManager.List || flag3))))
						{
							this.DataManager_ItemChanged(this.dataManager, new ItemChangedEventArgs(-1));
						}
					}
					this.displayMemberConverter = null;
				}
				if (flag)
				{
					this.OnDataSourceChanged(EventArgs.Empty);
				}
				if (flag2)
				{
					this.OnDisplayMemberChanged(EventArgs.Empty);
				}
			}
			finally
			{
				this.inSetDataConnection = false;
			}
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x00044AF8 File Offset: 0x00043AF8
		private void UnwireDataSource()
		{
			if (this.dataSource is IComponent)
			{
				((IComponent)this.dataSource).Disposed -= this.DataSourceDisposed;
			}
			ISupportInitializeNotification supportInitializeNotification = this.dataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null && this.isDataSourceInitEventHooked)
			{
				supportInitializeNotification.Initialized -= this.DataSourceInitialized;
				this.isDataSourceInitEventHooked = false;
			}
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x00044B60 File Offset: 0x00043B60
		private void WireDataSource()
		{
			if (this.dataSource is IComponent)
			{
				((IComponent)this.dataSource).Disposed += this.DataSourceDisposed;
			}
			ISupportInitializeNotification supportInitializeNotification = this.dataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null && !supportInitializeNotification.IsInitialized)
			{
				supportInitializeNotification.Initialized += this.DataSourceInitialized;
				this.isDataSourceInitEventHooked = true;
				this.isDataSourceInitialized = false;
				return;
			}
			this.isDataSourceInitialized = true;
		}

		// Token: 0x06002050 RID: 8272
		protected abstract void SetItemsCore(IList items);

		// Token: 0x06002051 RID: 8273 RVA: 0x00044BD5 File Offset: 0x00043BD5
		protected virtual void SetItemCore(int index, object value)
		{
		}

		// Token: 0x0400147F RID: 5247
		private static readonly object EVENT_DATASOURCECHANGED = new object();

		// Token: 0x04001480 RID: 5248
		private static readonly object EVENT_DISPLAYMEMBERCHANGED = new object();

		// Token: 0x04001481 RID: 5249
		private static readonly object EVENT_VALUEMEMBERCHANGED = new object();

		// Token: 0x04001482 RID: 5250
		private static readonly object EVENT_SELECTEDVALUECHANGED = new object();

		// Token: 0x04001483 RID: 5251
		private static readonly object EVENT_FORMATINFOCHANGED = new object();

		// Token: 0x04001484 RID: 5252
		private static readonly object EVENT_FORMATSTRINGCHANGED = new object();

		// Token: 0x04001485 RID: 5253
		private static readonly object EVENT_FORMATTINGENABLEDCHANGED = new object();

		// Token: 0x04001486 RID: 5254
		private object dataSource;

		// Token: 0x04001487 RID: 5255
		private CurrencyManager dataManager;

		// Token: 0x04001488 RID: 5256
		private BindingMemberInfo displayMember;

		// Token: 0x04001489 RID: 5257
		private BindingMemberInfo valueMember;

		// Token: 0x0400148A RID: 5258
		private string formatString = string.Empty;

		// Token: 0x0400148B RID: 5259
		private IFormatProvider formatInfo;

		// Token: 0x0400148C RID: 5260
		private bool formattingEnabled;

		// Token: 0x0400148D RID: 5261
		private static readonly object EVENT_FORMAT = new object();

		// Token: 0x0400148E RID: 5262
		private TypeConverter displayMemberConverter;

		// Token: 0x0400148F RID: 5263
		private static TypeConverter stringTypeConverter = null;

		// Token: 0x04001490 RID: 5264
		private bool isDataSourceInitialized;

		// Token: 0x04001491 RID: 5265
		private bool isDataSourceInitEventHooked;

		// Token: 0x04001492 RID: 5266
		private bool inSetDataConnection;
	}
}

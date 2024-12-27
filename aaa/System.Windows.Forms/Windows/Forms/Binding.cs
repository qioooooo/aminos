using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000239 RID: 569
	[TypeConverter(typeof(ListBindingConverter))]
	public class Binding
	{
		// Token: 0x06001B0C RID: 6924 RVA: 0x0003406C File Offset: 0x0003306C
		public Binding(string propertyName, object dataSource, string dataMember)
			: this(propertyName, dataSource, dataMember, false, DataSourceUpdateMode.OnValidation, null, string.Empty, null)
		{
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x0003408C File Offset: 0x0003308C
		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled)
			: this(propertyName, dataSource, dataMember, formattingEnabled, DataSourceUpdateMode.OnValidation, null, string.Empty, null)
		{
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x000340AC File Offset: 0x000330AC
		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode)
			: this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, null, string.Empty, null)
		{
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x000340D0 File Offset: 0x000330D0
		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue)
			: this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, string.Empty, null)
		{
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x000340F4 File Offset: 0x000330F4
		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue, string formatString)
			: this(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, formatString, null)
		{
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x00034114 File Offset: 0x00033114
		public Binding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue, string formatString, IFormatProvider formatInfo)
		{
			this.propertyName = "";
			this.formatString = string.Empty;
			this.dsNullValue = Formatter.GetDefaultDataSourceNullValue(null);
			base..ctor();
			this.bindToObject = new BindToObject(this, dataSource, dataMember);
			this.propertyName = propertyName;
			this.formattingEnabled = formattingEnabled;
			this.formatString = formatString;
			this.nullValue = nullValue;
			this.formatInfo = formatInfo;
			this.formattingEnabled = formattingEnabled;
			this.dataSourceUpdateMode = dataSourceUpdateMode;
			this.CheckBinding();
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x00034194 File Offset: 0x00033194
		private Binding()
		{
			this.propertyName = "";
			this.formatString = string.Empty;
			this.dsNullValue = Formatter.GetDefaultDataSourceNullValue(null);
			base..ctor();
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x000341BE File Offset: 0x000331BE
		internal BindToObject BindToObject
		{
			get
			{
				return this.bindToObject;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x000341C6 File Offset: 0x000331C6
		public object DataSource
		{
			get
			{
				return this.bindToObject.DataSource;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x000341D3 File Offset: 0x000331D3
		public BindingMemberInfo BindingMemberInfo
		{
			get
			{
				return this.bindToObject.BindingMemberInfo;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06001B16 RID: 6934 RVA: 0x000341E0 File Offset: 0x000331E0
		[DefaultValue(null)]
		public IBindableComponent BindableComponent
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.control;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x000341E8 File Offset: 0x000331E8
		[DefaultValue(null)]
		public Control Control
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.control as Control;
			}
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x000341F8 File Offset: 0x000331F8
		internal static bool IsComponentCreated(IBindableComponent component)
		{
			Control control = component as Control;
			return control == null || control.Created;
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x00034217 File Offset: 0x00033217
		internal bool ComponentCreated
		{
			get
			{
				return Binding.IsComponentCreated(this.control);
			}
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x00034224 File Offset: 0x00033224
		private void FormLoaded(object sender, EventArgs e)
		{
			this.CheckBinding();
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0003422C File Offset: 0x0003322C
		internal void SetBindableComponent(IBindableComponent value)
		{
			if (this.control != value)
			{
				IBindableComponent bindableComponent = this.control;
				this.BindTarget(false);
				this.control = value;
				this.BindTarget(true);
				try
				{
					this.CheckBinding();
				}
				catch
				{
					this.BindTarget(false);
					this.control = bindableComponent;
					this.BindTarget(true);
					throw;
				}
				BindingContext.UpdateBinding((this.control != null && Binding.IsComponentCreated(this.control)) ? this.control.BindingContext : null, this);
				Form form = value as Form;
				if (form != null)
				{
					form.Load += this.FormLoaded;
				}
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06001B1C RID: 6940 RVA: 0x000342D8 File Offset: 0x000332D8
		public bool IsBinding
		{
			get
			{
				return this.bound;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06001B1D RID: 6941 RVA: 0x000342E0 File Offset: 0x000332E0
		public BindingManagerBase BindingManagerBase
		{
			get
			{
				return this.bindingManagerBase;
			}
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x000342E8 File Offset: 0x000332E8
		internal void SetListManager(BindingManagerBase bindingManagerBase)
		{
			if (this.bindingManagerBase is CurrencyManager)
			{
				((CurrencyManager)this.bindingManagerBase).MetaDataChanged -= this.binding_MetaDataChanged;
			}
			this.bindingManagerBase = bindingManagerBase;
			if (this.bindingManagerBase is CurrencyManager)
			{
				((CurrencyManager)this.bindingManagerBase).MetaDataChanged += this.binding_MetaDataChanged;
			}
			this.BindToObject.SetBindingManagerBase(bindingManagerBase);
			this.CheckBinding();
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06001B1F RID: 6943 RVA: 0x00034360 File Offset: 0x00033360
		[DefaultValue("")]
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06001B20 RID: 6944 RVA: 0x00034368 File Offset: 0x00033368
		// (remove) Token: 0x06001B21 RID: 6945 RVA: 0x00034381 File Offset: 0x00033381
		public event BindingCompleteEventHandler BindingComplete
		{
			add
			{
				this.onComplete = (BindingCompleteEventHandler)Delegate.Combine(this.onComplete, value);
			}
			remove
			{
				this.onComplete = (BindingCompleteEventHandler)Delegate.Remove(this.onComplete, value);
			}
		}

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06001B22 RID: 6946 RVA: 0x0003439A File Offset: 0x0003339A
		// (remove) Token: 0x06001B23 RID: 6947 RVA: 0x000343B3 File Offset: 0x000333B3
		public event ConvertEventHandler Parse
		{
			add
			{
				this.onParse = (ConvertEventHandler)Delegate.Combine(this.onParse, value);
			}
			remove
			{
				this.onParse = (ConvertEventHandler)Delegate.Remove(this.onParse, value);
			}
		}

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06001B24 RID: 6948 RVA: 0x000343CC File Offset: 0x000333CC
		// (remove) Token: 0x06001B25 RID: 6949 RVA: 0x000343E5 File Offset: 0x000333E5
		public event ConvertEventHandler Format
		{
			add
			{
				this.onFormat = (ConvertEventHandler)Delegate.Combine(this.onFormat, value);
			}
			remove
			{
				this.onFormat = (ConvertEventHandler)Delegate.Remove(this.onFormat, value);
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06001B26 RID: 6950 RVA: 0x000343FE File Offset: 0x000333FE
		// (set) Token: 0x06001B27 RID: 6951 RVA: 0x00034406 File Offset: 0x00033406
		[DefaultValue(false)]
		public bool FormattingEnabled
		{
			get
			{
				return this.formattingEnabled;
			}
			set
			{
				if (this.formattingEnabled != value)
				{
					this.formattingEnabled = value;
					if (this.IsBinding)
					{
						this.PushData();
					}
				}
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001B28 RID: 6952 RVA: 0x00034427 File Offset: 0x00033427
		// (set) Token: 0x06001B29 RID: 6953 RVA: 0x0003442F File Offset: 0x0003342F
		[DefaultValue(null)]
		public IFormatProvider FormatInfo
		{
			get
			{
				return this.formatInfo;
			}
			set
			{
				if (this.formatInfo != value)
				{
					this.formatInfo = value;
					if (this.IsBinding)
					{
						this.PushData();
					}
				}
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x00034450 File Offset: 0x00033450
		// (set) Token: 0x06001B2B RID: 6955 RVA: 0x00034458 File Offset: 0x00033458
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
					if (this.IsBinding)
					{
						this.PushData();
					}
				}
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x00034488 File Offset: 0x00033488
		// (set) Token: 0x06001B2D RID: 6957 RVA: 0x00034490 File Offset: 0x00033490
		public object NullValue
		{
			get
			{
				return this.nullValue;
			}
			set
			{
				if (!object.Equals(this.nullValue, value))
				{
					this.nullValue = value;
					if (this.IsBinding && Formatter.IsNullData(this.bindToObject.GetValue(), this.dsNullValue))
					{
						this.PushData();
					}
				}
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06001B2E RID: 6958 RVA: 0x000344CE File Offset: 0x000334CE
		// (set) Token: 0x06001B2F RID: 6959 RVA: 0x000344D8 File Offset: 0x000334D8
		public object DataSourceNullValue
		{
			get
			{
				return this.dsNullValue;
			}
			set
			{
				if (!object.Equals(this.dsNullValue, value))
				{
					object obj = this.dsNullValue;
					this.dsNullValue = value;
					this.dsNullValueSet = true;
					if (this.IsBinding)
					{
						object value2 = this.bindToObject.GetValue();
						if (Formatter.IsNullData(value2, obj))
						{
							this.WriteValue();
						}
						if (Formatter.IsNullData(value2, value))
						{
							this.ReadValue();
						}
					}
				}
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001B30 RID: 6960 RVA: 0x0003453A File Offset: 0x0003353A
		// (set) Token: 0x06001B31 RID: 6961 RVA: 0x00034542 File Offset: 0x00033542
		[DefaultValue(ControlUpdateMode.OnPropertyChanged)]
		public ControlUpdateMode ControlUpdateMode
		{
			get
			{
				return this.controlUpdateMode;
			}
			set
			{
				if (this.controlUpdateMode != value)
				{
					this.controlUpdateMode = value;
					if (this.IsBinding)
					{
						this.PushData();
					}
				}
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001B32 RID: 6962 RVA: 0x00034563 File Offset: 0x00033563
		// (set) Token: 0x06001B33 RID: 6963 RVA: 0x0003456B File Offset: 0x0003356B
		[DefaultValue(DataSourceUpdateMode.OnValidation)]
		public DataSourceUpdateMode DataSourceUpdateMode
		{
			get
			{
				return this.dataSourceUpdateMode;
			}
			set
			{
				if (this.dataSourceUpdateMode != value)
				{
					this.dataSourceUpdateMode = value;
				}
			}
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x00034580 File Offset: 0x00033580
		private void BindTarget(bool bind)
		{
			if (bind)
			{
				if (this.IsBinding)
				{
					if (this.propInfo != null && this.control != null)
					{
						EventHandler eventHandler = new EventHandler(this.Target_PropertyChanged);
						this.propInfo.AddValueChanged(this.control, eventHandler);
					}
					if (this.validateInfo != null)
					{
						CancelEventHandler cancelEventHandler = new CancelEventHandler(this.Target_Validate);
						this.validateInfo.AddEventHandler(this.control, cancelEventHandler);
						return;
					}
				}
			}
			else
			{
				if (this.propInfo != null && this.control != null)
				{
					EventHandler eventHandler2 = new EventHandler(this.Target_PropertyChanged);
					this.propInfo.RemoveValueChanged(this.control, eventHandler2);
				}
				if (this.validateInfo != null)
				{
					CancelEventHandler cancelEventHandler2 = new CancelEventHandler(this.Target_Validate);
					this.validateInfo.RemoveEventHandler(this.control, cancelEventHandler2);
				}
			}
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x00034648 File Offset: 0x00033648
		private void binding_MetaDataChanged(object sender, EventArgs e)
		{
			this.CheckBinding();
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x00034650 File Offset: 0x00033650
		private void CheckBinding()
		{
			this.bindToObject.CheckBinding();
			if (this.control != null && this.propertyName.Length > 0)
			{
				this.control.DataBindings.CheckDuplicates(this);
				Type type = this.control.GetType();
				string text = this.propertyName + "IsNull";
				PropertyDescriptor propertyDescriptor = null;
				PropertyDescriptor propertyDescriptor2 = null;
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(this.control)[typeof(InheritanceAttribute)];
				PropertyDescriptorCollection propertyDescriptorCollection;
				if (inheritanceAttribute != null && inheritanceAttribute.InheritanceLevel != InheritanceLevel.NotInherited)
				{
					propertyDescriptorCollection = TypeDescriptor.GetProperties(type);
				}
				else
				{
					propertyDescriptorCollection = TypeDescriptor.GetProperties(this.control);
				}
				for (int i = 0; i < propertyDescriptorCollection.Count; i++)
				{
					if (propertyDescriptor == null && string.Equals(propertyDescriptorCollection[i].Name, this.propertyName, StringComparison.OrdinalIgnoreCase))
					{
						propertyDescriptor = propertyDescriptorCollection[i];
						if (propertyDescriptor2 != null)
						{
							break;
						}
					}
					if (propertyDescriptor2 == null && string.Equals(propertyDescriptorCollection[i].Name, text, StringComparison.OrdinalIgnoreCase))
					{
						propertyDescriptor2 = propertyDescriptorCollection[i];
						if (propertyDescriptor != null)
						{
							break;
						}
					}
				}
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("ListBindingBindProperty", new object[] { this.propertyName }), "PropertyName");
				}
				if (propertyDescriptor.IsReadOnly && this.controlUpdateMode != ControlUpdateMode.Never)
				{
					throw new ArgumentException(SR.GetString("ListBindingBindPropertyReadOnly", new object[] { this.propertyName }), "PropertyName");
				}
				this.propInfo = propertyDescriptor;
				Type propertyType = this.propInfo.PropertyType;
				this.propInfoConverter = this.propInfo.Converter;
				if (propertyDescriptor2 != null && propertyDescriptor2.PropertyType == typeof(bool) && !propertyDescriptor2.IsReadOnly)
				{
					this.propIsNullInfo = propertyDescriptor2;
				}
				EventDescriptor eventDescriptor = null;
				string text2 = "Validating";
				EventDescriptorCollection events = TypeDescriptor.GetEvents(this.control);
				for (int j = 0; j < events.Count; j++)
				{
					if (eventDescriptor == null && string.Equals(events[j].Name, text2, StringComparison.OrdinalIgnoreCase))
					{
						eventDescriptor = events[j];
						break;
					}
				}
				this.validateInfo = eventDescriptor;
			}
			else
			{
				this.propInfo = null;
				this.validateInfo = null;
			}
			this.UpdateIsBinding();
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x00034888 File Offset: 0x00033888
		internal bool ControlAtDesignTime()
		{
			IComponent component = this.control;
			if (component == null)
			{
				return false;
			}
			ISite site = component.Site;
			return site != null && site.DesignMode;
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x000348B3 File Offset: 0x000338B3
		private object GetDataSourceNullValue(Type type)
		{
			if (!this.dsNullValueSet)
			{
				return Formatter.GetDefaultDataSourceNullValue(type);
			}
			return this.dsNullValue;
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x000348CC File Offset: 0x000338CC
		private object GetPropValue()
		{
			bool flag = false;
			if (this.propIsNullInfo != null)
			{
				flag = (bool)this.propIsNullInfo.GetValue(this.control);
			}
			object obj;
			if (flag)
			{
				obj = this.DataSourceNullValue;
			}
			else
			{
				obj = this.propInfo.GetValue(this.control);
				if (obj == null)
				{
					obj = this.DataSourceNullValue;
				}
			}
			return obj;
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x00034924 File Offset: 0x00033924
		private BindingCompleteEventArgs CreateBindingCompleteEventArgs(BindingCompleteContext context, Exception ex)
		{
			bool flag = false;
			string text = string.Empty;
			BindingCompleteState bindingCompleteState = BindingCompleteState.Success;
			if (ex != null)
			{
				text = ex.Message;
				bindingCompleteState = BindingCompleteState.Exception;
				flag = true;
			}
			else
			{
				text = this.BindToObject.DataErrorText;
				if (!string.IsNullOrEmpty(text))
				{
					bindingCompleteState = BindingCompleteState.DataError;
				}
			}
			return new BindingCompleteEventArgs(this, bindingCompleteState, context, text, ex, flag);
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x0003496C File Offset: 0x0003396C
		protected virtual void OnBindingComplete(BindingCompleteEventArgs e)
		{
			if (!this.inOnBindingComplete)
			{
				try
				{
					this.inOnBindingComplete = true;
					if (this.onComplete != null)
					{
						this.onComplete(this, e);
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					e.Cancel = true;
				}
				catch
				{
					e.Cancel = true;
				}
				finally
				{
					this.inOnBindingComplete = false;
				}
			}
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x000349F0 File Offset: 0x000339F0
		protected virtual void OnParse(ConvertEventArgs cevent)
		{
			if (this.onParse != null)
			{
				this.onParse(this, cevent);
			}
			if (!this.formattingEnabled && !(cevent.Value is DBNull) && cevent.Value != null && cevent.DesiredType != null && !cevent.DesiredType.IsInstanceOfType(cevent.Value) && cevent.Value is IConvertible)
			{
				cevent.Value = Convert.ChangeType(cevent.Value, cevent.DesiredType, CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x00034A74 File Offset: 0x00033A74
		protected virtual void OnFormat(ConvertEventArgs cevent)
		{
			if (this.onFormat != null)
			{
				this.onFormat(this, cevent);
			}
			if (!this.formattingEnabled && !(cevent.Value is DBNull) && cevent.DesiredType != null && !cevent.DesiredType.IsInstanceOfType(cevent.Value) && cevent.Value is IConvertible)
			{
				cevent.Value = Convert.ChangeType(cevent.Value, cevent.DesiredType, CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x00034AF0 File Offset: 0x00033AF0
		private object ParseObject(object value)
		{
			Type bindToType = this.bindToObject.BindToType;
			if (this.formattingEnabled)
			{
				ConvertEventArgs convertEventArgs = new ConvertEventArgs(value, bindToType);
				this.OnParse(convertEventArgs);
				object value2 = convertEventArgs.Value;
				if (!object.Equals(value, value2))
				{
					return value2;
				}
				TypeConverter typeConverter = null;
				if (this.bindToObject.FieldInfo != null)
				{
					typeConverter = this.bindToObject.FieldInfo.Converter;
				}
				return Formatter.ParseObject(value, bindToType, (value == null) ? this.propInfo.PropertyType : value.GetType(), typeConverter, this.propInfoConverter, this.formatInfo, this.nullValue, this.GetDataSourceNullValue(bindToType));
			}
			else
			{
				ConvertEventArgs convertEventArgs2 = new ConvertEventArgs(value, bindToType);
				this.OnParse(convertEventArgs2);
				if (convertEventArgs2.Value != null && (convertEventArgs2.Value.GetType().IsSubclassOf(bindToType) || convertEventArgs2.Value.GetType() == bindToType || convertEventArgs2.Value is DBNull))
				{
					return convertEventArgs2.Value;
				}
				TypeConverter converter = TypeDescriptor.GetConverter((value != null) ? value.GetType() : typeof(object));
				if (converter != null && converter.CanConvertTo(bindToType))
				{
					return converter.ConvertTo(value, bindToType);
				}
				if (value is IConvertible)
				{
					object obj = Convert.ChangeType(value, bindToType, CultureInfo.CurrentCulture);
					if (obj != null && (obj.GetType().IsSubclassOf(bindToType) || obj.GetType() == bindToType))
					{
						return obj;
					}
				}
				return null;
			}
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x00034C48 File Offset: 0x00033C48
		private object FormatObject(object value)
		{
			if (this.ControlAtDesignTime())
			{
				return value;
			}
			Type propertyType = this.propInfo.PropertyType;
			if (this.formattingEnabled)
			{
				ConvertEventArgs convertEventArgs = new ConvertEventArgs(value, propertyType);
				this.OnFormat(convertEventArgs);
				if (convertEventArgs.Value != value)
				{
					return convertEventArgs.Value;
				}
				TypeConverter typeConverter = null;
				if (this.bindToObject.FieldInfo != null)
				{
					typeConverter = this.bindToObject.FieldInfo.Converter;
				}
				return Formatter.FormatObject(value, propertyType, typeConverter, this.propInfoConverter, this.formatString, this.formatInfo, this.nullValue, this.dsNullValue);
			}
			else
			{
				ConvertEventArgs convertEventArgs2 = new ConvertEventArgs(value, propertyType);
				this.OnFormat(convertEventArgs2);
				object obj = convertEventArgs2.Value;
				if (propertyType == typeof(object))
				{
					return value;
				}
				if (obj != null && (obj.GetType().IsSubclassOf(propertyType) || obj.GetType() == propertyType))
				{
					return obj;
				}
				TypeConverter converter = TypeDescriptor.GetConverter((value != null) ? value.GetType() : typeof(object));
				if (converter != null && converter.CanConvertTo(propertyType))
				{
					return converter.ConvertTo(value, propertyType);
				}
				if (value is IConvertible)
				{
					obj = Convert.ChangeType(value, propertyType, CultureInfo.CurrentCulture);
					if (obj != null && (obj.GetType().IsSubclassOf(propertyType) || obj.GetType() == propertyType))
					{
						return obj;
					}
				}
				throw new FormatException(SR.GetString("ListBindingFormatFailed"));
			}
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x00034D9C File Offset: 0x00033D9C
		internal bool PullData()
		{
			return this.PullData(true, false);
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x00034DA6 File Offset: 0x00033DA6
		internal bool PullData(bool reformat)
		{
			return this.PullData(reformat, false);
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x00034DB0 File Offset: 0x00033DB0
		internal bool PullData(bool reformat, bool force)
		{
			if (this.ControlUpdateMode == ControlUpdateMode.Never)
			{
				reformat = false;
			}
			bool flag = false;
			object obj = null;
			Exception ex = null;
			if (!this.IsBinding)
			{
				return false;
			}
			if (!force)
			{
				if (this.propInfo.SupportsChangeEvents && !this.modified)
				{
					return false;
				}
				if (this.DataSourceUpdateMode == DataSourceUpdateMode.Never)
				{
					return false;
				}
			}
			if (this.inPushOrPull && this.formattingEnabled)
			{
				return false;
			}
			this.inPushOrPull = true;
			object propValue = this.GetPropValue();
			try
			{
				obj = this.ParseObject(propValue);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			try
			{
				if (ex != null || (!this.FormattingEnabled && obj == null))
				{
					flag = true;
					obj = this.bindToObject.GetValue();
				}
				if (reformat && (!this.FormattingEnabled || !flag))
				{
					object obj2 = this.FormatObject(obj);
					if (force || !this.FormattingEnabled || !object.Equals(obj2, propValue))
					{
						this.SetPropValue(obj2);
					}
				}
				if (!flag)
				{
					this.bindToObject.SetValue(obj);
				}
			}
			catch (Exception ex3)
			{
				ex = ex3;
				if (!this.FormattingEnabled)
				{
					throw;
				}
			}
			finally
			{
				this.inPushOrPull = false;
			}
			if (this.FormattingEnabled)
			{
				BindingCompleteEventArgs bindingCompleteEventArgs = this.CreateBindingCompleteEventArgs(BindingCompleteContext.DataSourceUpdate, ex);
				this.OnBindingComplete(bindingCompleteEventArgs);
				if (bindingCompleteEventArgs.BindingCompleteState == BindingCompleteState.Success && !bindingCompleteEventArgs.Cancel)
				{
					this.modified = false;
				}
				return bindingCompleteEventArgs.Cancel;
			}
			this.modified = false;
			return false;
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x00034F18 File Offset: 0x00033F18
		internal bool PushData()
		{
			return this.PushData(false);
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x00034F24 File Offset: 0x00033F24
		internal bool PushData(bool force)
		{
			Exception ex = null;
			if (!force && this.ControlUpdateMode == ControlUpdateMode.Never)
			{
				return false;
			}
			if (this.inPushOrPull && this.formattingEnabled)
			{
				return false;
			}
			this.inPushOrPull = true;
			try
			{
				if (this.IsBinding)
				{
					object value = this.bindToObject.GetValue();
					object obj = this.FormatObject(value);
					this.SetPropValue(obj);
					this.modified = false;
				}
				else
				{
					this.SetPropValue(null);
				}
			}
			catch (Exception ex2)
			{
				ex = ex2;
				if (!this.FormattingEnabled)
				{
					throw;
				}
			}
			finally
			{
				this.inPushOrPull = false;
			}
			if (this.FormattingEnabled)
			{
				BindingCompleteEventArgs bindingCompleteEventArgs = this.CreateBindingCompleteEventArgs(BindingCompleteContext.ControlUpdate, ex);
				this.OnBindingComplete(bindingCompleteEventArgs);
				return bindingCompleteEventArgs.Cancel;
			}
			return false;
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x00034FEC File Offset: 0x00033FEC
		public void ReadValue()
		{
			this.PushData(true);
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x00034FF6 File Offset: 0x00033FF6
		public void WriteValue()
		{
			this.PullData(true, true);
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x00035004 File Offset: 0x00034004
		private void SetPropValue(object value)
		{
			if (this.ControlAtDesignTime())
			{
				return;
			}
			this.inSetPropValue = true;
			try
			{
				bool flag = value == null || Formatter.IsNullData(value, this.DataSourceNullValue);
				if (flag)
				{
					if (this.propIsNullInfo != null)
					{
						this.propIsNullInfo.SetValue(this.control, true);
					}
					else if (this.propInfo.PropertyType == typeof(object))
					{
						this.propInfo.SetValue(this.control, this.DataSourceNullValue);
					}
					else
					{
						this.propInfo.SetValue(this.control, null);
					}
				}
				else
				{
					this.propInfo.SetValue(this.control, value);
				}
			}
			finally
			{
				this.inSetPropValue = false;
			}
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x000350C8 File Offset: 0x000340C8
		private bool ShouldSerializeFormatString()
		{
			return this.formatString != null && this.formatString.Length > 0;
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x000350E2 File Offset: 0x000340E2
		private bool ShouldSerializeNullValue()
		{
			return this.nullValue != null;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x000350F0 File Offset: 0x000340F0
		private bool ShouldSerializeDataSourceNullValue()
		{
			return this.dsNullValueSet && this.dsNullValue != Formatter.GetDefaultDataSourceNullValue(null);
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x0003510D File Offset: 0x0003410D
		private void Target_PropertyChanged(object sender, EventArgs e)
		{
			if (this.inSetPropValue)
			{
				return;
			}
			if (this.IsBinding)
			{
				this.modified = true;
				if (this.DataSourceUpdateMode == DataSourceUpdateMode.OnPropertyChanged)
				{
					this.PullData(false);
					this.modified = true;
				}
			}
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x00035140 File Offset: 0x00034140
		private void Target_Validate(object sender, CancelEventArgs e)
		{
			try
			{
				if (this.PullData(true))
				{
					e.Cancel = true;
				}
			}
			catch
			{
				e.Cancel = true;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001B4D RID: 6989 RVA: 0x0003517C File Offset: 0x0003417C
		internal bool IsBindable
		{
			get
			{
				return this.control != null && this.propertyName.Length > 0 && this.bindToObject.DataSource != null && this.bindingManagerBase != null;
			}
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x000351B0 File Offset: 0x000341B0
		internal void UpdateIsBinding()
		{
			bool flag = this.IsBindable && this.ComponentCreated && this.bindingManagerBase.IsBinding;
			if (this.bound != flag)
			{
				this.bound = flag;
				this.BindTarget(flag);
				if (this.bound)
				{
					if (this.controlUpdateMode == ControlUpdateMode.Never)
					{
						this.PullData(false, true);
						return;
					}
					this.PushData();
				}
			}
		}

		// Token: 0x040012F7 RID: 4855
		private IBindableComponent control;

		// Token: 0x040012F8 RID: 4856
		private BindingManagerBase bindingManagerBase;

		// Token: 0x040012F9 RID: 4857
		private BindToObject bindToObject;

		// Token: 0x040012FA RID: 4858
		private string propertyName;

		// Token: 0x040012FB RID: 4859
		private PropertyDescriptor propInfo;

		// Token: 0x040012FC RID: 4860
		private PropertyDescriptor propIsNullInfo;

		// Token: 0x040012FD RID: 4861
		private EventDescriptor validateInfo;

		// Token: 0x040012FE RID: 4862
		private TypeConverter propInfoConverter;

		// Token: 0x040012FF RID: 4863
		private bool formattingEnabled;

		// Token: 0x04001300 RID: 4864
		private bool bound;

		// Token: 0x04001301 RID: 4865
		private bool modified;

		// Token: 0x04001302 RID: 4866
		private bool inSetPropValue;

		// Token: 0x04001303 RID: 4867
		private bool inPushOrPull;

		// Token: 0x04001304 RID: 4868
		private bool inOnBindingComplete;

		// Token: 0x04001305 RID: 4869
		private string formatString;

		// Token: 0x04001306 RID: 4870
		private IFormatProvider formatInfo;

		// Token: 0x04001307 RID: 4871
		private object nullValue;

		// Token: 0x04001308 RID: 4872
		private object dsNullValue;

		// Token: 0x04001309 RID: 4873
		private bool dsNullValueSet;

		// Token: 0x0400130A RID: 4874
		private ConvertEventHandler onParse;

		// Token: 0x0400130B RID: 4875
		private ConvertEventHandler onFormat;

		// Token: 0x0400130C RID: 4876
		private ControlUpdateMode controlUpdateMode;

		// Token: 0x0400130D RID: 4877
		private DataSourceUpdateMode dataSourceUpdateMode;

		// Token: 0x0400130E RID: 4878
		private BindingCompleteEventHandler onComplete;
	}
}

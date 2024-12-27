using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000251 RID: 593
	internal class BindToObject
	{
		// Token: 0x06001F2B RID: 7979 RVA: 0x00041575 File Offset: 0x00040575
		private void PropValueChanged(object sender, EventArgs e)
		{
			if (this.bindingManager != null)
			{
				this.bindingManager.OnCurrentChanged(EventArgs.Empty);
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001F2C RID: 7980 RVA: 0x00041590 File Offset: 0x00040590
		private bool IsDataSourceInitialized
		{
			get
			{
				if (this.dataSourceInitialized)
				{
					return true;
				}
				ISupportInitializeNotification supportInitializeNotification = this.dataSource as ISupportInitializeNotification;
				if (supportInitializeNotification == null || supportInitializeNotification.IsInitialized)
				{
					this.dataSourceInitialized = true;
					return true;
				}
				if (this.waitingOnDataSource)
				{
					return false;
				}
				supportInitializeNotification.Initialized += this.DataSource_Initialized;
				this.waitingOnDataSource = true;
				return false;
			}
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x000415EB File Offset: 0x000405EB
		internal BindToObject(Binding owner, object dataSource, string dataMember)
		{
			this.owner = owner;
			this.dataSource = dataSource;
			this.dataMember = new BindingMemberInfo(dataMember);
			this.CheckBinding();
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x00041620 File Offset: 0x00040620
		private void DataSource_Initialized(object sender, EventArgs e)
		{
			ISupportInitializeNotification supportInitializeNotification = this.dataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null)
			{
				supportInitializeNotification.Initialized -= this.DataSource_Initialized;
			}
			this.waitingOnDataSource = false;
			this.dataSourceInitialized = true;
			this.CheckBinding();
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x00041664 File Offset: 0x00040664
		internal void SetBindingManagerBase(BindingManagerBase lManager)
		{
			if (this.bindingManager == lManager)
			{
				return;
			}
			if (this.bindingManager != null && this.fieldInfo != null && this.bindingManager.IsBinding && !(this.bindingManager is CurrencyManager))
			{
				this.fieldInfo.RemoveValueChanged(this.bindingManager.Current, new EventHandler(this.PropValueChanged));
				this.fieldInfo = null;
			}
			this.bindingManager = lManager;
			this.CheckBinding();
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x000416DB File Offset: 0x000406DB
		internal string DataErrorText
		{
			get
			{
				return this.errorText;
			}
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x000416E4 File Offset: 0x000406E4
		private string GetErrorText(object value)
		{
			IDataErrorInfo dataErrorInfo = value as IDataErrorInfo;
			string text = string.Empty;
			if (dataErrorInfo != null)
			{
				if (this.fieldInfo == null)
				{
					text = dataErrorInfo.Error;
				}
				else
				{
					text = dataErrorInfo[this.fieldInfo.Name];
				}
			}
			return text ?? string.Empty;
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x00041730 File Offset: 0x00040730
		internal object GetValue()
		{
			object obj = this.bindingManager.Current;
			this.errorText = this.GetErrorText(obj);
			if (this.fieldInfo != null)
			{
				obj = this.fieldInfo.GetValue(obj);
			}
			return obj;
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001F33 RID: 7987 RVA: 0x0004176C File Offset: 0x0004076C
		internal Type BindToType
		{
			get
			{
				if (this.dataMember.BindingField.Length == 0)
				{
					Type type = this.bindingManager.BindType;
					if (typeof(Array).IsAssignableFrom(type))
					{
						type = type.GetElementType();
					}
					return type;
				}
				if (this.fieldInfo != null)
				{
					return this.fieldInfo.PropertyType;
				}
				return null;
			}
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x000417C8 File Offset: 0x000407C8
		internal void SetValue(object value)
		{
			object obj = null;
			if (this.fieldInfo != null)
			{
				obj = this.bindingManager.Current;
				if (obj is IEditableObject)
				{
					((IEditableObject)obj).BeginEdit();
				}
				if (!this.fieldInfo.IsReadOnly)
				{
					this.fieldInfo.SetValue(obj, value);
				}
			}
			else
			{
				CurrencyManager currencyManager = this.bindingManager as CurrencyManager;
				if (currencyManager != null)
				{
					currencyManager[currencyManager.Position] = value;
					obj = value;
				}
			}
			this.errorText = this.GetErrorText(obj);
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001F35 RID: 7989 RVA: 0x00041845 File Offset: 0x00040845
		internal BindingMemberInfo BindingMemberInfo
		{
			get
			{
				return this.dataMember;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001F36 RID: 7990 RVA: 0x0004184D File Offset: 0x0004084D
		internal object DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001F37 RID: 7991 RVA: 0x00041855 File Offset: 0x00040855
		internal PropertyDescriptor FieldInfo
		{
			get
			{
				return this.fieldInfo;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x0004185D File Offset: 0x0004085D
		internal BindingManagerBase BindingManagerBase
		{
			get
			{
				return this.bindingManager;
			}
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x00041868 File Offset: 0x00040868
		internal void CheckBinding()
		{
			if (this.owner != null && this.owner.BindableComponent != null && this.owner.ControlAtDesignTime())
			{
				return;
			}
			if (this.owner.BindingManagerBase != null && this.fieldInfo != null && this.owner.BindingManagerBase.IsBinding && !(this.owner.BindingManagerBase is CurrencyManager))
			{
				this.fieldInfo.RemoveValueChanged(this.owner.BindingManagerBase.Current, new EventHandler(this.PropValueChanged));
			}
			if (this.owner != null && this.owner.BindingManagerBase != null && this.owner.BindableComponent != null && this.owner.ComponentCreated && this.IsDataSourceInitialized)
			{
				string bindingField = this.dataMember.BindingField;
				this.fieldInfo = this.owner.BindingManagerBase.GetItemProperties().Find(bindingField, true);
				if (this.owner.BindingManagerBase.DataSource != null && this.fieldInfo == null && bindingField.Length > 0)
				{
					throw new ArgumentException(SR.GetString("ListBindingBindField", new object[] { bindingField }), "dataMember");
				}
				if (this.fieldInfo != null && this.owner.BindingManagerBase.IsBinding && !(this.owner.BindingManagerBase is CurrencyManager))
				{
					this.fieldInfo.AddValueChanged(this.owner.BindingManagerBase.Current, new EventHandler(this.PropValueChanged));
					return;
				}
			}
			else
			{
				this.fieldInfo = null;
			}
		}

		// Token: 0x04001411 RID: 5137
		private PropertyDescriptor fieldInfo;

		// Token: 0x04001412 RID: 5138
		private BindingMemberInfo dataMember;

		// Token: 0x04001413 RID: 5139
		private object dataSource;

		// Token: 0x04001414 RID: 5140
		private BindingManagerBase bindingManager;

		// Token: 0x04001415 RID: 5141
		private Binding owner;

		// Token: 0x04001416 RID: 5142
		private string errorText = string.Empty;

		// Token: 0x04001417 RID: 5143
		private bool dataSourceInitialized;

		// Token: 0x04001418 RID: 5144
		private bool waitingOnDataSource;
	}
}

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020002B1 RID: 689
	[Editor("System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[DefaultEvent("CollectionChanged")]
	[TypeConverter("System.Windows.Forms.Design.ControlBindingsConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class ControlBindingsCollection : BindingsCollection
	{
		// Token: 0x060025CB RID: 9675 RVA: 0x00058B70 File Offset: 0x00057B70
		public ControlBindingsCollection(IBindableComponent control)
		{
			this.control = control;
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x060025CC RID: 9676 RVA: 0x00058B7F File Offset: 0x00057B7F
		public IBindableComponent BindableComponent
		{
			get
			{
				return this.control;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x060025CD RID: 9677 RVA: 0x00058B87 File Offset: 0x00057B87
		public Control Control
		{
			get
			{
				return this.control as Control;
			}
		}

		// Token: 0x170005FA RID: 1530
		public Binding this[string propertyName]
		{
			get
			{
				foreach (object obj in this)
				{
					Binding binding = (Binding)obj;
					if (string.Equals(binding.PropertyName, propertyName, StringComparison.OrdinalIgnoreCase))
					{
						return binding;
					}
				}
				return null;
			}
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x00058BF8 File Offset: 0x00057BF8
		public new void Add(Binding binding)
		{
			base.Add(binding);
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x00058C04 File Offset: 0x00057C04
		public Binding Add(string propertyName, object dataSource, string dataMember)
		{
			return this.Add(propertyName, dataSource, dataMember, false, this.DefaultDataSourceUpdateMode, null, string.Empty, null);
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x00058C28 File Offset: 0x00057C28
		public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled)
		{
			return this.Add(propertyName, dataSource, dataMember, formattingEnabled, this.DefaultDataSourceUpdateMode, null, string.Empty, null);
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00058C50 File Offset: 0x00057C50
		public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode)
		{
			return this.Add(propertyName, dataSource, dataMember, formattingEnabled, updateMode, null, string.Empty, null);
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x00058C74 File Offset: 0x00057C74
		public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode, object nullValue)
		{
			return this.Add(propertyName, dataSource, dataMember, formattingEnabled, updateMode, nullValue, string.Empty, null);
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x00058C98 File Offset: 0x00057C98
		public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode, object nullValue, string formatString)
		{
			return this.Add(propertyName, dataSource, dataMember, formattingEnabled, updateMode, nullValue, formatString, null);
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x00058CB8 File Offset: 0x00057CB8
		public Binding Add(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode updateMode, object nullValue, string formatString, IFormatProvider formatInfo)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			Binding binding = new Binding(propertyName, dataSource, dataMember, formattingEnabled, updateMode, nullValue, formatString, formatInfo);
			this.Add(binding);
			return binding;
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x00058CF0 File Offset: 0x00057CF0
		protected override void AddCore(Binding dataBinding)
		{
			if (dataBinding == null)
			{
				throw new ArgumentNullException("dataBinding");
			}
			if (dataBinding.BindableComponent == this.control)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionAdd1"));
			}
			if (dataBinding.BindableComponent != null)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionAdd2"));
			}
			dataBinding.SetBindableComponent(this.control);
			base.AddCore(dataBinding);
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00058D54 File Offset: 0x00057D54
		internal void CheckDuplicates(Binding binding)
		{
			if (binding.PropertyName.Length == 0)
			{
				return;
			}
			for (int i = 0; i < this.Count; i++)
			{
				if (binding != base[i] && base[i].PropertyName.Length > 0 && string.Compare(binding.PropertyName, base[i].PropertyName, false, CultureInfo.InvariantCulture) == 0)
				{
					throw new ArgumentException(SR.GetString("BindingsCollectionDup"), "binding");
				}
			}
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x00058DD2 File Offset: 0x00057DD2
		public new void Clear()
		{
			base.Clear();
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x00058DDC File Offset: 0x00057DDC
		protected override void ClearCore()
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				Binding binding = base[i];
				binding.SetBindableComponent(null);
			}
			base.ClearCore();
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x060025DA RID: 9690 RVA: 0x00058E11 File Offset: 0x00057E11
		// (set) Token: 0x060025DB RID: 9691 RVA: 0x00058E19 File Offset: 0x00057E19
		public DataSourceUpdateMode DefaultDataSourceUpdateMode
		{
			get
			{
				return this.defaultDataSourceUpdateMode;
			}
			set
			{
				this.defaultDataSourceUpdateMode = value;
			}
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x00058E22 File Offset: 0x00057E22
		public new void Remove(Binding binding)
		{
			base.Remove(binding);
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x00058E2B File Offset: 0x00057E2B
		public new void RemoveAt(int index)
		{
			base.RemoveAt(index);
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x00058E34 File Offset: 0x00057E34
		protected override void RemoveCore(Binding dataBinding)
		{
			if (dataBinding.BindableComponent != this.control)
			{
				throw new ArgumentException(SR.GetString("BindingsCollectionForeign"));
			}
			dataBinding.SetBindableComponent(null);
			base.RemoveCore(dataBinding);
		}

		// Token: 0x04001606 RID: 5638
		internal IBindableComponent control;

		// Token: 0x04001607 RID: 5639
		private DataSourceUpdateMode defaultDataSourceUpdateMode;
	}
}

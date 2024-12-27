using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200041C RID: 1052
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public static class ListSourceHelper
	{
		// Token: 0x060032D2 RID: 13010 RVA: 0x000DD6E8 File Offset: 0x000DC6E8
		public static bool ContainsListCollection(IDataSource dataSource)
		{
			ICollection viewNames = dataSource.GetViewNames();
			return viewNames != null && viewNames.Count > 0;
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x000DD70C File Offset: 0x000DC70C
		public static IList GetList(IDataSource dataSource)
		{
			ICollection viewNames = dataSource.GetViewNames();
			if (viewNames != null && viewNames.Count > 0)
			{
				return new ListSourceHelper.ListSourceList(dataSource);
			}
			return null;
		}

		// Token: 0x0200041D RID: 1053
		internal sealed class ListSourceList : CollectionBase, ITypedList
		{
			// Token: 0x060032D4 RID: 13012 RVA: 0x000DD734 File Offset: 0x000DC734
			public ListSourceList(IDataSource dataSource)
			{
				this._dataSource = dataSource;
				((IList)this).Add(new ListSourceHelper.ListSourceRow(this._dataSource));
			}

			// Token: 0x060032D5 RID: 13013 RVA: 0x000DD755 File Offset: 0x000DC755
			string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
			{
				return string.Empty;
			}

			// Token: 0x060032D6 RID: 13014 RVA: 0x000DD75C File Offset: 0x000DC75C
			PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
			{
				if (this._dataSource != null)
				{
					ICollection viewNames = this._dataSource.GetViewNames();
					if (viewNames != null && viewNames.Count > 0)
					{
						string[] array = new string[viewNames.Count];
						viewNames.CopyTo(array, 0);
						PropertyDescriptor[] array2 = new PropertyDescriptor[viewNames.Count];
						for (int i = 0; i < array.Length; i++)
						{
							array2[i] = new ListSourceHelper.ListSourcePropertyDescriptor(array[i]);
						}
						return new PropertyDescriptorCollection(array2);
					}
				}
				return new PropertyDescriptorCollection(null);
			}

			// Token: 0x040023D2 RID: 9170
			private IDataSource _dataSource;
		}

		// Token: 0x0200041E RID: 1054
		internal class ListSourceRow
		{
			// Token: 0x060032D7 RID: 13015 RVA: 0x000DD7CF File Offset: 0x000DC7CF
			public ListSourceRow(IDataSource dataSource)
			{
				this._dataSource = dataSource;
			}

			// Token: 0x17000B33 RID: 2867
			// (get) Token: 0x060032D8 RID: 13016 RVA: 0x000DD7DE File Offset: 0x000DC7DE
			public IDataSource DataSource
			{
				get
				{
					return this._dataSource;
				}
			}

			// Token: 0x040023D3 RID: 9171
			private IDataSource _dataSource;
		}

		// Token: 0x0200041F RID: 1055
		internal class ListSourcePropertyDescriptor : PropertyDescriptor
		{
			// Token: 0x060032D9 RID: 13017 RVA: 0x000DD7E6 File Offset: 0x000DC7E6
			public ListSourcePropertyDescriptor(string name)
				: base(name, null)
			{
				this._name = name;
			}

			// Token: 0x17000B34 RID: 2868
			// (get) Token: 0x060032DA RID: 13018 RVA: 0x000DD7F7 File Offset: 0x000DC7F7
			public override Type ComponentType
			{
				get
				{
					return typeof(ListSourceHelper.ListSourceRow);
				}
			}

			// Token: 0x17000B35 RID: 2869
			// (get) Token: 0x060032DB RID: 13019 RVA: 0x000DD803 File Offset: 0x000DC803
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000B36 RID: 2870
			// (get) Token: 0x060032DC RID: 13020 RVA: 0x000DD806 File Offset: 0x000DC806
			public override Type PropertyType
			{
				get
				{
					return typeof(IEnumerable);
				}
			}

			// Token: 0x060032DD RID: 13021 RVA: 0x000DD812 File Offset: 0x000DC812
			public override bool CanResetValue(object value)
			{
				return false;
			}

			// Token: 0x060032DE RID: 13022 RVA: 0x000DD818 File Offset: 0x000DC818
			public override object GetValue(object source)
			{
				if (source is ListSourceHelper.ListSourceRow)
				{
					ListSourceHelper.ListSourceRow listSourceRow = (ListSourceHelper.ListSourceRow)source;
					IDataSource dataSource = listSourceRow.DataSource;
					return dataSource.GetView(this._name).ExecuteSelect(DataSourceSelectArguments.Empty);
				}
				return null;
			}

			// Token: 0x060032DF RID: 13023 RVA: 0x000DD853 File Offset: 0x000DC853
			public override void ResetValue(object component)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060032E0 RID: 13024 RVA: 0x000DD85A File Offset: 0x000DC85A
			public override void SetValue(object component, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060032E1 RID: 13025 RVA: 0x000DD861 File Offset: 0x000DC861
			public override bool ShouldSerializeValue(object component)
			{
				return false;
			}

			// Token: 0x040023D4 RID: 9172
			private string _name;
		}
	}
}

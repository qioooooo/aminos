using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003F5 RID: 1013
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class FilterableAttribute : Attribute
	{
		// Token: 0x0600320C RID: 12812 RVA: 0x000DC150 File Offset: 0x000DB150
		public FilterableAttribute(bool filterable)
		{
			this._filterable = filterable;
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x0600320D RID: 12813 RVA: 0x000DC15F File Offset: 0x000DB15F
		public bool Filterable
		{
			get
			{
				return this._filterable;
			}
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x000DC168 File Offset: 0x000DB168
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			FilterableAttribute filterableAttribute = obj as FilterableAttribute;
			return filterableAttribute != null && filterableAttribute.Filterable == this._filterable;
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x000DC195 File Offset: 0x000DB195
		public override int GetHashCode()
		{
			return this._filterable.GetHashCode();
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x000DC1A2 File Offset: 0x000DB1A2
		public override bool IsDefaultAttribute()
		{
			return this.Equals(FilterableAttribute.Default);
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x000DC1AF File Offset: 0x000DB1AF
		public static bool IsObjectFilterable(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return FilterableAttribute.IsTypeFilterable(instance.GetType());
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x000DC1CC File Offset: 0x000DB1CC
		public static bool IsPropertyFilterable(PropertyDescriptor propertyDescriptor)
		{
			FilterableAttribute filterableAttribute = (FilterableAttribute)propertyDescriptor.Attributes[typeof(FilterableAttribute)];
			return filterableAttribute == null || filterableAttribute.Filterable;
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x000DC200 File Offset: 0x000DB200
		public static bool IsTypeFilterable(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			object obj = FilterableAttribute._filterableTypes[type];
			if (obj != null)
			{
				return (bool)obj;
			}
			AttributeCollection attributes = TypeDescriptor.GetAttributes(type);
			FilterableAttribute filterableAttribute = (FilterableAttribute)attributes[typeof(FilterableAttribute)];
			obj = filterableAttribute != null && filterableAttribute.Filterable;
			FilterableAttribute._filterableTypes[type] = obj;
			return (bool)obj;
		}

		// Token: 0x040022F0 RID: 8944
		public static readonly FilterableAttribute Yes = new FilterableAttribute(true);

		// Token: 0x040022F1 RID: 8945
		public static readonly FilterableAttribute No = new FilterableAttribute(false);

		// Token: 0x040022F2 RID: 8946
		public static readonly FilterableAttribute Default = FilterableAttribute.Yes;

		// Token: 0x040022F3 RID: 8947
		private bool _filterable;

		// Token: 0x040022F4 RID: 8948
		private static Hashtable _filterableTypes = Hashtable.Synchronized(new Hashtable());
	}
}

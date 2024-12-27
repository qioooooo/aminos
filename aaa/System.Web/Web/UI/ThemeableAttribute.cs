using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000478 RID: 1144
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ThemeableAttribute : Attribute
	{
		// Token: 0x060035C3 RID: 13763 RVA: 0x000E7FE7 File Offset: 0x000E6FE7
		public ThemeableAttribute(bool themeable)
		{
			this._themeable = themeable;
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x060035C4 RID: 13764 RVA: 0x000E7FF6 File Offset: 0x000E6FF6
		public bool Themeable
		{
			get
			{
				return this._themeable;
			}
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x000E8000 File Offset: 0x000E7000
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ThemeableAttribute themeableAttribute = obj as ThemeableAttribute;
			return themeableAttribute != null && themeableAttribute.Themeable == this._themeable;
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x000E802D File Offset: 0x000E702D
		public override int GetHashCode()
		{
			return this._themeable.GetHashCode();
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x000E803A File Offset: 0x000E703A
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ThemeableAttribute.Default);
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x000E8047 File Offset: 0x000E7047
		public static bool IsObjectThemeable(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			return ThemeableAttribute.IsTypeThemeable(instance.GetType());
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x000E8064 File Offset: 0x000E7064
		public static bool IsTypeThemeable(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			object obj = ThemeableAttribute._themeableTypes[type];
			if (obj != null)
			{
				return (bool)obj;
			}
			ThemeableAttribute themeableAttribute = Attribute.GetCustomAttribute(type, typeof(ThemeableAttribute)) as ThemeableAttribute;
			obj = themeableAttribute != null && themeableAttribute.Themeable;
			ThemeableAttribute._themeableTypes[type] = obj;
			return (bool)obj;
		}

		// Token: 0x04002555 RID: 9557
		public static readonly ThemeableAttribute Yes = new ThemeableAttribute(true);

		// Token: 0x04002556 RID: 9558
		public static readonly ThemeableAttribute No = new ThemeableAttribute(false);

		// Token: 0x04002557 RID: 9559
		public static readonly ThemeableAttribute Default = ThemeableAttribute.Yes;

		// Token: 0x04002558 RID: 9560
		private bool _themeable;

		// Token: 0x04002559 RID: 9561
		private static Hashtable _themeableTypes = Hashtable.Synchronized(new Hashtable());
	}
}

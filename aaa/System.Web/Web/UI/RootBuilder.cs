using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000425 RID: 1061
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class RootBuilder : TemplateBuilder
	{
		// Token: 0x06003300 RID: 13056 RVA: 0x000DDBE1 File Offset: 0x000DCBE1
		public RootBuilder()
		{
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x000DDBE9 File Offset: 0x000DCBE9
		public RootBuilder(TemplateParser parser)
		{
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06003302 RID: 13058 RVA: 0x000DDBF1 File Offset: 0x000DCBF1
		public IDictionary BuiltObjects
		{
			get
			{
				if (this._builtObjects == null)
				{
					this._builtObjects = new Hashtable(RootBuilder.ReferenceKeyComparer.Default);
				}
				return this._builtObjects;
			}
		}

		// Token: 0x06003303 RID: 13059 RVA: 0x000DDC11 File Offset: 0x000DCC11
		internal void SetTypeMapper(MainTagNameToTypeMapper typeMapper)
		{
			this._typeMapper = typeMapper;
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x000DDC1C File Offset: 0x000DCC1C
		public override Type GetChildControlType(string tagName, IDictionary attribs)
		{
			return this._typeMapper.GetControlType(tagName, attribs, true);
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x000DDC39 File Offset: 0x000DCC39
		internal override void PrepareNoCompilePageSupport()
		{
			base.PrepareNoCompilePageSupport();
			this._typeMapper = null;
		}

		// Token: 0x040023DF RID: 9183
		private MainTagNameToTypeMapper _typeMapper;

		// Token: 0x040023E0 RID: 9184
		private IDictionary _builtObjects;

		// Token: 0x02000426 RID: 1062
		private class ReferenceKeyComparer : IComparer, IEqualityComparer
		{
			// Token: 0x06003306 RID: 13062 RVA: 0x000DDC48 File Offset: 0x000DCC48
			bool IEqualityComparer.Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}

			// Token: 0x06003307 RID: 13063 RVA: 0x000DDC51 File Offset: 0x000DCC51
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x06003308 RID: 13064 RVA: 0x000DDC59 File Offset: 0x000DCC59
			int IComparer.Compare(object x, object y)
			{
				if (object.ReferenceEquals(x, y))
				{
					return 0;
				}
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return 1;
				}
				return 1;
			}

			// Token: 0x040023E1 RID: 9185
			internal static readonly RootBuilder.ReferenceKeyComparer Default = new RootBuilder.ReferenceKeyComparer();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006E8 RID: 1768
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class PersonalizationStateQuery
	{
		// Token: 0x0600569D RID: 22173 RVA: 0x0015D880 File Offset: 0x0015C880
		static PersonalizationStateQuery()
		{
			PersonalizationStateQuery._knownPropertyTypeMappings["PathToMatch"] = typeof(string);
			PersonalizationStateQuery._knownPropertyTypeMappings["UserInactiveSinceDate"] = typeof(DateTime);
			PersonalizationStateQuery._knownPropertyTypeMappings["UsernameToMatch"] = typeof(string);
		}

		// Token: 0x0600569E RID: 22174 RVA: 0x0015D8E7 File Offset: 0x0015C8E7
		public PersonalizationStateQuery()
		{
			this._data = new HybridDictionary(true);
			this._data["UserInactiveSinceDate"] = PersonalizationAdministration.DefaultInactiveSinceDate;
		}

		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x0600569F RID: 22175 RVA: 0x0015D915 File Offset: 0x0015C915
		// (set) Token: 0x060056A0 RID: 22176 RVA: 0x0015D927 File Offset: 0x0015C927
		public string PathToMatch
		{
			get
			{
				return (string)this["PathToMatch"];
			}
			set
			{
				if (value != null)
				{
					value = value.Trim();
				}
				this._data["PathToMatch"] = value;
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x060056A1 RID: 22177 RVA: 0x0015D948 File Offset: 0x0015C948
		// (set) Token: 0x060056A2 RID: 22178 RVA: 0x0015D967 File Offset: 0x0015C967
		public DateTime UserInactiveSinceDate
		{
			get
			{
				object obj = this["UserInactiveSinceDate"];
				return (DateTime)obj;
			}
			set
			{
				this._data["UserInactiveSinceDate"] = value;
			}
		}

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x060056A3 RID: 22179 RVA: 0x0015D97F File Offset: 0x0015C97F
		// (set) Token: 0x060056A4 RID: 22180 RVA: 0x0015D991 File Offset: 0x0015C991
		public string UsernameToMatch
		{
			get
			{
				return (string)this["UsernameToMatch"];
			}
			set
			{
				if (value != null)
				{
					value = value.Trim();
				}
				this._data["UsernameToMatch"] = value;
			}
		}

		// Token: 0x1700165D RID: 5725
		public object this[string queryKey]
		{
			get
			{
				queryKey = StringUtil.CheckAndTrimString(queryKey, "queryKey");
				return this._data[queryKey];
			}
			set
			{
				queryKey = StringUtil.CheckAndTrimString(queryKey, "queryKey");
				if (PersonalizationStateQuery._knownPropertyTypeMappings.ContainsKey(queryKey))
				{
					Type type = PersonalizationStateQuery._knownPropertyTypeMappings[queryKey];
					if ((value == null && type.IsValueType) || (value != null && !type.IsAssignableFrom(value.GetType())))
					{
						throw new ArgumentException(SR.GetString("PersonalizationStateQuery_IncorrectValueType", new object[] { queryKey, type.FullName }));
					}
				}
				this._data[queryKey] = value;
			}
		}

		// Token: 0x04002F6A RID: 12138
		private static readonly Dictionary<string, Type> _knownPropertyTypeMappings = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04002F6B RID: 12139
		private HybridDictionary _data;
	}
}

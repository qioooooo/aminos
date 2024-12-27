using System;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x020003D0 RID: 976
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DataBinding
	{
		// Token: 0x06002FA6 RID: 12198 RVA: 0x000D4139 File Offset: 0x000D3139
		public DataBinding(string propertyName, Type propertyType, string expression)
		{
			this.propertyName = propertyName;
			this.propertyType = propertyType;
			this.expression = expression;
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06002FA7 RID: 12199 RVA: 0x000D4156 File Offset: 0x000D3156
		// (set) Token: 0x06002FA8 RID: 12200 RVA: 0x000D415E File Offset: 0x000D315E
		public string Expression
		{
			get
			{
				return this.expression;
			}
			set
			{
				this.expression = value;
			}
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06002FA9 RID: 12201 RVA: 0x000D4167 File Offset: 0x000D3167
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06002FAA RID: 12202 RVA: 0x000D416F File Offset: 0x000D316F
		public Type PropertyType
		{
			get
			{
				return this.propertyType;
			}
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000D4177 File Offset: 0x000D3177
		public override int GetHashCode()
		{
			return this.propertyName.ToLower(CultureInfo.InvariantCulture).GetHashCode();
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x000D4190 File Offset: 0x000D3190
		public override bool Equals(object obj)
		{
			if (obj != null && obj is DataBinding)
			{
				DataBinding dataBinding = (DataBinding)obj;
				return StringUtil.EqualsIgnoreCase(this.propertyName, dataBinding.PropertyName);
			}
			return false;
		}

		// Token: 0x040021EC RID: 8684
		private string propertyName;

		// Token: 0x040021ED RID: 8685
		private Type propertyType;

		// Token: 0x040021EE RID: 8686
		private string expression;
	}
}

using System;
using System.Reflection;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006DA RID: 1754
	internal sealed class PersonalizablePropertyEntry
	{
		// Token: 0x0600561A RID: 22042 RVA: 0x0015BF0D File Offset: 0x0015AF0D
		public PersonalizablePropertyEntry(PropertyInfo pi, PersonalizableAttribute attr)
		{
			this._propertyInfo = pi;
			this._scope = attr.Scope;
			this._isSensitive = attr.IsSensitive;
		}

		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x0600561B RID: 22043 RVA: 0x0015BF34 File Offset: 0x0015AF34
		public bool IsSensitive
		{
			get
			{
				return this._isSensitive;
			}
		}

		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x0600561C RID: 22044 RVA: 0x0015BF3C File Offset: 0x0015AF3C
		public PersonalizationScope Scope
		{
			get
			{
				return this._scope;
			}
		}

		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x0600561D RID: 22045 RVA: 0x0015BF44 File Offset: 0x0015AF44
		public PropertyInfo PropertyInfo
		{
			get
			{
				return this._propertyInfo;
			}
		}

		// Token: 0x04002F49 RID: 12105
		private PropertyInfo _propertyInfo;

		// Token: 0x04002F4A RID: 12106
		private PersonalizationScope _scope;

		// Token: 0x04002F4B RID: 12107
		private bool _isSensitive;
	}
}

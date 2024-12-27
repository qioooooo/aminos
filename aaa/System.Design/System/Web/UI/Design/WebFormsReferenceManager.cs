using System;
using System.Collections;

namespace System.Web.UI.Design
{
	// Token: 0x020003B2 RID: 946
	public abstract class WebFormsReferenceManager
	{
		// Token: 0x06002303 RID: 8963
		public abstract Type GetType(string tagPrefix, string tagName);

		// Token: 0x06002304 RID: 8964
		public abstract string GetTagPrefix(Type objectType);

		// Token: 0x06002305 RID: 8965
		public abstract string RegisterTagPrefix(Type objectType);

		// Token: 0x06002306 RID: 8966
		public abstract ICollection GetRegisterDirectives();

		// Token: 0x06002307 RID: 8967
		public abstract string GetUserControlPath(string tagPrefix, string tagName);
	}
}

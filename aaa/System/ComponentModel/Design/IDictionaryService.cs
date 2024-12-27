using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000182 RID: 386
	public interface IDictionaryService
	{
		// Token: 0x06000C67 RID: 3175
		object GetKey(object value);

		// Token: 0x06000C68 RID: 3176
		object GetValue(object key);

		// Token: 0x06000C69 RID: 3177
		void SetValue(object key, object value);
	}
}

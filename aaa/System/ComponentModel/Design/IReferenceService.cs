using System;

namespace System.ComponentModel.Design
{
	// Token: 0x0200018B RID: 395
	public interface IReferenceService
	{
		// Token: 0x06000C8E RID: 3214
		IComponent GetComponent(object reference);

		// Token: 0x06000C8F RID: 3215
		object GetReference(string name);

		// Token: 0x06000C90 RID: 3216
		string GetName(object reference);

		// Token: 0x06000C91 RID: 3217
		object[] GetReferences();

		// Token: 0x06000C92 RID: 3218
		object[] GetReferences(Type baseType);
	}
}

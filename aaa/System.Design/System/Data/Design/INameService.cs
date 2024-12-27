using System;

namespace System.Data.Design
{
	// Token: 0x020000AA RID: 170
	internal interface INameService
	{
		// Token: 0x060007EE RID: 2030
		string CreateUniqueName(INamedObjectCollection container, Type type);

		// Token: 0x060007EF RID: 2031
		string CreateUniqueName(INamedObjectCollection container, string proposed);

		// Token: 0x060007F0 RID: 2032
		string CreateUniqueName(INamedObjectCollection container, string proposedNameRoot, int startSuffix);

		// Token: 0x060007F1 RID: 2033
		void ValidateName(string name);

		// Token: 0x060007F2 RID: 2034
		void ValidateUniqueName(INamedObjectCollection container, string name);

		// Token: 0x060007F3 RID: 2035
		void ValidateUniqueName(INamedObjectCollection container, INamedObject namedObject, string proposedName);
	}
}

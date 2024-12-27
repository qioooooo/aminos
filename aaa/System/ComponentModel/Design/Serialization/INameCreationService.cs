using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001AD RID: 429
	public interface INameCreationService
	{
		// Token: 0x06000D2C RID: 3372
		string CreateName(IContainer container, Type dataType);

		// Token: 0x06000D2D RID: 3373
		bool IsValidName(string name);

		// Token: 0x06000D2E RID: 3374
		void ValidateName(string name);
	}
}

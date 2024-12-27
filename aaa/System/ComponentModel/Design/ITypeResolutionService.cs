using System;
using System.Reflection;

namespace System.ComponentModel.Design
{
	// Token: 0x02000192 RID: 402
	public interface ITypeResolutionService
	{
		// Token: 0x06000CA7 RID: 3239
		Assembly GetAssembly(AssemblyName name);

		// Token: 0x06000CA8 RID: 3240
		Assembly GetAssembly(AssemblyName name, bool throwOnError);

		// Token: 0x06000CA9 RID: 3241
		Type GetType(string name);

		// Token: 0x06000CAA RID: 3242
		Type GetType(string name, bool throwOnError);

		// Token: 0x06000CAB RID: 3243
		Type GetType(string name, bool throwOnError, bool ignoreCase);

		// Token: 0x06000CAC RID: 3244
		void ReferenceAssembly(AssemblyName name);

		// Token: 0x06000CAD RID: 3245
		string GetPathOfAssembly(AssemblyName name);
	}
}

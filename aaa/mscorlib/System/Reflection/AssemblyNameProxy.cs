using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002DC RID: 732
	[ComVisible(true)]
	public class AssemblyNameProxy : MarshalByRefObject
	{
		// Token: 0x06001CFF RID: 7423 RVA: 0x0004A1B0 File Offset: 0x000491B0
		public AssemblyName GetAssemblyName(string assemblyFile)
		{
			return AssemblyName.nGetFileInformation(assemblyFile);
		}
	}
}

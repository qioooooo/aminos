using System;
using Microsoft.Vsa;

namespace Microsoft.JScript.Vsa
{
	// Token: 0x0200013C RID: 316
	internal class DefaultVsaSite : BaseVsaSite
	{
		// Token: 0x06000E72 RID: 3698 RVA: 0x000622D7 File Offset: 0x000612D7
		public override bool OnCompilerError(IVsaError error)
		{
			throw (JScriptException)error;
		}
	}
}

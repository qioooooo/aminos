using System;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000091 RID: 145
	public interface INeedEngine
	{
		// Token: 0x0600069D RID: 1693
		VsaEngine GetEngine();

		// Token: 0x0600069E RID: 1694
		void SetEngine(VsaEngine engine);
	}
}

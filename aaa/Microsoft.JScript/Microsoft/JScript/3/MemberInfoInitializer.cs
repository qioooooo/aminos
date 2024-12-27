using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x0200003F RID: 63
	[ComVisible(true)]
	[Guid("98A3BF0A-1B56-4f32-ACE0-594FEB27EC48")]
	public interface MemberInfoInitializer
	{
		// Token: 0x06000299 RID: 665
		void Initialize(string name, COMMemberInfo dispatch);

		// Token: 0x0600029A RID: 666
		COMMemberInfo GetCOMMemberInfo();
	}
}

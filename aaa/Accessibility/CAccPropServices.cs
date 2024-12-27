using System;
using System.Runtime.InteropServices;

namespace Accessibility
{
	// Token: 0x0200000B RID: 11
	[Guid("6E26E776-04F0-495D-80E4-3330352E3169")]
	[CoClass(typeof(CAccPropServicesClass))]
	[ComImport]
	public interface CAccPropServices : IAccPropServices
	{
	}
}

using System;

namespace Microsoft.VisualBasic
{
	[Flags]
	public enum FileAttribute
	{
		Normal = 0,
		ReadOnly = 1,
		Hidden = 2,
		System = 4,
		Volume = 8,
		Directory = 16,
		Archive = 32
	}
}

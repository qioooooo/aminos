using System;

namespace Aladdin.HASP
{
	[Serializable]
	public enum HaspFileId
	{
		None,
		Main = 65520,
		License = 65522,
		ReadOnly = 65525,
		ReadWrite = 65524,
		Custom = 123456
	}
}

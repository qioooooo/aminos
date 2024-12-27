using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002CB RID: 715
	internal interface INameScope
	{
		// Token: 0x17000827 RID: 2087
		object this[string name, string ns] { get; set; }
	}
}

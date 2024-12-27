using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002BD RID: 701
	[ComVisible(true)]
	public interface ISymbolVariable
	{
		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001B8B RID: 7051
		string Name { get; }

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001B8C RID: 7052
		object Attributes { get; }

		// Token: 0x06001B8D RID: 7053
		byte[] GetSignature();

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001B8E RID: 7054
		SymAddressKind AddressKind { get; }

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001B8F RID: 7055
		int AddressField1 { get; }

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001B90 RID: 7056
		int AddressField2 { get; }

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001B91 RID: 7057
		int AddressField3 { get; }

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001B92 RID: 7058
		int StartOffset { get; }

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001B93 RID: 7059
		int EndOffset { get; }
	}
}

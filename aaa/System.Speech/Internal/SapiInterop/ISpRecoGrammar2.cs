using System;
using System.Runtime.InteropServices;

namespace System.Speech.Internal.SapiInterop
{
	// Token: 0x0200007C RID: 124
	[Guid("4B37BC9E-9ED6-44a3-93D3-18F022B79EC3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISpRecoGrammar2
	{
		// Token: 0x0600021C RID: 540
		void GetRules(out IntPtr ppCoMemRules, out uint puNumRules);

		// Token: 0x0600021D RID: 541
		void LoadCmdFromFile2([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, SPLOADOPTIONS Options, [MarshalAs(UnmanagedType.LPWStr)] string pszSharingUri, [MarshalAs(UnmanagedType.LPWStr)] string pszBaseUri);

		// Token: 0x0600021E RID: 542
		void LoadCmdFromMemory2(IntPtr pGrammar, SPLOADOPTIONS Options, [MarshalAs(UnmanagedType.LPWStr)] string pszSharingUri, [MarshalAs(UnmanagedType.LPWStr)] string pszBaseUri);

		// Token: 0x0600021F RID: 543
		void SetRulePriority([MarshalAs(UnmanagedType.LPWStr)] string pszRuleName, uint ulRuleId, int nRulePriority);

		// Token: 0x06000220 RID: 544
		void SetRuleWeight([MarshalAs(UnmanagedType.LPWStr)] string pszRuleName, uint ulRuleId, float flWeight);

		// Token: 0x06000221 RID: 545
		void SetDictationWeight(float flWeight);

		// Token: 0x06000222 RID: 546
		void SetGrammarLoader(ISpGrammarResourceLoader pLoader);

		// Token: 0x06000223 RID: 547
		void Slot2();
	}
}

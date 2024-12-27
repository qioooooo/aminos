using System;
using System.Text;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000085 RID: 133
	internal interface ICanonicalizableNode
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600024B RID: 587
		// (set) Token: 0x0600024C RID: 588
		bool IsInNodeSet { get; set; }

		// Token: 0x0600024D RID: 589
		void Write(StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc);

		// Token: 0x0600024E RID: 590
		void WriteHash(HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc);
	}
}

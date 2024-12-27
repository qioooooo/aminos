using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000186 RID: 390
	public interface IHelpService
	{
		// Token: 0x06000C75 RID: 3189
		void AddContextAttribute(string name, string value, HelpKeywordType keywordType);

		// Token: 0x06000C76 RID: 3190
		void ClearContextAttributes();

		// Token: 0x06000C77 RID: 3191
		IHelpService CreateLocalContext(HelpContextType contextType);

		// Token: 0x06000C78 RID: 3192
		void RemoveContextAttribute(string name, string value);

		// Token: 0x06000C79 RID: 3193
		void RemoveLocalContext(IHelpService localContext);

		// Token: 0x06000C7A RID: 3194
		void ShowHelpFromKeyword(string helpKeyword);

		// Token: 0x06000C7B RID: 3195
		void ShowHelpFromUrl(string helpUrl);
	}
}

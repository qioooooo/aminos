using System;

namespace System.Globalization
{
	// Token: 0x02000394 RID: 916
	internal class TokenHashValue
	{
		// Token: 0x06002547 RID: 9543 RVA: 0x0006845B File Offset: 0x0006745B
		internal TokenHashValue(string tokenString, TokenType tokenType, int tokenValue)
		{
			this.tokenString = tokenString;
			this.tokenType = tokenType;
			this.tokenValue = tokenValue;
		}

		// Token: 0x0400107E RID: 4222
		internal string tokenString;

		// Token: 0x0400107F RID: 4223
		internal TokenType tokenType;

		// Token: 0x04001080 RID: 4224
		internal int tokenValue;
	}
}

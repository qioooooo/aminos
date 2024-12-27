using System;
using System.IO;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000B8 RID: 184
	public interface IRelDecryptor
	{
		// Token: 0x0600043D RID: 1085
		Stream Decrypt(EncryptionMethod encryptionMethod, KeyInfo keyInfo, Stream toDecrypt);
	}
}

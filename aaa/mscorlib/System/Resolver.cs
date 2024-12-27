using System;
using System.Reflection;
using System.Threading;

namespace System
{
	// Token: 0x0200010E RID: 270
	internal abstract class Resolver
	{
		// Token: 0x06000FC8 RID: 4040
		internal abstract void GetJitContext(ref int securityControlFlags, ref RuntimeTypeHandle typeOwner);

		// Token: 0x06000FC9 RID: 4041
		internal abstract byte[] GetCodeInfo(ref int stackSize, ref int initLocals, ref int EHCount);

		// Token: 0x06000FCA RID: 4042
		internal abstract byte[] GetLocalsSignature();

		// Token: 0x06000FCB RID: 4043
		internal unsafe abstract void GetEHInfo(int EHNumber, void* exception);

		// Token: 0x06000FCC RID: 4044
		internal abstract byte[] GetRawEHInfo();

		// Token: 0x06000FCD RID: 4045
		internal abstract string GetStringLiteral(int token);

		// Token: 0x06000FCE RID: 4046
		internal unsafe abstract void* ResolveToken(int token);

		// Token: 0x06000FCF RID: 4047
		internal abstract int ParentToken(int token);

		// Token: 0x06000FD0 RID: 4048
		internal abstract byte[] ResolveSignature(int token, int fromMethod);

		// Token: 0x06000FD1 RID: 4049
		internal abstract int IsValidToken(int token);

		// Token: 0x06000FD2 RID: 4050
		internal abstract MethodInfo GetDynamicMethod();

		// Token: 0x06000FD3 RID: 4051
		internal abstract CompressedStack GetSecurityContext();

		// Token: 0x04000519 RID: 1305
		internal const int COR_ILEXCEPTION_CLAUSE_CACHED_CLASS = 268435456;

		// Token: 0x0400051A RID: 1306
		internal const int COR_ILEXCEPTION_CLAUSE_MUST_CACHE_CLASS = 536870912;

		// Token: 0x0400051B RID: 1307
		internal const int TypeToken = 1;

		// Token: 0x0400051C RID: 1308
		internal const int MethodToken = 2;

		// Token: 0x0400051D RID: 1309
		internal const int FieldToken = 4;

		// Token: 0x0200010F RID: 271
		internal struct CORINFO_EH_CLAUSE
		{
			// Token: 0x0400051E RID: 1310
			internal int Flags;

			// Token: 0x0400051F RID: 1311
			internal int TryOffset;

			// Token: 0x04000520 RID: 1312
			internal int TryLength;

			// Token: 0x04000521 RID: 1313
			internal int HandlerOffset;

			// Token: 0x04000522 RID: 1314
			internal int HandlerLength;

			// Token: 0x04000523 RID: 1315
			internal int ClassTokenOrFilterOffset;
		}
	}
}

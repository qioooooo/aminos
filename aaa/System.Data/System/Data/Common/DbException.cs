using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Data.Common
{
	// Token: 0x0200013A RID: 314
	[Serializable]
	public abstract class DbException : ExternalException
	{
		// Token: 0x0600149F RID: 5279 RVA: 0x002282A8 File Offset: 0x002276A8
		protected DbException()
		{
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x002282BC File Offset: 0x002276BC
		protected DbException(string message)
			: base(message)
		{
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x002282D0 File Offset: 0x002276D0
		protected DbException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x002282E8 File Offset: 0x002276E8
		protected DbException(string message, int errorCode)
			: base(message, errorCode)
		{
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00228300 File Offset: 0x00227700
		protected DbException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}

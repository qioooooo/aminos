using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO.IsolatedStorage
{
	// Token: 0x0200079B RID: 1947
	[ComVisible(true)]
	[Serializable]
	public class IsolatedStorageException : Exception
	{
		// Token: 0x06004610 RID: 17936 RVA: 0x000F034C File Offset: 0x000EF34C
		public IsolatedStorageException()
			: base(Environment.GetResourceString("IsolatedStorage_Exception"))
		{
			base.SetErrorCode(-2146233264);
		}

		// Token: 0x06004611 RID: 17937 RVA: 0x000F0369 File Offset: 0x000EF369
		public IsolatedStorageException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233264);
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x000F037D File Offset: 0x000EF37D
		public IsolatedStorageException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233264);
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x000F0392 File Offset: 0x000EF392
		protected IsolatedStorageException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}

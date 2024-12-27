using System;
using System.Runtime.InteropServices;
using System.Transactions;

namespace System.EnterpriseServices
{
	// Token: 0x020000C4 RID: 196
	public sealed class BYOT
	{
		// Token: 0x06000486 RID: 1158 RVA: 0x0000DEA1 File Offset: 0x0000CEA1
		private BYOT()
		{
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000DEA9 File Offset: 0x0000CEA9
		private static object GetByotServer()
		{
			return new xByotServer();
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000DEB0 File Offset: 0x0000CEB0
		public static object CreateWithTransaction(object transaction, Type t)
		{
			Guid guid = Marshal.GenerateGuidForType(t);
			Transaction transaction2 = transaction as Transaction;
			ITransaction transaction3;
			if (transaction2 != null)
			{
				object byotServer = BYOT.GetByotServer();
				ICreateWithLocalTransaction createWithLocalTransaction = byotServer as ICreateWithLocalTransaction;
				if (createWithLocalTransaction != null)
				{
					return createWithLocalTransaction.CreateInstanceWithSysTx(new TransactionProxy(transaction2), guid, Util.IID_IUnknown);
				}
				transaction3 = (ITransaction)TransactionInterop.GetDtcTransaction(transaction2);
			}
			else
			{
				transaction3 = (ITransaction)transaction;
			}
			return ((ICreateWithTransactionEx)BYOT.GetByotServer()).CreateInstance(transaction3, guid, Util.IID_IUnknown);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000DF28 File Offset: 0x0000CF28
		public static object CreateWithTipTransaction(string url, Type t)
		{
			Guid guid = Marshal.GenerateGuidForType(t);
			return ((ICreateWithTipTransactionEx)BYOT.GetByotServer()).CreateInstance(url, guid, Util.IID_IUnknown);
		}
	}
}

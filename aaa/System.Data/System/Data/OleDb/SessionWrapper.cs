using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000254 RID: 596
	internal sealed class SessionWrapper : WrappedIUnknown
	{
		// Token: 0x06002097 RID: 8343 RVA: 0x002637B0 File Offset: 0x00262BB0
		internal SessionWrapper()
		{
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x002637C4 File Offset: 0x00262BC4
		internal void QueryInterfaceIDBCreateCommand(OleDbConnectionString constr)
		{
			if (!constr.HaveQueriedForCreateCommand || constr.DangerousIDBCreateCommandCreateCommand != null)
			{
				IntPtr zero = IntPtr.Zero;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					IntPtr intPtr = Marshal.ReadIntPtr(this.handle, 0);
					IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr, 0);
					UnsafeNativeMethods.IUnknownQueryInterface unknownQueryInterface = (UnsafeNativeMethods.IUnknownQueryInterface)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(UnsafeNativeMethods.IUnknownQueryInterface));
					int num = unknownQueryInterface(this.handle, ref ODB.IID_IDBCreateCommand, ref zero);
					if (0 <= num && IntPtr.Zero != zero)
					{
						intPtr = Marshal.ReadIntPtr(zero, 0);
						intPtr2 = Marshal.ReadIntPtr(intPtr, 3 * IntPtr.Size);
						this.DangerousIDBCreateCommandCreateCommand = (UnsafeNativeMethods.IDBCreateCommandCreateCommand)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(UnsafeNativeMethods.IDBCreateCommandCreateCommand));
						constr.DangerousIDBCreateCommandCreateCommand = this.DangerousIDBCreateCommandCreateCommand;
					}
					constr.HaveQueriedForCreateCommand = true;
				}
				finally
				{
					if (IntPtr.Zero != zero)
					{
						IntPtr handle = this.handle;
						this.handle = zero;
						Marshal.Release(handle);
					}
				}
			}
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x002638C8 File Offset: 0x00262CC8
		internal void VerifyIDBCreateCommand(OleDbConnectionString constr)
		{
			IntPtr intPtr = Marshal.ReadIntPtr(this.handle, 0);
			IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr, 3 * IntPtr.Size);
			UnsafeNativeMethods.IDBCreateCommandCreateCommand idbcreateCommandCreateCommand = constr.DangerousIDBCreateCommandCreateCommand;
			if (idbcreateCommandCreateCommand == null || intPtr2 != Marshal.GetFunctionPointerForDelegate(idbcreateCommandCreateCommand))
			{
				idbcreateCommandCreateCommand = (UnsafeNativeMethods.IDBCreateCommandCreateCommand)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(UnsafeNativeMethods.IDBCreateCommandCreateCommand));
				constr.DangerousIDBCreateCommandCreateCommand = idbcreateCommandCreateCommand;
			}
			this.DangerousIDBCreateCommandCreateCommand = idbcreateCommandCreateCommand;
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x0026392C File Offset: 0x00262D2C
		internal OleDbHResult CreateCommand(ref object icommandText)
		{
			OleDbHResult oleDbHResult = OleDbHResult.E_NOINTERFACE;
			UnsafeNativeMethods.IDBCreateCommandCreateCommand dangerousIDBCreateCommandCreateCommand = this.DangerousIDBCreateCommandCreateCommand;
			if (dangerousIDBCreateCommandCreateCommand != null)
			{
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					base.DangerousAddRef(ref flag);
					oleDbHResult = dangerousIDBCreateCommandCreateCommand(this.handle, IntPtr.Zero, ref ODB.IID_ICommandText, ref icommandText);
				}
				finally
				{
					if (flag)
					{
						base.DangerousRelease();
					}
				}
			}
			return oleDbHResult;
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x0026399C File Offset: 0x00262D9C
		internal IDBSchemaRowsetWrapper IDBSchemaRowset(OleDbConnectionInternal connection)
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|session> %d#, IDBSchemaRowset\n", connection.ObjectID);
			return new IDBSchemaRowsetWrapper(base.ComWrapper());
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x002639C4 File Offset: 0x00262DC4
		internal IOpenRowsetWrapper IOpenRowset(OleDbConnectionInternal connection)
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|session> %d#, IOpenRowset\n", connection.ObjectID);
			return new IOpenRowsetWrapper(base.ComWrapper());
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x002639EC File Offset: 0x00262DEC
		internal ITransactionJoinWrapper ITransactionJoin(OleDbConnectionInternal connection)
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|session> %d#, ITransactionJoin\n", connection.ObjectID);
			return new ITransactionJoinWrapper(base.ComWrapper());
		}

		// Token: 0x04001518 RID: 5400
		private UnsafeNativeMethods.IDBCreateCommandCreateCommand DangerousIDBCreateCommandCreateCommand;
	}
}

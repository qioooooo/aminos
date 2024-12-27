using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000253 RID: 595
	internal sealed class DataSourceWrapper : WrappedIUnknown
	{
		// Token: 0x06002093 RID: 8339 RVA: 0x00263550 File Offset: 0x00262950
		internal DataSourceWrapper()
		{
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x00263564 File Offset: 0x00262964
		internal OleDbHResult InitializeAndCreateSession(OleDbConnectionString constr, ref SessionWrapper sessionWrapper)
		{
			bool flag = false;
			IntPtr zero = IntPtr.Zero;
			RuntimeHelpers.PrepareConstrainedRegions();
			OleDbHResult oleDbHResult;
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = Marshal.ReadIntPtr(this.handle, 0);
				IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr, 0);
				UnsafeNativeMethods.IUnknownQueryInterface unknownQueryInterface = constr.DangerousDataSourceIUnknownQueryInterface;
				if (unknownQueryInterface == null || intPtr2 != Marshal.GetFunctionPointerForDelegate(unknownQueryInterface))
				{
					unknownQueryInterface = (UnsafeNativeMethods.IUnknownQueryInterface)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(UnsafeNativeMethods.IUnknownQueryInterface));
					constr.DangerousDataSourceIUnknownQueryInterface = unknownQueryInterface;
				}
				intPtr = Marshal.ReadIntPtr(this.handle, 0);
				intPtr2 = Marshal.ReadIntPtr(intPtr, 3 * IntPtr.Size);
				UnsafeNativeMethods.IDBInitializeInitialize idbinitializeInitialize = constr.DangerousIDBInitializeInitialize;
				if (idbinitializeInitialize == null || intPtr2 != Marshal.GetFunctionPointerForDelegate(idbinitializeInitialize))
				{
					idbinitializeInitialize = (UnsafeNativeMethods.IDBInitializeInitialize)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(UnsafeNativeMethods.IDBInitializeInitialize));
					constr.DangerousIDBInitializeInitialize = idbinitializeInitialize;
				}
				oleDbHResult = idbinitializeInitialize(this.handle);
				if (OleDbHResult.S_OK <= oleDbHResult || OleDbHResult.DB_E_ALREADYINITIALIZED == oleDbHResult)
				{
					oleDbHResult = (OleDbHResult)unknownQueryInterface(this.handle, ref ODB.IID_IDBCreateSession, ref zero);
					if (OleDbHResult.S_OK <= oleDbHResult && IntPtr.Zero != zero)
					{
						intPtr = Marshal.ReadIntPtr(zero, 0);
						intPtr2 = Marshal.ReadIntPtr(intPtr, 3 * IntPtr.Size);
						UnsafeNativeMethods.IDBCreateSessionCreateSession idbcreateSessionCreateSession = constr.DangerousIDBCreateSessionCreateSession;
						if (idbcreateSessionCreateSession == null || intPtr2 != Marshal.GetFunctionPointerForDelegate(idbcreateSessionCreateSession))
						{
							idbcreateSessionCreateSession = (UnsafeNativeMethods.IDBCreateSessionCreateSession)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(UnsafeNativeMethods.IDBCreateSessionCreateSession));
							constr.DangerousIDBCreateSessionCreateSession = idbcreateSessionCreateSession;
						}
						if (constr.DangerousIDBCreateCommandCreateCommand != null)
						{
							oleDbHResult = idbcreateSessionCreateSession(zero, IntPtr.Zero, ref ODB.IID_IDBCreateCommand, ref sessionWrapper);
							if (OleDbHResult.S_OK <= oleDbHResult && !sessionWrapper.IsInvalid)
							{
								sessionWrapper.VerifyIDBCreateCommand(constr);
							}
						}
						else
						{
							oleDbHResult = idbcreateSessionCreateSession(zero, IntPtr.Zero, ref ODB.IID_IUnknown, ref sessionWrapper);
							if (OleDbHResult.S_OK <= oleDbHResult && !sessionWrapper.IsInvalid)
							{
								sessionWrapper.QueryInterfaceIDBCreateCommand(constr);
							}
						}
					}
				}
			}
			finally
			{
				if (IntPtr.Zero != zero)
				{
					Marshal.Release(zero);
				}
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return oleDbHResult;
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00263760 File Offset: 0x00262B60
		internal IDBInfoWrapper IDBInfo(OleDbConnectionInternal connection)
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|datasource> %d#, IDBInfo\n", connection.ObjectID);
			return new IDBInfoWrapper(base.ComWrapper());
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x00263788 File Offset: 0x00262B88
		internal IDBPropertiesWrapper IDBProperties(OleDbConnectionInternal connection)
		{
			Bid.Trace("<oledb.IUnknown.QueryInterface|API|OLEDB|datasource> %d#, IDBProperties\n", connection.ObjectID);
			return new IDBPropertiesWrapper(base.ComWrapper());
		}
	}
}

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Management
{
	// Token: 0x020000B6 RID: 182
	[Serializable]
	internal sealed class IWbemClassObjectFreeThreaded : IDisposable, ISerializable
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x00026660 File Offset: 0x00025660
		public IWbemClassObjectFreeThreaded(IntPtr pWbemClassObject)
		{
			this.pWbemClassObject = pWbemClassObject;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0002667A File Offset: 0x0002567A
		public static implicit operator IntPtr(IWbemClassObjectFreeThreaded wbemClassObject)
		{
			if (wbemClassObject == null)
			{
				return IntPtr.Zero;
			}
			return wbemClassObject.pWbemClassObject;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0002668C File Offset: 0x0002568C
		public IWbemClassObjectFreeThreaded(SerializationInfo info, StreamingContext context)
		{
			byte[] array = info.GetValue("flatWbemClassObject", typeof(byte[])) as byte[];
			if (array == null)
			{
				throw new SerializationException();
			}
			this.DeserializeFromBlob(array);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x000266D5 File Offset: 0x000256D5
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("flatWbemClassObject", this.SerializeToBlob());
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x000266E8 File Offset: 0x000256E8
		public void Dispose()
		{
			this.Dispose_(false);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x000266F1 File Offset: 0x000256F1
		private void Dispose_(bool finalization)
		{
			if (this.pWbemClassObject != IntPtr.Zero)
			{
				Marshal.Release(this.pWbemClassObject);
				this.pWbemClassObject = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00026724 File Offset: 0x00025724
		~IWbemClassObjectFreeThreaded()
		{
			this.Dispose_(true);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00026754 File Offset: 0x00025754
		private void DeserializeFromBlob(byte[] rg)
		{
			IntPtr intPtr = IntPtr.Zero;
			IStream stream = null;
			try
			{
				this.pWbemClassObject = IntPtr.Zero;
				intPtr = Marshal.AllocHGlobal(rg.Length);
				Marshal.Copy(rg, 0, intPtr, rg.Length);
				stream = IWbemClassObjectFreeThreaded.CreateStreamOnHGlobal(intPtr, 0);
				this.pWbemClassObject = IWbemClassObjectFreeThreaded.CoUnmarshalInterface(stream, ref IWbemClassObjectFreeThreaded.IID_IWbemClassObject);
			}
			finally
			{
				if (stream != null)
				{
					Marshal.ReleaseComObject(stream);
				}
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x000267D4 File Offset: 0x000257D4
		private byte[] SerializeToBlob()
		{
			byte[] array = null;
			IStream stream = null;
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				stream = IWbemClassObjectFreeThreaded.CreateStreamOnHGlobal(IntPtr.Zero, 1);
				IWbemClassObjectFreeThreaded.CoMarshalInterface(stream, ref IWbemClassObjectFreeThreaded.IID_IWbemClassObject, this.pWbemClassObject, 2U, IntPtr.Zero, 2U);
				global::System.Runtime.InteropServices.ComTypes.STATSTG statstg;
				stream.Stat(out statstg, 0);
				array = new byte[statstg.cbSize];
				intPtr = IWbemClassObjectFreeThreaded.GlobalLock(IWbemClassObjectFreeThreaded.GetHGlobalFromStream(stream));
				Marshal.Copy(intPtr, array, 0, (int)statstg.cbSize);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					IWbemClassObjectFreeThreaded.GlobalUnlock(intPtr);
				}
				if (stream != null)
				{
					Marshal.ReleaseComObject(stream);
				}
			}
			GC.KeepAlive(this);
			return array;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0002687C File Offset: 0x0002587C
		public int GetQualifierSet_(out IWbemQualifierSetFreeThreaded ppQualSet)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			IntPtr intPtr;
			int num = WmiNetUtilsHelper.GetQualifierSet_f(3, this.pWbemClassObject, out intPtr);
			if (num < 0)
			{
				ppQualSet = null;
			}
			else
			{
				ppQualSet = new IWbemQualifierSetFreeThreaded(intPtr);
			}
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x000268D4 File Offset: 0x000258D4
		public int Get_(string wszName, int lFlags, ref object pVal, ref int pType, ref int plFlavor)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.Get_f(4, this.pWbemClassObject, wszName, lFlags, ref pVal, ref pType, ref plFlavor);
			if (num == -2147217393 && string.Compare(wszName, "__path", StringComparison.OrdinalIgnoreCase) == 0)
			{
				num = 0;
				pType = 8;
				plFlavor = 64;
				pVal = DBNull.Value;
			}
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00026948 File Offset: 0x00025948
		public int Put_(string wszName, int lFlags, ref object pVal, int Type)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.Put_f(5, this.pWbemClassObject, wszName, lFlags, ref pVal, Type);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00026990 File Offset: 0x00025990
		public int Delete_(string wszName)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.Delete_f(6, this.pWbemClassObject, wszName);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x000269D4 File Offset: 0x000259D4
		public int GetNames_(string wszQualifierName, int lFlags, ref object pQualifierVal, out string[] pNames)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.GetNames_f(7, this.pWbemClassObject, wszQualifierName, lFlags, ref pQualifierVal, out pNames);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00026A1C File Offset: 0x00025A1C
		public int BeginEnumeration_(int lEnumFlags)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.BeginEnumeration_f(8, this.pWbemClassObject, lEnumFlags);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00026A60 File Offset: 0x00025A60
		public int Next_(int lFlags, ref string strName, ref object pVal, ref int pType, ref int plFlavor)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			pVal = null;
			strName = null;
			int num = WmiNetUtilsHelper.Next_f(9, this.pWbemClassObject, lFlags, ref strName, ref pVal, ref pType, ref plFlavor);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00026AB4 File Offset: 0x00025AB4
		public int EndEnumeration_()
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.EndEnumeration_f(10, this.pWbemClassObject);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00026AF8 File Offset: 0x00025AF8
		public int GetPropertyQualifierSet_(string wszProperty, out IWbemQualifierSetFreeThreaded ppQualSet)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			IntPtr intPtr;
			int num = WmiNetUtilsHelper.GetPropertyQualifierSet_f(11, this.pWbemClassObject, wszProperty, out intPtr);
			if (num < 0)
			{
				ppQualSet = null;
			}
			else
			{
				ppQualSet = new IWbemQualifierSetFreeThreaded(intPtr);
			}
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00026B50 File Offset: 0x00025B50
		public int Clone_(out IWbemClassObjectFreeThreaded ppCopy)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			IntPtr intPtr;
			int num = WmiNetUtilsHelper.Clone_f(12, this.pWbemClassObject, out intPtr);
			if (num < 0)
			{
				ppCopy = null;
			}
			else
			{
				ppCopy = new IWbemClassObjectFreeThreaded(intPtr);
			}
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00026BA8 File Offset: 0x00025BA8
		public int GetObjectText_(int lFlags, out string pstrObjectText)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.GetObjectText_f(13, this.pWbemClassObject, lFlags, out pstrObjectText);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00026BF0 File Offset: 0x00025BF0
		public int SpawnDerivedClass_(int lFlags, out IWbemClassObjectFreeThreaded ppNewClass)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			IntPtr intPtr;
			int num = WmiNetUtilsHelper.SpawnDerivedClass_f(14, this.pWbemClassObject, lFlags, out intPtr);
			if (num < 0)
			{
				ppNewClass = null;
			}
			else
			{
				ppNewClass = new IWbemClassObjectFreeThreaded(intPtr);
			}
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00026C48 File Offset: 0x00025C48
		public int SpawnInstance_(int lFlags, out IWbemClassObjectFreeThreaded ppNewInstance)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			IntPtr intPtr;
			int num = WmiNetUtilsHelper.SpawnInstance_f(15, this.pWbemClassObject, lFlags, out intPtr);
			if (num < 0)
			{
				ppNewInstance = null;
			}
			else
			{
				ppNewInstance = new IWbemClassObjectFreeThreaded(intPtr);
			}
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00026CA0 File Offset: 0x00025CA0
		public int CompareTo_(int lFlags, IWbemClassObjectFreeThreaded pCompareTo)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.CompareTo_f(16, this.pWbemClassObject, lFlags, pCompareTo.pWbemClassObject);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00026CEC File Offset: 0x00025CEC
		public int GetPropertyOrigin_(string wszName, out string pstrClassName)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.GetPropertyOrigin_f(17, this.pWbemClassObject, wszName, out pstrClassName);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00026D34 File Offset: 0x00025D34
		public int InheritsFrom_(string strAncestor)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.InheritsFrom_f(18, this.pWbemClassObject, strAncestor);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00026D7C File Offset: 0x00025D7C
		public int GetMethod_(string wszName, int lFlags, out IWbemClassObjectFreeThreaded ppInSignature, out IWbemClassObjectFreeThreaded ppOutSignature)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			IntPtr intPtr;
			IntPtr intPtr2;
			int num = WmiNetUtilsHelper.GetMethod_f(19, this.pWbemClassObject, wszName, lFlags, out intPtr, out intPtr2);
			ppInSignature = null;
			ppOutSignature = null;
			if (num >= 0)
			{
				if (intPtr != IntPtr.Zero)
				{
					ppInSignature = new IWbemClassObjectFreeThreaded(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					ppOutSignature = new IWbemClassObjectFreeThreaded(intPtr2);
				}
			}
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00026DFC File Offset: 0x00025DFC
		public int PutMethod_(string wszName, int lFlags, IWbemClassObjectFreeThreaded pInSignature, IWbemClassObjectFreeThreaded pOutSignature)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.PutMethod_f(20, this.pWbemClassObject, wszName, lFlags, pInSignature, pOutSignature);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00026E50 File Offset: 0x00025E50
		public int DeleteMethod_(string wszName)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.DeleteMethod_f(21, this.pWbemClassObject, wszName);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00026E98 File Offset: 0x00025E98
		public int BeginMethodEnumeration_(int lEnumFlags)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.BeginMethodEnumeration_f(22, this.pWbemClassObject, lEnumFlags);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00026EE0 File Offset: 0x00025EE0
		public int NextMethod_(int lFlags, out string pstrName, out IWbemClassObjectFreeThreaded ppInSignature, out IWbemClassObjectFreeThreaded ppOutSignature)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			IntPtr intPtr;
			IntPtr intPtr2;
			int num = WmiNetUtilsHelper.NextMethod_f(23, this.pWbemClassObject, lFlags, out pstrName, out intPtr, out intPtr2);
			ppInSignature = null;
			ppOutSignature = null;
			if (num >= 0)
			{
				if (intPtr != IntPtr.Zero)
				{
					ppInSignature = new IWbemClassObjectFreeThreaded(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					ppOutSignature = new IWbemClassObjectFreeThreaded(intPtr2);
				}
			}
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00026F60 File Offset: 0x00025F60
		public int EndMethodEnumeration_()
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.EndMethodEnumeration_f(24, this.pWbemClassObject);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00026FA4 File Offset: 0x00025FA4
		public int GetMethodQualifierSet_(string wszMethod, out IWbemQualifierSetFreeThreaded ppQualSet)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			IntPtr intPtr;
			int num = WmiNetUtilsHelper.GetMethodQualifierSet_f(25, this.pWbemClassObject, wszMethod, out intPtr);
			if (num < 0)
			{
				ppQualSet = null;
			}
			else
			{
				ppQualSet = new IWbemQualifierSetFreeThreaded(intPtr);
			}
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00026FFC File Offset: 0x00025FFC
		public int GetMethodOrigin_(string wszMethodName, out string pstrClassName)
		{
			if (this.pWbemClassObject == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemClassObjectFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.GetMethodOrigin_f(26, this.pWbemClassObject, wszMethodName, out pstrClassName);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x0600058A RID: 1418
		[DllImport("ole32.dll", PreserveSig = false)]
		private static extern IStream CreateStreamOnHGlobal(IntPtr hGlobal, int fDeleteOnRelease);

		// Token: 0x0600058B RID: 1419
		[DllImport("ole32.dll", PreserveSig = false)]
		private static extern IntPtr GetHGlobalFromStream([In] IStream pstm);

		// Token: 0x0600058C RID: 1420
		[DllImport("kernel32.dll")]
		private static extern IntPtr GlobalLock([In] IntPtr hGlobal);

		// Token: 0x0600058D RID: 1421
		[DllImport("kernel32.dll")]
		private static extern int GlobalUnlock([In] IntPtr pData);

		// Token: 0x0600058E RID: 1422
		[DllImport("ole32.dll", PreserveSig = false)]
		private static extern void CoMarshalInterface([In] IStream pStm, [In] ref Guid riid, [In] IntPtr Unk, [In] uint dwDestContext, [In] IntPtr pvDestContext, [In] uint mshlflags);

		// Token: 0x0600058F RID: 1423
		[DllImport("ole32.dll", PreserveSig = false)]
		private static extern IntPtr CoUnmarshalInterface([In] IStream pStm, [In] ref Guid riid);

		// Token: 0x040002D1 RID: 721
		private const string SerializationBlobName = "flatWbemClassObject";

		// Token: 0x040002D2 RID: 722
		private static readonly string name = typeof(IWbemClassObjectFreeThreaded).FullName;

		// Token: 0x040002D3 RID: 723
		public static Guid IID_IWbemClassObject = new Guid("DC12A681-737F-11CF-884D-00AA004B2E24");

		// Token: 0x040002D4 RID: 724
		private IntPtr pWbemClassObject = IntPtr.Zero;

		// Token: 0x020000B7 RID: 183
		private enum STATFLAG
		{
			// Token: 0x040002D6 RID: 726
			STATFLAG_DEFAULT,
			// Token: 0x040002D7 RID: 727
			STATFLAG_NONAME
		}

		// Token: 0x020000B8 RID: 184
		private enum MSHCTX
		{
			// Token: 0x040002D9 RID: 729
			MSHCTX_LOCAL,
			// Token: 0x040002DA RID: 730
			MSHCTX_NOSHAREDMEM,
			// Token: 0x040002DB RID: 731
			MSHCTX_DIFFERENTMACHINE,
			// Token: 0x040002DC RID: 732
			MSHCTX_INPROC
		}

		// Token: 0x020000B9 RID: 185
		private enum MSHLFLAGS
		{
			// Token: 0x040002DE RID: 734
			MSHLFLAGS_NORMAL,
			// Token: 0x040002DF RID: 735
			MSHLFLAGS_TABLESTRONG,
			// Token: 0x040002E0 RID: 736
			MSHLFLAGS_TABLEWEAK,
			// Token: 0x040002E1 RID: 737
			MSHLFLAGS_NOPING
		}
	}
}

using System;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x0200025E RID: 606
	internal sealed class RowBinding : DbBuffer
	{
		// Token: 0x060020B7 RID: 8375 RVA: 0x002641B4 File Offset: 0x002635B4
		internal static RowBinding CreateBuffer(int bindingCount, int databuffersize, bool needToReset)
		{
			int num = RowBinding.AlignDataSize(bindingCount * ODB.SizeOf_tagDBBINDING);
			int num2 = RowBinding.AlignDataSize(num + databuffersize) + 8;
			return new RowBinding(bindingCount, num, databuffersize, num2, needToReset);
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x002641E4 File Offset: 0x002635E4
		private RowBinding(int bindingCount, int headerLength, int dataLength, int length, bool needToReset)
			: base(length)
		{
			this._bindingCount = bindingCount;
			this._headerLength = headerLength;
			this._dataLength = dataLength;
			this._emptyStringOffset = length - 8;
			this._needToReset = needToReset;
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x00264220 File Offset: 0x00263620
		internal void StartDataBlock()
		{
			if (this._haveData)
			{
				this.ResetValues();
			}
			this._haveData = true;
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00264244 File Offset: 0x00263644
		internal int BindingCount()
		{
			return this._bindingCount;
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00264258 File Offset: 0x00263658
		internal IntPtr DangerousGetAccessorHandle()
		{
			return this._accessorHandle;
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x0026426C File Offset: 0x0026366C
		internal IntPtr DangerousGetDataPtr()
		{
			return ADP.IntPtrOffset(base.DangerousGetHandle(), this._headerLength);
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x0026428C File Offset: 0x0026368C
		internal IntPtr DangerousGetDataPtr(int valueOffset)
		{
			return ADP.IntPtrOffset(base.DangerousGetHandle(), valueOffset);
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x002642A8 File Offset: 0x002636A8
		internal OleDbHResult CreateAccessor(UnsafeNativeMethods.IAccessor iaccessor, int flags, ColumnBinding[] bindings)
		{
			int[] array = new int[this.BindingCount()];
			this._iaccessor = iaccessor;
			Bid.Trace("<oledb.IAccessor.CreateAccessor|API|OLEDB>\n");
			OleDbHResult oleDbHResult = iaccessor.CreateAccessor(flags, (IntPtr)array.Length, this, (IntPtr)this._dataLength, out this._accessorHandle, array);
			Bid.Trace("<oledb.IAccessor.CreateAccessor|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != 0)
				{
					if (4 == flags)
					{
						throw ODB.BadStatus_ParamAcc(bindings[i].ColumnBindingOrdinal, (DBBindStatus)array[i]);
					}
					if (2 == flags)
					{
						throw ODB.BadStatusRowAccessor(bindings[i].ColumnBindingOrdinal, (DBBindStatus)array[i]);
					}
				}
			}
			return oleDbHResult;
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x00264340 File Offset: 0x00263740
		internal ColumnBinding[] SetBindings(OleDbDataReader dataReader, Bindings bindings, int indexStart, int indexForAccessor, OleDbParameter[] parameters, tagDBBINDING[] dbbindings, bool ifIRowsetElseIRow)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = base.DangerousGetHandle();
				for (int i = 0; i < dbbindings.Length; i++)
				{
					IntPtr intPtr2 = ADP.IntPtrOffset(intPtr, i * ODB.SizeOf_tagDBBINDING);
					Marshal.StructureToPtr(dbbindings[i], intPtr2, false);
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			ColumnBinding[] array = new ColumnBinding[dbbindings.Length];
			for (int j = 0; j < array.Length; j++)
			{
				int num = indexStart + j;
				OleDbParameter oleDbParameter = ((parameters != null) ? parameters[num] : null);
				array[j] = new ColumnBinding(dataReader, num, indexForAccessor, j, oleDbParameter, this, bindings, dbbindings[j], this._headerLength, ifIRowsetElseIRow);
			}
			return array;
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x00264400 File Offset: 0x00263800
		internal static int AlignDataSize(int value)
		{
			return Math.Max(8, (value + 7) & -8);
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x0026441C File Offset: 0x0026381C
		internal object GetVariantValue(int offset)
		{
			object obj = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				obj = Marshal.GetObjectForNativeVariant(intPtr);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			if (obj == null)
			{
				return DBNull.Value;
			}
			return obj;
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x00264480 File Offset: 0x00263880
		internal void SetVariantValue(int offset, object value)
		{
			IntPtr intPtr = ADP.PtrZero;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Marshal.GetNativeVariantForObject(value, intPtr);
				}
				finally
				{
					NativeOledbWrapper.MemoryCopy(ADP.IntPtrOffset(intPtr, ODB.SizeOf_Variant), intPtr, ODB.SizeOf_Variant);
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x00264514 File Offset: 0x00263914
		internal void SetBstrValue(int offset, string value)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			IntPtr intPtr;
			try
			{
				base.DangerousAddRef(ref flag);
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					intPtr = SafeNativeMethods.SysAllocStringLen(value, value.Length);
					Marshal.WriteIntPtr(this.handle, offset, intPtr);
					Marshal.WriteIntPtr(this.handle, offset + ADP.PtrSize, intPtr);
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			if (IntPtr.Zero == intPtr)
			{
				throw new OutOfMemoryException();
			}
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x002645B8 File Offset: 0x002639B8
		internal void SetByRefValue(int offset, IntPtr pinnedValue)
		{
			if (ADP.PtrZero == pinnedValue)
			{
				pinnedValue = ADP.IntPtrOffset(this.handle, this._emptyStringOffset);
			}
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					Marshal.WriteIntPtr(this.handle, offset, pinnedValue);
					Marshal.WriteIntPtr(this.handle, offset + ADP.PtrSize, pinnedValue);
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x0026465C File Offset: 0x00263A5C
		internal void CloseFromConnection()
		{
			this._iaccessor = null;
			this._accessorHandle = ODB.DB_INVALID_HACCESSOR;
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x0026467C File Offset: 0x00263A7C
		internal new void Dispose()
		{
			this.ResetValues();
			UnsafeNativeMethods.IAccessor iaccessor = this._iaccessor;
			IntPtr accessorHandle = this._accessorHandle;
			this._iaccessor = null;
			this._accessorHandle = ODB.DB_INVALID_HACCESSOR;
			if (ODB.DB_INVALID_HACCESSOR != accessorHandle && iaccessor != null)
			{
				int num;
				OleDbHResult oleDbHResult = iaccessor.ReleaseAccessor(accessorHandle, out num);
				if (oleDbHResult < OleDbHResult.S_OK)
				{
					SafeNativeMethods.Wrapper.ClearErrorInfo();
				}
			}
			base.Dispose();
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x002646D8 File Offset: 0x00263AD8
		internal void ResetValues()
		{
			if (this._needToReset && this._haveData)
			{
				lock (this)
				{
					bool flag = false;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						base.DangerousAddRef(ref flag);
						this.ResetValues(base.DangerousGetHandle(), this._iaccessor);
					}
					finally
					{
						if (flag)
						{
							base.DangerousRelease();
						}
					}
					return;
				}
			}
			this._haveData = false;
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x00264770 File Offset: 0x00263B70
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private void ResetValues(IntPtr buffer, object iaccessor)
		{
			for (int i = 0; i < this._bindingCount; i++)
			{
				IntPtr intPtr = ADP.IntPtrOffset(buffer, i * ODB.SizeOf_tagDBBINDING);
				int num = this._headerLength + Marshal.ReadIntPtr(intPtr, ODB.OffsetOf_tagDBBINDING_obValue).ToInt32();
				short num2 = Marshal.ReadInt16(intPtr, ODB.OffsetOf_tagDBBINDING_wType);
				short num3 = num2;
				if (num3 <= 12)
				{
					if (num3 != 8)
					{
						if (num3 == 12)
						{
							RowBinding.FreeVariant(buffer, num);
						}
					}
					else
					{
						RowBinding.FreeBstr(buffer, num);
					}
				}
				else
				{
					switch (num3)
					{
					case 136:
						if (iaccessor != null)
						{
							RowBinding.FreeChapter(buffer, num, iaccessor);
						}
						break;
					case 137:
						break;
					case 138:
						RowBinding.FreePropVariant(buffer, num);
						break;
					default:
						switch (num3)
						{
						case 16512:
						case 16514:
							RowBinding.FreeCoTaskMem(buffer, num);
							break;
						}
						break;
					}
				}
			}
			this._haveData = false;
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x00264848 File Offset: 0x00263C48
		private static void FreeChapter(IntPtr buffer, int valueOffset, object iaccessor)
		{
			UnsafeNativeMethods.IChapteredRowset chapteredRowset = iaccessor as UnsafeNativeMethods.IChapteredRowset;
			IntPtr intPtr = SafeNativeMethods.InterlockedExchangePointer(ADP.IntPtrOffset(buffer, valueOffset), ADP.PtrZero);
			if (ODB.DB_NULL_HCHAPTER != intPtr)
			{
				Bid.Trace("<oledb.IChapteredRowset.ReleaseChapter|API|OLEDB> Chapter=%Id\n", intPtr);
				int num;
				OleDbHResult oleDbHResult = chapteredRowset.ReleaseChapter(intPtr, out num);
				Bid.Trace("<oledb.IChapteredRowset.ReleaseChapter|API|OLEDB|RET> %08X{HRESULT}, RefCount=%d\n", oleDbHResult, num);
			}
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x0026489C File Offset: 0x00263C9C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private static void FreeBstr(IntPtr buffer, int valueOffset)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				IntPtr intPtr = Marshal.ReadIntPtr(buffer, valueOffset);
				IntPtr intPtr2 = Marshal.ReadIntPtr(buffer, valueOffset + ADP.PtrSize);
				if (ADP.PtrZero != intPtr && intPtr != intPtr2)
				{
					SafeNativeMethods.SysFreeString(intPtr);
				}
				if (ADP.PtrZero != intPtr2)
				{
					SafeNativeMethods.SysFreeString(intPtr2);
				}
				Marshal.WriteIntPtr(buffer, valueOffset, ADP.PtrZero);
				Marshal.WriteIntPtr(buffer, valueOffset + ADP.PtrSize, ADP.PtrZero);
			}
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00264930 File Offset: 0x00263D30
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private static void FreeCoTaskMem(IntPtr buffer, int valueOffset)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				IntPtr intPtr = Marshal.ReadIntPtr(buffer, valueOffset);
				IntPtr intPtr2 = Marshal.ReadIntPtr(buffer, valueOffset + ADP.PtrSize);
				if (ADP.PtrZero != intPtr && intPtr != intPtr2)
				{
					SafeNativeMethods.CoTaskMemFree(intPtr);
				}
				Marshal.WriteIntPtr(buffer, valueOffset, ADP.PtrZero);
				Marshal.WriteIntPtr(buffer, valueOffset + ADP.PtrSize, ADP.PtrZero);
			}
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x002649B4 File Offset: 0x00263DB4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private static void FreeVariant(IntPtr buffer, int valueOffset)
		{
			IntPtr intPtr = ADP.IntPtrOffset(buffer, valueOffset);
			IntPtr intPtr2 = ADP.IntPtrOffset(buffer, valueOffset + ODB.SizeOf_Variant);
			bool flag = NativeOledbWrapper.MemoryCompare(intPtr, intPtr2, ODB.SizeOf_Variant);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				SafeNativeMethods.VariantClear(intPtr);
				if (flag)
				{
					SafeNativeMethods.VariantClear(intPtr2);
				}
				else
				{
					SafeNativeMethods.ZeroMemory(intPtr2, (IntPtr)ODB.SizeOf_Variant);
				}
			}
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x00264A2C File Offset: 0x00263E2C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private static void FreePropVariant(IntPtr buffer, int valueOffset)
		{
			IntPtr intPtr = ADP.IntPtrOffset(buffer, valueOffset);
			IntPtr intPtr2 = ADP.IntPtrOffset(buffer, valueOffset + NativeOledbWrapper.SizeOfPROPVARIANT);
			bool flag = NativeOledbWrapper.MemoryCompare(intPtr, intPtr2, NativeOledbWrapper.SizeOfPROPVARIANT);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				SafeNativeMethods.PropVariantClear(intPtr);
				if (flag)
				{
					SafeNativeMethods.PropVariantClear(intPtr2);
				}
				else
				{
					SafeNativeMethods.ZeroMemory(intPtr2, (IntPtr)NativeOledbWrapper.SizeOfPROPVARIANT);
				}
			}
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x00264AA4 File Offset: 0x00263EA4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal IntPtr InterlockedExchangePointer(int offset)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			IntPtr intPtr2;
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), offset);
				intPtr2 = SafeNativeMethods.InterlockedExchangePointer(intPtr, IntPtr.Zero);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return intPtr2;
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x00264B04 File Offset: 0x00263F04
		protected override bool ReleaseHandle()
		{
			this._iaccessor = null;
			if (this._needToReset && this._haveData)
			{
				IntPtr handle = this.handle;
				if (IntPtr.Zero != handle)
				{
					this.ResetValues(handle, null);
				}
			}
			return base.ReleaseHandle();
		}

		// Token: 0x04001537 RID: 5431
		private readonly int _bindingCount;

		// Token: 0x04001538 RID: 5432
		private readonly int _headerLength;

		// Token: 0x04001539 RID: 5433
		private readonly int _dataLength;

		// Token: 0x0400153A RID: 5434
		private readonly int _emptyStringOffset;

		// Token: 0x0400153B RID: 5435
		private UnsafeNativeMethods.IAccessor _iaccessor;

		// Token: 0x0400153C RID: 5436
		private IntPtr _accessorHandle;

		// Token: 0x0400153D RID: 5437
		private readonly bool _needToReset;

		// Token: 0x0400153E RID: 5438
		private bool _haveData;
	}
}

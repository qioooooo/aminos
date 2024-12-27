using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x0200020E RID: 526
	internal sealed class DBPropSet : SafeHandle
	{
		// Token: 0x06001D81 RID: 7553 RVA: 0x00251CD4 File Offset: 0x002510D4
		private DBPropSet()
			: base(IntPtr.Zero, true)
		{
			this.propertySetCount = 0;
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x00251CF4 File Offset: 0x002510F4
		internal DBPropSet(int propertysetCount)
			: this()
		{
			this.propertySetCount = propertysetCount;
			IntPtr intPtr = (IntPtr)(propertysetCount * ODB.SizeOf_tagDBPROPSET);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this.handle = SafeNativeMethods.CoTaskMemAlloc(intPtr);
				if (ADP.PtrZero != this.handle)
				{
					SafeNativeMethods.ZeroMemory(this.handle, intPtr);
				}
			}
			if (ADP.PtrZero == this.handle)
			{
				throw new OutOfMemoryException();
			}
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x00251D84 File Offset: 0x00251184
		internal DBPropSet(UnsafeNativeMethods.IDBProperties properties, PropertyIDSet propidset, out OleDbHResult hr)
			: this()
		{
			int num = 0;
			if (propidset != null)
			{
				num = propidset.Count;
			}
			Bid.Trace("<oledb.IDBProperties.GetProperties|API|OLEDB>\n");
			hr = properties.GetProperties(num, propidset, out this.propertySetCount, out this.handle);
			Bid.Trace("<oledb.IDBProperties.GetProperties|API|OLEDB|RET> %08X{HRESULT}\n", hr);
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x00251DD0 File Offset: 0x002511D0
		internal DBPropSet(UnsafeNativeMethods.IRowsetInfo properties, PropertyIDSet propidset, out OleDbHResult hr)
			: this()
		{
			int num = 0;
			if (propidset != null)
			{
				num = propidset.Count;
			}
			Bid.Trace("<oledb.IRowsetInfo.GetProperties|API|OLEDB>\n");
			hr = properties.GetProperties(num, propidset, out this.propertySetCount, out this.handle);
			Bid.Trace("<oledb.IRowsetInfo.GetProperties|API|OLEDB|RET> %08X{HRESULT}\n", hr);
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x00251E1C File Offset: 0x0025121C
		internal DBPropSet(UnsafeNativeMethods.ICommandProperties properties, PropertyIDSet propidset, out OleDbHResult hr)
			: this()
		{
			int num = 0;
			if (propidset != null)
			{
				num = propidset.Count;
			}
			Bid.Trace("<oledb.ICommandProperties.GetProperties|API|OLEDB>\n");
			hr = properties.GetProperties(num, propidset, out this.propertySetCount, out this.handle);
			Bid.Trace("<oledb.ICommandProperties.GetProperties|API|OLEDB|RET> %08X{HRESULT}\n", hr);
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001D86 RID: 7558 RVA: 0x00251E68 File Offset: 0x00251268
		public override bool IsInvalid
		{
			get
			{
				return IntPtr.Zero == this.handle;
			}
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x00251E88 File Offset: 0x00251288
		protected override bool ReleaseHandle()
		{
			IntPtr handle = this.handle;
			this.handle = IntPtr.Zero;
			if (ADP.PtrZero != handle)
			{
				int num = this.propertySetCount;
				int i = 0;
				int num2 = 0;
				while (i < num)
				{
					IntPtr intPtr = Marshal.ReadIntPtr(handle, num2);
					if (ADP.PtrZero != intPtr)
					{
						int num3 = Marshal.ReadInt32(handle, num2 + ADP.PtrSize);
						IntPtr intPtr2 = ADP.IntPtrOffset(intPtr, ODB.OffsetOf_tagDBPROP_Value);
						int j = 0;
						while (j < num3)
						{
							SafeNativeMethods.VariantClear(intPtr2);
							j++;
							intPtr2 = ADP.IntPtrOffset(intPtr2, ODB.SizeOf_tagDBPROP);
						}
						SafeNativeMethods.CoTaskMemFree(intPtr);
					}
					i++;
					num2 += ODB.SizeOf_tagDBPROPSET;
				}
				SafeNativeMethods.CoTaskMemFree(handle);
			}
			return true;
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x00251F3C File Offset: 0x0025133C
		internal int PropertySetCount
		{
			get
			{
				return this.propertySetCount;
			}
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x00251F50 File Offset: 0x00251350
		internal tagDBPROP[] GetPropertySet(int index, out Guid propertyset)
		{
			if (index < 0 || this.PropertySetCount <= index)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidBuffer);
			}
			tagDBPROPSET tagDBPROPSET = new tagDBPROPSET();
			tagDBPROP[] array = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr = ADP.IntPtrOffset(base.DangerousGetHandle(), index * ODB.SizeOf_tagDBPROPSET);
				Marshal.PtrToStructure(intPtr, tagDBPROPSET);
				propertyset = tagDBPROPSET.guidPropertySet;
				array = new tagDBPROP[tagDBPROPSET.cProperties];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new tagDBPROP();
					IntPtr intPtr2 = ADP.IntPtrOffset(tagDBPROPSET.rgProperties, i * ODB.SizeOf_tagDBPROP);
					Marshal.PtrToStructure(intPtr2, array[i]);
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return array;
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x0025201C File Offset: 0x0025141C
		internal void SetPropertySet(int index, Guid propertySet, tagDBPROP[] properties)
		{
			if (index < 0 || this.PropertySetCount <= index)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidBuffer);
			}
			IntPtr intPtr = (IntPtr)(properties.Length * ODB.SizeOf_tagDBPROP);
			tagDBPROPSET tagDBPROPSET = new tagDBPROPSET(properties.Length, propertySet);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr2 = ADP.IntPtrOffset(base.DangerousGetHandle(), index * ODB.SizeOf_tagDBPROPSET);
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					tagDBPROPSET.rgProperties = SafeNativeMethods.CoTaskMemAlloc(intPtr);
					if (ADP.PtrZero != tagDBPROPSET.rgProperties)
					{
						SafeNativeMethods.ZeroMemory(tagDBPROPSET.rgProperties, intPtr);
						Marshal.StructureToPtr(tagDBPROPSET, intPtr2, false);
					}
				}
				if (ADP.PtrZero == tagDBPROPSET.rgProperties)
				{
					throw new OutOfMemoryException();
				}
				for (int i = 0; i < properties.Length; i++)
				{
					IntPtr intPtr3 = ADP.IntPtrOffset(tagDBPROPSET.rgProperties, i * ODB.SizeOf_tagDBPROP);
					Marshal.StructureToPtr(properties[i], intPtr3, false);
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

		// Token: 0x06001D8B RID: 7563 RVA: 0x0025213C File Offset: 0x0025153C
		internal static DBPropSet CreateProperty(Guid propertySet, int propertyId, bool required, object value)
		{
			tagDBPROP tagDBPROP = new tagDBPROP(propertyId, required, value);
			DBPropSet dbpropSet = new DBPropSet(1);
			dbpropSet.SetPropertySet(0, propertySet, new tagDBPROP[] { tagDBPROP });
			return dbpropSet;
		}

		// Token: 0x040010B2 RID: 4274
		private readonly int propertySetCount;
	}
}

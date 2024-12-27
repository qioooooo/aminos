using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x0200025D RID: 605
	internal sealed class PropertyInfoSet : SafeHandle
	{
		// Token: 0x060020B2 RID: 8370 RVA: 0x00263D28 File Offset: 0x00263128
		internal PropertyInfoSet(UnsafeNativeMethods.IDBProperties idbProperties, PropertyIDSet propIDSet)
			: base(IntPtr.Zero, true)
		{
			int count = propIDSet.Count;
			Bid.Trace("<oledb.IDBProperties.GetPropertyInfo|API|OLEDB>\n");
			RuntimeHelpers.PrepareConstrainedRegions();
			OleDbHResult propertyInfo;
			try
			{
			}
			finally
			{
				propertyInfo = idbProperties.GetPropertyInfo(count, propIDSet, out this.setCount, out this.handle, out this.descBuffer);
			}
			Bid.Trace("<oledb.IDBProperties.GetPropertyInfo|API|OLEDB|RET> %08X{HRESULT}\n", propertyInfo);
			if (OleDbHResult.S_OK <= propertyInfo && ADP.PtrZero != this.handle)
			{
				SafeNativeMethods.Wrapper.ClearErrorInfo();
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060020B3 RID: 8371 RVA: 0x00263DB8 File Offset: 0x002631B8
		public override bool IsInvalid
		{
			get
			{
				return IntPtr.Zero == this.handle && IntPtr.Zero == this.descBuffer;
			}
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x00263DEC File Offset: 0x002631EC
		internal Dictionary<string, OleDbPropertyInfo> GetValues()
		{
			Dictionary<string, OleDbPropertyInfo> dictionary = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				if (ADP.PtrZero != this.handle)
				{
					dictionary = new Dictionary<string, OleDbPropertyInfo>(StringComparer.OrdinalIgnoreCase);
					IntPtr intPtr = this.handle;
					tagDBPROPINFO tagDBPROPINFO = new tagDBPROPINFO();
					tagDBPROPINFOSET tagDBPROPINFOSET = new tagDBPROPINFOSET();
					int i = 0;
					while (i < this.setCount)
					{
						Marshal.PtrToStructure(intPtr, tagDBPROPINFOSET);
						int cPropertyInfos = tagDBPROPINFOSET.cPropertyInfos;
						IntPtr intPtr2 = tagDBPROPINFOSET.rgPropertyInfos;
						int j = 0;
						while (j < cPropertyInfos)
						{
							Marshal.PtrToStructure(intPtr2, tagDBPROPINFO);
							OleDbPropertyInfo oleDbPropertyInfo = new OleDbPropertyInfo();
							oleDbPropertyInfo._propertySet = tagDBPROPINFOSET.guidPropertySet;
							oleDbPropertyInfo._propertyID = tagDBPROPINFO.dwPropertyID;
							oleDbPropertyInfo._flags = tagDBPROPINFO.dwFlags;
							oleDbPropertyInfo._vtype = (int)tagDBPROPINFO.vtType;
							oleDbPropertyInfo._supportedValues = tagDBPROPINFO.vValue;
							oleDbPropertyInfo._description = tagDBPROPINFO.pwszDescription;
							oleDbPropertyInfo._lowercase = tagDBPROPINFO.pwszDescription.ToLower(CultureInfo.InvariantCulture);
							oleDbPropertyInfo._type = PropertyInfoSet.FromVtType((int)tagDBPROPINFO.vtType);
							if (Bid.AdvancedOn)
							{
								Bid.Trace("<oledb.struct.OleDbPropertyInfo|INFO|ADV> \n");
							}
							dictionary[oleDbPropertyInfo._lowercase] = oleDbPropertyInfo;
							j++;
							intPtr2 = ADP.IntPtrOffset(intPtr2, ODB.SizeOf_tagDBPROPINFO);
						}
						i++;
						intPtr = ADP.IntPtrOffset(intPtr, ODB.SizeOf_tagDBPROPINFOSET);
					}
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return dictionary;
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x00263F6C File Offset: 0x0026336C
		protected override bool ReleaseHandle()
		{
			IntPtr handle = this.handle;
			this.handle = IntPtr.Zero;
			if (IntPtr.Zero != handle)
			{
				int num = this.setCount;
				for (int i = 0; i < num; i++)
				{
					int num2 = i * ODB.SizeOf_tagDBPROPINFOSET;
					IntPtr intPtr = Marshal.ReadIntPtr(handle, num2);
					if (IntPtr.Zero != intPtr)
					{
						int num3 = Marshal.ReadInt32(handle, num2 + ADP.PtrSize);
						for (int j = 0; j < num3; j++)
						{
							IntPtr intPtr2 = ADP.IntPtrOffset(intPtr, j * ODB.SizeOf_tagDBPROPINFO + ODB.OffsetOf_tagDBPROPINFO_Value);
							SafeNativeMethods.VariantClear(intPtr2);
						}
						SafeNativeMethods.CoTaskMemFree(intPtr);
					}
				}
				SafeNativeMethods.CoTaskMemFree(handle);
			}
			handle = this.descBuffer;
			this.descBuffer = IntPtr.Zero;
			if (IntPtr.Zero != handle)
			{
				SafeNativeMethods.CoTaskMemFree(handle);
			}
			return true;
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x0026403C File Offset: 0x0026343C
		internal static Type FromVtType(int vartype)
		{
			switch (vartype)
			{
			case 0:
				return null;
			case 1:
				return typeof(DBNull);
			case 2:
				return typeof(short);
			case 3:
				return typeof(int);
			case 4:
				return typeof(float);
			case 5:
				return typeof(double);
			case 6:
				return typeof(decimal);
			case 7:
				return typeof(DateTime);
			case 8:
				return typeof(string);
			case 9:
				return typeof(object);
			case 10:
				return typeof(int);
			case 11:
				return typeof(bool);
			case 12:
				return typeof(object);
			case 13:
				return typeof(object);
			case 14:
				return typeof(decimal);
			case 16:
				return typeof(sbyte);
			case 17:
				return typeof(byte);
			case 18:
				return typeof(ushort);
			case 19:
				return typeof(uint);
			case 20:
				return typeof(long);
			case 21:
				return typeof(ulong);
			case 22:
				return typeof(int);
			case 23:
				return typeof(uint);
			}
			return typeof(object);
		}

		// Token: 0x04001535 RID: 5429
		private int setCount;

		// Token: 0x04001536 RID: 5430
		private IntPtr descBuffer;
	}
}

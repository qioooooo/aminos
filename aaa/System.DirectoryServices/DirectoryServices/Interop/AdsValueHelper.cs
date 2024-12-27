using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.DirectoryServices.Interop
{
	// Token: 0x02000059 RID: 89
	internal class AdsValueHelper
	{
		// Token: 0x060001FA RID: 506 RVA: 0x00007FAB File Offset: 0x00006FAB
		public AdsValueHelper(AdsValue adsvalue)
		{
			this.adsvalue = adsvalue;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00007FBC File Offset: 0x00006FBC
		public AdsValueHelper(object managedValue)
		{
			AdsType adsTypeForManagedType = this.GetAdsTypeForManagedType(managedValue.GetType());
			this.SetValue(managedValue, adsTypeForManagedType);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00007FE4 File Offset: 0x00006FE4
		public AdsValueHelper(object managedValue, AdsType adsType)
		{
			this.SetValue(managedValue, adsType);
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00007FF4 File Offset: 0x00006FF4
		// (set) Token: 0x060001FE RID: 510 RVA: 0x0000801C File Offset: 0x0000701C
		public long LowInt64
		{
			get
			{
				return (long)((ulong)this.adsvalue.generic.a + (ulong)((ulong)((long)this.adsvalue.generic.b) << 32));
			}
			set
			{
				this.adsvalue.generic.a = (int)(value & (long)((ulong)(-1)));
				this.adsvalue.generic.b = (int)(value >> 32);
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00008048 File Offset: 0x00007048
		~AdsValueHelper()
		{
			if (this.pinnedHandle.IsAllocated)
			{
				this.pinnedHandle.Free();
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00008088 File Offset: 0x00007088
		private AdsType GetAdsTypeForManagedType(Type type)
		{
			if (type == typeof(int))
			{
				return AdsType.ADSTYPE_INTEGER;
			}
			if (type == typeof(long))
			{
				return AdsType.ADSTYPE_LARGE_INTEGER;
			}
			if (type == typeof(bool))
			{
				return AdsType.ADSTYPE_BOOLEAN;
			}
			return AdsType.ADSTYPE_UNKNOWN;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000080BA File Offset: 0x000070BA
		public AdsValue GetStruct()
		{
			return this.adsvalue;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000080C2 File Offset: 0x000070C2
		private static ushort LowOfInt(int i)
		{
			return (ushort)(i & 65535);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000080CC File Offset: 0x000070CC
		private static ushort HighOfInt(int i)
		{
			return (ushort)((i >> 16) & 65535);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000080DC File Offset: 0x000070DC
		public object GetValue()
		{
			switch (this.adsvalue.dwType)
			{
			case 0:
				throw new InvalidOperationException(Res.GetString("DSConvertTypeInvalid"));
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 12:
				return Marshal.PtrToStringUni(this.adsvalue.pointer.value);
			case 6:
				return this.adsvalue.generic.a != 0;
			case 7:
				return this.adsvalue.generic.a;
			case 8:
			case 11:
			case 25:
			{
				int length = this.adsvalue.octetString.length;
				byte[] array = new byte[length];
				Marshal.Copy(this.adsvalue.octetString.value, array, 0, length);
				return array;
			}
			case 9:
			{
				SystemTime systemTime = default(SystemTime);
				systemTime.wYear = AdsValueHelper.LowOfInt(this.adsvalue.generic.a);
				systemTime.wMonth = AdsValueHelper.HighOfInt(this.adsvalue.generic.a);
				systemTime.wDayOfWeek = AdsValueHelper.LowOfInt(this.adsvalue.generic.b);
				systemTime.wDay = AdsValueHelper.HighOfInt(this.adsvalue.generic.b);
				systemTime.wHour = AdsValueHelper.LowOfInt(this.adsvalue.generic.c);
				systemTime.wMinute = AdsValueHelper.HighOfInt(this.adsvalue.generic.c);
				systemTime.wSecond = AdsValueHelper.LowOfInt(this.adsvalue.generic.d);
				systemTime.wMilliseconds = AdsValueHelper.HighOfInt(this.adsvalue.generic.d);
				return new DateTime((int)systemTime.wYear, (int)systemTime.wMonth, (int)systemTime.wDay, (int)systemTime.wHour, (int)systemTime.wMinute, (int)systemTime.wSecond, (int)systemTime.wMilliseconds);
			}
			case 10:
				return this.LowInt64;
			case 13:
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
			case 24:
			case 26:
				return new NotImplementedException(Res.GetString("DSAdsvalueTypeNYI", new object[] { "0x" + Convert.ToString(this.adsvalue.dwType, 16) }));
			case 27:
			{
				DnWithBinary dnWithBinary = new DnWithBinary();
				Marshal.PtrToStructure(this.adsvalue.pointer.value, dnWithBinary);
				byte[] array2 = new byte[dnWithBinary.dwLength];
				Marshal.Copy(dnWithBinary.lpBinaryValue, array2, 0, dnWithBinary.dwLength);
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i].ToString("X", CultureInfo.InvariantCulture);
					if (text.Length == 1)
					{
						stringBuilder2.Append("0");
					}
					stringBuilder2.Append(text);
				}
				stringBuilder.Append("B:");
				stringBuilder.Append(stringBuilder2.Length);
				stringBuilder.Append(":");
				stringBuilder.Append(stringBuilder2.ToString());
				stringBuilder.Append(":");
				stringBuilder.Append(Marshal.PtrToStringUni(dnWithBinary.pszDNString));
				return stringBuilder.ToString();
			}
			case 28:
			{
				DnWithString dnWithString = new DnWithString();
				Marshal.PtrToStructure(this.adsvalue.pointer.value, dnWithString);
				string text2 = Marshal.PtrToStringUni(dnWithString.pszStringValue);
				if (text2 == null)
				{
					text2 = "";
				}
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.Append("S:");
				stringBuilder3.Append(text2.Length);
				stringBuilder3.Append(":");
				stringBuilder3.Append(text2);
				stringBuilder3.Append(":");
				stringBuilder3.Append(Marshal.PtrToStringUni(dnWithString.pszDNString));
				return stringBuilder3.ToString();
			}
			default:
				return new ArgumentException(Res.GetString("DSConvertFailed", new object[]
				{
					"0x" + Convert.ToString(this.LowInt64, 16),
					"0x" + Convert.ToString(this.adsvalue.dwType, 16)
				}));
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000854C File Offset: 0x0000754C
		public object GetVlvValue()
		{
			AdsVLV adsVLV = new AdsVLV();
			Marshal.PtrToStructure(this.adsvalue.octetString.value, adsVLV);
			byte[] array = null;
			if (adsVLV.contextID != (IntPtr)0 && adsVLV.contextIDlength != 0)
			{
				array = new byte[adsVLV.contextIDlength];
				Marshal.Copy(adsVLV.contextID, array, 0, adsVLV.contextIDlength);
			}
			DirectoryVirtualListView directoryVirtualListView = new DirectoryVirtualListView();
			directoryVirtualListView.Offset = adsVLV.offset;
			directoryVirtualListView.ApproximateTotal = adsVLV.contentCount;
			DirectoryVirtualListViewContext directoryVirtualListViewContext = new DirectoryVirtualListViewContext(array);
			directoryVirtualListView.DirectoryVirtualListViewContext = directoryVirtualListViewContext;
			return directoryVirtualListView;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000085E0 File Offset: 0x000075E0
		private void SetValue(object managedValue, AdsType adsType)
		{
			this.adsvalue = default(AdsValue);
			this.adsvalue.dwType = (int)adsType;
			switch (adsType)
			{
			case AdsType.ADSTYPE_CASE_IGNORE_STRING:
				this.pinnedHandle = GCHandle.Alloc(managedValue, GCHandleType.Pinned);
				this.adsvalue.pointer.value = this.pinnedHandle.AddrOfPinnedObject();
				return;
			case AdsType.ADSTYPE_BOOLEAN:
				if ((bool)managedValue)
				{
					this.LowInt64 = -1L;
					return;
				}
				this.LowInt64 = 0L;
				return;
			case AdsType.ADSTYPE_INTEGER:
				this.adsvalue.generic.a = (int)managedValue;
				this.adsvalue.generic.b = 0;
				return;
			case AdsType.ADSTYPE_LARGE_INTEGER:
				this.LowInt64 = (long)managedValue;
				return;
			case AdsType.ADSTYPE_PROV_SPECIFIC:
			{
				byte[] array = (byte[])managedValue;
				this.adsvalue.octetString.length = array.Length;
				this.pinnedHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				this.adsvalue.octetString.value = this.pinnedHandle.AddrOfPinnedObject();
				return;
			}
			}
			throw new NotImplementedException(Res.GetString("DSAdsvalueTypeNYI", new object[] { "0x" + Convert.ToString((int)adsType, 16) }));
		}

		// Token: 0x0400027A RID: 634
		public AdsValue adsvalue;

		// Token: 0x0400027B RID: 635
		private GCHandle pinnedHandle;
	}
}

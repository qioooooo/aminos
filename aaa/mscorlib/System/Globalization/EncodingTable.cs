using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Globalization
{
	// Token: 0x0200039C RID: 924
	internal static class EncodingTable
	{
		// Token: 0x06002563 RID: 9571 RVA: 0x00068D7C File Offset: 0x00067D7C
		private unsafe static int internalGetCodePageFromName(string name)
		{
			int i = 0;
			int num = EncodingTable.lastEncodingItem;
			while (num - i > 3)
			{
				int num2 = (num - i) / 2 + i;
				bool flag;
				int num3 = string.nativeCompareOrdinalWC(name, EncodingTable.encodingDataPtr[num2].webName, true, out flag);
				if (num3 == 0)
				{
					return EncodingTable.encodingDataPtr[num2].codePage;
				}
				if (num3 < 0)
				{
					num = num2;
				}
				else
				{
					i = num2;
				}
			}
			while (i <= num)
			{
				bool flag2;
				if (string.nativeCompareOrdinalWC(name, EncodingTable.encodingDataPtr[i].webName, true, out flag2) == 0)
				{
					return EncodingTable.encodingDataPtr[i].codePage;
				}
				i++;
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_EncodingNotSupported"), new object[] { name }), "name");
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x00068E4C File Offset: 0x00067E4C
		internal unsafe static EncodingInfo[] GetEncodings()
		{
			if (EncodingTable.lastCodePageItem == 0)
			{
				int num = 0;
				while (EncodingTable.codePageDataPtr[num].codePage != 0)
				{
					num++;
				}
				EncodingTable.lastCodePageItem = num;
			}
			EncodingInfo[] array = new EncodingInfo[EncodingTable.lastCodePageItem];
			for (int i = 0; i < EncodingTable.lastCodePageItem; i++)
			{
				array[i] = new EncodingInfo(EncodingTable.codePageDataPtr[i].codePage, new string(EncodingTable.codePageDataPtr[i].webName), Environment.GetResourceString("Globalization.cp_" + EncodingTable.codePageDataPtr[i].codePage));
			}
			return array;
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x00068F00 File Offset: 0x00067F00
		internal static int GetCodePageFromName(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			object obj = EncodingTable.hashByName[name];
			if (obj != null)
			{
				return (int)obj;
			}
			int num = EncodingTable.internalGetCodePageFromName(name);
			EncodingTable.hashByName[name] = num;
			return num;
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x00068F4C File Offset: 0x00067F4C
		internal unsafe static CodePageDataItem GetCodePageDataItem(int codepage)
		{
			CodePageDataItem codePageDataItem = (CodePageDataItem)EncodingTable.hashByCodePage[codepage];
			if (codePageDataItem != null)
			{
				return codePageDataItem;
			}
			int num = 0;
			int codePage;
			while ((codePage = EncodingTable.codePageDataPtr[num].codePage) != 0)
			{
				if (codePage == codepage)
				{
					codePageDataItem = new CodePageDataItem(num);
					EncodingTable.hashByCodePage[codepage] = codePageDataItem;
					return codePageDataItem;
				}
				num++;
			}
			return null;
		}

		// Token: 0x06002567 RID: 9575
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern InternalEncodingDataItem* GetEncodingData();

		// Token: 0x06002568 RID: 9576
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetNumEncodingItems();

		// Token: 0x06002569 RID: 9577
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern InternalCodePageDataItem* GetCodePageData();

		// Token: 0x0600256A RID: 9578
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern byte* nativeCreateOpenFileMapping(string inSectionName, int inBytesToAllocate, out IntPtr mappedFileHandle);

		// Token: 0x040010C9 RID: 4297
		private static int lastEncodingItem = EncodingTable.GetNumEncodingItems() - 1;

		// Token: 0x040010CA RID: 4298
		private static int lastCodePageItem;

		// Token: 0x040010CB RID: 4299
		internal unsafe static InternalEncodingDataItem* encodingDataPtr = EncodingTable.GetEncodingData();

		// Token: 0x040010CC RID: 4300
		internal unsafe static InternalCodePageDataItem* codePageDataPtr = EncodingTable.GetCodePageData();

		// Token: 0x040010CD RID: 4301
		internal static Hashtable hashByName = Hashtable.Synchronized(new Hashtable(StringComparer.OrdinalIgnoreCase));

		// Token: 0x040010CE RID: 4302
		internal static Hashtable hashByCodePage = Hashtable.Synchronized(new Hashtable());
	}
}

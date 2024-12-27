using System;
using System.Collections;
using System.Threading;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F9 RID: 1273
	internal class GCHandleCookieTable
	{
		// Token: 0x06003181 RID: 12673 RVA: 0x000A99B4 File Offset: 0x000A89B4
		internal GCHandleCookieTable()
		{
			this.m_HandleList = new IntPtr[10];
			this.m_CycleCounts = new byte[10];
			this.m_HandleToCookieMap = new Hashtable();
			this.m_FreeIndex = 1;
			for (int i = 0; i < 10; i++)
			{
				this.m_HandleList[i] = IntPtr.Zero;
				this.m_CycleCounts[i] = 0;
			}
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x000A9A20 File Offset: 0x000A8A20
		internal IntPtr FindOrAddHandle(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			object obj = this.m_HandleToCookieMap[handle];
			if (obj != null)
			{
				return (IntPtr)obj;
			}
			IntPtr intPtr = IntPtr.Zero;
			int i = this.m_FreeIndex;
			if (i < this.m_HandleList.Length && this.m_HandleList[i] == IntPtr.Zero && Interlocked.CompareExchange(ref this.m_HandleList[i], handle, IntPtr.Zero) == IntPtr.Zero)
			{
				intPtr = this.GetCookieFromData((uint)i, this.m_CycleCounts[i]);
				if (i + 1 < this.m_HandleList.Length)
				{
					this.m_FreeIndex = i + 1;
				}
			}
			if (intPtr == IntPtr.Zero)
			{
				i = 1;
				while (i < 16777215)
				{
					if (this.m_HandleList[i] == IntPtr.Zero && Interlocked.CompareExchange(ref this.m_HandleList[i], handle, IntPtr.Zero) == IntPtr.Zero)
					{
						intPtr = this.GetCookieFromData((uint)i, this.m_CycleCounts[i]);
						if (i + 1 < this.m_HandleList.Length)
						{
							this.m_FreeIndex = i + 1;
							break;
						}
						break;
					}
					else
					{
						if (i + 1 >= this.m_HandleList.Length)
						{
							lock (this)
							{
								if (i + 1 >= this.m_HandleList.Length)
								{
									this.GrowArrays();
								}
							}
						}
						i++;
					}
				}
			}
			if (intPtr == IntPtr.Zero)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("OutOfMemory_GCHandleMDA"));
			}
			lock (this)
			{
				obj = this.m_HandleToCookieMap[handle];
				if (obj != null)
				{
					this.m_HandleList[i] = IntPtr.Zero;
					intPtr = (IntPtr)obj;
				}
				else
				{
					this.m_HandleToCookieMap[handle] = intPtr;
				}
			}
			return intPtr;
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x000A9C34 File Offset: 0x000A8C34
		internal IntPtr GetHandle(IntPtr cookie)
		{
			IntPtr zero = IntPtr.Zero;
			if (!this.ValidateCookie(cookie))
			{
				return IntPtr.Zero;
			}
			return this.m_HandleList[this.GetIndexFromCookie(cookie)];
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x000A9C70 File Offset: 0x000A8C70
		internal void RemoveHandleIfPresent(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return;
			}
			object obj = this.m_HandleToCookieMap[handle];
			if (obj != null)
			{
				IntPtr intPtr = (IntPtr)obj;
				if (!this.ValidateCookie(intPtr))
				{
					return;
				}
				int indexFromCookie = this.GetIndexFromCookie(intPtr);
				byte[] cycleCounts = this.m_CycleCounts;
				int num = indexFromCookie;
				cycleCounts[num] += 1;
				this.m_HandleList[indexFromCookie] = IntPtr.Zero;
				this.m_HandleToCookieMap.Remove(handle);
				this.m_FreeIndex = indexFromCookie;
			}
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x000A9D04 File Offset: 0x000A8D04
		private bool ValidateCookie(IntPtr cookie)
		{
			int num;
			byte b;
			this.GetDataFromCookie(cookie, out num, out b);
			if (num >= 16777215)
			{
				return false;
			}
			if (num >= this.m_HandleList.Length)
			{
				return false;
			}
			if (this.m_HandleList[num] == IntPtr.Zero)
			{
				return false;
			}
			byte b2 = (byte)(AppDomain.CurrentDomain.Id % 255);
			byte b3 = this.m_CycleCounts[num] ^ b2;
			return b == b3;
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x000A9D78 File Offset: 0x000A8D78
		private void GrowArrays()
		{
			int num = this.m_HandleList.Length;
			IntPtr[] array = new IntPtr[num * 2];
			byte[] array2 = new byte[num * 2];
			Array.Copy(this.m_HandleList, array, num);
			Array.Copy(this.m_CycleCounts, array2, num);
			this.m_HandleList = array;
			this.m_CycleCounts = array2;
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x000A9DC8 File Offset: 0x000A8DC8
		private IntPtr GetCookieFromData(uint index, byte cycleCount)
		{
			byte b = (byte)(AppDomain.CurrentDomain.Id % 255);
			return (IntPtr)((long)((long)(cycleCount ^ b) << 24) + (long)((ulong)index) + 1L);
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x000A9DFC File Offset: 0x000A8DFC
		private void GetDataFromCookie(IntPtr cookie, out int index, out byte xorData)
		{
			uint num = (uint)(int)cookie;
			index = (int)((num & 16777215U) - 1U);
			xorData = (byte)((num & 4278190080U) >> 24);
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x000A9E28 File Offset: 0x000A8E28
		private int GetIndexFromCookie(IntPtr cookie)
		{
			uint num = (uint)(int)cookie;
			return (int)((num & 16777215U) - 1U);
		}

		// Token: 0x04001974 RID: 6516
		private const int MaxListSize = 16777215;

		// Token: 0x04001975 RID: 6517
		private const uint CookieMaskIndex = 16777215U;

		// Token: 0x04001976 RID: 6518
		private const uint CookieMaskSentinal = 4278190080U;

		// Token: 0x04001977 RID: 6519
		private Hashtable m_HandleToCookieMap;

		// Token: 0x04001978 RID: 6520
		private IntPtr[] m_HandleList;

		// Token: 0x04001979 RID: 6521
		private byte[] m_CycleCounts;

		// Token: 0x0400197A RID: 6522
		private int m_FreeIndex;
	}
}

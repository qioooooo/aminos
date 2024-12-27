using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000020 RID: 32
	public class ManagementObjectCollection : ICollection, IEnumerable, IDisposable
	{
		// Token: 0x06000100 RID: 256 RVA: 0x000071A4 File Offset: 0x000061A4
		internal ManagementObjectCollection(ManagementScope scope, EnumerationOptions options, IEnumWbemClassObject enumWbem)
		{
			if (options != null)
			{
				this.options = (EnumerationOptions)options.Clone();
			}
			else
			{
				this.options = new EnumerationOptions();
			}
			if (scope != null)
			{
				this.scope = scope.Clone();
			}
			else
			{
				this.scope = ManagementScope._Clone(null);
			}
			this.enumWbem = enumWbem;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000071FC File Offset: 0x000061FC
		~ManagementObjectCollection()
		{
			this.Dispose(false);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000722C File Offset: 0x0000622C
		public void Dispose()
		{
			if (!this.isDisposed)
			{
				this.Dispose(true);
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000723D File Offset: 0x0000623D
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				GC.SuppressFinalize(this);
				this.isDisposed = true;
			}
			Marshal.ReleaseComObject(this.enumWbem);
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000725C File Offset: 0x0000625C
		public int Count
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException(ManagementObjectCollection.name);
				}
				int num = 0;
				IEnumerator enumerator = this.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00007294 File Offset: 0x00006294
		public bool IsSynchronized
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException(ManagementObjectCollection.name);
				}
				return false;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000072AA File Offset: 0x000062AA
		public object SyncRoot
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException(ManagementObjectCollection.name);
				}
				return this;
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000072C0 File Offset: 0x000062C0
		public void CopyTo(Array array, int index)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException(ManagementObjectCollection.name);
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < array.GetLowerBound(0) || index > array.GetUpperBound(0))
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int num = array.Length - index;
			int num2 = 0;
			ArrayList arrayList = new ArrayList();
			foreach (ManagementBaseObject managementBaseObject in this)
			{
				arrayList.Add(managementBaseObject);
				num2++;
				if (num2 > num)
				{
					throw new ArgumentException(null, "index");
				}
			}
			arrayList.CopyTo(array, index);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000735C File Offset: 0x0000635C
		public void CopyTo(ManagementBaseObject[] objectCollection, int index)
		{
			this.CopyTo(objectCollection, index);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00007368 File Offset: 0x00006368
		public ManagementObjectCollection.ManagementObjectEnumerator GetEnumerator()
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException(ManagementObjectCollection.name);
			}
			if (this.options.Rewindable)
			{
				IEnumWbemClassObject enumWbemClassObject = null;
				int num = 0;
				try
				{
					num = this.scope.GetSecuredIEnumWbemClassObjectHandler(this.enumWbem).Clone_(ref enumWbemClassObject);
					if (((long)num & (long)((ulong)(-2147483648))) == 0L)
					{
						num = this.scope.GetSecuredIEnumWbemClassObjectHandler(enumWbemClassObject).Reset_();
					}
				}
				catch (COMException ex)
				{
					ManagementException.ThrowWithExtendedInfo(ex);
				}
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else if (((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
				return new ManagementObjectCollection.ManagementObjectEnumerator(this, enumWbemClassObject);
			}
			return new ManagementObjectCollection.ManagementObjectEnumerator(this, this.enumWbem);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000743C File Offset: 0x0000643C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400010A RID: 266
		private static readonly string name = typeof(ManagementObjectCollection).FullName;

		// Token: 0x0400010B RID: 267
		internal ManagementScope scope;

		// Token: 0x0400010C RID: 268
		internal EnumerationOptions options;

		// Token: 0x0400010D RID: 269
		private IEnumWbemClassObject enumWbem;

		// Token: 0x0400010E RID: 270
		private bool isDisposed;

		// Token: 0x02000021 RID: 33
		public class ManagementObjectEnumerator : IEnumerator, IDisposable
		{
			// Token: 0x0600010C RID: 268 RVA: 0x0000745C File Offset: 0x0000645C
			internal ManagementObjectEnumerator(ManagementObjectCollection collectionObject, IEnumWbemClassObject enumWbem)
			{
				this.enumWbem = enumWbem;
				this.collectionObject = collectionObject;
				this.cachedObjects = new IWbemClassObjectFreeThreaded[collectionObject.options.BlockSize];
				this.cachedCount = 0U;
				this.cacheIndex = -1;
				this.atEndOfCollection = false;
			}

			// Token: 0x0600010D RID: 269 RVA: 0x000074A8 File Offset: 0x000064A8
			~ManagementObjectEnumerator()
			{
				this.Dispose();
			}

			// Token: 0x0600010E RID: 270 RVA: 0x000074D4 File Offset: 0x000064D4
			public void Dispose()
			{
				if (!this.isDisposed)
				{
					if (this.enumWbem != null)
					{
						Marshal.ReleaseComObject(this.enumWbem);
						this.enumWbem = null;
					}
					this.cachedObjects = null;
					this.collectionObject = null;
					this.isDisposed = true;
					GC.SuppressFinalize(this);
				}
			}

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x0600010F RID: 271 RVA: 0x00007514 File Offset: 0x00006514
			public ManagementBaseObject Current
			{
				get
				{
					if (this.isDisposed)
					{
						throw new ObjectDisposedException(ManagementObjectCollection.ManagementObjectEnumerator.name);
					}
					if (this.cacheIndex < 0)
					{
						throw new InvalidOperationException();
					}
					return ManagementBaseObject.GetBaseObject(this.cachedObjects[this.cacheIndex], this.collectionObject.scope);
				}
			}

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x06000110 RID: 272 RVA: 0x00007560 File Offset: 0x00006560
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000111 RID: 273 RVA: 0x00007568 File Offset: 0x00006568
			public bool MoveNext()
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException(ManagementObjectCollection.ManagementObjectEnumerator.name);
				}
				if (this.atEndOfCollection)
				{
					return false;
				}
				this.cacheIndex++;
				if ((ulong)this.cachedCount - (ulong)((long)this.cacheIndex) == 0UL)
				{
					int num = ((this.collectionObject.options.Timeout.Ticks == long.MaxValue) ? (-1) : ((int)this.collectionObject.options.Timeout.TotalMilliseconds));
					SecurityHandler securityHandler = this.collectionObject.scope.GetSecurityHandler();
					IWbemClassObject_DoNotMarshal[] array = new IWbemClassObject_DoNotMarshal[this.collectionObject.options.BlockSize];
					int num2 = this.collectionObject.scope.GetSecuredIEnumWbemClassObjectHandler(this.enumWbem).Next_(num, (uint)this.collectionObject.options.BlockSize, array, ref this.cachedCount);
					securityHandler.Reset();
					if (num2 >= 0)
					{
						int num3 = 0;
						while ((long)num3 < (long)((ulong)this.cachedCount))
						{
							this.cachedObjects[num3] = new IWbemClassObjectFreeThreaded(Marshal.GetIUnknownForObject(array[num3]));
							num3++;
						}
					}
					if (num2 < 0)
					{
						if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
						{
							ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
						}
						else
						{
							Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
						}
					}
					else
					{
						if (num2 == 262148 && this.cachedCount == 0U)
						{
							ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
						}
						if (num2 == 1 && this.cachedCount == 0U)
						{
							this.atEndOfCollection = true;
							this.cacheIndex--;
							return false;
						}
					}
					this.cacheIndex = 0;
				}
				return true;
			}

			// Token: 0x06000112 RID: 274 RVA: 0x000076FC File Offset: 0x000066FC
			public void Reset()
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException(ManagementObjectCollection.ManagementObjectEnumerator.name);
				}
				if (!this.collectionObject.options.Rewindable)
				{
					throw new InvalidOperationException();
				}
				SecurityHandler securityHandler = this.collectionObject.scope.GetSecurityHandler();
				int num = 0;
				try
				{
					num = this.collectionObject.scope.GetSecuredIEnumWbemClassObjectHandler(this.enumWbem).Reset_();
				}
				catch (COMException ex)
				{
					ManagementException.ThrowWithExtendedInfo(ex);
				}
				finally
				{
					securityHandler.Reset();
				}
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				}
				else if (((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
				int num2 = ((this.cacheIndex >= 0) ? this.cacheIndex : 0);
				while ((long)num2 < (long)((ulong)this.cachedCount))
				{
					Marshal.ReleaseComObject((IWbemClassObject_DoNotMarshal)Marshal.GetObjectForIUnknown(this.cachedObjects[num2]));
					num2++;
				}
				this.cachedCount = 0U;
				this.cacheIndex = -1;
				this.atEndOfCollection = false;
			}

			// Token: 0x0400010F RID: 271
			private static readonly string name = typeof(ManagementObjectCollection.ManagementObjectEnumerator).FullName;

			// Token: 0x04000110 RID: 272
			private IEnumWbemClassObject enumWbem;

			// Token: 0x04000111 RID: 273
			private ManagementObjectCollection collectionObject;

			// Token: 0x04000112 RID: 274
			private uint cachedCount;

			// Token: 0x04000113 RID: 275
			private int cacheIndex;

			// Token: 0x04000114 RID: 276
			private IWbemClassObjectFreeThreaded[] cachedObjects;

			// Token: 0x04000115 RID: 277
			private bool atEndOfCollection;

			// Token: 0x04000116 RID: 278
			private bool isDisposed;
		}
	}
}

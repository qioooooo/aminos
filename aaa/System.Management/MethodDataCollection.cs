using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000084 RID: 132
	public class MethodDataCollection : ICollection, IEnumerable
	{
		// Token: 0x060003B7 RID: 951 RVA: 0x0001051E File Offset: 0x0000F51E
		internal MethodDataCollection(ManagementObject parent)
		{
			this.parent = parent;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00010530 File Offset: 0x0000F530
		public int Count
		{
			get
			{
				int num = 0;
				IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
				IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded2 = null;
				int num2 = -2147217407;
				lock (typeof(MethodDataCollection.enumLock))
				{
					try
					{
						num2 = this.parent.wbemObject.BeginMethodEnumeration_(0);
						if (num2 >= 0)
						{
							string text = "";
							while (text != null && num2 >= 0 && num2 != 262149)
							{
								text = null;
								wbemClassObjectFreeThreaded = null;
								wbemClassObjectFreeThreaded2 = null;
								num2 = this.parent.wbemObject.NextMethod_(0, out text, out wbemClassObjectFreeThreaded, out wbemClassObjectFreeThreaded2);
								if (num2 >= 0 && num2 != 262149)
								{
									num++;
								}
							}
							this.parent.wbemObject.EndMethodEnumeration_();
						}
					}
					catch (COMException ex)
					{
						ManagementException.ThrowWithExtendedInfo(ex);
					}
				}
				if (((long)num2 & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num2);
				}
				else if (((long)num2 & (long)((ulong)(-2147483648))) != 0L)
				{
					Marshal.ThrowExceptionForHR(num2, WmiNetUtilsHelper.GetErrorInfo_f());
				}
				return num;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00010640 File Offset: 0x0000F640
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00010643 File Offset: 0x0000F643
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00010648 File Offset: 0x0000F648
		public void CopyTo(Array array, int index)
		{
			foreach (MethodData methodData in this)
			{
				array.SetValue(methodData, index++);
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000106A0 File Offset: 0x0000F6A0
		public void CopyTo(MethodData[] methodArray, int index)
		{
			this.CopyTo(methodArray, index);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x000106AA File Offset: 0x0000F6AA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new MethodDataCollection.MethodDataEnumerator(this.parent);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000106B7 File Offset: 0x0000F6B7
		public MethodDataCollection.MethodDataEnumerator GetEnumerator()
		{
			return new MethodDataCollection.MethodDataEnumerator(this.parent);
		}

		// Token: 0x1700009F RID: 159
		public virtual MethodData this[string methodName]
		{
			get
			{
				if (methodName == null)
				{
					throw new ArgumentNullException("methodName");
				}
				return new MethodData(this.parent, methodName);
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000106E0 File Offset: 0x0000F6E0
		public virtual void Remove(string methodName)
		{
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				throw new InvalidOperationException();
			}
			int num = -2147217407;
			try
			{
				num = this.parent.wbemObject.DeleteMethod_(methodName);
			}
			catch (COMException ex)
			{
				ManagementException.ThrowWithExtendedInfo(ex);
			}
			if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				return;
			}
			if (((long)num & (long)((ulong)(-2147483648))) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00010774 File Offset: 0x0000F774
		public virtual void Add(string methodName)
		{
			this.Add(methodName, null, null);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00010780 File Offset: 0x0000F780
		public virtual void Add(string methodName, ManagementBaseObject inParameters, ManagementBaseObject outParameters)
		{
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded2 = null;
			if (this.parent.GetType() == typeof(ManagementObject))
			{
				throw new InvalidOperationException();
			}
			if (inParameters != null)
			{
				wbemClassObjectFreeThreaded = inParameters.wbemObject;
			}
			if (outParameters != null)
			{
				wbemClassObjectFreeThreaded2 = outParameters.wbemObject;
			}
			int num = -2147217407;
			try
			{
				num = this.parent.wbemObject.PutMethod_(methodName, 0, wbemClassObjectFreeThreaded, wbemClassObjectFreeThreaded2);
			}
			catch (COMException ex)
			{
				ManagementException.ThrowWithExtendedInfo(ex);
			}
			if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				return;
			}
			if (((long)num & (long)((ulong)(-2147483648))) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x040001EE RID: 494
		private ManagementObject parent;

		// Token: 0x02000085 RID: 133
		private class enumLock
		{
		}

		// Token: 0x02000086 RID: 134
		public class MethodDataEnumerator : IEnumerator
		{
			// Token: 0x060003C4 RID: 964 RVA: 0x00010838 File Offset: 0x0000F838
			internal MethodDataEnumerator(ManagementObject parent)
			{
				this.parent = parent;
				this.methodNames = new ArrayList();
				IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
				IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded2 = null;
				int num = -2147217407;
				lock (typeof(MethodDataCollection.enumLock))
				{
					try
					{
						num = parent.wbemObject.BeginMethodEnumeration_(0);
						if (num >= 0)
						{
							string text = "";
							while (text != null && num >= 0 && num != 262149)
							{
								text = null;
								num = parent.wbemObject.NextMethod_(0, out text, out wbemClassObjectFreeThreaded, out wbemClassObjectFreeThreaded2);
								if (num >= 0 && num != 262149)
								{
									this.methodNames.Add(text);
								}
							}
							parent.wbemObject.EndMethodEnumeration_();
						}
					}
					catch (COMException ex)
					{
						ManagementException.ThrowWithExtendedInfo(ex);
					}
					this.en = this.methodNames.GetEnumerator();
				}
				if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
				{
					ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					return;
				}
				if (((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
				}
			}

			// Token: 0x170000A0 RID: 160
			// (get) Token: 0x060003C5 RID: 965 RVA: 0x00010954 File Offset: 0x0000F954
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x170000A1 RID: 161
			// (get) Token: 0x060003C6 RID: 966 RVA: 0x0001095C File Offset: 0x0000F95C
			public MethodData Current
			{
				get
				{
					return new MethodData(this.parent, (string)this.en.Current);
				}
			}

			// Token: 0x060003C7 RID: 967 RVA: 0x00010979 File Offset: 0x0000F979
			public bool MoveNext()
			{
				return this.en.MoveNext();
			}

			// Token: 0x060003C8 RID: 968 RVA: 0x00010986 File Offset: 0x0000F986
			public void Reset()
			{
				this.en.Reset();
			}

			// Token: 0x040001EF RID: 495
			private ManagementObject parent;

			// Token: 0x040001F0 RID: 496
			private ArrayList methodNames;

			// Token: 0x040001F1 RID: 497
			private IEnumerator en;
		}
	}
}

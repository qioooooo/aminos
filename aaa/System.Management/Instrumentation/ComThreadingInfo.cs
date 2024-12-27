using System;
using System.Runtime.InteropServices;

namespace System.Management.Instrumentation
{
	// Token: 0x02000095 RID: 149
	internal class ComThreadingInfo
	{
		// Token: 0x06000466 RID: 1126 RVA: 0x00021BF8 File Offset: 0x00020BF8
		private ComThreadingInfo()
		{
			ComThreadingInfo.IComThreadingInfo comThreadingInfo = (ComThreadingInfo.IComThreadingInfo)ComThreadingInfo.CoGetObjectContext(ref this.IID_IUnknown);
			this.apartmentType = comThreadingInfo.GetCurrentApartmentType();
			this.threadType = comThreadingInfo.GetCurrentThreadType();
			this.logicalThreadId = comThreadingInfo.GetCurrentLogicalThreadId();
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00021C50 File Offset: 0x00020C50
		public static ComThreadingInfo Current
		{
			get
			{
				return new ComThreadingInfo();
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00021C57 File Offset: 0x00020C57
		public override string ToString()
		{
			return string.Format("{{{0}}} - {1} - {2}", this.LogicalThreadId, this.ApartmentType, this.ThreadType);
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x00021C84 File Offset: 0x00020C84
		public ComThreadingInfo.APTTYPE ApartmentType
		{
			get
			{
				return this.apartmentType;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x00021C8C File Offset: 0x00020C8C
		public ComThreadingInfo.THDTYPE ThreadType
		{
			get
			{
				return this.threadType;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00021C94 File Offset: 0x00020C94
		public Guid LogicalThreadId
		{
			get
			{
				return this.logicalThreadId;
			}
		}

		// Token: 0x0600046C RID: 1132
		[DllImport("ole32.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		private static extern object CoGetObjectContext([In] ref Guid riid);

		// Token: 0x04000266 RID: 614
		private Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

		// Token: 0x04000267 RID: 615
		private ComThreadingInfo.APTTYPE apartmentType;

		// Token: 0x04000268 RID: 616
		private ComThreadingInfo.THDTYPE threadType;

		// Token: 0x04000269 RID: 617
		private Guid logicalThreadId;

		// Token: 0x02000096 RID: 150
		public enum APTTYPE
		{
			// Token: 0x0400026B RID: 619
			APTTYPE_CURRENT = -1,
			// Token: 0x0400026C RID: 620
			APTTYPE_STA,
			// Token: 0x0400026D RID: 621
			APTTYPE_MTA,
			// Token: 0x0400026E RID: 622
			APTTYPE_NA,
			// Token: 0x0400026F RID: 623
			APTTYPE_MAINSTA
		}

		// Token: 0x02000097 RID: 151
		public enum THDTYPE
		{
			// Token: 0x04000271 RID: 625
			THDTYPE_BLOCKMESSAGES,
			// Token: 0x04000272 RID: 626
			THDTYPE_PROCESSMESSAGES
		}

		// Token: 0x02000098 RID: 152
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("000001ce-0000-0000-C000-000000000046")]
		[ComImport]
		private interface IComThreadingInfo
		{
			// Token: 0x0600046D RID: 1133
			ComThreadingInfo.APTTYPE GetCurrentApartmentType();

			// Token: 0x0600046E RID: 1134
			ComThreadingInfo.THDTYPE GetCurrentThreadType();

			// Token: 0x0600046F RID: 1135
			Guid GetCurrentLogicalThreadId();

			// Token: 0x06000470 RID: 1136
			void SetCurrentLogicalThreadId([In] Guid rguid);
		}
	}
}

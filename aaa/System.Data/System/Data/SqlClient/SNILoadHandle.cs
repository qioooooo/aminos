using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.SqlClient
{
	// Token: 0x0200032F RID: 815
	internal sealed class SNILoadHandle : SafeHandle
	{
		// Token: 0x06002A8C RID: 10892 RVA: 0x0029D0F4 File Offset: 0x0029C4F4
		private SNILoadHandle()
			: base(IntPtr.Zero, true)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this._sniStatus = SNINativeMethodWrapper.SNIInitialize();
				uint num = 0U;
				SNINativeMethodWrapper.SNIQueryInfo(SNINativeMethodWrapper.QTypes.SNI_QUERY_CLIENT_ENCRYPT_POSSIBLE, ref num);
				this._encryptionOption = ((num == 0U) ? EncryptionOptions.NOT_SUP : EncryptionOptions.OFF);
				this.handle = (IntPtr)1;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002A8D RID: 10893 RVA: 0x0029D18C File Offset: 0x0029C58C
		public override bool IsInvalid
		{
			get
			{
				return IntPtr.Zero == this.handle;
			}
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x0029D1AC File Offset: 0x0029C5AC
		protected override bool ReleaseHandle()
		{
			if (this.handle != IntPtr.Zero)
			{
				if (this._sniStatus == 0U)
				{
					LocalDBAPI.ReleaseDLLHandles();
					SNINativeMethodWrapper.SNITerminate();
				}
				this.handle = IntPtr.Zero;
			}
			return true;
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002A8F RID: 10895 RVA: 0x0029D1EC File Offset: 0x0029C5EC
		public uint SNIStatus
		{
			get
			{
				return this._sniStatus;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002A90 RID: 10896 RVA: 0x0029D200 File Offset: 0x0029C600
		public EncryptionOptions Options
		{
			get
			{
				return this._encryptionOption;
			}
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x0029D214 File Offset: 0x0029C614
		private static void ReadDispatcher(IntPtr key, IntPtr packet, uint error)
		{
			if (IntPtr.Zero != key)
			{
				TdsParserStateObject tdsParserStateObject = (TdsParserStateObject)((GCHandle)key).Target;
				if (tdsParserStateObject != null)
				{
					tdsParserStateObject.ReadAsyncCallback(IntPtr.Zero, packet, error);
				}
			}
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x0029D254 File Offset: 0x0029C654
		private static void WriteDispatcher(IntPtr key, IntPtr packet, uint error)
		{
			if (IntPtr.Zero != key)
			{
				TdsParserStateObject tdsParserStateObject = (TdsParserStateObject)((GCHandle)key).Target;
				if (tdsParserStateObject != null)
				{
					tdsParserStateObject.WriteAsyncCallback(IntPtr.Zero, packet, error);
				}
			}
		}

		// Token: 0x04001BF1 RID: 7153
		internal static readonly SNILoadHandle SingletonInstance = new SNILoadHandle();

		// Token: 0x04001BF2 RID: 7154
		internal readonly SNINativeMethodWrapper.SqlAsyncCallbackDelegate ReadAsyncCallbackDispatcher = new SNINativeMethodWrapper.SqlAsyncCallbackDelegate(SNILoadHandle.ReadDispatcher);

		// Token: 0x04001BF3 RID: 7155
		internal readonly SNINativeMethodWrapper.SqlAsyncCallbackDelegate WriteAsyncCallbackDispatcher = new SNINativeMethodWrapper.SqlAsyncCallbackDelegate(SNILoadHandle.WriteDispatcher);

		// Token: 0x04001BF4 RID: 7156
		private readonly uint _sniStatus = uint.MaxValue;

		// Token: 0x04001BF5 RID: 7157
		private readonly EncryptionOptions _encryptionOption;
	}
}

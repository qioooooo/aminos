using System;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006BA RID: 1722
	[Serializable]
	internal class CrossAppDomainData
	{
		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06003E94 RID: 16020 RVA: 0x000D7492 File Offset: 0x000D6492
		internal virtual IntPtr ContextID
		{
			get
			{
				return new IntPtr((int)this._ContextID);
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06003E95 RID: 16021 RVA: 0x000D74A4 File Offset: 0x000D64A4
		internal virtual int DomainID
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._DomainID;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06003E96 RID: 16022 RVA: 0x000D74AC File Offset: 0x000D64AC
		internal virtual string ProcessGuid
		{
			get
			{
				return this._processGuid;
			}
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x000D74B4 File Offset: 0x000D64B4
		internal CrossAppDomainData(IntPtr ctxId, int domainID, string processGuid)
		{
			this._DomainID = domainID;
			this._processGuid = processGuid;
			this._ContextID = ctxId.ToInt32();
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x000D74E8 File Offset: 0x000D64E8
		internal bool IsFromThisProcess()
		{
			return Identity.ProcessGuid.Equals(this._processGuid);
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x000D74FA File Offset: 0x000D64FA
		internal bool IsFromThisAppDomain()
		{
			return this.IsFromThisProcess() && Thread.GetDomain().GetId() == this._DomainID;
		}

		// Token: 0x04001FA6 RID: 8102
		private object _ContextID = 0;

		// Token: 0x04001FA7 RID: 8103
		private int _DomainID;

		// Token: 0x04001FA8 RID: 8104
		private string _processGuid;
	}
}

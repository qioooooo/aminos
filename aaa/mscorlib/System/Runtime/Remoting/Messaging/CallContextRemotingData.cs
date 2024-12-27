using System;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000696 RID: 1686
	[Serializable]
	internal class CallContextRemotingData : ICloneable
	{
		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06003D69 RID: 15721 RVA: 0x000D2ADC File Offset: 0x000D1ADC
		// (set) Token: 0x06003D6A RID: 15722 RVA: 0x000D2AE4 File Offset: 0x000D1AE4
		internal string LogicalCallID
		{
			get
			{
				return this._logicalCallID;
			}
			set
			{
				this._logicalCallID = value;
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06003D6B RID: 15723 RVA: 0x000D2AED File Offset: 0x000D1AED
		internal bool HasInfo
		{
			get
			{
				return this._logicalCallID != null;
			}
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x000D2AFC File Offset: 0x000D1AFC
		public object Clone()
		{
			return new CallContextRemotingData
			{
				LogicalCallID = this.LogicalCallID
			};
		}

		// Token: 0x04001F33 RID: 7987
		private string _logicalCallID;
	}
}

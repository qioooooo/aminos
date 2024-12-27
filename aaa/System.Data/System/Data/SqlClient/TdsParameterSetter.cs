using System;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x0200031B RID: 795
	internal class TdsParameterSetter : SmiTypedGetterSetter
	{
		// Token: 0x060029C0 RID: 10688 RVA: 0x00293DE0 File Offset: 0x002931E0
		internal TdsParameterSetter(TdsParserStateObject stateObj, SmiMetaData md)
		{
			this._target = new TdsRecordBufferSetter(stateObj, md);
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x060029C1 RID: 10689 RVA: 0x00293E00 File Offset: 0x00293200
		internal override bool CanGet
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x060029C2 RID: 10690 RVA: 0x00293E10 File Offset: 0x00293210
		internal override bool CanSet
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x00293E20 File Offset: 0x00293220
		internal override SmiTypedGetterSetter GetTypedGetterSetter(SmiEventSink sink, int ordinal)
		{
			return this._target;
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x00293E34 File Offset: 0x00293234
		public override void SetDBNull(SmiEventSink sink, int ordinal)
		{
			this._target.EndElements(sink);
		}

		// Token: 0x04001B35 RID: 6965
		private TdsRecordBufferSetter _target;
	}
}

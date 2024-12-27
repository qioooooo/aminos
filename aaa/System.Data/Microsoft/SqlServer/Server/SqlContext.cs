using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security.Principal;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200004E RID: 78
	public sealed class SqlContext
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x001CDAC8 File Offset: 0x001CCEC8
		private SqlContext(SmiContext smiContext)
		{
			this._smiContext = smiContext;
			this._smiContext.OutOfScope += this.OnOutOfScope;
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x001CDAFC File Offset: 0x001CCEFC
		public static bool IsAvailable
		{
			get
			{
				return InOutOfProcHelper.InProc;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x001CDB10 File Offset: 0x001CCF10
		public static SqlPipe Pipe
		{
			get
			{
				return SqlContext.CurrentContext.InstancePipe;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x001CDB28 File Offset: 0x001CCF28
		public static SqlTriggerContext TriggerContext
		{
			get
			{
				return SqlContext.CurrentContext.InstanceTriggerContext;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x001CDB40 File Offset: 0x001CCF40
		public static WindowsIdentity WindowsIdentity
		{
			get
			{
				return SqlContext.CurrentContext.InstanceWindowsIdentity;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x001CDB58 File Offset: 0x001CCF58
		private static SqlContext CurrentContext
		{
			get
			{
				SmiContext currentContext = SmiContextFactory.Instance.GetCurrentContext();
				SqlContext sqlContext = (SqlContext)currentContext.GetContextValue(1);
				if (sqlContext == null)
				{
					sqlContext = new SqlContext(currentContext);
					currentContext.SetContextValue(1, sqlContext);
				}
				return sqlContext;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060002FA RID: 762 RVA: 0x001CDB90 File Offset: 0x001CCF90
		private SqlPipe InstancePipe
		{
			get
			{
				if (this._pipe == null && this._smiContext.HasContextPipe)
				{
					this._pipe = new SqlPipe(this._smiContext);
				}
				return this._pipe;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060002FB RID: 763 RVA: 0x001CDBCC File Offset: 0x001CCFCC
		private SqlTriggerContext InstanceTriggerContext
		{
			get
			{
				if (this._triggerContext == null)
				{
					SmiEventSink_Default smiEventSink_Default = new SmiEventSink_Default();
					bool[] array;
					TriggerAction triggerAction;
					SqlXml sqlXml;
					this._smiContext.GetTriggerInfo(smiEventSink_Default, out array, out triggerAction, out sqlXml);
					smiEventSink_Default.ProcessMessagesAndThrow();
					if (triggerAction != TriggerAction.Invalid)
					{
						this._triggerContext = new SqlTriggerContext(triggerAction, array, sqlXml);
					}
				}
				return this._triggerContext;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002FC RID: 764 RVA: 0x001CDC18 File Offset: 0x001CD018
		private WindowsIdentity InstanceWindowsIdentity
		{
			get
			{
				return this._smiContext.WindowsIdentity;
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x001CDC30 File Offset: 0x001CD030
		private void OnOutOfScope(object s, EventArgs e)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.SqlContext.OutOfScope|ADV> SqlContext is out of scope\n");
			}
			if (this._pipe != null)
			{
				this._pipe.OnOutOfScope();
			}
			this._triggerContext = null;
		}

		// Token: 0x0400060A RID: 1546
		private SmiContext _smiContext;

		// Token: 0x0400060B RID: 1547
		private SqlPipe _pipe;

		// Token: 0x0400060C RID: 1548
		private SqlTriggerContext _triggerContext;
	}
}

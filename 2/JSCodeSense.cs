using System;
using System.Security.Permissions;
using Microsoft.JScript.Vsa;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000098 RID: 152
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	internal class JSCodeSense : IVsaSite, IParseText
	{
		// Token: 0x060006B6 RID: 1718 RVA: 0x0002F010 File Offset: 0x0002E010
		internal JSCodeSense()
		{
			this._engine = new VsaEngine(true);
			this._engine.InitVsaEngine("JSC://Microsoft.JScript.Vsa.VsaEngine", this);
			this._codeBlock = (IVsaCodeItem)this._engine.Items.CreateItem("Code", VsaItemType.Code, VsaItemFlag.None);
			this._errorHandler = null;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0002F069 File Offset: 0x0002E069
		public virtual void GetCompiledState(out byte[] pe, out byte[] debugInfo)
		{
			pe = null;
			debugInfo = null;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0002F071 File Offset: 0x0002E071
		public virtual object GetGlobalInstance(string Name)
		{
			return null;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0002F074 File Offset: 0x0002E074
		public virtual object GetEventSourceInstance(string ItemName, string EventSourceName)
		{
			return null;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002F077 File Offset: 0x0002E077
		public virtual bool OnCompilerError(IVsaError error)
		{
			return !(error is IVsaFullErrorInfo) || this._errorHandler.OnCompilerError((IVsaFullErrorInfo)error);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002F094 File Offset: 0x0002E094
		public virtual void Parse(string code, IErrorHandler errorHandler)
		{
			this._engine.Reset();
			this._errorHandler = errorHandler;
			this._codeBlock.SourceText = code;
			this._engine.CheckForErrors();
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002F0BF File Offset: 0x0002E0BF
		public virtual void Notify(string notification, object value)
		{
		}

		// Token: 0x0400030D RID: 781
		private VsaEngine _engine;

		// Token: 0x0400030E RID: 782
		private IVsaCodeItem _codeBlock;

		// Token: 0x0400030F RID: 783
		private IErrorHandler _errorHandler;
	}
}

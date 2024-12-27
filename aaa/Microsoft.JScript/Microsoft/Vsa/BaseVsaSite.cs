using System;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x02000026 RID: 38
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	public class BaseVsaSite : IVsaSite
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00008046 File Offset: 0x00007046
		public virtual byte[] Assembly
		{
			get
			{
				throw new VsaException(VsaError.GetCompiledStateFailed);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00008052 File Offset: 0x00007052
		public virtual byte[] DebugInfo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008055 File Offset: 0x00007055
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void GetCompiledState(out byte[] pe, out byte[] debugInfo)
		{
			pe = this.Assembly;
			debugInfo = this.DebugInfo;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008067 File Offset: 0x00007067
		public virtual object GetEventSourceInstance(string itemName, string eventSourceName)
		{
			throw new VsaException(VsaError.CallbackUnexpected);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00008073 File Offset: 0x00007073
		public virtual object GetGlobalInstance(string name)
		{
			throw new VsaException(VsaError.CallbackUnexpected);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000807F File Offset: 0x0000707F
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual void Notify(string notify, object optional)
		{
			throw new VsaException(VsaError.CallbackUnexpected);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000808B File Offset: 0x0000708B
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public virtual bool OnCompilerError(IVsaError error)
		{
			return false;
		}
	}
}

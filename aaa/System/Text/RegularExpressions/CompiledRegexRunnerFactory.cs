using System;
using System.Reflection.Emit;
using System.Security.Permissions;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000033 RID: 51
	internal sealed class CompiledRegexRunnerFactory : RegexRunnerFactory
	{
		// Token: 0x0600024F RID: 591 RVA: 0x00012316 File Offset: 0x00011316
		internal CompiledRegexRunnerFactory(DynamicMethod go, DynamicMethod firstChar, DynamicMethod trackCount)
		{
			this.goMethod = go;
			this.findFirstCharMethod = firstChar;
			this.initTrackCountMethod = trackCount;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00012334 File Offset: 0x00011334
		protected internal override RegexRunner CreateInstance()
		{
			CompiledRegexRunner compiledRegexRunner = new CompiledRegexRunner();
			new ReflectionPermission(PermissionState.Unrestricted).Assert();
			compiledRegexRunner.SetDelegates((NoParamDelegate)this.goMethod.CreateDelegate(typeof(NoParamDelegate)), (FindFirstCharDelegate)this.findFirstCharMethod.CreateDelegate(typeof(FindFirstCharDelegate)), (NoParamDelegate)this.initTrackCountMethod.CreateDelegate(typeof(NoParamDelegate)));
			return compiledRegexRunner;
		}

		// Token: 0x040007D1 RID: 2001
		private DynamicMethod goMethod;

		// Token: 0x040007D2 RID: 2002
		private DynamicMethod findFirstCharMethod;

		// Token: 0x040007D3 RID: 2003
		private DynamicMethod initTrackCountMethod;
	}
}

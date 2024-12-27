using System;
using System.Diagnostics;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000060 RID: 96
	public class DebugBreak : AST
	{
		// Token: 0x060004CE RID: 1230 RVA: 0x00024636 File Offset: 0x00023636
		internal DebugBreak(Context context)
			: base(context)
		{
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0002463F File Offset: 0x0002363F
		internal override object Evaluate()
		{
			Debugger.Break();
			return new Completion();
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0002464B File Offset: 0x0002364B
		internal override AST PartiallyEvaluate()
		{
			return this;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0002464E File Offset: 0x0002364E
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.context.EmitLineInfo(il);
			il.Emit(OpCodes.Call, CompilerGlobals.debugBreak);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0002466C File Offset: 0x0002366C
		internal override void TranslateToILInitializer(ILGenerator il)
		{
		}
	}
}

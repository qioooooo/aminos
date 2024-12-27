using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000017 RID: 23
	public sealed class AssemblyCustomAttributeList : AST
	{
		// Token: 0x06000112 RID: 274 RVA: 0x00006BF2 File Offset: 0x00005BF2
		internal AssemblyCustomAttributeList(CustomAttributeList list)
			: base(list.context)
		{
			this.list = list;
			this.okToUse = false;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00006C0E File Offset: 0x00005C0E
		internal override object Evaluate()
		{
			return null;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00006C11 File Offset: 0x00005C11
		internal void Process()
		{
			this.okToUse = true;
			this.list.SetTarget(this);
			this.list.PartiallyEvaluate();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00006C32 File Offset: 0x00005C32
		internal override AST PartiallyEvaluate()
		{
			if (!this.okToUse)
			{
				this.context.HandleError(JSError.AssemblyAttributesMustBeGlobal);
			}
			return this;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00006C50 File Offset: 0x00005C50
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			foreach (CustomAttributeBuilder customAttributeBuilder in this.list.GetCustomAttributeBuilders(false))
			{
				base.compilerGlobals.assemblyBuilder.SetCustomAttribute(customAttributeBuilder);
			}
			if (rtype != Typeob.Void)
			{
				il.Emit(OpCodes.Ldnull);
				if (rtype.IsValueType)
				{
					Convert.Emit(this, il, Typeob.Object, rtype);
				}
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006CB5 File Offset: 0x00005CB5
		internal override void TranslateToILInitializer(ILGenerator il)
		{
		}

		// Token: 0x0400003E RID: 62
		private CustomAttributeList list;

		// Token: 0x0400003F RID: 63
		internal bool okToUse;
	}
}

using System;
using System.Diagnostics;

namespace Microsoft.JScript
{
	// Token: 0x0200000E RID: 14
	public sealed class ArgumentsObject : JSObject
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00004B74 File Offset: 0x00003B74
		internal ArgumentsObject(ScriptObject parent, object[] arguments, FunctionObject function, Closure callee, ScriptObject scope, ArgumentsObject caller)
			: base(parent)
		{
			this.arguments = arguments;
			this.formal_names = function.formal_parameters;
			this.scope = scope;
			this.callee = callee;
			this.caller = caller;
			this.length = arguments.Length;
			this.noExpando = false;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004BC8 File Offset: 0x00003BC8
		internal override object GetValueAtIndex(uint index)
		{
			if ((ulong)index < (ulong)((long)this.arguments.Length))
			{
				return this.arguments[(int)index];
			}
			return base.GetValueAtIndex(index);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004BE8 File Offset: 0x00003BE8
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override object GetMemberValue(string name)
		{
			long num = ArrayObject.Array_index_for(name);
			if (num < 0L)
			{
				return base.GetMemberValue(name);
			}
			return this.GetValueAtIndex((uint)num);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004C11 File Offset: 0x00003C11
		internal override void SetValueAtIndex(uint index, object value)
		{
			if ((ulong)index < (ulong)((long)this.arguments.Length))
			{
				this.arguments[(int)index] = value;
				return;
			}
			base.SetValueAtIndex(index, value);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004C32 File Offset: 0x00003C32
		internal object[] ToArray()
		{
			return this.arguments;
		}

		// Token: 0x04000024 RID: 36
		private object[] arguments;

		// Token: 0x04000025 RID: 37
		private string[] formal_names;

		// Token: 0x04000026 RID: 38
		private ScriptObject scope;

		// Token: 0x04000027 RID: 39
		public object callee;

		// Token: 0x04000028 RID: 40
		public object caller;

		// Token: 0x04000029 RID: 41
		public object length;
	}
}

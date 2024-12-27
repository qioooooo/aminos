using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x0200009D RID: 157
	internal sealed class JSClosureMethod : JSMethod
	{
		// Token: 0x06000704 RID: 1796 RVA: 0x00031074 File Offset: 0x00030074
		internal JSClosureMethod(MethodInfo method)
			: base(null)
		{
			this.method = method;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00031084 File Offset: 0x00030084
		internal override object Construct(object[] args)
		{
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0003108D File Offset: 0x0003008D
		public override MethodAttributes Attributes
		{
			get
			{
				return (this.method.Attributes & ~MethodAttributes.Virtual) | MethodAttributes.Static;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x000310A0 File Offset: 0x000300A0
		public override Type DeclaringType
		{
			get
			{
				return this.method.DeclaringType;
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000310AD File Offset: 0x000300AD
		public override ParameterInfo[] GetParameters()
		{
			return this.method.GetParameters();
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x000310BA File Offset: 0x000300BA
		internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
		{
			if (this.method is JSMethod)
			{
				return ((JSMethod)this.method).GetMethodInfo(compilerGlobals);
			}
			return this.method;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000310E1 File Offset: 0x000300E1
		internal override object Invoke(object obj, object thisob, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			if (obj is StackFrame)
			{
				return this.method.Invoke(((StackFrame)((StackFrame)obj).engine.ScriptObjectStackTop()).closureInstance, options, binder, parameters, culture);
			}
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0003111F File Offset: 0x0003011F
		public override string Name
		{
			get
			{
				return this.method.Name;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x0003112C File Offset: 0x0003012C
		public override Type ReturnType
		{
			get
			{
				return this.method.ReturnType;
			}
		}

		// Token: 0x0400031E RID: 798
		internal MethodInfo method;
	}
}

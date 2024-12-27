using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000A8 RID: 168
	internal sealed class JSExpandoIndexerMethod : JSMethod
	{
		// Token: 0x060007C0 RID: 1984 RVA: 0x000358C4 File Offset: 0x000348C4
		internal JSExpandoIndexerMethod(ClassScope classScope, bool isGetter)
			: base(null)
		{
			this.isGetter = isGetter;
			this.classScope = classScope;
			this.GetterParams = new ParameterInfo[]
			{
				new ParameterDeclaration(Typeob.String, "field")
			};
			this.SetterParams = new ParameterInfo[]
			{
				new ParameterDeclaration(Typeob.String, "field"),
				new ParameterDeclaration(Typeob.Object, "value")
			};
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00035938 File Offset: 0x00034938
		internal override object Construct(object[] args)
		{
			throw new JScriptException(JSError.InvalidCall);
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00035940 File Offset: 0x00034940
		public override MethodAttributes Attributes
		{
			get
			{
				return MethodAttributes.Public;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00035943 File Offset: 0x00034943
		public override Type DeclaringType
		{
			get
			{
				return this.classScope.GetTypeBuilderOrEnumBuilder();
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00035950 File Offset: 0x00034950
		public override ParameterInfo[] GetParameters()
		{
			if (this.isGetter)
			{
				return this.GetterParams;
			}
			return this.SetterParams;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00035968 File Offset: 0x00034968
		internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
		{
			if (this.isGetter)
			{
				if (this.token == null)
				{
					this.token = this.classScope.owner.GetExpandoIndexerGetter();
				}
			}
			else if (this.token == null)
			{
				this.token = this.classScope.owner.GetExpandoIndexerSetter();
			}
			return this.token;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x000359C1 File Offset: 0x000349C1
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override object Invoke(object obj, object thisob, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new JScriptException(JSError.InvalidCall);
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x000359C9 File Offset: 0x000349C9
		public override string Name
		{
			get
			{
				if (this.isGetter)
				{
					return "get_Item";
				}
				return "set_Item";
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x000359DE File Offset: 0x000349DE
		public override Type ReturnType
		{
			get
			{
				if (this.isGetter)
				{
					return Typeob.Object;
				}
				return Typeob.Void;
			}
		}

		// Token: 0x04000427 RID: 1063
		private ClassScope classScope;

		// Token: 0x04000428 RID: 1064
		private bool isGetter;

		// Token: 0x04000429 RID: 1065
		private MethodInfo token;

		// Token: 0x0400042A RID: 1066
		private ParameterInfo[] GetterParams;

		// Token: 0x0400042B RID: 1067
		private ParameterInfo[] SetterParams;
	}
}

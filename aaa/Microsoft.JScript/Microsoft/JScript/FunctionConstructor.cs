using System;
using System.Text;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000081 RID: 129
	public sealed class FunctionConstructor : ScriptFunction
	{
		// Token: 0x060005BF RID: 1471 RVA: 0x000284F2 File Offset: 0x000274F2
		internal FunctionConstructor()
			: base(FunctionPrototype.ob, "Function", 1)
		{
			this.originalPrototype = FunctionPrototype.ob;
			FunctionPrototype._constructor = this;
			this.proto = FunctionPrototype.ob;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00028521 File Offset: 0x00027521
		internal FunctionConstructor(LenientFunctionPrototype prototypeProp)
			: base(prototypeProp, "Function", 1)
		{
			this.originalPrototype = prototypeProp;
			prototypeProp.constructor = this;
			this.proto = prototypeProp;
			this.noExpando = false;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0002854C File Offset: 0x0002754C
		internal override object Call(object[] args, object thisob)
		{
			return this.Construct(args, this.engine);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0002855B File Offset: 0x0002755B
		internal override object Construct(object[] args)
		{
			return this.Construct(args, this.engine);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0002856C File Offset: 0x0002756C
		internal ScriptFunction Construct(object[] args, VsaEngine engine)
		{
			StringBuilder stringBuilder = new StringBuilder("function anonymous(");
			int i = 0;
			int num = args.Length - 2;
			while (i < num)
			{
				stringBuilder.Append(Convert.ToString(args[i]));
				stringBuilder.Append(", ");
				i++;
			}
			if (args.Length > 1)
			{
				stringBuilder.Append(Convert.ToString(args[args.Length - 2]));
			}
			stringBuilder.Append(") {\n");
			if (args.Length > 0)
			{
				stringBuilder.Append(Convert.ToString(args[args.Length - 1]));
			}
			stringBuilder.Append("\n}");
			Context context = new Context(new DocumentContext("anonymous", engine), stringBuilder.ToString());
			JSParser jsparser = new JSParser(context);
			engine.PushScriptObject(((IActivationObject)engine.ScriptObjectStackTop()).GetGlobalScope());
			ScriptFunction scriptFunction;
			try
			{
				scriptFunction = (ScriptFunction)jsparser.ParseFunctionExpression().PartiallyEvaluate().Evaluate();
			}
			finally
			{
				engine.PopScriptObject();
			}
			return scriptFunction;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00028664 File Offset: 0x00027664
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new ScriptFunction CreateInstance(params object[] args)
		{
			return this.Construct(args, this.engine);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00028673 File Offset: 0x00027673
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public ScriptFunction Invoke(params object[] args)
		{
			return this.Construct(args, this.engine);
		}

		// Token: 0x04000280 RID: 640
		internal static readonly FunctionConstructor ob = new FunctionConstructor();

		// Token: 0x04000281 RID: 641
		internal FunctionPrototype originalPrototype;
	}
}

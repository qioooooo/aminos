using System;
using System.Collections;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000140 RID: 320
	internal class Function : AstNode
	{
		// Token: 0x06001228 RID: 4648 RVA: 0x0004FBCE File Offset: 0x0004EBCE
		public Function(Function.FunctionType ftype, ArrayList argumentList)
		{
			this.functionType = ftype;
			this.argumentList = new ArrayList(argumentList);
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0004FBE9 File Offset: 0x0004EBE9
		public Function(string prefix, string name, ArrayList argumentList)
		{
			this.functionType = Function.FunctionType.FuncUserDefined;
			this.prefix = prefix;
			this.name = name;
			this.argumentList = new ArrayList(argumentList);
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0004FC13 File Offset: 0x0004EC13
		public Function(Function.FunctionType ftype)
		{
			this.functionType = ftype;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0004FC22 File Offset: 0x0004EC22
		public Function(Function.FunctionType ftype, AstNode arg)
		{
			this.functionType = ftype;
			this.argumentList = new ArrayList();
			this.argumentList.Add(arg);
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x0004FC49 File Offset: 0x0004EC49
		public override AstNode.AstType Type
		{
			get
			{
				return AstNode.AstType.Function;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x0600122D RID: 4653 RVA: 0x0004FC4C File Offset: 0x0004EC4C
		public override XPathResultType ReturnType
		{
			get
			{
				return Function.ReturnTypes[(int)this.functionType];
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600122E RID: 4654 RVA: 0x0004FC5A File Offset: 0x0004EC5A
		public Function.FunctionType TypeOfFunction
		{
			get
			{
				return this.functionType;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x0004FC62 File Offset: 0x0004EC62
		public ArrayList ArgumentList
		{
			get
			{
				return this.argumentList;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x0004FC6A File Offset: 0x0004EC6A
		public string Prefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001231 RID: 4657 RVA: 0x0004FC72 File Offset: 0x0004EC72
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x04000B65 RID: 2917
		private Function.FunctionType functionType;

		// Token: 0x04000B66 RID: 2918
		private ArrayList argumentList;

		// Token: 0x04000B67 RID: 2919
		private string name;

		// Token: 0x04000B68 RID: 2920
		private string prefix;

		// Token: 0x04000B69 RID: 2921
		internal static XPathResultType[] ReturnTypes = new XPathResultType[]
		{
			XPathResultType.Number,
			XPathResultType.Number,
			XPathResultType.Number,
			XPathResultType.NodeSet,
			XPathResultType.String,
			XPathResultType.String,
			XPathResultType.String,
			XPathResultType.String,
			XPathResultType.Boolean,
			XPathResultType.Number,
			XPathResultType.Boolean,
			XPathResultType.Boolean,
			XPathResultType.Boolean,
			XPathResultType.String,
			XPathResultType.Boolean,
			XPathResultType.Boolean,
			XPathResultType.String,
			XPathResultType.String,
			XPathResultType.String,
			XPathResultType.Number,
			XPathResultType.String,
			XPathResultType.String,
			XPathResultType.Boolean,
			XPathResultType.Number,
			XPathResultType.Number,
			XPathResultType.Number,
			XPathResultType.Number,
			XPathResultType.Any
		};

		// Token: 0x02000141 RID: 321
		public enum FunctionType
		{
			// Token: 0x04000B6B RID: 2923
			FuncLast,
			// Token: 0x04000B6C RID: 2924
			FuncPosition,
			// Token: 0x04000B6D RID: 2925
			FuncCount,
			// Token: 0x04000B6E RID: 2926
			FuncID,
			// Token: 0x04000B6F RID: 2927
			FuncLocalName,
			// Token: 0x04000B70 RID: 2928
			FuncNameSpaceUri,
			// Token: 0x04000B71 RID: 2929
			FuncName,
			// Token: 0x04000B72 RID: 2930
			FuncString,
			// Token: 0x04000B73 RID: 2931
			FuncBoolean,
			// Token: 0x04000B74 RID: 2932
			FuncNumber,
			// Token: 0x04000B75 RID: 2933
			FuncTrue,
			// Token: 0x04000B76 RID: 2934
			FuncFalse,
			// Token: 0x04000B77 RID: 2935
			FuncNot,
			// Token: 0x04000B78 RID: 2936
			FuncConcat,
			// Token: 0x04000B79 RID: 2937
			FuncStartsWith,
			// Token: 0x04000B7A RID: 2938
			FuncContains,
			// Token: 0x04000B7B RID: 2939
			FuncSubstringBefore,
			// Token: 0x04000B7C RID: 2940
			FuncSubstringAfter,
			// Token: 0x04000B7D RID: 2941
			FuncSubstring,
			// Token: 0x04000B7E RID: 2942
			FuncStringLength,
			// Token: 0x04000B7F RID: 2943
			FuncNormalize,
			// Token: 0x04000B80 RID: 2944
			FuncTranslate,
			// Token: 0x04000B81 RID: 2945
			FuncLang,
			// Token: 0x04000B82 RID: 2946
			FuncSum,
			// Token: 0x04000B83 RID: 2947
			FuncFloor,
			// Token: 0x04000B84 RID: 2948
			FuncCeiling,
			// Token: 0x04000B85 RID: 2949
			FuncRound,
			// Token: 0x04000B86 RID: 2950
			FuncUserDefined
		}
	}
}

using System;
using System.Globalization;

namespace System.Data
{
	// Token: 0x020001B1 RID: 433
	internal sealed class ExprException
	{
		// Token: 0x060018D2 RID: 6354 RVA: 0x0023BFD8 File Offset: 0x0023B3D8
		private ExprException()
		{
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x0023BFEC File Offset: 0x0023B3EC
		private static OverflowException _Overflow(string error)
		{
			OverflowException ex = new OverflowException(error);
			ExceptionBuilder.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x0023C008 File Offset: 0x0023B408
		private static InvalidExpressionException _Expr(string error)
		{
			InvalidExpressionException ex = new InvalidExpressionException(error);
			ExceptionBuilder.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x0023C024 File Offset: 0x0023B424
		private static SyntaxErrorException _Syntax(string error)
		{
			SyntaxErrorException ex = new SyntaxErrorException(error);
			ExceptionBuilder.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0023C040 File Offset: 0x0023B440
		private static EvaluateException _Eval(string error)
		{
			EvaluateException ex = new EvaluateException(error);
			ExceptionBuilder.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0023C05C File Offset: 0x0023B45C
		private static EvaluateException _Eval(string error, Exception innerException)
		{
			EvaluateException ex = new EvaluateException(error);
			ExceptionBuilder.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0023C078 File Offset: 0x0023B478
		public static Exception InvokeArgument()
		{
			return ExceptionBuilder._Argument(Res.GetString("Expr_InvokeArgument"));
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x0023C094 File Offset: 0x0023B494
		public static Exception NYI(string moreinfo)
		{
			string @string = Res.GetString("Expr_NYI", new object[] { moreinfo });
			return ExprException._Expr(@string);
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x0023C0C0 File Offset: 0x0023B4C0
		public static Exception MissingOperand(OperatorInfo before)
		{
			return ExprException._Syntax(Res.GetString("Expr_MissingOperand", new object[] { Operators.ToString(before.op) }));
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x0023C0F4 File Offset: 0x0023B4F4
		public static Exception MissingOperator(string token)
		{
			return ExprException._Syntax(Res.GetString("Expr_MissingOperand", new object[] { token }));
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x0023C11C File Offset: 0x0023B51C
		public static Exception TypeMismatch(string expr)
		{
			return ExprException._Eval(Res.GetString("Expr_TypeMismatch", new object[] { expr }));
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x0023C144 File Offset: 0x0023B544
		public static Exception FunctionArgumentOutOfRange(string arg, string func)
		{
			return ExceptionBuilder._ArgumentOutOfRange(arg, Res.GetString("Expr_ArgumentOutofRange", new object[] { func }));
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x0023C170 File Offset: 0x0023B570
		public static Exception ExpressionTooComplex()
		{
			return ExprException._Eval(Res.GetString("Expr_ExpressionTooComplex"));
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x0023C18C File Offset: 0x0023B58C
		public static Exception UnboundName(string name)
		{
			return ExprException._Eval(Res.GetString("Expr_UnboundName", new object[] { name }));
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x0023C1B4 File Offset: 0x0023B5B4
		public static Exception InvalidString(string str)
		{
			return ExprException._Syntax(Res.GetString("Expr_InvalidString", new object[] { str }));
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x0023C1DC File Offset: 0x0023B5DC
		public static Exception UndefinedFunction(string name)
		{
			return ExprException._Eval(Res.GetString("Expr_UndefinedFunction", new object[] { name }));
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x0023C204 File Offset: 0x0023B604
		public static Exception SyntaxError()
		{
			return ExprException._Syntax(Res.GetString("Expr_Syntax"));
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x0023C220 File Offset: 0x0023B620
		public static Exception FunctionArgumentCount(string name)
		{
			return ExprException._Eval(Res.GetString("Expr_FunctionArgumentCount", new object[] { name }));
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x0023C248 File Offset: 0x0023B648
		public static Exception MissingRightParen()
		{
			return ExprException._Syntax(Res.GetString("Expr_MissingRightParen"));
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x0023C264 File Offset: 0x0023B664
		public static Exception UnknownToken(string token, int position)
		{
			return ExprException._Syntax(Res.GetString("Expr_UnknownToken", new object[]
			{
				token,
				position.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x0023C29C File Offset: 0x0023B69C
		public static Exception UnknownToken(Tokens tokExpected, Tokens tokCurr, int position)
		{
			return ExprException._Syntax(Res.GetString("Expr_UnknownToken1", new object[]
			{
				tokExpected.ToString(),
				tokCurr.ToString(),
				position.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x0023C2EC File Offset: 0x0023B6EC
		public static Exception DatatypeConvertion(Type type1, Type type2)
		{
			return ExprException._Eval(Res.GetString("Expr_DatatypeConvertion", new object[]
			{
				type1.ToString(),
				type2.ToString()
			}));
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x0023C324 File Offset: 0x0023B724
		public static Exception DatavalueConvertion(object value, Type type, Exception innerException)
		{
			return ExprException._Eval(Res.GetString("Expr_DatavalueConvertion", new object[]
			{
				value.ToString(),
				type.ToString()
			}), innerException);
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x0023C35C File Offset: 0x0023B75C
		public static Exception InvalidName(string name)
		{
			return ExprException._Syntax(Res.GetString("Expr_InvalidName", new object[] { name }));
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x0023C384 File Offset: 0x0023B784
		public static Exception InvalidDate(string date)
		{
			return ExprException._Syntax(Res.GetString("Expr_InvalidDate", new object[] { date }));
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x0023C3AC File Offset: 0x0023B7AC
		public static Exception NonConstantArgument()
		{
			return ExprException._Eval(Res.GetString("Expr_NonConstantArgument"));
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x0023C3C8 File Offset: 0x0023B7C8
		public static Exception InvalidPattern(string pat)
		{
			return ExprException._Eval(Res.GetString("Expr_InvalidPattern", new object[] { pat }));
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x0023C3F0 File Offset: 0x0023B7F0
		public static Exception InWithoutParentheses()
		{
			return ExprException._Syntax(Res.GetString("Expr_InWithoutParentheses"));
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x0023C40C File Offset: 0x0023B80C
		public static Exception InWithoutList()
		{
			return ExprException._Syntax(Res.GetString("Expr_InWithoutList"));
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x0023C428 File Offset: 0x0023B828
		public static Exception InvalidIsSyntax()
		{
			return ExprException._Syntax(Res.GetString("Expr_IsSyntax"));
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x0023C444 File Offset: 0x0023B844
		public static Exception Overflow(Type type)
		{
			return ExprException._Overflow(Res.GetString("Expr_Overflow", new object[] { type.Name }));
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x0023C474 File Offset: 0x0023B874
		public static Exception ArgumentType(string function, int arg, Type type)
		{
			return ExprException._Eval(Res.GetString("Expr_ArgumentType", new object[]
			{
				function,
				arg.ToString(CultureInfo.InvariantCulture),
				type.ToString()
			}));
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x0023C4B4 File Offset: 0x0023B8B4
		public static Exception ArgumentTypeInteger(string function, int arg)
		{
			return ExprException._Eval(Res.GetString("Expr_ArgumentTypeInteger", new object[]
			{
				function,
				arg.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x0023C4EC File Offset: 0x0023B8EC
		public static Exception TypeMismatchInBinop(int op, Type type1, Type type2)
		{
			return ExprException._Eval(Res.GetString("Expr_TypeMismatchInBinop", new object[]
			{
				Operators.ToString(op),
				type1.ToString(),
				type2.ToString()
			}));
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x0023C52C File Offset: 0x0023B92C
		public static Exception AmbiguousBinop(int op, Type type1, Type type2)
		{
			return ExprException._Eval(Res.GetString("Expr_AmbiguousBinop", new object[]
			{
				Operators.ToString(op),
				type1.ToString(),
				type2.ToString()
			}));
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x0023C56C File Offset: 0x0023B96C
		public static Exception UnsupportedOperator(int op)
		{
			return ExprException._Eval(Res.GetString("Expr_UnsupportedOperator", new object[] { Operators.ToString(op) }));
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x0023C59C File Offset: 0x0023B99C
		public static Exception InvalidNameBracketing(string name)
		{
			return ExprException._Syntax(Res.GetString("Expr_InvalidNameBracketing", new object[] { name }));
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x0023C5C4 File Offset: 0x0023B9C4
		public static Exception MissingOperandBefore(string op)
		{
			return ExprException._Syntax(Res.GetString("Expr_MissingOperandBefore", new object[] { op }));
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x0023C5EC File Offset: 0x0023B9EC
		public static Exception TooManyRightParentheses()
		{
			return ExprException._Syntax(Res.GetString("Expr_TooManyRightParentheses"));
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x0023C608 File Offset: 0x0023BA08
		public static Exception UnresolvedRelation(string name, string expr)
		{
			return ExprException._Eval(Res.GetString("Expr_UnresolvedRelation", new object[] { name, expr }));
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x0023C634 File Offset: 0x0023BA34
		internal static EvaluateException BindFailure(string relationName)
		{
			return ExprException._Eval(Res.GetString("Expr_BindFailure", new object[] { relationName }));
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x0023C65C File Offset: 0x0023BA5C
		public static Exception AggregateArgument()
		{
			return ExprException._Syntax(Res.GetString("Expr_AggregateArgument"));
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x0023C678 File Offset: 0x0023BA78
		public static Exception AggregateUnbound(string expr)
		{
			return ExprException._Eval(Res.GetString("Expr_AggregateUnbound", new object[] { expr }));
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x0023C6A0 File Offset: 0x0023BAA0
		public static Exception EvalNoContext()
		{
			return ExprException._Eval(Res.GetString("Expr_EvalNoContext"));
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x0023C6BC File Offset: 0x0023BABC
		public static Exception ExpressionUnbound(string expr)
		{
			return ExprException._Eval(Res.GetString("Expr_ExpressionUnbound", new object[] { expr }));
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x0023C6E4 File Offset: 0x0023BAE4
		public static Exception ComputeNotAggregate(string expr)
		{
			return ExprException._Eval(Res.GetString("Expr_ComputeNotAggregate", new object[] { expr }));
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x0023C70C File Offset: 0x0023BB0C
		public static Exception FilterConvertion(string expr)
		{
			return ExprException._Eval(Res.GetString("Expr_FilterConvertion", new object[] { expr }));
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0023C734 File Offset: 0x0023BB34
		public static Exception LookupArgument()
		{
			return ExprException._Syntax(Res.GetString("Expr_LookupArgument"));
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0023C750 File Offset: 0x0023BB50
		public static Exception InvalidType(string typeName)
		{
			return ExprException._Eval(Res.GetString("Expr_InvalidType", new object[] { typeName }));
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x0023C778 File Offset: 0x0023BB78
		public static Exception InvalidHoursArgument()
		{
			return ExprException._Eval(Res.GetString("Expr_InvalidHoursArgument"));
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x0023C794 File Offset: 0x0023BB94
		public static Exception InvalidMinutesArgument()
		{
			return ExprException._Eval(Res.GetString("Expr_InvalidMinutesArgument"));
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x0023C7B0 File Offset: 0x0023BBB0
		public static Exception InvalidTimeZoneRange()
		{
			return ExprException._Eval(Res.GetString("Expr_InvalidTimeZoneRange"));
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x0023C7CC File Offset: 0x0023BBCC
		public static Exception MismatchKindandTimeSpan()
		{
			return ExprException._Eval(Res.GetString("Expr_MismatchKindandTimeSpan"));
		}
	}
}

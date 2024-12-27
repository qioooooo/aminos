using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Web.Configuration
{
	// Token: 0x0200024E RID: 590
	internal static class StdValidatorsAndConverters
	{
		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001F37 RID: 7991 RVA: 0x0008ABDC File Offset: 0x00089BDC
		internal static TypeConverter InfiniteTimeSpanConverter
		{
			get
			{
				if (StdValidatorsAndConverters.s_infiniteTimeSpanConverter == null)
				{
					StdValidatorsAndConverters.s_infiniteTimeSpanConverter = new InfiniteTimeSpanConverter();
				}
				return StdValidatorsAndConverters.s_infiniteTimeSpanConverter;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x0008ABF4 File Offset: 0x00089BF4
		internal static TypeConverter TimeSpanMinutesConverter
		{
			get
			{
				if (StdValidatorsAndConverters.s_timeSpanMinutesConverter == null)
				{
					StdValidatorsAndConverters.s_timeSpanMinutesConverter = new TimeSpanMinutesConverter();
				}
				return StdValidatorsAndConverters.s_timeSpanMinutesConverter;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06001F39 RID: 7993 RVA: 0x0008AC0C File Offset: 0x00089C0C
		internal static TypeConverter TimeSpanMinutesOrInfiniteConverter
		{
			get
			{
				if (StdValidatorsAndConverters.s_timeSpanMinutesOrInfiniteConverter == null)
				{
					StdValidatorsAndConverters.s_timeSpanMinutesOrInfiniteConverter = new TimeSpanMinutesOrInfiniteConverter();
				}
				return StdValidatorsAndConverters.s_timeSpanMinutesOrInfiniteConverter;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001F3A RID: 7994 RVA: 0x0008AC24 File Offset: 0x00089C24
		internal static TypeConverter TimeSpanSecondsConverter
		{
			get
			{
				if (StdValidatorsAndConverters.s_timeSpanSecondsConverter == null)
				{
					StdValidatorsAndConverters.s_timeSpanSecondsConverter = new TimeSpanSecondsConverter();
				}
				return StdValidatorsAndConverters.s_timeSpanSecondsConverter;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001F3B RID: 7995 RVA: 0x0008AC3C File Offset: 0x00089C3C
		internal static TypeConverter TimeSpanSecondsOrInfiniteConverter
		{
			get
			{
				/*
An exception occurred when decompiling this method (06001F3B)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.ComponentModel.TypeConverter System.Web.Configuration.StdValidatorsAndConverters::get_TimeSpanSecondsOrInfiniteConverter()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.BuildGraph(List`1 nodes, ILLabel entryLabel) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 98
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 65
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 351
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x0008AC54 File Offset: 0x00089C54
		internal static TypeConverter WhiteSpaceTrimStringConverter
		{
			get
			{
				if (StdValidatorsAndConverters.s_whiteSpaceTrimStringConverter == null)
				{
					StdValidatorsAndConverters.s_whiteSpaceTrimStringConverter = new WhiteSpaceTrimStringConverter();
				}
				return StdValidatorsAndConverters.s_whiteSpaceTrimStringConverter;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001F3D RID: 7997 RVA: 0x0008AC6C File Offset: 0x00089C6C
		internal static ConfigurationValidatorBase PositiveTimeSpanValidator
		{
			get
			{
				if (StdValidatorsAndConverters.s_positiveTimeSpanValidator == null)
				{
					StdValidatorsAndConverters.s_positiveTimeSpanValidator = new PositiveTimeSpanValidator();
				}
				return StdValidatorsAndConverters.s_positiveTimeSpanValidator;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001F3E RID: 7998 RVA: 0x0008AC84 File Offset: 0x00089C84
		internal static ConfigurationValidatorBase NonEmptyStringValidator
		{
			get
			{
				if (StdValidatorsAndConverters.s_nonEmptyStringValidator == null)
				{
					StdValidatorsAndConverters.s_nonEmptyStringValidator = new StringValidator(1);
				}
				return StdValidatorsAndConverters.s_nonEmptyStringValidator;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001F3F RID: 7999 RVA: 0x0008AC9D File Offset: 0x00089C9D
		internal static ConfigurationValidatorBase NonZeroPositiveIntegerValidator
		{
			get
			{
				if (StdValidatorsAndConverters.s_nonZeroPositiveIntegerValidator == null)
				{
					StdValidatorsAndConverters.s_nonZeroPositiveIntegerValidator = new IntegerValidator(1, int.MaxValue);
				}
				return StdValidatorsAndConverters.s_nonZeroPositiveIntegerValidator;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001F40 RID: 8000 RVA: 0x0008ACBB File Offset: 0x00089CBB
		internal static ConfigurationValidatorBase PositiveIntegerValidator
		{
			get
			{
				if (StdValidatorsAndConverters.s_positiveIntegerValidator == null)
				{
					StdValidatorsAndConverters.s_positiveIntegerValidator = new IntegerValidator(0, int.MaxValue);
				}
				return StdValidatorsAndConverters.s_positiveIntegerValidator;
			}
		}

		// Token: 0x04001A48 RID: 6728
		private static TypeConverter s_infiniteTimeSpanConverter;

		// Token: 0x04001A49 RID: 6729
		private static TypeConverter s_timeSpanMinutesConverter;

		// Token: 0x04001A4A RID: 6730
		private static TypeConverter s_timeSpanMinutesOrInfiniteConverter;

		// Token: 0x04001A4B RID: 6731
		private static TypeConverter s_timeSpanSecondsConverter;

		// Token: 0x04001A4C RID: 6732
		private static TypeConverter s_timeSpanSecondsOrInfiniteConverter;

		// Token: 0x04001A4D RID: 6733
		private static TypeConverter s_whiteSpaceTrimStringConverter;

		// Token: 0x04001A4E RID: 6734
		private static ConfigurationValidatorBase s_positiveTimeSpanValidator;

		// Token: 0x04001A4F RID: 6735
		private static ConfigurationValidatorBase s_nonEmptyStringValidator;

		// Token: 0x04001A50 RID: 6736
		private static ConfigurationValidatorBase s_nonZeroPositiveIntegerValidator;

		// Token: 0x04001A51 RID: 6737
		private static ConfigurationValidatorBase s_positiveIntegerValidator;
	}
}

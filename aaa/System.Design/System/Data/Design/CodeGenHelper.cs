using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data.SqlTypes;
using System.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace System.Data.Design
{
	// Token: 0x02000067 RID: 103
	internal sealed class CodeGenHelper
	{
		// Token: 0x0600043A RID: 1082 RVA: 0x00002EFE File Offset: 0x00001EFE
		private CodeGenHelper()
		{
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00002F06 File Offset: 0x00001F06
		internal static CodeExpression This()
		{
			return new CodeThisReferenceExpression();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00002F0D File Offset: 0x00001F0D
		internal static CodeExpression Base()
		{
			return new CodeBaseReferenceExpression();
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00002F14 File Offset: 0x00001F14
		internal static CodeExpression Value()
		{
			return new CodePropertySetValueReferenceExpression();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00002F1B File Offset: 0x00001F1B
		internal static CodeTypeReference Type(string type)
		{
			return new CodeTypeReference(type);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00002F23 File Offset: 0x00001F23
		internal static CodeTypeReference Type(Type type)
		{
			return new CodeTypeReference(type);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00002F2C File Offset: 0x00001F2C
		internal static CodeTypeReference NullableType(Type type)
		{
			return new CodeTypeReference(typeof(Nullable))
			{
				Options = CodeTypeReferenceOptions.GlobalReference,
				TypeArguments = { CodeGenHelper.GlobalType(type) }
			};
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00002F63 File Offset: 0x00001F63
		internal static CodeTypeReference Type(string type, int rank)
		{
			return new CodeTypeReference(type, rank);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00002F6C File Offset: 0x00001F6C
		internal static CodeTypeReference GlobalType(Type type)
		{
			return new CodeTypeReference(type.ToString(), CodeTypeReferenceOptions.GlobalReference);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00002F7A File Offset: 0x00001F7A
		internal static CodeTypeReference GlobalType(Type type, int rank)
		{
			return new CodeTypeReference(CodeGenHelper.GlobalType(type), rank);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00002F88 File Offset: 0x00001F88
		internal static CodeTypeReference GlobalType(string type)
		{
			return new CodeTypeReference(type, CodeTypeReferenceOptions.GlobalReference);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00002F91 File Offset: 0x00001F91
		internal static CodeTypeReferenceExpression TypeExpr(CodeTypeReference type)
		{
			return new CodeTypeReferenceExpression(type);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00002F99 File Offset: 0x00001F99
		internal static CodeTypeReferenceExpression GlobalTypeExpr(Type type)
		{
			return new CodeTypeReferenceExpression(CodeGenHelper.GlobalType(type));
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00002FA6 File Offset: 0x00001FA6
		internal static CodeTypeReferenceExpression GlobalTypeExpr(string type)
		{
			return new CodeTypeReferenceExpression(CodeGenHelper.GlobalType(type));
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00002FB3 File Offset: 0x00001FB3
		internal static CodeTypeReference GlobalGenericType(string fullTypeName, Type itemType)
		{
			return CodeGenHelper.GlobalGenericType(fullTypeName, CodeGenHelper.GlobalType(itemType));
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00002FC4 File Offset: 0x00001FC4
		internal static CodeTypeReference GlobalGenericType(string fullTypeName, CodeTypeReference itemType)
		{
			return new CodeTypeReference(fullTypeName, new CodeTypeReference[] { itemType })
			{
				Options = CodeTypeReferenceOptions.GlobalReference
			};
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00002FEC File Offset: 0x00001FEC
		internal static CodeExpression Cast(CodeTypeReference type, CodeExpression expr)
		{
			return new CodeCastExpression(type, expr);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00002FF5 File Offset: 0x00001FF5
		internal static CodeExpression TypeOf(CodeTypeReference type)
		{
			return new CodeTypeOfExpression(type);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00002FFD File Offset: 0x00001FFD
		internal static CodeExpression Field(CodeExpression exp, string field)
		{
			return new CodeFieldReferenceExpression(exp, field);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00003006 File Offset: 0x00002006
		internal static CodeExpression ThisField(string field)
		{
			return new CodeFieldReferenceExpression(CodeGenHelper.This(), field);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00003013 File Offset: 0x00002013
		internal static CodeExpression Property(CodeExpression exp, string property)
		{
			return new CodePropertyReferenceExpression(exp, property);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000301C File Offset: 0x0000201C
		internal static CodeExpression ThisProperty(string property)
		{
			return new CodePropertyReferenceExpression(CodeGenHelper.This(), property);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00003029 File Offset: 0x00002029
		internal static CodeExpression Argument(string argument)
		{
			return new CodeArgumentReferenceExpression(argument);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00003031 File Offset: 0x00002031
		internal static CodeExpression Variable(string variable)
		{
			return new CodeVariableReferenceExpression(variable);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00003039 File Offset: 0x00002039
		internal static CodeExpression Event(string eventName)
		{
			return new CodeEventReferenceExpression(CodeGenHelper.This(), eventName);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00003046 File Offset: 0x00002046
		internal static CodeExpression New(CodeTypeReference type, CodeExpression[] parameters)
		{
			return new CodeObjectCreateExpression(type, parameters);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000304F File Offset: 0x0000204F
		internal static CodeExpression NewArray(CodeTypeReference type, int size)
		{
			return new CodeArrayCreateExpression(type, size);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00003058 File Offset: 0x00002058
		internal static CodeExpression NewArray(CodeTypeReference type, params CodeExpression[] initializers)
		{
			return new CodeArrayCreateExpression(type, initializers);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00003061 File Offset: 0x00002061
		internal static CodeExpression Primitive(object primitive)
		{
			return new CodePrimitiveExpression(primitive);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00003069 File Offset: 0x00002069
		internal static CodeExpression Str(string str)
		{
			return CodeGenHelper.Primitive(str);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00003071 File Offset: 0x00002071
		internal static CodeExpression MethodCall(CodeExpression targetObject, string methodName, CodeExpression[] parameters)
		{
			return new CodeMethodInvokeExpression(targetObject, methodName, parameters);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000307B File Offset: 0x0000207B
		internal static CodeStatement MethodCallStm(CodeExpression targetObject, string methodName, CodeExpression[] parameters)
		{
			return CodeGenHelper.Stm(CodeGenHelper.MethodCall(targetObject, methodName, parameters));
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000308A File Offset: 0x0000208A
		internal static CodeExpression MethodCall(CodeExpression targetObject, string methodName)
		{
			return new CodeMethodInvokeExpression(targetObject, methodName, new CodeExpression[0]);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00003099 File Offset: 0x00002099
		internal static CodeStatement MethodCallStm(CodeExpression targetObject, string methodName)
		{
			return CodeGenHelper.Stm(CodeGenHelper.MethodCall(targetObject, methodName));
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000030A8 File Offset: 0x000020A8
		internal static CodeExpression MethodCall(CodeExpression targetObject, string methodName, CodeExpression par)
		{
			return new CodeMethodInvokeExpression(targetObject, methodName, new CodeExpression[] { par });
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000030C8 File Offset: 0x000020C8
		internal static CodeStatement MethodCallStm(CodeExpression targetObject, string methodName, CodeExpression par)
		{
			return CodeGenHelper.Stm(CodeGenHelper.MethodCall(targetObject, methodName, par));
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000030D8 File Offset: 0x000020D8
		internal static CodeExpression DelegateCall(CodeExpression targetObject, CodeExpression par)
		{
			return new CodeDelegateInvokeExpression(targetObject, new CodeExpression[]
			{
				CodeGenHelper.This(),
				par
			});
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00003100 File Offset: 0x00002100
		internal static CodeExpression Indexer(CodeExpression targetObject, CodeExpression indices)
		{
			return new CodeIndexerExpression(targetObject, new CodeExpression[] { indices });
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00003120 File Offset: 0x00002120
		internal static CodeExpression ArrayIndexer(CodeExpression targetObject, CodeExpression indices)
		{
			return new CodeArrayIndexerExpression(targetObject, new CodeExpression[] { indices });
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00003140 File Offset: 0x00002140
		internal static CodeExpression ReferenceEquals(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.MethodCall(CodeGenHelper.GlobalTypeExpr(typeof(object)), "ReferenceEquals", new CodeExpression[] { left, right });
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00003176 File Offset: 0x00002176
		internal static CodeExpression ReferenceNotEquals(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.EQ(CodeGenHelper.ReferenceEquals(left, right), CodeGenHelper.Primitive(false));
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000318F File Offset: 0x0000218F
		internal static CodeBinaryOperatorExpression BinOperator(CodeExpression left, CodeBinaryOperatorType op, CodeExpression right)
		{
			return new CodeBinaryOperatorExpression(left, op, right);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00003199 File Offset: 0x00002199
		internal static CodeBinaryOperatorExpression IdNotEQ(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.IdentityInequality, right);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000031A3 File Offset: 0x000021A3
		internal static CodeBinaryOperatorExpression IdEQ(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.IdentityEquality, right);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000031AD File Offset: 0x000021AD
		internal static CodeBinaryOperatorExpression IdIsNull(CodeExpression id)
		{
			return CodeGenHelper.IdEQ(id, CodeGenHelper.Primitive(null));
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000031BB File Offset: 0x000021BB
		internal static CodeBinaryOperatorExpression IdIsNotNull(CodeExpression id)
		{
			return CodeGenHelper.IdNotEQ(id, CodeGenHelper.Primitive(null));
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000031C9 File Offset: 0x000021C9
		internal static CodeBinaryOperatorExpression EQ(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.ValueEquality, right);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000031D3 File Offset: 0x000021D3
		internal static CodeBinaryOperatorExpression NotEQ(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.EQ(CodeGenHelper.EQ(left, right), CodeGenHelper.Primitive(false));
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000031EC File Offset: 0x000021EC
		internal static CodeBinaryOperatorExpression BitwiseAnd(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.BitwiseAnd, right);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000031F7 File Offset: 0x000021F7
		internal static CodeBinaryOperatorExpression And(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.BooleanAnd, right);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00003202 File Offset: 0x00002202
		internal static CodeBinaryOperatorExpression Or(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.BooleanOr, right);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000320D File Offset: 0x0000220D
		internal static CodeBinaryOperatorExpression Less(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.LessThan, right);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00003218 File Offset: 0x00002218
		internal static CodeStatement Stm(CodeExpression expr)
		{
			return new CodeExpressionStatement(expr);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00003220 File Offset: 0x00002220
		internal static CodeStatement Return(CodeExpression expr)
		{
			return new CodeMethodReturnStatement(expr);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00003228 File Offset: 0x00002228
		internal static CodeStatement Return()
		{
			return new CodeMethodReturnStatement();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000322F File Offset: 0x0000222F
		internal static CodeStatement Assign(CodeExpression left, CodeExpression right)
		{
			return new CodeAssignStatement(left, right);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00003238 File Offset: 0x00002238
		internal static CodeStatement Throw(CodeTypeReference exception, string arg)
		{
			return new CodeThrowExceptionStatement(CodeGenHelper.New(exception, new CodeExpression[] { CodeGenHelper.Str(arg) }));
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00003264 File Offset: 0x00002264
		internal static CodeStatement Throw(CodeTypeReference exception, string arg, string inner)
		{
			return new CodeThrowExceptionStatement(CodeGenHelper.New(exception, new CodeExpression[]
			{
				CodeGenHelper.Str(arg),
				CodeGenHelper.Variable(inner)
			}));
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00003298 File Offset: 0x00002298
		internal static CodeStatement Throw(CodeTypeReference exception, string arg, CodeExpression inner)
		{
			return new CodeThrowExceptionStatement(CodeGenHelper.New(exception, new CodeExpression[]
			{
				CodeGenHelper.Str(arg),
				inner
			}));
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000032C5 File Offset: 0x000022C5
		internal static CodeCommentStatement Comment(string comment, bool docSummary)
		{
			if (docSummary)
			{
				return new CodeCommentStatement("<summary>\r\n" + comment + "\r\n</summary>", docSummary);
			}
			return new CodeCommentStatement(comment);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000032E7 File Offset: 0x000022E7
		internal static CodeStatement If(CodeExpression cond, CodeStatement[] trueStms, CodeStatement[] falseStms)
		{
			return new CodeConditionStatement(cond, trueStms, falseStms);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x000032F4 File Offset: 0x000022F4
		internal static CodeStatement If(CodeExpression cond, CodeStatement trueStm, CodeStatement falseStm)
		{
			return new CodeConditionStatement(cond, new CodeStatement[] { trueStm }, new CodeStatement[] { falseStm });
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000331F File Offset: 0x0000231F
		internal static CodeStatement If(CodeExpression cond, CodeStatement[] trueStms)
		{
			return new CodeConditionStatement(cond, trueStms);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00003328 File Offset: 0x00002328
		internal static CodeStatement If(CodeExpression cond, CodeStatement trueStm)
		{
			return CodeGenHelper.If(cond, new CodeStatement[] { trueStm });
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00003347 File Offset: 0x00002347
		internal static CodeMemberField FieldDecl(CodeTypeReference type, string name)
		{
			return new CodeMemberField(type, name);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00003350 File Offset: 0x00002350
		internal static CodeMemberField FieldDecl(CodeTypeReference type, string name, CodeExpression initExpr)
		{
			return new CodeMemberField(type, name)
			{
				InitExpression = initExpr
			};
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00003370 File Offset: 0x00002370
		internal static CodeTypeDeclaration Class(string name, bool isPartial, TypeAttributes typeAttributes)
		{
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(name);
			codeTypeDeclaration.IsPartial = isPartial;
			codeTypeDeclaration.TypeAttributes = typeAttributes;
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(CodeGenHelper.GlobalType(typeof(GeneratedCodeAttribute)), new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(CodeGenHelper.Str(typeof(TypedDataSetGenerator).FullName)),
				new CodeAttributeArgument(CodeGenHelper.Str("2.0.0.0"))
			});
			codeTypeDeclaration.CustomAttributes.Add(codeAttributeDeclaration);
			return codeTypeDeclaration;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000033EC File Offset: 0x000023EC
		internal static CodeConstructor Constructor(MemberAttributes attributes)
		{
			return new CodeConstructor
			{
				Attributes = attributes,
				CustomAttributes = { CodeGenHelper.AttributeDecl(typeof(DebuggerNonUserCodeAttribute).FullName) }
			};
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00003428 File Offset: 0x00002428
		internal static CodeMemberMethod MethodDecl(CodeTypeReference type, string name, MemberAttributes attributes)
		{
			return new CodeMemberMethod
			{
				ReturnType = type,
				Name = name,
				Attributes = attributes,
				CustomAttributes = { CodeGenHelper.AttributeDecl(typeof(DebuggerNonUserCodeAttribute).FullName) }
			};
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00003474 File Offset: 0x00002474
		internal static CodeMemberProperty PropertyDecl(CodeTypeReference type, string name, MemberAttributes attributes)
		{
			return new CodeMemberProperty
			{
				Type = type,
				Name = name,
				Attributes = attributes,
				CustomAttributes = { CodeGenHelper.AttributeDecl(typeof(DebuggerNonUserCodeAttribute).FullName) }
			};
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x000034BD File Offset: 0x000024BD
		internal static CodeStatement VariableDecl(CodeTypeReference type, string name)
		{
			return new CodeVariableDeclarationStatement(type, name);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000034C6 File Offset: 0x000024C6
		internal static CodeStatement VariableDecl(CodeTypeReference type, string name, CodeExpression initExpr)
		{
			return new CodeVariableDeclarationStatement(type, name, initExpr);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000034D0 File Offset: 0x000024D0
		internal static CodeStatement ForLoop(CodeStatement initStmt, CodeExpression testExpression, CodeStatement incrementStmt, CodeStatement[] statements)
		{
			return new CodeIterationStatement(initStmt, testExpression, incrementStmt, statements);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x000034DC File Offset: 0x000024DC
		internal static CodeMemberEvent EventDecl(string type, string name)
		{
			return new CodeMemberEvent
			{
				Name = name,
				Type = CodeGenHelper.Type(type),
				Attributes = (MemberAttributes)24578
			};
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000350E File Offset: 0x0000250E
		internal static CodeParameterDeclarationExpression ParameterDecl(CodeTypeReference type, string name)
		{
			return new CodeParameterDeclarationExpression(type, name);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00003517 File Offset: 0x00002517
		internal static CodeAttributeDeclaration AttributeDecl(string name)
		{
			return new CodeAttributeDeclaration(CodeGenHelper.GlobalType(name));
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00003524 File Offset: 0x00002524
		internal static CodeAttributeDeclaration AttributeDecl(string name, CodeExpression value)
		{
			return new CodeAttributeDeclaration(CodeGenHelper.GlobalType(name), new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(value)
			});
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00003550 File Offset: 0x00002550
		internal static CodeAttributeDeclaration AttributeDecl(string name, CodeExpression value1, CodeExpression value2)
		{
			return new CodeAttributeDeclaration(CodeGenHelper.GlobalType(name), new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(value1),
				new CodeAttributeArgument(value2)
			});
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00003584 File Offset: 0x00002584
		internal static CodeStatement Try(CodeStatement tryStmnt, CodeCatchClause catchClause)
		{
			return new CodeTryCatchFinallyStatement(new CodeStatement[] { tryStmnt }, new CodeCatchClause[] { catchClause });
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000035AE File Offset: 0x000025AE
		internal static CodeStatement Try(CodeStatement[] tryStmnts, CodeCatchClause[] catchClauses, CodeStatement[] finallyStmnts)
		{
			return new CodeTryCatchFinallyStatement(tryStmnts, catchClauses, finallyStmnts);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000035B8 File Offset: 0x000025B8
		internal static CodeCatchClause Catch(CodeTypeReference type, string name, CodeStatement catchStmnt)
		{
			CodeCatchClause codeCatchClause = new CodeCatchClause();
			codeCatchClause.CatchExceptionType = type;
			codeCatchClause.LocalName = name;
			if (catchStmnt != null)
			{
				codeCatchClause.Statements.Add(catchStmnt);
			}
			return codeCatchClause;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x000035EC File Offset: 0x000025EC
		internal static FieldDirection ParameterDirectionToFieldDirection(ParameterDirection paramDirection)
		{
			switch (paramDirection)
			{
			case ParameterDirection.Input:
				return FieldDirection.In;
			case ParameterDirection.Output:
				return FieldDirection.Out;
			case ParameterDirection.InputOutput:
				return FieldDirection.Ref;
			case ParameterDirection.ReturnValue:
				throw new InternalException("Can't map from ParameterDirection.ReturnValue to FieldDirection.");
			}
			throw new InternalException("Unknown ParameterDirection.");
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00003642 File Offset: 0x00002642
		internal static CodeExpression GenerateDbNullCheck(CodeExpression returnParam)
		{
			return CodeGenHelper.Or(CodeGenHelper.IdEQ(returnParam, CodeGenHelper.Primitive(null)), CodeGenHelper.IdEQ(CodeGenHelper.MethodCall(returnParam, "GetType"), CodeGenHelper.TypeOf(CodeGenHelper.GlobalType(typeof(DBNull)))));
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000367C File Offset: 0x0000267C
		internal static CodeExpression GenerateNullExpression(Type returnType)
		{
			if (CodeGenHelper.IsSqlType(returnType))
			{
				return CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(returnType), "Null");
			}
			if (returnType == typeof(object))
			{
				return CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DBNull)), "Value");
			}
			if (!returnType.IsValueType)
			{
				return CodeGenHelper.Primitive(null);
			}
			return null;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000036DC File Offset: 0x000026DC
		internal static CodeExpression GenerateConvertExpression(CodeExpression sourceExpression, Type sourceType, Type targetType)
		{
			if (sourceType == targetType)
			{
				return sourceExpression;
			}
			if (CodeGenHelper.IsSqlType(sourceType))
			{
				if (CodeGenHelper.IsSqlType(targetType))
				{
					throw new InternalException("Cannot perform the conversion between 2 SqlTypes.");
				}
				PropertyInfo property = sourceType.GetProperty("Value");
				if (property == null)
				{
					throw new InternalException("Type does not expose a 'Value' property.");
				}
				Type propertyType = property.PropertyType;
				CodeExpression codeExpression = new CodePropertyReferenceExpression(sourceExpression, "Value");
				return CodeGenHelper.GenerateUrtConvertExpression(codeExpression, propertyType, targetType);
			}
			else
			{
				if (CodeGenHelper.IsSqlType(targetType))
				{
					PropertyInfo property2 = targetType.GetProperty("Value");
					Type propertyType2 = property2.PropertyType;
					CodeExpression codeExpression2 = CodeGenHelper.GenerateUrtConvertExpression(sourceExpression, sourceType, propertyType2);
					return new CodeObjectCreateExpression(targetType, new CodeExpression[] { codeExpression2 });
				}
				return CodeGenHelper.GenerateUrtConvertExpression(sourceExpression, sourceType, targetType);
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00003788 File Offset: 0x00002788
		internal static string GetTypeName(CodeDomProvider codeProvider, string string1, string string2)
		{
			string typeOutput = codeProvider.GetTypeOutput(CodeGenHelper.Type(typeof(Activator)));
			string text = typeOutput.Replace("System", "").Replace("Activator", "");
			return string1 + text + string2;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x000037D4 File Offset: 0x000027D4
		internal static bool SupportsMultipleNamespaces(CodeDomProvider codeProvider)
		{
			string text = MemberNameValidator.GenerateIdName("TestNs1", codeProvider, false);
			string text2 = MemberNameValidator.GenerateIdName("TestNs2", codeProvider, false);
			CodeNamespace codeNamespace = new CodeNamespace(text);
			CodeNamespace codeNamespace2 = new CodeNamespace(text2);
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
			codeCompileUnit.Namespaces.Add(codeNamespace);
			codeCompileUnit.Namespaces.Add(codeNamespace2);
			StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			codeProvider.GenerateCodeFromCompileUnit(codeCompileUnit, stringWriter, new CodeGeneratorOptions());
			string text3 = stringWriter.GetStringBuilder().ToString();
			return text3.Contains(text) && text3.Contains(text2);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000386C File Offset: 0x0000286C
		internal static DSGeneratorProblem GenerateValueExprAndFieldInit(DesignColumn designColumn, object valueObj, object value, string className, string fieldName, out CodeExpression valueExpr, out CodeExpression fieldInit)
		{
			DataColumn dataColumn = designColumn.DataColumn;
			valueExpr = null;
			fieldInit = null;
			if (dataColumn.DataType == typeof(char) || dataColumn.DataType == typeof(string) || dataColumn.DataType == typeof(decimal) || dataColumn.DataType == typeof(bool) || dataColumn.DataType == typeof(float) || dataColumn.DataType == typeof(double) || dataColumn.DataType == typeof(sbyte) || dataColumn.DataType == typeof(byte) || dataColumn.DataType == typeof(short) || dataColumn.DataType == typeof(ushort) || dataColumn.DataType == typeof(int) || dataColumn.DataType == typeof(uint) || dataColumn.DataType == typeof(long) || dataColumn.DataType == typeof(ulong))
			{
				valueExpr = CodeGenHelper.Primitive(valueObj);
			}
			else
			{
				valueExpr = CodeGenHelper.Field(CodeGenHelper.TypeExpr(CodeGenHelper.Type(className)), fieldName);
				if (dataColumn.DataType == typeof(byte[]))
				{
					fieldInit = CodeGenHelper.MethodCall(CodeGenHelper.GlobalTypeExpr(typeof(Convert)), "FromBase64String", CodeGenHelper.Primitive(value));
				}
				else if (dataColumn.DataType == typeof(DateTime))
				{
					fieldInit = CodeGenHelper.MethodCall(CodeGenHelper.GlobalTypeExpr(dataColumn.DataType), "Parse", CodeGenHelper.Primitive(((DateTime)valueObj).ToString(DateTimeFormatInfo.InvariantInfo)));
				}
				else if (dataColumn.DataType == typeof(TimeSpan))
				{
					fieldInit = CodeGenHelper.MethodCall(CodeGenHelper.GlobalTypeExpr(dataColumn.DataType), "Parse", CodeGenHelper.Primitive(valueObj.ToString()));
				}
				else
				{
					ConstructorInfo constructor = dataColumn.DataType.GetConstructor(new Type[] { typeof(string) });
					if (constructor == null)
					{
						return new DSGeneratorProblem(SR.GetString("CG_NoCtor1", new object[]
						{
							dataColumn.ColumnName,
							dataColumn.DataType.Name
						}), ProblemSeverity.NonFatalError, designColumn);
					}
					constructor.Invoke(new object[] { value });
					fieldInit = CodeGenHelper.New(CodeGenHelper.GlobalType(dataColumn.DataType), new CodeExpression[] { CodeGenHelper.Primitive(value) });
				}
			}
			return null;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00003B0C File Offset: 0x00002B0C
		internal static string GetLanguageExtension(CodeDomProvider codeProvider)
		{
			if (codeProvider == null)
			{
				return string.Empty;
			}
			string text = "." + codeProvider.FileExtension;
			if (text.StartsWith("..", StringComparison.Ordinal))
			{
				text = text.Substring(1);
			}
			return text;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00003B4A File Offset: 0x00002B4A
		internal static bool IsGeneratingJSharpCode(CodeDomProvider codeProvider)
		{
			return StringUtil.EqualValue(CodeGenHelper.GetLanguageExtension(codeProvider), ".jsl");
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00003B5C File Offset: 0x00002B5C
		private static bool IsSqlType(Type type)
		{
			return type == typeof(SqlBinary) || type == typeof(SqlBoolean) || type == typeof(SqlByte) || type == typeof(SqlDateTime) || type == typeof(SqlDecimal) || type == typeof(SqlDouble) || type == typeof(SqlGuid) || type == typeof(SqlInt16) || type == typeof(SqlInt32) || type == typeof(SqlInt64) || type == typeof(SqlMoney) || type == typeof(SqlSingle) || type == typeof(SqlString);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00003C20 File Offset: 0x00002C20
		private static CodeExpression GenerateUrtConvertExpression(CodeExpression sourceExpression, Type sourceUrtType, Type targetUrtType)
		{
			if (sourceUrtType == targetUrtType)
			{
				return sourceExpression;
			}
			if (sourceUrtType == typeof(object))
			{
				return CodeGenHelper.Cast(CodeGenHelper.GlobalType(targetUrtType), sourceExpression);
			}
			if (ConversionHelper.CanConvert(sourceUrtType, targetUrtType))
			{
				return new CodeMethodInvokeExpression(CodeGenHelper.GlobalTypeExpr("System.Convert"), ConversionHelper.GetConversionMethodName(sourceUrtType, targetUrtType), new CodeExpression[] { sourceExpression });
			}
			return new CodeCastExpression(CodeGenHelper.GlobalType(targetUrtType), new CodeMethodInvokeExpression(CodeGenHelper.GlobalTypeExpr("System.Convert"), "ChangeType", new CodeExpression[]
			{
				sourceExpression,
				CodeGenHelper.TypeOf(CodeGenHelper.GlobalType(targetUrtType))
			}));
		}
	}
}

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
	internal sealed class CodeGenHelper
	{
		private CodeGenHelper()
		{
		}

		internal static CodeExpression This()
		{
			return new CodeThisReferenceExpression();
		}

		internal static CodeExpression Base()
		{
			return new CodeBaseReferenceExpression();
		}

		internal static CodeExpression Value()
		{
			return new CodePropertySetValueReferenceExpression();
		}

		internal static CodeTypeReference Type(string type)
		{
			return new CodeTypeReference(type);
		}

		internal static CodeTypeReference Type(Type type)
		{
			return new CodeTypeReference(type);
		}

		internal static CodeTypeReference NullableType(Type type)
		{
			return new CodeTypeReference(typeof(Nullable))
			{
				Options = CodeTypeReferenceOptions.GlobalReference,
				TypeArguments = { CodeGenHelper.GlobalType(type) }
			};
		}

		internal static CodeTypeReference Type(string type, int rank)
		{
			return new CodeTypeReference(type, rank);
		}

		internal static CodeTypeReference GlobalType(Type type)
		{
			return new CodeTypeReference(type.ToString(), CodeTypeReferenceOptions.GlobalReference);
		}

		internal static CodeTypeReference GlobalType(Type type, int rank)
		{
			return new CodeTypeReference(CodeGenHelper.GlobalType(type), rank);
		}

		internal static CodeTypeReference GlobalType(string type)
		{
			return new CodeTypeReference(type, CodeTypeReferenceOptions.GlobalReference);
		}

		internal static CodeTypeReferenceExpression TypeExpr(CodeTypeReference type)
		{
			return new CodeTypeReferenceExpression(type);
		}

		internal static CodeTypeReferenceExpression GlobalTypeExpr(Type type)
		{
			return new CodeTypeReferenceExpression(CodeGenHelper.GlobalType(type));
		}

		internal static CodeTypeReferenceExpression GlobalTypeExpr(string type)
		{
			return new CodeTypeReferenceExpression(CodeGenHelper.GlobalType(type));
		}

		internal static CodeTypeReference GlobalGenericType(string fullTypeName, Type itemType)
		{
			return CodeGenHelper.GlobalGenericType(fullTypeName, CodeGenHelper.GlobalType(itemType));
		}

		internal static CodeTypeReference GlobalGenericType(string fullTypeName, CodeTypeReference itemType)
		{
			return new CodeTypeReference(fullTypeName, new CodeTypeReference[] { itemType })
			{
				Options = CodeTypeReferenceOptions.GlobalReference
			};
		}

		internal static CodeExpression Cast(CodeTypeReference type, CodeExpression expr)
		{
			return new CodeCastExpression(type, expr);
		}

		internal static CodeExpression TypeOf(CodeTypeReference type)
		{
			return new CodeTypeOfExpression(type);
		}

		internal static CodeExpression Field(CodeExpression exp, string field)
		{
			return new CodeFieldReferenceExpression(exp, field);
		}

		internal static CodeExpression ThisField(string field)
		{
			return new CodeFieldReferenceExpression(CodeGenHelper.This(), field);
		}

		internal static CodeExpression Property(CodeExpression exp, string property)
		{
			return new CodePropertyReferenceExpression(exp, property);
		}

		internal static CodeExpression ThisProperty(string property)
		{
			return new CodePropertyReferenceExpression(CodeGenHelper.This(), property);
		}

		internal static CodeExpression Argument(string argument)
		{
			return new CodeArgumentReferenceExpression(argument);
		}

		internal static CodeExpression Variable(string variable)
		{
			return new CodeVariableReferenceExpression(variable);
		}

		internal static CodeExpression Event(string eventName)
		{
			return new CodeEventReferenceExpression(CodeGenHelper.This(), eventName);
		}

		internal static CodeExpression New(CodeTypeReference type, CodeExpression[] parameters)
		{
			return new CodeObjectCreateExpression(type, parameters);
		}

		internal static CodeExpression NewArray(CodeTypeReference type, int size)
		{
			return new CodeArrayCreateExpression(type, size);
		}

		internal static CodeExpression NewArray(CodeTypeReference type, params CodeExpression[] initializers)
		{
			return new CodeArrayCreateExpression(type, initializers);
		}

		internal static CodeExpression Primitive(object primitive)
		{
			return new CodePrimitiveExpression(primitive);
		}

		internal static CodeExpression Str(string str)
		{
			return CodeGenHelper.Primitive(str);
		}

		internal static CodeExpression MethodCall(CodeExpression targetObject, string methodName, CodeExpression[] parameters)
		{
			return new CodeMethodInvokeExpression(targetObject, methodName, parameters);
		}

		internal static CodeStatement MethodCallStm(CodeExpression targetObject, string methodName, CodeExpression[] parameters)
		{
			return CodeGenHelper.Stm(CodeGenHelper.MethodCall(targetObject, methodName, parameters));
		}

		internal static CodeExpression MethodCall(CodeExpression targetObject, string methodName)
		{
			return new CodeMethodInvokeExpression(targetObject, methodName, new CodeExpression[0]);
		}

		internal static CodeStatement MethodCallStm(CodeExpression targetObject, string methodName)
		{
			return CodeGenHelper.Stm(CodeGenHelper.MethodCall(targetObject, methodName));
		}

		internal static CodeExpression MethodCall(CodeExpression targetObject, string methodName, CodeExpression par)
		{
			return new CodeMethodInvokeExpression(targetObject, methodName, new CodeExpression[] { par });
		}

		internal static CodeStatement MethodCallStm(CodeExpression targetObject, string methodName, CodeExpression par)
		{
			return CodeGenHelper.Stm(CodeGenHelper.MethodCall(targetObject, methodName, par));
		}

		internal static CodeExpression DelegateCall(CodeExpression targetObject, CodeExpression par)
		{
			return new CodeDelegateInvokeExpression(targetObject, new CodeExpression[]
			{
				CodeGenHelper.This(),
				par
			});
		}

		internal static CodeExpression Indexer(CodeExpression targetObject, CodeExpression indices)
		{
			return new CodeIndexerExpression(targetObject, new CodeExpression[] { indices });
		}

		internal static CodeExpression ArrayIndexer(CodeExpression targetObject, CodeExpression indices)
		{
			return new CodeArrayIndexerExpression(targetObject, new CodeExpression[] { indices });
		}

		internal static CodeExpression ReferenceEquals(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.MethodCall(CodeGenHelper.GlobalTypeExpr(typeof(object)), "ReferenceEquals", new CodeExpression[] { left, right });
		}

		internal static CodeExpression ReferenceNotEquals(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.EQ(CodeGenHelper.ReferenceEquals(left, right), CodeGenHelper.Primitive(false));
		}

		internal static CodeBinaryOperatorExpression BinOperator(CodeExpression left, CodeBinaryOperatorType op, CodeExpression right)
		{
			return new CodeBinaryOperatorExpression(left, op, right);
		}

		internal static CodeBinaryOperatorExpression IdNotEQ(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.IdentityInequality, right);
		}

		internal static CodeBinaryOperatorExpression IdEQ(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.IdentityEquality, right);
		}

		internal static CodeBinaryOperatorExpression IdIsNull(CodeExpression id)
		{
			return CodeGenHelper.IdEQ(id, CodeGenHelper.Primitive(null));
		}

		internal static CodeBinaryOperatorExpression IdIsNotNull(CodeExpression id)
		{
			return CodeGenHelper.IdNotEQ(id, CodeGenHelper.Primitive(null));
		}

		internal static CodeBinaryOperatorExpression EQ(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.ValueEquality, right);
		}

		internal static CodeBinaryOperatorExpression NotEQ(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.EQ(CodeGenHelper.EQ(left, right), CodeGenHelper.Primitive(false));
		}

		internal static CodeBinaryOperatorExpression BitwiseAnd(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.BitwiseAnd, right);
		}

		internal static CodeBinaryOperatorExpression And(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.BooleanAnd, right);
		}

		internal static CodeBinaryOperatorExpression Or(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.BooleanOr, right);
		}

		internal static CodeBinaryOperatorExpression Less(CodeExpression left, CodeExpression right)
		{
			return CodeGenHelper.BinOperator(left, CodeBinaryOperatorType.LessThan, right);
		}

		internal static CodeStatement Stm(CodeExpression expr)
		{
			return new CodeExpressionStatement(expr);
		}

		internal static CodeStatement Return(CodeExpression expr)
		{
			return new CodeMethodReturnStatement(expr);
		}

		internal static CodeStatement Return()
		{
			return new CodeMethodReturnStatement();
		}

		internal static CodeStatement Assign(CodeExpression left, CodeExpression right)
		{
			return new CodeAssignStatement(left, right);
		}

		internal static CodeStatement Throw(CodeTypeReference exception, string arg)
		{
			return new CodeThrowExceptionStatement(CodeGenHelper.New(exception, new CodeExpression[] { CodeGenHelper.Str(arg) }));
		}

		internal static CodeStatement Throw(CodeTypeReference exception, string arg, string inner)
		{
			return new CodeThrowExceptionStatement(CodeGenHelper.New(exception, new CodeExpression[]
			{
				CodeGenHelper.Str(arg),
				CodeGenHelper.Variable(inner)
			}));
		}

		internal static CodeStatement Throw(CodeTypeReference exception, string arg, CodeExpression inner)
		{
			return new CodeThrowExceptionStatement(CodeGenHelper.New(exception, new CodeExpression[]
			{
				CodeGenHelper.Str(arg),
				inner
			}));
		}

		internal static CodeCommentStatement Comment(string comment, bool docSummary)
		{
			if (docSummary)
			{
				return new CodeCommentStatement("<summary>\r\n" + comment + "\r\n</summary>", docSummary);
			}
			return new CodeCommentStatement(comment);
		}

		internal static CodeStatement If(CodeExpression cond, CodeStatement[] trueStms, CodeStatement[] falseStms)
		{
			return new CodeConditionStatement(cond, trueStms, falseStms);
		}

		internal static CodeStatement If(CodeExpression cond, CodeStatement trueStm, CodeStatement falseStm)
		{
			return new CodeConditionStatement(cond, new CodeStatement[] { trueStm }, new CodeStatement[] { falseStm });
		}

		internal static CodeStatement If(CodeExpression cond, CodeStatement[] trueStms)
		{
			return new CodeConditionStatement(cond, trueStms);
		}

		internal static CodeStatement If(CodeExpression cond, CodeStatement trueStm)
		{
			return CodeGenHelper.If(cond, new CodeStatement[] { trueStm });
		}

		internal static CodeMemberField FieldDecl(CodeTypeReference type, string name)
		{
			return new CodeMemberField(type, name);
		}

		internal static CodeMemberField FieldDecl(CodeTypeReference type, string name, CodeExpression initExpr)
		{
			return new CodeMemberField(type, name)
			{
				InitExpression = initExpr
			};
		}

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

		internal static CodeConstructor Constructor(MemberAttributes attributes)
		{
			return new CodeConstructor
			{
				Attributes = attributes,
				CustomAttributes = { CodeGenHelper.AttributeDecl(typeof(DebuggerNonUserCodeAttribute).FullName) }
			};
		}

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

		internal static CodeStatement VariableDecl(CodeTypeReference type, string name)
		{
			return new CodeVariableDeclarationStatement(type, name);
		}

		internal static CodeStatement VariableDecl(CodeTypeReference type, string name, CodeExpression initExpr)
		{
			return new CodeVariableDeclarationStatement(type, name, initExpr);
		}

		internal static CodeStatement ForLoop(CodeStatement initStmt, CodeExpression testExpression, CodeStatement incrementStmt, CodeStatement[] statements)
		{
			return new CodeIterationStatement(initStmt, testExpression, incrementStmt, statements);
		}

		internal static CodeMemberEvent EventDecl(string type, string name)
		{
			return new CodeMemberEvent
			{
				Name = name,
				Type = CodeGenHelper.Type(type),
				Attributes = (MemberAttributes)24578
			};
		}

		internal static CodeParameterDeclarationExpression ParameterDecl(CodeTypeReference type, string name)
		{
			return new CodeParameterDeclarationExpression(type, name);
		}

		internal static CodeAttributeDeclaration AttributeDecl(string name)
		{
			return new CodeAttributeDeclaration(CodeGenHelper.GlobalType(name));
		}

		internal static CodeAttributeDeclaration AttributeDecl(string name, CodeExpression value)
		{
			return new CodeAttributeDeclaration(CodeGenHelper.GlobalType(name), new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(value)
			});
		}

		internal static CodeAttributeDeclaration AttributeDecl(string name, CodeExpression value1, CodeExpression value2)
		{
			return new CodeAttributeDeclaration(CodeGenHelper.GlobalType(name), new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(value1),
				new CodeAttributeArgument(value2)
			});
		}

		internal static CodeStatement Try(CodeStatement tryStmnt, CodeCatchClause catchClause)
		{
			return new CodeTryCatchFinallyStatement(new CodeStatement[] { tryStmnt }, new CodeCatchClause[] { catchClause });
		}

		internal static CodeStatement Try(CodeStatement[] tryStmnts, CodeCatchClause[] catchClauses, CodeStatement[] finallyStmnts)
		{
			return new CodeTryCatchFinallyStatement(tryStmnts, catchClauses, finallyStmnts);
		}

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

		internal static CodeExpression GenerateDbNullCheck(CodeExpression returnParam)
		{
			return CodeGenHelper.Or(CodeGenHelper.IdEQ(returnParam, CodeGenHelper.Primitive(null)), CodeGenHelper.IdEQ(CodeGenHelper.MethodCall(returnParam, "GetType"), CodeGenHelper.TypeOf(CodeGenHelper.GlobalType(typeof(DBNull)))));
		}

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

		internal static string GetTypeName(CodeDomProvider codeProvider, string string1, string string2)
		{
			string typeOutput = codeProvider.GetTypeOutput(CodeGenHelper.Type(typeof(Activator)));
			string text = typeOutput.Replace("System", "").Replace("Activator", "");
			return string1 + text + string2;
		}

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

		internal static bool IsGeneratingJSharpCode(CodeDomProvider codeProvider)
		{
			return StringUtil.EqualValue(CodeGenHelper.GetLanguageExtension(codeProvider), ".jsl");
		}

		private static bool IsSqlType(Type type)
		{
			return type == typeof(SqlBinary) || type == typeof(SqlBoolean) || type == typeof(SqlByte) || type == typeof(SqlDateTime) || type == typeof(SqlDecimal) || type == typeof(SqlDouble) || type == typeof(SqlGuid) || type == typeof(SqlInt16) || type == typeof(SqlInt32) || type == typeof(SqlInt64) || type == typeof(SqlMoney) || type == typeof(SqlSingle) || type == typeof(SqlString);
		}

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

﻿using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace System.Xml.Serialization
{
	// Token: 0x02000333 RID: 819
	internal class ReflectionAwareCodeGen
	{
		// Token: 0x060027FF RID: 10239 RVA: 0x000CEFD1 File Offset: 0x000CDFD1
		internal ReflectionAwareCodeGen(IndentedWriter writer)
		{
			this.writer = writer;
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000CEFE0 File Offset: 0x000CDFE0
		internal void WriteReflectionInit(TypeScope scope)
		{
			foreach (object obj in scope.Types)
			{
				Type type = (Type)obj;
				TypeDesc typeDesc = scope.GetTypeDesc(type);
				if (typeDesc.UseReflection)
				{
					this.WriteTypeInfo(scope, typeDesc, type);
				}
			}
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000CF04C File Offset: 0x000CE04C
		private string WriteTypeInfo(TypeScope scope, TypeDesc typeDesc, Type type)
		{
			this.InitTheFirstTime();
			string csharpName = typeDesc.CSharpName;
			string text = (string)this.reflectionVariables[csharpName];
			if (text != null)
			{
				return text;
			}
			if (type.IsArray)
			{
				text = this.GenerateVariableName("array", typeDesc.CSharpName);
				TypeDesc arrayElementTypeDesc = typeDesc.ArrayElementTypeDesc;
				if (arrayElementTypeDesc.UseReflection)
				{
					string text2 = this.WriteTypeInfo(scope, arrayElementTypeDesc, scope.GetTypeFromTypeDesc(arrayElementTypeDesc));
					this.writer.WriteLine(string.Concat(new string[]
					{
						"static ",
						typeof(Type).FullName,
						" ",
						text,
						" = ",
						text2,
						".MakeArrayType();"
					}));
				}
				else
				{
					string text3 = this.WriteAssemblyInfo(type);
					this.writer.Write(string.Concat(new string[]
					{
						"static ",
						typeof(Type).FullName,
						" ",
						text,
						" = ",
						text3,
						".GetType("
					}));
					this.WriteQuotedCSharpString(type.FullName);
					this.writer.WriteLine(");");
				}
			}
			else
			{
				text = this.GenerateVariableName("type", typeDesc.CSharpName);
				Type underlyingType = Nullable.GetUnderlyingType(type);
				if (underlyingType != null)
				{
					string text4 = this.WriteTypeInfo(scope, scope.GetTypeDesc(underlyingType), underlyingType);
					this.writer.WriteLine(string.Concat(new string[]
					{
						"static ",
						typeof(Type).FullName,
						" ",
						text,
						" = typeof(System.Nullable<>).MakeGenericType(new ",
						typeof(Type).FullName,
						"[] {",
						text4,
						"});"
					}));
				}
				else
				{
					string text5 = this.WriteAssemblyInfo(type);
					this.writer.Write(string.Concat(new string[]
					{
						"static ",
						typeof(Type).FullName,
						" ",
						text,
						" = ",
						text5,
						".GetType("
					}));
					this.WriteQuotedCSharpString(type.FullName);
					this.writer.WriteLine(");");
				}
			}
			this.reflectionVariables.Add(csharpName, text);
			TypeMapping typeMappingFromTypeDesc = scope.GetTypeMappingFromTypeDesc(typeDesc);
			if (typeMappingFromTypeDesc != null)
			{
				this.WriteMappingInfo(typeMappingFromTypeDesc, text, type);
			}
			if (typeDesc.IsCollection || typeDesc.IsEnumerable)
			{
				TypeDesc arrayElementTypeDesc2 = typeDesc.ArrayElementTypeDesc;
				if (arrayElementTypeDesc2.UseReflection)
				{
					this.WriteTypeInfo(scope, arrayElementTypeDesc2, scope.GetTypeFromTypeDesc(arrayElementTypeDesc2));
				}
				this.WriteCollectionInfo(text, typeDesc, type);
			}
			return text;
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000CF334 File Offset: 0x000CE334
		private void InitTheFirstTime()
		{
			if (this.reflectionVariables == null)
			{
				this.reflectionVariables = new Hashtable();
				this.writer.Write(string.Format(CultureInfo.InvariantCulture, ReflectionAwareCodeGen.helperClassesForUseReflection, new object[]
				{
					"object",
					"string",
					typeof(Type).FullName,
					typeof(FieldInfo).FullName,
					typeof(PropertyInfo).FullName,
					typeof(MemberInfo).FullName,
					typeof(MemberTypes).FullName
				}));
				this.WriteDefaultIndexerInit(typeof(IList), typeof(Array).FullName, false, false);
			}
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000CF408 File Offset: 0x000CE408
		private void WriteMappingInfo(TypeMapping mapping, string typeVariable, Type type)
		{
			string csharpName = mapping.TypeDesc.CSharpName;
			if (mapping is StructMapping)
			{
				StructMapping structMapping = mapping as StructMapping;
				for (int i = 0; i < structMapping.Members.Length; i++)
				{
					MemberMapping memberMapping = structMapping.Members[i];
					this.WriteMemberInfo(type, csharpName, typeVariable, memberMapping.Name);
					if (memberMapping.CheckShouldPersist)
					{
						string text = "ShouldSerialize" + memberMapping.Name;
						this.WriteMethodInfo(csharpName, typeVariable, text, false, new string[0]);
					}
					if (memberMapping.CheckSpecified != SpecifiedAccessor.None)
					{
						string text2 = memberMapping.Name + "Specified";
						this.WriteMemberInfo(type, csharpName, typeVariable, text2);
					}
					if (memberMapping.ChoiceIdentifier != null)
					{
						string memberName = memberMapping.ChoiceIdentifier.MemberName;
						this.WriteMemberInfo(type, csharpName, typeVariable, memberName);
					}
				}
				return;
			}
			if (mapping is EnumMapping)
			{
				FieldInfo[] fields = type.GetFields();
				for (int j = 0; j < fields.Length; j++)
				{
					this.WriteMemberInfo(type, csharpName, typeVariable, fields[j].Name);
				}
			}
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000CF510 File Offset: 0x000CE510
		private void WriteCollectionInfo(string typeVariable, TypeDesc typeDesc, Type type)
		{
			string csharpName = CodeIdentifier.GetCSharpName(type);
			string csharpName2 = typeDesc.ArrayElementTypeDesc.CSharpName;
			bool useReflection = typeDesc.ArrayElementTypeDesc.UseReflection;
			if (typeDesc.IsCollection)
			{
				this.WriteDefaultIndexerInit(type, csharpName, typeDesc.UseReflection, useReflection);
			}
			else if (typeDesc.IsEnumerable)
			{
				if (typeDesc.IsGenericInterface)
				{
					this.WriteMethodInfo(csharpName, typeVariable, "System.Collections.Generic.IEnumerable*", true, new string[0]);
				}
				else if (!typeDesc.IsPrivateImplementation)
				{
					this.WriteMethodInfo(csharpName, typeVariable, "GetEnumerator", true, new string[0]);
				}
			}
			this.WriteMethodInfo(csharpName, typeVariable, "Add", false, new string[] { this.GetStringForTypeof(csharpName2, useReflection) });
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000CF5BC File Offset: 0x000CE5BC
		private string WriteAssemblyInfo(Type type)
		{
			string fullName = type.Assembly.FullName;
			string text = (string)this.reflectionVariables[fullName];
			if (text == null)
			{
				int num = fullName.IndexOf(',');
				string text2 = ((num > -1) ? fullName.Substring(0, num) : fullName);
				text = this.GenerateVariableName("assembly", text2);
				this.writer.Write(string.Concat(new string[]
				{
					"static ",
					typeof(Assembly).FullName,
					" ",
					text,
					" = ResolveDynamicAssembly("
				}));
				this.WriteQuotedCSharpString(DynamicAssemblies.GetName(type.Assembly));
				this.writer.WriteLine(");");
				this.reflectionVariables.Add(fullName, text);
			}
			return text;
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x000CF690 File Offset: 0x000CE690
		private string WriteMemberInfo(Type type, string escapedName, string typeVariable, string memberName)
		{
			MemberInfo[] member = type.GetMember(memberName);
			for (int i = 0; i < member.Length; i++)
			{
				MemberTypes memberType = member[i].MemberType;
				if (memberType == MemberTypes.Property)
				{
					string text = this.GenerateVariableName("prop", memberName);
					this.writer.Write(string.Concat(new string[] { "static XSPropInfo ", text, " = new XSPropInfo(", typeVariable, ", " }));
					this.WriteQuotedCSharpString(memberName);
					this.writer.WriteLine(");");
					this.reflectionVariables.Add(memberName + ":" + escapedName, text);
					return text;
				}
				if (memberType == MemberTypes.Field)
				{
					string text2 = this.GenerateVariableName("field", memberName);
					this.writer.Write(string.Concat(new string[] { "static XSFieldInfo ", text2, " = new XSFieldInfo(", typeVariable, ", " }));
					this.WriteQuotedCSharpString(memberName);
					this.writer.WriteLine(");");
					this.reflectionVariables.Add(memberName + ":" + escapedName, text2);
					return text2;
				}
			}
			throw new InvalidOperationException(Res.GetString("XmlSerializerUnsupportedType", new object[] { member[0].ToString() }));
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000CF7FC File Offset: 0x000CE7FC
		private string WriteMethodInfo(string escapedName, string typeVariable, string memberName, bool isNonPublic, params string[] paramTypes)
		{
			string text = this.GenerateVariableName("method", memberName);
			this.writer.Write(string.Concat(new string[]
			{
				"static ",
				typeof(MethodInfo).FullName,
				" ",
				text,
				" = ",
				typeVariable,
				".GetMethod("
			}));
			this.WriteQuotedCSharpString(memberName);
			this.writer.Write(", ");
			string fullName = typeof(BindingFlags).FullName;
			this.writer.Write(fullName);
			this.writer.Write(".Public | ");
			this.writer.Write(fullName);
			this.writer.Write(".Instance | ");
			this.writer.Write(fullName);
			this.writer.Write(".Static");
			if (isNonPublic)
			{
				this.writer.Write(" | ");
				this.writer.Write(fullName);
				this.writer.Write(".NonPublic");
			}
			this.writer.Write(", null, ");
			this.writer.Write("new " + typeof(Type).FullName + "[] { ");
			for (int i = 0; i < paramTypes.Length; i++)
			{
				this.writer.Write(paramTypes[i]);
				if (i < paramTypes.Length - 1)
				{
					this.writer.Write(", ");
				}
			}
			this.writer.WriteLine("}, null);");
			this.reflectionVariables.Add(memberName + ":" + escapedName, text);
			return text;
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x000CF9AC File Offset: 0x000CE9AC
		private string WriteDefaultIndexerInit(Type type, string escapedName, bool collectionUseReflection, bool elementUseReflection)
		{
			string text = this.GenerateVariableName("item", escapedName);
			PropertyInfo defaultIndexer = TypeScope.GetDefaultIndexer(type, null);
			this.writer.Write("static XSArrayInfo ");
			this.writer.Write(text);
			this.writer.Write("= new XSArrayInfo(");
			this.writer.Write(this.GetStringForTypeof(CodeIdentifier.GetCSharpName(type), collectionUseReflection));
			this.writer.Write(".GetProperty(");
			this.WriteQuotedCSharpString(defaultIndexer.Name);
			this.writer.Write(",");
			this.writer.Write(this.GetStringForTypeof(CodeIdentifier.GetCSharpName(defaultIndexer.PropertyType), elementUseReflection));
			this.writer.Write(",new ");
			this.writer.Write(typeof(Type[]).FullName);
			this.writer.WriteLine("{typeof(int)}));");
			this.reflectionVariables.Add("0:" + escapedName, text);
			return text;
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000CFAB0 File Offset: 0x000CEAB0
		private string GenerateVariableName(string prefix, string fullName)
		{
			this.nextReflectionVariableNumber++;
			return string.Concat(new object[]
			{
				prefix,
				this.nextReflectionVariableNumber,
				"_",
				CodeIdentifier.MakeValidInternal(fullName.Replace('.', '_'))
			});
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x000CFB04 File Offset: 0x000CEB04
		internal string GetReflectionVariable(string typeFullName, string memberName)
		{
			string text;
			if (memberName == null)
			{
				text = typeFullName;
			}
			else
			{
				text = memberName + ":" + typeFullName;
			}
			return (string)this.reflectionVariables[text];
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x000CFB38 File Offset: 0x000CEB38
		internal string GetStringForMethodInvoke(string obj, string escapedTypeName, string methodName, bool useReflection, params string[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (useReflection)
			{
				stringBuilder.Append(this.GetReflectionVariable(escapedTypeName, methodName));
				stringBuilder.Append(".Invoke(");
				stringBuilder.Append(obj);
				stringBuilder.Append(", new object[] {");
			}
			else
			{
				stringBuilder.Append(obj);
				stringBuilder.Append(".@");
				stringBuilder.Append(methodName);
				stringBuilder.Append("(");
			}
			for (int i = 0; i < args.Length; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(args[i]);
			}
			if (useReflection)
			{
				stringBuilder.Append("})");
			}
			else
			{
				stringBuilder.Append(")");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x000CFBF8 File Offset: 0x000CEBF8
		internal string GetStringForEnumCompare(EnumMapping mapping, string memberName, bool useReflection)
		{
			if (!useReflection)
			{
				CodeIdentifier.CheckValidIdentifier(memberName);
				return mapping.TypeDesc.CSharpName + ".@" + memberName;
			}
			string stringForEnumMember = this.GetStringForEnumMember(mapping.TypeDesc.CSharpName, memberName, useReflection);
			return this.GetStringForEnumLongValue(stringForEnumMember, useReflection);
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000CFC44 File Offset: 0x000CEC44
		internal string GetStringForEnumLongValue(string variable, bool useReflection)
		{
			if (useReflection)
			{
				return typeof(Convert).FullName + ".ToInt64(" + variable + ")";
			}
			return string.Concat(new string[]
			{
				"((",
				typeof(long).FullName,
				")",
				variable,
				")"
			});
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x000CFCAF File Offset: 0x000CECAF
		internal string GetStringForTypeof(string typeFullName, bool useReflection)
		{
			if (useReflection)
			{
				return this.GetReflectionVariable(typeFullName, null);
			}
			return "typeof(" + typeFullName + ")";
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x000CFCD0 File Offset: 0x000CECD0
		internal string GetStringForMember(string obj, string memberName, TypeDesc typeDesc)
		{
			if (!typeDesc.UseReflection)
			{
				return obj + ".@" + memberName;
			}
			while (typeDesc != null)
			{
				string csharpName = typeDesc.CSharpName;
				string reflectionVariable = this.GetReflectionVariable(csharpName, memberName);
				if (reflectionVariable != null)
				{
					return reflectionVariable + "[" + obj + "]";
				}
				typeDesc = typeDesc.BaseTypeDesc;
				if (typeDesc != null && !typeDesc.UseReflection)
				{
					return string.Concat(new string[] { "((", typeDesc.CSharpName, ")", obj, ").@", memberName });
				}
			}
			return "[" + obj + "]";
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x000CFD74 File Offset: 0x000CED74
		internal string GetStringForEnumMember(string typeFullName, string memberName, bool useReflection)
		{
			if (!useReflection)
			{
				return typeFullName + ".@" + memberName;
			}
			string reflectionVariable = this.GetReflectionVariable(typeFullName, memberName);
			return reflectionVariable + "[null]";
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x000CFDA8 File Offset: 0x000CEDA8
		internal string GetStringForArrayMember(string arrayName, string subscript, TypeDesc arrayTypeDesc)
		{
			if (!arrayTypeDesc.UseReflection)
			{
				return arrayName + "[" + subscript + "]";
			}
			string text = (arrayTypeDesc.IsCollection ? arrayTypeDesc.CSharpName : typeof(Array).FullName);
			string reflectionVariable = this.GetReflectionVariable(text, "0");
			return string.Concat(new string[] { reflectionVariable, "[", arrayName, ", ", subscript, "]" });
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x000CFE30 File Offset: 0x000CEE30
		internal string GetStringForMethod(string obj, string typeFullName, string memberName, bool useReflection)
		{
			if (!useReflection)
			{
				return obj + "." + memberName + "(";
			}
			string reflectionVariable = this.GetReflectionVariable(typeFullName, memberName);
			return reflectionVariable + ".Invoke(" + obj + ", new object[]{";
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x000CFE6D File Offset: 0x000CEE6D
		internal string GetStringForCreateInstance(string escapedTypeName, bool useReflection, bool ctorInaccessible, bool cast)
		{
			return this.GetStringForCreateInstance(escapedTypeName, useReflection, ctorInaccessible, cast, string.Empty);
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x000CFE80 File Offset: 0x000CEE80
		internal string GetStringForCreateInstance(string escapedTypeName, bool useReflection, bool ctorInaccessible, bool cast, string arg)
		{
			if (!useReflection && !ctorInaccessible)
			{
				return string.Concat(new string[] { "new ", escapedTypeName, "(", arg, ")" });
			}
			return this.GetStringForCreateInstance(this.GetStringForTypeof(escapedTypeName, useReflection), (cast && !useReflection) ? escapedTypeName : null, ctorInaccessible, arg);
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x000CFEE0 File Offset: 0x000CEEE0
		internal string GetStringForCreateInstance(string type, string cast, bool nonPublic, string arg)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (cast != null && cast.Length > 0)
			{
				stringBuilder.Append("(");
				stringBuilder.Append(cast);
				stringBuilder.Append(")");
			}
			stringBuilder.Append(typeof(Activator).FullName);
			stringBuilder.Append(".CreateInstance(");
			stringBuilder.Append(type);
			stringBuilder.Append(", ");
			string fullName = typeof(BindingFlags).FullName;
			stringBuilder.Append(fullName);
			stringBuilder.Append(".Instance | ");
			stringBuilder.Append(fullName);
			stringBuilder.Append(".Public | ");
			stringBuilder.Append(fullName);
			stringBuilder.Append(".CreateInstance");
			if (nonPublic)
			{
				stringBuilder.Append(" | ");
				stringBuilder.Append(fullName);
				stringBuilder.Append(".NonPublic");
			}
			if (arg == null || arg.Length == 0)
			{
				stringBuilder.Append(", null, new object[0], null)");
			}
			else
			{
				stringBuilder.Append(", null, new object[] { ");
				stringBuilder.Append(arg);
				stringBuilder.Append(" }, null)");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000D0008 File Offset: 0x000CF008
		internal void WriteLocalDecl(string typeFullName, string variableName, string initValue, bool useReflection)
		{
			if (useReflection)
			{
				typeFullName = "object";
			}
			this.writer.Write(typeFullName);
			this.writer.Write(" ");
			this.writer.Write(variableName);
			if (initValue != null)
			{
				this.writer.Write(" = ");
				if (!useReflection && initValue != "null")
				{
					this.writer.Write("(" + typeFullName + ")");
				}
				this.writer.Write(initValue);
			}
			this.writer.WriteLine(";");
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000D00A4 File Offset: 0x000CF0A4
		internal void WriteCreateInstance(string escapedName, string source, bool useReflection, bool ctorInaccessible)
		{
			this.writer.Write(useReflection ? "object" : escapedName);
			this.writer.Write(" ");
			this.writer.Write(source);
			this.writer.Write(" = ");
			this.writer.Write(this.GetStringForCreateInstance(escapedName, useReflection, ctorInaccessible, !useReflection && ctorInaccessible));
			this.writer.WriteLine(";");
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000D0120 File Offset: 0x000CF120
		internal void WriteInstanceOf(string source, string escapedTypeName, bool useReflection)
		{
			if (!useReflection)
			{
				this.writer.Write(source);
				this.writer.Write(" is ");
				this.writer.Write(escapedTypeName);
				return;
			}
			this.writer.Write(this.GetReflectionVariable(escapedTypeName, null));
			this.writer.Write(".IsAssignableFrom(");
			this.writer.Write(source);
			this.writer.Write(".GetType())");
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000D0198 File Offset: 0x000CF198
		internal void WriteArrayLocalDecl(string typeName, string variableName, string initValue, TypeDesc arrayTypeDesc)
		{
			if (arrayTypeDesc.UseReflection)
			{
				if (arrayTypeDesc.IsEnumerable)
				{
					typeName = typeof(IEnumerable).FullName;
				}
				else if (arrayTypeDesc.IsCollection)
				{
					typeName = typeof(ICollection).FullName;
				}
				else
				{
					typeName = typeof(Array).FullName;
				}
			}
			this.writer.Write(typeName);
			this.writer.Write(" ");
			this.writer.Write(variableName);
			if (initValue != null)
			{
				this.writer.Write(" = ");
				if (initValue != "null")
				{
					this.writer.Write("(" + typeName + ")");
				}
				this.writer.Write(initValue);
			}
			this.writer.WriteLine(";");
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000D0278 File Offset: 0x000CF278
		internal void WriteEnumCase(string fullTypeName, ConstantMapping c, bool useReflection)
		{
			this.writer.Write("case ");
			if (useReflection)
			{
				this.writer.Write(c.Value.ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				this.writer.Write(fullTypeName);
				this.writer.Write(".@");
				CodeIdentifier.CheckValidIdentifier(c.Name);
				this.writer.Write(c.Name);
			}
			this.writer.Write(": ");
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000D0300 File Offset: 0x000CF300
		internal void WriteTypeCompare(string variable, string escapedTypeName, bool useReflection)
		{
			this.writer.Write(variable);
			this.writer.Write(" == ");
			this.writer.Write(this.GetStringForTypeof(escapedTypeName, useReflection));
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000D0334 File Offset: 0x000CF334
		internal void WriteArrayTypeCompare(string variable, string escapedTypeName, string elementTypeName, bool useReflection)
		{
			if (!useReflection)
			{
				this.writer.Write(variable);
				this.writer.Write(" == typeof(");
				this.writer.Write(escapedTypeName);
				this.writer.Write(")");
				return;
			}
			this.writer.Write(variable);
			this.writer.Write(".IsArray ");
			this.writer.Write(" && ");
			this.WriteTypeCompare(variable + ".GetElementType()", elementTypeName, useReflection);
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x000D03C0 File Offset: 0x000CF3C0
		internal static void WriteQuotedCSharpString(IndentedWriter writer, string value)
		{
			if (value == null)
			{
				writer.Write("null");
				return;
			}
			writer.Write("@\"");
			foreach (char c in value)
			{
				if (c < ' ')
				{
					if (c == '\r')
					{
						writer.Write("\\r");
					}
					else if (c == '\n')
					{
						writer.Write("\\n");
					}
					else if (c == '\t')
					{
						writer.Write("\\t");
					}
					else
					{
						byte b = (byte)c;
						writer.Write("\\x");
						writer.Write("0123456789ABCDEF"[b >> 4]);
						writer.Write("0123456789ABCDEF"[(int)(b & 15)]);
					}
				}
				else if (c == '"')
				{
					writer.Write("\"\"");
				}
				else
				{
					writer.Write(c);
				}
			}
			writer.Write("\"");
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x000D049E File Offset: 0x000CF49E
		internal void WriteQuotedCSharpString(string value)
		{
			ReflectionAwareCodeGen.WriteQuotedCSharpString(this.writer, value);
		}

		// Token: 0x0400166A RID: 5738
		private const string hexDigits = "0123456789ABCDEF";

		// Token: 0x0400166B RID: 5739
		private const string arrayMemberKey = "0";

		// Token: 0x0400166C RID: 5740
		private Hashtable reflectionVariables;

		// Token: 0x0400166D RID: 5741
		private int nextReflectionVariableNumber;

		// Token: 0x0400166E RID: 5742
		private IndentedWriter writer;

		// Token: 0x0400166F RID: 5743
		private static string helperClassesForUseReflection = "\r\n    sealed class XSFieldInfo {{\r\n       {3} fieldInfo;\r\n        public XSFieldInfo({2} t, {1} memberName){{\r\n            fieldInfo = t.GetField(memberName);\r\n        }}\r\n        public {0} this[{0} o] {{\r\n            get {{\r\n                return fieldInfo.GetValue(o);\r\n            }}\r\n            set {{\r\n                fieldInfo.SetValue(o, value);\r\n            }}\r\n        }}\r\n\r\n    }}\r\n    sealed class XSPropInfo {{\r\n        {4} propInfo;\r\n        public XSPropInfo({2} t, {1} memberName){{\r\n            propInfo = t.GetProperty(memberName);\r\n        }}\r\n        public {0} this[{0} o] {{\r\n            get {{\r\n                return propInfo.GetValue(o, null);\r\n            }}\r\n            set {{\r\n                propInfo.SetValue(o, value, null);\r\n            }}\r\n        }}\r\n    }}\r\n    sealed class XSArrayInfo {{\r\n        {4} propInfo;\r\n        public XSArrayInfo({4} propInfo){{\r\n            this.propInfo = propInfo;\r\n        }}\r\n        public {0} this[{0} a, int i] {{\r\n            get {{\r\n                return propInfo.GetValue(a, new {0}[]{{i}});\r\n            }}\r\n            set {{\r\n                propInfo.SetValue(a, value, new {0}[]{{i}});\r\n            }}\r\n        }}\r\n    }}\r\n";
	}
}

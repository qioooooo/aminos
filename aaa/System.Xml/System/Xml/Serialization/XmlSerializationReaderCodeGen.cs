﻿using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x0200032B RID: 811
	internal class XmlSerializationReaderCodeGen : XmlSerializationCodeGen
	{
		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x060026EA RID: 9962 RVA: 0x000BFF2C File Offset: 0x000BEF2C
		internal Hashtable Enums
		{
			get
			{
				if (this.enums == null)
				{
					this.enums = new Hashtable();
				}
				return this.enums;
			}
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x000BFF47 File Offset: 0x000BEF47
		internal XmlSerializationReaderCodeGen(IndentedWriter writer, TypeScope[] scopes, string access, string className)
			: base(writer, scopes, access, className)
		{
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x000BFF6C File Offset: 0x000BEF6C
		internal void GenerateBegin()
		{
			base.Writer.Write(base.Access);
			base.Writer.Write(" class ");
			base.Writer.Write(base.ClassName);
			base.Writer.Write(" : ");
			base.Writer.Write(typeof(XmlSerializationReader).FullName);
			base.Writer.WriteLine(" {");
			base.Writer.Indent++;
			foreach (TypeScope typeScope in base.Scopes)
			{
				foreach (object obj in typeScope.TypeMappings)
				{
					TypeMapping typeMapping = (TypeMapping)obj;
					if (typeMapping is StructMapping || typeMapping is EnumMapping || typeMapping is NullableMapping)
					{
						base.MethodNames.Add(typeMapping, this.NextMethodName(typeMapping.TypeDesc.Name));
					}
				}
				base.RaCodeGen.WriteReflectionInit(typeScope);
			}
			foreach (TypeScope typeScope2 in base.Scopes)
			{
				foreach (object obj2 in typeScope2.TypeMappings)
				{
					TypeMapping typeMapping2 = (TypeMapping)obj2;
					if (typeMapping2.IsSoap)
					{
						if (typeMapping2 is StructMapping)
						{
							this.WriteStructMethod((StructMapping)typeMapping2);
						}
						else if (typeMapping2 is EnumMapping)
						{
							this.WriteEnumMethod((EnumMapping)typeMapping2);
						}
						else if (typeMapping2 is NullableMapping)
						{
							this.WriteNullableMethod((NullableMapping)typeMapping2);
						}
					}
				}
			}
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000C0164 File Offset: 0x000BF164
		internal override void GenerateMethod(TypeMapping mapping)
		{
			if (base.GeneratedMethods.Contains(mapping))
			{
				return;
			}
			base.GeneratedMethods[mapping] = mapping;
			if (mapping is StructMapping)
			{
				this.WriteStructMethod((StructMapping)mapping);
				return;
			}
			if (mapping is EnumMapping)
			{
				this.WriteEnumMethod((EnumMapping)mapping);
				return;
			}
			if (mapping is NullableMapping)
			{
				this.WriteNullableMethod((NullableMapping)mapping);
			}
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x000C01CB File Offset: 0x000BF1CB
		internal void GenerateEnd()
		{
			this.GenerateEnd(new string[0], new XmlMapping[0], new Type[0]);
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x000C01E8 File Offset: 0x000BF1E8
		internal void GenerateEnd(string[] methods, XmlMapping[] xmlMappings, Type[] types)
		{
			base.GenerateReferencedMethods();
			this.GenerateInitCallbacksMethod();
			foreach (object obj in this.createMethods.Values)
			{
				XmlSerializationReaderCodeGen.CreateCollectionInfo createCollectionInfo = (XmlSerializationReaderCodeGen.CreateCollectionInfo)obj;
				this.WriteCreateCollectionMethod(createCollectionInfo);
			}
			base.Writer.WriteLine();
			foreach (object obj2 in this.idNames.Values)
			{
				string text = (string)obj2;
				base.Writer.Write("string ");
				base.Writer.Write(text);
				base.Writer.WriteLine(";");
			}
			base.Writer.WriteLine();
			base.Writer.WriteLine("protected override void InitIDs() {");
			base.Writer.Indent++;
			foreach (object obj3 in this.idNames.Keys)
			{
				string text2 = (string)obj3;
				string text3 = (string)this.idNames[text2];
				base.Writer.Write(text3);
				base.Writer.Write(" = Reader.NameTable.Add(");
				base.WriteQuotedCSharpString(text2);
				base.Writer.WriteLine(");");
			}
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x000C03E4 File Offset: 0x000BF3E4
		internal string GenerateElement(XmlMapping xmlMapping)
		{
			if (!xmlMapping.IsReadable)
			{
				return null;
			}
			if (!xmlMapping.GenerateSerializer)
			{
				throw new ArgumentException(Res.GetString("XmlInternalError"), "xmlMapping");
			}
			if (xmlMapping is XmlTypeMapping)
			{
				return this.GenerateTypeElement((XmlTypeMapping)xmlMapping);
			}
			if (xmlMapping is XmlMembersMapping)
			{
				return this.GenerateMembersElement((XmlMembersMapping)xmlMapping);
			}
			throw new ArgumentException(Res.GetString("XmlInternalError"), "xmlMapping");
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x000C0458 File Offset: 0x000BF458
		private void WriteIsStartTag(string name, string ns)
		{
			base.Writer.Write("if (Reader.IsStartElement(");
			this.WriteID(name);
			base.Writer.Write(", ");
			this.WriteID(ns);
			base.Writer.WriteLine(")) {");
			base.Writer.Indent++;
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x000C04B8 File Offset: 0x000BF4B8
		private void WriteUnknownNode(string func, string node, ElementAccessor e, bool anyIfs)
		{
			if (anyIfs)
			{
				base.Writer.WriteLine("else {");
				base.Writer.Indent++;
			}
			base.Writer.Write(func);
			base.Writer.Write("(");
			base.Writer.Write(node);
			if (e != null)
			{
				base.Writer.Write(", ");
				string text = ((e.Form == XmlSchemaForm.Qualified) ? e.Namespace : "");
				text += ":";
				text += e.Name;
				ReflectionAwareCodeGen.WriteQuotedCSharpString(base.Writer, text);
			}
			base.Writer.WriteLine(");");
			if (anyIfs)
			{
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
			}
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x000C059C File Offset: 0x000BF59C
		private void GenerateInitCallbacksMethod()
		{
			base.Writer.WriteLine();
			base.Writer.WriteLine("protected override void InitCallbacks() {");
			base.Writer.Indent++;
			string text = this.NextMethodName("Array");
			bool flag = false;
			foreach (TypeScope typeScope in base.Scopes)
			{
				foreach (object obj in typeScope.TypeMappings)
				{
					TypeMapping typeMapping = (TypeMapping)obj;
					if (typeMapping.IsSoap && (typeMapping is StructMapping || typeMapping is EnumMapping || typeMapping is ArrayMapping || typeMapping is NullableMapping) && !typeMapping.TypeDesc.IsRoot)
					{
						string text2;
						if (typeMapping is ArrayMapping)
						{
							text2 = text;
							flag = true;
						}
						else
						{
							text2 = (string)base.MethodNames[typeMapping];
						}
						base.Writer.Write("AddReadCallback(");
						this.WriteID(typeMapping.TypeName);
						base.Writer.Write(", ");
						this.WriteID(typeMapping.Namespace);
						base.Writer.Write(", ");
						base.Writer.Write(base.RaCodeGen.GetStringForTypeof(typeMapping.TypeDesc.CSharpName, typeMapping.TypeDesc.UseReflection));
						base.Writer.Write(", new ");
						base.Writer.Write(typeof(XmlSerializationReadCallback).FullName);
						base.Writer.Write("(this.");
						base.Writer.Write(text2);
						base.Writer.WriteLine("));");
					}
				}
			}
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			if (flag)
			{
				base.Writer.WriteLine();
				base.Writer.Write("object ");
				base.Writer.Write(text);
				base.Writer.WriteLine("() {");
				base.Writer.Indent++;
				base.Writer.WriteLine("// dummy array method");
				base.Writer.WriteLine("UnknownNode(null);");
				base.Writer.WriteLine("return null;");
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
			}
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x000C085C File Offset: 0x000BF85C
		private string GenerateMembersElement(XmlMembersMapping xmlMembersMapping)
		{
			if (xmlMembersMapping.Accessor.IsSoap)
			{
				return this.GenerateEncodedMembersElement(xmlMembersMapping);
			}
			return this.GenerateLiteralMembersElement(xmlMembersMapping);
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x000C087C File Offset: 0x000BF87C
		private string GetChoiceIdentifierSource(MemberMapping[] mappings, MemberMapping member)
		{
			string text = null;
			if (member.ChoiceIdentifier != null)
			{
				for (int i = 0; i < mappings.Length; i++)
				{
					if (mappings[i].Name == member.ChoiceIdentifier.MemberName)
					{
						text = "p[" + i.ToString(CultureInfo.InvariantCulture) + "]";
						break;
					}
				}
			}
			return text;
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x000C08DA File Offset: 0x000BF8DA
		private string GetChoiceIdentifierSource(MemberMapping mapping, string parent, TypeDesc parentTypeDesc)
		{
			if (mapping.ChoiceIdentifier == null)
			{
				return "";
			}
			CodeIdentifier.CheckValidIdentifier(mapping.ChoiceIdentifier.MemberName);
			return base.RaCodeGen.GetStringForMember(parent, mapping.ChoiceIdentifier.MemberName, parentTypeDesc);
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x000C0914 File Offset: 0x000BF914
		private string GenerateLiteralMembersElement(XmlMembersMapping xmlMembersMapping)
		{
			ElementAccessor accessor = xmlMembersMapping.Accessor;
			MemberMapping[] members = ((MembersMapping)accessor.Mapping).Members;
			bool hasWrapperElement = ((MembersMapping)accessor.Mapping).HasWrapperElement;
			string text = this.NextMethodName(accessor.Name);
			base.Writer.WriteLine();
			base.Writer.Write("public object[] ");
			base.Writer.Write(text);
			base.Writer.WriteLine("() {");
			base.Writer.Indent++;
			base.Writer.WriteLine("Reader.MoveToContent();");
			base.Writer.Write("object[] p = new object[");
			base.Writer.Write(members.Length.ToString(CultureInfo.InvariantCulture));
			base.Writer.WriteLine("];");
			this.InitializeValueTypes("p", members);
			int num = 0;
			if (hasWrapperElement)
			{
				num = this.WriteWhileNotLoopStart();
				base.Writer.Indent++;
				this.WriteIsStartTag(accessor.Name, (accessor.Form == XmlSchemaForm.Qualified) ? accessor.Namespace : "");
			}
			XmlSerializationReaderCodeGen.Member member = null;
			XmlSerializationReaderCodeGen.Member member2 = null;
			XmlSerializationReaderCodeGen.Member member3 = null;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			ArrayList arrayList3 = new ArrayList();
			for (int i = 0; i < members.Length; i++)
			{
				MemberMapping memberMapping = members[i];
				string text2 = "p[" + i.ToString(CultureInfo.InvariantCulture) + "]";
				string text3 = text2;
				if (memberMapping.Xmlns != null)
				{
					text3 = string.Concat(new string[]
					{
						"((",
						memberMapping.TypeDesc.CSharpName,
						")",
						text2,
						")"
					});
				}
				string choiceIdentifierSource = this.GetChoiceIdentifierSource(members, memberMapping);
				XmlSerializationReaderCodeGen.Member member4 = new XmlSerializationReaderCodeGen.Member(this, text2, text3, "a", i, memberMapping, choiceIdentifierSource);
				XmlSerializationReaderCodeGen.Member member5 = new XmlSerializationReaderCodeGen.Member(this, text2, null, "a", i, memberMapping, choiceIdentifierSource);
				if (!memberMapping.IsSequence)
				{
					member4.ParamsReadSource = "paramsRead[" + i.ToString(CultureInfo.InvariantCulture) + "]";
				}
				if (memberMapping.CheckSpecified == SpecifiedAccessor.ReadWrite)
				{
					string text4 = memberMapping.Name + "Specified";
					for (int j = 0; j < members.Length; j++)
					{
						if (members[j].Name == text4)
						{
							member4.CheckSpecifiedSource = "p[" + j.ToString(CultureInfo.InvariantCulture) + "]";
							break;
						}
					}
				}
				bool flag = false;
				if (memberMapping.Text != null)
				{
					member = member5;
				}
				if (memberMapping.Attribute != null && memberMapping.Attribute.Any)
				{
					member3 = member5;
				}
				if (memberMapping.Attribute != null || memberMapping.Xmlns != null)
				{
					arrayList3.Add(member4);
				}
				else if (memberMapping.Text != null)
				{
					arrayList2.Add(member4);
				}
				if (!memberMapping.IsSequence)
				{
					for (int k = 0; k < memberMapping.Elements.Length; k++)
					{
						if (memberMapping.Elements[k].Any && memberMapping.Elements[k].Name.Length == 0)
						{
							member2 = member5;
							if (memberMapping.Attribute == null && memberMapping.Text == null)
							{
								arrayList2.Add(member5);
							}
							flag = true;
							break;
						}
					}
				}
				if (memberMapping.Attribute != null || memberMapping.Text != null || flag)
				{
					arrayList.Add(member5);
				}
				else if (memberMapping.TypeDesc.IsArrayLike && (memberMapping.Elements.Length != 1 || !(memberMapping.Elements[0].Mapping is ArrayMapping)))
				{
					arrayList.Add(member5);
					arrayList2.Add(member5);
				}
				else
				{
					if (memberMapping.TypeDesc.IsArrayLike && !memberMapping.TypeDesc.IsArray)
					{
						member4.ParamsReadSource = null;
					}
					arrayList.Add(member4);
				}
			}
			XmlSerializationReaderCodeGen.Member[] array = (XmlSerializationReaderCodeGen.Member[])arrayList.ToArray(typeof(XmlSerializationReaderCodeGen.Member));
			XmlSerializationReaderCodeGen.Member[] array2 = (XmlSerializationReaderCodeGen.Member[])arrayList2.ToArray(typeof(XmlSerializationReaderCodeGen.Member));
			if (array.Length > 0 && array[0].Mapping.IsReturnValue)
			{
				base.Writer.WriteLine("IsReturnValue = true;");
			}
			this.WriteParamsRead(members.Length);
			if (arrayList3.Count > 0)
			{
				XmlSerializationReaderCodeGen.Member[] array3 = (XmlSerializationReaderCodeGen.Member[])arrayList3.ToArray(typeof(XmlSerializationReaderCodeGen.Member));
				this.WriteMemberBegin(array3);
				this.WriteAttributes(array3, member3, "UnknownNode", "(object)p");
				this.WriteMemberEnd(array3);
				base.Writer.WriteLine("Reader.MoveToElement();");
			}
			this.WriteMemberBegin(array2);
			if (hasWrapperElement)
			{
				base.Writer.WriteLine("if (Reader.IsEmptyElement) { Reader.Skip(); Reader.MoveToContent(); continue; }");
				base.Writer.WriteLine("Reader.ReadStartElement();");
			}
			if (this.IsSequence(array))
			{
				base.Writer.WriteLine("int state = 0;");
			}
			int num2 = this.WriteWhileNotLoopStart();
			base.Writer.Indent++;
			string text5 = "UnknownNode((object)p, " + this.ExpectedElements(array) + ");";
			this.WriteMemberElements(array, text5, text5, member2, member, null);
			base.Writer.WriteLine("Reader.MoveToContent();");
			this.WriteWhileLoopEnd(num2);
			this.WriteMemberEnd(array2);
			if (hasWrapperElement)
			{
				base.Writer.WriteLine("ReadEndElement();");
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
				this.WriteUnknownNode("UnknownNode", "null", accessor, true);
				base.Writer.WriteLine("Reader.MoveToContent();");
				this.WriteWhileLoopEnd(num);
			}
			base.Writer.WriteLine("return p;");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			return text;
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x000C0F00 File Offset: 0x000BFF00
		private void InitializeValueTypes(string arrayName, MemberMapping[] mappings)
		{
			for (int i = 0; i < mappings.Length; i++)
			{
				if (mappings[i].TypeDesc.IsValueType)
				{
					base.Writer.Write(arrayName);
					base.Writer.Write("[");
					base.Writer.Write(i.ToString(CultureInfo.InvariantCulture));
					base.Writer.Write("] = ");
					if (mappings[i].TypeDesc.IsOptionalValue && mappings[i].TypeDesc.BaseTypeDesc.UseReflection)
					{
						base.Writer.Write("null");
					}
					else
					{
						base.Writer.Write(base.RaCodeGen.GetStringForCreateInstance(mappings[i].TypeDesc.CSharpName, mappings[i].TypeDesc.UseReflection, false, false));
					}
					base.Writer.WriteLine(";");
				}
			}
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000C0FF0 File Offset: 0x000BFFF0
		private string GenerateEncodedMembersElement(XmlMembersMapping xmlMembersMapping)
		{
			ElementAccessor accessor = xmlMembersMapping.Accessor;
			MembersMapping membersMapping = (MembersMapping)accessor.Mapping;
			MemberMapping[] members = membersMapping.Members;
			bool hasWrapperElement = membersMapping.HasWrapperElement;
			bool writeAccessors = membersMapping.WriteAccessors;
			string text = this.NextMethodName(accessor.Name);
			base.Writer.WriteLine();
			base.Writer.Write("public object[] ");
			base.Writer.Write(text);
			base.Writer.WriteLine("() {");
			base.Writer.Indent++;
			base.Writer.WriteLine("Reader.MoveToContent();");
			base.Writer.Write("object[] p = new object[");
			base.Writer.Write(members.Length.ToString(CultureInfo.InvariantCulture));
			base.Writer.WriteLine("];");
			this.InitializeValueTypes("p", members);
			if (hasWrapperElement)
			{
				this.WriteReadNonRoots();
				if (membersMapping.ValidateRpcWrapperElement)
				{
					base.Writer.Write("if (!");
					this.WriteXmlNodeEqual("Reader", accessor.Name, (accessor.Form == XmlSchemaForm.Qualified) ? accessor.Namespace : "");
					base.Writer.WriteLine(") throw CreateUnknownNodeException();");
				}
				base.Writer.WriteLine("bool isEmptyWrapper = Reader.IsEmptyElement;");
				base.Writer.WriteLine("Reader.ReadStartElement();");
			}
			XmlSerializationReaderCodeGen.Member[] array = new XmlSerializationReaderCodeGen.Member[members.Length];
			for (int i = 0; i < members.Length; i++)
			{
				MemberMapping memberMapping = members[i];
				string text2 = "p[" + i.ToString(CultureInfo.InvariantCulture) + "]";
				string text3 = text2;
				if (memberMapping.Xmlns != null)
				{
					text3 = string.Concat(new string[]
					{
						"((",
						memberMapping.TypeDesc.CSharpName,
						")",
						text2,
						")"
					});
				}
				XmlSerializationReaderCodeGen.Member member = new XmlSerializationReaderCodeGen.Member(this, text2, text3, "a", i, memberMapping);
				if (!memberMapping.IsSequence)
				{
					member.ParamsReadSource = "paramsRead[" + i.ToString(CultureInfo.InvariantCulture) + "]";
				}
				array[i] = member;
				if (memberMapping.CheckSpecified == SpecifiedAccessor.ReadWrite)
				{
					string text4 = memberMapping.Name + "Specified";
					for (int j = 0; j < members.Length; j++)
					{
						if (members[j].Name == text4)
						{
							member.CheckSpecifiedSource = "p[" + j.ToString(CultureInfo.InvariantCulture) + "]";
							break;
						}
					}
				}
			}
			string text5 = "fixup_" + text;
			bool flag = this.WriteMemberFixupBegin(array, text5, "p");
			if (array.Length > 0 && array[0].Mapping.IsReturnValue)
			{
				base.Writer.WriteLine("IsReturnValue = true;");
			}
			string text6 = ((!hasWrapperElement && !writeAccessors) ? "hrefList" : null);
			if (text6 != null)
			{
				this.WriteInitCheckTypeHrefList(text6);
			}
			this.WriteParamsRead(members.Length);
			int num = this.WriteWhileNotLoopStart();
			base.Writer.Indent++;
			string text7 = ((text6 == null) ? "UnknownNode((object)p);" : "if (Reader.GetAttribute(\"id\", null) != null) { ReadReferencedElement(); } else { UnknownNode((object)p); }");
			this.WriteMemberElements(array, text7, "UnknownNode((object)p);", null, null, text6);
			base.Writer.WriteLine("Reader.MoveToContent();");
			this.WriteWhileLoopEnd(num);
			if (hasWrapperElement)
			{
				base.Writer.WriteLine("if (!isEmptyWrapper) ReadEndElement();");
			}
			if (text6 != null)
			{
				this.WriteHandleHrefList(array, text6);
			}
			base.Writer.WriteLine("ReadReferencedElements();");
			base.Writer.WriteLine("return p;");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			if (flag)
			{
				this.WriteFixupMethod(text5, array, "object[]", false, false, "p");
			}
			return text;
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x000C13D4 File Offset: 0x000C03D4
		private void WriteCreateCollection(TypeDesc td, string source)
		{
			bool useReflection = td.UseReflection;
			string text = ((td.ArrayElementTypeDesc == null) ? "object" : td.ArrayElementTypeDesc.CSharpName) + "[]";
			bool flag = td.ArrayElementTypeDesc != null && td.ArrayElementTypeDesc.UseReflection;
			if (flag)
			{
				text = typeof(Array).FullName;
			}
			base.Writer.Write(text);
			base.Writer.Write(" ");
			base.Writer.Write("ci =");
			base.Writer.Write("(" + text + ")");
			base.Writer.Write(source);
			base.Writer.WriteLine(";");
			base.Writer.WriteLine("for (int i = 0; i < ci.Length; i++) {");
			base.Writer.Indent++;
			base.Writer.Write(base.RaCodeGen.GetStringForMethod("c", td.CSharpName, "Add", useReflection));
			if (!flag)
			{
				base.Writer.Write("ci[i]");
			}
			else
			{
				base.Writer.Write(base.RaCodeGen.GetReflectionVariable(typeof(Array).FullName, "0") + "[ci , i]");
			}
			if (useReflection)
			{
				base.Writer.WriteLine("}");
			}
			base.Writer.WriteLine(");");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x000C1574 File Offset: 0x000C0574
		private string GenerateTypeElement(XmlTypeMapping xmlTypeMapping)
		{
			ElementAccessor accessor = xmlTypeMapping.Accessor;
			TypeMapping mapping = accessor.Mapping;
			string text = this.NextMethodName(accessor.Name);
			base.Writer.WriteLine();
			base.Writer.Write("public object ");
			base.Writer.Write(text);
			base.Writer.WriteLine("() {");
			base.Writer.Indent++;
			base.Writer.WriteLine("object o = null;");
			XmlSerializationReaderCodeGen.Member[] array = new XmlSerializationReaderCodeGen.Member[]
			{
				new XmlSerializationReaderCodeGen.Member(this, "o", "o", "a", 0, new MemberMapping
				{
					TypeDesc = mapping.TypeDesc,
					Elements = new ElementAccessor[] { accessor }
				})
			};
			base.Writer.WriteLine("Reader.MoveToContent();");
			string text2 = "UnknownNode(null, " + this.ExpectedElements(array) + ");";
			this.WriteMemberElements(array, "throw CreateUnknownNodeException();", text2, accessor.Any ? array[0] : null, null, null);
			if (accessor.IsSoap)
			{
				base.Writer.WriteLine("Referenced(o);");
				base.Writer.WriteLine("ReadReferencedElements();");
			}
			base.Writer.WriteLine("return (object)o;");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			return text;
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x000C16EC File Offset: 0x000C06EC
		private string NextMethodName(string name)
		{
			return "Read" + (++base.NextMethodNumber).ToString(CultureInfo.InvariantCulture) + "_" + CodeIdentifier.MakeValidInternal(name);
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x000C172C File Offset: 0x000C072C
		private string NextIdName(string name)
		{
			string text = "id";
			int num = ++this.nextIdNumber;
			return text + num.ToString(CultureInfo.InvariantCulture) + "_" + CodeIdentifier.MakeValidInternal(name);
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x000C176C File Offset: 0x000C076C
		private void WritePrimitive(TypeMapping mapping, string source)
		{
			if (mapping is EnumMapping)
			{
				string text = base.ReferenceMapping(mapping);
				if (text == null)
				{
					throw new InvalidOperationException(Res.GetString("XmlMissingMethodEnum", new object[] { mapping.TypeDesc.Name }));
				}
				if (mapping.IsSoap)
				{
					base.Writer.Write("(");
					base.Writer.Write(mapping.TypeDesc.CSharpName);
					base.Writer.Write(")");
				}
				base.Writer.Write(text);
				base.Writer.Write("(");
				if (!mapping.IsSoap)
				{
					base.Writer.Write(source);
				}
				base.Writer.Write(")");
				return;
			}
			else
			{
				if (mapping.TypeDesc == base.StringTypeDesc)
				{
					base.Writer.Write(source);
					return;
				}
				if (!(mapping.TypeDesc.FormatterName == "String"))
				{
					if (!mapping.TypeDesc.HasCustomFormatter)
					{
						base.Writer.Write(typeof(XmlConvert).FullName);
						base.Writer.Write(".");
					}
					base.Writer.Write("To");
					base.Writer.Write(mapping.TypeDesc.FormatterName);
					base.Writer.Write("(");
					base.Writer.Write(source);
					base.Writer.Write(")");
					return;
				}
				if (mapping.TypeDesc.CollapseWhitespace)
				{
					base.Writer.Write("CollapseWhitespace(");
					base.Writer.Write(source);
					base.Writer.Write(")");
					return;
				}
				base.Writer.Write(source);
				return;
			}
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x000C1938 File Offset: 0x000C0938
		private string MakeUnique(EnumMapping mapping, string name)
		{
			string text = name;
			object obj = this.Enums[text];
			if (obj != null)
			{
				if (obj == mapping)
				{
					return null;
				}
				int num = 0;
				while (obj != null)
				{
					num++;
					text = name + num.ToString(CultureInfo.InvariantCulture);
					obj = this.Enums[text];
				}
			}
			this.Enums.Add(text, mapping);
			return text;
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x000C1998 File Offset: 0x000C0998
		private string WriteHashtable(EnumMapping mapping, string typeName)
		{
			CodeIdentifier.CheckValidIdentifier(typeName);
			string text = this.MakeUnique(mapping, typeName + "Values");
			if (text == null)
			{
				return CodeIdentifier.GetCSharpName(typeName);
			}
			string text2 = this.MakeUnique(mapping, "_" + text);
			text = CodeIdentifier.GetCSharpName(text);
			base.Writer.WriteLine();
			base.Writer.Write(typeof(Hashtable).FullName);
			base.Writer.Write(" ");
			base.Writer.Write(text2);
			base.Writer.WriteLine(";");
			base.Writer.WriteLine();
			base.Writer.Write("internal ");
			base.Writer.Write(typeof(Hashtable).FullName);
			base.Writer.Write(" ");
			base.Writer.Write(text);
			base.Writer.WriteLine(" {");
			base.Writer.Indent++;
			base.Writer.WriteLine("get {");
			base.Writer.Indent++;
			base.Writer.Write("if ((object)");
			base.Writer.Write(text2);
			base.Writer.WriteLine(" == null) {");
			base.Writer.Indent++;
			base.Writer.Write(typeof(Hashtable).FullName);
			base.Writer.Write(" h = new ");
			base.Writer.Write(typeof(Hashtable).FullName);
			base.Writer.WriteLine("();");
			ConstantMapping[] constants = mapping.Constants;
			for (int i = 0; i < constants.Length; i++)
			{
				base.Writer.Write("h.Add(");
				base.WriteQuotedCSharpString(constants[i].XmlName);
				if (!mapping.TypeDesc.UseReflection)
				{
					base.Writer.Write(", (long)");
					base.Writer.Write(mapping.TypeDesc.CSharpName);
					base.Writer.Write(".@");
					CodeIdentifier.CheckValidIdentifier(constants[i].Name);
					base.Writer.Write(constants[i].Name);
				}
				else
				{
					base.Writer.Write(", ");
					base.Writer.Write(constants[i].Value.ToString(CultureInfo.InvariantCulture) + "L");
				}
				base.Writer.WriteLine(");");
			}
			base.Writer.Write(text2);
			base.Writer.WriteLine(" = h;");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			base.Writer.Write("return ");
			base.Writer.Write(text2);
			base.Writer.WriteLine(";");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			return text;
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000C1CF8 File Offset: 0x000C0CF8
		private void WriteEnumMethod(EnumMapping mapping)
		{
			string text = null;
			if (mapping.IsFlags)
			{
				text = this.WriteHashtable(mapping, mapping.TypeDesc.Name);
			}
			string text2 = (string)base.MethodNames[mapping];
			base.Writer.WriteLine();
			bool useReflection = mapping.TypeDesc.UseReflection;
			string csharpName = mapping.TypeDesc.CSharpName;
			if (mapping.IsSoap)
			{
				base.Writer.Write("object");
				base.Writer.Write(" ");
				base.Writer.Write(text2);
				base.Writer.WriteLine("() {");
				base.Writer.Indent++;
				base.Writer.WriteLine("string s = Reader.ReadElementString();");
			}
			else
			{
				base.Writer.Write(useReflection ? "object" : csharpName);
				base.Writer.Write(" ");
				base.Writer.Write(text2);
				base.Writer.WriteLine("(string s) {");
				base.Writer.Indent++;
			}
			ConstantMapping[] constants = mapping.Constants;
			if (mapping.IsFlags)
			{
				if (useReflection)
				{
					base.Writer.Write("return ");
					base.Writer.Write(typeof(Enum).FullName);
					base.Writer.Write(".ToObject(");
					base.Writer.Write(base.RaCodeGen.GetStringForTypeof(csharpName, useReflection));
					base.Writer.Write(", ToEnum(s, ");
					base.Writer.Write(text);
					base.Writer.Write(", ");
					base.WriteQuotedCSharpString(csharpName);
					base.Writer.WriteLine("));");
				}
				else
				{
					base.Writer.Write("return (");
					base.Writer.Write(csharpName);
					base.Writer.Write(")ToEnum(s, ");
					base.Writer.Write(text);
					base.Writer.Write(", ");
					base.WriteQuotedCSharpString(csharpName);
					base.Writer.WriteLine(");");
				}
			}
			else
			{
				base.Writer.WriteLine("switch (s) {");
				base.Writer.Indent++;
				Hashtable hashtable = new Hashtable();
				foreach (ConstantMapping constantMapping in constants)
				{
					CodeIdentifier.CheckValidIdentifier(constantMapping.Name);
					if (hashtable[constantMapping.XmlName] == null)
					{
						base.Writer.Write("case ");
						base.WriteQuotedCSharpString(constantMapping.XmlName);
						base.Writer.Write(": return ");
						base.Writer.Write(base.RaCodeGen.GetStringForEnumMember(csharpName, constantMapping.Name, useReflection));
						base.Writer.WriteLine(";");
						hashtable[constantMapping.XmlName] = constantMapping.XmlName;
					}
				}
				base.Writer.Write("default: throw CreateUnknownConstantException(s, ");
				base.Writer.Write(base.RaCodeGen.GetStringForTypeof(csharpName, useReflection));
				base.Writer.WriteLine(");");
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
			}
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000C2084 File Offset: 0x000C1084
		private void WriteDerivedTypes(StructMapping mapping, bool isTypedReturn, string returnTypeName)
		{
			for (StructMapping structMapping = mapping.DerivedMappings; structMapping != null; structMapping = structMapping.NextDerivedMapping)
			{
				base.Writer.Write("else if (");
				this.WriteQNameEqual("xsiType", structMapping.TypeName, structMapping.Namespace);
				base.Writer.WriteLine(")");
				base.Writer.Indent++;
				string text = base.ReferenceMapping(structMapping);
				base.Writer.Write("return ");
				if (structMapping.TypeDesc.UseReflection && isTypedReturn)
				{
					base.Writer.Write("(" + returnTypeName + ")");
				}
				base.Writer.Write(text);
				base.Writer.Write("(");
				if (structMapping.TypeDesc.IsNullable)
				{
					base.Writer.Write("isNullable, ");
				}
				base.Writer.WriteLine("false);");
				base.Writer.Indent--;
				this.WriteDerivedTypes(structMapping, isTypedReturn, returnTypeName);
			}
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x000C219C File Offset: 0x000C119C
		private void WriteEnumAndArrayTypes()
		{
			foreach (TypeScope typeScope in base.Scopes)
			{
				foreach (object obj in typeScope.TypeMappings)
				{
					Mapping mapping = (Mapping)obj;
					if (!mapping.IsSoap)
					{
						if (mapping is EnumMapping)
						{
							EnumMapping enumMapping = (EnumMapping)mapping;
							base.Writer.Write("else if (");
							this.WriteQNameEqual("xsiType", enumMapping.TypeName, enumMapping.Namespace);
							base.Writer.WriteLine(") {");
							base.Writer.Indent++;
							base.Writer.WriteLine("Reader.ReadStartElement();");
							string text = base.ReferenceMapping(enumMapping);
							base.Writer.Write("object e = ");
							base.Writer.Write(text);
							base.Writer.WriteLine("(CollapseWhitespace(Reader.ReadString()));");
							base.Writer.WriteLine("ReadEndElement();");
							base.Writer.WriteLine("return e;");
							base.Writer.Indent--;
							base.Writer.WriteLine("}");
						}
						else if (mapping is ArrayMapping)
						{
							ArrayMapping arrayMapping = (ArrayMapping)mapping;
							if (arrayMapping.TypeDesc.HasDefaultConstructor)
							{
								base.Writer.Write("else if (");
								this.WriteQNameEqual("xsiType", arrayMapping.TypeName, arrayMapping.Namespace);
								base.Writer.WriteLine(") {");
								base.Writer.Indent++;
								XmlSerializationReaderCodeGen.Member member = new XmlSerializationReaderCodeGen.Member(this, "a", "z", 0, new MemberMapping
								{
									TypeDesc = arrayMapping.TypeDesc,
									Elements = arrayMapping.Elements
								});
								TypeDesc typeDesc = arrayMapping.TypeDesc;
								string csharpName = arrayMapping.TypeDesc.CSharpName;
								if (typeDesc.UseReflection)
								{
									if (typeDesc.IsArray)
									{
										base.Writer.Write(typeof(Array).FullName);
									}
									else
									{
										base.Writer.Write("object");
									}
								}
								else
								{
									base.Writer.Write(csharpName);
								}
								base.Writer.Write(" a = ");
								if (arrayMapping.TypeDesc.IsValueType)
								{
									base.Writer.Write(base.RaCodeGen.GetStringForCreateInstance(csharpName, typeDesc.UseReflection, false, false));
									base.Writer.WriteLine(";");
								}
								else
								{
									base.Writer.WriteLine("null;");
								}
								this.WriteArray(member.Source, member.ArrayName, arrayMapping, false, false, -1);
								base.Writer.WriteLine("return a;");
								base.Writer.Indent--;
								base.Writer.WriteLine("}");
							}
						}
					}
				}
			}
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000C24DC File Offset: 0x000C14DC
		private void WriteNullableMethod(NullableMapping nullableMapping)
		{
			string text = (string)base.MethodNames[nullableMapping];
			bool useReflection = nullableMapping.BaseMapping.TypeDesc.UseReflection;
			string text2 = (useReflection ? "object" : nullableMapping.TypeDesc.CSharpName);
			base.Writer.WriteLine();
			base.Writer.Write(text2);
			base.Writer.Write(" ");
			base.Writer.Write(text);
			base.Writer.WriteLine("(bool checkType) {");
			base.Writer.Indent++;
			base.Writer.Write(text2);
			base.Writer.Write(" o = ");
			if (useReflection)
			{
				base.Writer.Write("null");
			}
			else
			{
				base.Writer.Write("default(");
				base.Writer.Write(text2);
				base.Writer.Write(")");
			}
			base.Writer.WriteLine(";");
			base.Writer.WriteLine("if (ReadNull())");
			base.Writer.Indent++;
			base.Writer.WriteLine("return o;");
			base.Writer.Indent--;
			this.WriteElement("o", null, null, new ElementAccessor
			{
				Mapping = nullableMapping.BaseMapping,
				Any = false,
				IsNullable = nullableMapping.BaseMapping.TypeDesc.IsNullable
			}, null, null, false, false, -1, -1);
			base.Writer.WriteLine("return o;");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000C26A1 File Offset: 0x000C16A1
		private void WriteStructMethod(StructMapping structMapping)
		{
			if (structMapping.IsSoap)
			{
				this.WriteEncodedStructMethod(structMapping);
				return;
			}
			this.WriteLiteralStructMethod(structMapping);
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000C26BC File Offset: 0x000C16BC
		private void WriteLiteralStructMethod(StructMapping structMapping)
		{
			string text = (string)base.MethodNames[structMapping];
			bool useReflection = structMapping.TypeDesc.UseReflection;
			string text2 = (useReflection ? "object" : structMapping.TypeDesc.CSharpName);
			base.Writer.WriteLine();
			base.Writer.Write(text2);
			base.Writer.Write(" ");
			base.Writer.Write(text);
			base.Writer.Write("(");
			if (structMapping.TypeDesc.IsNullable)
			{
				base.Writer.Write("bool isNullable, ");
			}
			base.Writer.WriteLine("bool checkType) {");
			base.Writer.Indent++;
			base.Writer.Write(typeof(XmlQualifiedName).FullName);
			base.Writer.WriteLine(" xsiType = checkType ? GetXsiType() : null;");
			base.Writer.WriteLine("bool isNull = false;");
			if (structMapping.TypeDesc.IsNullable)
			{
				base.Writer.WriteLine("if (isNullable) isNull = ReadNull();");
			}
			base.Writer.WriteLine("if (checkType) {");
			if (structMapping.TypeDesc.IsRoot)
			{
				base.Writer.Indent++;
				base.Writer.WriteLine("if (isNull) {");
				base.Writer.Indent++;
				base.Writer.WriteLine("if (xsiType != null) return (" + text2 + ")ReadTypedNull(xsiType);");
				base.Writer.Write("else return ");
				if (structMapping.TypeDesc.IsValueType)
				{
					base.Writer.Write(base.RaCodeGen.GetStringForCreateInstance(structMapping.TypeDesc.CSharpName, useReflection, false, false));
					base.Writer.WriteLine(";");
				}
				else
				{
					base.Writer.WriteLine("null;");
				}
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
			}
			base.Writer.Write("if (xsiType == null");
			if (!structMapping.TypeDesc.IsRoot)
			{
				base.Writer.Write(" || ");
				this.WriteQNameEqual("xsiType", structMapping.TypeName, structMapping.Namespace);
			}
			base.Writer.WriteLine(") {");
			if (structMapping.TypeDesc.IsRoot)
			{
				base.Writer.Indent++;
				base.Writer.WriteLine("return ReadTypedPrimitive(new System.Xml.XmlQualifiedName(\"anyType\", \"http://www.w3.org/2001/XMLSchema\"));");
				base.Writer.Indent--;
			}
			base.Writer.WriteLine("}");
			this.WriteDerivedTypes(structMapping, !useReflection && !structMapping.TypeDesc.IsRoot, text2);
			if (structMapping.TypeDesc.IsRoot)
			{
				this.WriteEnumAndArrayTypes();
			}
			base.Writer.WriteLine("else");
			base.Writer.Indent++;
			if (structMapping.TypeDesc.IsRoot)
			{
				base.Writer.Write("return ReadTypedPrimitive((");
			}
			else
			{
				base.Writer.Write("throw CreateUnknownTypeException((");
			}
			base.Writer.Write(typeof(XmlQualifiedName).FullName);
			base.Writer.WriteLine(")xsiType);");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			if (structMapping.TypeDesc.IsNullable)
			{
				base.Writer.WriteLine("if (isNull) return null;");
			}
			if (structMapping.TypeDesc.IsAbstract)
			{
				base.Writer.Write("throw CreateAbstractTypeException(");
				base.WriteQuotedCSharpString(structMapping.TypeName);
				base.Writer.Write(", ");
				base.WriteQuotedCSharpString(structMapping.Namespace);
				base.Writer.WriteLine(");");
			}
			else
			{
				if (structMapping.TypeDesc.Type != null && typeof(XmlSchemaObject).IsAssignableFrom(structMapping.TypeDesc.Type))
				{
					base.Writer.WriteLine("DecodeName = false;");
				}
				this.WriteCreateMapping(structMapping, "o");
				MemberMapping[] allMembers = TypeScope.GetAllMembers(structMapping);
				XmlSerializationReaderCodeGen.Member member = null;
				XmlSerializationReaderCodeGen.Member member2 = null;
				XmlSerializationReaderCodeGen.Member member3 = null;
				bool flag = structMapping.HasExplicitSequence();
				ArrayList arrayList = new ArrayList(allMembers.Length);
				ArrayList arrayList2 = new ArrayList(allMembers.Length);
				ArrayList arrayList3 = new ArrayList(allMembers.Length);
				for (int i = 0; i < allMembers.Length; i++)
				{
					MemberMapping memberMapping = allMembers[i];
					CodeIdentifier.CheckValidIdentifier(memberMapping.Name);
					string stringForMember = base.RaCodeGen.GetStringForMember("o", memberMapping.Name, structMapping.TypeDesc);
					XmlSerializationReaderCodeGen.Member member4 = new XmlSerializationReaderCodeGen.Member(this, stringForMember, "a", i, memberMapping, this.GetChoiceIdentifierSource(memberMapping, "o", structMapping.TypeDesc));
					if (!memberMapping.IsSequence)
					{
						member4.ParamsReadSource = "paramsRead[" + i.ToString(CultureInfo.InvariantCulture) + "]";
					}
					member4.IsNullable = memberMapping.TypeDesc.IsNullable;
					if (memberMapping.CheckSpecified == SpecifiedAccessor.ReadWrite)
					{
						member4.CheckSpecifiedSource = base.RaCodeGen.GetStringForMember("o", memberMapping.Name + "Specified", structMapping.TypeDesc);
					}
					if (memberMapping.Text != null)
					{
						member = member4;
					}
					if (memberMapping.Attribute != null && memberMapping.Attribute.Any)
					{
						member3 = member4;
					}
					if (!flag)
					{
						for (int j = 0; j < memberMapping.Elements.Length; j++)
						{
							if (memberMapping.Elements[j].Any && (memberMapping.Elements[j].Name == null || memberMapping.Elements[j].Name.Length == 0))
							{
								member2 = member4;
								break;
							}
						}
					}
					else if (memberMapping.IsParticle && !memberMapping.IsSequence)
					{
						StructMapping structMapping2;
						structMapping.FindDeclaringMapping(memberMapping, out structMapping2, structMapping.TypeName);
						throw new InvalidOperationException(Res.GetString("XmlSequenceHierarchy", new object[]
						{
							structMapping.TypeDesc.FullName,
							memberMapping.Name,
							structMapping2.TypeDesc.FullName,
							"Order"
						}));
					}
					if (memberMapping.Attribute == null && memberMapping.Elements.Length == 1 && memberMapping.Elements[0].Mapping is ArrayMapping)
					{
						arrayList3.Add(new XmlSerializationReaderCodeGen.Member(this, stringForMember, stringForMember, "a", i, memberMapping, this.GetChoiceIdentifierSource(memberMapping, "o", structMapping.TypeDesc))
						{
							CheckSpecifiedSource = member4.CheckSpecifiedSource
						});
					}
					else
					{
						arrayList3.Add(member4);
					}
					if (memberMapping.TypeDesc.IsArrayLike)
					{
						arrayList.Add(member4);
						if (memberMapping.TypeDesc.IsArrayLike && (memberMapping.Elements.Length != 1 || !(memberMapping.Elements[0].Mapping is ArrayMapping)))
						{
							member4.ParamsReadSource = null;
							if (member4 != member && member4 != member2)
							{
								arrayList2.Add(member4);
							}
						}
						else if (!memberMapping.TypeDesc.IsArray)
						{
							member4.ParamsReadSource = null;
						}
					}
				}
				if (member2 != null)
				{
					arrayList2.Add(member2);
				}
				if (member != null && member != member2)
				{
					arrayList2.Add(member);
				}
				XmlSerializationReaderCodeGen.Member[] array = (XmlSerializationReaderCodeGen.Member[])arrayList.ToArray(typeof(XmlSerializationReaderCodeGen.Member));
				XmlSerializationReaderCodeGen.Member[] array2 = (XmlSerializationReaderCodeGen.Member[])arrayList2.ToArray(typeof(XmlSerializationReaderCodeGen.Member));
				XmlSerializationReaderCodeGen.Member[] array3 = (XmlSerializationReaderCodeGen.Member[])arrayList3.ToArray(typeof(XmlSerializationReaderCodeGen.Member));
				this.WriteMemberBegin(array);
				this.WriteParamsRead(allMembers.Length);
				this.WriteAttributes(array3, member3, "UnknownNode", "(object)o");
				if (member3 != null)
				{
					this.WriteMemberEnd(array);
				}
				base.Writer.WriteLine("Reader.MoveToElement();");
				base.Writer.WriteLine("if (Reader.IsEmptyElement) {");
				base.Writer.Indent++;
				base.Writer.WriteLine("Reader.Skip();");
				this.WriteMemberEnd(array2);
				base.Writer.WriteLine("return o;");
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
				base.Writer.WriteLine("Reader.ReadStartElement();");
				if (this.IsSequence(array3))
				{
					base.Writer.WriteLine("int state = 0;");
				}
				int num = this.WriteWhileNotLoopStart();
				base.Writer.Indent++;
				string text3 = "UnknownNode((object)o, " + this.ExpectedElements(array3) + ");";
				this.WriteMemberElements(array3, text3, text3, member2, member, null);
				base.Writer.WriteLine("Reader.MoveToContent();");
				this.WriteWhileLoopEnd(num);
				this.WriteMemberEnd(array2);
				base.Writer.WriteLine("ReadEndElement();");
				base.Writer.WriteLine("return o;");
			}
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000C2FF0 File Offset: 0x000C1FF0
		private void WriteEncodedStructMethod(StructMapping structMapping)
		{
			if (structMapping.TypeDesc.IsRoot)
			{
				return;
			}
			bool useReflection = structMapping.TypeDesc.UseReflection;
			string text = (string)base.MethodNames[structMapping];
			base.Writer.WriteLine();
			base.Writer.Write("object");
			base.Writer.Write(" ");
			base.Writer.Write(text);
			base.Writer.Write("(");
			base.Writer.WriteLine(") {");
			base.Writer.Indent++;
			XmlSerializationReaderCodeGen.Member[] array;
			bool flag;
			string text2;
			if (structMapping.TypeDesc.IsAbstract)
			{
				base.Writer.Write("throw CreateAbstractTypeException(");
				base.WriteQuotedCSharpString(structMapping.TypeName);
				base.Writer.Write(", ");
				base.WriteQuotedCSharpString(structMapping.Namespace);
				base.Writer.WriteLine(");");
				array = new XmlSerializationReaderCodeGen.Member[0];
				flag = false;
				text2 = null;
			}
			else
			{
				this.WriteCreateMapping(structMapping, "o");
				MemberMapping[] allMembers = TypeScope.GetAllMembers(structMapping);
				array = new XmlSerializationReaderCodeGen.Member[allMembers.Length];
				for (int i = 0; i < allMembers.Length; i++)
				{
					MemberMapping memberMapping = allMembers[i];
					CodeIdentifier.CheckValidIdentifier(memberMapping.Name);
					string stringForMember = base.RaCodeGen.GetStringForMember("o", memberMapping.Name, structMapping.TypeDesc);
					XmlSerializationReaderCodeGen.Member member = new XmlSerializationReaderCodeGen.Member(this, stringForMember, stringForMember, "a", i, memberMapping, this.GetChoiceIdentifierSource(memberMapping, "o", structMapping.TypeDesc));
					if (memberMapping.CheckSpecified == SpecifiedAccessor.ReadWrite)
					{
						member.CheckSpecifiedSource = base.RaCodeGen.GetStringForMember("o", memberMapping.Name + "Specified", structMapping.TypeDesc);
					}
					if (!memberMapping.IsSequence)
					{
						member.ParamsReadSource = "paramsRead[" + i.ToString(CultureInfo.InvariantCulture) + "]";
					}
					array[i] = member;
				}
				text2 = "fixup_" + text;
				flag = this.WriteMemberFixupBegin(array, text2, "o");
				this.WriteParamsRead(allMembers.Length);
				this.WriteAttributes(array, null, "UnknownNode", "(object)o");
				base.Writer.WriteLine("Reader.MoveToElement();");
				base.Writer.WriteLine("if (Reader.IsEmptyElement) { Reader.Skip(); return o; }");
				base.Writer.WriteLine("Reader.ReadStartElement();");
				int num = this.WriteWhileNotLoopStart();
				base.Writer.Indent++;
				this.WriteMemberElements(array, "UnknownNode((object)o);", "UnknownNode((object)o);", null, null, null);
				base.Writer.WriteLine("Reader.MoveToContent();");
				this.WriteWhileLoopEnd(num);
				base.Writer.WriteLine("ReadEndElement();");
				base.Writer.WriteLine("return o;");
			}
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			if (flag)
			{
				this.WriteFixupMethod(text2, array, structMapping.TypeDesc.CSharpName, structMapping.TypeDesc.UseReflection, true, "o");
			}
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000C330C File Offset: 0x000C230C
		private void WriteFixupMethod(string fixupMethodName, XmlSerializationReaderCodeGen.Member[] members, string typeName, bool useReflection, bool typed, string source)
		{
			base.Writer.WriteLine();
			base.Writer.Write("void ");
			base.Writer.Write(fixupMethodName);
			base.Writer.WriteLine("(object objFixup) {");
			base.Writer.Indent++;
			base.Writer.WriteLine("Fixup fixup = (Fixup)objFixup;");
			this.WriteLocalDecl(typeName, source, "fixup.Source", useReflection);
			base.Writer.WriteLine("string[] ids = fixup.Ids;");
			foreach (XmlSerializationReaderCodeGen.Member member in members)
			{
				if (member.MultiRef)
				{
					string text = member.FixupIndex.ToString(CultureInfo.InvariantCulture);
					base.Writer.Write("if (ids[");
					base.Writer.Write(text);
					base.Writer.WriteLine("] != null) {");
					base.Writer.Indent++;
					string arraySource = member.ArraySource;
					string text2 = "GetTarget(ids[" + text + "])";
					TypeDesc typeDesc = member.Mapping.TypeDesc;
					if (typeDesc.IsCollection || typeDesc.IsEnumerable)
					{
						this.WriteAddCollectionFixup(typeDesc, member.Mapping.ReadOnly, arraySource, text2);
					}
					else
					{
						if (typed)
						{
							base.Writer.WriteLine("try {");
							base.Writer.Indent++;
							this.WriteSourceBeginTyped(arraySource, member.Mapping.TypeDesc);
						}
						else
						{
							this.WriteSourceBegin(arraySource);
						}
						base.Writer.Write(text2);
						this.WriteSourceEnd(arraySource);
						base.Writer.WriteLine(";");
						if (member.Mapping.CheckSpecified == SpecifiedAccessor.ReadWrite && member.CheckSpecifiedSource != null && member.CheckSpecifiedSource.Length > 0)
						{
							base.Writer.Write(member.CheckSpecifiedSource);
							base.Writer.WriteLine(" = true;");
						}
						if (typed)
						{
							this.WriteCatchCastException(member.Mapping.TypeDesc, text2, "ids[" + text + "]");
						}
					}
					base.Writer.Indent--;
					base.Writer.WriteLine("}");
				}
			}
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000C3578 File Offset: 0x000C2578
		private void WriteAddCollectionFixup(TypeDesc typeDesc, bool readOnly, string memberSource, string targetSource)
		{
			base.Writer.WriteLine("// get array of the collection items");
			bool useReflection = typeDesc.UseReflection;
			XmlSerializationReaderCodeGen.CreateCollectionInfo createCollectionInfo = (XmlSerializationReaderCodeGen.CreateCollectionInfo)this.createMethods[typeDesc];
			if (createCollectionInfo == null)
			{
				string text = "create";
				int num = ++this.nextCreateMethodNumber;
				string text2 = text + num.ToString(CultureInfo.InvariantCulture) + "_" + typeDesc.Name;
				createCollectionInfo = new XmlSerializationReaderCodeGen.CreateCollectionInfo(text2, typeDesc);
				this.createMethods.Add(typeDesc, createCollectionInfo);
			}
			base.Writer.Write("if ((object)(");
			base.Writer.Write(memberSource);
			base.Writer.WriteLine(") == null) {");
			base.Writer.Indent++;
			if (readOnly)
			{
				base.Writer.Write("throw CreateReadOnlyCollectionException(");
				base.WriteQuotedCSharpString(typeDesc.CSharpName);
				base.Writer.WriteLine(");");
			}
			else
			{
				base.Writer.Write(memberSource);
				base.Writer.Write(" = ");
				base.Writer.Write(base.RaCodeGen.GetStringForCreateInstance(typeDesc.CSharpName, typeDesc.UseReflection, typeDesc.CannotNew, true));
				base.Writer.WriteLine(";");
			}
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			base.Writer.Write("CollectionFixup collectionFixup = new CollectionFixup(");
			base.Writer.Write(memberSource);
			base.Writer.Write(", ");
			base.Writer.Write("new ");
			base.Writer.Write(typeof(XmlSerializationCollectionFixupCallback).FullName);
			base.Writer.Write("(this.");
			base.Writer.Write(createCollectionInfo.Name);
			base.Writer.Write("), ");
			base.Writer.Write(targetSource);
			base.Writer.WriteLine(");");
			base.Writer.WriteLine("AddFixup(collectionFixup);");
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000C3798 File Offset: 0x000C2798
		private void WriteCreateCollectionMethod(XmlSerializationReaderCodeGen.CreateCollectionInfo c)
		{
			base.Writer.Write("void ");
			base.Writer.Write(c.Name);
			base.Writer.WriteLine("(object collection, object collectionItems) {");
			base.Writer.Indent++;
			base.Writer.WriteLine("if (collectionItems == null) return;");
			base.Writer.WriteLine("if (collection == null) return;");
			TypeDesc typeDesc = c.TypeDesc;
			bool useReflection = typeDesc.UseReflection;
			string csharpName = typeDesc.CSharpName;
			this.WriteLocalDecl(csharpName, "c", "collection", useReflection);
			this.WriteCreateCollection(typeDesc, "collectionItems");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x000C3860 File Offset: 0x000C2860
		private void WriteQNameEqual(string source, string name, string ns)
		{
			base.Writer.Write("((object) ((");
			base.Writer.Write(typeof(XmlQualifiedName).FullName);
			base.Writer.Write(")");
			base.Writer.Write(source);
			base.Writer.Write(").Name == (object)");
			this.WriteID(name);
			base.Writer.Write(" && (object) ((");
			base.Writer.Write(typeof(XmlQualifiedName).FullName);
			base.Writer.Write(")");
			base.Writer.Write(source);
			base.Writer.Write(").Namespace == (object)");
			this.WriteID(ns);
			base.Writer.Write(")");
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000C3938 File Offset: 0x000C2938
		private void WriteXmlNodeEqual(string source, string name, string ns)
		{
			base.Writer.Write("(");
			if (name != null && name.Length > 0)
			{
				base.Writer.Write("(object) ");
				base.Writer.Write(source);
				base.Writer.Write(".LocalName == (object)");
				this.WriteID(name);
				base.Writer.Write(" && ");
			}
			base.Writer.Write("(object) ");
			base.Writer.Write(source);
			base.Writer.Write(".NamespaceURI == (object)");
			this.WriteID(ns);
			base.Writer.Write(")");
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x000C39E8 File Offset: 0x000C29E8
		private void WriteID(string name)
		{
			if (name == null)
			{
				name = "";
			}
			string text = (string)this.idNames[name];
			if (text == null)
			{
				text = this.NextIdName(name);
				this.idNames.Add(name, text);
			}
			base.Writer.Write(text);
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x000C3A38 File Offset: 0x000C2A38
		private void WriteAttributes(XmlSerializationReaderCodeGen.Member[] members, XmlSerializationReaderCodeGen.Member anyAttribute, string elseCall, string firstParam)
		{
			int num = 0;
			XmlSerializationReaderCodeGen.Member member = null;
			ArrayList arrayList = new ArrayList();
			base.Writer.WriteLine("while (Reader.MoveToNextAttribute()) {");
			base.Writer.Indent++;
			foreach (XmlSerializationReaderCodeGen.Member member2 in members)
			{
				if (member2.Mapping.Xmlns != null)
				{
					member = member2;
				}
				else if (!member2.Mapping.Ignore)
				{
					AttributeAccessor attribute = member2.Mapping.Attribute;
					if (attribute != null && !attribute.Any)
					{
						arrayList.Add(attribute);
						if (num++ > 0)
						{
							base.Writer.Write("else ");
						}
						base.Writer.Write("if (");
						if (member2.ParamsReadSource != null)
						{
							base.Writer.Write("!");
							base.Writer.Write(member2.ParamsReadSource);
							base.Writer.Write(" && ");
						}
						if (attribute.IsSpecialXmlNamespace)
						{
							this.WriteXmlNodeEqual("Reader", attribute.Name, "http://www.w3.org/XML/1998/namespace");
						}
						else
						{
							this.WriteXmlNodeEqual("Reader", attribute.Name, (attribute.Form == XmlSchemaForm.Qualified) ? attribute.Namespace : "");
						}
						base.Writer.WriteLine(") {");
						base.Writer.Indent++;
						this.WriteAttribute(member2);
						base.Writer.Indent--;
						base.Writer.WriteLine("}");
					}
				}
			}
			if (num > 0)
			{
				base.Writer.Write("else ");
			}
			if (member != null)
			{
				base.Writer.WriteLine("if (IsXmlnsAttribute(Reader.Name)) {");
				base.Writer.Indent++;
				base.Writer.Write("if (");
				base.Writer.Write(member.Source);
				base.Writer.Write(" == null) ");
				base.Writer.Write(member.Source);
				base.Writer.Write(" = new ");
				base.Writer.Write(member.Mapping.TypeDesc.CSharpName);
				base.Writer.WriteLine("();");
				base.Writer.Write(string.Concat(new string[]
				{
					"((",
					member.Mapping.TypeDesc.CSharpName,
					")",
					member.ArraySource,
					")"
				}));
				base.Writer.WriteLine(".Add(Reader.Name.Length == 5 ? \"\" : Reader.LocalName, Reader.Value);");
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
				base.Writer.WriteLine("else {");
				base.Writer.Indent++;
			}
			else
			{
				base.Writer.WriteLine("if (!IsXmlnsAttribute(Reader.Name)) {");
				base.Writer.Indent++;
			}
			if (anyAttribute != null)
			{
				base.Writer.Write(typeof(XmlAttribute).FullName);
				base.Writer.Write(" attr = ");
				base.Writer.Write("(");
				base.Writer.Write(typeof(XmlAttribute).FullName);
				base.Writer.WriteLine(") Document.ReadNode(Reader);");
				base.Writer.WriteLine("ParseWsdlArrayType(attr);");
				this.WriteAttribute(anyAttribute);
			}
			else
			{
				base.Writer.Write(elseCall);
				base.Writer.Write("(");
				base.Writer.Write(firstParam);
				if (arrayList.Count > 0)
				{
					base.Writer.Write(", ");
					string text = "";
					for (int j = 0; j < arrayList.Count; j++)
					{
						AttributeAccessor attributeAccessor = (AttributeAccessor)arrayList[j];
						if (j > 0)
						{
							text += ", ";
						}
						text += (attributeAccessor.IsSpecialXmlNamespace ? "http://www.w3.org/XML/1998/namespace" : (((attributeAccessor.Form == XmlSchemaForm.Qualified) ? attributeAccessor.Namespace : "") + ":" + attributeAccessor.Name));
					}
					base.WriteQuotedCSharpString(text);
				}
				base.Writer.WriteLine(");");
			}
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000C3F0C File Offset: 0x000C2F0C
		private void WriteAttribute(XmlSerializationReaderCodeGen.Member member)
		{
			AttributeAccessor attribute = member.Mapping.Attribute;
			if (attribute.Mapping is SpecialMapping)
			{
				SpecialMapping specialMapping = (SpecialMapping)attribute.Mapping;
				if (specialMapping.TypeDesc.Kind == TypeKind.Attribute)
				{
					this.WriteSourceBegin(member.ArraySource);
					base.Writer.Write("attr");
					this.WriteSourceEnd(member.ArraySource);
					base.Writer.WriteLine(";");
				}
				else
				{
					if (!specialMapping.TypeDesc.CanBeAttributeValue)
					{
						throw new InvalidOperationException(Res.GetString("XmlInternalError"));
					}
					base.Writer.Write("if (attr is ");
					base.Writer.Write(typeof(XmlAttribute).FullName);
					base.Writer.WriteLine(") {");
					base.Writer.Indent++;
					this.WriteSourceBegin(member.ArraySource);
					base.Writer.Write("(");
					base.Writer.Write(typeof(XmlAttribute).FullName);
					base.Writer.Write(")attr");
					this.WriteSourceEnd(member.ArraySource);
					base.Writer.WriteLine(";");
					base.Writer.Indent--;
					base.Writer.WriteLine("}");
				}
			}
			else if (attribute.IsList)
			{
				base.Writer.WriteLine("string listValues = Reader.Value;");
				base.Writer.WriteLine("string[] vals = listValues.Split(null);");
				base.Writer.WriteLine("for (int i = 0; i < vals.Length; i++) {");
				base.Writer.Indent++;
				string arraySource = this.GetArraySource(member.Mapping.TypeDesc, member.ArrayName);
				this.WriteSourceBegin(arraySource);
				this.WritePrimitive(attribute.Mapping, "vals[i]");
				this.WriteSourceEnd(arraySource);
				base.Writer.WriteLine(";");
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
			}
			else
			{
				this.WriteSourceBegin(member.ArraySource);
				this.WritePrimitive(attribute.Mapping, attribute.IsList ? "vals[i]" : "Reader.Value");
				this.WriteSourceEnd(member.ArraySource);
				base.Writer.WriteLine(";");
			}
			if (member.Mapping.CheckSpecified == SpecifiedAccessor.ReadWrite && member.CheckSpecifiedSource != null && member.CheckSpecifiedSource.Length > 0)
			{
				base.Writer.Write(member.CheckSpecifiedSource);
				base.Writer.WriteLine(" = true;");
			}
			if (member.ParamsReadSource != null)
			{
				base.Writer.Write(member.ParamsReadSource);
				base.Writer.WriteLine(" = true;");
			}
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x000C41F4 File Offset: 0x000C31F4
		private bool WriteMemberFixupBegin(XmlSerializationReaderCodeGen.Member[] members, string fixupMethodName, string source)
		{
			int num = 0;
			foreach (XmlSerializationReaderCodeGen.Member member in members)
			{
				if (member.Mapping.Elements.Length != 0)
				{
					TypeMapping mapping = member.Mapping.Elements[0].Mapping;
					if (mapping is StructMapping || mapping is ArrayMapping || mapping is PrimitiveMapping || mapping is NullableMapping)
					{
						member.MultiRef = true;
						member.FixupIndex = num++;
					}
				}
			}
			if (num > 0)
			{
				base.Writer.Write("Fixup fixup = new Fixup(");
				base.Writer.Write(source);
				base.Writer.Write(", ");
				base.Writer.Write("new ");
				base.Writer.Write(typeof(XmlSerializationFixupCallback).FullName);
				base.Writer.Write("(this.");
				base.Writer.Write(fixupMethodName);
				base.Writer.Write("), ");
				base.Writer.Write(num.ToString(CultureInfo.InvariantCulture));
				base.Writer.WriteLine(");");
				base.Writer.WriteLine("AddFixup(fixup);");
				return true;
			}
			return false;
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x000C432C File Offset: 0x000C332C
		private void WriteMemberBegin(XmlSerializationReaderCodeGen.Member[] members)
		{
			foreach (XmlSerializationReaderCodeGen.Member member in members)
			{
				if (member.IsArrayLike)
				{
					string arrayName = member.ArrayName;
					string text = "c" + arrayName;
					TypeDesc typeDesc = member.Mapping.TypeDesc;
					string csharpName = typeDesc.CSharpName;
					if (member.Mapping.TypeDesc.IsArray)
					{
						this.WriteArrayLocalDecl(typeDesc.CSharpName, arrayName, "null", typeDesc);
						base.Writer.Write("int ");
						base.Writer.Write(text);
						base.Writer.WriteLine(" = 0;");
						if (member.Mapping.ChoiceIdentifier != null)
						{
							this.WriteArrayLocalDecl(member.Mapping.ChoiceIdentifier.Mapping.TypeDesc.CSharpName + "[]", member.ChoiceArrayName, "null", member.Mapping.ChoiceIdentifier.Mapping.TypeDesc);
							base.Writer.Write("int c");
							base.Writer.Write(member.ChoiceArrayName);
							base.Writer.WriteLine(" = 0;");
						}
					}
					else
					{
						bool useReflection = typeDesc.UseReflection;
						if (member.Source[member.Source.Length - 1] == '(' || member.Source[member.Source.Length - 1] == '{')
						{
							this.WriteCreateInstance(csharpName, arrayName, useReflection, typeDesc.CannotNew);
							base.Writer.Write(member.Source);
							base.Writer.Write(arrayName);
							if (member.Source[member.Source.Length - 1] == '{')
							{
								base.Writer.WriteLine("});");
							}
							else
							{
								base.Writer.WriteLine(");");
							}
						}
						else
						{
							if (member.IsList && !member.Mapping.ReadOnly && member.Mapping.TypeDesc.IsNullable)
							{
								base.Writer.Write("if ((object)(");
								base.Writer.Write(member.Source);
								base.Writer.Write(") == null) ");
								if (!member.Mapping.TypeDesc.HasDefaultConstructor)
								{
									base.Writer.Write("throw CreateReadOnlyCollectionException(");
									base.WriteQuotedCSharpString(member.Mapping.TypeDesc.CSharpName);
									base.Writer.WriteLine(");");
								}
								else
								{
									base.Writer.Write(member.Source);
									base.Writer.Write(" = ");
									base.Writer.Write(base.RaCodeGen.GetStringForCreateInstance(csharpName, useReflection, typeDesc.CannotNew, true));
									base.Writer.WriteLine(";");
								}
							}
							this.WriteLocalDecl(csharpName, arrayName, member.Source, useReflection);
						}
					}
				}
			}
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x000C4630 File Offset: 0x000C3630
		private string ExpectedElements(XmlSerializationReaderCodeGen.Member[] members)
		{
			if (this.IsSequence(members))
			{
				return "null";
			}
			string text = string.Empty;
			bool flag = true;
			foreach (XmlSerializationReaderCodeGen.Member member in members)
			{
				if (member.Mapping.Xmlns == null && !member.Mapping.Ignore && !member.Mapping.IsText && !member.Mapping.IsAttribute)
				{
					foreach (ElementAccessor elementAccessor in member.Mapping.Elements)
					{
						string text2 = ((elementAccessor.Form == XmlSchemaForm.Qualified) ? elementAccessor.Namespace : "");
						if (!elementAccessor.Any || (elementAccessor.Name != null && elementAccessor.Name.Length != 0))
						{
							if (!flag)
							{
								text += ", ";
							}
							text = text + text2 + ":" + elementAccessor.Name;
							flag = false;
						}
					}
				}
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			ReflectionAwareCodeGen.WriteQuotedCSharpString(new IndentedWriter(stringWriter, true), text);
			return stringWriter.ToString();
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000C4754 File Offset: 0x000C3754
		private void WriteMemberElements(XmlSerializationReaderCodeGen.Member[] members, string elementElseString, string elseString, XmlSerializationReaderCodeGen.Member anyElement, XmlSerializationReaderCodeGen.Member anyText, string checkTypeHrefsSource)
		{
			bool flag = checkTypeHrefsSource != null && checkTypeHrefsSource.Length > 0;
			if (anyText != null)
			{
				base.Writer.WriteLine("string tmp = null;");
			}
			base.Writer.Write("if (Reader.NodeType == ");
			base.Writer.Write(typeof(XmlNodeType).FullName);
			base.Writer.WriteLine(".Element) {");
			base.Writer.Indent++;
			if (flag)
			{
				this.WriteIfNotSoapRoot(elementElseString + " continue;");
				this.WriteMemberElementsCheckType(checkTypeHrefsSource);
			}
			else
			{
				this.WriteMemberElementsIf(members, anyElement, elementElseString, null);
			}
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			if (anyText != null)
			{
				this.WriteMemberText(anyText, elseString);
			}
			base.Writer.WriteLine("else {");
			base.Writer.Indent++;
			base.Writer.WriteLine(elseString);
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000C4880 File Offset: 0x000C3880
		private void WriteMemberText(XmlSerializationReaderCodeGen.Member anyText, string elseString)
		{
			base.Writer.Write("else if (Reader.NodeType == ");
			base.Writer.Write(typeof(XmlNodeType).FullName);
			base.Writer.WriteLine(".Text || ");
			base.Writer.Write("Reader.NodeType == ");
			base.Writer.Write(typeof(XmlNodeType).FullName);
			base.Writer.WriteLine(".CDATA || ");
			base.Writer.Write("Reader.NodeType == ");
			base.Writer.Write(typeof(XmlNodeType).FullName);
			base.Writer.WriteLine(".Whitespace || ");
			base.Writer.Write("Reader.NodeType == ");
			base.Writer.Write(typeof(XmlNodeType).FullName);
			base.Writer.WriteLine(".SignificantWhitespace) {");
			base.Writer.Indent++;
			if (anyText != null)
			{
				this.WriteText(anyText);
			}
			else
			{
				base.Writer.Write(elseString);
				base.Writer.WriteLine(";");
			}
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x000C49D4 File Offset: 0x000C39D4
		private void WriteText(XmlSerializationReaderCodeGen.Member member)
		{
			TextAccessor text = member.Mapping.Text;
			if (text.Mapping is SpecialMapping)
			{
				SpecialMapping specialMapping = (SpecialMapping)text.Mapping;
				this.WriteSourceBeginTyped(member.ArraySource, specialMapping.TypeDesc);
				TypeKind kind = specialMapping.TypeDesc.Kind;
				if (kind != TypeKind.Node)
				{
					throw new InvalidOperationException(Res.GetString("XmlInternalError"));
				}
				base.Writer.Write("Document.CreateTextNode(Reader.ReadString())");
				this.WriteSourceEnd(member.ArraySource);
			}
			else
			{
				if (member.IsArrayLike)
				{
					this.WriteSourceBegin(member.ArraySource);
					if (text.Mapping.TypeDesc.CollapseWhitespace)
					{
						base.Writer.Write("CollapseWhitespace(Reader.ReadString())");
					}
					else
					{
						base.Writer.Write("Reader.ReadString()");
					}
				}
				else if (text.Mapping.TypeDesc == base.StringTypeDesc || text.Mapping.TypeDesc.FormatterName == "String")
				{
					base.Writer.Write("tmp = ReadString(tmp, ");
					if (text.Mapping.TypeDesc.CollapseWhitespace)
					{
						base.Writer.WriteLine("true);");
					}
					else
					{
						base.Writer.WriteLine("false);");
					}
					this.WriteSourceBegin(member.ArraySource);
					base.Writer.Write("tmp");
				}
				else
				{
					this.WriteSourceBegin(member.ArraySource);
					this.WritePrimitive(text.Mapping, "Reader.ReadString()");
				}
				this.WriteSourceEnd(member.ArraySource);
			}
			base.Writer.WriteLine(";");
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x000C4B78 File Offset: 0x000C3B78
		private void WriteMemberElementsCheckType(string checkTypeHrefsSource)
		{
			base.Writer.WriteLine("string refElemId = null;");
			base.Writer.WriteLine("object refElem = ReadReferencingElement(null, null, true, out refElemId);");
			base.Writer.WriteLine("if (refElemId != null) {");
			base.Writer.Indent++;
			base.Writer.Write(checkTypeHrefsSource);
			base.Writer.WriteLine(".Add(refElemId);");
			base.Writer.Write(checkTypeHrefsSource);
			base.Writer.WriteLine("IsObject.Add(false);");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			base.Writer.WriteLine("else if (refElem != null) {");
			base.Writer.Indent++;
			base.Writer.Write(checkTypeHrefsSource);
			base.Writer.WriteLine(".Add(refElem);");
			base.Writer.Write(checkTypeHrefsSource);
			base.Writer.WriteLine("IsObject.Add(true);");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x000C4CA4 File Offset: 0x000C3CA4
		private void WriteMemberElementsElse(XmlSerializationReaderCodeGen.Member anyElement, string elementElseString)
		{
			if (anyElement != null)
			{
				ElementAccessor[] elements = anyElement.Mapping.Elements;
				for (int i = 0; i < elements.Length; i++)
				{
					ElementAccessor elementAccessor = elements[i];
					if (elementAccessor.Any && elementAccessor.Name.Length == 0)
					{
						this.WriteElement(anyElement.ArraySource, anyElement.ArrayName, anyElement.ChoiceArraySource, elementAccessor, anyElement.Mapping.ChoiceIdentifier, (anyElement.Mapping.CheckSpecified == SpecifiedAccessor.ReadWrite) ? anyElement.CheckSpecifiedSource : null, false, false, -1, i);
						return;
					}
				}
				return;
			}
			base.Writer.WriteLine(elementElseString);
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000C4D34 File Offset: 0x000C3D34
		private bool IsSequence(XmlSerializationReaderCodeGen.Member[] members)
		{
			for (int i = 0; i < members.Length; i++)
			{
				if (members[i].Mapping.IsParticle && members[i].Mapping.IsSequence)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000C4D70 File Offset: 0x000C3D70
		private void WriteMemberElementsIf(XmlSerializationReaderCodeGen.Member[] members, XmlSerializationReaderCodeGen.Member anyElement, string elementElseString, string checkTypeSource)
		{
			bool flag = checkTypeSource != null && checkTypeSource.Length > 0;
			int num = 0;
			bool flag2 = this.IsSequence(members);
			if (flag2)
			{
				base.Writer.WriteLine("switch (state) {");
			}
			int num2 = 0;
			foreach (XmlSerializationReaderCodeGen.Member member in members)
			{
				if (member.Mapping.Xmlns == null && !member.Mapping.Ignore && (!flag2 || (!member.Mapping.IsText && !member.Mapping.IsAttribute)))
				{
					bool flag3 = true;
					ChoiceIdentifierAccessor choiceIdentifier = member.Mapping.ChoiceIdentifier;
					ElementAccessor[] elements = member.Mapping.Elements;
					for (int j = 0; j < elements.Length; j++)
					{
						ElementAccessor elementAccessor = elements[j];
						string text = ((elementAccessor.Form == XmlSchemaForm.Qualified) ? elementAccessor.Namespace : "");
						if (flag2 || !elementAccessor.Any || (elementAccessor.Name != null && elementAccessor.Name.Length != 0))
						{
							if (!flag3 || (!flag2 && num > 0))
							{
								base.Writer.Write("else ");
							}
							else if (flag2)
							{
								base.Writer.Write("case ");
								base.Writer.Write(num2.ToString(CultureInfo.InvariantCulture));
								base.Writer.WriteLine(":");
								base.Writer.Indent++;
							}
							num++;
							flag3 = false;
							base.Writer.Write("if (");
							if (member.ParamsReadSource != null)
							{
								base.Writer.Write("!");
								base.Writer.Write(member.ParamsReadSource);
								base.Writer.Write(" && ");
							}
							if (flag)
							{
								if (elementAccessor.Mapping is NullableMapping)
								{
									TypeDesc typeDesc = ((NullableMapping)elementAccessor.Mapping).BaseMapping.TypeDesc;
									base.Writer.Write(base.RaCodeGen.GetStringForTypeof(typeDesc.CSharpName, typeDesc.UseReflection));
								}
								else
								{
									base.Writer.Write(base.RaCodeGen.GetStringForTypeof(elementAccessor.Mapping.TypeDesc.CSharpName, elementAccessor.Mapping.TypeDesc.UseReflection));
								}
								base.Writer.Write(".IsAssignableFrom(");
								base.Writer.Write(checkTypeSource);
								base.Writer.Write("Type)");
							}
							else
							{
								if (member.Mapping.IsReturnValue)
								{
									base.Writer.Write("(IsReturnValue || ");
								}
								if (flag2 && elementAccessor.Any && elementAccessor.AnyNamespaces == null)
								{
									base.Writer.Write("true");
								}
								else
								{
									this.WriteXmlNodeEqual("Reader", elementAccessor.Name, text);
								}
								if (member.Mapping.IsReturnValue)
								{
									base.Writer.Write(")");
								}
							}
							base.Writer.WriteLine(") {");
							base.Writer.Indent++;
							if (flag)
							{
								if (elementAccessor.Mapping.TypeDesc.IsValueType || elementAccessor.Mapping is NullableMapping)
								{
									base.Writer.Write("if (");
									base.Writer.Write(checkTypeSource);
									base.Writer.WriteLine(" != null) {");
									base.Writer.Indent++;
								}
								if (elementAccessor.Mapping is NullableMapping)
								{
									this.WriteSourceBegin(member.ArraySource);
									TypeDesc typeDesc2 = ((NullableMapping)elementAccessor.Mapping).BaseMapping.TypeDesc;
									base.Writer.Write(base.RaCodeGen.GetStringForCreateInstance(elementAccessor.Mapping.TypeDesc.CSharpName, elementAccessor.Mapping.TypeDesc.UseReflection, false, true, "(" + typeDesc2.CSharpName + ")" + checkTypeSource));
								}
								else
								{
									this.WriteSourceBeginTyped(member.ArraySource, elementAccessor.Mapping.TypeDesc);
									base.Writer.Write(checkTypeSource);
								}
								this.WriteSourceEnd(member.ArraySource);
								base.Writer.WriteLine(";");
								if (elementAccessor.Mapping.TypeDesc.IsValueType)
								{
									base.Writer.Indent--;
									base.Writer.WriteLine("}");
								}
								if (member.FixupIndex >= 0)
								{
									base.Writer.Write("fixup.Ids[");
									base.Writer.Write(member.FixupIndex.ToString(CultureInfo.InvariantCulture));
									base.Writer.Write("] = ");
									base.Writer.Write(checkTypeSource);
									base.Writer.WriteLine("Id;");
								}
							}
							else
							{
								this.WriteElement(member.ArraySource, member.ArrayName, member.ChoiceArraySource, elementAccessor, choiceIdentifier, (member.Mapping.CheckSpecified == SpecifiedAccessor.ReadWrite) ? member.CheckSpecifiedSource : null, member.IsList && member.Mapping.TypeDesc.IsNullable, member.Mapping.ReadOnly, member.FixupIndex, j);
							}
							if (member.Mapping.IsReturnValue)
							{
								base.Writer.WriteLine("IsReturnValue = false;");
							}
							if (member.ParamsReadSource != null)
							{
								base.Writer.Write(member.ParamsReadSource);
								base.Writer.WriteLine(" = true;");
							}
							base.Writer.Indent--;
							base.Writer.WriteLine("}");
						}
					}
					if (flag2)
					{
						if (member.IsArrayLike)
						{
							base.Writer.WriteLine("else {");
							base.Writer.Indent++;
						}
						num2++;
						base.Writer.Write("state = ");
						base.Writer.Write(num2.ToString(CultureInfo.InvariantCulture));
						base.Writer.WriteLine(";");
						if (member.IsArrayLike)
						{
							base.Writer.Indent--;
							base.Writer.WriteLine("}");
						}
						base.Writer.WriteLine("break;");
						base.Writer.Indent--;
					}
				}
			}
			if (num > 0)
			{
				if (flag2)
				{
					base.Writer.WriteLine("default:");
				}
				else
				{
					base.Writer.WriteLine("else {");
				}
				base.Writer.Indent++;
			}
			this.WriteMemberElementsElse(anyElement, elementElseString);
			if (num > 0)
			{
				if (flag2)
				{
					base.Writer.WriteLine("break;");
				}
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
			}
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000C5487 File Offset: 0x000C4487
		private string GetArraySource(TypeDesc typeDesc, string arrayName)
		{
			return this.GetArraySource(typeDesc, arrayName, false);
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000C5494 File Offset: 0x000C4494
		private string GetArraySource(TypeDesc typeDesc, string arrayName, bool multiRef)
		{
			string text = "c" + arrayName;
			string text2 = "";
			if (multiRef)
			{
				text2 = "soap = (System.Object[])EnsureArrayIndex(soap, " + text + "+2, typeof(System.Object)); ";
			}
			bool useReflection = typeDesc.UseReflection;
			if (typeDesc.IsArray)
			{
				string csharpName = typeDesc.ArrayElementTypeDesc.CSharpName;
				bool useReflection2 = typeDesc.ArrayElementTypeDesc.UseReflection;
				string text3 = (useReflection ? "" : ("(" + csharpName + "[])"));
				text2 = string.Concat(new string[]
				{
					text2,
					arrayName,
					" = ",
					text3,
					"EnsureArrayIndex(",
					arrayName,
					", ",
					text,
					", ",
					base.RaCodeGen.GetStringForTypeof(csharpName, useReflection2),
					");"
				});
				string stringForArrayMember = base.RaCodeGen.GetStringForArrayMember(arrayName, text + "++", typeDesc);
				if (multiRef)
				{
					text2 = text2 + " soap[1] = " + arrayName + ";";
					text2 = string.Concat(new string[] { text2, " if (ReadReference(out soap[", text, "+2])) ", stringForArrayMember, " = null; else " });
				}
				return text2 + stringForArrayMember;
			}
			return base.RaCodeGen.GetStringForMethod(arrayName, typeDesc.CSharpName, "Add", useReflection);
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x000C560A File Offset: 0x000C460A
		private void WriteMemberEnd(XmlSerializationReaderCodeGen.Member[] members)
		{
			this.WriteMemberEnd(members, false);
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000C5614 File Offset: 0x000C4614
		private void WriteMemberEnd(XmlSerializationReaderCodeGen.Member[] members, bool soapRefs)
		{
			foreach (XmlSerializationReaderCodeGen.Member member in members)
			{
				if (member.IsArrayLike)
				{
					TypeDesc typeDesc = member.Mapping.TypeDesc;
					if (typeDesc.IsArray)
					{
						this.WriteSourceBegin(member.Source);
						if (soapRefs)
						{
							base.Writer.Write(" soap[1] = ");
						}
						string text = member.ArrayName;
						string text2 = "c" + text;
						bool useReflection = typeDesc.ArrayElementTypeDesc.UseReflection;
						string csharpName = typeDesc.ArrayElementTypeDesc.CSharpName;
						if (!useReflection)
						{
							base.Writer.Write("(" + csharpName + "[])");
						}
						base.Writer.Write("ShrinkArray(");
						base.Writer.Write(text);
						base.Writer.Write(", ");
						base.Writer.Write(text2);
						base.Writer.Write(", ");
						base.Writer.Write(base.RaCodeGen.GetStringForTypeof(csharpName, useReflection));
						base.Writer.Write(", ");
						this.WriteBooleanValue(member.IsNullable);
						base.Writer.Write(")");
						this.WriteSourceEnd(member.Source);
						base.Writer.WriteLine(";");
						if (member.Mapping.ChoiceIdentifier != null)
						{
							this.WriteSourceBegin(member.ChoiceSource);
							text = member.ChoiceArrayName;
							text2 = "c" + text;
							bool useReflection2 = member.Mapping.ChoiceIdentifier.Mapping.TypeDesc.UseReflection;
							string csharpName2 = member.Mapping.ChoiceIdentifier.Mapping.TypeDesc.CSharpName;
							if (!useReflection2)
							{
								base.Writer.Write("(" + csharpName2 + "[])");
							}
							base.Writer.Write("ShrinkArray(");
							base.Writer.Write(text);
							base.Writer.Write(", ");
							base.Writer.Write(text2);
							base.Writer.Write(", ");
							base.Writer.Write(base.RaCodeGen.GetStringForTypeof(csharpName2, useReflection2));
							base.Writer.Write(", ");
							this.WriteBooleanValue(member.IsNullable);
							base.Writer.Write(")");
							this.WriteSourceEnd(member.ChoiceSource);
							base.Writer.WriteLine(";");
						}
					}
					else if (typeDesc.IsValueType)
					{
						base.Writer.Write(member.Source);
						base.Writer.Write(" = ");
						base.Writer.Write(member.ArrayName);
						base.Writer.WriteLine(";");
					}
				}
			}
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000C58F4 File Offset: 0x000C48F4
		private void WriteSourceBeginTyped(string source, TypeDesc typeDesc)
		{
			this.WriteSourceBegin(source);
			if (typeDesc != null && !typeDesc.UseReflection)
			{
				base.Writer.Write("(");
				base.Writer.Write(typeDesc.CSharpName);
				base.Writer.Write(")");
			}
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000C5944 File Offset: 0x000C4944
		private void WriteSourceBegin(string source)
		{
			base.Writer.Write(source);
			if (source[source.Length - 1] != '(' && source[source.Length - 1] != '{')
			{
				base.Writer.Write(" = ");
			}
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x000C5994 File Offset: 0x000C4994
		private void WriteSourceEnd(string source)
		{
			if (source[source.Length - 1] == '(')
			{
				base.Writer.Write(")");
				return;
			}
			if (source[source.Length - 1] == '{')
			{
				base.Writer.Write("})");
			}
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x000C59E8 File Offset: 0x000C49E8
		private void WriteArray(string source, string arrayName, ArrayMapping arrayMapping, bool readOnly, bool isNullable, int fixupIndex)
		{
			if (!arrayMapping.IsSoap)
			{
				base.Writer.WriteLine("if (!ReadNull()) {");
				base.Writer.Indent++;
				XmlSerializationReaderCodeGen.Member member = new XmlSerializationReaderCodeGen.Member(this, source, arrayName, 0, new MemberMapping
				{
					Elements = arrayMapping.Elements,
					TypeDesc = arrayMapping.TypeDesc,
					ReadOnly = readOnly
				}, false);
				member.IsNullable = false;
				XmlSerializationReaderCodeGen.Member[] array = new XmlSerializationReaderCodeGen.Member[] { member };
				this.WriteMemberBegin(array);
				if (readOnly)
				{
					base.Writer.Write("if (((object)(");
					base.Writer.Write(member.ArrayName);
					base.Writer.Write(") == null) || ");
				}
				else
				{
					base.Writer.Write("if (");
				}
				base.Writer.WriteLine("(Reader.IsEmptyElement)) {");
				base.Writer.Indent++;
				base.Writer.WriteLine("Reader.Skip();");
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
				base.Writer.WriteLine("else {");
				base.Writer.Indent++;
				base.Writer.WriteLine("Reader.ReadStartElement();");
				int num = this.WriteWhileNotLoopStart();
				base.Writer.Indent++;
				string text = "UnknownNode(null, " + this.ExpectedElements(array) + ");";
				this.WriteMemberElements(array, text, text, null, null, null);
				base.Writer.WriteLine("Reader.MoveToContent();");
				this.WriteWhileLoopEnd(num);
				base.Writer.Indent--;
				base.Writer.WriteLine("ReadEndElement();");
				base.Writer.WriteLine("}");
				this.WriteMemberEnd(array, false);
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
				if (isNullable)
				{
					base.Writer.WriteLine("else {");
					base.Writer.Indent++;
					member.IsNullable = true;
					this.WriteMemberBegin(array);
					this.WriteMemberEnd(array);
					base.Writer.Indent--;
					base.Writer.WriteLine("}");
				}
				return;
			}
			base.Writer.Write("object rre = ");
			base.Writer.Write((fixupIndex >= 0) ? "ReadReferencingElement" : "ReadReferencedElement");
			base.Writer.Write("(");
			this.WriteID(arrayMapping.TypeName);
			base.Writer.Write(", ");
			this.WriteID(arrayMapping.Namespace);
			if (fixupIndex >= 0)
			{
				base.Writer.Write(", ");
				base.Writer.Write("out fixup.Ids[");
				base.Writer.Write(fixupIndex.ToString(CultureInfo.InvariantCulture));
				base.Writer.Write("]");
			}
			base.Writer.WriteLine(");");
			TypeDesc typeDesc = arrayMapping.TypeDesc;
			if (typeDesc.IsEnumerable || typeDesc.IsCollection)
			{
				base.Writer.WriteLine("if (rre != null) {");
				base.Writer.Indent++;
				this.WriteAddCollectionFixup(typeDesc, readOnly, source, "rre");
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
				return;
			}
			base.Writer.WriteLine("try {");
			base.Writer.Indent++;
			this.WriteSourceBeginTyped(source, arrayMapping.TypeDesc);
			base.Writer.Write("rre");
			this.WriteSourceEnd(source);
			base.Writer.WriteLine(";");
			this.WriteCatchCastException(arrayMapping.TypeDesc, "rre", null);
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x000C5DF0 File Offset: 0x000C4DF0
		private void WriteElement(string source, string arrayName, string choiceSource, ElementAccessor element, ChoiceIdentifierAccessor choice, string checkSpecified, bool checkForNull, bool readOnly, int fixupIndex, int elementIndex)
		{
			if (checkSpecified != null && checkSpecified.Length > 0)
			{
				base.Writer.Write(checkSpecified);
				base.Writer.WriteLine(" = true;");
			}
			if (element.Mapping is ArrayMapping)
			{
				this.WriteArray(source, arrayName, (ArrayMapping)element.Mapping, readOnly, element.IsNullable, fixupIndex);
			}
			else if (element.Mapping is NullableMapping)
			{
				string text = base.ReferenceMapping(element.Mapping);
				this.WriteSourceBegin(source);
				base.Writer.Write(text);
				base.Writer.Write("(true)");
				this.WriteSourceEnd(source);
				base.Writer.WriteLine(";");
			}
			else if (!element.Mapping.IsSoap && element.Mapping is PrimitiveMapping)
			{
				if (element.IsNullable)
				{
					base.Writer.WriteLine("if (ReadNull()) {");
					base.Writer.Indent++;
					this.WriteSourceBegin(source);
					if (element.Mapping.TypeDesc.IsValueType)
					{
						base.Writer.Write(base.RaCodeGen.GetStringForCreateInstance(element.Mapping.TypeDesc.CSharpName, element.Mapping.TypeDesc.UseReflection, false, false));
					}
					else
					{
						base.Writer.Write("null");
					}
					this.WriteSourceEnd(source);
					base.Writer.WriteLine(";");
					base.Writer.Indent--;
					base.Writer.WriteLine("}");
					base.Writer.Write("else ");
				}
				if (element.Default != null && element.Default != DBNull.Value && element.Mapping.TypeDesc.IsValueType)
				{
					base.Writer.WriteLine("if (Reader.IsEmptyElement) {");
					base.Writer.Indent++;
					base.Writer.WriteLine("Reader.Skip();");
					base.Writer.Indent--;
					base.Writer.WriteLine("}");
					base.Writer.WriteLine("else {");
				}
				else
				{
					base.Writer.WriteLine("{");
				}
				base.Writer.Indent++;
				this.WriteSourceBegin(source);
				if (element.Mapping.TypeDesc == base.QnameTypeDesc)
				{
					base.Writer.Write("ReadElementQualifiedName()");
				}
				else
				{
					string formatterName;
					string text2;
					if ((formatterName = element.Mapping.TypeDesc.FormatterName) != null && (formatterName == "ByteArrayBase64" || formatterName == "ByteArrayHex"))
					{
						text2 = "false";
					}
					else
					{
						text2 = "Reader.ReadElementString()";
					}
					this.WritePrimitive(element.Mapping, text2);
				}
				this.WriteSourceEnd(source);
				base.Writer.WriteLine(";");
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
			}
			else if (element.Mapping is StructMapping || (element.Mapping.IsSoap && element.Mapping is PrimitiveMapping))
			{
				TypeMapping mapping = element.Mapping;
				if (mapping.IsSoap)
				{
					base.Writer.Write("object rre = ");
					base.Writer.Write((fixupIndex >= 0) ? "ReadReferencingElement" : "ReadReferencedElement");
					base.Writer.Write("(");
					this.WriteID(mapping.TypeName);
					base.Writer.Write(", ");
					this.WriteID(mapping.Namespace);
					if (fixupIndex >= 0)
					{
						base.Writer.Write(", out fixup.Ids[");
						base.Writer.Write(fixupIndex.ToString(CultureInfo.InvariantCulture));
						base.Writer.Write("]");
					}
					base.Writer.Write(")");
					this.WriteSourceEnd(source);
					base.Writer.WriteLine(";");
					if (mapping.TypeDesc.IsValueType)
					{
						base.Writer.WriteLine("if (rre != null) {");
						base.Writer.Indent++;
					}
					base.Writer.WriteLine("try {");
					base.Writer.Indent++;
					this.WriteSourceBeginTyped(source, mapping.TypeDesc);
					base.Writer.Write("rre");
					this.WriteSourceEnd(source);
					base.Writer.WriteLine(";");
					this.WriteCatchCastException(mapping.TypeDesc, "rre", null);
					base.Writer.Write("Referenced(");
					base.Writer.Write(source);
					base.Writer.WriteLine(");");
					if (mapping.TypeDesc.IsValueType)
					{
						base.Writer.Indent--;
						base.Writer.WriteLine("}");
					}
				}
				else
				{
					string text3 = base.ReferenceMapping(mapping);
					if (checkForNull)
					{
						base.Writer.Write("if ((object)(");
						base.Writer.Write(arrayName);
						base.Writer.Write(") == null) Reader.Skip(); else ");
					}
					this.WriteSourceBegin(source);
					base.Writer.Write(text3);
					base.Writer.Write("(");
					if (mapping.TypeDesc.IsNullable)
					{
						this.WriteBooleanValue(element.IsNullable);
						base.Writer.Write(", ");
					}
					base.Writer.Write("true");
					base.Writer.Write(")");
					this.WriteSourceEnd(source);
					base.Writer.WriteLine(";");
				}
			}
			else
			{
				if (element.Mapping is SpecialMapping)
				{
					SpecialMapping specialMapping = (SpecialMapping)element.Mapping;
					switch (specialMapping.TypeDesc.Kind)
					{
					case TypeKind.Node:
					{
						bool flag = specialMapping.TypeDesc.FullName == typeof(XmlDocument).FullName;
						this.WriteSourceBeginTyped(source, specialMapping.TypeDesc);
						base.Writer.Write(flag ? "ReadXmlDocument(" : "ReadXmlNode(");
						base.Writer.Write(element.Any ? "false" : "true");
						base.Writer.Write(")");
						this.WriteSourceEnd(source);
						base.Writer.WriteLine(";");
						goto IL_08BC;
					}
					case TypeKind.Serializable:
					{
						SerializableMapping serializableMapping = (SerializableMapping)element.Mapping;
						if (serializableMapping.DerivedMappings != null)
						{
							base.Writer.Write(typeof(XmlQualifiedName).FullName);
							base.Writer.WriteLine(" tser = GetXsiType();");
							base.Writer.Write("if (tser == null");
							base.Writer.Write(" || ");
							this.WriteQNameEqual("tser", serializableMapping.XsiType.Name, serializableMapping.XsiType.Namespace);
							base.Writer.WriteLine(") {");
							base.Writer.Indent++;
						}
						this.WriteSourceBeginTyped(source, serializableMapping.TypeDesc);
						base.Writer.Write("ReadSerializable(( ");
						base.Writer.Write(typeof(IXmlSerializable).FullName);
						base.Writer.Write(")");
						base.Writer.Write(base.RaCodeGen.GetStringForCreateInstance(serializableMapping.TypeDesc.CSharpName, serializableMapping.TypeDesc.UseReflection, serializableMapping.TypeDesc.CannotNew, false));
						bool flag2 = !element.Any && XmlSerializationCodeGen.IsWildcard(serializableMapping);
						if (flag2)
						{
							base.Writer.WriteLine(", true");
						}
						base.Writer.Write(")");
						this.WriteSourceEnd(source);
						base.Writer.WriteLine(";");
						if (serializableMapping.DerivedMappings != null)
						{
							base.Writer.Indent--;
							base.Writer.WriteLine("}");
							this.WriteDerivedSerializable(serializableMapping, serializableMapping, source, flag2);
							this.WriteUnknownNode("UnknownNode", "null", null, true);
							goto IL_08BC;
						}
						goto IL_08BC;
					}
					}
					throw new InvalidOperationException(Res.GetString("XmlInternalError"));
				}
				throw new InvalidOperationException(Res.GetString("XmlInternalError"));
			}
			IL_08BC:
			if (choice != null)
			{
				string csharpName = choice.Mapping.TypeDesc.CSharpName;
				base.Writer.Write(choiceSource);
				base.Writer.Write(" = ");
				CodeIdentifier.CheckValidIdentifier(choice.MemberIds[elementIndex]);
				base.Writer.Write(base.RaCodeGen.GetStringForEnumMember(csharpName, choice.MemberIds[elementIndex], choice.Mapping.TypeDesc.UseReflection));
				base.Writer.WriteLine(";");
			}
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x000C6744 File Offset: 0x000C5744
		private void WriteDerivedSerializable(SerializableMapping head, SerializableMapping mapping, string source, bool isWrappedAny)
		{
			if (mapping == null)
			{
				return;
			}
			for (SerializableMapping serializableMapping = mapping.DerivedMappings; serializableMapping != null; serializableMapping = serializableMapping.NextDerivedMapping)
			{
				base.Writer.Write("else if (tser == null");
				base.Writer.Write(" || ");
				this.WriteQNameEqual("tser", serializableMapping.XsiType.Name, serializableMapping.XsiType.Namespace);
				base.Writer.WriteLine(") {");
				base.Writer.Indent++;
				if (serializableMapping.Type != null)
				{
					if (head.Type.IsAssignableFrom(serializableMapping.Type))
					{
						this.WriteSourceBeginTyped(source, head.TypeDesc);
						base.Writer.Write("ReadSerializable(( ");
						base.Writer.Write(typeof(IXmlSerializable).FullName);
						base.Writer.Write(")");
						base.Writer.Write(base.RaCodeGen.GetStringForCreateInstance(serializableMapping.TypeDesc.CSharpName, serializableMapping.TypeDesc.UseReflection, serializableMapping.TypeDesc.CannotNew, false));
						if (isWrappedAny)
						{
							base.Writer.WriteLine(", true");
						}
						base.Writer.Write(")");
						this.WriteSourceEnd(source);
						base.Writer.WriteLine(";");
					}
					else
					{
						base.Writer.Write("throw CreateBadDerivationException(");
						base.WriteQuotedCSharpString(serializableMapping.XsiType.Name);
						base.Writer.Write(", ");
						base.WriteQuotedCSharpString(serializableMapping.XsiType.Namespace);
						base.Writer.Write(", ");
						base.WriteQuotedCSharpString(head.XsiType.Name);
						base.Writer.Write(", ");
						base.WriteQuotedCSharpString(head.XsiType.Namespace);
						base.Writer.Write(", ");
						base.WriteQuotedCSharpString(serializableMapping.Type.FullName);
						base.Writer.Write(", ");
						base.WriteQuotedCSharpString(head.Type.FullName);
						base.Writer.WriteLine(");");
					}
				}
				else
				{
					base.Writer.WriteLine("// missing real mapping for " + serializableMapping.XsiType);
					base.Writer.Write("throw CreateMissingIXmlSerializableType(");
					base.WriteQuotedCSharpString(serializableMapping.XsiType.Name);
					base.Writer.Write(", ");
					base.WriteQuotedCSharpString(serializableMapping.XsiType.Namespace);
					base.Writer.Write(", ");
					base.WriteQuotedCSharpString(head.Type.FullName);
					base.Writer.WriteLine(");");
				}
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
				this.WriteDerivedSerializable(head, serializableMapping, source, isWrappedAny);
			}
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000C6A4C File Offset: 0x000C5A4C
		private int WriteWhileNotLoopStart()
		{
			base.Writer.WriteLine("Reader.MoveToContent();");
			int num = this.WriteWhileLoopStartCheck();
			base.Writer.Write("while (Reader.NodeType != ");
			base.Writer.Write(typeof(XmlNodeType).FullName);
			base.Writer.Write(".EndElement && Reader.NodeType != ");
			base.Writer.Write(typeof(XmlNodeType).FullName);
			base.Writer.WriteLine(".None) {");
			return num;
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x000C6AD5 File Offset: 0x000C5AD5
		private void WriteWhileLoopEnd(int loopIndex)
		{
			this.WriteWhileLoopEndCheck(loopIndex);
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x000C6B04 File Offset: 0x000C5B04
		private int WriteWhileLoopStartCheck()
		{
			base.Writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "int whileIterations{0} = 0;", new object[] { this.nextWhileLoopIndex }));
			base.Writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "int readerCount{0} = ReaderCount;", new object[] { this.nextWhileLoopIndex }));
			return this.nextWhileLoopIndex++;
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000C6B84 File Offset: 0x000C5B84
		private void WriteWhileLoopEndCheck(int loopIndex)
		{
			base.Writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "CheckReaderCount(ref whileIterations{0}, ref readerCount{1});", new object[] { loopIndex, loopIndex }));
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000C6BC5 File Offset: 0x000C5BC5
		private void WriteParamsRead(int length)
		{
			base.Writer.Write("bool[] paramsRead = new bool[");
			base.Writer.Write(length.ToString(CultureInfo.InvariantCulture));
			base.Writer.WriteLine("];");
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000C6C00 File Offset: 0x000C5C00
		private void WriteReadNonRoots()
		{
			base.Writer.WriteLine("Reader.MoveToContent();");
			int num = this.WriteWhileLoopStartCheck();
			base.Writer.Write("while (Reader.NodeType == ");
			base.Writer.Write(typeof(XmlNodeType).FullName);
			base.Writer.WriteLine(".Element) {");
			base.Writer.Indent++;
			base.Writer.Write("string root = Reader.GetAttribute(\"root\", \"");
			base.Writer.Write("http://schemas.xmlsoap.org/soap/encoding/");
			base.Writer.WriteLine("\");");
			base.Writer.Write("if (root == null || ");
			base.Writer.Write(typeof(XmlConvert).FullName);
			base.Writer.WriteLine(".ToBoolean(root)) break;");
			base.Writer.WriteLine("ReadReferencedElement();");
			base.Writer.WriteLine("Reader.MoveToContent();");
			this.WriteWhileLoopEnd(num);
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000C6D02 File Offset: 0x000C5D02
		private void WriteBooleanValue(bool value)
		{
			base.Writer.Write(value ? "true" : "false");
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000C6D20 File Offset: 0x000C5D20
		private void WriteInitCheckTypeHrefList(string source)
		{
			base.Writer.Write(typeof(ArrayList).FullName);
			base.Writer.Write(" ");
			base.Writer.Write(source);
			base.Writer.Write(" = new ");
			base.Writer.Write(typeof(ArrayList).FullName);
			base.Writer.WriteLine("();");
			base.Writer.Write(typeof(ArrayList).FullName);
			base.Writer.Write(" ");
			base.Writer.Write(source);
			base.Writer.Write("IsObject = new ");
			base.Writer.Write(typeof(ArrayList).FullName);
			base.Writer.WriteLine("();");
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x000C6E10 File Offset: 0x000C5E10
		private void WriteHandleHrefList(XmlSerializationReaderCodeGen.Member[] members, string listSource)
		{
			base.Writer.WriteLine("int isObjectIndex = 0;");
			base.Writer.Write("foreach (object obj in ");
			base.Writer.Write(listSource);
			base.Writer.WriteLine(") {");
			base.Writer.Indent++;
			base.Writer.WriteLine("bool isReferenced = true;");
			base.Writer.Write("bool isObject = (bool)");
			base.Writer.Write(listSource);
			base.Writer.WriteLine("IsObject[isObjectIndex++];");
			base.Writer.WriteLine("object refObj = isObject ? obj : GetTarget((string)obj);");
			base.Writer.WriteLine("if (refObj == null) continue;");
			base.Writer.Write(typeof(Type).FullName);
			base.Writer.WriteLine(" refObjType = refObj.GetType();");
			base.Writer.WriteLine("string refObjId = null;");
			this.WriteMemberElementsIf(members, null, "isReferenced = false;", "refObj");
			base.Writer.WriteLine("if (isObject && isReferenced) Referenced(refObj); // need to mark this obj as ref'd since we didn't do GetTarget");
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000C6F48 File Offset: 0x000C5F48
		private void WriteIfNotSoapRoot(string source)
		{
			base.Writer.Write("if (Reader.GetAttribute(\"root\", \"");
			base.Writer.Write("http://schemas.xmlsoap.org/soap/encoding/");
			base.Writer.WriteLine("\") == \"0\") {");
			base.Writer.Indent++;
			base.Writer.WriteLine(source);
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x000C6FC8 File Offset: 0x000C5FC8
		private void WriteCreateMapping(TypeMapping mapping, string local)
		{
			string csharpName = mapping.TypeDesc.CSharpName;
			bool useReflection = mapping.TypeDesc.UseReflection;
			bool cannotNew = mapping.TypeDesc.CannotNew;
			base.Writer.Write(useReflection ? "object" : csharpName);
			base.Writer.Write(" ");
			base.Writer.Write(local);
			base.Writer.WriteLine(";");
			if (cannotNew)
			{
				base.Writer.WriteLine("try {");
				base.Writer.Indent++;
			}
			base.Writer.Write(local);
			base.Writer.Write(" = ");
			base.Writer.Write(base.RaCodeGen.GetStringForCreateInstance(csharpName, useReflection, mapping.TypeDesc.CannotNew, true));
			base.Writer.WriteLine(";");
			if (cannotNew)
			{
				this.WriteCatchException(typeof(MissingMethodException));
				base.Writer.Indent++;
				base.Writer.Write("throw CreateInaccessibleConstructorException(");
				base.WriteQuotedCSharpString(csharpName);
				base.Writer.WriteLine(");");
				this.WriteCatchException(typeof(SecurityException));
				base.Writer.Indent++;
				base.Writer.Write("throw CreateCtorHasSecurityException(");
				base.WriteQuotedCSharpString(csharpName);
				base.Writer.WriteLine(");");
				base.Writer.Indent--;
				base.Writer.WriteLine("}");
			}
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x000C7170 File Offset: 0x000C6170
		private void WriteCatchException(Type exceptionType)
		{
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
			base.Writer.Write("catch (");
			base.Writer.Write(exceptionType.FullName);
			base.Writer.WriteLine(") {");
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000C71D4 File Offset: 0x000C61D4
		private void WriteCatchCastException(TypeDesc typeDesc, string source, string id)
		{
			this.WriteCatchException(typeof(InvalidCastException));
			base.Writer.Indent++;
			base.Writer.Write("throw CreateInvalidCastException(");
			base.Writer.Write(base.RaCodeGen.GetStringForTypeof(typeDesc.CSharpName, typeDesc.UseReflection));
			base.Writer.Write(", ");
			base.Writer.Write(source);
			if (id == null)
			{
				base.Writer.WriteLine(", null);");
			}
			else
			{
				base.Writer.Write(", (string)");
				base.Writer.Write(id);
				base.Writer.WriteLine(");");
			}
			base.Writer.Indent--;
			base.Writer.WriteLine("}");
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x000C72B6 File Offset: 0x000C62B6
		private void WriteArrayLocalDecl(string typeName, string variableName, string initValue, TypeDesc arrayTypeDesc)
		{
			base.RaCodeGen.WriteArrayLocalDecl(typeName, variableName, initValue, arrayTypeDesc);
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x000C72C8 File Offset: 0x000C62C8
		private void WriteCreateInstance(string escapedName, string source, bool useReflection, bool ctorInaccessible)
		{
			base.RaCodeGen.WriteCreateInstance(escapedName, source, useReflection, ctorInaccessible);
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000C72DA File Offset: 0x000C62DA
		private void WriteLocalDecl(string typeFullName, string variableName, string initValue, bool useReflection)
		{
			base.RaCodeGen.WriteLocalDecl(typeFullName, variableName, initValue, useReflection);
		}

		// Token: 0x0400163E RID: 5694
		private Hashtable idNames = new Hashtable();

		// Token: 0x0400163F RID: 5695
		private Hashtable enums;

		// Token: 0x04001640 RID: 5696
		private Hashtable createMethods = new Hashtable();

		// Token: 0x04001641 RID: 5697
		private int nextCreateMethodNumber;

		// Token: 0x04001642 RID: 5698
		private int nextIdNumber;

		// Token: 0x04001643 RID: 5699
		private int nextWhileLoopIndex;

		// Token: 0x0200032C RID: 812
		private class CreateCollectionInfo
		{
			// Token: 0x06002734 RID: 10036 RVA: 0x000C72EC File Offset: 0x000C62EC
			internal CreateCollectionInfo(string name, TypeDesc td)
			{
				this.name = name;
				this.td = td;
			}

			// Token: 0x1700096D RID: 2413
			// (get) Token: 0x06002735 RID: 10037 RVA: 0x000C7302 File Offset: 0x000C6302
			internal string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x1700096E RID: 2414
			// (get) Token: 0x06002736 RID: 10038 RVA: 0x000C730A File Offset: 0x000C630A
			internal TypeDesc TypeDesc
			{
				get
				{
					return this.td;
				}
			}

			// Token: 0x04001644 RID: 5700
			private string name;

			// Token: 0x04001645 RID: 5701
			private TypeDesc td;
		}

		// Token: 0x0200032D RID: 813
		private class Member
		{
			// Token: 0x06002737 RID: 10039 RVA: 0x000C7314 File Offset: 0x000C6314
			internal Member(XmlSerializationReaderCodeGen outerClass, string source, string arrayName, int i, MemberMapping mapping)
				: this(outerClass, source, null, arrayName, i, mapping, false, null)
			{
			}

			// Token: 0x06002738 RID: 10040 RVA: 0x000C7334 File Offset: 0x000C6334
			internal Member(XmlSerializationReaderCodeGen outerClass, string source, string arrayName, int i, MemberMapping mapping, string choiceSource)
				: this(outerClass, source, null, arrayName, i, mapping, false, choiceSource)
			{
			}

			// Token: 0x06002739 RID: 10041 RVA: 0x000C7354 File Offset: 0x000C6354
			internal Member(XmlSerializationReaderCodeGen outerClass, string source, string arraySource, string arrayName, int i, MemberMapping mapping)
				: this(outerClass, source, arraySource, arrayName, i, mapping, false, null)
			{
			}

			// Token: 0x0600273A RID: 10042 RVA: 0x000C7374 File Offset: 0x000C6374
			internal Member(XmlSerializationReaderCodeGen outerClass, string source, string arraySource, string arrayName, int i, MemberMapping mapping, string choiceSource)
				: this(outerClass, source, arraySource, arrayName, i, mapping, false, choiceSource)
			{
			}

			// Token: 0x0600273B RID: 10043 RVA: 0x000C7394 File Offset: 0x000C6394
			internal Member(XmlSerializationReaderCodeGen outerClass, string source, string arrayName, int i, MemberMapping mapping, bool multiRef)
				: this(outerClass, source, null, arrayName, i, mapping, multiRef, null)
			{
			}

			// Token: 0x0600273C RID: 10044 RVA: 0x000C73B4 File Offset: 0x000C63B4
			internal Member(XmlSerializationReaderCodeGen outerClass, string source, string arraySource, string arrayName, int i, MemberMapping mapping, bool multiRef, string choiceSource)
			{
				this.source = source;
				this.arrayName = arrayName + "_" + i.ToString(CultureInfo.InvariantCulture);
				this.choiceArrayName = "choice_" + this.arrayName;
				this.choiceSource = choiceSource;
				ElementAccessor[] elements = mapping.Elements;
				if (mapping.TypeDesc.IsArrayLike)
				{
					if (arraySource != null)
					{
						this.arraySource = arraySource;
					}
					else
					{
						this.arraySource = outerClass.GetArraySource(mapping.TypeDesc, this.arrayName, multiRef);
					}
					this.isArray = mapping.TypeDesc.IsArray;
					this.isList = !this.isArray;
					if (mapping.ChoiceIdentifier != null)
					{
						this.choiceArraySource = outerClass.GetArraySource(mapping.TypeDesc, this.choiceArrayName, multiRef);
						string text = this.choiceArrayName;
						string text2 = "c" + text;
						bool useReflection = mapping.ChoiceIdentifier.Mapping.TypeDesc.UseReflection;
						string csharpName = mapping.ChoiceIdentifier.Mapping.TypeDesc.CSharpName;
						string text3 = (useReflection ? "" : ("(" + csharpName + "[])"));
						string text4 = string.Concat(new string[]
						{
							text,
							" = ",
							text3,
							"EnsureArrayIndex(",
							text,
							", ",
							text2,
							", ",
							outerClass.RaCodeGen.GetStringForTypeof(csharpName, useReflection),
							");"
						});
						this.choiceArraySource = text4 + outerClass.RaCodeGen.GetStringForArrayMember(text, text2 + "++", mapping.ChoiceIdentifier.Mapping.TypeDesc);
					}
					else
					{
						this.choiceArraySource = this.choiceSource;
					}
				}
				else
				{
					this.arraySource = ((arraySource == null) ? source : arraySource);
					this.choiceArraySource = this.choiceSource;
				}
				this.mapping = mapping;
			}

			// Token: 0x1700096F RID: 2415
			// (get) Token: 0x0600273D RID: 10045 RVA: 0x000C75C2 File Offset: 0x000C65C2
			internal MemberMapping Mapping
			{
				get
				{
					return this.mapping;
				}
			}

			// Token: 0x17000970 RID: 2416
			// (get) Token: 0x0600273E RID: 10046 RVA: 0x000C75CA File Offset: 0x000C65CA
			internal string Source
			{
				get
				{
					return this.source;
				}
			}

			// Token: 0x17000971 RID: 2417
			// (get) Token: 0x0600273F RID: 10047 RVA: 0x000C75D2 File Offset: 0x000C65D2
			internal string ArrayName
			{
				get
				{
					return this.arrayName;
				}
			}

			// Token: 0x17000972 RID: 2418
			// (get) Token: 0x06002740 RID: 10048 RVA: 0x000C75DA File Offset: 0x000C65DA
			internal string ArraySource
			{
				get
				{
					return this.arraySource;
				}
			}

			// Token: 0x17000973 RID: 2419
			// (get) Token: 0x06002741 RID: 10049 RVA: 0x000C75E2 File Offset: 0x000C65E2
			internal bool IsList
			{
				get
				{
					return this.isList;
				}
			}

			// Token: 0x17000974 RID: 2420
			// (get) Token: 0x06002742 RID: 10050 RVA: 0x000C75EA File Offset: 0x000C65EA
			internal bool IsArrayLike
			{
				get
				{
					return this.isArray || this.isList;
				}
			}

			// Token: 0x17000975 RID: 2421
			// (get) Token: 0x06002743 RID: 10051 RVA: 0x000C75FC File Offset: 0x000C65FC
			// (set) Token: 0x06002744 RID: 10052 RVA: 0x000C7604 File Offset: 0x000C6604
			internal bool IsNullable
			{
				get
				{
					return this.isNullable;
				}
				set
				{
					this.isNullable = value;
				}
			}

			// Token: 0x17000976 RID: 2422
			// (get) Token: 0x06002745 RID: 10053 RVA: 0x000C760D File Offset: 0x000C660D
			// (set) Token: 0x06002746 RID: 10054 RVA: 0x000C7615 File Offset: 0x000C6615
			internal bool MultiRef
			{
				get
				{
					return this.multiRef;
				}
				set
				{
					this.multiRef = value;
				}
			}

			// Token: 0x17000977 RID: 2423
			// (get) Token: 0x06002747 RID: 10055 RVA: 0x000C761E File Offset: 0x000C661E
			// (set) Token: 0x06002748 RID: 10056 RVA: 0x000C7626 File Offset: 0x000C6626
			internal int FixupIndex
			{
				get
				{
					return this.fixupIndex;
				}
				set
				{
					this.fixupIndex = value;
				}
			}

			// Token: 0x17000978 RID: 2424
			// (get) Token: 0x06002749 RID: 10057 RVA: 0x000C762F File Offset: 0x000C662F
			// (set) Token: 0x0600274A RID: 10058 RVA: 0x000C7637 File Offset: 0x000C6637
			internal string ParamsReadSource
			{
				get
				{
					return this.paramsReadSource;
				}
				set
				{
					this.paramsReadSource = value;
				}
			}

			// Token: 0x17000979 RID: 2425
			// (get) Token: 0x0600274B RID: 10059 RVA: 0x000C7640 File Offset: 0x000C6640
			// (set) Token: 0x0600274C RID: 10060 RVA: 0x000C7648 File Offset: 0x000C6648
			internal string CheckSpecifiedSource
			{
				get
				{
					return this.checkSpecifiedSource;
				}
				set
				{
					this.checkSpecifiedSource = value;
				}
			}

			// Token: 0x1700097A RID: 2426
			// (get) Token: 0x0600274D RID: 10061 RVA: 0x000C7651 File Offset: 0x000C6651
			internal string ChoiceSource
			{
				get
				{
					return this.choiceSource;
				}
			}

			// Token: 0x1700097B RID: 2427
			// (get) Token: 0x0600274E RID: 10062 RVA: 0x000C7659 File Offset: 0x000C6659
			internal string ChoiceArrayName
			{
				get
				{
					return this.choiceArrayName;
				}
			}

			// Token: 0x1700097C RID: 2428
			// (get) Token: 0x0600274F RID: 10063 RVA: 0x000C7661 File Offset: 0x000C6661
			internal string ChoiceArraySource
			{
				get
				{
					return this.choiceArraySource;
				}
			}

			// Token: 0x04001646 RID: 5702
			private string source;

			// Token: 0x04001647 RID: 5703
			private string arrayName;

			// Token: 0x04001648 RID: 5704
			private string arraySource;

			// Token: 0x04001649 RID: 5705
			private string choiceArrayName;

			// Token: 0x0400164A RID: 5706
			private string choiceSource;

			// Token: 0x0400164B RID: 5707
			private string choiceArraySource;

			// Token: 0x0400164C RID: 5708
			private MemberMapping mapping;

			// Token: 0x0400164D RID: 5709
			private bool isArray;

			// Token: 0x0400164E RID: 5710
			private bool isList;

			// Token: 0x0400164F RID: 5711
			private bool isNullable;

			// Token: 0x04001650 RID: 5712
			private bool multiRef;

			// Token: 0x04001651 RID: 5713
			private int fixupIndex = -1;

			// Token: 0x04001652 RID: 5714
			private string paramsReadSource;

			// Token: 0x04001653 RID: 5715
			private string checkSpecifiedSource;
		}
	}
}

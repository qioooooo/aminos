using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x020002F7 RID: 759
	internal class TypeScope
	{
		// Token: 0x060023AB RID: 9131 RVA: 0x000A88D0 File Offset: 0x000A78D0
		static TypeScope()
		{
			TypeScope.AddPrimitive(typeof(string), "string", "String", (TypeFlags)2106);
			TypeScope.AddPrimitive(typeof(int), "int", "Int32", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(bool), "boolean", "Boolean", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(short), "short", "Int16", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(long), "long", "Int64", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(float), "float", "Single", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(double), "double", "Double", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(decimal), "decimal", "Decimal", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(DateTime), "dateTime", "DateTime", (TypeFlags)4200);
			TypeScope.AddPrimitive(typeof(XmlQualifiedName), "QName", "XmlQualifiedName", (TypeFlags)5226);
			TypeScope.AddPrimitive(typeof(byte), "unsignedByte", "Byte", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(sbyte), "byte", "SByte", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(ushort), "unsignedShort", "UInt16", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(uint), "unsignedInt", "UInt32", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(ulong), "unsignedLong", "UInt64", (TypeFlags)4136);
			TypeScope.AddPrimitive(typeof(DateTime), "date", "Date", (TypeFlags)4328);
			TypeScope.AddPrimitive(typeof(DateTime), "time", "Time", (TypeFlags)4328);
			TypeScope.AddPrimitive(typeof(string), "Name", "XmlName", (TypeFlags)234);
			TypeScope.AddPrimitive(typeof(string), "NCName", "XmlNCName", (TypeFlags)234);
			TypeScope.AddPrimitive(typeof(string), "NMTOKEN", "XmlNmToken", (TypeFlags)234);
			TypeScope.AddPrimitive(typeof(string), "NMTOKENS", "XmlNmTokens", (TypeFlags)234);
			TypeScope.AddPrimitive(typeof(byte[]), "base64Binary", "ByteArrayBase64", (TypeFlags)6890);
			TypeScope.AddPrimitive(typeof(byte[]), "hexBinary", "ByteArrayHex", (TypeFlags)6890);
			XmlSchemaPatternFacet xmlSchemaPatternFacet = new XmlSchemaPatternFacet();
			xmlSchemaPatternFacet.Value = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
			TypeScope.AddNonXsdPrimitive(typeof(Guid), "guid", "http://microsoft.com/wsdl/types/", "Guid", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), new XmlSchemaFacet[] { xmlSchemaPatternFacet }, (TypeFlags)4648);
			TypeScope.AddNonXsdPrimitive(typeof(char), "char", "http://microsoft.com/wsdl/types/", "Char", new XmlQualifiedName("unsignedShort", "http://www.w3.org/2001/XMLSchema"), new XmlSchemaFacet[0], (TypeFlags)616);
			TypeScope.AddSoapEncodedTypes("http://schemas.xmlsoap.org/soap/encoding/");
			TypeScope.AddPrimitive(typeof(string), "normalizedString", "String", (TypeFlags)2234);
			for (int i = 0; i < TypeScope.unsupportedTypes.Length; i++)
			{
				TypeScope.AddPrimitive(typeof(string), TypeScope.unsupportedTypes[i], "String", (TypeFlags)32954);
			}
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x000A8D48 File Offset: 0x000A7D48
		internal static bool IsKnownType(Type type)
		{
			if (type == typeof(object))
			{
				return true;
			}
			if (type.IsEnum)
			{
				return false;
			}
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				return true;
			case TypeCode.Char:
				return true;
			case TypeCode.SByte:
				return true;
			case TypeCode.Byte:
				return true;
			case TypeCode.Int16:
				return true;
			case TypeCode.UInt16:
				return true;
			case TypeCode.Int32:
				return true;
			case TypeCode.UInt32:
				return true;
			case TypeCode.Int64:
				return true;
			case TypeCode.UInt64:
				return true;
			case TypeCode.Single:
				return true;
			case TypeCode.Double:
				return true;
			case TypeCode.Decimal:
				return true;
			case TypeCode.DateTime:
				return true;
			case TypeCode.String:
				return true;
			}
			return type == typeof(XmlQualifiedName) || type == typeof(byte[]) || type == typeof(Guid) || type == typeof(XmlNode[]);
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x000A8E1C File Offset: 0x000A7E1C
		private static void AddSoapEncodedTypes(string ns)
		{
			TypeScope.AddSoapEncodedPrimitive(typeof(string), "normalizedString", ns, "String", new XmlQualifiedName("normalizedString", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)2218);
			for (int i = 0; i < TypeScope.unsupportedTypes.Length; i++)
			{
				TypeScope.AddSoapEncodedPrimitive(typeof(string), TypeScope.unsupportedTypes[i], ns, "String", new XmlQualifiedName(TypeScope.unsupportedTypes[i], "http://www.w3.org/2001/XMLSchema"), (TypeFlags)32938);
			}
			TypeScope.AddSoapEncodedPrimitive(typeof(string), "string", ns, "String", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)58);
			TypeScope.AddSoapEncodedPrimitive(typeof(int), "int", ns, "Int32", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(bool), "boolean", ns, "Boolean", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(short), "short", ns, "Int16", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(long), "long", ns, "Int64", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(float), "float", ns, "Single", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(double), "double", ns, "Double", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(decimal), "decimal", ns, "Decimal", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(DateTime), "dateTime", ns, "DateTime", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4200);
			TypeScope.AddSoapEncodedPrimitive(typeof(XmlQualifiedName), "QName", ns, "XmlQualifiedName", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)5226);
			TypeScope.AddSoapEncodedPrimitive(typeof(byte), "unsignedByte", ns, "Byte", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(sbyte), "byte", ns, "SByte", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(ushort), "unsignedShort", ns, "UInt16", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(uint), "unsignedInt", ns, "UInt32", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(ulong), "unsignedLong", ns, "UInt64", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4136);
			TypeScope.AddSoapEncodedPrimitive(typeof(DateTime), "date", ns, "Date", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4328);
			TypeScope.AddSoapEncodedPrimitive(typeof(DateTime), "time", ns, "Time", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4328);
			TypeScope.AddSoapEncodedPrimitive(typeof(string), "Name", ns, "XmlName", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)234);
			TypeScope.AddSoapEncodedPrimitive(typeof(string), "NCName", ns, "XmlNCName", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)234);
			TypeScope.AddSoapEncodedPrimitive(typeof(string), "NMTOKEN", ns, "XmlNmToken", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)234);
			TypeScope.AddSoapEncodedPrimitive(typeof(string), "NMTOKENS", ns, "XmlNmTokens", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)234);
			TypeScope.AddSoapEncodedPrimitive(typeof(byte[]), "base64Binary", ns, "ByteArrayBase64", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4842);
			TypeScope.AddSoapEncodedPrimitive(typeof(byte[]), "hexBinary", ns, "ByteArrayHex", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)4842);
			TypeScope.AddSoapEncodedPrimitive(typeof(string), "arrayCoordinate", ns, "String", new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)40);
			TypeScope.AddSoapEncodedPrimitive(typeof(byte[]), "base64", ns, "ByteArrayBase64", new XmlQualifiedName("base64Binary", "http://www.w3.org/2001/XMLSchema"), (TypeFlags)554);
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x000A9314 File Offset: 0x000A8314
		private static void AddPrimitive(Type type, string dataTypeName, string formatterName, TypeFlags flags)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
			xmlSchemaSimpleType.Name = dataTypeName;
			TypeDesc typeDesc = new TypeDesc(type, true, xmlSchemaSimpleType, formatterName, flags);
			if (TypeScope.primitiveTypes[type] == null)
			{
				TypeScope.primitiveTypes.Add(type, typeDesc);
			}
			TypeScope.primitiveDataTypes.Add(xmlSchemaSimpleType, typeDesc);
			TypeScope.primitiveNames.Add(dataTypeName, "http://www.w3.org/2001/XMLSchema", typeDesc);
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x000A9370 File Offset: 0x000A8370
		private static void AddNonXsdPrimitive(Type type, string dataTypeName, string ns, string formatterName, XmlQualifiedName baseTypeName, XmlSchemaFacet[] facets, TypeFlags flags)
		{
			XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
			xmlSchemaSimpleType.Name = dataTypeName;
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
			xmlSchemaSimpleTypeRestriction.BaseTypeName = baseTypeName;
			foreach (XmlSchemaFacet xmlSchemaFacet in facets)
			{
				xmlSchemaSimpleTypeRestriction.Facets.Add(xmlSchemaFacet);
			}
			xmlSchemaSimpleType.Content = xmlSchemaSimpleTypeRestriction;
			TypeDesc typeDesc = new TypeDesc(type, false, xmlSchemaSimpleType, formatterName, flags);
			if (TypeScope.primitiveTypes[type] == null)
			{
				TypeScope.primitiveTypes.Add(type, typeDesc);
			}
			TypeScope.primitiveDataTypes.Add(xmlSchemaSimpleType, typeDesc);
			TypeScope.primitiveNames.Add(dataTypeName, ns, typeDesc);
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x000A9407 File Offset: 0x000A8407
		private static void AddSoapEncodedPrimitive(Type type, string dataTypeName, string ns, string formatterName, XmlQualifiedName baseTypeName, TypeFlags flags)
		{
			TypeScope.AddNonXsdPrimitive(type, dataTypeName, ns, formatterName, baseTypeName, new XmlSchemaFacet[0], flags);
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x000A941C File Offset: 0x000A841C
		internal TypeDesc GetTypeDesc(string name, string ns)
		{
			return this.GetTypeDesc(name, ns, (TypeFlags)56);
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x000A9428 File Offset: 0x000A8428
		internal TypeDesc GetTypeDesc(string name, string ns, TypeFlags flags)
		{
			TypeDesc typeDesc = (TypeDesc)TypeScope.primitiveNames[name, ns];
			if (typeDesc != null && (typeDesc.Flags & flags) != TypeFlags.None)
			{
				return typeDesc;
			}
			return null;
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x000A9457 File Offset: 0x000A8457
		internal TypeDesc GetTypeDesc(XmlSchemaSimpleType dataType)
		{
			return (TypeDesc)TypeScope.primitiveDataTypes[dataType];
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000A9469 File Offset: 0x000A8469
		internal TypeDesc GetTypeDesc(Type type)
		{
			return this.GetTypeDesc(type, null, true, true);
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000A9475 File Offset: 0x000A8475
		internal TypeDesc GetTypeDesc(Type type, MemberInfo source)
		{
			return this.GetTypeDesc(type, source, true, true);
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x000A9481 File Offset: 0x000A8481
		internal TypeDesc GetTypeDesc(Type type, MemberInfo source, bool directReference)
		{
			return this.GetTypeDesc(type, source, directReference, true);
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x000A9490 File Offset: 0x000A8490
		internal TypeDesc GetTypeDesc(Type type, MemberInfo source, bool directReference, bool throwOnError)
		{
			if (type.ContainsGenericParameters)
			{
				throw new InvalidOperationException(Res.GetString("XmlUnsupportedOpenGenericType", new object[] { type.ToString() }));
			}
			TypeDesc typeDesc = (TypeDesc)TypeScope.primitiveTypes[type];
			if (typeDesc == null)
			{
				typeDesc = (TypeDesc)this.typeDescs[type];
				if (typeDesc == null)
				{
					typeDesc = this.ImportTypeDesc(type, source, directReference);
				}
			}
			if (throwOnError)
			{
				typeDesc.CheckSupported();
			}
			return typeDesc;
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x000A9504 File Offset: 0x000A8504
		internal TypeDesc GetArrayTypeDesc(Type type)
		{
			TypeDesc typeDesc = (TypeDesc)this.arrayTypeDescs[type];
			if (typeDesc == null)
			{
				typeDesc = this.GetTypeDesc(type);
				if (!typeDesc.IsArrayLike)
				{
					typeDesc = this.ImportTypeDesc(type, null, false);
				}
				typeDesc.CheckSupported();
				this.arrayTypeDescs.Add(type, typeDesc);
			}
			return typeDesc;
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x000A9554 File Offset: 0x000A8554
		internal TypeMapping GetTypeMappingFromTypeDesc(TypeDesc typeDesc)
		{
			foreach (object obj in this.TypeMappings)
			{
				TypeMapping typeMapping = (TypeMapping)obj;
				if (typeMapping.TypeDesc == typeDesc)
				{
					return typeMapping;
				}
			}
			return null;
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000A95B8 File Offset: 0x000A85B8
		internal Type GetTypeFromTypeDesc(TypeDesc typeDesc)
		{
			if (typeDesc.Type != null)
			{
				return typeDesc.Type;
			}
			foreach (object obj in this.typeDescs)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (dictionaryEntry.Value == typeDesc)
				{
					return dictionaryEntry.Key as Type;
				}
			}
			return null;
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x000A9638 File Offset: 0x000A8638
		private TypeDesc ImportTypeDesc(Type type, MemberInfo memberInfo, bool directReference)
		{
			Type type2 = null;
			Type type3 = null;
			TypeFlags typeFlags = TypeFlags.None;
			Exception ex = null;
			if (!type.IsPublic && !type.IsNestedPublic)
			{
				typeFlags |= TypeFlags.Unsupported;
				ex = new InvalidOperationException(Res.GetString("XmlTypeInaccessible", new object[] { type.FullName }));
			}
			else if (type.IsAbstract && type.IsSealed)
			{
				typeFlags |= TypeFlags.Unsupported;
				ex = new InvalidOperationException(Res.GetString("XmlTypeStatic", new object[] { type.FullName }));
			}
			if (DynamicAssemblies.IsTypeDynamic(type))
			{
				typeFlags |= TypeFlags.UseReflection;
			}
			if (!type.IsValueType)
			{
				typeFlags |= TypeFlags.Reference;
			}
			TypeKind typeKind;
			if (type == typeof(object))
			{
				typeKind = TypeKind.Root;
				typeFlags |= TypeFlags.HasDefaultConstructor;
			}
			else if (type == typeof(ValueType))
			{
				typeKind = TypeKind.Enum;
				typeFlags |= TypeFlags.Unsupported;
				if (ex == null)
				{
					ex = new NotSupportedException(Res.GetString("XmlSerializerUnsupportedType", new object[] { type.FullName }));
				}
			}
			else if (type == typeof(void))
			{
				typeKind = TypeKind.Void;
			}
			else if (typeof(IXmlSerializable).IsAssignableFrom(type))
			{
				typeKind = TypeKind.Serializable;
				typeFlags |= (TypeFlags)36;
				typeFlags |= TypeScope.GetConstructorFlags(type, ref ex);
			}
			else if (type.IsArray)
			{
				typeKind = TypeKind.Array;
				if (type.GetArrayRank() > 1)
				{
					typeFlags |= TypeFlags.Unsupported;
					if (ex == null)
					{
						ex = new NotSupportedException(Res.GetString("XmlUnsupportedRank", new object[] { type.FullName }));
					}
				}
				type2 = type.GetElementType();
				typeFlags |= TypeFlags.HasDefaultConstructor;
			}
			else if (typeof(ICollection).IsAssignableFrom(type))
			{
				typeKind = TypeKind.Collection;
				type2 = TypeScope.GetCollectionElementType(type, (memberInfo == null) ? null : (memberInfo.DeclaringType.FullName + "." + memberInfo.Name));
				typeFlags |= TypeScope.GetConstructorFlags(type, ref ex);
			}
			else if (type == typeof(XmlQualifiedName))
			{
				typeKind = TypeKind.Primitive;
			}
			else if (type.IsPrimitive)
			{
				typeKind = TypeKind.Primitive;
				typeFlags |= TypeFlags.Unsupported;
				if (ex == null)
				{
					ex = new NotSupportedException(Res.GetString("XmlSerializerUnsupportedType", new object[] { type.FullName }));
				}
			}
			else if (type.IsEnum)
			{
				typeKind = TypeKind.Enum;
			}
			else if (type.IsValueType)
			{
				typeKind = TypeKind.Struct;
				if (TypeScope.IsOptionalValue(type))
				{
					type3 = type.GetGenericArguments()[0];
					typeFlags |= TypeFlags.OptionalValue;
				}
				else
				{
					type3 = type.BaseType;
				}
				if (type.IsAbstract)
				{
					typeFlags |= TypeFlags.Abstract;
				}
			}
			else if (type.IsClass)
			{
				if (type == typeof(XmlAttribute))
				{
					typeKind = TypeKind.Attribute;
					typeFlags |= (TypeFlags)12;
				}
				else if (typeof(XmlNode).IsAssignableFrom(type))
				{
					typeKind = TypeKind.Node;
					type3 = type.BaseType;
					typeFlags |= (TypeFlags)52;
					if (typeof(XmlText).IsAssignableFrom(type))
					{
						typeFlags &= (TypeFlags)(-33);
					}
					else if (typeof(XmlElement).IsAssignableFrom(type))
					{
						typeFlags &= (TypeFlags)(-17);
					}
					else if (type.IsAssignableFrom(typeof(XmlAttribute)))
					{
						typeFlags |= TypeFlags.CanBeAttributeValue;
					}
				}
				else
				{
					typeKind = TypeKind.Class;
					type3 = type.BaseType;
					if (type.IsAbstract)
					{
						typeFlags |= TypeFlags.Abstract;
					}
				}
			}
			else if (type.IsInterface)
			{
				typeKind = TypeKind.Void;
				typeFlags |= TypeFlags.Unsupported;
				if (ex == null)
				{
					if (memberInfo == null)
					{
						ex = new NotSupportedException(Res.GetString("XmlUnsupportedInterface", new object[] { type.FullName }));
					}
					else
					{
						ex = new NotSupportedException(Res.GetString("XmlUnsupportedInterfaceDetails", new object[]
						{
							memberInfo.DeclaringType.FullName + "." + memberInfo.Name,
							type.FullName
						}));
					}
				}
			}
			else
			{
				typeKind = TypeKind.Void;
				typeFlags |= TypeFlags.Unsupported;
				if (ex == null)
				{
					ex = new NotSupportedException(Res.GetString("XmlSerializerUnsupportedType", new object[] { type.FullName }));
				}
			}
			if (typeKind == TypeKind.Class && !type.IsAbstract)
			{
				typeFlags |= TypeScope.GetConstructorFlags(type, ref ex);
			}
			if ((typeKind == TypeKind.Struct || typeKind == TypeKind.Class) && typeof(IEnumerable).IsAssignableFrom(type))
			{
				type2 = TypeScope.GetEnumeratorElementType(type, ref typeFlags);
				typeKind = TypeKind.Enumerable;
				typeFlags |= TypeScope.GetConstructorFlags(type, ref ex);
			}
			TypeDesc typeDesc = new TypeDesc(type, CodeIdentifier.MakeValid(TypeScope.TypeName(type)), type.ToString(), typeKind, null, typeFlags, null);
			typeDesc.Exception = ex;
			if (directReference && (typeDesc.IsClass || typeKind == TypeKind.Serializable))
			{
				typeDesc.CheckNeedConstructor();
			}
			if (typeDesc.IsUnsupported)
			{
				return typeDesc;
			}
			this.typeDescs.Add(type, typeDesc);
			if (type2 != null)
			{
				TypeDesc typeDesc2 = this.GetTypeDesc(type2, memberInfo, true, false);
				if (directReference && (typeDesc2.IsCollection || typeDesc2.IsEnumerable) && !typeDesc2.IsPrimitive)
				{
					typeDesc2.CheckNeedConstructor();
				}
				typeDesc.ArrayElementTypeDesc = typeDesc2;
			}
			if (type3 != null && type3 != typeof(object) && type3 != typeof(ValueType))
			{
				typeDesc.BaseTypeDesc = this.GetTypeDesc(type3, memberInfo, false, false);
			}
			if (type.IsNestedPublic)
			{
				Type type4 = type.DeclaringType;
				while (type4 != null && !type4.ContainsGenericParameters)
				{
					this.GetTypeDesc(type4, null, false);
					type4 = type4.DeclaringType;
				}
			}
			return typeDesc;
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x000A9BB3 File Offset: 0x000A8BB3
		internal static bool IsOptionalValue(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>).GetGenericTypeDefinition();
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x000A9BD8 File Offset: 0x000A8BD8
		internal static string TypeName(Type t)
		{
			if (t.IsArray)
			{
				return "ArrayOf" + TypeScope.TypeName(t.GetElementType());
			}
			if (t.IsGenericType)
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				string text = t.Name;
				int num = text.IndexOf("`", StringComparison.Ordinal);
				if (num >= 0)
				{
					text = text.Substring(0, num);
				}
				stringBuilder.Append(text);
				stringBuilder.Append("Of");
				Type[] genericArguments = t.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					stringBuilder.Append(TypeScope.TypeName(genericArguments[i]));
					stringBuilder2.Append(genericArguments[i].Namespace);
				}
				return stringBuilder.ToString();
			}
			return t.Name;
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x000A9C9C File Offset: 0x000A8C9C
		internal static Type GetArrayElementType(Type type, string memberInfo)
		{
			if (type.IsArray)
			{
				return type.GetElementType();
			}
			if (typeof(ICollection).IsAssignableFrom(type))
			{
				return TypeScope.GetCollectionElementType(type, memberInfo);
			}
			if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				TypeFlags typeFlags = TypeFlags.None;
				return TypeScope.GetEnumeratorElementType(type, ref typeFlags);
			}
			return null;
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x000A9CF0 File Offset: 0x000A8CF0
		internal static MemberMapping[] GetAllMembers(StructMapping mapping)
		{
			if (mapping.BaseMapping == null)
			{
				return mapping.Members;
			}
			ArrayList arrayList = new ArrayList();
			TypeScope.GetAllMembers(mapping, arrayList);
			return (MemberMapping[])arrayList.ToArray(typeof(MemberMapping));
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x000A9D30 File Offset: 0x000A8D30
		internal static void GetAllMembers(StructMapping mapping, ArrayList list)
		{
			if (mapping.BaseMapping != null)
			{
				TypeScope.GetAllMembers(mapping.BaseMapping, list);
			}
			for (int i = 0; i < mapping.Members.Length; i++)
			{
				list.Add(mapping.Members[i]);
			}
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x000A9D74 File Offset: 0x000A8D74
		private static TypeFlags GetConstructorFlags(Type type, ref Exception exception)
		{
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
			if (constructor != null)
			{
				TypeFlags typeFlags = TypeFlags.HasDefaultConstructor;
				if (!constructor.IsPublic)
				{
					typeFlags |= TypeFlags.CtorInaccessible;
				}
				else
				{
					object[] customAttributes = constructor.GetCustomAttributes(typeof(ObsoleteAttribute), false);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						ObsoleteAttribute obsoleteAttribute = (ObsoleteAttribute)customAttributes[0];
						if (obsoleteAttribute.IsError)
						{
							typeFlags |= TypeFlags.CtorInaccessible;
						}
					}
				}
				return typeFlags;
			}
			return TypeFlags.None;
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000A9DE4 File Offset: 0x000A8DE4
		private static Type GetEnumeratorElementType(Type type, ref TypeFlags flags)
		{
			if (!typeof(IEnumerable).IsAssignableFrom(type))
			{
				return null;
			}
			MethodInfo methodInfo = type.GetMethod("GetEnumerator", new Type[0]);
			if (methodInfo == null || !typeof(IEnumerator).IsAssignableFrom(methodInfo.ReturnType))
			{
				methodInfo = null;
				foreach (MemberInfo memberInfo in type.GetMember("System.Collections.Generic.IEnumerable<*", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					methodInfo = memberInfo as MethodInfo;
					if (methodInfo != null && typeof(IEnumerator).IsAssignableFrom(methodInfo.ReturnType))
					{
						flags |= TypeFlags.GenericInterface;
						break;
					}
					methodInfo = null;
				}
				if (methodInfo == null)
				{
					flags |= TypeFlags.UsePrivateImplementation;
					methodInfo = type.GetMethod("System.Collections.IEnumerable.GetEnumerator", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
				}
			}
			if (methodInfo == null || !typeof(IEnumerator).IsAssignableFrom(methodInfo.ReturnType))
			{
				return null;
			}
			XmlAttributes xmlAttributes = new XmlAttributes(methodInfo);
			if (xmlAttributes.XmlIgnore)
			{
				return null;
			}
			PropertyInfo property = methodInfo.ReturnType.GetProperty("Current");
			Type type2 = ((property == null) ? typeof(object) : property.PropertyType);
			MethodInfo methodInfo2 = type.GetMethod("Add", new Type[] { type2 });
			if (methodInfo2 == null && type2 != typeof(object))
			{
				type2 = typeof(object);
				methodInfo2 = type.GetMethod("Add", new Type[] { type2 });
			}
			if (methodInfo2 == null)
			{
				throw new InvalidOperationException(Res.GetString("XmlNoAddMethod", new object[] { type.FullName, type2, "IEnumerable" }));
			}
			return type2;
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x000A9F98 File Offset: 0x000A8F98
		internal static PropertyInfo GetDefaultIndexer(Type type, string memberInfo)
		{
			if (typeof(IDictionary).IsAssignableFrom(type))
			{
				if (memberInfo == null)
				{
					throw new NotSupportedException(Res.GetString("XmlUnsupportedIDictionary", new object[] { type.FullName }));
				}
				throw new NotSupportedException(Res.GetString("XmlUnsupportedIDictionaryDetails", new object[] { memberInfo, type.FullName }));
			}
			else
			{
				MemberInfo[] defaultMembers = type.GetDefaultMembers();
				PropertyInfo propertyInfo = null;
				if (defaultMembers != null && defaultMembers.Length > 0)
				{
					for (Type type2 = type; type2 != null; type2 = type2.BaseType)
					{
						for (int i = 0; i < defaultMembers.Length; i++)
						{
							if (defaultMembers[i] is PropertyInfo)
							{
								PropertyInfo propertyInfo2 = (PropertyInfo)defaultMembers[i];
								if (propertyInfo2.DeclaringType == type2 && propertyInfo2.CanRead)
								{
									MethodInfo getMethod = propertyInfo2.GetGetMethod();
									ParameterInfo[] parameters = getMethod.GetParameters();
									if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int))
									{
										propertyInfo = propertyInfo2;
										break;
									}
								}
							}
						}
						if (propertyInfo != null)
						{
							break;
						}
					}
				}
				if (propertyInfo == null)
				{
					throw new InvalidOperationException(Res.GetString("XmlNoDefaultAccessors", new object[] { type.FullName }));
				}
				if (type.GetMethod("Add", new Type[] { propertyInfo.PropertyType }) == null)
				{
					throw new InvalidOperationException(Res.GetString("XmlNoAddMethod", new object[] { type.FullName, propertyInfo.PropertyType, "ICollection" }));
				}
				return propertyInfo;
			}
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x000AA116 File Offset: 0x000A9116
		private static Type GetCollectionElementType(Type type, string memberInfo)
		{
			return TypeScope.GetDefaultIndexer(type, memberInfo).PropertyType;
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x000AA124 File Offset: 0x000A9124
		internal static XmlQualifiedName ParseWsdlArrayType(string type, out string dims, XmlSchemaObject parent)
		{
			int num = type.LastIndexOf(':');
			string text;
			if (num <= 0)
			{
				text = "";
			}
			else
			{
				text = type.Substring(0, num);
			}
			int num2 = type.IndexOf('[', num + 1);
			if (num2 <= num)
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidArrayTypeSyntax", new object[] { type }));
			}
			string text2 = type.Substring(num + 1, num2 - num - 1);
			dims = type.Substring(num2);
			while (parent != null)
			{
				if (parent.Namespaces != null)
				{
					string text3 = (string)parent.Namespaces.Namespaces[text];
					if (text3 != null)
					{
						text = text3;
						break;
					}
				}
				parent = parent.Parent;
			}
			return new XmlQualifiedName(text2, text);
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x000AA1D3 File Offset: 0x000A91D3
		internal ICollection Types
		{
			get
			{
				return this.typeDescs.Keys;
			}
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x000AA1E0 File Offset: 0x000A91E0
		internal void AddTypeMapping(TypeMapping typeMapping)
		{
			this.typeMappings.Add(typeMapping);
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x000AA1EF File Offset: 0x000A91EF
		internal ICollection TypeMappings
		{
			get
			{
				return this.typeMappings;
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x060023C9 RID: 9161 RVA: 0x000AA1F7 File Offset: 0x000A91F7
		internal static Hashtable PrimtiveTypes
		{
			get
			{
				return TypeScope.primitiveTypes;
			}
		}

		// Token: 0x0400152B RID: 5419
		private Hashtable typeDescs = new Hashtable();

		// Token: 0x0400152C RID: 5420
		private Hashtable arrayTypeDescs = new Hashtable();

		// Token: 0x0400152D RID: 5421
		private ArrayList typeMappings = new ArrayList();

		// Token: 0x0400152E RID: 5422
		private static Hashtable primitiveTypes = new Hashtable();

		// Token: 0x0400152F RID: 5423
		private static Hashtable primitiveDataTypes = new Hashtable();

		// Token: 0x04001530 RID: 5424
		private static NameTable primitiveNames = new NameTable();

		// Token: 0x04001531 RID: 5425
		private static string[] unsupportedTypes = new string[]
		{
			"anyURI", "duration", "ENTITY", "ENTITIES", "gDay", "gMonth", "gMonthDay", "gYear", "gYearMonth", "ID",
			"IDREF", "IDREFS", "integer", "language", "negativeInteger", "nonNegativeInteger", "nonPositiveInteger", "NOTATION", "positiveInteger", "token"
		};
	}
}

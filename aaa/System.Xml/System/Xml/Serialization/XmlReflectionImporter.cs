using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x02000314 RID: 788
	public class XmlReflectionImporter
	{
		// Token: 0x06002533 RID: 9523 RVA: 0x000AE2BF File Offset: 0x000AD2BF
		public XmlReflectionImporter()
			: this(null, null)
		{
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x000AE2C9 File Offset: 0x000AD2C9
		public XmlReflectionImporter(string defaultNamespace)
			: this(null, defaultNamespace)
		{
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x000AE2D3 File Offset: 0x000AD2D3
		public XmlReflectionImporter(XmlAttributeOverrides attributeOverrides)
			: this(attributeOverrides, null)
		{
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x000AE2E0 File Offset: 0x000AD2E0
		public XmlReflectionImporter(XmlAttributeOverrides attributeOverrides, string defaultNamespace)
		{
			if (defaultNamespace == null)
			{
				defaultNamespace = string.Empty;
			}
			if (attributeOverrides == null)
			{
				attributeOverrides = new XmlAttributeOverrides();
			}
			this.attributeOverrides = attributeOverrides;
			this.defaultNs = defaultNamespace;
			this.typeScope = new TypeScope();
			this.modelScope = new ModelScope(this.typeScope);
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000AE36F File Offset: 0x000AD36F
		public void IncludeTypes(ICustomAttributeProvider provider)
		{
			this.IncludeTypes(provider, new RecursionLimiter());
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000AE380 File Offset: 0x000AD380
		private void IncludeTypes(ICustomAttributeProvider provider, RecursionLimiter limiter)
		{
			object[] customAttributes = provider.GetCustomAttributes(typeof(XmlIncludeAttribute), false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				Type type = ((XmlIncludeAttribute)customAttributes[i]).Type;
				this.IncludeType(type, limiter);
			}
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000AE3C3 File Offset: 0x000AD3C3
		public void IncludeType(Type type)
		{
			this.IncludeType(type, new RecursionLimiter());
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000AE3D4 File Offset: 0x000AD3D4
		private void IncludeType(Type type, RecursionLimiter limiter)
		{
			int num = this.arrayNestingLevel;
			XmlArrayItemAttributes xmlArrayItemAttributes = this.savedArrayItemAttributes;
			string text = this.savedArrayNamespace;
			this.arrayNestingLevel = 0;
			this.savedArrayItemAttributes = null;
			this.savedArrayNamespace = null;
			TypeMapping typeMapping = this.ImportTypeMapping(this.modelScope.GetTypeModel(type), this.defaultNs, XmlReflectionImporter.ImportContext.Element, string.Empty, null, limiter);
			if (typeMapping.IsAnonymousType && !typeMapping.TypeDesc.IsSpecial)
			{
				throw new InvalidOperationException(Res.GetString("XmlAnonymousInclude", new object[] { type.FullName }));
			}
			this.arrayNestingLevel = num;
			this.savedArrayItemAttributes = xmlArrayItemAttributes;
			this.savedArrayNamespace = text;
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000AE47A File Offset: 0x000AD47A
		public XmlTypeMapping ImportTypeMapping(Type type)
		{
			return this.ImportTypeMapping(type, null, null);
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000AE485 File Offset: 0x000AD485
		public XmlTypeMapping ImportTypeMapping(Type type, string defaultNamespace)
		{
			return this.ImportTypeMapping(type, null, defaultNamespace);
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000AE490 File Offset: 0x000AD490
		public XmlTypeMapping ImportTypeMapping(Type type, XmlRootAttribute root)
		{
			return this.ImportTypeMapping(type, root, null);
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000AE49C File Offset: 0x000AD49C
		public XmlTypeMapping ImportTypeMapping(Type type, XmlRootAttribute root, string defaultNamespace)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			XmlTypeMapping xmlTypeMapping = new XmlTypeMapping(this.typeScope, this.ImportElement(this.modelScope.GetTypeModel(type), root, defaultNamespace, new RecursionLimiter()));
			xmlTypeMapping.SetKeyInternal(XmlMapping.GenerateKey(type, root, defaultNamespace));
			xmlTypeMapping.GenerateSerializer = true;
			return xmlTypeMapping;
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x000AE4F2 File Offset: 0x000AD4F2
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members, bool hasWrapperElement)
		{
			return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, false);
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x000AE500 File Offset: 0x000AD500
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members, bool hasWrapperElement, bool rpc)
		{
			return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, rpc, false);
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x000AE510 File Offset: 0x000AD510
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members, bool hasWrapperElement, bool rpc, bool openModel)
		{
			return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, rpc, openModel, XmlMappingAccess.Read | XmlMappingAccess.Write);
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x000AE524 File Offset: 0x000AD524
		public XmlMembersMapping ImportMembersMapping(string elementName, string ns, XmlReflectionMember[] members, bool hasWrapperElement, bool rpc, bool openModel, XmlMappingAccess access)
		{
			ElementAccessor elementAccessor = new ElementAccessor();
			elementAccessor.Name = ((elementName == null || elementName.Length == 0) ? elementName : XmlConvert.EncodeLocalName(elementName));
			elementAccessor.Namespace = ns;
			MembersMapping membersMapping = this.ImportMembersMapping(members, ns, hasWrapperElement, rpc, openModel, new RecursionLimiter());
			elementAccessor.Mapping = membersMapping;
			elementAccessor.Form = XmlSchemaForm.Qualified;
			if (!rpc)
			{
				if (hasWrapperElement)
				{
					elementAccessor = (ElementAccessor)this.ReconcileAccessor(elementAccessor, this.elements);
				}
				else
				{
					foreach (MemberMapping memberMapping in membersMapping.Members)
					{
						if (memberMapping.Elements != null && memberMapping.Elements.Length > 0)
						{
							memberMapping.Elements[0] = (ElementAccessor)this.ReconcileAccessor(memberMapping.Elements[0], this.elements);
						}
					}
				}
			}
			return new XmlMembersMapping(this.typeScope, elementAccessor, access)
			{
				GenerateSerializer = true
			};
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x000AE604 File Offset: 0x000AD604
		private XmlAttributes GetAttributes(Type type, bool canBeSimpleType)
		{
			XmlAttributes xmlAttributes = this.attributeOverrides[type];
			if (xmlAttributes != null)
			{
				return xmlAttributes;
			}
			if (canBeSimpleType && TypeScope.IsKnownType(type))
			{
				return this.defaultAttributes;
			}
			return new XmlAttributes(type);
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x000AE63C File Offset: 0x000AD63C
		private XmlAttributes GetAttributes(MemberInfo memberInfo)
		{
			XmlAttributes xmlAttributes = this.attributeOverrides[memberInfo.DeclaringType, memberInfo.Name];
			if (xmlAttributes != null)
			{
				return xmlAttributes;
			}
			return new XmlAttributes(memberInfo);
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x000AE66C File Offset: 0x000AD66C
		private ElementAccessor ImportElement(TypeModel model, XmlRootAttribute root, string defaultNamespace, RecursionLimiter limiter)
		{
			XmlAttributes attributes = this.GetAttributes(model.Type, true);
			if (root == null)
			{
				root = attributes.XmlRoot;
			}
			string text = ((root == null) ? null : root.Namespace);
			if (text == null)
			{
				text = defaultNamespace;
			}
			if (text == null)
			{
				text = this.defaultNs;
			}
			this.arrayNestingLevel = -1;
			this.savedArrayItemAttributes = null;
			this.savedArrayNamespace = null;
			ElementAccessor elementAccessor = XmlReflectionImporter.CreateElementAccessor(this.ImportTypeMapping(model, text, XmlReflectionImporter.ImportContext.Element, string.Empty, attributes, limiter), text);
			if (root != null)
			{
				if (root.ElementName.Length > 0)
				{
					elementAccessor.Name = XmlConvert.EncodeLocalName(root.ElementName);
				}
				if (root.IsNullableSpecified && !root.IsNullable && model.TypeDesc.IsOptionalValue)
				{
					throw new InvalidOperationException(Res.GetString("XmlInvalidNotNullable", new object[]
					{
						model.TypeDesc.BaseTypeDesc.FullName,
						"XmlRoot"
					}));
				}
				elementAccessor.IsNullable = (root.IsNullableSpecified ? root.IsNullable : (model.TypeDesc.IsNullable || model.TypeDesc.IsOptionalValue));
				XmlReflectionImporter.CheckNullable(elementAccessor.IsNullable, model.TypeDesc, elementAccessor.Mapping);
			}
			else
			{
				elementAccessor.IsNullable = model.TypeDesc.IsNullable || model.TypeDesc.IsOptionalValue;
			}
			elementAccessor.Form = XmlSchemaForm.Qualified;
			return (ElementAccessor)this.ReconcileAccessor(elementAccessor, this.elements);
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x000AE7D4 File Offset: 0x000AD7D4
		private static string GetMappingName(Mapping mapping)
		{
			if (mapping is MembersMapping)
			{
				return "(method)";
			}
			if (mapping is TypeMapping)
			{
				return ((TypeMapping)mapping).TypeDesc.FullName;
			}
			throw new ArgumentException(Res.GetString("XmlInternalError"), "mapping");
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x000AE811 File Offset: 0x000AD811
		private ElementAccessor ReconcileLocalAccessor(ElementAccessor accessor, string ns)
		{
			if (accessor.Namespace == ns)
			{
				return accessor;
			}
			return (ElementAccessor)this.ReconcileAccessor(accessor, this.elements);
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000AE838 File Offset: 0x000AD838
		private Accessor ReconcileAccessor(Accessor accessor, NameTable accessors)
		{
			if (accessor.Any && accessor.Name.Length == 0)
			{
				return accessor;
			}
			Accessor accessor2 = (Accessor)accessors[accessor.Name, accessor.Namespace];
			if (accessor2 == null)
			{
				accessor.IsTopLevelInSchema = true;
				accessors.Add(accessor.Name, accessor.Namespace, accessor);
				return accessor;
			}
			if (accessor2.Mapping == accessor.Mapping)
			{
				return accessor2;
			}
			if (!(accessor.Mapping is MembersMapping) && !(accessor2.Mapping is MembersMapping) && (accessor.Mapping.TypeDesc == accessor2.Mapping.TypeDesc || (accessor2.Mapping is NullableMapping && accessor.Mapping.TypeDesc == ((NullableMapping)accessor2.Mapping).BaseMapping.TypeDesc) || (accessor.Mapping is NullableMapping && ((NullableMapping)accessor.Mapping).BaseMapping.TypeDesc == accessor2.Mapping.TypeDesc)))
			{
				string text = Convert.ToString(accessor.Default, CultureInfo.InvariantCulture);
				string text2 = Convert.ToString(accessor2.Default, CultureInfo.InvariantCulture);
				if (text == text2)
				{
					return accessor2;
				}
				throw new InvalidOperationException(Res.GetString("XmlCannotReconcileAccessorDefault", new object[] { accessor.Name, accessor.Namespace, text, text2 }));
			}
			else
			{
				if (accessor.Mapping is MembersMapping || accessor2.Mapping is MembersMapping)
				{
					throw new InvalidOperationException(Res.GetString("XmlMethodTypeNameConflict", new object[] { accessor.Name, accessor.Namespace }));
				}
				if (accessor.Mapping is ArrayMapping)
				{
					if (!(accessor2.Mapping is ArrayMapping))
					{
						throw new InvalidOperationException(Res.GetString("XmlCannotReconcileAccessor", new object[]
						{
							accessor.Name,
							accessor.Namespace,
							XmlReflectionImporter.GetMappingName(accessor2.Mapping),
							XmlReflectionImporter.GetMappingName(accessor.Mapping)
						}));
					}
					ArrayMapping arrayMapping = (ArrayMapping)accessor.Mapping;
					ArrayMapping arrayMapping2 = (arrayMapping.IsAnonymousType ? null : ((ArrayMapping)this.types[accessor2.Mapping.TypeName, accessor2.Mapping.Namespace]));
					ArrayMapping arrayMapping3 = arrayMapping2;
					while (arrayMapping2 != null)
					{
						if (arrayMapping2 == accessor.Mapping)
						{
							return accessor2;
						}
						arrayMapping2 = arrayMapping2.Next;
					}
					arrayMapping.Next = arrayMapping3;
					if (!arrayMapping.IsAnonymousType)
					{
						this.types[accessor2.Mapping.TypeName, accessor2.Mapping.Namespace] = arrayMapping;
					}
					return accessor2;
				}
				else
				{
					if (accessor is AttributeAccessor)
					{
						throw new InvalidOperationException(Res.GetString("XmlCannotReconcileAttributeAccessor", new object[]
						{
							accessor.Name,
							accessor.Namespace,
							XmlReflectionImporter.GetMappingName(accessor2.Mapping),
							XmlReflectionImporter.GetMappingName(accessor.Mapping)
						}));
					}
					throw new InvalidOperationException(Res.GetString("XmlCannotReconcileAccessor", new object[]
					{
						accessor.Name,
						accessor.Namespace,
						XmlReflectionImporter.GetMappingName(accessor2.Mapping),
						XmlReflectionImporter.GetMappingName(accessor.Mapping)
					}));
				}
			}
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000AEB84 File Offset: 0x000ADB84
		private Exception CreateReflectionException(string context, Exception e)
		{
			return new InvalidOperationException(Res.GetString("XmlReflectionError", new object[] { context }), e);
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x000AEBB0 File Offset: 0x000ADBB0
		private Exception CreateTypeReflectionException(string context, Exception e)
		{
			return new InvalidOperationException(Res.GetString("XmlTypeReflectionError", new object[] { context }), e);
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x000AEBDC File Offset: 0x000ADBDC
		private Exception CreateMemberReflectionException(FieldModel model, Exception e)
		{
			return new InvalidOperationException(Res.GetString(model.IsProperty ? "XmlPropertyReflectionError" : "XmlFieldReflectionError", new object[] { model.Name }), e);
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x000AEC1C File Offset: 0x000ADC1C
		private TypeMapping ImportTypeMapping(TypeModel model, string ns, XmlReflectionImporter.ImportContext context, string dataType, XmlAttributes a, RecursionLimiter limiter)
		{
			return this.ImportTypeMapping(model, ns, context, dataType, a, false, false, limiter);
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000AEC3C File Offset: 0x000ADC3C
		private TypeMapping ImportTypeMapping(TypeModel model, string ns, XmlReflectionImporter.ImportContext context, string dataType, XmlAttributes a, bool repeats, bool openModel, RecursionLimiter limiter)
		{
			TypeMapping typeMapping2;
			try
			{
				if (dataType.Length > 0)
				{
					TypeDesc typeDesc = (TypeScope.IsOptionalValue(model.Type) ? model.TypeDesc.BaseTypeDesc : model.TypeDesc);
					if (!typeDesc.IsPrimitive)
					{
						throw new InvalidOperationException(Res.GetString("XmlInvalidDataTypeUsage", new object[] { dataType, "XmlElementAttribute.DataType" }));
					}
					TypeDesc typeDesc2 = this.typeScope.GetTypeDesc(dataType, "http://www.w3.org/2001/XMLSchema");
					if (typeDesc2 == null)
					{
						throw new InvalidOperationException(Res.GetString("XmlInvalidXsdDataType", new object[]
						{
							dataType,
							"XmlElementAttribute.DataType",
							new XmlQualifiedName(dataType, "http://www.w3.org/2001/XMLSchema").ToString()
						}));
					}
					if (typeDesc.FullName != typeDesc2.FullName)
					{
						throw new InvalidOperationException(Res.GetString("XmlDataTypeMismatch", new object[] { dataType, "XmlElementAttribute.DataType", typeDesc.FullName }));
					}
				}
				if (a == null)
				{
					a = this.GetAttributes(model.Type, false);
				}
				if ((a.XmlFlags & (XmlAttributeFlags)(-193)) != (XmlAttributeFlags)0)
				{
					throw new InvalidOperationException(Res.GetString("XmlInvalidTypeAttributes", new object[] { model.Type.FullName }));
				}
				switch (model.TypeDesc.Kind)
				{
				case TypeKind.Root:
				case TypeKind.Struct:
				case TypeKind.Class:
					if (context != XmlReflectionImporter.ImportContext.Element)
					{
						throw XmlReflectionImporter.UnsupportedException(model.TypeDesc, context);
					}
					if (model.TypeDesc.IsOptionalValue)
					{
						TypeDesc typeDesc3 = (string.IsNullOrEmpty(dataType) ? model.TypeDesc.BaseTypeDesc : this.typeScope.GetTypeDesc(dataType, "http://www.w3.org/2001/XMLSchema"));
						string text = ((typeDesc3.DataType == null) ? typeDesc3.Name : typeDesc3.DataType.Name);
						TypeMapping typeMapping = this.GetTypeMapping(text, ns, typeDesc3, this.types, null);
						if (typeMapping == null)
						{
							typeMapping = this.ImportTypeMapping(this.modelScope.GetTypeModel(model.TypeDesc.BaseTypeDesc.Type), ns, context, dataType, null, repeats, openModel, limiter);
						}
						typeMapping2 = this.CreateNullableMapping(typeMapping, model.TypeDesc.Type);
					}
					else
					{
						typeMapping2 = this.ImportStructLikeMapping((StructModel)model, ns, openModel, a, limiter);
					}
					break;
				case TypeKind.Primitive:
					if (a.XmlFlags != (XmlAttributeFlags)0)
					{
						throw XmlReflectionImporter.InvalidAttributeUseException(model.Type);
					}
					typeMapping2 = this.ImportPrimitiveMapping((PrimitiveModel)model, context, dataType, repeats);
					break;
				case TypeKind.Enum:
					typeMapping2 = this.ImportEnumMapping((EnumModel)model, ns, repeats);
					break;
				case TypeKind.Array:
				case TypeKind.Collection:
				case TypeKind.Enumerable:
				{
					if (context != XmlReflectionImporter.ImportContext.Element)
					{
						throw XmlReflectionImporter.UnsupportedException(model.TypeDesc, context);
					}
					this.arrayNestingLevel++;
					ArrayMapping arrayMapping = this.ImportArrayLikeMapping((ArrayModel)model, ns, limiter);
					this.arrayNestingLevel--;
					typeMapping2 = arrayMapping;
					break;
				}
				default:
					if (model.TypeDesc.Kind == TypeKind.Serializable)
					{
						if ((a.XmlFlags & (XmlAttributeFlags)(-65)) != (XmlAttributeFlags)0)
						{
							throw new InvalidOperationException(Res.GetString("XmlSerializableAttributes", new object[]
							{
								model.TypeDesc.FullName,
								typeof(XmlSchemaProviderAttribute).Name
							}));
						}
					}
					else if (a.XmlFlags != (XmlAttributeFlags)0)
					{
						throw XmlReflectionImporter.InvalidAttributeUseException(model.Type);
					}
					if (!model.TypeDesc.IsSpecial)
					{
						throw XmlReflectionImporter.UnsupportedException(model.TypeDesc, context);
					}
					typeMapping2 = this.ImportSpecialMapping(model.Type, model.TypeDesc, ns, context, limiter);
					break;
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw this.CreateTypeReflectionException(model.TypeDesc.FullName, ex);
			}
			catch
			{
				throw this.CreateTypeReflectionException(model.TypeDesc.FullName, null);
			}
			return typeMapping2;
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x000AF04C File Offset: 0x000AE04C
		internal static MethodInfo GetMethodFromSchemaProvider(XmlSchemaProviderAttribute provider, Type type)
		{
			if (provider.IsAny)
			{
				return null;
			}
			if (provider.MethodName == null)
			{
				throw new ArgumentNullException("MethodName");
			}
			if (!CodeGenerator.IsValidLanguageIndependentIdentifier(provider.MethodName))
			{
				throw new ArgumentException(Res.GetString("XmlGetSchemaMethodName", new object[] { provider.MethodName }), "MethodName");
			}
			MethodInfo method = type.GetMethod(provider.MethodName, BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(XmlSchemaSet) }, null);
			if (method == null)
			{
				throw new InvalidOperationException(Res.GetString("XmlGetSchemaMethodMissing", new object[]
				{
					provider.MethodName,
					typeof(XmlSchemaSet).Name,
					type.FullName
				}));
			}
			if (!typeof(XmlQualifiedName).IsAssignableFrom(method.ReturnType) && !typeof(XmlSchemaType).IsAssignableFrom(method.ReturnType))
			{
				throw new InvalidOperationException(Res.GetString("XmlGetSchemaMethodReturnType", new object[]
				{
					type.Name,
					provider.MethodName,
					typeof(XmlSchemaProviderAttribute).Name,
					typeof(XmlQualifiedName).FullName,
					typeof(XmlSchemaType).FullName
				}));
			}
			return method;
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x000AF1A8 File Offset: 0x000AE1A8
		private SpecialMapping ImportSpecialMapping(Type type, TypeDesc typeDesc, string ns, XmlReflectionImporter.ImportContext context, RecursionLimiter limiter)
		{
			if (this.specials == null)
			{
				this.specials = new Hashtable();
			}
			SpecialMapping specialMapping = (SpecialMapping)this.specials[type];
			if (specialMapping != null)
			{
				this.CheckContext(specialMapping.TypeDesc, context);
				return specialMapping;
			}
			if (typeDesc.Kind == TypeKind.Serializable)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(XmlSchemaProviderAttribute), false);
				SerializableMapping serializableMapping;
				if (customAttributes.Length > 0)
				{
					XmlSchemaProviderAttribute xmlSchemaProviderAttribute = (XmlSchemaProviderAttribute)customAttributes[0];
					MethodInfo methodFromSchemaProvider = XmlReflectionImporter.GetMethodFromSchemaProvider(xmlSchemaProviderAttribute, type);
					serializableMapping = new SerializableMapping(methodFromSchemaProvider, xmlSchemaProviderAttribute.IsAny, ns);
					XmlQualifiedName xsiType = serializableMapping.XsiType;
					if (xsiType != null && !xsiType.IsEmpty)
					{
						if (this.serializables == null)
						{
							this.serializables = new NameTable();
						}
						SerializableMapping serializableMapping2 = (SerializableMapping)this.serializables[xsiType];
						if (serializableMapping2 != null)
						{
							if (serializableMapping2.Type == null)
							{
								serializableMapping = serializableMapping2;
							}
							else if (serializableMapping2.Type != type)
							{
								SerializableMapping next = serializableMapping2.Next;
								serializableMapping2.Next = serializableMapping;
								serializableMapping.Next = next;
							}
						}
						else
						{
							XmlSchemaType xsdType = serializableMapping.XsdType;
							if (xsdType != null)
							{
								this.SetBase(serializableMapping, xsdType.DerivedFrom);
							}
							this.serializables[xsiType] = serializableMapping;
						}
						serializableMapping.TypeName = xsiType.Name;
						serializableMapping.Namespace = xsiType.Namespace;
					}
					serializableMapping.TypeDesc = typeDesc;
					serializableMapping.Type = type;
					this.IncludeTypes(type);
				}
				else
				{
					serializableMapping = new SerializableMapping();
					serializableMapping.TypeDesc = typeDesc;
					serializableMapping.Type = type;
				}
				specialMapping = serializableMapping;
			}
			else
			{
				specialMapping = new SpecialMapping();
				specialMapping.TypeDesc = typeDesc;
			}
			this.CheckContext(typeDesc, context);
			this.specials.Add(type, specialMapping);
			this.typeScope.AddTypeMapping(specialMapping);
			return specialMapping;
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x000AF358 File Offset: 0x000AE358
		internal static void ValidationCallbackWithErrorCode(object sender, ValidationEventArgs args)
		{
			if (args.Severity == XmlSeverityType.Error)
			{
				throw new InvalidOperationException(Res.GetString("XmlSerializableSchemaError", new object[]
				{
					typeof(IXmlSerializable).Name,
					args.Message
				}));
			}
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000AF3A0 File Offset: 0x000AE3A0
		internal void SetBase(SerializableMapping mapping, XmlQualifiedName baseQname)
		{
			if (baseQname.IsEmpty)
			{
				return;
			}
			if (baseQname.Namespace == "http://www.w3.org/2001/XMLSchema")
			{
				return;
			}
			XmlSchemaSet schemas = mapping.Schemas;
			ArrayList arrayList = (ArrayList)schemas.Schemas(baseQname.Namespace);
			if (arrayList.Count == 0)
			{
				throw new InvalidOperationException(Res.GetString("XmlMissingSchema", new object[] { baseQname.Namespace }));
			}
			if (arrayList.Count > 1)
			{
				throw new InvalidOperationException(Res.GetString("XmlGetSchemaInclude", new object[]
				{
					baseQname.Namespace,
					typeof(IXmlSerializable).Name,
					"GetSchema"
				}));
			}
			XmlSchema xmlSchema = (XmlSchema)arrayList[0];
			XmlSchemaType xmlSchemaType = (XmlSchemaType)xmlSchema.SchemaTypes[baseQname];
			xmlSchemaType = ((xmlSchemaType.Redefined != null) ? xmlSchemaType.Redefined : xmlSchemaType);
			if (this.serializables[baseQname] == null)
			{
				SerializableMapping serializableMapping = new SerializableMapping(baseQname, schemas);
				this.SetBase(serializableMapping, xmlSchemaType.DerivedFrom);
				this.serializables.Add(baseQname, serializableMapping);
			}
			mapping.SetBaseMapping((SerializableMapping)this.serializables[baseQname]);
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000AF4D4 File Offset: 0x000AE4D4
		private static string GetContextName(XmlReflectionImporter.ImportContext context)
		{
			switch (context)
			{
			case XmlReflectionImporter.ImportContext.Text:
				return "text";
			case XmlReflectionImporter.ImportContext.Attribute:
				return "attribute";
			case XmlReflectionImporter.ImportContext.Element:
				return "element";
			default:
				throw new ArgumentException(Res.GetString("XmlInternalError"), "context");
			}
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000AF520 File Offset: 0x000AE520
		private static Exception InvalidAttributeUseException(Type type)
		{
			return new InvalidOperationException(Res.GetString("XmlInvalidAttributeUse", new object[] { type.FullName }));
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000AF550 File Offset: 0x000AE550
		private static Exception UnsupportedException(TypeDesc typeDesc, XmlReflectionImporter.ImportContext context)
		{
			return new InvalidOperationException(Res.GetString("XmlIllegalTypeContext", new object[]
			{
				typeDesc.FullName,
				XmlReflectionImporter.GetContextName(context)
			}));
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000AF588 File Offset: 0x000AE588
		private StructMapping CreateRootMapping()
		{
			TypeDesc typeDesc = this.typeScope.GetTypeDesc(typeof(object));
			return new StructMapping
			{
				TypeDesc = typeDesc,
				TypeName = "anyType",
				Namespace = "http://www.w3.org/2001/XMLSchema",
				Members = new MemberMapping[0],
				IncludeInSchema = false
			};
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000AF5E4 File Offset: 0x000AE5E4
		private NullableMapping CreateNullableMapping(TypeMapping baseMapping, Type type)
		{
			TypeDesc nullableTypeDesc = baseMapping.TypeDesc.GetNullableTypeDesc(type);
			TypeMapping typeMapping;
			if (!baseMapping.IsAnonymousType)
			{
				typeMapping = (TypeMapping)this.nullables[baseMapping.TypeName, baseMapping.Namespace];
			}
			else
			{
				typeMapping = (TypeMapping)this.anonymous[type];
			}
			NullableMapping nullableMapping;
			if (typeMapping == null)
			{
				nullableMapping = new NullableMapping();
				nullableMapping.BaseMapping = baseMapping;
				nullableMapping.TypeDesc = nullableTypeDesc;
				nullableMapping.TypeName = baseMapping.TypeName;
				nullableMapping.Namespace = baseMapping.Namespace;
				nullableMapping.IncludeInSchema = baseMapping.IncludeInSchema;
				if (!baseMapping.IsAnonymousType)
				{
					this.nullables.Add(baseMapping.TypeName, baseMapping.Namespace, nullableMapping);
				}
				else
				{
					this.anonymous[type] = nullableMapping;
				}
				this.typeScope.AddTypeMapping(nullableMapping);
				return nullableMapping;
			}
			if (!(typeMapping is NullableMapping))
			{
				throw new InvalidOperationException(Res.GetString("XmlTypesDuplicate", new object[]
				{
					nullableTypeDesc.FullName,
					typeMapping.TypeDesc.FullName,
					nullableTypeDesc.Name,
					typeMapping.Namespace
				}));
			}
			nullableMapping = (NullableMapping)typeMapping;
			if (nullableMapping.BaseMapping is PrimitiveMapping && baseMapping is PrimitiveMapping)
			{
				return nullableMapping;
			}
			if (nullableMapping.BaseMapping == baseMapping)
			{
				return nullableMapping;
			}
			throw new InvalidOperationException(Res.GetString("XmlTypesDuplicate", new object[]
			{
				nullableTypeDesc.FullName,
				typeMapping.TypeDesc.FullName,
				nullableTypeDesc.Name,
				typeMapping.Namespace
			}));
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000AF76A File Offset: 0x000AE76A
		private StructMapping GetRootMapping()
		{
			if (this.root == null)
			{
				this.root = this.CreateRootMapping();
				this.typeScope.AddTypeMapping(this.root);
			}
			return this.root;
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000AF798 File Offset: 0x000AE798
		private TypeMapping GetTypeMapping(string typeName, string ns, TypeDesc typeDesc, NameTable typeLib, Type type)
		{
			TypeMapping typeMapping;
			if (typeName == null || typeName.Length == 0)
			{
				typeMapping = ((type == null) ? null : ((TypeMapping)this.anonymous[type]));
			}
			else
			{
				typeMapping = (TypeMapping)typeLib[typeName, ns];
			}
			if (typeMapping == null)
			{
				return null;
			}
			if (!typeMapping.IsAnonymousType && typeMapping.TypeDesc != typeDesc)
			{
				throw new InvalidOperationException(Res.GetString("XmlTypesDuplicate", new object[]
				{
					typeDesc.FullName,
					typeMapping.TypeDesc.FullName,
					typeName,
					ns
				}));
			}
			return typeMapping;
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000AF82C File Offset: 0x000AE82C
		private StructMapping ImportStructLikeMapping(StructModel model, string ns, bool openModel, XmlAttributes a, RecursionLimiter limiter)
		{
			if (model.TypeDesc.Kind == TypeKind.Root)
			{
				return this.GetRootMapping();
			}
			if (a == null)
			{
				a = this.GetAttributes(model.Type, false);
			}
			string text = ns;
			if (a.XmlType != null && a.XmlType.Namespace != null)
			{
				text = a.XmlType.Namespace;
			}
			else if (a.XmlRoot != null && a.XmlRoot.Namespace != null)
			{
				text = a.XmlRoot.Namespace;
			}
			string text2 = (XmlReflectionImporter.IsAnonymousType(a, ns) ? null : this.XsdTypeName(model.Type, a, model.TypeDesc.Name));
			text2 = XmlConvert.EncodeLocalName(text2);
			StructMapping structMapping = (StructMapping)this.GetTypeMapping(text2, text, model.TypeDesc, this.types, model.Type);
			if (structMapping == null)
			{
				structMapping = new StructMapping();
				structMapping.TypeDesc = model.TypeDesc;
				structMapping.Namespace = text;
				structMapping.TypeName = text2;
				if (!structMapping.IsAnonymousType)
				{
					this.types.Add(text2, text, structMapping);
				}
				else
				{
					this.anonymous[model.Type] = structMapping;
				}
				if (a.XmlType != null)
				{
					structMapping.IncludeInSchema = a.XmlType.IncludeInSchema;
				}
				if (limiter.IsExceededLimit)
				{
					limiter.DeferredWorkItems.Add(new ImportStructWorkItem(model, structMapping));
					return structMapping;
				}
				limiter.Depth++;
				this.InitializeStructMembers(structMapping, model, openModel, text2, limiter);
				while (limiter.DeferredWorkItems.Count > 0)
				{
					int num = limiter.DeferredWorkItems.Count - 1;
					ImportStructWorkItem importStructWorkItem = limiter.DeferredWorkItems[num];
					if (this.InitializeStructMembers(importStructWorkItem.Mapping, importStructWorkItem.Model, openModel, text2, limiter))
					{
						limiter.DeferredWorkItems.RemoveAt(num);
					}
				}
				limiter.Depth--;
			}
			return structMapping;
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000AFA08 File Offset: 0x000AEA08
		private bool InitializeStructMembers(StructMapping mapping, StructModel model, bool openModel, string typeName, RecursionLimiter limiter)
		{
			if (mapping.IsFullyInitialized)
			{
				return true;
			}
			if (model.TypeDesc.BaseTypeDesc != null)
			{
				TypeModel typeModel = this.modelScope.GetTypeModel(model.Type.BaseType, false);
				if (!(typeModel is StructModel))
				{
					throw new NotSupportedException(Res.GetString("XmlUnsupportedInheritance", new object[] { model.Type.BaseType.FullName }));
				}
				StructMapping structMapping = this.ImportStructLikeMapping((StructModel)typeModel, mapping.Namespace, openModel, null, limiter);
				int num = limiter.DeferredWorkItems.IndexOf(structMapping);
				if (num < 0)
				{
					mapping.BaseMapping = structMapping;
					ICollection collection = mapping.BaseMapping.LocalAttributes.Values;
					foreach (object obj in collection)
					{
						AttributeAccessor attributeAccessor = (AttributeAccessor)obj;
						XmlReflectionImporter.AddUniqueAccessor(mapping.LocalAttributes, attributeAccessor);
					}
					if (mapping.BaseMapping.HasExplicitSequence())
					{
						goto IL_01CD;
					}
					collection = mapping.BaseMapping.LocalElements.Values;
					using (IEnumerator enumerator2 = collection.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							ElementAccessor elementAccessor = (ElementAccessor)obj2;
							XmlReflectionImporter.AddUniqueAccessor(mapping.LocalElements, elementAccessor);
						}
						goto IL_01CD;
					}
				}
				if (!limiter.DeferredWorkItems.Contains(mapping))
				{
					limiter.DeferredWorkItems.Add(new ImportStructWorkItem(model, mapping));
				}
				int num2 = limiter.DeferredWorkItems.Count - 1;
				if (num < num2)
				{
					ImportStructWorkItem importStructWorkItem = limiter.DeferredWorkItems[num];
					limiter.DeferredWorkItems[num] = limiter.DeferredWorkItems[num2];
					limiter.DeferredWorkItems[num2] = importStructWorkItem;
				}
				return false;
			}
			IL_01CD:
			ArrayList arrayList = new ArrayList();
			TextAccessor textAccessor = null;
			bool flag = false;
			bool flag2 = false;
			foreach (MemberInfo memberInfo in model.GetMemberInfos())
			{
				if ((memberInfo.MemberType & (MemberTypes.Field | MemberTypes.Property)) != (MemberTypes)0)
				{
					XmlAttributes attributes = this.GetAttributes(memberInfo);
					if (!attributes.XmlIgnore)
					{
						FieldModel fieldModel = model.GetFieldModel(memberInfo);
						if (fieldModel != null)
						{
							try
							{
								MemberMapping memberMapping = this.ImportFieldMapping(model, fieldModel, attributes, mapping.Namespace, limiter);
								if (memberMapping != null)
								{
									if (mapping.BaseMapping == null || !mapping.BaseMapping.Declares(memberMapping, mapping.TypeName))
									{
										flag2 |= memberMapping.IsSequence;
										XmlReflectionImporter.AddUniqueAccessor(memberMapping, mapping.LocalElements, mapping.LocalAttributes, flag2);
										if (memberMapping.Text != null)
										{
											if (!memberMapping.Text.Mapping.TypeDesc.CanBeTextValue && memberMapping.Text.Mapping.IsList)
											{
												throw new InvalidOperationException(Res.GetString("XmlIllegalTypedTextAttribute", new object[]
												{
													typeName,
													memberMapping.Text.Name,
													memberMapping.Text.Mapping.TypeDesc.FullName
												}));
											}
											if (textAccessor != null)
											{
												throw new InvalidOperationException(Res.GetString("XmlIllegalMultipleText", new object[] { model.Type.FullName }));
											}
											textAccessor = memberMapping.Text;
										}
										if (memberMapping.Xmlns != null)
										{
											if (mapping.XmlnsMember != null)
											{
												throw new InvalidOperationException(Res.GetString("XmlMultipleXmlns", new object[] { model.Type.FullName }));
											}
											mapping.XmlnsMember = memberMapping;
										}
										if (memberMapping.Elements != null && memberMapping.Elements.Length != 0)
										{
											flag = true;
										}
										arrayList.Add(memberMapping);
									}
								}
							}
							catch (Exception ex)
							{
								if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
								{
									throw;
								}
								throw this.CreateMemberReflectionException(fieldModel, ex);
							}
							catch
							{
								throw this.CreateMemberReflectionException(fieldModel, null);
							}
						}
					}
				}
			}
			mapping.SetContentModel(textAccessor, flag);
			if (flag2)
			{
				Hashtable hashtable = new Hashtable();
				for (int j = 0; j < arrayList.Count; j++)
				{
					MemberMapping memberMapping2 = (MemberMapping)arrayList[j];
					if (memberMapping2.IsParticle)
					{
						if (!memberMapping2.IsSequence)
						{
							throw new InvalidOperationException(Res.GetString("XmlSequenceInconsistent", new object[] { "Order", memberMapping2.Name }));
						}
						if (hashtable[memberMapping2.SequenceId] != null)
						{
							throw new InvalidOperationException(Res.GetString("XmlSequenceUnique", new object[]
							{
								memberMapping2.SequenceId.ToString(CultureInfo.InvariantCulture),
								"Order",
								memberMapping2.Name
							}));
						}
						hashtable[memberMapping2.SequenceId] = memberMapping2;
					}
				}
				arrayList.Sort(new MemberMappingComparer());
			}
			mapping.Members = (MemberMapping[])arrayList.ToArray(typeof(MemberMapping));
			if (mapping.BaseMapping == null)
			{
				mapping.BaseMapping = this.GetRootMapping();
			}
			if (mapping.XmlnsMember != null && mapping.BaseMapping.HasXmlnsMember)
			{
				throw new InvalidOperationException(Res.GetString("XmlMultipleXmlns", new object[] { model.Type.FullName }));
			}
			this.IncludeTypes(model.Type, limiter);
			this.typeScope.AddTypeMapping(mapping);
			if (openModel)
			{
				mapping.IsOpenModel = true;
			}
			return true;
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000B0000 File Offset: 0x000AF000
		private static bool IsAnonymousType(XmlAttributes a, string contextNs)
		{
			if (a.XmlType != null && a.XmlType.AnonymousType)
			{
				string @namespace = a.XmlType.Namespace;
				return string.IsNullOrEmpty(@namespace) || @namespace == contextNs;
			}
			return false;
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x000B0044 File Offset: 0x000AF044
		internal string XsdTypeName(Type type)
		{
			if (type == typeof(object))
			{
				return "anyType";
			}
			TypeDesc typeDesc = this.typeScope.GetTypeDesc(type);
			if (typeDesc.IsPrimitive && typeDesc.DataType != null && typeDesc.DataType.Name != null && typeDesc.DataType.Name.Length > 0)
			{
				return typeDesc.DataType.Name;
			}
			return this.XsdTypeName(type, this.GetAttributes(type, false), typeDesc.Name);
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000B00C4 File Offset: 0x000AF0C4
		internal string XsdTypeName(Type type, XmlAttributes a, string name)
		{
			string text = name;
			if (a.XmlType != null && a.XmlType.TypeName.Length > 0)
			{
				text = a.XmlType.TypeName;
			}
			if (type.IsGenericType && text.IndexOf('{') >= 0)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				Type[] genericArguments = genericTypeDefinition.GetGenericArguments();
				Type[] genericArguments2 = type.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					string text2 = "{" + genericArguments[i] + "}";
					if (text.Contains(text2))
					{
						text = text.Replace(text2, this.XsdTypeName(genericArguments2[i]));
						if (text.IndexOf('{') < 0)
						{
							break;
						}
					}
				}
			}
			return text;
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x000B0174 File Offset: 0x000AF174
		private static int CountAtLevel(XmlArrayItemAttributes attributes, int level)
		{
			int num = 0;
			for (int i = 0; i < attributes.Count; i++)
			{
				if (attributes[i].NestingLevel == level)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000B01A8 File Offset: 0x000AF1A8
		private void SetArrayMappingType(ArrayMapping mapping, string defaultNs, Type type)
		{
			XmlAttributes attributes = this.GetAttributes(type, false);
			bool flag = XmlReflectionImporter.IsAnonymousType(attributes, defaultNs);
			if (flag)
			{
				mapping.TypeName = null;
				mapping.Namespace = defaultNs;
				return;
			}
			ElementAccessor elementAccessor = null;
			TypeMapping typeMapping;
			if (mapping.Elements.Length == 1)
			{
				elementAccessor = mapping.Elements[0];
				typeMapping = elementAccessor.Mapping;
			}
			else
			{
				typeMapping = null;
			}
			bool flag2 = true;
			string text;
			string text2;
			if (attributes.XmlType != null)
			{
				text = attributes.XmlType.Namespace;
				text2 = this.XsdTypeName(type, attributes, attributes.XmlType.TypeName);
				text2 = XmlConvert.EncodeLocalName(text2);
				flag2 = text2 == null;
			}
			else if (typeMapping is EnumMapping)
			{
				text = typeMapping.Namespace;
				text2 = typeMapping.DefaultElementName;
			}
			else if (typeMapping is PrimitiveMapping)
			{
				text = defaultNs;
				text2 = typeMapping.TypeDesc.DataType.Name;
			}
			else if (typeMapping is StructMapping && typeMapping.TypeDesc.IsRoot)
			{
				text = defaultNs;
				text2 = "anyType";
			}
			else if (typeMapping != null)
			{
				text = ((typeMapping.Namespace == "http://www.w3.org/2001/XMLSchema") ? defaultNs : typeMapping.Namespace);
				text2 = typeMapping.DefaultElementName;
			}
			else
			{
				text = defaultNs;
				text2 = "Choice" + this.choiceNum++;
			}
			if (text2 == null)
			{
				text2 = "Any";
			}
			if (elementAccessor != null)
			{
				text = elementAccessor.Namespace;
			}
			if (text == null)
			{
				text = defaultNs;
			}
			string text3;
			text2 = (text3 = (flag2 ? ("ArrayOf" + CodeIdentifier.MakePascal(text2)) : text2));
			int num = 1;
			TypeMapping typeMapping2 = (TypeMapping)this.types[text3, text];
			while (typeMapping2 != null)
			{
				if (typeMapping2 is ArrayMapping)
				{
					ArrayMapping arrayMapping = (ArrayMapping)typeMapping2;
					if (AccessorMapping.ElementsMatch(arrayMapping.Elements, mapping.Elements))
					{
						break;
					}
				}
				text3 = text2 + num.ToString(CultureInfo.InvariantCulture);
				typeMapping2 = (TypeMapping)this.types[text3, text];
				num++;
			}
			mapping.TypeName = text3;
			mapping.Namespace = text;
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000B03A4 File Offset: 0x000AF3A4
		private ArrayMapping ImportArrayLikeMapping(ArrayModel model, string ns, RecursionLimiter limiter)
		{
			ArrayMapping arrayMapping = new ArrayMapping();
			arrayMapping.TypeDesc = model.TypeDesc;
			if (this.savedArrayItemAttributes == null)
			{
				this.savedArrayItemAttributes = new XmlArrayItemAttributes();
			}
			if (XmlReflectionImporter.CountAtLevel(this.savedArrayItemAttributes, this.arrayNestingLevel) == 0)
			{
				this.savedArrayItemAttributes.Add(XmlReflectionImporter.CreateArrayItemAttribute(this.typeScope.GetTypeDesc(model.Element.Type), this.arrayNestingLevel));
			}
			this.CreateArrayElementsFromAttributes(arrayMapping, this.savedArrayItemAttributes, model.Element.Type, (this.savedArrayNamespace == null) ? ns : this.savedArrayNamespace, limiter);
			this.SetArrayMappingType(arrayMapping, ns, model.Type);
			for (int i = 0; i < arrayMapping.Elements.Length; i++)
			{
				arrayMapping.Elements[i] = this.ReconcileLocalAccessor(arrayMapping.Elements[i], arrayMapping.Namespace);
			}
			this.IncludeTypes(model.Type);
			ArrayMapping arrayMapping2 = (ArrayMapping)this.types[arrayMapping.TypeName, arrayMapping.Namespace];
			if (arrayMapping2 != null)
			{
				ArrayMapping arrayMapping3 = arrayMapping2;
				while (arrayMapping2 != null)
				{
					if (arrayMapping2.TypeDesc == model.TypeDesc)
					{
						return arrayMapping2;
					}
					arrayMapping2 = arrayMapping2.Next;
				}
				arrayMapping.Next = arrayMapping3;
				if (!arrayMapping.IsAnonymousType)
				{
					this.types[arrayMapping.TypeName, arrayMapping.Namespace] = arrayMapping;
				}
				else
				{
					this.anonymous[model.Type] = arrayMapping;
				}
				return arrayMapping;
			}
			this.typeScope.AddTypeMapping(arrayMapping);
			if (!arrayMapping.IsAnonymousType)
			{
				this.types.Add(arrayMapping.TypeName, arrayMapping.Namespace, arrayMapping);
			}
			else
			{
				this.anonymous[model.Type] = arrayMapping;
			}
			return arrayMapping;
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000B0548 File Offset: 0x000AF548
		private void CheckContext(TypeDesc typeDesc, XmlReflectionImporter.ImportContext context)
		{
			switch (context)
			{
			case XmlReflectionImporter.ImportContext.Text:
				if (typeDesc.CanBeTextValue || typeDesc.IsEnum || typeDesc.IsPrimitive)
				{
					return;
				}
				break;
			case XmlReflectionImporter.ImportContext.Attribute:
				if (typeDesc.CanBeAttributeValue)
				{
					return;
				}
				break;
			case XmlReflectionImporter.ImportContext.Element:
				if (typeDesc.CanBeElementValue)
				{
					return;
				}
				break;
			default:
				throw new ArgumentException(Res.GetString("XmlInternalError"), "context");
			}
			throw XmlReflectionImporter.UnsupportedException(typeDesc, context);
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000B05B4 File Offset: 0x000AF5B4
		private PrimitiveMapping ImportPrimitiveMapping(PrimitiveModel model, XmlReflectionImporter.ImportContext context, string dataType, bool repeats)
		{
			PrimitiveMapping primitiveMapping = new PrimitiveMapping();
			if (dataType.Length > 0)
			{
				primitiveMapping.TypeDesc = this.typeScope.GetTypeDesc(dataType, "http://www.w3.org/2001/XMLSchema");
				if (primitiveMapping.TypeDesc == null)
				{
					primitiveMapping.TypeDesc = this.typeScope.GetTypeDesc(dataType, "http://microsoft.com/wsdl/types/");
					if (primitiveMapping.TypeDesc == null)
					{
						throw new InvalidOperationException(Res.GetString("XmlUdeclaredXsdType", new object[] { dataType }));
					}
				}
			}
			else
			{
				primitiveMapping.TypeDesc = model.TypeDesc;
			}
			primitiveMapping.TypeName = primitiveMapping.TypeDesc.DataType.Name;
			primitiveMapping.Namespace = (primitiveMapping.TypeDesc.IsXsdType ? "http://www.w3.org/2001/XMLSchema" : "http://microsoft.com/wsdl/types/");
			primitiveMapping.IsList = repeats;
			this.CheckContext(primitiveMapping.TypeDesc, context);
			return primitiveMapping;
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000B0684 File Offset: 0x000AF684
		private EnumMapping ImportEnumMapping(EnumModel model, string ns, bool repeats)
		{
			XmlAttributes attributes = this.GetAttributes(model.Type, false);
			string text = ns;
			if (attributes.XmlType != null && attributes.XmlType.Namespace != null)
			{
				text = attributes.XmlType.Namespace;
			}
			string text2 = (XmlReflectionImporter.IsAnonymousType(attributes, ns) ? null : this.XsdTypeName(model.Type, attributes, model.TypeDesc.Name));
			text2 = XmlConvert.EncodeLocalName(text2);
			EnumMapping enumMapping = (EnumMapping)this.GetTypeMapping(text2, text, model.TypeDesc, this.types, model.Type);
			if (enumMapping == null)
			{
				enumMapping = new EnumMapping();
				enumMapping.TypeDesc = model.TypeDesc;
				enumMapping.TypeName = text2;
				enumMapping.Namespace = text;
				enumMapping.IsFlags = model.Type.IsDefined(typeof(FlagsAttribute), false);
				if (enumMapping.IsFlags && repeats)
				{
					throw new InvalidOperationException(Res.GetString("XmlIllegalAttributeFlagsArray", new object[] { model.TypeDesc.FullName }));
				}
				enumMapping.IsList = repeats;
				enumMapping.IncludeInSchema = attributes.XmlType == null || attributes.XmlType.IncludeInSchema;
				if (!enumMapping.IsAnonymousType)
				{
					this.types.Add(text2, text, enumMapping);
				}
				else
				{
					this.anonymous[model.Type] = enumMapping;
				}
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < model.Constants.Length; i++)
				{
					ConstantMapping constantMapping = this.ImportConstantMapping(model.Constants[i]);
					if (constantMapping != null)
					{
						arrayList.Add(constantMapping);
					}
				}
				if (arrayList.Count == 0)
				{
					throw new InvalidOperationException(Res.GetString("XmlNoSerializableMembers", new object[] { model.TypeDesc.FullName }));
				}
				enumMapping.Constants = (ConstantMapping[])arrayList.ToArray(typeof(ConstantMapping));
				this.typeScope.AddTypeMapping(enumMapping);
			}
			return enumMapping;
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000B086C File Offset: 0x000AF86C
		private ConstantMapping ImportConstantMapping(ConstantModel model)
		{
			XmlAttributes attributes = this.GetAttributes(model.FieldInfo);
			if (attributes.XmlIgnore)
			{
				return null;
			}
			if ((attributes.XmlFlags & (XmlAttributeFlags)(-2)) != (XmlAttributeFlags)0)
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidConstantAttribute"));
			}
			if (attributes.XmlEnum == null)
			{
				attributes.XmlEnum = new XmlEnumAttribute();
			}
			return new ConstantMapping
			{
				XmlName = ((attributes.XmlEnum.Name == null) ? model.Name : attributes.XmlEnum.Name),
				Name = model.Name,
				Value = model.Value
			};
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000B0904 File Offset: 0x000AF904
		private MembersMapping ImportMembersMapping(XmlReflectionMember[] xmlReflectionMembers, string ns, bool hasWrapperElement, bool rpc, bool openModel, RecursionLimiter limiter)
		{
			MembersMapping membersMapping = new MembersMapping();
			membersMapping.TypeDesc = this.typeScope.GetTypeDesc(typeof(object[]));
			MemberMapping[] array = new MemberMapping[xmlReflectionMembers.Length];
			NameTable nameTable = new NameTable();
			NameTable nameTable2 = new NameTable();
			TextAccessor textAccessor = null;
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					MemberMapping memberMapping = this.ImportMemberMapping(xmlReflectionMembers[i], ns, xmlReflectionMembers, rpc, openModel, limiter);
					if (!hasWrapperElement && memberMapping.Attribute != null)
					{
						if (rpc)
						{
							throw new InvalidOperationException(Res.GetString("XmlRpcLitAttributeAttributes"));
						}
						throw new InvalidOperationException(Res.GetString("XmlInvalidAttributeType", new object[] { "XmlAttribute" }));
					}
					else
					{
						if (rpc && xmlReflectionMembers[i].IsReturnValue)
						{
							if (i > 0)
							{
								throw new InvalidOperationException(Res.GetString("XmlInvalidReturnPosition"));
							}
							memberMapping.IsReturnValue = true;
						}
						array[i] = memberMapping;
						flag |= memberMapping.IsSequence;
						if (!xmlReflectionMembers[i].XmlAttributes.XmlIgnore)
						{
							XmlReflectionImporter.AddUniqueAccessor(memberMapping, nameTable, nameTable2, flag);
						}
						array[i] = memberMapping;
						if (memberMapping.Text != null)
						{
							if (textAccessor != null)
							{
								throw new InvalidOperationException(Res.GetString("XmlIllegalMultipleTextMembers"));
							}
							textAccessor = memberMapping.Text;
						}
						if (memberMapping.Xmlns != null)
						{
							if (membersMapping.XmlnsMember != null)
							{
								throw new InvalidOperationException(Res.GetString("XmlMultipleXmlnsMembers"));
							}
							membersMapping.XmlnsMember = memberMapping;
						}
					}
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					throw this.CreateReflectionException(xmlReflectionMembers[i].MemberName, ex);
				}
				catch
				{
					throw this.CreateReflectionException(xmlReflectionMembers[i].MemberName, null);
				}
			}
			if (flag)
			{
				throw new InvalidOperationException(Res.GetString("XmlSequenceMembers", new object[] { "Order" }));
			}
			membersMapping.Members = array;
			membersMapping.HasWrapperElement = hasWrapperElement;
			return membersMapping;
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000B0B1C File Offset: 0x000AFB1C
		private MemberMapping ImportMemberMapping(XmlReflectionMember xmlReflectionMember, string ns, XmlReflectionMember[] xmlReflectionMembers, bool rpc, bool openModel, RecursionLimiter limiter)
		{
			XmlSchemaForm xmlSchemaForm = (rpc ? XmlSchemaForm.Unqualified : XmlSchemaForm.Qualified);
			XmlAttributes xmlAttributes = xmlReflectionMember.XmlAttributes;
			TypeDesc typeDesc = this.typeScope.GetTypeDesc(xmlReflectionMember.MemberType);
			if (xmlAttributes.XmlFlags == (XmlAttributeFlags)0)
			{
				if (typeDesc.IsArrayLike)
				{
					XmlArrayAttribute xmlArrayAttribute = XmlReflectionImporter.CreateArrayAttribute(typeDesc);
					xmlArrayAttribute.ElementName = xmlReflectionMember.MemberName;
					xmlArrayAttribute.Namespace = (rpc ? null : ns);
					xmlArrayAttribute.Form = xmlSchemaForm;
					xmlAttributes.XmlArray = xmlArrayAttribute;
				}
				else
				{
					XmlElementAttribute xmlElementAttribute = XmlReflectionImporter.CreateElementAttribute(typeDesc);
					if (typeDesc.IsStructLike)
					{
						XmlAttributes xmlAttributes2 = new XmlAttributes(xmlReflectionMember.MemberType);
						if (xmlAttributes2.XmlRoot != null)
						{
							if (xmlAttributes2.XmlRoot.ElementName.Length > 0)
							{
								xmlElementAttribute.ElementName = xmlAttributes2.XmlRoot.ElementName;
							}
							if (rpc)
							{
								xmlElementAttribute.Namespace = null;
								if (xmlAttributes2.XmlRoot.IsNullableSpecified)
								{
									xmlElementAttribute.IsNullable = xmlAttributes2.XmlRoot.IsNullable;
								}
							}
							else
							{
								xmlElementAttribute.Namespace = xmlAttributes2.XmlRoot.Namespace;
								xmlElementAttribute.IsNullable = xmlAttributes2.XmlRoot.IsNullable;
							}
						}
					}
					if (xmlElementAttribute.ElementName.Length == 0)
					{
						xmlElementAttribute.ElementName = xmlReflectionMember.MemberName;
					}
					if (xmlElementAttribute.Namespace == null && !rpc)
					{
						xmlElementAttribute.Namespace = ns;
					}
					xmlElementAttribute.Form = xmlSchemaForm;
					xmlAttributes.XmlElements.Add(xmlElementAttribute);
				}
			}
			else if (xmlAttributes.XmlRoot != null)
			{
				XmlReflectionImporter.CheckNullable(xmlAttributes.XmlRoot.IsNullable, typeDesc, null);
			}
			MemberMapping memberMapping = new MemberMapping();
			memberMapping.Name = xmlReflectionMember.MemberName;
			bool flag = XmlReflectionImporter.FindSpecifiedMember(xmlReflectionMember.MemberName, xmlReflectionMembers) != null;
			FieldModel fieldModel = new FieldModel(xmlReflectionMember.MemberName, xmlReflectionMember.MemberType, this.typeScope.GetTypeDesc(xmlReflectionMember.MemberType), flag, false);
			memberMapping.CheckShouldPersist = fieldModel.CheckShouldPersist;
			memberMapping.CheckSpecified = fieldModel.CheckSpecified;
			memberMapping.ReadOnly = fieldModel.ReadOnly;
			Type type = null;
			if (xmlAttributes.XmlChoiceIdentifier != null)
			{
				type = this.GetChoiceIdentifierType(xmlAttributes.XmlChoiceIdentifier, xmlReflectionMembers, typeDesc.IsArrayLike, fieldModel.Name);
			}
			this.ImportAccessorMapping(memberMapping, fieldModel, xmlAttributes, ns, type, rpc, openModel, limiter);
			if (xmlReflectionMember.OverrideIsNullable && memberMapping.Elements.Length > 0)
			{
				memberMapping.Elements[0].IsNullable = false;
			}
			return memberMapping;
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000B0D74 File Offset: 0x000AFD74
		internal static XmlReflectionMember FindSpecifiedMember(string memberName, XmlReflectionMember[] reflectionMembers)
		{
			for (int i = 0; i < reflectionMembers.Length; i++)
			{
				if (string.Compare(reflectionMembers[i].MemberName, memberName + "Specified", StringComparison.Ordinal) == 0)
				{
					return reflectionMembers[i];
				}
			}
			return null;
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000B0DB0 File Offset: 0x000AFDB0
		private MemberMapping ImportFieldMapping(StructModel parent, FieldModel model, XmlAttributes a, string ns, RecursionLimiter limiter)
		{
			MemberMapping memberMapping = new MemberMapping();
			memberMapping.Name = model.Name;
			memberMapping.CheckShouldPersist = model.CheckShouldPersist;
			memberMapping.CheckSpecified = model.CheckSpecified;
			memberMapping.ReadOnly = model.ReadOnly;
			Type type = null;
			if (a.XmlChoiceIdentifier != null)
			{
				type = this.GetChoiceIdentifierType(a.XmlChoiceIdentifier, parent, model.FieldTypeDesc.IsArrayLike, model.Name);
			}
			this.ImportAccessorMapping(memberMapping, model, a, ns, type, false, false, limiter);
			return memberMapping;
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000B0E30 File Offset: 0x000AFE30
		private Type CheckChoiceIdentifierType(Type type, bool isArrayLike, string identifierName, string memberName)
		{
			if (type.IsArray)
			{
				if (!isArrayLike)
				{
					throw new InvalidOperationException(Res.GetString("XmlChoiceIdentifierType", new object[]
					{
						identifierName,
						memberName,
						type.GetElementType().FullName
					}));
				}
				type = type.GetElementType();
			}
			else if (isArrayLike)
			{
				throw new InvalidOperationException(Res.GetString("XmlChoiceIdentifierArrayType", new object[] { identifierName, memberName, type.FullName }));
			}
			if (!type.IsEnum)
			{
				throw new InvalidOperationException(Res.GetString("XmlChoiceIdentifierTypeEnum", new object[] { identifierName }));
			}
			return type;
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x000B0ED4 File Offset: 0x000AFED4
		private Type GetChoiceIdentifierType(XmlChoiceIdentifierAttribute choice, XmlReflectionMember[] xmlReflectionMembers, bool isArrayLike, string accessorName)
		{
			for (int i = 0; i < xmlReflectionMembers.Length; i++)
			{
				if (choice.MemberName == xmlReflectionMembers[i].MemberName)
				{
					return this.CheckChoiceIdentifierType(xmlReflectionMembers[i].MemberType, isArrayLike, choice.MemberName, accessorName);
				}
			}
			throw new InvalidOperationException(Res.GetString("XmlChoiceIdentiferMemberMissing", new object[] { choice.MemberName, accessorName }));
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000B0F44 File Offset: 0x000AFF44
		private Type GetChoiceIdentifierType(XmlChoiceIdentifierAttribute choice, StructModel structModel, bool isArrayLike, string accessorName)
		{
			MemberInfo[] array = structModel.Type.GetMember(choice.MemberName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			if (array == null || array.Length == 0)
			{
				PropertyInfo property = structModel.Type.GetProperty(choice.MemberName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
				if (property == null)
				{
					throw new InvalidOperationException(Res.GetString("XmlChoiceIdentiferMemberMissing", new object[] { choice.MemberName, accessorName }));
				}
				array = new MemberInfo[] { property };
			}
			else if (array.Length > 1)
			{
				throw new InvalidOperationException(Res.GetString("XmlChoiceIdentiferAmbiguous", new object[] { choice.MemberName }));
			}
			FieldModel fieldModel = structModel.GetFieldModel(array[0]);
			if (fieldModel == null)
			{
				throw new InvalidOperationException(Res.GetString("XmlChoiceIdentiferMemberMissing", new object[] { choice.MemberName, accessorName }));
			}
			Type fieldType = fieldModel.FieldType;
			return this.CheckChoiceIdentifierType(fieldType, isArrayLike, choice.MemberName, accessorName);
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x000B103C File Offset: 0x000B003C
		private void CreateArrayElementsFromAttributes(ArrayMapping arrayMapping, XmlArrayItemAttributes attributes, Type arrayElementType, string arrayElementNs, RecursionLimiter limiter)
		{
			NameTable nameTable = new NameTable();
			int num = 0;
			while (attributes != null && num < attributes.Count)
			{
				XmlArrayItemAttribute xmlArrayItemAttribute = attributes[num];
				if (xmlArrayItemAttribute.NestingLevel == this.arrayNestingLevel)
				{
					Type type = ((xmlArrayItemAttribute.Type != null) ? xmlArrayItemAttribute.Type : arrayElementType);
					TypeDesc typeDesc = this.typeScope.GetTypeDesc(type);
					ElementAccessor elementAccessor = new ElementAccessor();
					elementAccessor.Namespace = ((xmlArrayItemAttribute.Namespace == null) ? arrayElementNs : xmlArrayItemAttribute.Namespace);
					elementAccessor.Mapping = this.ImportTypeMapping(this.modelScope.GetTypeModel(type), elementAccessor.Namespace, XmlReflectionImporter.ImportContext.Element, xmlArrayItemAttribute.DataType, null, limiter);
					elementAccessor.Name = ((xmlArrayItemAttribute.ElementName.Length == 0) ? elementAccessor.Mapping.DefaultElementName : XmlConvert.EncodeLocalName(xmlArrayItemAttribute.ElementName));
					elementAccessor.IsNullable = (xmlArrayItemAttribute.IsNullableSpecified ? xmlArrayItemAttribute.IsNullable : (typeDesc.IsNullable || typeDesc.IsOptionalValue));
					elementAccessor.Form = ((xmlArrayItemAttribute.Form == XmlSchemaForm.None) ? XmlSchemaForm.Qualified : xmlArrayItemAttribute.Form);
					XmlReflectionImporter.CheckForm(elementAccessor.Form, arrayElementNs != elementAccessor.Namespace);
					XmlReflectionImporter.CheckNullable(elementAccessor.IsNullable, typeDesc, elementAccessor.Mapping);
					XmlReflectionImporter.AddUniqueAccessor(nameTable, elementAccessor);
				}
				num++;
			}
			arrayMapping.Elements = (ElementAccessor[])nameTable.ToArray(typeof(ElementAccessor));
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x000B11B0 File Offset: 0x000B01B0
		private void ImportAccessorMapping(MemberMapping accessor, FieldModel model, XmlAttributes a, string ns, Type choiceIdentifierType, bool rpc, bool openModel, RecursionLimiter limiter)
		{
			XmlSchemaForm xmlSchemaForm = XmlSchemaForm.Qualified;
			int num = this.arrayNestingLevel;
			int num2 = -1;
			XmlArrayItemAttributes xmlArrayItemAttributes = this.savedArrayItemAttributes;
			string text = this.savedArrayNamespace;
			this.arrayNestingLevel = 0;
			this.savedArrayItemAttributes = null;
			this.savedArrayNamespace = null;
			Type fieldType = model.FieldType;
			string name = model.Name;
			ArrayList arrayList = new ArrayList();
			NameTable nameTable = new NameTable();
			accessor.TypeDesc = this.typeScope.GetTypeDesc(fieldType);
			XmlAttributeFlags xmlFlags = a.XmlFlags;
			accessor.Ignore = a.XmlIgnore;
			if (rpc)
			{
				this.CheckTopLevelAttributes(a, name);
			}
			else
			{
				this.CheckAmbiguousChoice(a, fieldType, name);
			}
			XmlAttributeFlags xmlAttributeFlags = (XmlAttributeFlags)1300;
			XmlAttributeFlags xmlAttributeFlags2 = (XmlAttributeFlags)544;
			XmlAttributeFlags xmlAttributeFlags3 = (XmlAttributeFlags)10;
			if ((xmlFlags & xmlAttributeFlags3) != (XmlAttributeFlags)0 && fieldType == typeof(byte[]))
			{
				accessor.TypeDesc = this.typeScope.GetArrayTypeDesc(fieldType);
			}
			if (a.XmlChoiceIdentifier != null)
			{
				accessor.ChoiceIdentifier = new ChoiceIdentifierAccessor();
				accessor.ChoiceIdentifier.MemberName = a.XmlChoiceIdentifier.MemberName;
				accessor.ChoiceIdentifier.Mapping = this.ImportTypeMapping(this.modelScope.GetTypeModel(choiceIdentifierType), ns, XmlReflectionImporter.ImportContext.Element, string.Empty, null, limiter);
				this.CheckChoiceIdentifierMapping((EnumMapping)accessor.ChoiceIdentifier.Mapping);
			}
			if (accessor.TypeDesc.IsArrayLike)
			{
				Type arrayElementType = TypeScope.GetArrayElementType(fieldType, model.FieldTypeDesc.FullName + "." + model.Name);
				if ((xmlFlags & xmlAttributeFlags2) != (XmlAttributeFlags)0)
				{
					if ((xmlFlags & xmlAttributeFlags2) != xmlFlags)
					{
						throw new InvalidOperationException(Res.GetString("XmlIllegalAttributesArrayAttribute"));
					}
					if (a.XmlAttribute != null && !accessor.TypeDesc.ArrayElementTypeDesc.IsPrimitive && !accessor.TypeDesc.ArrayElementTypeDesc.IsEnum)
					{
						if (accessor.TypeDesc.ArrayElementTypeDesc.Kind == TypeKind.Serializable)
						{
							throw new InvalidOperationException(Res.GetString("XmlIllegalAttrOrTextInterface", new object[]
							{
								name,
								accessor.TypeDesc.ArrayElementTypeDesc.FullName,
								typeof(IXmlSerializable).Name
							}));
						}
						throw new InvalidOperationException(Res.GetString("XmlIllegalAttrOrText", new object[]
						{
							name,
							accessor.TypeDesc.ArrayElementTypeDesc.FullName
						}));
					}
					else
					{
						bool flag = a.XmlAttribute != null && (accessor.TypeDesc.ArrayElementTypeDesc.IsPrimitive || accessor.TypeDesc.ArrayElementTypeDesc.IsEnum);
						if (a.XmlAnyAttribute != null)
						{
							a.XmlAttribute = new XmlAttributeAttribute();
						}
						AttributeAccessor attributeAccessor = new AttributeAccessor();
						Type type = ((a.XmlAttribute.Type == null) ? arrayElementType : a.XmlAttribute.Type);
						this.typeScope.GetTypeDesc(type);
						attributeAccessor.Name = Accessor.EscapeQName((a.XmlAttribute.AttributeName.Length == 0) ? name : a.XmlAttribute.AttributeName);
						attributeAccessor.Namespace = ((a.XmlAttribute.Namespace == null) ? ns : a.XmlAttribute.Namespace);
						attributeAccessor.Form = a.XmlAttribute.Form;
						if (attributeAccessor.Form == XmlSchemaForm.None && ns != attributeAccessor.Namespace)
						{
							attributeAccessor.Form = XmlSchemaForm.Qualified;
						}
						attributeAccessor.CheckSpecial();
						XmlReflectionImporter.CheckForm(attributeAccessor.Form, ns != attributeAccessor.Namespace);
						attributeAccessor.Mapping = this.ImportTypeMapping(this.modelScope.GetTypeModel(type), ns, XmlReflectionImporter.ImportContext.Attribute, a.XmlAttribute.DataType, null, flag, false, limiter);
						attributeAccessor.IsList = flag;
						attributeAccessor.Default = this.GetDefaultValue(model.FieldTypeDesc, model.FieldType, a);
						attributeAccessor.Any = a.XmlAnyAttribute != null;
						if (attributeAccessor.Form == XmlSchemaForm.Qualified && attributeAccessor.Namespace != ns)
						{
							if (this.xsdAttributes == null)
							{
								this.xsdAttributes = new NameTable();
							}
							attributeAccessor = (AttributeAccessor)this.ReconcileAccessor(attributeAccessor, this.xsdAttributes);
						}
						accessor.Attribute = attributeAccessor;
					}
				}
				else if ((xmlFlags & xmlAttributeFlags) != (XmlAttributeFlags)0)
				{
					if ((xmlFlags & xmlAttributeFlags) != xmlFlags)
					{
						throw new InvalidOperationException(Res.GetString("XmlIllegalElementsArrayAttribute"));
					}
					if (a.XmlText != null)
					{
						TextAccessor textAccessor = new TextAccessor();
						Type type2 = ((a.XmlText.Type == null) ? arrayElementType : a.XmlText.Type);
						TypeDesc typeDesc = this.typeScope.GetTypeDesc(type2);
						textAccessor.Name = name;
						textAccessor.Mapping = this.ImportTypeMapping(this.modelScope.GetTypeModel(type2), ns, XmlReflectionImporter.ImportContext.Text, a.XmlText.DataType, null, true, false, limiter);
						if (!(textAccessor.Mapping is SpecialMapping) && typeDesc != this.typeScope.GetTypeDesc(typeof(string)))
						{
							throw new InvalidOperationException(Res.GetString("XmlIllegalArrayTextAttribute", new object[] { name }));
						}
						accessor.Text = textAccessor;
					}
					if (a.XmlText == null && a.XmlElements.Count == 0 && a.XmlAnyElements.Count == 0)
					{
						a.XmlElements.Add(XmlReflectionImporter.CreateElementAttribute(accessor.TypeDesc));
					}
					for (int i = 0; i < a.XmlElements.Count; i++)
					{
						XmlElementAttribute xmlElementAttribute = a.XmlElements[i];
						Type type3 = ((xmlElementAttribute.Type == null) ? arrayElementType : xmlElementAttribute.Type);
						TypeDesc typeDesc2 = this.typeScope.GetTypeDesc(type3);
						TypeModel typeModel = this.modelScope.GetTypeModel(type3);
						ElementAccessor elementAccessor = new ElementAccessor();
						elementAccessor.Namespace = (rpc ? null : ((xmlElementAttribute.Namespace == null) ? ns : xmlElementAttribute.Namespace));
						elementAccessor.Mapping = this.ImportTypeMapping(typeModel, rpc ? ns : elementAccessor.Namespace, XmlReflectionImporter.ImportContext.Element, xmlElementAttribute.DataType, null, limiter);
						if (a.XmlElements.Count == 1)
						{
							elementAccessor.Name = XmlConvert.EncodeLocalName((xmlElementAttribute.ElementName.Length == 0) ? name : xmlElementAttribute.ElementName);
						}
						else
						{
							elementAccessor.Name = ((xmlElementAttribute.ElementName.Length == 0) ? elementAccessor.Mapping.DefaultElementName : XmlConvert.EncodeLocalName(xmlElementAttribute.ElementName));
						}
						elementAccessor.Default = this.GetDefaultValue(model.FieldTypeDesc, model.FieldType, a);
						if (xmlElementAttribute.IsNullableSpecified && !xmlElementAttribute.IsNullable && typeModel.TypeDesc.IsOptionalValue)
						{
							throw new InvalidOperationException(Res.GetString("XmlInvalidNotNullable", new object[]
							{
								typeModel.TypeDesc.BaseTypeDesc.FullName,
								"XmlElement"
							}));
						}
						elementAccessor.IsNullable = (xmlElementAttribute.IsNullableSpecified ? xmlElementAttribute.IsNullable : typeModel.TypeDesc.IsOptionalValue);
						elementAccessor.Form = (rpc ? XmlSchemaForm.Unqualified : ((xmlElementAttribute.Form == XmlSchemaForm.None) ? xmlSchemaForm : xmlElementAttribute.Form));
						XmlReflectionImporter.CheckNullable(elementAccessor.IsNullable, typeDesc2, elementAccessor.Mapping);
						if (!rpc)
						{
							XmlReflectionImporter.CheckForm(elementAccessor.Form, ns != elementAccessor.Namespace);
							elementAccessor = this.ReconcileLocalAccessor(elementAccessor, ns);
						}
						if (xmlElementAttribute.Order != -1)
						{
							if (xmlElementAttribute.Order != num2 && num2 != -1)
							{
								throw new InvalidOperationException(Res.GetString("XmlSequenceMatch", new object[] { "Order" }));
							}
							num2 = xmlElementAttribute.Order;
						}
						XmlReflectionImporter.AddUniqueAccessor(nameTable, elementAccessor);
						arrayList.Add(elementAccessor);
					}
					NameTable nameTable2 = new NameTable();
					for (int j = 0; j < a.XmlAnyElements.Count; j++)
					{
						XmlAnyElementAttribute xmlAnyElementAttribute = a.XmlAnyElements[j];
						Type type4 = (typeof(IXmlSerializable).IsAssignableFrom(arrayElementType) ? arrayElementType : (typeof(XmlNode).IsAssignableFrom(arrayElementType) ? arrayElementType : typeof(XmlElement)));
						if (!arrayElementType.IsAssignableFrom(type4))
						{
							throw new InvalidOperationException(Res.GetString("XmlIllegalAnyElement", new object[] { arrayElementType.FullName }));
						}
						string text2 = ((xmlAnyElementAttribute.Name.Length == 0) ? xmlAnyElementAttribute.Name : XmlConvert.EncodeLocalName(xmlAnyElementAttribute.Name));
						string text3 = (xmlAnyElementAttribute.NamespaceSpecified ? xmlAnyElementAttribute.Namespace : null);
						if (nameTable2[text2, text3] == null)
						{
							nameTable2[text2, text3] = xmlAnyElementAttribute;
							if (nameTable[text2, (text3 == null) ? ns : text3] != null)
							{
								throw new InvalidOperationException(Res.GetString("XmlAnyElementDuplicate", new object[]
								{
									name,
									xmlAnyElementAttribute.Name,
									(xmlAnyElementAttribute.Namespace == null) ? "null" : xmlAnyElementAttribute.Namespace
								}));
							}
							ElementAccessor elementAccessor2 = new ElementAccessor();
							elementAccessor2.Name = text2;
							elementAccessor2.Namespace = ((text3 == null) ? ns : text3);
							elementAccessor2.Any = true;
							elementAccessor2.AnyNamespaces = text3;
							TypeDesc typeDesc3 = this.typeScope.GetTypeDesc(type4);
							TypeModel typeModel2 = this.modelScope.GetTypeModel(type4);
							if (elementAccessor2.Name.Length > 0)
							{
								typeModel2.TypeDesc.IsMixed = true;
							}
							elementAccessor2.Mapping = this.ImportTypeMapping(typeModel2, elementAccessor2.Namespace, XmlReflectionImporter.ImportContext.Element, string.Empty, null, limiter);
							elementAccessor2.Default = this.GetDefaultValue(model.FieldTypeDesc, model.FieldType, a);
							elementAccessor2.IsNullable = false;
							elementAccessor2.Form = xmlSchemaForm;
							XmlReflectionImporter.CheckNullable(elementAccessor2.IsNullable, typeDesc3, elementAccessor2.Mapping);
							if (!rpc)
							{
								XmlReflectionImporter.CheckForm(elementAccessor2.Form, ns != elementAccessor2.Namespace);
								elementAccessor2 = this.ReconcileLocalAccessor(elementAccessor2, ns);
							}
							nameTable.Add(elementAccessor2.Name, elementAccessor2.Namespace, elementAccessor2);
							arrayList.Add(elementAccessor2);
							if (xmlAnyElementAttribute.Order != -1)
							{
								if (xmlAnyElementAttribute.Order != num2 && num2 != -1)
								{
									throw new InvalidOperationException(Res.GetString("XmlSequenceMatch", new object[] { "Order" }));
								}
								num2 = xmlAnyElementAttribute.Order;
							}
						}
					}
				}
				else
				{
					if ((xmlFlags & xmlAttributeFlags3) != (XmlAttributeFlags)0 && (xmlFlags & xmlAttributeFlags3) != xmlFlags)
					{
						throw new InvalidOperationException(Res.GetString("XmlIllegalArrayArrayAttribute"));
					}
					TypeDesc typeDesc4 = this.typeScope.GetTypeDesc(arrayElementType);
					if (a.XmlArray == null)
					{
						a.XmlArray = XmlReflectionImporter.CreateArrayAttribute(accessor.TypeDesc);
					}
					if (XmlReflectionImporter.CountAtLevel(a.XmlArrayItems, this.arrayNestingLevel) == 0)
					{
						a.XmlArrayItems.Add(XmlReflectionImporter.CreateArrayItemAttribute(typeDesc4, this.arrayNestingLevel));
					}
					ElementAccessor elementAccessor3 = new ElementAccessor();
					elementAccessor3.Name = XmlConvert.EncodeLocalName((a.XmlArray.ElementName.Length == 0) ? name : a.XmlArray.ElementName);
					elementAccessor3.Namespace = (rpc ? null : ((a.XmlArray.Namespace == null) ? ns : a.XmlArray.Namespace));
					this.savedArrayItemAttributes = a.XmlArrayItems;
					this.savedArrayNamespace = elementAccessor3.Namespace;
					ArrayMapping arrayMapping = this.ImportArrayLikeMapping(this.modelScope.GetArrayModel(fieldType), ns, limiter);
					elementAccessor3.Mapping = arrayMapping;
					elementAccessor3.IsNullable = a.XmlArray.IsNullable;
					elementAccessor3.Form = (rpc ? XmlSchemaForm.Unqualified : ((a.XmlArray.Form == XmlSchemaForm.None) ? xmlSchemaForm : a.XmlArray.Form));
					num2 = a.XmlArray.Order;
					XmlReflectionImporter.CheckNullable(elementAccessor3.IsNullable, accessor.TypeDesc, elementAccessor3.Mapping);
					if (!rpc)
					{
						XmlReflectionImporter.CheckForm(elementAccessor3.Form, ns != elementAccessor3.Namespace);
						elementAccessor3 = this.ReconcileLocalAccessor(elementAccessor3, ns);
					}
					this.savedArrayItemAttributes = null;
					this.savedArrayNamespace = null;
					XmlReflectionImporter.AddUniqueAccessor(nameTable, elementAccessor3);
					arrayList.Add(elementAccessor3);
				}
			}
			else if (!accessor.TypeDesc.IsVoid)
			{
				XmlAttributeFlags xmlAttributeFlags4 = (XmlAttributeFlags)3380;
				if ((xmlFlags & xmlAttributeFlags4) != xmlFlags)
				{
					throw new InvalidOperationException(Res.GetString("XmlIllegalAttribute"));
				}
				if (accessor.TypeDesc.IsPrimitive || accessor.TypeDesc.IsEnum)
				{
					if (a.XmlAnyElements.Count > 0)
					{
						throw new InvalidOperationException(Res.GetString("XmlIllegalAnyElement", new object[] { accessor.TypeDesc.FullName }));
					}
					if (a.XmlAttribute != null)
					{
						if (a.XmlElements.Count > 0)
						{
							throw new InvalidOperationException(Res.GetString("XmlIllegalAttribute"));
						}
						if (a.XmlAttribute.Type != null)
						{
							throw new InvalidOperationException(Res.GetString("XmlIllegalType", new object[] { "XmlAttribute" }));
						}
						AttributeAccessor attributeAccessor2 = new AttributeAccessor();
						attributeAccessor2.Name = Accessor.EscapeQName((a.XmlAttribute.AttributeName.Length == 0) ? name : a.XmlAttribute.AttributeName);
						attributeAccessor2.Namespace = ((a.XmlAttribute.Namespace == null) ? ns : a.XmlAttribute.Namespace);
						attributeAccessor2.Form = a.XmlAttribute.Form;
						if (attributeAccessor2.Form == XmlSchemaForm.None && ns != attributeAccessor2.Namespace)
						{
							attributeAccessor2.Form = XmlSchemaForm.Qualified;
						}
						attributeAccessor2.CheckSpecial();
						XmlReflectionImporter.CheckForm(attributeAccessor2.Form, ns != attributeAccessor2.Namespace);
						attributeAccessor2.Mapping = this.ImportTypeMapping(this.modelScope.GetTypeModel(fieldType), ns, XmlReflectionImporter.ImportContext.Attribute, a.XmlAttribute.DataType, null, limiter);
						attributeAccessor2.Default = this.GetDefaultValue(model.FieldTypeDesc, model.FieldType, a);
						attributeAccessor2.Any = a.XmlAnyAttribute != null;
						if (attributeAccessor2.Form == XmlSchemaForm.Qualified && attributeAccessor2.Namespace != ns)
						{
							if (this.xsdAttributes == null)
							{
								this.xsdAttributes = new NameTable();
							}
							attributeAccessor2 = (AttributeAccessor)this.ReconcileAccessor(attributeAccessor2, this.xsdAttributes);
						}
						accessor.Attribute = attributeAccessor2;
					}
					else
					{
						if (a.XmlText != null)
						{
							if (a.XmlText.Type != null && a.XmlText.Type != fieldType)
							{
								throw new InvalidOperationException(Res.GetString("XmlIllegalType", new object[] { "XmlText" }));
							}
							accessor.Text = new TextAccessor
							{
								Name = name,
								Mapping = this.ImportTypeMapping(this.modelScope.GetTypeModel(fieldType), ns, XmlReflectionImporter.ImportContext.Text, a.XmlText.DataType, null, limiter)
							};
						}
						else if (a.XmlElements.Count == 0)
						{
							a.XmlElements.Add(XmlReflectionImporter.CreateElementAttribute(accessor.TypeDesc));
						}
						for (int k = 0; k < a.XmlElements.Count; k++)
						{
							XmlElementAttribute xmlElementAttribute2 = a.XmlElements[k];
							if (xmlElementAttribute2.Type != null && this.typeScope.GetTypeDesc(xmlElementAttribute2.Type) != accessor.TypeDesc)
							{
								throw new InvalidOperationException(Res.GetString("XmlIllegalType", new object[] { "XmlElement" }));
							}
							ElementAccessor elementAccessor4 = new ElementAccessor();
							elementAccessor4.Name = XmlConvert.EncodeLocalName((xmlElementAttribute2.ElementName.Length == 0) ? name : xmlElementAttribute2.ElementName);
							elementAccessor4.Namespace = (rpc ? null : ((xmlElementAttribute2.Namespace == null) ? ns : xmlElementAttribute2.Namespace));
							TypeModel typeModel3 = this.modelScope.GetTypeModel(fieldType);
							elementAccessor4.Mapping = this.ImportTypeMapping(typeModel3, rpc ? ns : elementAccessor4.Namespace, XmlReflectionImporter.ImportContext.Element, xmlElementAttribute2.DataType, null, limiter);
							if (elementAccessor4.Mapping.TypeDesc.Kind == TypeKind.Node)
							{
								elementAccessor4.Any = true;
							}
							elementAccessor4.Default = this.GetDefaultValue(model.FieldTypeDesc, model.FieldType, a);
							if (xmlElementAttribute2.IsNullableSpecified && !xmlElementAttribute2.IsNullable && typeModel3.TypeDesc.IsOptionalValue)
							{
								throw new InvalidOperationException(Res.GetString("XmlInvalidNotNullable", new object[]
								{
									typeModel3.TypeDesc.BaseTypeDesc.FullName,
									"XmlElement"
								}));
							}
							elementAccessor4.IsNullable = (xmlElementAttribute2.IsNullableSpecified ? xmlElementAttribute2.IsNullable : typeModel3.TypeDesc.IsOptionalValue);
							elementAccessor4.Form = (rpc ? XmlSchemaForm.Unqualified : ((xmlElementAttribute2.Form == XmlSchemaForm.None) ? xmlSchemaForm : xmlElementAttribute2.Form));
							XmlReflectionImporter.CheckNullable(elementAccessor4.IsNullable, accessor.TypeDesc, elementAccessor4.Mapping);
							if (!rpc)
							{
								XmlReflectionImporter.CheckForm(elementAccessor4.Form, ns != elementAccessor4.Namespace);
								elementAccessor4 = this.ReconcileLocalAccessor(elementAccessor4, ns);
							}
							if (xmlElementAttribute2.Order != -1)
							{
								if (xmlElementAttribute2.Order != num2 && num2 != -1)
								{
									throw new InvalidOperationException(Res.GetString("XmlSequenceMatch", new object[] { "Order" }));
								}
								num2 = xmlElementAttribute2.Order;
							}
							XmlReflectionImporter.AddUniqueAccessor(nameTable, elementAccessor4);
							arrayList.Add(elementAccessor4);
						}
					}
				}
				else if (a.Xmlns)
				{
					if (xmlFlags != XmlAttributeFlags.XmlnsDeclarations)
					{
						throw new InvalidOperationException(Res.GetString("XmlSoleXmlnsAttribute"));
					}
					if (fieldType != typeof(XmlSerializerNamespaces))
					{
						throw new InvalidOperationException(Res.GetString("XmlXmlnsInvalidType", new object[]
						{
							name,
							fieldType.FullName,
							typeof(XmlSerializerNamespaces).FullName
						}));
					}
					accessor.Xmlns = new XmlnsAccessor();
					accessor.Ignore = true;
				}
				else if (a.XmlAttribute != null || a.XmlText != null)
				{
					if (accessor.TypeDesc.Kind == TypeKind.Serializable)
					{
						throw new InvalidOperationException(Res.GetString("XmlIllegalAttrOrTextInterface", new object[]
						{
							name,
							accessor.TypeDesc.FullName,
							typeof(IXmlSerializable).Name
						}));
					}
					throw new InvalidOperationException(Res.GetString("XmlIllegalAttrOrText", new object[] { name, accessor.TypeDesc }));
				}
				else
				{
					if (a.XmlElements.Count == 0 && a.XmlAnyElements.Count == 0)
					{
						a.XmlElements.Add(XmlReflectionImporter.CreateElementAttribute(accessor.TypeDesc));
					}
					for (int l = 0; l < a.XmlElements.Count; l++)
					{
						XmlElementAttribute xmlElementAttribute3 = a.XmlElements[l];
						Type type5 = ((xmlElementAttribute3.Type == null) ? fieldType : xmlElementAttribute3.Type);
						TypeDesc typeDesc5 = this.typeScope.GetTypeDesc(type5);
						ElementAccessor elementAccessor5 = new ElementAccessor();
						TypeModel typeModel4 = this.modelScope.GetTypeModel(type5);
						elementAccessor5.Namespace = (rpc ? null : ((xmlElementAttribute3.Namespace == null) ? ns : xmlElementAttribute3.Namespace));
						elementAccessor5.Mapping = this.ImportTypeMapping(typeModel4, rpc ? ns : elementAccessor5.Namespace, XmlReflectionImporter.ImportContext.Element, xmlElementAttribute3.DataType, null, false, openModel, limiter);
						if (a.XmlElements.Count == 1)
						{
							elementAccessor5.Name = XmlConvert.EncodeLocalName((xmlElementAttribute3.ElementName.Length == 0) ? name : xmlElementAttribute3.ElementName);
						}
						else
						{
							elementAccessor5.Name = ((xmlElementAttribute3.ElementName.Length == 0) ? elementAccessor5.Mapping.DefaultElementName : XmlConvert.EncodeLocalName(xmlElementAttribute3.ElementName));
						}
						elementAccessor5.Default = this.GetDefaultValue(model.FieldTypeDesc, model.FieldType, a);
						if (xmlElementAttribute3.IsNullableSpecified && !xmlElementAttribute3.IsNullable && typeModel4.TypeDesc.IsOptionalValue)
						{
							throw new InvalidOperationException(Res.GetString("XmlInvalidNotNullable", new object[]
							{
								typeModel4.TypeDesc.BaseTypeDesc.FullName,
								"XmlElement"
							}));
						}
						elementAccessor5.IsNullable = (xmlElementAttribute3.IsNullableSpecified ? xmlElementAttribute3.IsNullable : typeModel4.TypeDesc.IsOptionalValue);
						elementAccessor5.Form = (rpc ? XmlSchemaForm.Unqualified : ((xmlElementAttribute3.Form == XmlSchemaForm.None) ? xmlSchemaForm : xmlElementAttribute3.Form));
						XmlReflectionImporter.CheckNullable(elementAccessor5.IsNullable, typeDesc5, elementAccessor5.Mapping);
						if (!rpc)
						{
							XmlReflectionImporter.CheckForm(elementAccessor5.Form, ns != elementAccessor5.Namespace);
							elementAccessor5 = this.ReconcileLocalAccessor(elementAccessor5, ns);
						}
						if (xmlElementAttribute3.Order != -1)
						{
							if (xmlElementAttribute3.Order != num2 && num2 != -1)
							{
								throw new InvalidOperationException(Res.GetString("XmlSequenceMatch", new object[] { "Order" }));
							}
							num2 = xmlElementAttribute3.Order;
						}
						XmlReflectionImporter.AddUniqueAccessor(nameTable, elementAccessor5);
						arrayList.Add(elementAccessor5);
					}
					NameTable nameTable3 = new NameTable();
					for (int m = 0; m < a.XmlAnyElements.Count; m++)
					{
						XmlAnyElementAttribute xmlAnyElementAttribute2 = a.XmlAnyElements[m];
						Type type6 = (typeof(IXmlSerializable).IsAssignableFrom(fieldType) ? fieldType : (typeof(XmlNode).IsAssignableFrom(fieldType) ? fieldType : typeof(XmlElement)));
						if (!fieldType.IsAssignableFrom(type6))
						{
							throw new InvalidOperationException(Res.GetString("XmlIllegalAnyElement", new object[] { fieldType.FullName }));
						}
						string text4 = ((xmlAnyElementAttribute2.Name.Length == 0) ? xmlAnyElementAttribute2.Name : XmlConvert.EncodeLocalName(xmlAnyElementAttribute2.Name));
						string text5 = (xmlAnyElementAttribute2.NamespaceSpecified ? xmlAnyElementAttribute2.Namespace : null);
						if (nameTable3[text4, text5] == null)
						{
							nameTable3[text4, text5] = xmlAnyElementAttribute2;
							if (nameTable[text4, (text5 == null) ? ns : text5] != null)
							{
								throw new InvalidOperationException(Res.GetString("XmlAnyElementDuplicate", new object[]
								{
									name,
									xmlAnyElementAttribute2.Name,
									(xmlAnyElementAttribute2.Namespace == null) ? "null" : xmlAnyElementAttribute2.Namespace
								}));
							}
							ElementAccessor elementAccessor6 = new ElementAccessor();
							elementAccessor6.Name = text4;
							elementAccessor6.Namespace = ((text5 == null) ? ns : text5);
							elementAccessor6.Any = true;
							elementAccessor6.AnyNamespaces = text5;
							TypeDesc typeDesc6 = this.typeScope.GetTypeDesc(type6);
							TypeModel typeModel5 = this.modelScope.GetTypeModel(type6);
							if (elementAccessor6.Name.Length > 0)
							{
								typeModel5.TypeDesc.IsMixed = true;
							}
							elementAccessor6.Mapping = this.ImportTypeMapping(typeModel5, elementAccessor6.Namespace, XmlReflectionImporter.ImportContext.Element, string.Empty, null, false, openModel, limiter);
							elementAccessor6.Default = this.GetDefaultValue(model.FieldTypeDesc, model.FieldType, a);
							elementAccessor6.IsNullable = false;
							elementAccessor6.Form = xmlSchemaForm;
							XmlReflectionImporter.CheckNullable(elementAccessor6.IsNullable, typeDesc6, elementAccessor6.Mapping);
							if (!rpc)
							{
								XmlReflectionImporter.CheckForm(elementAccessor6.Form, ns != elementAccessor6.Namespace);
								elementAccessor6 = this.ReconcileLocalAccessor(elementAccessor6, ns);
							}
							if (xmlAnyElementAttribute2.Order != -1)
							{
								if (xmlAnyElementAttribute2.Order != num2 && num2 != -1)
								{
									throw new InvalidOperationException(Res.GetString("XmlSequenceMatch", new object[] { "Order" }));
								}
								num2 = xmlAnyElementAttribute2.Order;
							}
							nameTable.Add(elementAccessor6.Name, elementAccessor6.Namespace, elementAccessor6);
							arrayList.Add(elementAccessor6);
						}
					}
				}
			}
			accessor.Elements = (ElementAccessor[])arrayList.ToArray(typeof(ElementAccessor));
			accessor.SequenceId = num2;
			if (rpc)
			{
				if (accessor.TypeDesc.IsArrayLike && accessor.Elements.Length > 0 && !(accessor.Elements[0].Mapping is ArrayMapping))
				{
					throw new InvalidOperationException(Res.GetString("XmlRpcLitArrayElement", new object[] { accessor.Elements[0].Name }));
				}
				if (accessor.Xmlns != null)
				{
					throw new InvalidOperationException(Res.GetString("XmlRpcLitXmlns", new object[] { accessor.Name }));
				}
			}
			if (accessor.ChoiceIdentifier != null)
			{
				accessor.ChoiceIdentifier.MemberIds = new string[accessor.Elements.Length];
				int n = 0;
				while (n < accessor.Elements.Length)
				{
					bool flag2 = false;
					ElementAccessor elementAccessor7 = accessor.Elements[n];
					EnumMapping enumMapping = (EnumMapping)accessor.ChoiceIdentifier.Mapping;
					for (int num3 = 0; num3 < enumMapping.Constants.Length; num3++)
					{
						string xmlName = enumMapping.Constants[num3].XmlName;
						if (elementAccessor7.Any && elementAccessor7.Name.Length == 0)
						{
							string text6 = ((elementAccessor7.AnyNamespaces == null) ? "##any" : elementAccessor7.AnyNamespaces);
							if (xmlName.Substring(0, xmlName.Length - 1) == text6)
							{
								accessor.ChoiceIdentifier.MemberIds[n] = enumMapping.Constants[num3].Name;
								flag2 = true;
								break;
							}
						}
						else
						{
							int num4 = xmlName.LastIndexOf(':');
							string text7 = ((num4 < 0) ? enumMapping.Namespace : xmlName.Substring(0, num4));
							string text8 = ((num4 < 0) ? xmlName : xmlName.Substring(num4 + 1));
							if (elementAccessor7.Name == text8 && ((elementAccessor7.Form == XmlSchemaForm.Unqualified && string.IsNullOrEmpty(text7)) || elementAccessor7.Namespace == text7))
							{
								accessor.ChoiceIdentifier.MemberIds[n] = enumMapping.Constants[num3].Name;
								flag2 = true;
								break;
							}
						}
					}
					if (!flag2)
					{
						if (elementAccessor7.Any && elementAccessor7.Name.Length == 0)
						{
							throw new InvalidOperationException(Res.GetString("XmlChoiceMissingAnyValue", new object[] { accessor.ChoiceIdentifier.Mapping.TypeDesc.FullName }));
						}
						string text9 = ((elementAccessor7.Namespace != null && elementAccessor7.Namespace.Length > 0) ? (elementAccessor7.Namespace + ":" + elementAccessor7.Name) : elementAccessor7.Name);
						throw new InvalidOperationException(Res.GetString("XmlChoiceMissingValue", new object[]
						{
							accessor.ChoiceIdentifier.Mapping.TypeDesc.FullName,
							text9,
							elementAccessor7.Name,
							elementAccessor7.Namespace
						}));
					}
					else
					{
						n++;
					}
				}
			}
			this.arrayNestingLevel = num;
			this.savedArrayItemAttributes = xmlArrayItemAttributes;
			this.savedArrayNamespace = text;
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000B2CA0 File Offset: 0x000B1CA0
		private void CheckTopLevelAttributes(XmlAttributes a, string accessorName)
		{
			XmlAttributeFlags xmlFlags = a.XmlFlags;
			if ((xmlFlags & (XmlAttributeFlags)544) != (XmlAttributeFlags)0)
			{
				throw new InvalidOperationException(Res.GetString("XmlRpcLitAttributeAttributes"));
			}
			if ((xmlFlags & (XmlAttributeFlags)1284) != (XmlAttributeFlags)0)
			{
				throw new InvalidOperationException(Res.GetString("XmlRpcLitAttributes"));
			}
			if (a.XmlElements != null && a.XmlElements.Count > 0)
			{
				if (a.XmlElements.Count > 1)
				{
					throw new InvalidOperationException(Res.GetString("XmlRpcLitElements"));
				}
				XmlElementAttribute xmlElementAttribute = a.XmlElements[0];
				if (xmlElementAttribute.Namespace != null)
				{
					throw new InvalidOperationException(Res.GetString("XmlRpcLitElementNamespace", new object[] { "Namespace", xmlElementAttribute.Namespace }));
				}
				if (xmlElementAttribute.IsNullable)
				{
					throw new InvalidOperationException(Res.GetString("XmlRpcLitElementNullable", new object[] { "IsNullable", "true" }));
				}
			}
			if (a.XmlArray != null && a.XmlArray.Namespace != null)
			{
				throw new InvalidOperationException(Res.GetString("XmlRpcLitElementNamespace", new object[]
				{
					"Namespace",
					a.XmlArray.Namespace
				}));
			}
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000B2DD8 File Offset: 0x000B1DD8
		private void CheckAmbiguousChoice(XmlAttributes a, Type accessorType, string accessorName)
		{
			Hashtable hashtable = new Hashtable();
			XmlElementAttributes xmlElements = a.XmlElements;
			if (xmlElements != null && xmlElements.Count >= 2 && a.XmlChoiceIdentifier == null)
			{
				for (int i = 0; i < xmlElements.Count; i++)
				{
					Type type = ((xmlElements[i].Type == null) ? accessorType : xmlElements[i].Type);
					if (hashtable.Contains(type))
					{
						throw new InvalidOperationException(Res.GetString("XmlChoiceIdentiferMissing", new object[]
						{
							typeof(XmlChoiceIdentifierAttribute).Name,
							accessorName
						}));
					}
					hashtable.Add(type, false);
				}
			}
			if (hashtable.Contains(typeof(XmlElement)) && a.XmlAnyElements.Count > 0)
			{
				throw new InvalidOperationException(Res.GetString("XmlChoiceIdentiferMissing", new object[]
				{
					typeof(XmlChoiceIdentifierAttribute).Name,
					accessorName
				}));
			}
			XmlArrayItemAttributes xmlArrayItems = a.XmlArrayItems;
			if (xmlArrayItems != null && xmlArrayItems.Count >= 2)
			{
				NameTable nameTable = new NameTable();
				for (int j = 0; j < xmlArrayItems.Count; j++)
				{
					Type type2 = ((xmlArrayItems[j].Type == null) ? accessorType : xmlArrayItems[j].Type);
					string text = xmlArrayItems[j].NestingLevel.ToString(CultureInfo.InvariantCulture);
					XmlArrayItemAttribute xmlArrayItemAttribute = (XmlArrayItemAttribute)nameTable[type2.FullName, text];
					if (xmlArrayItemAttribute != null)
					{
						throw new InvalidOperationException(Res.GetString("XmlArrayItemAmbiguousTypes", new object[]
						{
							accessorName,
							xmlArrayItemAttribute.ElementName,
							xmlArrayItems[j].ElementName,
							typeof(XmlElementAttribute).Name,
							typeof(XmlChoiceIdentifierAttribute).Name,
							accessorName
						}));
					}
					nameTable[type2.FullName, text] = xmlArrayItems[j];
				}
			}
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000B2FF0 File Offset: 0x000B1FF0
		private void CheckChoiceIdentifierMapping(EnumMapping choiceMapping)
		{
			NameTable nameTable = new NameTable();
			for (int i = 0; i < choiceMapping.Constants.Length; i++)
			{
				string xmlName = choiceMapping.Constants[i].XmlName;
				int num = xmlName.LastIndexOf(':');
				string text = ((num < 0) ? xmlName : xmlName.Substring(num + 1));
				string text2 = ((num < 0) ? "" : xmlName.Substring(0, num));
				if (nameTable[text, text2] != null)
				{
					throw new InvalidOperationException(Res.GetString("XmlChoiceIdDuplicate", new object[] { choiceMapping.TypeName, xmlName }));
				}
				nameTable.Add(text, text2, choiceMapping.Constants[i]);
			}
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000B30A4 File Offset: 0x000B20A4
		private object GetDefaultValue(TypeDesc fieldTypeDesc, Type t, XmlAttributes a)
		{
			if (a.XmlDefaultValue == null || a.XmlDefaultValue == DBNull.Value)
			{
				return null;
			}
			if (fieldTypeDesc.Kind != TypeKind.Primitive && fieldTypeDesc.Kind != TypeKind.Enum)
			{
				a.XmlDefaultValue = null;
				return a.XmlDefaultValue;
			}
			if (fieldTypeDesc.Kind != TypeKind.Enum)
			{
				return a.XmlDefaultValue;
			}
			string text = Enum.Format(t, a.XmlDefaultValue, "G").Replace(",", " ");
			string text2 = Enum.Format(t, a.XmlDefaultValue, "D");
			if (text == text2)
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidDefaultValue", new object[]
				{
					text,
					a.XmlDefaultValue.GetType().FullName
				}));
			}
			return text;
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000B3164 File Offset: 0x000B2164
		private static XmlArrayItemAttribute CreateArrayItemAttribute(TypeDesc typeDesc, int nestingLevel)
		{
			return new XmlArrayItemAttribute
			{
				NestingLevel = nestingLevel
			};
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x000B3180 File Offset: 0x000B2180
		private static XmlArrayAttribute CreateArrayAttribute(TypeDesc typeDesc)
		{
			return new XmlArrayAttribute();
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000B3194 File Offset: 0x000B2194
		private static XmlElementAttribute CreateElementAttribute(TypeDesc typeDesc)
		{
			return new XmlElementAttribute
			{
				IsNullable = typeDesc.IsOptionalValue
			};
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000B31B4 File Offset: 0x000B21B4
		private static void AddUniqueAccessor(INameScope scope, Accessor accessor)
		{
			Accessor accessor2 = (Accessor)scope[accessor.Name, accessor.Namespace];
			if (accessor2 == null)
			{
				scope[accessor.Name, accessor.Namespace] = accessor;
				return;
			}
			if (accessor is ElementAccessor)
			{
				throw new InvalidOperationException(Res.GetString("XmlDuplicateElementName", new object[] { accessor2.Name, accessor2.Namespace }));
			}
			throw new InvalidOperationException(Res.GetString("XmlDuplicateAttributeName", new object[] { accessor2.Name, accessor2.Namespace }));
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000B324C File Offset: 0x000B224C
		private static void AddUniqueAccessor(MemberMapping member, INameScope elements, INameScope attributes, bool isSequence)
		{
			if (member.Attribute != null)
			{
				XmlReflectionImporter.AddUniqueAccessor(attributes, member.Attribute);
				return;
			}
			if (!isSequence && member.Elements != null && member.Elements.Length > 0)
			{
				for (int i = 0; i < member.Elements.Length; i++)
				{
					XmlReflectionImporter.AddUniqueAccessor(elements, member.Elements[i]);
				}
			}
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x000B32A5 File Offset: 0x000B22A5
		private static void CheckForm(XmlSchemaForm form, bool isQualified)
		{
			if (isQualified && form == XmlSchemaForm.Unqualified)
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidFormUnqualified"));
			}
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000B32C0 File Offset: 0x000B22C0
		private static void CheckNullable(bool isNullable, TypeDesc typeDesc, TypeMapping mapping)
		{
			if (mapping is NullableMapping)
			{
				return;
			}
			if (mapping is SerializableMapping)
			{
				return;
			}
			if (isNullable && !typeDesc.IsNullable)
			{
				throw new InvalidOperationException(Res.GetString("XmlInvalidIsNullable", new object[] { typeDesc.FullName }));
			}
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000B330C File Offset: 0x000B230C
		private static ElementAccessor CreateElementAccessor(TypeMapping mapping, string ns)
		{
			ElementAccessor elementAccessor = new ElementAccessor();
			bool flag = mapping.TypeDesc.Kind == TypeKind.Node;
			if (!flag && mapping is SerializableMapping)
			{
				flag = ((SerializableMapping)mapping).IsAny;
			}
			if (flag)
			{
				elementAccessor.Any = true;
			}
			else
			{
				elementAccessor.Name = mapping.DefaultElementName;
				elementAccessor.Namespace = ns;
			}
			elementAccessor.Mapping = mapping;
			return elementAccessor;
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000B3370 File Offset: 0x000B2370
		internal static XmlTypeMapping GetTopLevelMapping(Type type, string defaultNamespace)
		{
			XmlAttributes xmlAttributes = new XmlAttributes(type);
			TypeDesc typeDesc = new TypeScope().GetTypeDesc(type);
			ElementAccessor elementAccessor = new ElementAccessor();
			if (typeDesc.Kind == TypeKind.Node)
			{
				elementAccessor.Any = true;
			}
			else
			{
				string text = ((xmlAttributes.XmlRoot == null) ? defaultNamespace : xmlAttributes.XmlRoot.Namespace);
				string text2 = string.Empty;
				if (xmlAttributes.XmlType != null)
				{
					text2 = xmlAttributes.XmlType.TypeName;
				}
				if (text2.Length == 0)
				{
					text2 = type.Name;
				}
				elementAccessor.Name = XmlConvert.EncodeLocalName(text2);
				elementAccessor.Namespace = text;
			}
			XmlTypeMapping xmlTypeMapping = new XmlTypeMapping(null, elementAccessor);
			xmlTypeMapping.SetKeyInternal(XmlMapping.GenerateKey(type, xmlAttributes.XmlRoot, defaultNamespace));
			return xmlTypeMapping;
		}

		// Token: 0x0400158D RID: 5517
		private TypeScope typeScope;

		// Token: 0x0400158E RID: 5518
		private XmlAttributeOverrides attributeOverrides;

		// Token: 0x0400158F RID: 5519
		private XmlAttributes defaultAttributes = new XmlAttributes();

		// Token: 0x04001590 RID: 5520
		private NameTable types = new NameTable();

		// Token: 0x04001591 RID: 5521
		private NameTable nullables = new NameTable();

		// Token: 0x04001592 RID: 5522
		private NameTable elements = new NameTable();

		// Token: 0x04001593 RID: 5523
		private NameTable xsdAttributes;

		// Token: 0x04001594 RID: 5524
		private Hashtable specials;

		// Token: 0x04001595 RID: 5525
		private Hashtable anonymous = new Hashtable();

		// Token: 0x04001596 RID: 5526
		private NameTable serializables;

		// Token: 0x04001597 RID: 5527
		private StructMapping root;

		// Token: 0x04001598 RID: 5528
		private string defaultNs;

		// Token: 0x04001599 RID: 5529
		private ModelScope modelScope;

		// Token: 0x0400159A RID: 5530
		private int arrayNestingLevel;

		// Token: 0x0400159B RID: 5531
		private XmlArrayItemAttributes savedArrayItemAttributes;

		// Token: 0x0400159C RID: 5532
		private string savedArrayNamespace;

		// Token: 0x0400159D RID: 5533
		private int choiceNum = 1;

		// Token: 0x02000315 RID: 789
		private enum ImportContext
		{
			// Token: 0x0400159F RID: 5535
			Text,
			// Token: 0x040015A0 RID: 5536
			Attribute,
			// Token: 0x040015A1 RID: 5537
			Element
		}
	}
}

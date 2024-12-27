using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002EA RID: 746
	[ComVisible(true)]
	[Serializable]
	public sealed class CustomAttributeData
	{
		// Token: 0x06001DD8 RID: 7640 RVA: 0x0004A210 File Offset: 0x00049210
		public static IList<CustomAttributeData> GetCustomAttributes(MemberInfo target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(target.Module, target.MetadataToken);
			int num = 0;
			Attribute[] array = null;
			if (target is RuntimeType)
			{
				array = PseudoCustomAttribute.GetCustomAttributes((RuntimeType)target, typeof(object), false, out num);
			}
			else if (target is RuntimeMethodInfo)
			{
				array = PseudoCustomAttribute.GetCustomAttributes((RuntimeMethodInfo)target, typeof(object), false, out num);
			}
			else if (target is RuntimeFieldInfo)
			{
				array = PseudoCustomAttribute.GetCustomAttributes((RuntimeFieldInfo)target, typeof(object), out num);
			}
			if (num == 0)
			{
				return customAttributes;
			}
			CustomAttributeData[] array2 = new CustomAttributeData[customAttributes.Count + num];
			customAttributes.CopyTo(array2, num);
			for (int i = 0; i < num; i++)
			{
				if (!PseudoCustomAttribute.IsSecurityAttribute(array[i].GetType()))
				{
					array2[i] = new CustomAttributeData(array[i]);
				}
			}
			return Array.AsReadOnly<CustomAttributeData>(array2);
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x0004A2F5 File Offset: 0x000492F5
		public static IList<CustomAttributeData> GetCustomAttributes(Module target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (target.IsResourceInternal())
			{
				return new List<CustomAttributeData>();
			}
			return CustomAttributeData.GetCustomAttributes(target, target.MetadataToken);
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x0004A320 File Offset: 0x00049320
		public static IList<CustomAttributeData> GetCustomAttributes(Assembly target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			return CustomAttributeData.GetCustomAttributes(target.ManifestModule, target.AssemblyHandle.GetToken());
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x0004A354 File Offset: 0x00049354
		public static IList<CustomAttributeData> GetCustomAttributes(ParameterInfo target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(target.Member.Module, target.MetadataToken);
			int num = 0;
			Attribute[] customAttributes2 = PseudoCustomAttribute.GetCustomAttributes(target, typeof(object), out num);
			if (num == 0)
			{
				return customAttributes;
			}
			CustomAttributeData[] array = new CustomAttributeData[customAttributes.Count + num];
			customAttributes.CopyTo(array, num);
			for (int i = 0; i < num; i++)
			{
				array[i] = new CustomAttributeData(customAttributes2[i]);
			}
			return Array.AsReadOnly<CustomAttributeData>(array);
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x0004A3DC File Offset: 0x000493DC
		private static CustomAttributeEncoding TypeToCustomAttributeEncoding(Type type)
		{
			if (type == typeof(int))
			{
				return CustomAttributeEncoding.Int32;
			}
			if (type.IsEnum)
			{
				return CustomAttributeEncoding.Enum;
			}
			if (type == typeof(string))
			{
				return CustomAttributeEncoding.String;
			}
			if (type == typeof(Type))
			{
				return CustomAttributeEncoding.Type;
			}
			if (type == typeof(object))
			{
				return CustomAttributeEncoding.Object;
			}
			if (type.IsArray)
			{
				return CustomAttributeEncoding.Array;
			}
			if (type == typeof(char))
			{
				return CustomAttributeEncoding.Char;
			}
			if (type == typeof(bool))
			{
				return CustomAttributeEncoding.Boolean;
			}
			if (type == typeof(byte))
			{
				return CustomAttributeEncoding.Byte;
			}
			if (type == typeof(sbyte))
			{
				return CustomAttributeEncoding.SByte;
			}
			if (type == typeof(short))
			{
				return CustomAttributeEncoding.Int16;
			}
			if (type == typeof(ushort))
			{
				return CustomAttributeEncoding.UInt16;
			}
			if (type == typeof(uint))
			{
				return CustomAttributeEncoding.UInt32;
			}
			if (type == typeof(long))
			{
				return CustomAttributeEncoding.Int64;
			}
			if (type == typeof(ulong))
			{
				return CustomAttributeEncoding.UInt64;
			}
			if (type == typeof(float))
			{
				return CustomAttributeEncoding.Float;
			}
			if (type == typeof(double))
			{
				return CustomAttributeEncoding.Double;
			}
			if (type.IsClass)
			{
				return CustomAttributeEncoding.Object;
			}
			if (type.IsInterface)
			{
				return CustomAttributeEncoding.Object;
			}
			if (type.IsValueType)
			{
				return CustomAttributeEncoding.Undefined;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidKindOfTypeForCA"), "type");
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0004A51C File Offset: 0x0004951C
		private static CustomAttributeType InitCustomAttributeType(Type parameterType, Module scope)
		{
			CustomAttributeEncoding customAttributeEncoding = CustomAttributeData.TypeToCustomAttributeEncoding(parameterType);
			CustomAttributeEncoding customAttributeEncoding2 = CustomAttributeEncoding.Undefined;
			CustomAttributeEncoding customAttributeEncoding3 = CustomAttributeEncoding.Undefined;
			string text = null;
			if (customAttributeEncoding == CustomAttributeEncoding.Array)
			{
				parameterType = parameterType.GetElementType();
				customAttributeEncoding2 = CustomAttributeData.TypeToCustomAttributeEncoding(parameterType);
			}
			if (customAttributeEncoding == CustomAttributeEncoding.Enum || customAttributeEncoding2 == CustomAttributeEncoding.Enum)
			{
				customAttributeEncoding3 = CustomAttributeData.TypeToCustomAttributeEncoding(Enum.GetUnderlyingType(parameterType));
				if (parameterType.Module == scope)
				{
					text = parameterType.FullName;
				}
				else
				{
					text = parameterType.AssemblyQualifiedName;
				}
			}
			return new CustomAttributeType(customAttributeEncoding, customAttributeEncoding2, customAttributeEncoding3, text);
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x0004A584 File Offset: 0x00049584
		private static IList<CustomAttributeData> GetCustomAttributes(Module module, int tkTarget)
		{
			CustomAttributeRecord[] customAttributeRecords = CustomAttributeData.GetCustomAttributeRecords(module, tkTarget);
			CustomAttributeData[] array = new CustomAttributeData[customAttributeRecords.Length];
			for (int i = 0; i < customAttributeRecords.Length; i++)
			{
				array[i] = new CustomAttributeData(module, customAttributeRecords[i]);
			}
			return Array.AsReadOnly<CustomAttributeData>(array);
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x0004A5CC File Offset: 0x000495CC
		internal unsafe static CustomAttributeRecord[] GetCustomAttributeRecords(Module module, int targetToken)
		{
			MetadataImport metadataImport = module.MetadataImport;
			int num = metadataImport.EnumCustomAttributesCount(targetToken);
			int* ptr = stackalloc int[4 * num];
			metadataImport.EnumCustomAttributes(targetToken, ptr, num);
			CustomAttributeRecord[] array = new CustomAttributeRecord[num];
			for (int i = 0; i < num; i++)
			{
				metadataImport.GetCustomAttributeProps(ptr[i], out array[i].tkCtor.Value, out array[i].blob);
			}
			return array;
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x0004A640 File Offset: 0x00049640
		internal static CustomAttributeTypedArgument Filter(IList<CustomAttributeData> attrs, Type caType, string name)
		{
			for (int i = 0; i < attrs.Count; i++)
			{
				if (attrs[i].Constructor.DeclaringType == caType)
				{
					IList<CustomAttributeNamedArgument> namedArguments = attrs[i].NamedArguments;
					for (int j = 0; j < namedArguments.Count; j++)
					{
						if (namedArguments[j].MemberInfo.Name.Equals(name))
						{
							return namedArguments[j].TypedValue;
						}
					}
				}
			}
			return default(CustomAttributeTypedArgument);
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x0004A6C8 File Offset: 0x000496C8
		internal static CustomAttributeTypedArgument Filter(IList<CustomAttributeData> attrs, Type caType, int parameter)
		{
			for (int i = 0; i < attrs.Count; i++)
			{
				if (attrs[i].Constructor.DeclaringType == caType)
				{
					return attrs[i].ConstructorArguments[parameter];
				}
			}
			return default(CustomAttributeTypedArgument);
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x0004A718 File Offset: 0x00049718
		internal CustomAttributeData(Module scope, CustomAttributeRecord caRecord)
		{
			this.m_scope = scope;
			this.m_ctor = (ConstructorInfo)RuntimeType.GetMethodBase(scope, caRecord.tkCtor);
			ParameterInfo[] parametersNoCopy = this.m_ctor.GetParametersNoCopy();
			this.m_ctorParams = new CustomAttributeCtorParameter[parametersNoCopy.Length];
			for (int i = 0; i < parametersNoCopy.Length; i++)
			{
				this.m_ctorParams[i] = new CustomAttributeCtorParameter(CustomAttributeData.InitCustomAttributeType(parametersNoCopy[i].ParameterType, scope));
			}
			FieldInfo[] fields = this.m_ctor.DeclaringType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			PropertyInfo[] properties = this.m_ctor.DeclaringType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			this.m_namedParams = new CustomAttributeNamedParameter[properties.Length + fields.Length];
			for (int j = 0; j < fields.Length; j++)
			{
				this.m_namedParams[j] = new CustomAttributeNamedParameter(fields[j].Name, CustomAttributeEncoding.Field, CustomAttributeData.InitCustomAttributeType(fields[j].FieldType, scope));
			}
			for (int k = 0; k < properties.Length; k++)
			{
				this.m_namedParams[k + fields.Length] = new CustomAttributeNamedParameter(properties[k].Name, CustomAttributeEncoding.Property, CustomAttributeData.InitCustomAttributeType(properties[k].PropertyType, scope));
			}
			this.m_members = new MemberInfo[fields.Length + properties.Length];
			fields.CopyTo(this.m_members, 0);
			properties.CopyTo(this.m_members, fields.Length);
			CustomAttributeEncodedArgument.ParseAttributeArguments(caRecord.blob, ref this.m_ctorParams, ref this.m_namedParams, this.m_scope);
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x0004A8AC File Offset: 0x000498AC
		internal CustomAttributeData(Attribute attribute)
		{
			if (attribute is DllImportAttribute)
			{
				this.Init((DllImportAttribute)attribute);
				return;
			}
			if (attribute is FieldOffsetAttribute)
			{
				this.Init((FieldOffsetAttribute)attribute);
				return;
			}
			if (attribute is MarshalAsAttribute)
			{
				this.Init((MarshalAsAttribute)attribute);
				return;
			}
			this.Init(attribute);
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x0004A908 File Offset: 0x00049908
		private void Init(DllImportAttribute dllImport)
		{
			Type typeFromHandle = typeof(DllImportAttribute);
			this.m_ctor = typeFromHandle.GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];
			this.m_typedCtorArgs = Array.AsReadOnly<CustomAttributeTypedArgument>(new CustomAttributeTypedArgument[]
			{
				new CustomAttributeTypedArgument(dllImport.Value)
			});
			this.m_namedArgs = Array.AsReadOnly<CustomAttributeNamedArgument>(new CustomAttributeNamedArgument[]
			{
				new CustomAttributeNamedArgument(typeFromHandle.GetField("EntryPoint"), dllImport.EntryPoint),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("CharSet"), dllImport.CharSet),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("ExactSpelling"), dllImport.ExactSpelling),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("SetLastError"), dllImport.SetLastError),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("PreserveSig"), dllImport.PreserveSig),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("CallingConvention"), dllImport.CallingConvention),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("BestFitMapping"), dllImport.BestFitMapping),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("ThrowOnUnmappableChar"), dllImport.ThrowOnUnmappableChar)
			});
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x0004AAA0 File Offset: 0x00049AA0
		private void Init(FieldOffsetAttribute fieldOffset)
		{
			this.m_ctor = typeof(FieldOffsetAttribute).GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];
			this.m_typedCtorArgs = Array.AsReadOnly<CustomAttributeTypedArgument>(new CustomAttributeTypedArgument[]
			{
				new CustomAttributeTypedArgument(fieldOffset.Value)
			});
			this.m_namedArgs = Array.AsReadOnly<CustomAttributeNamedArgument>(new CustomAttributeNamedArgument[0]);
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x0004AB08 File Offset: 0x00049B08
		private void Init(MarshalAsAttribute marshalAs)
		{
			Type typeFromHandle = typeof(MarshalAsAttribute);
			this.m_ctor = typeFromHandle.GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];
			this.m_typedCtorArgs = Array.AsReadOnly<CustomAttributeTypedArgument>(new CustomAttributeTypedArgument[]
			{
				new CustomAttributeTypedArgument(marshalAs.Value)
			});
			int num = 3;
			if (marshalAs.MarshalType != null)
			{
				num++;
			}
			if (marshalAs.MarshalTypeRef != null)
			{
				num++;
			}
			if (marshalAs.MarshalCookie != null)
			{
				num++;
			}
			num++;
			num++;
			if (marshalAs.SafeArrayUserDefinedSubType != null)
			{
				num++;
			}
			CustomAttributeNamedArgument[] array = new CustomAttributeNamedArgument[num];
			num = 0;
			array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("ArraySubType"), marshalAs.ArraySubType);
			array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("SizeParamIndex"), marshalAs.SizeParamIndex);
			array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("SizeConst"), marshalAs.SizeConst);
			array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("IidParameterIndex"), marshalAs.IidParameterIndex);
			array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("SafeArraySubType"), marshalAs.SafeArraySubType);
			if (marshalAs.MarshalType != null)
			{
				array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("MarshalType"), marshalAs.MarshalType);
			}
			if (marshalAs.MarshalTypeRef != null)
			{
				array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("MarshalTypeRef"), marshalAs.MarshalTypeRef);
			}
			if (marshalAs.MarshalCookie != null)
			{
				array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("MarshalCookie"), marshalAs.MarshalCookie);
			}
			if (marshalAs.SafeArrayUserDefinedSubType != null)
			{
				array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("SafeArrayUserDefinedSubType"), marshalAs.SafeArrayUserDefinedSubType);
			}
			this.m_namedArgs = Array.AsReadOnly<CustomAttributeNamedArgument>(array);
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x0004AD3D File Offset: 0x00049D3D
		private void Init(object pca)
		{
			this.m_ctor = pca.GetType().GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];
			this.m_typedCtorArgs = Array.AsReadOnly<CustomAttributeTypedArgument>(new CustomAttributeTypedArgument[0]);
			this.m_namedArgs = Array.AsReadOnly<CustomAttributeNamedArgument>(new CustomAttributeNamedArgument[0]);
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x0004AD78 File Offset: 0x00049D78
		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < this.ConstructorArguments.Count; i++)
			{
				text += string.Format(CultureInfo.CurrentCulture, (i == 0) ? "{0}" : ", {0}", new object[] { this.ConstructorArguments[i] });
			}
			string text2 = "";
			for (int j = 0; j < this.NamedArguments.Count; j++)
			{
				text2 += string.Format(CultureInfo.CurrentCulture, (j == 0 && text.Length == 0) ? "{0}" : ", {0}", new object[] { this.NamedArguments[j] });
			}
			return string.Format(CultureInfo.CurrentCulture, "[{0}({1}{2})]", new object[]
			{
				this.Constructor.DeclaringType.FullName,
				text,
				text2
			});
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x0004AE7A File Offset: 0x00049E7A
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x0004AE82 File Offset: 0x00049E82
		public override bool Equals(object obj)
		{
			return obj == this;
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001DEB RID: 7659 RVA: 0x0004AE88 File Offset: 0x00049E88
		[ComVisible(true)]
		public ConstructorInfo Constructor
		{
			get
			{
				return this.m_ctor;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x0004AE90 File Offset: 0x00049E90
		[ComVisible(true)]
		public IList<CustomAttributeTypedArgument> ConstructorArguments
		{
			get
			{
				if (this.m_typedCtorArgs == null)
				{
					CustomAttributeTypedArgument[] array = new CustomAttributeTypedArgument[this.m_ctorParams.Length];
					for (int i = 0; i < array.Length; i++)
					{
						CustomAttributeEncodedArgument customAttributeEncodedArgument = this.m_ctorParams[i].CustomAttributeEncodedArgument;
						array[i] = new CustomAttributeTypedArgument(this.m_scope, this.m_ctorParams[i].CustomAttributeEncodedArgument);
					}
					this.m_typedCtorArgs = Array.AsReadOnly<CustomAttributeTypedArgument>(array);
				}
				return this.m_typedCtorArgs;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001DED RID: 7661 RVA: 0x0004AF10 File Offset: 0x00049F10
		public IList<CustomAttributeNamedArgument> NamedArguments
		{
			get
			{
				if (this.m_namedArgs == null)
				{
					if (this.m_namedParams == null)
					{
						return null;
					}
					int num = 0;
					for (int i = 0; i < this.m_namedParams.Length; i++)
					{
						if (this.m_namedParams[i].EncodedArgument.CustomAttributeType.EncodedType != CustomAttributeEncoding.Undefined)
						{
							num++;
						}
					}
					CustomAttributeNamedArgument[] array = new CustomAttributeNamedArgument[num];
					int j = 0;
					int num2 = 0;
					while (j < this.m_namedParams.Length)
					{
						if (this.m_namedParams[j].EncodedArgument.CustomAttributeType.EncodedType != CustomAttributeEncoding.Undefined)
						{
							array[num2++] = new CustomAttributeNamedArgument(this.m_members[j], new CustomAttributeTypedArgument(this.m_scope, this.m_namedParams[j].EncodedArgument));
						}
						j++;
					}
					this.m_namedArgs = Array.AsReadOnly<CustomAttributeNamedArgument>(array);
				}
				return this.m_namedArgs;
			}
		}

		// Token: 0x04000ABF RID: 2751
		private ConstructorInfo m_ctor;

		// Token: 0x04000AC0 RID: 2752
		private Module m_scope;

		// Token: 0x04000AC1 RID: 2753
		private MemberInfo[] m_members;

		// Token: 0x04000AC2 RID: 2754
		private CustomAttributeCtorParameter[] m_ctorParams;

		// Token: 0x04000AC3 RID: 2755
		private CustomAttributeNamedParameter[] m_namedParams;

		// Token: 0x04000AC4 RID: 2756
		private IList<CustomAttributeTypedArgument> m_typedCtorArgs;

		// Token: 0x04000AC5 RID: 2757
		private IList<CustomAttributeNamedArgument> m_namedArgs;
	}
}

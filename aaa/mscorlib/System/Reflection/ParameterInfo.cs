using System;
using System.Collections.Generic;
using System.Reflection.Cache;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Reflection
{
	// Token: 0x02000341 RID: 833
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_ParameterInfo))]
	[ComVisible(true)]
	[Serializable]
	public class ParameterInfo : _ParameterInfo, ICustomAttributeProvider
	{
		// Token: 0x06002124 RID: 8484 RVA: 0x000533F0 File Offset: 0x000523F0
		internal static ParameterInfo[] GetParameters(MethodBase method, MemberInfo member, Signature sig)
		{
			ParameterInfo parameterInfo;
			return ParameterInfo.GetParameters(method, member, sig, out parameterInfo, false);
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x00053408 File Offset: 0x00052408
		internal static ParameterInfo GetReturnParameter(MethodBase method, MemberInfo member, Signature sig)
		{
			ParameterInfo parameterInfo;
			ParameterInfo.GetParameters(method, member, sig, out parameterInfo, true);
			return parameterInfo;
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x00053424 File Offset: 0x00052424
		internal unsafe static ParameterInfo[] GetParameters(MethodBase method, MemberInfo member, Signature sig, out ParameterInfo returnParameter, bool fetchReturnParameter)
		{
			RuntimeMethodHandle methodHandle = method.GetMethodHandle();
			returnParameter = null;
			int num = sig.Arguments.Length;
			ParameterInfo[] array = (fetchReturnParameter ? null : new ParameterInfo[num]);
			int methodDef = methodHandle.GetMethodDef();
			int num2 = 0;
			if (!global::System.Reflection.MetadataToken.IsNullToken(methodDef))
			{
				MetadataImport metadataImport = methodHandle.GetDeclaringType().GetModuleHandle().GetMetadataImport();
				num2 = metadataImport.EnumParamsCount(methodDef);
				int* ptr = stackalloc int[4 * num2];
				metadataImport.EnumParams(methodDef, ptr, num2);
				uint num3 = 0U;
				while ((ulong)num3 < (ulong)((long)num2))
				{
					int num4 = ptr[num3];
					int num5;
					ParameterAttributes parameterAttributes;
					metadataImport.GetParamDefProps(num4, out num5, out parameterAttributes);
					num5--;
					if (fetchReturnParameter && num5 == -1)
					{
						returnParameter = new ParameterInfo(sig, metadataImport, num4, num5, parameterAttributes, member);
					}
					else if (!fetchReturnParameter && num5 >= 0)
					{
						array[num5] = new ParameterInfo(sig, metadataImport, num4, num5, parameterAttributes, member);
					}
					num3 += 1U;
				}
			}
			if (fetchReturnParameter)
			{
				if (returnParameter == null)
				{
					returnParameter = new ParameterInfo(sig, MetadataImport.EmptyImport, 0, -1, ParameterAttributes.None, member);
				}
			}
			else if (num2 < array.Length + 1)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == null)
					{
						array[i] = new ParameterInfo(sig, MetadataImport.EmptyImport, 0, i, ParameterAttributes.None, member);
					}
				}
			}
			return array;
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x0005355B File Offset: 0x0005255B
		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			Type parameterType = this.ParameterType;
			string name = this.Name;
			this.DefaultValueImpl = this.DefaultValue;
			this._importer = IntPtr.Zero;
			this._token = this.m_tkParamDef;
			this.bExtraConstChecked = false;
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x00053598 File Offset: 0x00052598
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (this.MemberImpl == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
			}
			MemberTypes memberType = this.MemberImpl.MemberType;
			ParameterInfo parameterInfo;
			if (memberType != MemberTypes.Constructor && memberType != MemberTypes.Method)
			{
				if (memberType != MemberTypes.Property)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_NoParameterInfo"));
				}
				ParameterInfo[] array = ((PropertyInfo)this.MemberImpl).GetIndexParameters();
				if (array == null || this.PositionImpl <= -1 || this.PositionImpl >= array.Length)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_BadParameterInfo"));
				}
				parameterInfo = array[this.PositionImpl];
			}
			else if (this.PositionImpl == -1)
			{
				if (this.MemberImpl.MemberType != MemberTypes.Method)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_BadParameterInfo"));
				}
				parameterInfo = ((MethodInfo)this.MemberImpl).ReturnParameter;
			}
			else
			{
				ParameterInfo[] array = ((MethodBase)this.MemberImpl).GetParametersNoCopy();
				if (array == null || this.PositionImpl >= array.Length)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_BadParameterInfo"));
				}
				parameterInfo = array[this.PositionImpl];
			}
			this.m_tkParamDef = parameterInfo.m_tkParamDef;
			this.m_scope = parameterInfo.m_scope;
			this.m_signature = parameterInfo.m_signature;
			this.m_nameIsCached = true;
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000536D6 File Offset: 0x000526D6
		protected ParameterInfo()
		{
			this.m_nameIsCached = true;
			this.m_noDefaultValue = true;
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000536EE File Offset: 0x000526EE
		internal ParameterInfo(ParameterInfo accessor, RuntimePropertyInfo property)
			: this(accessor, property)
		{
			this.m_signature = property.Signature;
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x00053704 File Offset: 0x00052704
		internal ParameterInfo(ParameterInfo accessor, MethodBuilderInstantiation method)
			: this(accessor, method)
		{
			this.m_signature = accessor.m_signature;
			if (this.ClassImpl.IsGenericParameter)
			{
				this.ClassImpl = method.GetGenericArguments()[this.ClassImpl.GenericParameterPosition];
			}
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x00053740 File Offset: 0x00052740
		private ParameterInfo(ParameterInfo accessor, MemberInfo member)
		{
			this.MemberImpl = member;
			this.NameImpl = accessor.Name;
			this.m_nameIsCached = true;
			this.ClassImpl = accessor.ParameterType;
			this.PositionImpl = accessor.Position;
			this.AttrsImpl = accessor.Attributes;
			this.m_tkParamDef = (global::System.Reflection.MetadataToken.IsNullToken(accessor.MetadataToken) ? 134217728 : accessor.MetadataToken);
			this.m_scope = accessor.m_scope;
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x000537C0 File Offset: 0x000527C0
		private ParameterInfo(Signature signature, MetadataImport scope, int tkParamDef, int position, ParameterAttributes attributes, MemberInfo member)
		{
			this.PositionImpl = position;
			this.MemberImpl = member;
			this.m_signature = signature;
			this.m_tkParamDef = (global::System.Reflection.MetadataToken.IsNullToken(tkParamDef) ? 134217728 : tkParamDef);
			this.m_scope = scope;
			this.AttrsImpl = attributes;
			this.ClassImpl = null;
			this.NameImpl = null;
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x00053820 File Offset: 0x00052820
		internal ParameterInfo(MethodInfo owner, string name, RuntimeType parameterType, int position)
		{
			this.MemberImpl = owner;
			this.NameImpl = name;
			this.m_nameIsCached = true;
			this.m_noDefaultValue = true;
			this.ClassImpl = parameterType;
			this.PositionImpl = position;
			this.AttrsImpl = ParameterAttributes.None;
			this.m_tkParamDef = 134217728;
			this.m_scope = MetadataImport.EmptyImport;
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x0600212F RID: 8495 RVA: 0x0005387D File Offset: 0x0005287D
		private bool IsLegacyParameterInfo
		{
			get
			{
				return base.GetType() != typeof(ParameterInfo);
			}
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x00053894 File Offset: 0x00052894
		internal void SetName(string name)
		{
			this.NameImpl = name;
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x0005389D File Offset: 0x0005289D
		internal void SetAttributes(ParameterAttributes attributes)
		{
			this.AttrsImpl = attributes;
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06002132 RID: 8498 RVA: 0x000538A8 File Offset: 0x000528A8
		public virtual Type ParameterType
		{
			get
			{
				if (this.ClassImpl == null && base.GetType() == typeof(ParameterInfo))
				{
					RuntimeTypeHandle runtimeTypeHandle;
					if (this.PositionImpl == -1)
					{
						runtimeTypeHandle = this.m_signature.ReturnTypeHandle;
					}
					else
					{
						runtimeTypeHandle = this.m_signature.Arguments[this.PositionImpl];
					}
					this.ClassImpl = runtimeTypeHandle.GetRuntimeType();
				}
				return this.ClassImpl;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06002133 RID: 8499 RVA: 0x00053918 File Offset: 0x00052918
		public virtual string Name
		{
			get
			{
				if (!this.m_nameIsCached)
				{
					if (!global::System.Reflection.MetadataToken.IsNullToken(this.m_tkParamDef))
					{
						string text = this.m_scope.GetName(this.m_tkParamDef).ToString();
						this.NameImpl = text;
					}
					this.m_nameIsCached = true;
				}
				return this.NameImpl;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06002134 RID: 8500 RVA: 0x00053972 File Offset: 0x00052972
		public virtual object DefaultValue
		{
			get
			{
				return this.GetDefaultValue(false);
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06002135 RID: 8501 RVA: 0x0005397B File Offset: 0x0005297B
		public virtual object RawDefaultValue
		{
			get
			{
				return this.GetDefaultValue(true);
			}
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x00053984 File Offset: 0x00052984
		internal object GetDefaultValue(bool raw)
		{
			object obj = null;
			if (!this.m_noDefaultValue)
			{
				if (this.ParameterType == typeof(DateTime))
				{
					if (raw)
					{
						CustomAttributeTypedArgument customAttributeTypedArgument = CustomAttributeData.Filter(CustomAttributeData.GetCustomAttributes(this), typeof(DateTimeConstantAttribute), 0);
						if (customAttributeTypedArgument.ArgumentType != null)
						{
							return new DateTime((long)customAttributeTypedArgument.Value);
						}
					}
					else
					{
						object[] customAttributes = this.GetCustomAttributes(typeof(DateTimeConstantAttribute), false);
						if (customAttributes != null && customAttributes.Length != 0)
						{
							return ((DateTimeConstantAttribute)customAttributes[0]).Value;
						}
					}
				}
				if (!global::System.Reflection.MetadataToken.IsNullToken(this.m_tkParamDef))
				{
					obj = MdConstant.GetValue(this.m_scope, this.m_tkParamDef, this.ParameterType.GetTypeHandleInternal(), raw);
				}
				if (obj == DBNull.Value)
				{
					if (raw)
					{
						IList<CustomAttributeData> customAttributes2 = CustomAttributeData.GetCustomAttributes(this);
						CustomAttributeTypedArgument customAttributeTypedArgument2 = CustomAttributeData.Filter(customAttributes2, ParameterInfo.s_CustomConstantAttributeType, "Value");
						if (customAttributeTypedArgument2.ArgumentType == null)
						{
							customAttributeTypedArgument2 = CustomAttributeData.Filter(customAttributes2, ParameterInfo.s_DecimalConstantAttributeType, "Value");
							if (customAttributeTypedArgument2.ArgumentType == null)
							{
								for (int i = 0; i < customAttributes2.Count; i++)
								{
									if (customAttributes2[i].Constructor.DeclaringType == ParameterInfo.s_DecimalConstantAttributeType)
									{
										ParameterInfo[] parameters = customAttributes2[i].Constructor.GetParameters();
										if (parameters.Length != 0)
										{
											if (parameters[2].ParameterType == typeof(uint))
											{
												IList<CustomAttributeTypedArgument> constructorArguments = customAttributes2[i].ConstructorArguments;
												int num = (int)((uint)constructorArguments[4].Value);
												int num2 = (int)((uint)constructorArguments[3].Value);
												int num3 = (int)((uint)constructorArguments[2].Value);
												byte b = (byte)constructorArguments[1].Value;
												byte b2 = (byte)constructorArguments[0].Value;
												customAttributeTypedArgument2 = new CustomAttributeTypedArgument(new decimal(num, num2, num3, b != 0, b2));
											}
											else
											{
												IList<CustomAttributeTypedArgument> constructorArguments2 = customAttributes2[i].ConstructorArguments;
												int num4 = (int)constructorArguments2[4].Value;
												int num5 = (int)constructorArguments2[3].Value;
												int num6 = (int)constructorArguments2[2].Value;
												byte b3 = (byte)constructorArguments2[1].Value;
												byte b4 = (byte)constructorArguments2[0].Value;
												customAttributeTypedArgument2 = new CustomAttributeTypedArgument(new decimal(num4, num5, num6, b3 != 0, b4));
											}
										}
									}
								}
							}
						}
						if (customAttributeTypedArgument2.ArgumentType != null)
						{
							obj = customAttributeTypedArgument2.Value;
						}
					}
					else
					{
						object[] array = this.GetCustomAttributes(ParameterInfo.s_CustomConstantAttributeType, false);
						if (array.Length != 0)
						{
							obj = ((CustomConstantAttribute)array[0]).Value;
						}
						else
						{
							array = this.GetCustomAttributes(ParameterInfo.s_DecimalConstantAttributeType, false);
							if (array.Length != 0)
							{
								obj = ((DecimalConstantAttribute)array[0]).Value;
							}
						}
					}
				}
				if (obj == DBNull.Value && this.IsOptional)
				{
					obj = Type.Missing;
				}
			}
			return obj;
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06002137 RID: 8503 RVA: 0x00053CC9 File Offset: 0x00052CC9
		public virtual int Position
		{
			get
			{
				return this.PositionImpl;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06002138 RID: 8504 RVA: 0x00053CD1 File Offset: 0x00052CD1
		public virtual ParameterAttributes Attributes
		{
			get
			{
				return this.AttrsImpl;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06002139 RID: 8505 RVA: 0x00053CD9 File Offset: 0x00052CD9
		public virtual MemberInfo Member
		{
			get
			{
				return this.MemberImpl;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x0600213A RID: 8506 RVA: 0x00053CE1 File Offset: 0x00052CE1
		public bool IsIn
		{
			get
			{
				return (this.Attributes & ParameterAttributes.In) != ParameterAttributes.None;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x00053CF1 File Offset: 0x00052CF1
		public bool IsOut
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Out) != ParameterAttributes.None;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x0600213C RID: 8508 RVA: 0x00053D01 File Offset: 0x00052D01
		public bool IsLcid
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Lcid) != ParameterAttributes.None;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x0600213D RID: 8509 RVA: 0x00053D11 File Offset: 0x00052D11
		public bool IsRetval
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Retval) != ParameterAttributes.None;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x0600213E RID: 8510 RVA: 0x00053D21 File Offset: 0x00052D21
		public bool IsOptional
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Optional) != ParameterAttributes.None;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x0600213F RID: 8511 RVA: 0x00053D32 File Offset: 0x00052D32
		public int MetadataToken
		{
			get
			{
				return this.m_tkParamDef;
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x00053D3A File Offset: 0x00052D3A
		public virtual Type[] GetRequiredCustomModifiers()
		{
			if (this.IsLegacyParameterInfo)
			{
				return new Type[0];
			}
			return this.m_signature.GetCustomModifiers(this.PositionImpl + 1, true);
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x00053D5F File Offset: 0x00052D5F
		public virtual Type[] GetOptionalCustomModifiers()
		{
			if (this.IsLegacyParameterInfo)
			{
				return new Type[0];
			}
			return this.m_signature.GetCustomModifiers(this.PositionImpl + 1, false);
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x00053D84 File Offset: 0x00052D84
		public override string ToString()
		{
			return this.ParameterType.SigToString() + " " + this.Name;
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x00053DA1 File Offset: 0x00052DA1
		public virtual object[] GetCustomAttributes(bool inherit)
		{
			if (this.IsLegacyParameterInfo)
			{
				return null;
			}
			if (global::System.Reflection.MetadataToken.IsNullToken(this.m_tkParamDef))
			{
				return new object[0];
			}
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x00053DD8 File Offset: 0x00052DD8
		public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (this.IsLegacyParameterInfo)
			{
				return null;
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (global::System.Reflection.MetadataToken.IsNullToken(this.m_tkParamDef))
			{
				return new object[0];
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this, runtimeType);
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x00053E3C File Offset: 0x00052E3C
		public virtual bool IsDefined(Type attributeType, bool inherit)
		{
			if (this.IsLegacyParameterInfo)
			{
				return false;
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (global::System.Reflection.MetadataToken.IsNullToken(this.m_tkParamDef))
			{
				return false;
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.IsDefined(this, runtimeType);
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06002146 RID: 8518 RVA: 0x00053E9C File Offset: 0x00052E9C
		internal InternalCache Cache
		{
			get
			{
				InternalCache internalCache = this.m_cachedData;
				if (internalCache == null)
				{
					internalCache = new InternalCache("ParameterInfo");
					InternalCache internalCache2 = Interlocked.CompareExchange<InternalCache>(ref this.m_cachedData, internalCache, null);
					if (internalCache2 != null)
					{
						internalCache = internalCache2;
					}
					GC.ClearCache += this.OnCacheClear;
				}
				return internalCache;
			}
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x00053EE3 File Offset: 0x00052EE3
		internal void OnCacheClear(object sender, ClearCacheEventArgs cacheEventArgs)
		{
			this.m_cachedData = null;
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x00053EEC File Offset: 0x00052EEC
		void _ParameterInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x00053EF3 File Offset: 0x00052EF3
		void _ParameterInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x00053EFA File Offset: 0x00052EFA
		void _ParameterInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x00053F01 File Offset: 0x00052F01
		void _ParameterInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000DD4 RID: 3540
		private static readonly Type s_DecimalConstantAttributeType = typeof(DecimalConstantAttribute);

		// Token: 0x04000DD5 RID: 3541
		private static readonly Type s_CustomConstantAttributeType = typeof(CustomConstantAttribute);

		// Token: 0x04000DD6 RID: 3542
		private static Type ParameterInfoType = typeof(ParameterInfo);

		// Token: 0x04000DD7 RID: 3543
		protected string NameImpl;

		// Token: 0x04000DD8 RID: 3544
		protected Type ClassImpl;

		// Token: 0x04000DD9 RID: 3545
		protected int PositionImpl;

		// Token: 0x04000DDA RID: 3546
		protected ParameterAttributes AttrsImpl;

		// Token: 0x04000DDB RID: 3547
		protected object DefaultValueImpl;

		// Token: 0x04000DDC RID: 3548
		protected MemberInfo MemberImpl;

		// Token: 0x04000DDD RID: 3549
		private IntPtr _importer;

		// Token: 0x04000DDE RID: 3550
		private int _token;

		// Token: 0x04000DDF RID: 3551
		private bool bExtraConstChecked;

		// Token: 0x04000DE0 RID: 3552
		[NonSerialized]
		private int m_tkParamDef;

		// Token: 0x04000DE1 RID: 3553
		[NonSerialized]
		private MetadataImport m_scope;

		// Token: 0x04000DE2 RID: 3554
		[NonSerialized]
		private Signature m_signature;

		// Token: 0x04000DE3 RID: 3555
		[NonSerialized]
		private volatile bool m_nameIsCached;

		// Token: 0x04000DE4 RID: 3556
		[NonSerialized]
		private readonly bool m_noDefaultValue;

		// Token: 0x04000DE5 RID: 3557
		private InternalCache m_cachedData;

		// Token: 0x02000342 RID: 834
		[Flags]
		private enum WhatIsCached
		{
			// Token: 0x04000DE7 RID: 3559
			Nothing = 0,
			// Token: 0x04000DE8 RID: 3560
			Name = 1,
			// Token: 0x04000DE9 RID: 3561
			ParameterType = 2,
			// Token: 0x04000DEA RID: 3562
			DefaultValue = 4,
			// Token: 0x04000DEB RID: 3563
			All = 7
		}
	}
}

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x0200080F RID: 2063
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_FieldBuilder))]
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class FieldBuilder : FieldInfo, _FieldBuilder
	{
		// Token: 0x060049E1 RID: 18913 RVA: 0x00101AB0 File Offset: 0x00100AB0
		internal FieldBuilder(TypeBuilder typeBuilder, string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
		{
			if (fieldName == null)
			{
				throw new ArgumentNullException("fieldName");
			}
			if (fieldName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "fieldName");
			}
			if (fieldName[0] == '\0')
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalName"), "fieldName");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type == typeof(void))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadFieldType"));
			}
			this.m_fieldName = fieldName;
			this.m_typeBuilder = typeBuilder;
			this.m_fieldType = type;
			this.m_Attributes = attributes & ~FieldAttributes.ReservedMask;
			SignatureHelper fieldSigHelper = SignatureHelper.GetFieldSigHelper(this.m_typeBuilder.Module);
			fieldSigHelper.AddArgument(type, requiredCustomModifiers, optionalCustomModifiers);
			int num;
			byte[] array = fieldSigHelper.InternalGetSignature(out num);
			this.m_fieldTok = TypeBuilder.InternalDefineField(typeBuilder.TypeToken.Token, fieldName, array, num, this.m_Attributes, this.m_typeBuilder.Module);
			this.m_tkField = new FieldToken(this.m_fieldTok, type);
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x00101BBE File Offset: 0x00100BBE
		internal void SetData(byte[] data, int size)
		{
			if (data != null)
			{
				this.m_data = new byte[data.Length];
				Array.Copy(data, this.m_data, data.Length);
			}
			this.m_typeBuilder.Module.InternalSetFieldRVAContent(this.m_tkField.Token, data, size);
		}

		// Token: 0x060049E3 RID: 18915 RVA: 0x00101BFD File Offset: 0x00100BFD
		internal TypeBuilder GetTypeBuilder()
		{
			return this.m_typeBuilder;
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x00101C05 File Offset: 0x00100C05
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_fieldTok;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x060049E5 RID: 18917 RVA: 0x00101C0D File Offset: 0x00100C0D
		public override Module Module
		{
			get
			{
				return this.m_typeBuilder.Module;
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x060049E6 RID: 18918 RVA: 0x00101C1A File Offset: 0x00100C1A
		public override string Name
		{
			get
			{
				return this.m_fieldName;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x060049E7 RID: 18919 RVA: 0x00101C22 File Offset: 0x00100C22
		public override Type DeclaringType
		{
			get
			{
				if (this.m_typeBuilder.m_isHiddenGlobalType)
				{
					return null;
				}
				return this.m_typeBuilder;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x060049E8 RID: 18920 RVA: 0x00101C39 File Offset: 0x00100C39
		public override Type ReflectedType
		{
			get
			{
				if (this.m_typeBuilder.m_isHiddenGlobalType)
				{
					return null;
				}
				return this.m_typeBuilder;
			}
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x060049E9 RID: 18921 RVA: 0x00101C50 File Offset: 0x00100C50
		public override Type FieldType
		{
			get
			{
				return this.m_fieldType;
			}
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x00101C58 File Offset: 0x00100C58
		public override object GetValue(object obj)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x00101C69 File Offset: 0x00100C69
		public override void SetValue(object obj, object val, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x060049EC RID: 18924 RVA: 0x00101C7A File Offset: 0x00100C7A
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x060049ED RID: 18925 RVA: 0x00101C8B File Offset: 0x00100C8B
		public override FieldAttributes Attributes
		{
			get
			{
				return this.m_Attributes;
			}
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x00101C93 File Offset: 0x00100C93
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x00101CA4 File Offset: 0x00100CA4
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x00101CB5 File Offset: 0x00100CB5
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x00101CC6 File Offset: 0x00100CC6
		public FieldToken GetToken()
		{
			return this.m_tkField;
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x00101CD0 File Offset: 0x00100CD0
		public void SetOffset(int iOffset)
		{
			this.m_typeBuilder.ThrowIfCreated();
			TypeBuilder.InternalSetFieldOffset(this.m_typeBuilder.Module, this.GetToken().Token, iOffset);
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x00101D08 File Offset: 0x00100D08
		[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public void SetMarshal(UnmanagedMarshal unmanagedMarshal)
		{
			this.m_typeBuilder.ThrowIfCreated();
			if (unmanagedMarshal == null)
			{
				throw new ArgumentNullException("unmanagedMarshal");
			}
			byte[] array = unmanagedMarshal.InternalGetBytes();
			TypeBuilder.InternalSetMarshalInfo(this.m_typeBuilder.Module, this.GetToken().Token, array, array.Length);
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x00101D58 File Offset: 0x00100D58
		public void SetConstant(object defaultValue)
		{
			this.m_typeBuilder.ThrowIfCreated();
			TypeBuilder.SetConstantValue(this.m_typeBuilder.Module, this.GetToken().Token, this.m_fieldType, defaultValue);
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x00101D98 File Offset: 0x00100D98
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			ModuleBuilder moduleBuilder = this.m_typeBuilder.Module as ModuleBuilder;
			this.m_typeBuilder.ThrowIfCreated();
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			TypeBuilder.InternalCreateCustomAttribute(this.m_tkField.Token, moduleBuilder.GetConstructorToken(con).Token, binaryAttribute, moduleBuilder, false);
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x00101E00 File Offset: 0x00100E00
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			this.m_typeBuilder.ThrowIfCreated();
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			ModuleBuilder moduleBuilder = this.m_typeBuilder.Module as ModuleBuilder;
			customBuilder.CreateCustomAttribute(moduleBuilder, this.m_tkField.Token);
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x00101E49 File Offset: 0x00100E49
		void _FieldBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x00101E50 File Offset: 0x00100E50
		void _FieldBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x00101E57 File Offset: 0x00100E57
		void _FieldBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x00101E5E File Offset: 0x00100E5E
		void _FieldBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002596 RID: 9622
		private int m_fieldTok;

		// Token: 0x04002597 RID: 9623
		private FieldToken m_tkField;

		// Token: 0x04002598 RID: 9624
		private TypeBuilder m_typeBuilder;

		// Token: 0x04002599 RID: 9625
		private string m_fieldName;

		// Token: 0x0400259A RID: 9626
		private FieldAttributes m_Attributes;

		// Token: 0x0400259B RID: 9627
		private Type m_fieldType;

		// Token: 0x0400259C RID: 9628
		internal byte[] m_data;
	}
}

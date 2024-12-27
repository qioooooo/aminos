using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x0200033B RID: 827
	[Serializable]
	internal sealed class MdFieldInfo : RuntimeFieldInfo, ISerializable
	{
		// Token: 0x060020DB RID: 8411 RVA: 0x000523A7 File Offset: 0x000513A7
		internal MdFieldInfo(int tkField, FieldAttributes fieldAttributes, RuntimeTypeHandle declaringTypeHandle, RuntimeType.RuntimeTypeCache reflectedTypeCache, BindingFlags bindingFlags)
			: base(reflectedTypeCache, declaringTypeHandle.GetRuntimeType(), bindingFlags)
		{
			this.m_tkField = tkField;
			this.m_name = null;
			this.m_fieldAttributes = fieldAttributes;
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000523D0 File Offset: 0x000513D0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			MdFieldInfo mdFieldInfo = o as MdFieldInfo;
			return mdFieldInfo != null && mdFieldInfo.m_tkField == this.m_tkField && this.m_declaringType.GetTypeHandleInternal().GetModuleHandle().Equals(mdFieldInfo.m_declaringType.GetTypeHandleInternal().GetModuleHandle());
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060020DD RID: 8413 RVA: 0x00052428 File Offset: 0x00051428
		public override string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = this.Module.MetadataImport.GetName(this.m_tkField).ToString();
				}
				return this.m_name;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060020DE RID: 8414 RVA: 0x00052470 File Offset: 0x00051470
		public override int MetadataToken
		{
			get
			{
				return this.m_tkField;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060020DF RID: 8415 RVA: 0x00052478 File Offset: 0x00051478
		public override Module Module
		{
			get
			{
				return this.m_declaringType.Module;
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x060020E0 RID: 8416 RVA: 0x00052485 File Offset: 0x00051485
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x060020E1 RID: 8417 RVA: 0x0005248C File Offset: 0x0005148C
		public override FieldAttributes Attributes
		{
			get
			{
				return this.m_fieldAttributes;
			}
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x00052494 File Offset: 0x00051494
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object GetValueDirect(TypedReference obj)
		{
			return this.GetValue(null);
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x0005249D File Offset: 0x0005149D
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override void SetValueDirect(TypedReference obj, object value)
		{
			throw new FieldAccessException(Environment.GetResourceString("Acc_ReadOnly"));
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x000524AE File Offset: 0x000514AE
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object GetValue(object obj)
		{
			return this.GetValue(false);
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x000524B7 File Offset: 0x000514B7
		public override object GetRawConstantValue()
		{
			return this.GetValue(true);
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x000524C0 File Offset: 0x000514C0
		internal object GetValue(bool raw)
		{
			object value = MdConstant.GetValue(this.Module.MetadataImport, this.m_tkField, this.FieldType.GetTypeHandleInternal(), raw);
			if (value == DBNull.Value)
			{
				throw new NotSupportedException(Environment.GetResourceString("Arg_EnumLitValueNotFound"));
			}
			return value;
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x00052509 File Offset: 0x00051509
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			throw new FieldAccessException(Environment.GetResourceString("Acc_ReadOnly"));
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x060020E8 RID: 8424 RVA: 0x0005251C File Offset: 0x0005151C
		public override Type FieldType
		{
			get
			{
				if (this.m_fieldType == null)
				{
					ConstArray sigOfFieldDef = this.Module.MetadataImport.GetSigOfFieldDef(this.m_tkField);
					this.m_fieldType = new Signature(sigOfFieldDef.Signature.ToPointer(), sigOfFieldDef.Length, this.m_declaringType.GetTypeHandleInternal()).FieldTypeHandle.GetRuntimeType();
				}
				return this.m_fieldType;
			}
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x0005258A File Offset: 0x0005158A
		public override Type[] GetRequiredCustomModifiers()
		{
			return new Type[0];
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00052592 File Offset: 0x00051592
		public override Type[] GetOptionalCustomModifiers()
		{
			return new Type[0];
		}

		// Token: 0x04000DB5 RID: 3509
		private int m_tkField;

		// Token: 0x04000DB6 RID: 3510
		private string m_name;

		// Token: 0x04000DB7 RID: 3511
		private Type m_fieldType;

		// Token: 0x04000DB8 RID: 3512
		private FieldAttributes m_fieldAttributes;
	}
}

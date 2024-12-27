using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x02000361 RID: 865
	internal sealed class SerializationFieldInfo : FieldInfo
	{
		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x0005731C File Offset: 0x0005631C
		public override Module Module
		{
			get
			{
				return this.m_field.Module;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x0600224C RID: 8780 RVA: 0x00057329 File Offset: 0x00056329
		public override int MetadataToken
		{
			get
			{
				return this.m_field.MetadataToken;
			}
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x00057336 File Offset: 0x00056336
		internal SerializationFieldInfo(RuntimeFieldInfo field, string namePrefix)
		{
			this.m_field = field;
			this.m_serializationName = namePrefix + SerializationFieldInfo.FakeNameSeparatorString + this.m_field.Name;
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x0600224E RID: 8782 RVA: 0x00057361 File Offset: 0x00056361
		public override string Name
		{
			get
			{
				return this.m_serializationName;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x00057369 File Offset: 0x00056369
		public override Type DeclaringType
		{
			get
			{
				return this.m_field.DeclaringType;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06002250 RID: 8784 RVA: 0x00057376 File Offset: 0x00056376
		public override Type ReflectedType
		{
			get
			{
				return this.m_field.ReflectedType;
			}
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x00057383 File Offset: 0x00056383
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_field.GetCustomAttributes(inherit);
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x00057391 File Offset: 0x00056391
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_field.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000573A0 File Offset: 0x000563A0
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_field.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06002254 RID: 8788 RVA: 0x000573AF File Offset: 0x000563AF
		public override Type FieldType
		{
			get
			{
				return this.m_field.FieldType;
			}
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x000573BC File Offset: 0x000563BC
		public override object GetValue(object obj)
		{
			return this.m_field.GetValue(obj);
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x000573CC File Offset: 0x000563CC
		internal object InternalGetValue(object obj, bool requiresAccessCheck)
		{
			RtFieldInfo rtFieldInfo = this.m_field as RtFieldInfo;
			if (rtFieldInfo != null)
			{
				return rtFieldInfo.InternalGetValue(obj, requiresAccessCheck);
			}
			return this.m_field.GetValue(obj);
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x000573FD File Offset: 0x000563FD
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			this.m_field.SetValue(obj, value, invokeAttr, binder, culture);
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x00057414 File Offset: 0x00056414
		internal void InternalSetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture, bool requiresAccessCheck, bool isBinderDefault)
		{
			RtFieldInfo rtFieldInfo = this.m_field as RtFieldInfo;
			if (rtFieldInfo != null)
			{
				rtFieldInfo.InternalSetValue(obj, value, invokeAttr, binder, culture, false);
				return;
			}
			this.m_field.SetValue(obj, value, invokeAttr, binder, culture);
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06002259 RID: 8793 RVA: 0x00057451 File Offset: 0x00056451
		internal RuntimeFieldInfo FieldInfo
		{
			get
			{
				return this.m_field;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x0600225A RID: 8794 RVA: 0x00057459 File Offset: 0x00056459
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				return this.m_field.FieldHandle;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x0600225B RID: 8795 RVA: 0x00057466 File Offset: 0x00056466
		public override FieldAttributes Attributes
		{
			get
			{
				return this.m_field.Attributes;
			}
		}

		// Token: 0x04000E44 RID: 3652
		internal static readonly char FakeNameSeparatorChar = '+';

		// Token: 0x04000E45 RID: 3653
		internal static readonly string FakeNameSeparatorString = "+";

		// Token: 0x04000E46 RID: 3654
		private RuntimeFieldInfo m_field;

		// Token: 0x04000E47 RID: 3655
		private string m_serializationName;
	}
}

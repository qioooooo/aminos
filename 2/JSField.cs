using System;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x0200009A RID: 154
	public abstract class JSField : FieldInfo
	{
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00030C03 File Offset: 0x0002FC03
		public override FieldAttributes Attributes
		{
			get
			{
				return FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x00030C06 File Offset: 0x0002FC06
		public override Type DeclaringType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x00030C09 File Offset: 0x0002FC09
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				return ((FieldInfo)this.GetMetaData()).FieldHandle;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x00030C1B File Offset: 0x0002FC1B
		public override Type FieldType
		{
			get
			{
				return Typeob.Object;
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00030C22 File Offset: 0x0002FC22
		public override object[] GetCustomAttributes(Type t, bool inherit)
		{
			return new FieldInfo[0];
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00030C2A File Offset: 0x0002FC2A
		public override object[] GetCustomAttributes(bool inherit)
		{
			return new FieldInfo[0];
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00030C32 File Offset: 0x0002FC32
		internal virtual object GetMetaData()
		{
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00030C3B File Offset: 0x0002FC3B
		internal virtual string GetClassFullName()
		{
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00030C44 File Offset: 0x0002FC44
		internal virtual PackageScope GetPackage()
		{
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00030C4D File Offset: 0x0002FC4D
		public override bool IsDefined(Type type, bool inherit)
		{
			return false;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00030C50 File Offset: 0x0002FC50
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Field;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00030C53 File Offset: 0x0002FC53
		public override string Name
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x00030C5A File Offset: 0x0002FC5A
		public override Type ReflectedType
		{
			get
			{
				return this.DeclaringType;
			}
		}
	}
}

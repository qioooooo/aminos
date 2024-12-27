using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E8 RID: 1256
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	public sealed class FieldOffsetAttribute : Attribute
	{
		// Token: 0x0600313C RID: 12604 RVA: 0x000A93C4 File Offset: 0x000A83C4
		internal static Attribute GetCustomAttribute(RuntimeFieldInfo field)
		{
			int num;
			if (field.DeclaringType != null && field.Module.MetadataImport.GetFieldOffset(field.DeclaringType.MetadataToken, field.MetadataToken, out num))
			{
				return new FieldOffsetAttribute(num);
			}
			return null;
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x000A9409 File Offset: 0x000A8409
		internal static bool IsDefined(RuntimeFieldInfo field)
		{
			return FieldOffsetAttribute.GetCustomAttribute(field) != null;
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x000A9417 File Offset: 0x000A8417
		public FieldOffsetAttribute(int offset)
		{
			this._val = offset;
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x0600313F RID: 12607 RVA: 0x000A9426 File Offset: 0x000A8426
		public int Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x0400194F RID: 6479
		internal int _val;
	}
}

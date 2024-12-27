using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004CD RID: 1229
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class TypeLibImportClassAttribute : Attribute
	{
		// Token: 0x060030FA RID: 12538 RVA: 0x000A8C6E File Offset: 0x000A7C6E
		public TypeLibImportClassAttribute(Type importClass)
		{
			this._importClassName = importClass.ToString();
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x060030FB RID: 12539 RVA: 0x000A8C82 File Offset: 0x000A7C82
		public string Value
		{
			get
			{
				return this._importClassName;
			}
		}

		// Token: 0x040018AD RID: 6317
		internal string _importClassName;
	}
}

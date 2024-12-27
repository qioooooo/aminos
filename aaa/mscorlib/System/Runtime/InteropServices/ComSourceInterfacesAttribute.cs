using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004D5 RID: 1237
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class ComSourceInterfacesAttribute : Attribute
	{
		// Token: 0x06003107 RID: 12551 RVA: 0x000A8D05 File Offset: 0x000A7D05
		public ComSourceInterfacesAttribute(string sourceInterfaces)
		{
			this._val = sourceInterfaces;
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x000A8D14 File Offset: 0x000A7D14
		public ComSourceInterfacesAttribute(Type sourceInterface)
		{
			this._val = sourceInterface.FullName;
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x000A8D28 File Offset: 0x000A7D28
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2)
		{
			this._val = sourceInterface1.FullName + "\0" + sourceInterface2.FullName;
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x000A8D4C File Offset: 0x000A7D4C
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3)
		{
			this._val = string.Concat(new string[] { sourceInterface1.FullName, "\0", sourceInterface2.FullName, "\0", sourceInterface3.FullName });
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x000A8DA0 File Offset: 0x000A7DA0
		public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3, Type sourceInterface4)
		{
			this._val = string.Concat(new string[] { sourceInterface1.FullName, "\0", sourceInterface2.FullName, "\0", sourceInterface3.FullName, "\0", sourceInterface4.FullName });
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x0600310C RID: 12556 RVA: 0x000A8E03 File Offset: 0x000A7E03
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040018B6 RID: 6326
		internal string _val;
	}
}

using System;

namespace System.Web.Services.Configuration
{
	// Token: 0x0200013B RID: 315
	internal class TypeAndName
	{
		// Token: 0x060009B5 RID: 2485 RVA: 0x00045FB7 File Offset: 0x00044FB7
		public TypeAndName(string name)
		{
			this.type = Type.GetType(name, true, true);
			this.name = name;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x00045FD4 File Offset: 0x00044FD4
		public TypeAndName(Type type)
		{
			this.type = type;
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00045FE3 File Offset: 0x00044FE3
		public override int GetHashCode()
		{
			return this.type.GetHashCode();
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00045FF0 File Offset: 0x00044FF0
		public override bool Equals(object comparand)
		{
			return this.type.Equals(((TypeAndName)comparand).type);
		}

		// Token: 0x04000616 RID: 1558
		public readonly Type type;

		// Token: 0x04000617 RID: 1559
		public readonly string name;
	}
}

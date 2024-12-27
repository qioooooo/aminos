using System;

namespace System.Runtime.Remoting
{
	// Token: 0x0200071E RID: 1822
	[Serializable]
	internal class DynamicTypeInfo : TypeInfo
	{
		// Token: 0x060041B7 RID: 16823 RVA: 0x000E0575 File Offset: 0x000DF575
		internal DynamicTypeInfo(Type typeOfObj)
			: base(typeOfObj)
		{
		}

		// Token: 0x060041B8 RID: 16824 RVA: 0x000E057E File Offset: 0x000DF57E
		public override bool CanCastTo(Type castType, object o)
		{
			return ((MarshalByRefObject)o).IsInstanceOfType(castType);
		}
	}
}

using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002D6 RID: 726
	internal class ArrayModel : TypeModel
	{
		// Token: 0x06002247 RID: 8775 RVA: 0x000A0944 File Offset: 0x0009F944
		internal ArrayModel(Type type, TypeDesc typeDesc, ModelScope scope)
			: base(type, typeDesc, scope)
		{
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06002248 RID: 8776 RVA: 0x000A094F File Offset: 0x0009F94F
		internal TypeModel Element
		{
			get
			{
				return base.ModelScope.GetTypeModel(TypeScope.GetArrayElementType(base.Type, null));
			}
		}
	}
}

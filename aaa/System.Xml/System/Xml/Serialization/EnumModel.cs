using System;
using System.Collections;
using System.Reflection;

namespace System.Xml.Serialization
{
	// Token: 0x020002DD RID: 733
	internal class EnumModel : TypeModel
	{
		// Token: 0x06002260 RID: 8800 RVA: 0x000A0EA7 File Offset: 0x0009FEA7
		internal EnumModel(Type type, TypeDesc typeDesc, ModelScope scope)
			: base(type, typeDesc, scope)
		{
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002261 RID: 8801 RVA: 0x000A0EB4 File Offset: 0x0009FEB4
		internal ConstantModel[] Constants
		{
			get
			{
				if (this.constants == null)
				{
					ArrayList arrayList = new ArrayList();
					foreach (FieldInfo fieldInfo in base.Type.GetFields())
					{
						ConstantModel constantModel = this.GetConstantModel(fieldInfo);
						if (constantModel != null)
						{
							arrayList.Add(constantModel);
						}
					}
					this.constants = (ConstantModel[])arrayList.ToArray(typeof(ConstantModel));
				}
				return this.constants;
			}
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000A0F24 File Offset: 0x0009FF24
		private ConstantModel GetConstantModel(FieldInfo fieldInfo)
		{
			if (fieldInfo.IsSpecialName)
			{
				return null;
			}
			return new ConstantModel(fieldInfo, ((IConvertible)fieldInfo.GetValue(null)).ToInt64(null));
		}

		// Token: 0x040014BF RID: 5311
		private ConstantModel[] constants;
	}
}

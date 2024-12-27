using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x020002D4 RID: 724
	internal class ModelScope
	{
		// Token: 0x0600223E RID: 8766 RVA: 0x000A07AC File Offset: 0x0009F7AC
		internal ModelScope(TypeScope typeScope)
		{
			this.typeScope = typeScope;
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x0600223F RID: 8767 RVA: 0x000A07D1 File Offset: 0x0009F7D1
		internal TypeScope TypeScope
		{
			get
			{
				return this.typeScope;
			}
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000A07D9 File Offset: 0x0009F7D9
		internal TypeModel GetTypeModel(Type type)
		{
			return this.GetTypeModel(type, true);
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x000A07E4 File Offset: 0x0009F7E4
		internal TypeModel GetTypeModel(Type type, bool directReference)
		{
			TypeModel typeModel = (TypeModel)this.models[type];
			if (typeModel != null)
			{
				return typeModel;
			}
			TypeDesc typeDesc = this.typeScope.GetTypeDesc(type, null, directReference);
			switch (typeDesc.Kind)
			{
			case TypeKind.Root:
			case TypeKind.Struct:
			case TypeKind.Class:
				typeModel = new StructModel(type, typeDesc, this);
				break;
			case TypeKind.Primitive:
				typeModel = new PrimitiveModel(type, typeDesc, this);
				break;
			case TypeKind.Enum:
				typeModel = new EnumModel(type, typeDesc, this);
				break;
			case TypeKind.Array:
			case TypeKind.Collection:
			case TypeKind.Enumerable:
				typeModel = new ArrayModel(type, typeDesc, this);
				break;
			default:
				if (!typeDesc.IsSpecial)
				{
					throw new NotSupportedException(Res.GetString("XmlUnsupportedTypeKind", new object[] { type.FullName }));
				}
				typeModel = new SpecialModel(type, typeDesc, this);
				break;
			}
			this.models.Add(type, typeModel);
			return typeModel;
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x000A08B4 File Offset: 0x0009F8B4
		internal ArrayModel GetArrayModel(Type type)
		{
			TypeModel typeModel = (TypeModel)this.arrayModels[type];
			if (typeModel == null)
			{
				typeModel = this.GetTypeModel(type);
				if (!(typeModel is ArrayModel))
				{
					TypeDesc arrayTypeDesc = this.typeScope.GetArrayTypeDesc(type);
					typeModel = new ArrayModel(type, arrayTypeDesc, this);
				}
				this.arrayModels.Add(type, typeModel);
			}
			return (ArrayModel)typeModel;
		}

		// Token: 0x040014AC RID: 5292
		private TypeScope typeScope;

		// Token: 0x040014AD RID: 5293
		private Hashtable models = new Hashtable();

		// Token: 0x040014AE RID: 5294
		private Hashtable arrayModels = new Hashtable();
	}
}

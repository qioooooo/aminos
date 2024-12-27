using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002D5 RID: 725
	internal abstract class TypeModel
	{
		// Token: 0x06002243 RID: 8771 RVA: 0x000A090F File Offset: 0x0009F90F
		protected TypeModel(Type type, TypeDesc typeDesc, ModelScope scope)
		{
			this.scope = scope;
			this.type = type;
			this.typeDesc = typeDesc;
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002244 RID: 8772 RVA: 0x000A092C File Offset: 0x0009F92C
		internal Type Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x000A0934 File Offset: 0x0009F934
		internal ModelScope ModelScope
		{
			get
			{
				return this.scope;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06002246 RID: 8774 RVA: 0x000A093C File Offset: 0x0009F93C
		internal TypeDesc TypeDesc
		{
			get
			{
				return this.typeDesc;
			}
		}

		// Token: 0x040014AF RID: 5295
		private TypeDesc typeDesc;

		// Token: 0x040014B0 RID: 5296
		private Type type;

		// Token: 0x040014B1 RID: 5297
		private ModelScope scope;
	}
}

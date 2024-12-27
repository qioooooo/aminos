using System;

namespace System.Web
{
	// Token: 0x02000093 RID: 147
	internal class HttpStaticObjectsEntry
	{
		// Token: 0x060007B5 RID: 1973 RVA: 0x00022B5F File Offset: 0x00021B5F
		internal HttpStaticObjectsEntry(string name, Type t, bool lateBound)
		{
			this._name = name;
			this._type = t;
			this._lateBound = lateBound;
			this._instance = null;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00022B83 File Offset: 0x00021B83
		internal HttpStaticObjectsEntry(string name, object instance, int dummy)
		{
			this._name = name;
			this._type = instance.GetType();
			this._instance = instance;
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00022BA5 File Offset: 0x00021BA5
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00022BAD File Offset: 0x00021BAD
		internal Type ObjectType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00022BB5 File Offset: 0x00021BB5
		internal bool LateBound
		{
			get
			{
				return this._lateBound;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x00022BBD File Offset: 0x00021BBD
		internal Type DeclaredType
		{
			get
			{
				if (!this._lateBound)
				{
					return this.ObjectType;
				}
				return typeof(object);
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x00022BD8 File Offset: 0x00021BD8
		internal bool HasInstance
		{
			get
			{
				return this._instance != null;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x00022BE8 File Offset: 0x00021BE8
		internal object Instance
		{
			get
			{
				if (this._instance == null)
				{
					lock (this)
					{
						if (this._instance == null)
						{
							this._instance = Activator.CreateInstance(this._type);
						}
					}
				}
				return this._instance;
			}
		}

		// Token: 0x04001161 RID: 4449
		private string _name;

		// Token: 0x04001162 RID: 4450
		private Type _type;

		// Token: 0x04001163 RID: 4451
		private bool _lateBound;

		// Token: 0x04001164 RID: 4452
		private object _instance;
	}
}

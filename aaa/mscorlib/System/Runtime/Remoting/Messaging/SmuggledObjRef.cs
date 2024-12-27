using System;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000716 RID: 1814
	internal class SmuggledObjRef
	{
		// Token: 0x06004185 RID: 16773 RVA: 0x000DFDB3 File Offset: 0x000DEDB3
		public SmuggledObjRef(ObjRef objRef)
		{
			this._objRef = objRef;
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06004186 RID: 16774 RVA: 0x000DFDC2 File Offset: 0x000DEDC2
		public ObjRef ObjRef
		{
			get
			{
				return this._objRef;
			}
		}

		// Token: 0x040020B5 RID: 8373
		private ObjRef _objRef;
	}
}

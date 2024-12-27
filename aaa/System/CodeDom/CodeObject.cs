using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000035 RID: 53
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeObject
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000256 RID: 598 RVA: 0x000123AF File Offset: 0x000113AF
		public IDictionary UserData
		{
			get
			{
				if (this.userData == null)
				{
					this.userData = new ListDictionary();
				}
				return this.userData;
			}
		}

		// Token: 0x040007D4 RID: 2004
		private IDictionary userData;
	}
}

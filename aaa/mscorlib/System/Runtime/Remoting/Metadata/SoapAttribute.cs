using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x02000731 RID: 1841
	[ComVisible(true)]
	public class SoapAttribute : Attribute
	{
		// Token: 0x06004237 RID: 16951 RVA: 0x000E2333 File Offset: 0x000E1333
		internal void SetReflectInfo(object info)
		{
			this.ReflectInfo = info;
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06004238 RID: 16952 RVA: 0x000E233C File Offset: 0x000E133C
		// (set) Token: 0x06004239 RID: 16953 RVA: 0x000E2344 File Offset: 0x000E1344
		public virtual string XmlNamespace
		{
			get
			{
				return this.ProtXmlNamespace;
			}
			set
			{
				this.ProtXmlNamespace = value;
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x0600423A RID: 16954 RVA: 0x000E234D File Offset: 0x000E134D
		// (set) Token: 0x0600423B RID: 16955 RVA: 0x000E2355 File Offset: 0x000E1355
		public virtual bool UseAttribute
		{
			get
			{
				return this._bUseAttribute;
			}
			set
			{
				this._bUseAttribute = value;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x0600423C RID: 16956 RVA: 0x000E235E File Offset: 0x000E135E
		// (set) Token: 0x0600423D RID: 16957 RVA: 0x000E2366 File Offset: 0x000E1366
		public virtual bool Embedded
		{
			get
			{
				return this._bEmbedded;
			}
			set
			{
				this._bEmbedded = value;
			}
		}

		// Token: 0x0400211C RID: 8476
		protected string ProtXmlNamespace;

		// Token: 0x0400211D RID: 8477
		private bool _bUseAttribute;

		// Token: 0x0400211E RID: 8478
		private bool _bEmbedded;

		// Token: 0x0400211F RID: 8479
		protected object ReflectInfo;
	}
}

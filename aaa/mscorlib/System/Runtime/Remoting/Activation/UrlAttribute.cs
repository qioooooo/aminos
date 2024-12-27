using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x02000793 RID: 1939
	[ComVisible(true)]
	[Serializable]
	public sealed class UrlAttribute : ContextAttribute
	{
		// Token: 0x06004569 RID: 17769 RVA: 0x000ECF47 File Offset: 0x000EBF47
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public UrlAttribute(string callsiteURL)
			: base(UrlAttribute.propertyName)
		{
			if (callsiteURL == null)
			{
				throw new ArgumentNullException("callsiteURL");
			}
			this.url = callsiteURL;
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x000ECF69 File Offset: 0x000EBF69
		public override bool Equals(object o)
		{
			return o is IContextProperty && o is UrlAttribute && ((UrlAttribute)o).UrlValue.Equals(this.url);
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x000ECF93 File Offset: 0x000EBF93
		public override int GetHashCode()
		{
			return this.url.GetHashCode();
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x000ECFA0 File Offset: 0x000EBFA0
		[ComVisible(true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override bool IsContextOK(Context ctx, IConstructionCallMessage msg)
		{
			return false;
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x000ECFA3 File Offset: 0x000EBFA3
		[ComVisible(true)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x0600456E RID: 17774 RVA: 0x000ECFA5 File Offset: 0x000EBFA5
		public string UrlValue
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return this.url;
			}
		}

		// Token: 0x04002254 RID: 8788
		private string url;

		// Token: 0x04002255 RID: 8789
		private static string propertyName = "UrlAttribute";
	}
}

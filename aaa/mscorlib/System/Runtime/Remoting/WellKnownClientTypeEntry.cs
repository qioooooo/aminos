using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x0200074A RID: 1866
	[ComVisible(true)]
	public class WellKnownClientTypeEntry : TypeEntry
	{
		// Token: 0x060042DE RID: 17118 RVA: 0x000E5510 File Offset: 0x000E4510
		public WellKnownClientTypeEntry(string typeName, string assemblyName, string objectUrl)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (objectUrl == null)
			{
				throw new ArgumentNullException("objectUrl");
			}
			base.TypeName = typeName;
			base.AssemblyName = assemblyName;
			this._objectUrl = objectUrl;
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x000E5564 File Offset: 0x000E4564
		public WellKnownClientTypeEntry(Type type, string objectUrl)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (objectUrl == null)
			{
				throw new ArgumentNullException("objectUrl");
			}
			base.TypeName = type.FullName;
			base.AssemblyName = type.Module.Assembly.nGetSimpleName();
			this._objectUrl = objectUrl;
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x060042E0 RID: 17120 RVA: 0x000E55BC File Offset: 0x000E45BC
		public string ObjectUrl
		{
			get
			{
				return this._objectUrl;
			}
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x060042E1 RID: 17121 RVA: 0x000E55C4 File Offset: 0x000E45C4
		public Type ObjectType
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				return RuntimeType.PrivateGetType(base.TypeName + ", " + base.AssemblyName, false, false, ref stackCrawlMark);
			}
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x060042E2 RID: 17122 RVA: 0x000E55F2 File Offset: 0x000E45F2
		// (set) Token: 0x060042E3 RID: 17123 RVA: 0x000E55FA File Offset: 0x000E45FA
		public string ApplicationUrl
		{
			get
			{
				return this._appUrl;
			}
			set
			{
				this._appUrl = value;
			}
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x000E5604 File Offset: 0x000E4604
		public override string ToString()
		{
			string text = string.Concat(new string[] { "type='", base.TypeName, ", ", base.AssemblyName, "'; url=", this._objectUrl });
			if (this._appUrl != null)
			{
				text = text + "; appUrl=" + this._appUrl;
			}
			return text;
		}

		// Token: 0x04002179 RID: 8569
		private string _objectUrl;

		// Token: 0x0400217A RID: 8570
		private string _appUrl;
	}
}

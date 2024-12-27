using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x02000748 RID: 1864
	[ComVisible(true)]
	public class ActivatedClientTypeEntry : TypeEntry
	{
		// Token: 0x060042D1 RID: 17105 RVA: 0x000E52E0 File Offset: 0x000E42E0
		public ActivatedClientTypeEntry(string typeName, string assemblyName, string appUrl)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (appUrl == null)
			{
				throw new ArgumentNullException("appUrl");
			}
			base.TypeName = typeName;
			base.AssemblyName = assemblyName;
			this._appUrl = appUrl;
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x000E5334 File Offset: 0x000E4334
		public ActivatedClientTypeEntry(Type type, string appUrl)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (appUrl == null)
			{
				throw new ArgumentNullException("appUrl");
			}
			base.TypeName = type.FullName;
			base.AssemblyName = type.Module.Assembly.nGetSimpleName();
			this._appUrl = appUrl;
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x060042D3 RID: 17107 RVA: 0x000E538C File Offset: 0x000E438C
		public string ApplicationUrl
		{
			get
			{
				return this._appUrl;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x060042D4 RID: 17108 RVA: 0x000E5394 File Offset: 0x000E4394
		public Type ObjectType
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				return RuntimeType.PrivateGetType(base.TypeName + ", " + base.AssemblyName, false, false, ref stackCrawlMark);
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x060042D5 RID: 17109 RVA: 0x000E53C2 File Offset: 0x000E43C2
		// (set) Token: 0x060042D6 RID: 17110 RVA: 0x000E53CA File Offset: 0x000E43CA
		public IContextAttribute[] ContextAttributes
		{
			get
			{
				return this._contextAttributes;
			}
			set
			{
				this._contextAttributes = value;
			}
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x000E53D4 File Offset: 0x000E43D4
		public override string ToString()
		{
			return string.Concat(new string[] { "type='", base.TypeName, ", ", base.AssemblyName, "'; appUrl=", this._appUrl });
		}

		// Token: 0x04002176 RID: 8566
		private string _appUrl;

		// Token: 0x04002177 RID: 8567
		private IContextAttribute[] _contextAttributes;
	}
}

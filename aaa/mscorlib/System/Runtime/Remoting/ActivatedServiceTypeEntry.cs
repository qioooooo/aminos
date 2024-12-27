using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x02000749 RID: 1865
	[ComVisible(true)]
	public class ActivatedServiceTypeEntry : TypeEntry
	{
		// Token: 0x060042D8 RID: 17112 RVA: 0x000E5421 File Offset: 0x000E4421
		public ActivatedServiceTypeEntry(string typeName, string assemblyName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			base.TypeName = typeName;
			base.AssemblyName = assemblyName;
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x000E5453 File Offset: 0x000E4453
		public ActivatedServiceTypeEntry(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			base.TypeName = type.FullName;
			base.AssemblyName = type.Module.Assembly.nGetSimpleName();
		}

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x060042DA RID: 17114 RVA: 0x000E548C File Offset: 0x000E448C
		public Type ObjectType
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				return RuntimeType.PrivateGetType(base.TypeName + ", " + base.AssemblyName, false, false, ref stackCrawlMark);
			}
		}

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x060042DB RID: 17115 RVA: 0x000E54BA File Offset: 0x000E44BA
		// (set) Token: 0x060042DC RID: 17116 RVA: 0x000E54C2 File Offset: 0x000E44C2
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

		// Token: 0x060042DD RID: 17117 RVA: 0x000E54CC File Offset: 0x000E44CC
		public override string ToString()
		{
			return string.Concat(new string[] { "type='", base.TypeName, ", ", base.AssemblyName, "'" });
		}

		// Token: 0x04002178 RID: 8568
		private IContextAttribute[] _contextAttributes;
	}
}

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Threading;

namespace System.Runtime.Remoting
{
	// Token: 0x0200074B RID: 1867
	[ComVisible(true)]
	public class WellKnownServiceTypeEntry : TypeEntry
	{
		// Token: 0x060042E5 RID: 17125 RVA: 0x000E5670 File Offset: 0x000E4670
		public WellKnownServiceTypeEntry(string typeName, string assemblyName, string objectUri, WellKnownObjectMode mode)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (objectUri == null)
			{
				throw new ArgumentNullException("objectUri");
			}
			base.TypeName = typeName;
			base.AssemblyName = assemblyName;
			this._objectUri = objectUri;
			this._mode = mode;
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x000E56CC File Offset: 0x000E46CC
		public WellKnownServiceTypeEntry(Type type, string objectUri, WellKnownObjectMode mode)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (objectUri == null)
			{
				throw new ArgumentNullException("objectUri");
			}
			base.TypeName = type.FullName;
			base.AssemblyName = type.Module.Assembly.FullName;
			this._objectUri = objectUri;
			this._mode = mode;
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x060042E7 RID: 17127 RVA: 0x000E572B File Offset: 0x000E472B
		public string ObjectUri
		{
			get
			{
				return this._objectUri;
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x060042E8 RID: 17128 RVA: 0x000E5733 File Offset: 0x000E4733
		public WellKnownObjectMode Mode
		{
			get
			{
				return this._mode;
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x060042E9 RID: 17129 RVA: 0x000E573C File Offset: 0x000E473C
		public Type ObjectType
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
				return RuntimeType.PrivateGetType(base.TypeName + ", " + base.AssemblyName, false, false, ref stackCrawlMark);
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x060042EA RID: 17130 RVA: 0x000E576A File Offset: 0x000E476A
		// (set) Token: 0x060042EB RID: 17131 RVA: 0x000E5772 File Offset: 0x000E4772
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

		// Token: 0x060042EC RID: 17132 RVA: 0x000E577C File Offset: 0x000E477C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"type='",
				base.TypeName,
				", ",
				base.AssemblyName,
				"'; objectUri=",
				this._objectUri,
				"; mode=",
				this._mode.ToString()
			});
		}

		// Token: 0x0400217B RID: 8571
		private string _objectUri;

		// Token: 0x0400217C RID: 8572
		private WellKnownObjectMode _mode;

		// Token: 0x0400217D RID: 8573
		private IContextAttribute[] _contextAttributes;
	}
}

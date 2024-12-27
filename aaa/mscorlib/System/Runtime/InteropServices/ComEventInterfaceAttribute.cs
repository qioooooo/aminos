using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004ED RID: 1261
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComEventInterfaceAttribute : Attribute
	{
		// Token: 0x06003149 RID: 12617 RVA: 0x000A9499 File Offset: 0x000A8499
		public ComEventInterfaceAttribute(Type SourceInterface, Type EventProvider)
		{
			this._SourceInterface = SourceInterface;
			this._EventProvider = EventProvider;
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x0600314A RID: 12618 RVA: 0x000A94AF File Offset: 0x000A84AF
		public Type SourceInterface
		{
			get
			{
				return this._SourceInterface;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x0600314B RID: 12619 RVA: 0x000A94B7 File Offset: 0x000A84B7
		public Type EventProvider
		{
			get
			{
				return this._EventProvider;
			}
		}

		// Token: 0x04001955 RID: 6485
		internal Type _SourceInterface;

		// Token: 0x04001956 RID: 6486
		internal Type _EventProvider;
	}
}

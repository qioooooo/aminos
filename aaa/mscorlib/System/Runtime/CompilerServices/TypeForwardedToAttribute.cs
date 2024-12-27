using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005F3 RID: 1523
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public sealed class TypeForwardedToAttribute : Attribute
	{
		// Token: 0x060037B4 RID: 14260 RVA: 0x000BB9D5 File Offset: 0x000BA9D5
		public TypeForwardedToAttribute(Type destination)
		{
			this._destination = destination;
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060037B5 RID: 14261 RVA: 0x000BB9E4 File Offset: 0x000BA9E4
		public Type Destination
		{
			get
			{
				return this._destination;
			}
		}

		// Token: 0x04001CB7 RID: 7351
		private Type _destination;
	}
}

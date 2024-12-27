using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005DE RID: 1502
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	internal sealed class TypeDependencyAttribute : Attribute
	{
		// Token: 0x060037AB RID: 14251 RVA: 0x000BB980 File Offset: 0x000BA980
		public TypeDependencyAttribute(string typeName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			this.typeName = typeName;
		}

		// Token: 0x04001CB6 RID: 7350
		private string typeName;
	}
}

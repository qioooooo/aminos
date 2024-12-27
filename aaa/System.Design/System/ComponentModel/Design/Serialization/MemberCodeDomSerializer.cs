using System;
using System.CodeDom;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200015C RID: 348
	public abstract class MemberCodeDomSerializer : CodeDomSerializerBase
	{
		// Token: 0x06000D11 RID: 3345
		public abstract void Serialize(IDesignerSerializationManager manager, object value, MemberDescriptor descriptor, CodeStatementCollection statements);

		// Token: 0x06000D12 RID: 3346
		public abstract bool ShouldSerialize(IDesignerSerializationManager manager, object value, MemberDescriptor descriptor);
	}
}

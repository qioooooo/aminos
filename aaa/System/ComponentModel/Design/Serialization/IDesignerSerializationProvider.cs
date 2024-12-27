using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001AB RID: 427
	public interface IDesignerSerializationProvider
	{
		// Token: 0x06000D29 RID: 3369
		object GetSerializer(IDesignerSerializationManager manager, object currentSerializer, Type objectType, Type serializerType);
	}
}

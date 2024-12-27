using System;
using System.Collections;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001AC RID: 428
	public interface IDesignerSerializationService
	{
		// Token: 0x06000D2A RID: 3370
		ICollection Deserialize(object serializationData);

		// Token: 0x06000D2B RID: 3371
		object Serialize(ICollection objects);
	}
}

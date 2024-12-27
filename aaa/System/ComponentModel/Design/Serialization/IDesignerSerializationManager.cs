using System;
using System.Collections;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001AA RID: 426
	public interface IDesignerSerializationManager : IServiceProvider
	{
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000D1A RID: 3354
		ContextStack Context { get; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000D1B RID: 3355
		PropertyDescriptorCollection Properties { get; }

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06000D1C RID: 3356
		// (remove) Token: 0x06000D1D RID: 3357
		event ResolveNameEventHandler ResolveName;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06000D1E RID: 3358
		// (remove) Token: 0x06000D1F RID: 3359
		event EventHandler SerializationComplete;

		// Token: 0x06000D20 RID: 3360
		void AddSerializationProvider(IDesignerSerializationProvider provider);

		// Token: 0x06000D21 RID: 3361
		object CreateInstance(Type type, ICollection arguments, string name, bool addToContainer);

		// Token: 0x06000D22 RID: 3362
		object GetInstance(string name);

		// Token: 0x06000D23 RID: 3363
		string GetName(object value);

		// Token: 0x06000D24 RID: 3364
		object GetSerializer(Type objectType, Type serializerType);

		// Token: 0x06000D25 RID: 3365
		Type GetType(string typeName);

		// Token: 0x06000D26 RID: 3366
		void RemoveSerializationProvider(IDesignerSerializationProvider provider);

		// Token: 0x06000D27 RID: 3367
		void ReportError(object errorInformation);

		// Token: 0x06000D28 RID: 3368
		void SetName(object instance, string name);
	}
}

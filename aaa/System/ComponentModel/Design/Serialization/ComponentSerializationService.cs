using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001A2 RID: 418
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class ComponentSerializationService
	{
		// Token: 0x06000CF0 RID: 3312
		public abstract SerializationStore CreateStore();

		// Token: 0x06000CF1 RID: 3313
		public abstract SerializationStore LoadStore(Stream stream);

		// Token: 0x06000CF2 RID: 3314
		public abstract void Serialize(SerializationStore store, object value);

		// Token: 0x06000CF3 RID: 3315
		public abstract void SerializeAbsolute(SerializationStore store, object value);

		// Token: 0x06000CF4 RID: 3316
		public abstract void SerializeMember(SerializationStore store, object owningObject, MemberDescriptor member);

		// Token: 0x06000CF5 RID: 3317
		public abstract void SerializeMemberAbsolute(SerializationStore store, object owningObject, MemberDescriptor member);

		// Token: 0x06000CF6 RID: 3318
		public abstract ICollection Deserialize(SerializationStore store);

		// Token: 0x06000CF7 RID: 3319
		public abstract ICollection Deserialize(SerializationStore store, IContainer container);

		// Token: 0x06000CF8 RID: 3320
		public abstract void DeserializeTo(SerializationStore store, IContainer container, bool validateRecycledTypes, bool applyDefaults);

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0002A378 File Offset: 0x00029378
		public void DeserializeTo(SerializationStore store, IContainer container)
		{
			this.DeserializeTo(store, container, true, true);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0002A384 File Offset: 0x00029384
		public void DeserializeTo(SerializationStore store, IContainer container, bool validateRecycledTypes)
		{
			this.DeserializeTo(store, container, validateRecycledTypes, true);
		}
	}
}

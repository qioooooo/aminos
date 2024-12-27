using System;
using System.Collections;
using System.Resources;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200014F RID: 335
	internal sealed class CodeDomSerializationProvider : IDesignerSerializationProvider
	{
		// Token: 0x06000CB3 RID: 3251 RVA: 0x00030ADC File Offset: 0x0002FADC
		private object GetCodeDomSerializer(IDesignerSerializationManager manager, object currentSerializer, Type objectType, Type serializerType)
		{
			if (currentSerializer != null)
			{
				return null;
			}
			if (objectType == null)
			{
				return PrimitiveCodeDomSerializer.Default;
			}
			if (typeof(IComponent).IsAssignableFrom(objectType))
			{
				return ComponentCodeDomSerializer.Default;
			}
			if (typeof(Enum).IsAssignableFrom(objectType))
			{
				return EnumCodeDomSerializer.Default;
			}
			if (objectType.IsPrimitive || objectType.IsEnum || objectType == typeof(string))
			{
				return PrimitiveCodeDomSerializer.Default;
			}
			if (typeof(ICollection).IsAssignableFrom(objectType))
			{
				return CollectionCodeDomSerializer.Default;
			}
			if (typeof(IContainer).IsAssignableFrom(objectType))
			{
				return ContainerCodeDomSerializer.Default;
			}
			if (typeof(ResourceManager).IsAssignableFrom(objectType))
			{
				return ResourceCodeDomSerializer.Default;
			}
			return CodeDomSerializer.Default;
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x00030B97 File Offset: 0x0002FB97
		private object GetMemberCodeDomSerializer(IDesignerSerializationManager manager, object currentSerializer, Type objectType, Type serializerType)
		{
			if (currentSerializer != null)
			{
				return null;
			}
			if (typeof(PropertyDescriptor).IsAssignableFrom(objectType))
			{
				return PropertyMemberCodeDomSerializer.Default;
			}
			if (typeof(EventDescriptor).IsAssignableFrom(objectType))
			{
				return EventMemberCodeDomSerializer.Default;
			}
			return null;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00030BCF File Offset: 0x0002FBCF
		private object GetTypeCodeDomSerializer(IDesignerSerializationManager manager, object currentSerializer, Type objectType, Type serializerType)
		{
			if (currentSerializer != null)
			{
				return null;
			}
			if (typeof(IComponent).IsAssignableFrom(objectType))
			{
				return ComponentTypeCodeDomSerializer.Default;
			}
			return TypeCodeDomSerializer.Default;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00030BF4 File Offset: 0x0002FBF4
		object IDesignerSerializationProvider.GetSerializer(IDesignerSerializationManager manager, object currentSerializer, Type objectType, Type serializerType)
		{
			if (serializerType == typeof(CodeDomSerializer))
			{
				return this.GetCodeDomSerializer(manager, currentSerializer, objectType, serializerType);
			}
			if (serializerType == typeof(MemberCodeDomSerializer))
			{
				return this.GetMemberCodeDomSerializer(manager, currentSerializer, objectType, serializerType);
			}
			if (serializerType == typeof(TypeCodeDomSerializer))
			{
				return this.GetTypeCodeDomSerializer(manager, currentSerializer, objectType, serializerType);
			}
			return null;
		}
	}
}

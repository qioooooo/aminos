using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000742 RID: 1858
	[AttributeUsage(AttributeTargets.Class)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebPartTransformerAttribute : Attribute
	{
		// Token: 0x06005A29 RID: 23081 RVA: 0x0016C14B File Offset: 0x0016B14B
		public WebPartTransformerAttribute(Type consumerType, Type providerType)
		{
			if (consumerType == null)
			{
				throw new ArgumentNullException("consumerType");
			}
			if (providerType == null)
			{
				throw new ArgumentNullException("providerType");
			}
			this._consumerType = consumerType;
			this._providerType = providerType;
		}

		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x06005A2A RID: 23082 RVA: 0x0016C17D File Offset: 0x0016B17D
		public Type ConsumerType
		{
			get
			{
				return this._consumerType;
			}
		}

		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x06005A2B RID: 23083 RVA: 0x0016C185 File Offset: 0x0016B185
		public Type ProviderType
		{
			get
			{
				return this._providerType;
			}
		}

		// Token: 0x06005A2C RID: 23084 RVA: 0x0016C18D File Offset: 0x0016B18D
		public static Type GetConsumerType(Type transformerType)
		{
			return WebPartTransformerAttribute.GetTransformerTypes(transformerType)[0];
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x0016C197 File Offset: 0x0016B197
		public static Type GetProviderType(Type transformerType)
		{
			return WebPartTransformerAttribute.GetTransformerTypes(transformerType)[1];
		}

		// Token: 0x06005A2E RID: 23086 RVA: 0x0016C1A4 File Offset: 0x0016B1A4
		private static Type[] GetTransformerTypes(Type transformerType)
		{
			if (transformerType == null)
			{
				throw new ArgumentNullException("transformerType");
			}
			if (!transformerType.IsSubclassOf(typeof(WebPartTransformer)))
			{
				throw new InvalidOperationException(SR.GetString("WebPartTransformerAttribute_NotTransformer", new object[] { transformerType.FullName }));
			}
			Type[] array = (Type[])WebPartTransformerAttribute.transformerCache[transformerType];
			if (array == null)
			{
				array = WebPartTransformerAttribute.GetTransformerTypesFromAttribute(transformerType);
				WebPartTransformerAttribute.transformerCache[transformerType] = array;
			}
			return array;
		}

		// Token: 0x06005A2F RID: 23087 RVA: 0x0016C21C File Offset: 0x0016B21C
		private static Type[] GetTransformerTypesFromAttribute(Type transformerType)
		{
			Type[] array = new Type[2];
			object[] customAttributes = transformerType.GetCustomAttributes(typeof(WebPartTransformerAttribute), true);
			if (customAttributes.Length != 1)
			{
				throw new InvalidOperationException(SR.GetString("WebPartTransformerAttribute_Missing", new object[] { transformerType.FullName }));
			}
			WebPartTransformerAttribute webPartTransformerAttribute = (WebPartTransformerAttribute)customAttributes[0];
			if (webPartTransformerAttribute.ConsumerType == webPartTransformerAttribute.ProviderType)
			{
				throw new InvalidOperationException(SR.GetString("WebPartTransformerAttribute_SameTypes"));
			}
			array[0] = webPartTransformerAttribute.ConsumerType;
			array[1] = webPartTransformerAttribute.ProviderType;
			return array;
		}

		// Token: 0x04003080 RID: 12416
		private static readonly Hashtable transformerCache = Hashtable.Synchronized(new Hashtable());

		// Token: 0x04003081 RID: 12417
		private Type _consumerType;

		// Token: 0x04003082 RID: 12418
		private Type _providerType;
	}
}

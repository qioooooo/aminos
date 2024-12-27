using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000077 RID: 119
	internal class SoapReflectedExtension : IComparable
	{
		// Token: 0x0600031E RID: 798 RVA: 0x0000E519 File Offset: 0x0000D519
		internal SoapReflectedExtension(Type type, SoapExtensionAttribute attribute)
			: this(type, attribute, attribute.Priority)
		{
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000E52C File Offset: 0x0000D52C
		internal SoapReflectedExtension(Type type, SoapExtensionAttribute attribute, int priority)
		{
			if (priority < 0)
			{
				throw new ArgumentException(Res.GetString("WebConfigInvalidExtensionPriority", new object[] { priority }), "priority");
			}
			this.type = type;
			this.attribute = attribute;
			this.priority = priority;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000E580 File Offset: 0x0000D580
		internal SoapExtension CreateInstance(object initializer)
		{
			SoapExtension soapExtension = (SoapExtension)Activator.CreateInstance(this.type);
			soapExtension.Initialize(initializer);
			return soapExtension;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000E5A8 File Offset: 0x0000D5A8
		internal object GetInitializer(LogicalMethodInfo methodInfo)
		{
			SoapExtension soapExtension = (SoapExtension)Activator.CreateInstance(this.type);
			return soapExtension.GetInitializer(methodInfo, this.attribute);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000E5D4 File Offset: 0x0000D5D4
		internal object GetInitializer(Type serviceType)
		{
			SoapExtension soapExtension = (SoapExtension)Activator.CreateInstance(this.type);
			return soapExtension.GetInitializer(serviceType);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000E5FC File Offset: 0x0000D5FC
		internal static object[] GetInitializers(LogicalMethodInfo methodInfo, SoapReflectedExtension[] extensions)
		{
			object[] array = new object[extensions.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = extensions[i].GetInitializer(methodInfo);
			}
			return array;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000E630 File Offset: 0x0000D630
		internal static object[] GetInitializers(Type serviceType, SoapReflectedExtension[] extensions)
		{
			object[] array = new object[extensions.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = extensions[i].GetInitializer(serviceType);
			}
			return array;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000E661 File Offset: 0x0000D661
		public int CompareTo(object o)
		{
			return this.priority - ((SoapReflectedExtension)o).priority;
		}

		// Token: 0x0400034A RID: 842
		private Type type;

		// Token: 0x0400034B RID: 843
		private SoapExtensionAttribute attribute;

		// Token: 0x0400034C RID: 844
		private int priority;
	}
}

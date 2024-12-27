using System;
using System.Globalization;
using System.Reflection;

namespace System.Net
{
	// Token: 0x0200040E RID: 1038
	internal class WebRequestPrefixElement
	{
		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060020BF RID: 8383 RVA: 0x00080FE4 File Offset: 0x0007FFE4
		// (set) Token: 0x060020C0 RID: 8384 RVA: 0x00081058 File Offset: 0x00080058
		public IWebRequestCreate Creator
		{
			get
			{
				if (this.creator == null && this.creatorType != null)
				{
					lock (this)
					{
						if (this.creator == null)
						{
							this.creator = (IWebRequestCreate)Activator.CreateInstance(this.creatorType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[0], CultureInfo.InvariantCulture);
						}
					}
				}
				return this.creator;
			}
			set
			{
				this.creator = value;
			}
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x00081064 File Offset: 0x00080064
		public WebRequestPrefixElement(string P, Type creatorType)
		{
			if (!typeof(IWebRequestCreate).IsAssignableFrom(creatorType))
			{
				throw new InvalidCastException(SR.GetString("net_invalid_cast", new object[] { creatorType.AssemblyQualifiedName, "IWebRequestCreate" }));
			}
			this.Prefix = P;
			this.creatorType = creatorType;
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x000810C0 File Offset: 0x000800C0
		public WebRequestPrefixElement(string P, IWebRequestCreate C)
		{
			this.Prefix = P;
			this.Creator = C;
		}

		// Token: 0x040020C5 RID: 8389
		public string Prefix;

		// Token: 0x040020C6 RID: 8390
		internal IWebRequestCreate creator;

		// Token: 0x040020C7 RID: 8391
		internal Type creatorType;
	}
}

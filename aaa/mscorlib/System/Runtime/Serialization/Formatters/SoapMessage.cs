using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007A5 RID: 1957
	[ComVisible(true)]
	[Serializable]
	public class SoapMessage : ISoapMessage
	{
		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x0600462D RID: 17965 RVA: 0x000F0476 File Offset: 0x000EF476
		// (set) Token: 0x0600462E RID: 17966 RVA: 0x000F047E File Offset: 0x000EF47E
		public string[] ParamNames
		{
			get
			{
				return this.paramNames;
			}
			set
			{
				this.paramNames = value;
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x0600462F RID: 17967 RVA: 0x000F0487 File Offset: 0x000EF487
		// (set) Token: 0x06004630 RID: 17968 RVA: 0x000F048F File Offset: 0x000EF48F
		public object[] ParamValues
		{
			get
			{
				return this.paramValues;
			}
			set
			{
				this.paramValues = value;
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06004631 RID: 17969 RVA: 0x000F0498 File Offset: 0x000EF498
		// (set) Token: 0x06004632 RID: 17970 RVA: 0x000F04A0 File Offset: 0x000EF4A0
		public Type[] ParamTypes
		{
			get
			{
				return this.paramTypes;
			}
			set
			{
				this.paramTypes = value;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06004633 RID: 17971 RVA: 0x000F04A9 File Offset: 0x000EF4A9
		// (set) Token: 0x06004634 RID: 17972 RVA: 0x000F04B1 File Offset: 0x000EF4B1
		public string MethodName
		{
			get
			{
				return this.methodName;
			}
			set
			{
				this.methodName = value;
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06004635 RID: 17973 RVA: 0x000F04BA File Offset: 0x000EF4BA
		// (set) Token: 0x06004636 RID: 17974 RVA: 0x000F04C2 File Offset: 0x000EF4C2
		public string XmlNameSpace
		{
			get
			{
				return this.xmlNameSpace;
			}
			set
			{
				this.xmlNameSpace = value;
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06004637 RID: 17975 RVA: 0x000F04CB File Offset: 0x000EF4CB
		// (set) Token: 0x06004638 RID: 17976 RVA: 0x000F04D3 File Offset: 0x000EF4D3
		public Header[] Headers
		{
			get
			{
				return this.headers;
			}
			set
			{
				this.headers = value;
			}
		}

		// Token: 0x040022BE RID: 8894
		internal string[] paramNames;

		// Token: 0x040022BF RID: 8895
		internal object[] paramValues;

		// Token: 0x040022C0 RID: 8896
		internal Type[] paramTypes;

		// Token: 0x040022C1 RID: 8897
		internal string methodName;

		// Token: 0x040022C2 RID: 8898
		internal string xmlNameSpace;

		// Token: 0x040022C3 RID: 8899
		internal Header[] headers;
	}
}

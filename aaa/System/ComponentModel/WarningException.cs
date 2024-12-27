using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000156 RID: 342
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class WarningException : SystemException
	{
		// Token: 0x06000B3C RID: 2876 RVA: 0x00027DC9 File Offset: 0x00026DC9
		public WarningException()
			: this(null, null, null)
		{
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00027DD4 File Offset: 0x00026DD4
		public WarningException(string message)
			: this(message, null, null)
		{
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00027DDF File Offset: 0x00026DDF
		public WarningException(string message, string helpUrl)
			: this(message, helpUrl, null)
		{
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00027DEA File Offset: 0x00026DEA
		public WarningException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00027DF4 File Offset: 0x00026DF4
		public WarningException(string message, string helpUrl, string helpTopic)
			: base(message)
		{
			this.helpUrl = helpUrl;
			this.helpTopic = helpTopic;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00027E0C File Offset: 0x00026E0C
		protected WarningException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.helpUrl = (string)info.GetValue("helpUrl", typeof(string));
			this.helpTopic = (string)info.GetValue("helpTopic", typeof(string));
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x00027E61 File Offset: 0x00026E61
		public string HelpUrl
		{
			get
			{
				return this.helpUrl;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x00027E69 File Offset: 0x00026E69
		public string HelpTopic
		{
			get
			{
				return this.helpTopic;
			}
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00027E71 File Offset: 0x00026E71
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("helpUrl", this.helpUrl);
			info.AddValue("helpTopic", this.helpTopic);
			base.GetObjectData(info, context);
		}

		// Token: 0x04000A9C RID: 2716
		private readonly string helpUrl;

		// Token: 0x04000A9D RID: 2717
		private readonly string helpTopic;
	}
}

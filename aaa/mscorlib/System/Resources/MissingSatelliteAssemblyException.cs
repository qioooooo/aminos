using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Resources
{
	// Token: 0x0200041D RID: 1053
	[ComVisible(true)]
	[Serializable]
	public class MissingSatelliteAssemblyException : SystemException
	{
		// Token: 0x06002B81 RID: 11137 RVA: 0x000925FC File Offset: 0x000915FC
		public MissingSatelliteAssemblyException()
			: base(Environment.GetResourceString("MissingSatelliteAssembly_Default"))
		{
			base.SetErrorCode(-2146233034);
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x00092619 File Offset: 0x00091619
		public MissingSatelliteAssemblyException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233034);
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x0009262D File Offset: 0x0009162D
		public MissingSatelliteAssemblyException(string message, string cultureName)
			: base(message)
		{
			base.SetErrorCode(-2146233034);
			this._cultureName = cultureName;
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x00092648 File Offset: 0x00091648
		public MissingSatelliteAssemblyException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233034);
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x0009265D File Offset: 0x0009165D
		protected MissingSatelliteAssemblyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002B86 RID: 11142 RVA: 0x00092667 File Offset: 0x00091667
		public string CultureName
		{
			get
			{
				return this._cultureName;
			}
		}

		// Token: 0x04001509 RID: 5385
		private string _cultureName;
	}
}

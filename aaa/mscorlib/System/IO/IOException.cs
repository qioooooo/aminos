using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x02000598 RID: 1432
	[ComVisible(true)]
	[Serializable]
	public class IOException : SystemException
	{
		// Token: 0x06003541 RID: 13633 RVA: 0x000B260A File Offset: 0x000B160A
		public IOException()
			: base(Environment.GetResourceString("Arg_IOException"))
		{
			base.SetErrorCode(-2146232800);
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x000B2627 File Offset: 0x000B1627
		public IOException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146232800);
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x000B263B File Offset: 0x000B163B
		public IOException(string message, int hresult)
			: base(message)
		{
			base.SetErrorCode(hresult);
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x000B264B File Offset: 0x000B164B
		internal IOException(string message, int hresult, string maybeFullPath)
			: base(message)
		{
			base.SetErrorCode(hresult);
			this._maybeFullPath = maybeFullPath;
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x000B2662 File Offset: 0x000B1662
		public IOException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146232800);
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x000B2677 File Offset: 0x000B1677
		protected IOException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04001BD3 RID: 7123
		[NonSerialized]
		private string _maybeFullPath;
	}
}

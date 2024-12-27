using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Data.OleDb
{
	// Token: 0x02000227 RID: 551
	[Serializable]
	public sealed class OleDbException : DbException
	{
		// Token: 0x06001F9D RID: 8093 RVA: 0x0025E5A0 File Offset: 0x0025D9A0
		internal OleDbException(string message, OleDbHResult errorCode, Exception inner)
			: base(message, inner)
		{
			base.HResult = (int)errorCode;
			this.oledbErrors = new OleDbErrorCollection(null);
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x0025E5C8 File Offset: 0x0025D9C8
		internal OleDbException(OleDbException previous, Exception inner)
			: base(previous.Message, inner)
		{
			base.HResult = previous.ErrorCode;
			this.oledbErrors = previous.oledbErrors;
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x0025E5FC File Offset: 0x0025D9FC
		private OleDbException(string message, Exception inner, string source, OleDbHResult errorCode, OleDbErrorCollection errors)
			: base(message, inner)
		{
			this.Source = source;
			base.HResult = (int)errorCode;
			this.oledbErrors = errors;
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x0025E628 File Offset: 0x0025DA28
		private OleDbException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
			this.oledbErrors = (OleDbErrorCollection)si.GetValue("oledbErrors", typeof(OleDbErrorCollection));
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x0025E660 File Offset: 0x0025DA60
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			if (si == null)
			{
				throw new ArgumentNullException("si");
			}
			si.AddValue("oledbErrors", this.oledbErrors, typeof(OleDbErrorCollection));
			base.GetObjectData(si, context);
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x0025E6A0 File Offset: 0x0025DAA0
		[TypeConverter(typeof(OleDbException.ErrorCodeConverter))]
		public override int ErrorCode
		{
			get
			{
				return base.ErrorCode;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x0025E6B4 File Offset: 0x0025DAB4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public OleDbErrorCollection Errors
		{
			get
			{
				OleDbErrorCollection oleDbErrorCollection = this.oledbErrors;
				if (oleDbErrorCollection == null)
				{
					return new OleDbErrorCollection(null);
				}
				return oleDbErrorCollection;
			}
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0025E6D4 File Offset: 0x0025DAD4
		internal bool ShouldSerializeErrors()
		{
			OleDbErrorCollection oleDbErrorCollection = this.oledbErrors;
			return oleDbErrorCollection != null && 0 < oleDbErrorCollection.Count;
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0025E6F8 File Offset: 0x0025DAF8
		internal static OleDbException CreateException(UnsafeNativeMethods.IErrorInfo errorInfo, OleDbHResult errorCode, Exception inner)
		{
			OleDbErrorCollection oleDbErrorCollection = new OleDbErrorCollection(errorInfo);
			string text = null;
			string text2 = null;
			if (errorInfo != null)
			{
				OleDbHResult oleDbHResult = errorInfo.GetDescription(out text);
				Bid.Trace("<oledb.IErrorInfo.GetDescription|API|OS|RET> %08X{HRESULT}, Description='%ls'\n", oleDbHResult, text);
				oleDbHResult = errorInfo.GetSource(out text2);
				Bid.Trace("<oledb.IErrorInfo.GetSource|API|OS|RET> %08X{HRESULT}, Source='%ls'\n", oleDbHResult, text2);
			}
			int count = oleDbErrorCollection.Count;
			if (0 < oleDbErrorCollection.Count)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (text != null && text != oleDbErrorCollection[0].Message)
				{
					stringBuilder.Append(text.TrimEnd(ODB.ErrorTrimCharacters));
					if (1 < count)
					{
						stringBuilder.Append(Environment.NewLine);
					}
				}
				for (int i = 0; i < count; i++)
				{
					if (0 < i)
					{
						stringBuilder.Append(Environment.NewLine);
					}
					stringBuilder.Append(oleDbErrorCollection[i].Message.TrimEnd(ODB.ErrorTrimCharacters));
				}
				text = stringBuilder.ToString();
			}
			if (ADP.IsEmpty(text))
			{
				text = ODB.NoErrorMessage(errorCode);
			}
			return new OleDbException(text, inner, text2, errorCode, oleDbErrorCollection);
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0025E7F8 File Offset: 0x0025DBF8
		internal static OleDbException CombineExceptions(List<OleDbException> exceptions)
		{
			if (1 < exceptions.Count)
			{
				OleDbErrorCollection oleDbErrorCollection = new OleDbErrorCollection(null);
				StringBuilder stringBuilder = new StringBuilder();
				foreach (OleDbException ex in exceptions)
				{
					oleDbErrorCollection.AddRange(ex.Errors);
					stringBuilder.Append(ex.Message);
					stringBuilder.Append(Environment.NewLine);
				}
				return new OleDbException(stringBuilder.ToString(), null, exceptions[0].Source, (OleDbHResult)exceptions[0].ErrorCode, oleDbErrorCollection);
			}
			return exceptions[0];
		}

		// Token: 0x040012E6 RID: 4838
		private OleDbErrorCollection oledbErrors;

		// Token: 0x02000228 RID: 552
		internal sealed class ErrorCodeConverter : Int32Converter
		{
			// Token: 0x06001FA8 RID: 8104 RVA: 0x0025E8CC File Offset: 0x0025DCCC
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw ADP.ArgumentNull("destinationType");
				}
				if (destinationType == typeof(string) && value != null && value is int)
				{
					return ODB.ELookup((OleDbHResult)value);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
	}
}

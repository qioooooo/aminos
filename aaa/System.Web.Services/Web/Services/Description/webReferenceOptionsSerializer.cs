using System;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x0200012C RID: 300
	internal sealed class webReferenceOptionsSerializer : XmlSerializer
	{
		// Token: 0x0600092A RID: 2346 RVA: 0x00043152 File Offset: 0x00042152
		protected override XmlSerializationReader CreateReader()
		{
			return new WebReferenceOptionsSerializationReader();
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00043159 File Offset: 0x00042159
		protected override XmlSerializationWriter CreateWriter()
		{
			return new WebReferenceOptionsSerializationWriter();
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00043160 File Offset: 0x00042160
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return true;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00043163 File Offset: 0x00042163
		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((WebReferenceOptionsSerializationWriter)writer).Write5_webReferenceOptions(objectToSerialize);
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00043171 File Offset: 0x00042171
		protected override object Deserialize(XmlSerializationReader reader)
		{
			return ((WebReferenceOptionsSerializationReader)reader).Read5_webReferenceOptions();
		}
	}
}

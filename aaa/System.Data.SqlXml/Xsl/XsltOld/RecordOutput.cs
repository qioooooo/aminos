using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000182 RID: 386
	internal interface RecordOutput
	{
		// Token: 0x06001036 RID: 4150
		Processor.OutputResult RecordDone(RecordBuilder record);

		// Token: 0x06001037 RID: 4151
		void TheEnd();
	}
}

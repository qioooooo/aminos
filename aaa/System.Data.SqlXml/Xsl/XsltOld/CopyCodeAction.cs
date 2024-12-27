using System;
using System.Collections;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000146 RID: 326
	internal class CopyCodeAction : Action
	{
		// Token: 0x06000E59 RID: 3673 RVA: 0x00049607 File Offset: 0x00048607
		internal CopyCodeAction()
		{
			this.copyEvents = new ArrayList();
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0004961A File Offset: 0x0004861A
		internal void AddEvent(Event copyEvent)
		{
			this.copyEvents.Add(copyEvent);
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00049629 File Offset: 0x00048629
		internal void AddEvents(ArrayList copyEvents)
		{
			this.copyEvents.AddRange(copyEvents);
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00049638 File Offset: 0x00048638
		internal override void ReplaceNamespaceAlias(Compiler compiler)
		{
			int count = this.copyEvents.Count;
			for (int i = 0; i < count; i++)
			{
				((Event)this.copyEvents[i]).ReplaceNamespaceAlias(compiler);
			}
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00049674 File Offset: 0x00048674
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				frame.Counter = 0;
				frame.State = 2;
				break;
			case 1:
				return;
			case 2:
				break;
			default:
				return;
			}
			while (processor.CanContinue)
			{
				Event @event = (Event)this.copyEvents[frame.Counter];
				if (!@event.Output(processor, frame))
				{
					return;
				}
				if (frame.IncrementCounter() >= this.copyEvents.Count)
				{
					frame.Finished();
					return;
				}
			}
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x000496EF File Offset: 0x000486EF
		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return ((Event)this.copyEvents[frame.Counter]).DbgData;
		}

		// Token: 0x04000950 RID: 2384
		private const int Outputting = 2;

		// Token: 0x04000951 RID: 2385
		private ArrayList copyEvents;
	}
}

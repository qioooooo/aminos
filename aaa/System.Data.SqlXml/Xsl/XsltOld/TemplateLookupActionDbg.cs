using System;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000198 RID: 408
	internal class TemplateLookupActionDbg : TemplateLookupAction
	{
		// Token: 0x0600116C RID: 4460 RVA: 0x00054288 File Offset: 0x00053288
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (this.mode == Compiler.BuiltInMode)
			{
				this.mode = processor.GetPrevioseMode();
			}
			processor.SetCurrentMode(this.mode);
			Action action;
			if (this.mode != null)
			{
				action = ((this.importsOf == null) ? processor.Stylesheet.FindTemplate(processor, frame.Node, this.mode) : this.importsOf.FindTemplateImports(processor, frame.Node, this.mode));
			}
			else
			{
				action = ((this.importsOf == null) ? processor.Stylesheet.FindTemplate(processor, frame.Node) : this.importsOf.FindTemplateImports(processor, frame.Node));
			}
			if (action == null && processor.RootAction.builtInSheet != null)
			{
				action = processor.RootAction.builtInSheet.FindTemplate(processor, frame.Node, Compiler.BuiltInMode);
			}
			if (action == null)
			{
				action = base.BuiltInTemplate(frame.Node);
			}
			if (action != null)
			{
				frame.SetAction(action);
				return;
			}
			frame.Finished();
		}
	}
}

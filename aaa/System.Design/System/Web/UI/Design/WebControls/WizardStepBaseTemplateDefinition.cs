using System;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000502 RID: 1282
	internal class WizardStepBaseTemplateDefinition : TemplateDefinition
	{
		// Token: 0x06002DBD RID: 11709 RVA: 0x00103646 File Offset: 0x00102646
		public WizardStepBaseTemplateDefinition(WizardDesigner designer, WizardStepBase step, string name, Style style)
			: base(designer, name, step, name, style)
		{

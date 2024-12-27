using System;
using System.Design;

namespace System.Web.UI.Design
{
	// Token: 0x02000397 RID: 919
	public class TemplatedEditableDesignerRegion : EditableDesignerRegion
	{
		// Token: 0x060021EF RID: 8687 RVA: 0x000BAAA7 File Offset: 0x000B9AA7
		public TemplatedEditableDesignerRegion(TemplateDefinition templateDefinition)
			: base(templateDefinition.Designer, templateDefinition.Name, templateDefinition.ServerControlsOnly)
		{
			this._templateDefinition = templateDefinition;
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x060021F0 RID: 8688 RVA: 0x000BAAC8 File Offset: 0x000B9AC8
		// (set) Token: 0x060021F1 RID: 8689 RVA: 0x000BAAD0 File Offset: 0x000B9AD0
		public virtual bool IsSingleInstanceTemplate
		{
			get
			{
				return this._isSingleInstance;
			}
			set
			{
				this._isSingleInstance = value;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060021F2 RID: 8690 RVA: 0x000BAAD9 File Offset: 0x000B9AD9
		// (set) Token: 0x060021F3 RID: 8691 RVA: 0x000BAAE6 File Offset: 0x000B9AE6
		public override bool SupportsDataBinding
		{
			get
			{
				return this._templateDefinition.SupportsDataBinding;
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("TemplateEditableDesignerRegion_CannotSetSupportsDataBinding"));
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x000BAAF7 File Offset: 0x000B9AF7
		public TemplateDefinition TemplateDefinition
		{
			get
			{
				return this._templateDefinition;
			}
		}

		// Token: 0x0400182C RID: 6188
		private TemplateDefinition _templateDefinition;

		// Token: 0x0400182D RID: 6189
		private bool _isSingleInstance;
	}
}

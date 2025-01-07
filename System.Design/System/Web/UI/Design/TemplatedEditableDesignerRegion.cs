using System;
using System.Design;

namespace System.Web.UI.Design
{
	public class TemplatedEditableDesignerRegion : EditableDesignerRegion
	{
		public TemplatedEditableDesignerRegion(TemplateDefinition templateDefinition)
			: base(templateDefinition.Designer, templateDefinition.Name, templateDefinition.ServerControlsOnly)
		{
			this._templateDefinition = templateDefinition;
		}

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

		public TemplateDefinition TemplateDefinition
		{
			get
			{
				return this._templateDefinition;
			}
		}

		private TemplateDefinition _templateDefinition;

		private bool _isSingleInstance;
	}
}

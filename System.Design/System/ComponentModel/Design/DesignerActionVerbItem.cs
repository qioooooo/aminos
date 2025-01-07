using System;

namespace System.ComponentModel.Design
{
	internal class DesignerActionVerbItem : DesignerActionMethodItem
	{
		public DesignerActionVerbItem(DesignerVerb verb)
		{
			if (verb == null)
			{
				throw new ArgumentNullException();
			}
			this._targetVerb = verb;
		}

		public override string Category
		{
			get
			{
				return "Verbs";
			}
		}

		public override string Description
		{
			get
			{
				return this._targetVerb.Description;
			}
		}

		public override string DisplayName
		{
			get
			{
				return this._targetVerb.Text;
			}
		}

		public override string MemberName
		{
			get
			{
				return null;
			}
		}

		public override bool IncludeAsDesignerVerb
		{
			get
			{
				return false;
			}
		}

		public override void Invoke()
		{
			this._targetVerb.Invoke();
		}

		private DesignerVerb _targetVerb;
	}
}

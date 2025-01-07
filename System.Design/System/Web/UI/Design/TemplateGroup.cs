using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	public class TemplateGroup
	{
		public TemplateGroup(string groupName)
			: this(groupName, null)
		{
		}

		public TemplateGroup(string groupName, Style groupStyle)
		{
			this._groupName = groupName;
			this._groupStyle = groupStyle;
		}

		public bool IsEmpty
		{
			get
			{
				return this._templates == null || this._templates.Count == 0;
			}
		}

		public string GroupName
		{
			get
			{
				return this._groupName;
			}
		}

		public Style GroupStyle
		{
			get
			{
				return this._groupStyle;
			}
		}

		public TemplateDefinition[] Templates
		{
			get
			{
				if (this._templates == null)
				{
					return TemplateGroup.emptyTemplateDefinitionArray;
				}
				return (TemplateDefinition[])this._templates.ToArray(typeof(TemplateDefinition));
			}
		}

		public void AddTemplateDefinition(TemplateDefinition templateDefinition)
		{
			if (this._templates == null)
			{
				this._templates = new ArrayList();
			}
			this._templates.Add(templateDefinition);
		}

		private static TemplateDefinition[] emptyTemplateDefinitionArray = new TemplateDefinition[0];

		private string _groupName;

		private Style _groupStyle;

		private ArrayList _templates;
	}
}

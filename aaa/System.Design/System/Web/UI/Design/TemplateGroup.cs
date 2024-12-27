using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x02000394 RID: 916
	public class TemplateGroup
	{
		// Token: 0x060021C7 RID: 8647 RVA: 0x000BA7D4 File Offset: 0x000B97D4
		public TemplateGroup(string groupName)
			: this(groupName, null)
		{
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x000BA7DE File Offset: 0x000B97DE
		public TemplateGroup(string groupName, Style groupStyle)
		{
			this._groupName = groupName;
			this._groupStyle = groupStyle;
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x060021C9 RID: 8649 RVA: 0x000BA7F4 File Offset: 0x000B97F4
		public bool IsEmpty
		{
			get
			{
				return this._templates == null || this._templates.Count == 0;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x000BA80E File Offset: 0x000B980E
		public string GroupName
		{
			get
			{
				return this._groupName;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x000BA816 File Offset: 0x000B9816
		public Style GroupStyle
		{
			get
			{
				return this._groupStyle;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x060021CC RID: 8652 RVA: 0x000BA81E File Offset: 0x000B981E
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

		// Token: 0x060021CD RID: 8653 RVA: 0x000BA848 File Offset: 0x000B9848
		public void AddTemplateDefinition(TemplateDefinition templateDefinition)
		{
			if (this._templates == null)
			{
				this._templates = new ArrayList();
			}
			this._templates.Add(templateDefinition);
		}

		// Token: 0x04001825 RID: 6181
		private static TemplateDefinition[] emptyTemplateDefinitionArray = new TemplateDefinition[0];

		// Token: 0x04001826 RID: 6182
		private string _groupName;

		// Token: 0x04001827 RID: 6183
		private Style _groupStyle;

		// Token: 0x04001828 RID: 6184
		private ArrayList _templates;
	}
}

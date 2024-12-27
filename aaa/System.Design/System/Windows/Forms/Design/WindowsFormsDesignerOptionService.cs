using System;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002E1 RID: 737
	public class WindowsFormsDesignerOptionService : DesignerOptionService
	{
		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001C56 RID: 7254 RVA: 0x0009F6ED File Offset: 0x0009E6ED
		public virtual DesignerOptions CompatibilityOptions
		{
			get
			{
				if (this._options == null)
				{
					this._options = new DesignerOptions();
				}
				return this._options;
			}
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x0009F708 File Offset: 0x0009E708
		protected override void PopulateOptionCollection(DesignerOptionService.DesignerOptionCollection options)
		{
			if (options.Parent == null)
			{
				DesignerOptions compatibilityOptions = this.CompatibilityOptions;
				if (compatibilityOptions != null)
				{
					base.CreateOptionCollection(options, "DesignerOptions", compatibilityOptions);
				}
			}
		}

		// Token: 0x040015DF RID: 5599
		private DesignerOptions _options;
	}
}

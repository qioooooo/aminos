using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002DA RID: 730
	internal class PropertyGridSite : ISite, IServiceProvider
	{
		// Token: 0x06001C33 RID: 7219 RVA: 0x0009F0FC File Offset: 0x0009E0FC
		public PropertyGridSite(IServiceProvider sp, IComponent comp)
		{
			this.sp = sp;
			this.comp = comp;
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001C34 RID: 7220 RVA: 0x0009F112 File Offset: 0x0009E112
		public IComponent Component
		{
			get
			{
				return this.comp;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001C35 RID: 7221 RVA: 0x0009F11A File Offset: 0x0009E11A
		public IContainer Container
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001C36 RID: 7222 RVA: 0x0009F11D File Offset: 0x0009E11D
		public bool DesignMode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001C37 RID: 7223 RVA: 0x0009F120 File Offset: 0x0009E120
		// (set) Token: 0x06001C38 RID: 7224 RVA: 0x0009F123 File Offset: 0x0009E123
		public string Name
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x0009F128 File Offset: 0x0009E128
		public object GetService(Type t)
		{
			if (!this.inGetService && this.sp != null)
			{
				try
				{
					this.inGetService = true;
					return this.sp.GetService(t);
				}
				finally
				{
					this.inGetService = false;
				}
			}
			return null;
		}

		// Token: 0x040015D8 RID: 5592
		private IServiceProvider sp;

		// Token: 0x040015D9 RID: 5593
		private IComponent comp;

		// Token: 0x040015DA RID: 5594
		private bool inGetService;
	}
}

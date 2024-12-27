using System;

namespace System.ComponentModel
{
	// Token: 0x0200013B RID: 315
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SettingsBindableAttribute : Attribute
	{
		// Token: 0x06000A4A RID: 2634 RVA: 0x00023DF5 File Offset: 0x00022DF5
		public SettingsBindableAttribute(bool bindable)
		{
			this._bindable = bindable;
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x00023E04 File Offset: 0x00022E04
		public bool Bindable
		{
			get
			{
				return this._bindable;
			}
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00023E0C File Offset: 0x00022E0C
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is SettingsBindableAttribute && ((SettingsBindableAttribute)obj).Bindable == this._bindable);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00023E34 File Offset: 0x00022E34
		public override int GetHashCode()
		{
			return this._bindable.GetHashCode();
		}

		// Token: 0x04000A69 RID: 2665
		public static readonly SettingsBindableAttribute Yes = new SettingsBindableAttribute(true);

		// Token: 0x04000A6A RID: 2666
		public static readonly SettingsBindableAttribute No = new SettingsBindableAttribute(false);

		// Token: 0x04000A6B RID: 2667
		private bool _bindable;
	}
}

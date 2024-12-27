using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x0200023B RID: 571
	public class BindingCompleteEventArgs : CancelEventArgs
	{
		// Token: 0x06001B4F RID: 6991 RVA: 0x00035215 File Offset: 0x00034215
		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context, string errorText, Exception exception, bool cancel)
			: base(cancel)
		{
			this.binding = binding;
			this.state = state;
			this.context = context;
			this.errorText = ((errorText == null) ? string.Empty : errorText);
			this.exception = exception;
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0003524F File Offset: 0x0003424F
		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context, string errorText, Exception exception)
			: this(binding, state, context, errorText, exception, true)
		{
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x0003525F File Offset: 0x0003425F
		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context, string errorText)
			: this(binding, state, context, errorText, null, true)
		{
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x0003526E File Offset: 0x0003426E
		public BindingCompleteEventArgs(Binding binding, BindingCompleteState state, BindingCompleteContext context)
			: this(binding, state, context, string.Empty, null, false)
		{
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001B53 RID: 6995 RVA: 0x00035280 File Offset: 0x00034280
		public Binding Binding
		{
			get
			{
				return this.binding;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001B54 RID: 6996 RVA: 0x00035288 File Offset: 0x00034288
		public BindingCompleteState BindingCompleteState
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001B55 RID: 6997 RVA: 0x00035290 File Offset: 0x00034290
		public BindingCompleteContext BindingCompleteContext
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06001B56 RID: 6998 RVA: 0x00035298 File Offset: 0x00034298
		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06001B57 RID: 6999 RVA: 0x000352A0 File Offset: 0x000342A0
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x04001312 RID: 4882
		private Binding binding;

		// Token: 0x04001313 RID: 4883
		private BindingCompleteState state;

		// Token: 0x04001314 RID: 4884
		private BindingCompleteContext context;

		// Token: 0x04001315 RID: 4885
		private string errorText;

		// Token: 0x04001316 RID: 4886
		private Exception exception;
	}
}

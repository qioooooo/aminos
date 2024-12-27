using System;
using System.CodeDom;
using System.Runtime.Serialization;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000150 RID: 336
	[Serializable]
	public class CodeDomSerializerException : SystemException
	{
		// Token: 0x06000CB8 RID: 3256 RVA: 0x00030C58 File Offset: 0x0002FC58
		public CodeDomSerializerException(string message, CodeLinePragma linePragma)
			: base(message)
		{
			this.linePragma = linePragma;
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x00030C68 File Offset: 0x0002FC68
		public CodeDomSerializerException(Exception ex, CodeLinePragma linePragma)
			: base(ex.Message, ex)
		{
			this.linePragma = linePragma;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00030C7E File Offset: 0x0002FC7E
		public CodeDomSerializerException(string message, IDesignerSerializationManager manager)
			: base(message)
		{
			this.FillLinePragmaFromContext(manager);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00030C8E File Offset: 0x0002FC8E
		public CodeDomSerializerException(Exception ex, IDesignerSerializationManager manager)
			: base(ex.Message, ex)
		{
			this.FillLinePragmaFromContext(manager);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00030CA4 File Offset: 0x0002FCA4
		protected CodeDomSerializerException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.linePragma = (CodeLinePragma)info.GetValue("linePragma", typeof(CodeLinePragma));
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x00030CCE File Offset: 0x0002FCCE
		public CodeLinePragma LinePragma
		{
			get
			{
				return this.linePragma;
			}
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00030CD8 File Offset: 0x0002FCD8
		private void FillLinePragmaFromContext(IDesignerSerializationManager manager)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			CodeStatement codeStatement = (CodeStatement)manager.Context[typeof(CodeStatement)];
			if (codeStatement != null)
			{
				CodeLinePragma codeLinePragma = codeStatement.LinePragma;
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00030D18 File Offset: 0x0002FD18
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("linePragma", this.linePragma);
			base.GetObjectData(info, context);
		}

		// Token: 0x04000EBD RID: 3773
		private CodeLinePragma linePragma;
	}
}

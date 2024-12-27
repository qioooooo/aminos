using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000320 RID: 800
	[ComVisible(true)]
	public sealed class ExceptionHandlingClause
	{
		// Token: 0x06001F48 RID: 8008 RVA: 0x0004F326 File Offset: 0x0004E326
		private ExceptionHandlingClause()
		{
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001F49 RID: 8009 RVA: 0x0004F32E File Offset: 0x0004E32E
		public ExceptionHandlingClauseOptions Flags
		{
			get
			{
				return this.m_flags;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001F4A RID: 8010 RVA: 0x0004F336 File Offset: 0x0004E336
		public int TryOffset
		{
			get
			{
				return this.m_tryOffset;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001F4B RID: 8011 RVA: 0x0004F33E File Offset: 0x0004E33E
		public int TryLength
		{
			get
			{
				return this.m_tryLength;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001F4C RID: 8012 RVA: 0x0004F346 File Offset: 0x0004E346
		public int HandlerOffset
		{
			get
			{
				return this.m_handlerOffset;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001F4D RID: 8013 RVA: 0x0004F34E File Offset: 0x0004E34E
		public int HandlerLength
		{
			get
			{
				return this.m_handlerLength;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001F4E RID: 8014 RVA: 0x0004F356 File Offset: 0x0004E356
		public int FilterOffset
		{
			get
			{
				if (this.m_flags != ExceptionHandlingClauseOptions.Filter)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_EHClauseNotFilter"));
				}
				return this.m_filterOffset;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x0004F378 File Offset: 0x0004E378
		public Type CatchType
		{
			get
			{
				if (this.m_flags != ExceptionHandlingClauseOptions.Clause)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_EHClauseNotClause"));
				}
				Type type = null;
				if (!MetadataToken.IsNullToken(this.m_catchMetadataToken))
				{
					Type declaringType = this.m_methodBody.m_methodBase.DeclaringType;
					Module module = ((declaringType == null) ? this.m_methodBody.m_methodBase.Module : declaringType.Module);
					type = module.ResolveType(this.m_catchMetadataToken, (declaringType == null) ? null : declaringType.GetGenericArguments(), (this.m_methodBody.m_methodBase is MethodInfo) ? this.m_methodBody.m_methodBase.GetGenericArguments() : null);
				}
				return type;
			}
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x0004F418 File Offset: 0x0004E418
		public override string ToString()
		{
			if (this.Flags == ExceptionHandlingClauseOptions.Clause)
			{
				return string.Format(CultureInfo.CurrentUICulture, "Flags={0}, TryOffset={1}, TryLength={2}, HandlerOffset={3}, HandlerLength={4}, CatchType={5}", new object[] { this.Flags, this.TryOffset, this.TryLength, this.HandlerOffset, this.HandlerLength, this.CatchType });
			}
			if (this.Flags == ExceptionHandlingClauseOptions.Filter)
			{
				return string.Format(CultureInfo.CurrentUICulture, "Flags={0}, TryOffset={1}, TryLength={2}, HandlerOffset={3}, HandlerLength={4}, FilterOffset={5}", new object[] { this.Flags, this.TryOffset, this.TryLength, this.HandlerOffset, this.HandlerLength, this.FilterOffset });
			}
			return string.Format(CultureInfo.CurrentUICulture, "Flags={0}, TryOffset={1}, TryLength={2}, HandlerOffset={3}, HandlerLength={4}", new object[] { this.Flags, this.TryOffset, this.TryLength, this.HandlerOffset, this.HandlerLength });
		}

		// Token: 0x04000D3E RID: 3390
		private MethodBody m_methodBody;

		// Token: 0x04000D3F RID: 3391
		private ExceptionHandlingClauseOptions m_flags;

		// Token: 0x04000D40 RID: 3392
		private int m_tryOffset;

		// Token: 0x04000D41 RID: 3393
		private int m_tryLength;

		// Token: 0x04000D42 RID: 3394
		private int m_handlerOffset;

		// Token: 0x04000D43 RID: 3395
		private int m_handlerLength;

		// Token: 0x04000D44 RID: 3396
		private int m_catchMetadataToken;

		// Token: 0x04000D45 RID: 3397
		private int m_filterOffset;
	}
}

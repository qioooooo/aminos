using System;
using System.Globalization;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008EA RID: 2282
	public abstract class QualifiedAce : KnownAce
	{
		// Token: 0x06005306 RID: 21254 RVA: 0x0012D34C File Offset: 0x0012C34C
		private AceQualifier QualifierFromType(AceType type, out bool isCallback)
		{
			switch (type)
			{
			case AceType.AccessAllowed:
				isCallback = false;
				return AceQualifier.AccessAllowed;
			case AceType.AccessDenied:
				isCallback = false;
				return AceQualifier.AccessDenied;
			case AceType.SystemAudit:
				isCallback = false;
				return AceQualifier.SystemAudit;
			case AceType.SystemAlarm:
				isCallback = false;
				return AceQualifier.SystemAlarm;
			case AceType.AccessAllowedObject:
				isCallback = false;
				return AceQualifier.AccessAllowed;
			case AceType.AccessDeniedObject:
				isCallback = false;
				return AceQualifier.AccessDenied;
			case AceType.SystemAuditObject:
				isCallback = false;
				return AceQualifier.SystemAudit;
			case AceType.SystemAlarmObject:
				isCallback = false;
				return AceQualifier.SystemAlarm;
			case AceType.AccessAllowedCallback:
				isCallback = true;
				return AceQualifier.AccessAllowed;
			case AceType.AccessDeniedCallback:
				isCallback = true;
				return AceQualifier.AccessDenied;
			case AceType.AccessAllowedCallbackObject:
				isCallback = true;
				return AceQualifier.AccessAllowed;
			case AceType.AccessDeniedCallbackObject:
				isCallback = true;
				return AceQualifier.AccessDenied;
			case AceType.SystemAuditCallback:
				isCallback = true;
				return AceQualifier.SystemAudit;
			case AceType.SystemAlarmCallback:
				isCallback = true;
				return AceQualifier.SystemAlarm;
			case AceType.SystemAuditCallbackObject:
				isCallback = true;
				return AceQualifier.SystemAudit;
			case AceType.SystemAlarmCallbackObject:
				isCallback = true;
				return AceQualifier.SystemAlarm;
			}
			throw new SystemException();
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x0012D3FC File Offset: 0x0012C3FC
		internal QualifiedAce(AceType type, AceFlags flags, int accessMask, SecurityIdentifier sid, byte[] opaque)
			: base(type, flags, accessMask, sid)
		{
			this._qualifier = this.QualifierFromType(type, out this._isCallback);
			this.SetOpaque(opaque);
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06005308 RID: 21256 RVA: 0x0012D424 File Offset: 0x0012C424
		public AceQualifier AceQualifier
		{
			get
			{
				return this._qualifier;
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06005309 RID: 21257 RVA: 0x0012D42C File Offset: 0x0012C42C
		public bool IsCallback
		{
			get
			{
				return this._isCallback;
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x0600530A RID: 21258
		internal abstract int MaxOpaqueLengthInternal { get; }

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x0600530B RID: 21259 RVA: 0x0012D434 File Offset: 0x0012C434
		public int OpaqueLength
		{
			get
			{
				if (this._opaque != null)
				{
					return this._opaque.Length;
				}
				return 0;
			}
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x0012D448 File Offset: 0x0012C448
		public byte[] GetOpaque()
		{
			return this._opaque;
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x0012D450 File Offset: 0x0012C450
		public void SetOpaque(byte[] opaque)
		{
			if (opaque != null)
			{
				if (opaque.Length > this.MaxOpaqueLengthInternal)
				{
					throw new ArgumentOutOfRangeException("opaque", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_ArrayLength"), new object[] { 0, this.MaxOpaqueLengthInternal }));
				}
				if (opaque.Length % 4 != 0)
				{
					throw new ArgumentOutOfRangeException("opaque", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_ArrayLengthMultiple"), new object[] { 4 }));
				}
			}
			this._opaque = opaque;
		}

		// Token: 0x04002AF2 RID: 10994
		private readonly bool _isCallback;

		// Token: 0x04002AF3 RID: 10995
		private readonly AceQualifier _qualifier;

		// Token: 0x04002AF4 RID: 10996
		private byte[] _opaque;
	}
}

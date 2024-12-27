using System;
using System.Collections.Generic;

namespace System.Net.Mail
{
	// Token: 0x0200069D RID: 1693
	internal static class MailHeaderInfo
	{
		// Token: 0x06003447 RID: 13383 RVA: 0x000DDC14 File Offset: 0x000DCC14
		static MailHeaderInfo()
		{
			for (int i = 0; i < MailHeaderInfo.m_HeaderInfo.Length; i++)
			{
				MailHeaderInfo.m_HeaderDictionary.Add(MailHeaderInfo.m_HeaderInfo[i].NormalizedName, i);
			}
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x000DDFBC File Offset: 0x000DCFBC
		internal static string GetString(MailHeaderID id)
		{
			if (id == MailHeaderID.Unknown || id == (MailHeaderID)33)
			{
				return null;
			}
			return MailHeaderInfo.m_HeaderInfo[(int)id].NormalizedName;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x000DDFEC File Offset: 0x000DCFEC
		internal static MailHeaderID GetID(string name)
		{
			int num;
			if (MailHeaderInfo.m_HeaderDictionary.TryGetValue(name, out num))
			{
				return (MailHeaderID)num;
			}
			return MailHeaderID.Unknown;
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x000DE00C File Offset: 0x000DD00C
		internal static bool IsWellKnown(string name)
		{
			int num;
			return MailHeaderInfo.m_HeaderDictionary.TryGetValue(name, out num);
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x000DE028 File Offset: 0x000DD028
		internal static bool IsSingleton(string name)
		{
			int num;
			return MailHeaderInfo.m_HeaderDictionary.TryGetValue(name, out num) && MailHeaderInfo.m_HeaderInfo[num].IsSingleton;
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x000DE05C File Offset: 0x000DD05C
		internal static string NormalizeCase(string name)
		{
			int num;
			if (MailHeaderInfo.m_HeaderDictionary.TryGetValue(name, out num))
			{
				return MailHeaderInfo.m_HeaderInfo[num].NormalizedName;
			}
			return name;
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x000DE090 File Offset: 0x000DD090
		internal static bool IsMatch(string name, MailHeaderID header)
		{
			int num;
			return MailHeaderInfo.m_HeaderDictionary.TryGetValue(name, out num) && num == (int)header;
		}

		// Token: 0x0400302B RID: 12331
		private static readonly MailHeaderInfo.HeaderInfo[] m_HeaderInfo = new MailHeaderInfo.HeaderInfo[]
		{
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Bcc, "Bcc", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Cc, "Cc", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Comments, "Comments", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentDescription, "Content-Description", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentDisposition, "Content-Disposition", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentID, "Content-ID", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentLocation, "Content-Location", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentTransferEncoding, "Content-Transfer-Encoding", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ContentType, "Content-Type", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Date, "Date", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.From, "From", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Importance, "Importance", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.InReplyTo, "In-Reply-To", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Keywords, "Keywords", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Max, "Max", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.MessageID, "Message-ID", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.MimeVersion, "MIME-Version", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Priority, "Priority", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.References, "References", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ReplyTo, "Reply-To", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentBcc, "Resent-Bcc", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentCc, "Resent-Cc", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentDate, "Resent-Date", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentFrom, "Resent-From", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentMessageID, "Resent-Message-ID", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentSender, "Resent-Sender", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.ResentTo, "Resent-To", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Sender, "Sender", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.Subject, "Subject", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.To, "To", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.XPriority, "X-Priority", true),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.XReceiver, "X-Receiver", false),
			new MailHeaderInfo.HeaderInfo(MailHeaderID.XSender, "X-Sender", true)
		};

		// Token: 0x0400302C RID: 12332
		private static readonly Dictionary<string, int> m_HeaderDictionary = new Dictionary<string, int>(33, StringComparer.OrdinalIgnoreCase);

		// Token: 0x0200069E RID: 1694
		private struct HeaderInfo
		{
			// Token: 0x0600344E RID: 13390 RVA: 0x000DE0B3 File Offset: 0x000DD0B3
			public HeaderInfo(MailHeaderID id, string name, bool isSingleton)
			{
				this.ID = id;
				this.NormalizedName = name;
				this.IsSingleton = isSingleton;
			}

			// Token: 0x0400302D RID: 12333
			public readonly string NormalizedName;

			// Token: 0x0400302E RID: 12334
			public readonly bool IsSingleton;

			// Token: 0x0400302F RID: 12335
			public readonly MailHeaderID ID;
		}
	}
}

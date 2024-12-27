using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x020000D5 RID: 213
	internal class StaticFileHandler : IHttpHandler
	{
		// Token: 0x060009BD RID: 2493 RVA: 0x0002A2BF File Offset: 0x000292BF
		internal StaticFileHandler()
		{
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0002A2C8 File Offset: 0x000292C8
		private static bool IsOutDated(string ifRangeHeader, DateTime lastModified)
		{
			bool flag;
			try
			{
				DateTime dateTime = lastModified.ToUniversalTime();
				DateTime dateTime2 = HttpDate.UtcParse(ifRangeHeader);
				flag = dateTime2 < dateTime;
			}
			catch
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0002A304 File Offset: 0x00029304
		private static string GenerateETag(HttpContext context, DateTime lastModified, DateTime now)
		{
			long num = lastModified.ToFileTime();
			long num2 = now.ToFileTime();
			string text = num.ToString("X8", CultureInfo.InvariantCulture);
			if (num2 - num <= 30000000L)
			{
				return "W/\"" + text + "\"";
			}
			return "\"" + text + "\"";
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0002A360 File Offset: 0x00029360
		private static FileInfo GetFileInfo(string virtualPathWithPathInfo, string physicalPath, HttpResponse response)
		{
			if (!FileUtil.FileExists(physicalPath))
			{
				throw new HttpException(404, SR.GetString("File_does_not_exist"));
			}
			if (physicalPath[physicalPath.Length - 1] == '.')
			{
				throw new HttpException(404, SR.GetString("File_does_not_exist"));
			}
			FileInfo fileInfo;
			try
			{
				fileInfo = new FileInfo(physicalPath);
			}
			catch (IOException ex)
			{
				if (!HttpRuntime.HasFilePermission(physicalPath))
				{
					throw new HttpException(404, SR.GetString("Error_trying_to_enumerate_files"));
				}
				throw new HttpException(404, SR.GetString("Error_trying_to_enumerate_files"), ex);
			}
			catch (SecurityException ex2)
			{
				if (!HttpRuntime.HasFilePermission(physicalPath))
				{
					throw new HttpException(401, SR.GetString("File_enumerator_access_denied"));
				}
				throw new HttpException(401, SR.GetString("File_enumerator_access_denied"), ex2);
			}
			if ((fileInfo.Attributes & FileAttributes.Hidden) != (FileAttributes)0)
			{
				throw new HttpException(404, SR.GetString("File_is_hidden"));
			}
			if ((fileInfo.Attributes & FileAttributes.Directory) != (FileAttributes)0)
			{
				if (StringUtil.StringEndsWith(virtualPathWithPathInfo, '/'))
				{
					throw new HttpException(403, SR.GetString("Missing_star_mapping"));
				}
				response.Redirect(virtualPathWithPathInfo + "/");
			}
			return fileInfo;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0002A49C File Offset: 0x0002949C
		private static bool GetLongFromSubstring(string s, ref int startIndex, out long result)
		{
			result = 0L;
			StaticFileHandler.MovePastSpaceCharacters(s, ref startIndex);
			int num = startIndex;
			StaticFileHandler.MovePastDigits(s, ref startIndex);
			int num2 = startIndex - 1;
			if (num2 < num)
			{
				return false;
			}
			long num3 = 1L;
			for (int i = num2; i >= num; i--)
			{
				int num4 = (int)(s[i] - '0');
				result += (long)num4 * num3;
				num3 *= 10L;
				if (result < 0L)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0002A500 File Offset: 0x00029500
		private static bool GetNextRange(string rangeHeader, ref int startIndex, long fileLength, out long offset, out long length, out bool isSatisfiable)
		{
			offset = 0L;
			length = 0L;
			isSatisfiable = false;
			if (fileLength <= 0L)
			{
				startIndex = rangeHeader.Length;
				return true;
			}
			StaticFileHandler.MovePastSpaceCharacters(rangeHeader, ref startIndex);
			if (startIndex < rangeHeader.Length && rangeHeader[startIndex] == '-')
			{
				startIndex++;
				if (!StaticFileHandler.GetLongFromSubstring(rangeHeader, ref startIndex, out length))
				{
					return false;
				}
				if (length > fileLength)
				{
					offset = 0L;
					length = fileLength;
				}
				else
				{
					offset = fileLength - length;
				}
				isSatisfiable = StaticFileHandler.IsRangeSatisfiable(offset, length, fileLength);
				return StaticFileHandler.IncrementToNextRange(rangeHeader, ref startIndex);
			}
			else
			{
				if (!StaticFileHandler.GetLongFromSubstring(rangeHeader, ref startIndex, out offset))
				{
					return false;
				}
				if (startIndex < rangeHeader.Length && rangeHeader[startIndex] == '-')
				{
					startIndex++;
					long num;
					if (!StaticFileHandler.GetLongFromSubstring(rangeHeader, ref startIndex, out num))
					{
						length = fileLength - offset;
					}
					else
					{
						if (num > fileLength - 1L)
						{
							num = fileLength - 1L;
						}
						length = num - offset + 1L;
						if (length < 1L)
						{
							return false;
						}
					}
					isSatisfiable = StaticFileHandler.IsRangeSatisfiable(offset, length, fileLength);
					return StaticFileHandler.IncrementToNextRange(rangeHeader, ref startIndex);
				}
				return false;
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0002A5FA File Offset: 0x000295FA
		private static bool IncrementToNextRange(string s, ref int startIndex)
		{
			StaticFileHandler.MovePastSpaceCharacters(s, ref startIndex);
			if (startIndex < s.Length)
			{
				if (s[startIndex] != ',')
				{
					return false;
				}
				startIndex++;
			}
			return true;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0002A622 File Offset: 0x00029622
		private static bool IsRangeSatisfiable(long offset, long length, long fileLength)
		{
			return offset < fileLength && length > 0L;
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x0002A62F File Offset: 0x0002962F
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0002A632 File Offset: 0x00029632
		private static bool IsSecurityError(int ErrorCode)
		{
			return ErrorCode == 5;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0002A638 File Offset: 0x00029638
		private static void MovePastSpaceCharacters(string s, ref int startIndex)
		{
			while (startIndex < s.Length && s[startIndex] == ' ')
			{
				startIndex++;
			}
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0002A658 File Offset: 0x00029658
		private static void MovePastDigits(string s, ref int startIndex)
		{
			while (startIndex < s.Length && s[startIndex] <= '9' && s[startIndex] >= '0')
			{
				startIndex++;
			}
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0002A684 File Offset: 0x00029684
		public void ProcessRequest(HttpContext context)
		{
			StaticFileHandler.ProcessRequestInternal(context);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0002A68C File Offset: 0x0002968C
		private static bool ProcessRequestForNonMapPathBasedVirtualFile(HttpRequest request, HttpResponse response)
		{
			bool flag = false;
			if (HostingEnvironment.UsingMapPathBasedVirtualPathProvider)
			{
				return flag;
			}
			VirtualFile virtualFile = null;
			string filePath = request.FilePath;
			if (HostingEnvironment.VirtualPathProvider.FileExists(filePath))
			{
				virtualFile = HostingEnvironment.VirtualPathProvider.GetFile(filePath);
			}
			if (virtualFile == null)
			{
				throw new HttpException(404, SR.GetString("File_does_not_exist"));
			}
			if (virtualFile is MapPathBasedVirtualFile)
			{
				return flag;
			}
			response.WriteVirtualFile(virtualFile);
			response.ContentType = MimeMapping.GetMimeMapping(filePath);
			return true;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0002A700 File Offset: 0x00029700
		internal static bool ProcessRangeRequest(HttpContext context, string physicalPath, long fileLength, string rangeHeader, string etag, DateTime lastModified)
		{
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			bool flag = false;
			if (fileLength <= 0L)
			{
				StaticFileHandler.SendRangeNotSatisfiable(response, fileLength);
				return true;
			}
			string text = request.Headers["If-Range"];
			if (text != null && text.Length > 1)
			{
				if (text[0] == '"')
				{
					if (text != etag)
					{
						return flag;
					}
				}
				else
				{
					if (text[0] == 'W' && text[1] == '/')
					{
						return flag;
					}
					if (StaticFileHandler.IsOutDated(text, lastModified))
					{
						return flag;
					}
				}
			}
			int num = rangeHeader.IndexOf('=');
			if (num == -1 || num == rangeHeader.Length - 1)
			{
				return flag;
			}
			int num2 = num + 1;
			bool flag2 = true;
			bool flag3 = false;
			ByteRange[] array = null;
			int num3 = 0;
			long num4 = 0L;
			while (num2 < rangeHeader.Length && flag2)
			{
				long num5;
				long num6;
				bool flag4;
				flag2 = StaticFileHandler.GetNextRange(rangeHeader, ref num2, fileLength, out num5, out num6, out flag4);
				if (!flag2)
				{
					break;
				}
				if (flag4)
				{
					if (array == null)
					{
						array = new ByteRange[16];
					}
					if (num3 >= array.Length)
					{
						ByteRange[] array2 = new ByteRange[array.Length * 2];
						Buffer.BlockCopy(array, 0, array2, 0, array.Length * Marshal.SizeOf(array[0]));
						array = array2;
					}
					array[num3].Offset = num5;
					array[num3].Length = num6;
					num3++;
					num4 += num6;
					if (num4 > fileLength * 5L)
					{
						flag3 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				return flag;
			}
			if (flag3)
			{
				StaticFileHandler.SendBadRequest(response);
				return true;
			}
			if (num3 == 0)
			{
				StaticFileHandler.SendRangeNotSatisfiable(response, fileLength);
				return true;
			}
			string mimeMapping = MimeMapping.GetMimeMapping(physicalPath);
			if (num3 == 1)
			{
				long num5 = array[0].Offset;
				long num6 = array[0].Length;
				response.ContentType = mimeMapping;
				string text2 = string.Format(CultureInfo.InvariantCulture, "bytes {0}-{1}/{2}", new object[]
				{
					num5,
					num5 + num6 - 1L,
					fileLength
				});
				response.AppendHeader("Content-Range", text2);
				StaticFileHandler.SendFile(physicalPath, num5, num6, fileLength, context);
			}
			else
			{
				response.ContentType = "multipart/byteranges; boundary=<q1w2e3r4t5y6u7i8o9p0zaxscdvfbgnhmjklkl>";
				string text3 = "Content-Type: " + mimeMapping + "\r\n";
				for (int i = 0; i < num3; i++)
				{
					long num5 = array[i].Offset;
					long num6 = array[i].Length;
					response.Write("--<q1w2e3r4t5y6u7i8o9p0zaxscdvfbgnhmjklkl>\r\n");
					response.Write(text3);
					response.Write("Content-Range: ");
					string text4 = string.Format(CultureInfo.InvariantCulture, "bytes {0}-{1}/{2}", new object[]
					{
						num5,
						num5 + num6 - 1L,
						fileLength
					});
					response.Write(text4);
					response.Write("\r\n\r\n");
					StaticFileHandler.SendFile(physicalPath, num5, num6, fileLength, context);
					response.Write("\r\n");
				}
				response.Write("--<q1w2e3r4t5y6u7i8o9p0zaxscdvfbgnhmjklkl>--\r\n\r\n");
			}
			response.StatusCode = 206;
			response.AppendHeader("Last-Modified", HttpUtility.FormatHttpDateTime(lastModified));
			response.AppendHeader("Accept-Ranges", "bytes");
			response.AppendHeader("ETag", etag);
			response.AppendHeader("Cache-Control", "public");
			return true;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0002AA64 File Offset: 0x00029A64
		internal static void ProcessRequestInternal(HttpContext context)
		{
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			if (StaticFileHandler.ProcessRequestForNonMapPathBasedVirtualFile(request, response))
			{
				return;
			}
			string path = request.Path;
			string physicalPath = request.PhysicalPath;
			FileInfo fileInfo = StaticFileHandler.GetFileInfo(path, physicalPath, response);
			DateTime dateTime = new DateTime(fileInfo.LastWriteTime.Year, fileInfo.LastWriteTime.Month, fileInfo.LastWriteTime.Day, fileInfo.LastWriteTime.Hour, fileInfo.LastWriteTime.Minute, fileInfo.LastWriteTime.Second, 0);
			DateTime now = DateTime.Now;
			if (dateTime > now)
			{
				dateTime = new DateTime(now.Ticks - now.Ticks % 10000000L);
			}
			string text = StaticFileHandler.GenerateETag(context, dateTime, now);
			long length = fileInfo.Length;
			string text2 = request.Headers["Range"];
			if (StringUtil.StringStartsWithIgnoreCase(text2, "bytes") && StaticFileHandler.ProcessRangeRequest(context, physicalPath, length, text2, text, dateTime))
			{
				return;
			}
			StaticFileHandler.SendFile(physicalPath, 0L, length, length, context);
			response.ContentType = MimeMapping.GetMimeMapping(physicalPath);
			response.AppendHeader("Accept-Ranges", "bytes");
			response.AddFileDependency(physicalPath);
			response.Cache.SetIgnoreRangeRequests();
			response.Cache.SetExpires(DateTime.Now.AddDays(1.0));
			response.Cache.SetLastModified(dateTime);
			response.Cache.SetETag(text);
			response.Cache.SetCacheability(HttpCacheability.Public);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0002AC02 File Offset: 0x00029C02
		private static void SendBadRequest(HttpResponse response)
		{
			response.StatusCode = 400;
			response.Write("<html><body>Bad Request</body></html>");
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0002AC1A File Offset: 0x00029C1A
		private static void SendRangeNotSatisfiable(HttpResponse response, long fileLength)
		{
			response.StatusCode = 416;
			response.ContentType = null;
			response.AppendHeader("Content-Range", "bytes */" + fileLength.ToString(NumberFormatInfo.InvariantInfo));
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0002AC50 File Offset: 0x00029C50
		private static void SendFile(string physicalPath, long offset, long length, long fileLength, HttpContext context)
		{
			try
			{
				context.Response.TransmitFile(physicalPath, offset, length);
			}
			catch (ExternalException ex)
			{
				if (StaticFileHandler.IsSecurityError(ex.ErrorCode))
				{
					throw new HttpException(401, SR.GetString("Resource_access_forbidden"));
				}
				throw;
			}
		}

		// Token: 0x04001257 RID: 4695
		private const string RANGE_BOUNDARY = "<q1w2e3r4t5y6u7i8o9p0zaxscdvfbgnhmjklkl>";

		// Token: 0x04001258 RID: 4696
		private const string MULTIPART_CONTENT_TYPE = "multipart/byteranges; boundary=<q1w2e3r4t5y6u7i8o9p0zaxscdvfbgnhmjklkl>";

		// Token: 0x04001259 RID: 4697
		private const string MULTIPART_RANGE_DELIMITER = "--<q1w2e3r4t5y6u7i8o9p0zaxscdvfbgnhmjklkl>\r\n";

		// Token: 0x0400125A RID: 4698
		private const string MULTIPART_RANGE_END = "--<q1w2e3r4t5y6u7i8o9p0zaxscdvfbgnhmjklkl>--\r\n\r\n";

		// Token: 0x0400125B RID: 4699
		private const string CONTENT_RANGE_FORMAT = "bytes {0}-{1}/{2}";

		// Token: 0x0400125C RID: 4700
		private const int MAX_RANGE_ALLOWED = 5;

		// Token: 0x0400125D RID: 4701
		private const int ERROR_ACCESS_DENIED = 5;
	}
}

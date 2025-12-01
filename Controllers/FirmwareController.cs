using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;

namespace FTPalters.Controllers
{
    //[RoutePrefix("GetFirmware")]
    public class FirmwareController : ApiController
    {
        [HttpGet]
        [Route("GetFirmware")]
        public async Task<HttpResponseMessage> GetFirmware(long? start = null, long? end = null)
        {
            string fileUrl = "http://rtul.co.th/ACG_S7G2.bin";

            using (var httpClient = new HttpClient())
            {
                byte[] fileBytes = await httpClient.GetByteArrayAsync(fileUrl);

                long total = fileBytes.Length;
                long realStart = start ?? 0;
                long realEnd = end ?? (realStart + 4095);
                if (realEnd >= total) realEnd = total - 1;

                long size = realEnd - realStart + 1;

                byte[] chunk = new byte[size];
                Buffer.BlockCopy(fileBytes, (int)realStart, chunk, 0, (int)size);

                string hex = BitConverter.ToString(chunk).Replace("-", " ");

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(hex, System.Text.Encoding.UTF8, "text/plain")
                };
            }
        }


        /*public HttpResponseMessage GetFirmware(long? start = null, long? end = null)
        {
            //string filePath = @"D:\ftp files\ACG_S7G2.bin";
            string filePath = @"rtul.co.th\ACG_S7G2.bin";


            if (!File.Exists(filePath))
                return Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");

            long totalSize = new FileInfo(filePath).Length;

            long realStart = start ?? 0;
            long realEnd = end ?? (realStart + 4095);

            if (realEnd >= totalSize)
                realEnd = totalSize - 1;

            long chunkSize = realEnd - realStart + 1;
            byte[] buffer = new byte[chunkSize];

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(realStart, SeekOrigin.Begin);
                fs.Read(buffer, 0, (int)chunkSize);
            }

            string hexOutput = BitConverter.ToString(buffer).Replace("-", " ");

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(hexOutput);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
            return response;
        }*/


        /*public HttpResponseMessage GetFirmware(long? start = null, long? end = null)
        {
            string filePath = @"D:\ftp files\ACG_S7G2.bin";

            if (!File.Exists(filePath))
                return Request.CreateResponse(HttpStatusCode.NotFound, "File not found.");

            long totalSize = new FileInfo(filePath).Length;

            long realStart = start ?? 0;
            long realEnd = (end ?? (realStart + 4095));

            if (realEnd >= totalSize)
                realEnd = totalSize - 1;

            long chunkSize = realEnd - realStart + 1;

            byte[] buffer = new byte[chunkSize];

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(realStart, SeekOrigin.Begin);
                fs.Read(buffer, 0, (int)chunkSize);
            }

            //response.Content = new ByteArrayContent(buffer);
            //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            string stringOutput = BitConverter.ToString(buffer).Replace("-", " ");
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(stringOutput);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

            return response;

        }*/

        /*public HttpResponseMessage GetFirmware()
        {
            string filePath = @"D:\ftp files\ACG_S7G2.bin";

            if (!File.Exists(filePath))
                return Request.CreateResponse(HttpStatusCode.NotFound);

            long totalSize = new FileInfo(filePath).Length;

            // ✔ If no Range header → return full file normally
            if (Request.Headers.Range == null)
            {
                var fullResponse = new HttpResponseMessage(HttpStatusCode.OK);
                fullResponse.Content = new StreamContent(new FileStream(filePath, FileMode.Open, FileAccess.Read));
                fullResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                fullResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "firmware.bin"
                };
                return fullResponse;
            }

            // ✔ Ranged request (partial content)
            var range = Request.Headers.Range.Ranges.First();
            long start = range.From ?? 0;
            long end = range.To ?? (start + 4095);
            if (end >= totalSize) end = totalSize - 1;

            long chunkSize = end - start + 1;
            byte[] buffer = new byte[chunkSize];

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fs.Seek(start, SeekOrigin.Begin);
                fs.Read(buffer, 0, (int)chunkSize);
            }

            var response = new HttpResponseMessage(HttpStatusCode.PartialContent);
            response.Content = new ByteArrayContent(buffer);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = chunkSize;
            response.Content.Headers.ContentRange = new ContentRangeHeaderValue(start, end, totalSize);

            return response;
        }*/
    }

}
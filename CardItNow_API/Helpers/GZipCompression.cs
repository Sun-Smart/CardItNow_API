using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System.Net.Http;
//using System.Web.Http.Filters;

namespace SunSmartnTireProducts.Helpers
{
    public class GzipCompressionAttribute : ActionFilterAttribute
    {
        //public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        //{
        //    var content = actionContext.Response.Content;
        //    var bytes = content == null ? null : content.ReadAsByteArrayAsync().Result;
        //    var zlibbedContent = bytes == null ? new byte[0] :
        //    CompressionHelper.GzipByte(bytes);
        //    actionContext.Response.Content = new ByteArrayContent(zlibbedContent);
        //    actionContext.Response.Content.Headers.Remove("Content-Type");
        //    actionContext.Response.Content.Headers.Add("Content-encoding", "gzip");
        //    actionContext.Response.Content.Headers.Add("Content-Type", "application/json");
        //    base.OnActionExecuted(actionContext);
        //}
    }

    public static class CompressionHelper
    {
        //public static byte[] GzipByte(byte[] str)
        //{
        //    if (str == null)
        //    {
        //        return null;
        //    }

        //    using (var output = new MemoryStream())
        //    {
        //        using (var compressor = new Ionic.Zlib.GZipStream(output, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestSpeed))
        //        {
        //            compressor.Write(str, 0, str.Length);
        //        }
        //        return output.ToArray();
        //    }
        //}
    }
}
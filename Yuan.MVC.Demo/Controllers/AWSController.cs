using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yuan.MVC.Demo.Models;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Text;

namespace Yuan.MVC.Demo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AWSController : ControllerBase
    {

        AmazonS3Client S3client;
        public AWSController()
        {

            var credentials = new BasicAWSCredentials("AKIATKNV342WBCXQQXHO", "avlOJTfaqqqqo/PI9h+Rd/UXaEjy67gDe/laYOKt");
            S3client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            var putrequest = new PutObjectRequest()
            {
                BucketName = "demo-bucket-2021s",
                Key = file.FileName,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType,
            };
            var result = await this.S3client.PutObjectAsync(putrequest);
            return Ok(result);
        }

        [HttpGet]
        [Route("showbucket")]
        public async Task<IActionResult> Showbucket()
        {
            var buckets = await S3client.ListBucketsAsync();
            int n = buckets.Buckets.Count();
            string[] results = new string[n];
            string result = new string("The source in AWS is:\n");
            int count = 0;
            foreach (var a in buckets.Buckets)
            {
                results[count] = a.BucketName;
                count++;
                var objects = await S3client.ListObjectsAsync(a.BucketName);
                result += new string("BucketsName:\t");
                result += a.BucketName.ToString();
                //Console.WriteLine(a.BucketName);
                if (objects != null)
                {
                    foreach (var o in objects.S3Objects)
                    {
                        result += new string("\nSourceName:\t");
                        result += o.Key.ToString();
                        var objectsres = await S3client.GetObjectAsync(a.BucketName, o.Key);
                        var bytes = new byte[objectsres.ResponseStream.Length];
                        objectsres.ResponseStream.Read(bytes, 0, bytes.Length);
                        CancellationTokenSource source = new CancellationTokenSource();
                        CancellationToken token = source.Token;
                        //Console.WriteLine(token.IsCancellationRequested);
                        //var sw = new StreamWriter("D:\\practice\\S3\\b.json");
                        var aa = Encoding.UTF8.GetString(bytes);
                        result += new string("\n The content of source is:\t");
                        result += aa;
                    }
                }
            }
            return new JsonResult(results);



        }
        [Route("products")]
        [HttpGet]
        public async Task<IActionResult> GetFile()
        {
            var request = new GetObjectRequest()
            {
                BucketName = "demo-bucket-2021s",
                Key = "products.json",
            };
            var response = await S3client.GetObjectAsync(request);
            var responsestream = response.ResponseStream;
            var stream = new MemoryStream();
            await responsestream.CopyToAsync(stream);
            stream.Position = 0;

            return new FileStreamResult(stream, response.Headers["Content-Type"])
            {
                FileDownloadName = "products.json"
            };
        }
    }


}
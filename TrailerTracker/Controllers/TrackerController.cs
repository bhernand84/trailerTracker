using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using TrailerTracker.Models;

namespace TrailerTracker.Controllers
{

    public class TrackerController : ApiController
    {
        [HttpGet]
        public TrackerPayload DidCheckIn(string trailerNumber)
        {
            var esn = getESNNumber(trailerNumber);
            var trailer = GetTrailerInfo(esn);
            bool didCheckIn = trailer.lastIdReportTime >= new DateTime(2017, 1, 1);
            TrackerPayload payload = new TrackerPayload(didCheckIn);
            payload.trailerInfo = trailer;
            return payload;
        }
        [Route("shouldOpen/{trailerNumber}")]
        [HttpGet]
        public TrackerPayload ShouldOpen(int trailerNumber)
        {
            TrackerPayload payload = new TrackerPayload();
            return payload;
        }

        protected string getESNNumber(string trailerNumber)
        {
            return "4642045738";
        }

        protected TrailerInfo GetTrailerInfo(string esn)
        {
            var deviceUrl = "https://puls.calamp.com/service/device/" + esn;
            var response = SendData(deviceUrl);
            String responseString = "";
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                responseString = reader.ReadToEnd();
            }

            if(responseString == null)
            {
                return null;
            }

            var trailerInfo = JsonConvert.DeserializeObject<TrailerInfo>(responseString);
                
            return trailerInfo;
        }

        protected static WebResponse SendData(string url)
        {

            //  			            
            // if you need to ignore Certificate validation failures (aka untrusted certificate + certificate chains)
            //  http://stackoverflow.com/questions/708210/how-to-use-http-get-request-in-c-sharp-with-ssl-protocol-violation            
            //			ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true); 

            // http://stackoverflow.com/questions/5653868/what-makes-this-https-webrequest-time-out-even-though-it-works-in-the-browser
            // Ssl3 is no longer supported by CalAmp.  If you need to specify a Protocol, use Tls.
            //			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.ContentLength = 0;
            request.ContentType = "application/json";
            request.MediaType = "application/json";

            byte[] authBytes = Encoding.UTF8.GetBytes("GeoDecisions:DCSreset17!".ToCharArray());

            request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", Convert.ToBase64String(authBytes)));

            try
            {
                var webresponse = request.GetResponse();
                return webresponse;
            }
            catch (WebException ex)
            {
                return ex.Response;
            }
        }

    }
}

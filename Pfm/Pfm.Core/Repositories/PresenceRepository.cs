using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pfm.Core.Interfaces;
using Regula.FaceSDK.WebClient.Api;
using Regula.FaceSDK.WebClient.Model;

namespace Pfm.Core.Repositories
{
    public class PresenceRepository : IPresence
    {
        public void Checkin(byte[] face, Action<string> successAction, Action<string> failAction)
        {
            try
            {
                var apiBasePath = "https://faceapi.regulaforensics.com";

                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Resources");
                var face1 = File.ReadAllBytes($"{path}/face_1.jpg");
                var face2 = face;

                var sdk = new FaceSdk(apiBasePath);

                var matchImage1 = new MatchImage(data: face1, type: ImageSource.LIVE);
                var matchImage2 = new MatchImage(data: face2, type: ImageSource.LIVE);


                var matchImages = new List<MatchImage> { matchImage1, matchImage2 };
                var matchingRequest = new MatchRequest(
                    null, 
                    false, 
                    matchImages
                );

                var matchingResponse = sdk.MatchingApi.Match(matchingRequest);

                decimal score = 0;
                foreach (var comparison in matchingResponse.Results)
                {
                    if(comparison.FirstIndex == 0)
                        score = comparison.Similarity * 100;
                }

                Console.WriteLine($"SCORE {score}");

                if(score >= 075)
                {
                    successAction("STATUS FACE :  COCOK");
                    failAction("");
                }
                else
                {
                    successAction("STATUS FACE : TIDAK COCOK");
                    failAction("");
                }

            }
            catch (System.Exception ex)
            {
                successAction("ERROR");
                failAction("");
                throw;
            }
        }
    }
}
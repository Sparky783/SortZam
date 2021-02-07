using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sortzam.Ihm.Models
{
    [DataContract]
    public class Settings
    {
        [DataMember]
        public string ApiHost { get; set; }

        [DataMember]
        public string ApiKey { get; set; }

        [DataMember]
        public string SecretKey { get; set; }


        public Settings()
        {
            ApiHost = "";
            ApiKey = "";
            SecretKey = "";

            // TODO: Remove before commit on Prod
            ApiHost = "identify-eu-west-1.acrcloud.com";
            ApiKey = "ca88123807e49300eaea0fb9441c1bde";
            SecretKey = "ri9MAp8fXzEXu300Apch3Qj74Hadz2XiJbr9izox";
        }
    }
}
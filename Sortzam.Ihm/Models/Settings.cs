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
        public bool UseAccount { get; set; }

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
        }
    }
}
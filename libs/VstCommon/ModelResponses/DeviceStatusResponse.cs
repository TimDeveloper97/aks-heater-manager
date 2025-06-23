using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VstCommon.ModelResponses
{
    public class DeviceStatusResponse : VstBaseResponse
    {
        [JsonProperty("U")]
        public float U { get; set; }

        [JsonProperty("I")]
        public float I { get; set; }

        [JsonProperty("C")]
        public float C { get; set; }
    }
}

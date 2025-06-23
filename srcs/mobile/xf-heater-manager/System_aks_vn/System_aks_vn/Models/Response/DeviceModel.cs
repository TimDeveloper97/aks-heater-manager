using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using VstCommon.ModelResponses;

namespace System_aks_vn.Models.Response
{
    public class DeviceModel
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("addr")]
        public string Addr { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("setting")]
        public DeviceSetting Setting { get; set; }

        public DeviceModel()
        {
            
        }

        public DeviceModel(DeviceResponse deviceResponse)
        {
            this.Model = deviceResponse.Model;
            this.Name = deviceResponse.Name;
            this.Version = deviceResponse.Version;
            this.Addr = deviceResponse.Addr;
            this.Id = deviceResponse.Id;
        }
    }

    public class DeviceSetting
    {
        [JsonProperty("SMS")]
        public object Smss { get; set; }
        [JsonProperty("CALL")]
        public object Calls { get; set; }
        [JsonProperty("PLAN")]
        public object PLan { get; set; }
    }
}

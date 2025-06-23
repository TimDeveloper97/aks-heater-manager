using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System_aks_vn.Domain;
using VstCommon.ModelResponses;

namespace System_aks_vn.Models.Response
{
    public class DeviceModel : BaseBinding
    {
        private string model;

        private string name;

        private string addr;

        private string version;

        private string id;

        [JsonProperty("setting")]
        public DeviceSetting Setting { get; set; }


        [JsonProperty("model")]
        public string Model { get => model; set => SetProperty(ref model, value); }
        [JsonProperty("name")]
        public string Name { get => name; set => SetProperty(ref name, value); }
        [JsonProperty("addr")]
        public string Addr { get => addr; set => SetProperty(ref addr, value)  ; }
        [JsonProperty("version")]
        public string Version { get => version; set => SetProperty(ref version, value); }
        [JsonProperty("_id")]
        public string Id { get => id; set => SetProperty(ref id, value); }

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

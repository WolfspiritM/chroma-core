using Chroma.NetCore.Api.Chroma;
using Chroma.NetCore.Api.Extensions;
using Chroma.NetCore.Api.Interfaces;
using Newtonsoft.Json;

namespace Chroma.NetCore.Api.Messages
{
    public class DeviceUpdateMessage : IHttpRequestMessage
    {
        public DeviceUpdateMessage(IDevice device, string message)
        {
            Device = device;
            Message = message;
        }

        public IDevice Device { get; }
        public Enums.HttpMessageMethod HttpMessageMethod => Enums.HttpMessageMethod.Put;
        public string UrlPath => $"chromasdk/{Device.Device}";
        public string Message { get; }
    }
}

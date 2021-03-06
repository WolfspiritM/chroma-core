﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chroma.NetCore.Api.Devices;
using Chroma.NetCore.Api.Interfaces;
using Chroma.NetCore.Api.Messages;

namespace Chroma.NetCore.Api.Chroma
{
    public class ChromaInstance : DeviceContainer
    {
        private readonly IClient client;
        private DeviceContainer container;

        public event Action DestroyMessage = delegate { };
        
        public ChromaInstance(IClient client)
        {
            this.client = client;
            container = this;
        }

        public  async Task<bool> Destroy()
        {
            DestroyMessage();
            var result = await client.UnRegister();

            var unregistered = Convert.ToInt32(result) == 0;
            return unregistered;
        }

        public async Task<List<RequestResponse>> Send(DeviceContainer deviceContainer = null)
        {
            container = deviceContainer ?? this;
            var effectIds = new List<string>();
            var devices = new Dictionary<string, IDevice>();
            var responses = new List<RequestResponse>();

            foreach (var device in container.Devices)
            {
                if(device.Value.ActiveEffect == Effect.Undefined)
                    continue;

                if (!string.IsNullOrEmpty(device.Value.EffectId))
                    effectIds.Add(device.Value.EffectId);
                else
                    devices[device.Key] = device.Value;
            }

            responses.AddRange(await SetEffect(effectIds));
            responses.AddRange( await SendDeviceUpdate(devices));

            return responses;
        }

        internal async Task<List<RequestResponse>> SendDeviceUpdate(Dictionary<string, IDevice> devices, bool store = false)
        {
            var responses = new List<RequestResponse>();

            foreach (var device in devices.Values)
            {
                IHttpRequestMessage message;
                var deviceMessage = device.GetDeviceMessage();
                if (deviceMessage == null) continue;


                if(store)
                    message = new DeviceMessage(device, deviceMessage);
                else
                    message = new DeviceUpdateMessage(device, deviceMessage);

                responses.Add( new RequestResponse(device,await client.Request(message)));
            }

            return responses;
        }

        internal async Task<List<string>> DeleteEffect(List<string> effectIds)
        {
            var responses = new List<string>();

            if (effectIds.Count <= 0)
                return responses;

            var message = new EffectMessage(effectIds, true);

            responses.Add(await client.Request(message));
            return responses;
        }

        internal async Task<List<RequestResponse>> SetEffect(List<string> effectIds)
        {
            var responses = new List<RequestResponse>();

            if (effectIds.Count <= 0)
                return responses;

            var message = new EffectMessage(effectIds);

            responses.Add(new RequestResponse(message.Device, await client.Request(message)));

            return responses;
        }
    }
}

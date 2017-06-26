using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Chroma.NetCore.Api.Chroma;
using Chroma.NetCore.Api.Extensions;
using Chroma.NetCore.Api.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Chroma.NetCore.Api.Interfaces
{
    public abstract class DeviceBase : IDevice, IDeviceData
    {

        #region Properties

        public abstract string Device { get; }
        public virtual List<Effect> Supports { get; }
        public Effect ActiveEffect { get; private set; }
        public string EffectId { get; set; }

        private string lastMessage = "";

        #endregion

        protected DeviceBase()
        {
            ActiveEffect = Effect.Undefined;
            EffectData = null;
        }

        public dynamic EffectData { get; set; }

        public void SetStatic(Color color)
        {
           SetDeviceEffect(Effect.ChromaStatic, color);
        }

        public virtual void SetAll(Color color)
        {
           SetStatic(color);
        }

        public void SetNone()
        {
           SetDeviceEffect(Effect.ChromaNone);
        }

        protected bool SetDeviceEffect(Effect effect, dynamic data = null)
        {
            this.ActiveEffect = effect;
            this.EffectData = data;

            return true;
        }

        public string GetDeviceMessage()
        {
            var newMessage = GenerateMessage(EffectData);
            if (lastMessage == newMessage) return null;
            lastMessage = newMessage;
            return newMessage;
        }


        private class Data
        {
            public string Effect { get; set; }
            public dynamic Param { get; set; }
        }

        internal string GenerateMessage(dynamic data)
        {
            var message = new Data();

            switch (ActiveEffect)
            {

                case Effect.ChromaNone:
                    message.Effect = ActiveEffect.GetStringValue();
                    message.Param = null;
                    break;

                case Effect.ChromaStatic:
                    message.Effect = ActiveEffect.GetStringValue();
                    message.Param = new { color = ((Color)data).ToBgr() };
                    break;

                case Effect.ChromaCustom2:
                case Effect.ChromaCustom:
                    message.Effect = ActiveEffect.GetStringValue();
                    var parammatrix = data.ToMatrix();
                    message.Param = (parammatrix.Length == 1) ? parammatrix[0] : parammatrix;
                    break;

                case Effect.ChromaCustomKey:
                    message.Effect = ActiveEffect.GetStringValue();
                    message.Param = new
                    {
                        color = data.color.ToMatrix(),
                        key = data.key.ToMatrix()
                    };
                    break;
            }

            var jsonString = JsonConvert.SerializeObject(message, Formatting.Indented,
                new JsonSerializerSettings() {ContractResolver = new CamelCasePropertyNamesContractResolver()});

            return jsonString;
        }

        public override string ToString()
        {
            return this.Device;
        }
    }
}

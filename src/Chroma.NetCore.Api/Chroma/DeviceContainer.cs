using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chroma.NetCore.Api.Devices;
using Chroma.NetCore.Api.Interfaces;

namespace Chroma.NetCore.Api.Chroma
{
    public class DeviceContainer
    {

        public Keyboard Keyboard { get => this.Devices["Keyboard"] as Keyboard; set => this.Devices["Keyboard"] = value; }
        public Headset Headset { get => this.Devices["Headset"] as Headset; set => this.Devices["Headset"] = value; }
        public Mouse Mouse { get => this.Devices["Mouse"] as Mouse; set => this.Devices["Mouse"] = value; }
        public Mousepad Mousepad { get => this.Devices["Mousepad"] as Mousepad; set => this.Devices["Mousepad"] = value; }
        public Keypad Keypad { get => this.Devices["Keypad"] as Keypad; set => this.Devices["Keypad"] = value; }
        public ChromaLink ChromaLink { get => this.Devices["ChromaLink"] as ChromaLink; set => this.Devices["ChromaLink"] = value; }

        public Dictionary<string, IDevice> Devices { get; }

        public DeviceContainer()
        {
            Devices = new Dictionary<string,IDevice>
            {
                { "Keyboard", new Keyboard()},
                { "Headset", new Headset()},
                { "Mouse", new Mouse()},
                { "Mousepad", new Mousepad()},
                { "Keypad", new Keypad()},
                { "ChromaLink", new ChromaLink()}
            };
        }

        
        public void SetAll(Color color)
        {
            foreach (var device in Devices.Values)
            {
                device.SetAll(color);
            }
        }
    }
}

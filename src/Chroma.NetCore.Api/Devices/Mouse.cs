using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chroma.NetCore.Api.Chroma;
using Chroma.NetCore.Api.Interfaces;

namespace Chroma.NetCore.Api.Devices
{
    public class Mouse : DeviceBase, IGridDevice
    {
        public override string Device => "mouse";

        public Grid Grid { get; }

        public Mouse()
        {
            Grid = new Grid(9,7);
        }
        
        public IGridDevice SetPosition(int row, int col, Color color)
        {
            Grid.SetPosition(row, col, color);
            this.SetDevice();
            return this;
        }

        public Color GetPosition(int row, int col)
        {
            return Grid.GetPosition(row, col) ?? Color.Black;
        }

        public override void SetAll(Color color)
        {
            Grid.Set(color);
            this.SetDevice();
            //this.SetStatic(color);
        }

        public bool SetDevice()
        {
            return SetDeviceEffect(Effect.ChromaCustom2, this.Grid);
        }
    }
}

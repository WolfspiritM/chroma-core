using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chroma.NetCore.Api.Chroma;
using Chroma.NetCore.Api.Interfaces;

namespace Chroma.NetCore.Api.Devices
{
    public class Keyboard : DeviceBase, IGridDevice
    {
        public override string Device => "keyboard";

        public Grid Grid { get; }
        private Grid Keys { get; }


        private const int ROWS = 6;
        private const int COLUMNS = 22;

        public Keyboard()
        {
            Grid = new Grid(ROWS, COLUMNS);
            Keys = new Grid(ROWS, COLUMNS, true);
        }

        public IGridDevice SetPosition(int row, int col, Color color)
        {
            Grid.SetPosition(row, col, color);
            this.SetDevice();
            return this;
        }

        public override void SetAll(Color color)
        {
            Grid.Set(color);
            this.SetDevice();
            //this.SetStatic(color);
        }

        public IGridDevice SetKey(List<Key> keys, Color color)
        {
            foreach (var key in keys)
            {
                SetKey(key, color);
            }

            this.SetDevice();
            return this;
        }

        public Color GetKey(Key key)
        {
            var row = (int)key >> 8;
            var col = (int)key & 0xFF;

            return Keys.GetPosition(row, col);
        }

        public Color GetPosition(int row, int col)
        {
            return Grid.GetPosition(row, col) ?? Color.Black;
        }

        public IGridDevice SetKey(Key key, Color color)
        {
            var row = (int)key >> 8;
            var col = (int)key & 0xFF;

            Keys.SetPosition(row, col, color);
            this.SetDevice();
            return this;
        }


        public bool SetDevice()
        {
            var customKeyEffect = new
            {
                color = Grid,
                key = Keys
            };

          return SetDeviceEffect(Effect.ChromaCustomKey, customKeyEffect);
        }
    }
}

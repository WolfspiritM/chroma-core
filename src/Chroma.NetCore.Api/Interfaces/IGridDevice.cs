using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Chroma.NetCore.Api.Chroma;

namespace Chroma.NetCore.Api.Interfaces
{
    public interface IGridDevice
    {
        Grid Grid { get; }

        IGridDevice SetPosition(int row, int col, Color color);
        Color GetPosition(int row, int col);

        bool SetDevice();
        
    }
}

using System.Threading.Tasks;
using Chroma.NetCore.Api.Exceptions;
using Newtonsoft.Json;

namespace Chroma.NetCore.Api.Chroma
{
    public class Grid
    {
        public int Rows { get; }
        public int Cols { get; }
        internal Color[][] Matrix { get; }

        private readonly Color intialColor;
        private bool isKeyGrid;

        public Grid(int rows, int cols, bool isKeyGrid = false, Color initialColor = null)
        {
            Rows = rows;
            Cols = cols;
            this.intialColor = initialColor ?? Color.Black;
            this.Matrix = new Color[rows][];
            this.isKeyGrid = isKeyGrid;
            InitGrid();
        }

        private void InitGrid()
        {
            for (int r = 0; r < Rows; r++)
            {
                Matrix[r] = new Color[Cols];
            }
        }
        public void Set(Color color)
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c= 0; c < Cols; c++)
                {
                    Matrix[r][c] = color;
                }
            }
        }

        public void SetPosition(int row, int col, Color color)
        {
            CheckBounds(row, col);

            //Set color for position in matrix grid
            Matrix[row][col] = color;
        }

        public void SetRow(int col, Color color)
        {
            CheckBounds(0, col);

            for (int r = 0; r < Rows; r++)
            {
                Matrix[r][col] = color;
            }
        }


        public void SetCol(int row, Color color)
        {
            CheckBounds(row, 0);

            for (int c = 0; c < Cols; c++)
            {
                Matrix[row][c] = color;
            }
        }

        public uint[][] ToMatrix()
        {
            var convertedMatrix = new uint[Rows][];

            for (int r = 0; r < Rows; r++)
            {
                convertedMatrix[r] = new uint[Cols];

                for (int c = 0; c < Cols; c++)
                {
                    if (Matrix[r][c] == null)
                    {
                        convertedMatrix[r][c] = 0;
                    }
                    else
                    {
                        var position = Matrix[r][c] ?? intialColor;
                        convertedMatrix[r][c] = position.ToBgr();
                        if (this.isKeyGrid)
                        {
                            convertedMatrix[r][c] += 0xFF000000;
                        }
                    }
                }
            }

            return convertedMatrix;
        }

        public string ToJson()
        {
            var json = JsonConvert.SerializeObject(ToMatrix());
            return json;
        }

        public Color GetPosition(int row, int col)
        {
            CheckBounds(row, col);
            return Matrix[row][col];
        }

        private void CheckBounds(int row, int col)
        {
            if (Rows <= row || row < 0)
                throw new ChromaNetCoreApiException($"The row index is out of range {row}");
            if (Cols <= col || col < 0)
                throw new ChromaNetCoreApiException($"The column index is out of range {col}");
        }
    }
}

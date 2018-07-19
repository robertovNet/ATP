using ATP.Common.Enums;
using System.Collections.Generic;

namespace ATP.Common.Entities
{
    public class Tendencia
    {
        public TipoTendencia Tipo { get; set; }

        public IEnumerable<ChartDataCandle> Candles { get; set; }
    }
}

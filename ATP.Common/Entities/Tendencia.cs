using ATP.Common.Enums;
using ATP.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ATP.Common.Entities
{
    public class Tendencia
    {
        public TipoTendencia Tipo { get; set; }

        public IEnumerable<ChartDataCandle> Candles { get; set; }

        public DateTime Start
        {
            get
            {
                if (!Candles.HasElements())
                    return default(DateTime);

                return Candles.First().DateTime;
            }
        }

        public DateTime End
        {
            get
            {
                if (!Candles.HasElements())
                    return default(DateTime);

                return Candles.Last().DateTime;
            }
        }

        public double Open
        {
            get
            {
                if (!Candles.HasElements())
                    return 0d;

                return Candles.First().Open;
            }
        }

        public double Close
        {
            get
            {
                if (!Candles.HasElements())
                    return 0d;

                return Candles.Last().Close;
            }
        }

        public double PercentChange
        {
            get
            {
                if (!Candles.HasElements())
                    return 0d;

                return ((Close - Open) / Open);
            }
        }

        public override string ToString()
        {
            var descripcion = $@"Start: {Start:dd/MM/yyyy HH:mm} End: {End:dd/MM/yyyy HH:mm} Open: {Open:N8} Close: {Close:N8} Change: {PercentChange:P3} Var: {Tipo.GetEnumDescription()}";
            return descripcion;
        }
    }
}

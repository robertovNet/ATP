using ATP.Common.Enums;
using System;

namespace ATP.Common.Entities
{
    public class ChartDataCandle
    {
        public long Date { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public double Volume { get; set; }

        public double QuoteVolume { get; set; }

        public double WeightedAverage { get; set; }

        public DateTime DateTime => DateTimeOffset.FromUnixTimeSeconds(Date).DateTime;

        public double PercentChange => (Close - Open) / Open;

        public TipoVariacion TipoVariacion
        {
            get
            {
                if (PercentChange > 12.0d)
                    return TipoVariacion.AlzaFuerte;

                if (PercentChange > 7.0d)
                    return TipoVariacion.AlzaModerada;

                if (PercentChange > 3.0d)
                    return TipoVariacion.AlzaEstabilizacion;

                if (PercentChange < -12.0d)
                    return TipoVariacion.BajaFuerte;

                if (PercentChange < -7.0d)
                    return TipoVariacion.BajaModerada;

                if (PercentChange < -3.0d)
                    return TipoVariacion.BajaEstabilizacion;

                return TipoVariacion.Despreciable;
            }
        }
    }
}

using ATP.Common.Enums;
using ATP.Common.Extensions;
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

        public double PercentChange => ((Close - Open) / Open);

        public TipoVariacion TipoVariacion
        {
            get
            {
                if (PercentChange > 0.12d)
                    return TipoVariacion.AlzaFuerte;

                if (PercentChange > 0.07d)
                    return TipoVariacion.AlzaModerada;

                if (PercentChange > 0.03d)
                    return TipoVariacion.AlzaEstabilizacion;

                if (PercentChange < -0.12d)
                    return TipoVariacion.BajaFuerte;

                if (PercentChange < -0.07d)
                    return TipoVariacion.BajaModerada;

                if (PercentChange < -0.03d)
                    return TipoVariacion.BajaEstabilizacion;

                return TipoVariacion.Despreciable;
            }
        }

        public override string ToString()
        {
            var description = $@"Date: {DateTime:dd/MM/yyyy HH:mm} Open: {Open:N8} High: {High:N8} Low: {Low:N8} Close: {Close:N8} V: {Volume:N2} QV: {QuoteVolume:N2} WAvg: {WeightedAverage:N8} Change: {PercentChange:P3} Var: {TipoVariacion.GetEnumDescription()}";
            return description;
        }
    }
}

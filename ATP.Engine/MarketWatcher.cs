using ATP.Common.Entities;
using ATP.Common.Entities.Requests;
using ATP.Common.Enums;
using ATP.Common.Helpers;
using ATP.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Timers;
using System.Linq;
using System.Configuration;
using ATP.Common.Contract.IServices;
using ATP.Common.Logic.Services;

namespace ATP.Engine
{
    public class MarketWatcher
    {
        private string publicExUrl;
        private double interval;
        private Timer timer;
        private ISubscriptionService subscriptionService;

        public MarketWatcher()
        {
            Init();
            RequestChartData();
            //OnTimeElapsed(null, null);
        }

        private void Init()
        {
            publicExUrl = ConfigurationManager.AppSettings["publicExURL"];
            interval = 10000d;
            timer = new Timer(interval);
            subscriptionService = new SubscriptionService();
            //SetTimer();
        }

        private void SetTimer()
        {
            timer.Elapsed += OnTimeElapsed;
            timer.AutoReset = true;
            timer.Start();
        }

        private async void OnTimeElapsed(object source, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            var subscribers = await subscriptionService.GetSubscribersAsync().ConfigureAwait(false);

            if (!subscribers.HasElements())
                return;

            var recipients = subscribers.Select(s => s.Email);
            var subject = "Trader Test";
            var bodyTemplate = "<h1>Hola</h1><br/>Mensaje enviado el {0} a las {1}";
            var templateParams = new[] { now.ToShortDateString(), now.ToShortTimeString() };

            await EmailHelper.SendAsync(recipients, subject, bodyTemplate, templateParams);
        }

        private async void RequestChartData()
        {
            var testPair = ConfigurationManager.AppSettings["testPair"];
            var request = new ChartDataRequest(testPair, DateTime.UtcNow.AddHours(-10), DateTime.UtcNow, CandlestickPeriod.MediaHora);
            var candles = Enumerable.Empty<ChartDataCandle>();

            using (var client = new HttpClient())
            {
                candles = await client.GetAsync<IEnumerable<ChartDataCandle>>(new EndPointConfiguration { Address = publicExUrl }, request);
            }

            if (!candles.HasElements())
                return;

            Tendencia tendencia = null;
            var tendencias = new List<Tendencia>();
            var candlesTendencia = new List<ChartDataCandle>();
            TipoTendencia? ultimoTipoTendencia = null;

            var variacionesNegativas = new[] { TipoVariacion.BajaEstabilizacion, TipoVariacion.BajaModerada, TipoVariacion.BajaFuerte };
            var variacionesPositivas = new[] { TipoVariacion.AlzaEstabilizacion, TipoVariacion.AlzaModerada, TipoVariacion.AlzaFuerte };

            foreach (var candle in candles)
            {
                var tipoTendenciaActual = TipoTendencia.Estable;

                if (variacionesNegativas.Contains(candle.TipoVariacion))
                    tipoTendenciaActual = TipoTendencia.Baja;
                else if (variacionesPositivas.Contains(candle.TipoVariacion))
                    tipoTendenciaActual = TipoTendencia.Alza;

                if (!ultimoTipoTendencia.HasValue)
                    tendencia = new Tendencia { Tipo = tipoTendenciaActual };
                else if (ultimoTipoTendencia.Value != tipoTendenciaActual)
                {
                    tendencia.Candles = candlesTendencia.OrderBy(ct => ct.DateTime);
                    tendencias.Add(tendencia);
                    tendencia = new Tendencia { Tipo = tipoTendenciaActual };
                    candlesTendencia = new List<ChartDataCandle>();
                }

                candlesTendencia.Add(candle);

                ultimoTipoTendencia = tipoTendenciaActual;
            }

            tendencia.Candles = candlesTendencia;
            tendencias.Add(tendencia);
            var tendenciasResult = tendencias.OrderBy(t => t.Start);
        }
    }
}

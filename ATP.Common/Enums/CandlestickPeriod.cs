using System.ComponentModel;
using System.Runtime.Serialization;

namespace ATP.Common.Enums
{
    public enum CandlestickPeriod
    {
        [Description("5 Minutos")]
        [EnumMember(Value = "300")]
        CincoMinutos = 300,

        [Description("15 Minutos")]
        [EnumMember(Value = "900")]
        QuinceMinutos = 900,

        [Description("30 Minutos")]
        [EnumMember(Value = "1800")]
        MediaHora = 1800,

        [Description("2 Horas")]
        [EnumMember(Value = "7200")]
        DosHoras = 7200,

        [Description("4 Horas")]
        [EnumMember(Value = "14400")]
        CuatroHoras = 14400,

        [Description("1 Día")]
        [EnumMember(Value = "86400")]
        UnDia = 86400
    }
}

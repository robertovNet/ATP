using System.ComponentModel;

namespace ATP.Common.Enums
{
    public enum TipoTendencia
    {
        [Description("Baja Sostenida")]
        BajaSostenida,

        [Description("Baja")]
        Baja,

        [Description("Baja Estabilizando")]
        BajaEstabilizando,

        [Description("Estable")]
        Estable,

        [Description("Alza Estabilizando")]
        AlzaEstabilizando,

        [Description("Alza")]
        Alza,

        [Description("Alza Sostenida")]
        AlzaSostenida
    }
}

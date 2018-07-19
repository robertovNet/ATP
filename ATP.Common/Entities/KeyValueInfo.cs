namespace ATP.Common.Entities
{
    public class KeyValueInfo
    {
        #region CONSTRUCTOR

        public KeyValueInfo()
        {
        }

        public KeyValueInfo(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public KeyValueInfo(int id, string nombre, string codigo)
        {
            Id = id;
            Nombre = nombre;
            Codigo = codigo;
        }

        #endregion CONSTRUCTOR

        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Codigo { get; set; }
    }
}

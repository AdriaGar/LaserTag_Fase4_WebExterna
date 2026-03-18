namespace Fase4_WebExterna.Models
{
    public class PerfilJugadorStats
    {
        public string id_jugador { get; set; }
        public ResumenStats resumen { get; set; }
        public List<HistorialStats> historial { get; set; }
    }

    public class ResumenStats
    {
        public int partides_jugades { get; set; }
        public int total_kills { get; set; }
        public int total_morts { get; set; }
        public int total_punts { get; set; }
    }

    public class HistorialStats
    {
        public string id_partida { get; set; }
        public int punts { get; set; }
        public int kills { get; set; }
        public int morts { get; set; }
        public int posicio { get; set; }
    }
}
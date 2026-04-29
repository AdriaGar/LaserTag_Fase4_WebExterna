public class RankingJugador
{
    public string id_jugador { get; set; }
    public string nom { get; set; }
    public string nickname { get; set; }
    public int total_punts { get; set; }
    public int total_kills { get; set; }
    public int total_morts { get; set; }
    public int partides_jugades { get; set; }
    public int posicio { get; set; }

}

public class RankingResponse
{
    public List<RankingJugador> ranking { get; set; }
    public int total { get; set; }
}
namespace clases
{
    public class Personaje
    {
        public HttpClient client = new HttpClient();
        public float Fuerza { get; set; }
        public float Velocidad { get; set; }
        public float Suerte { get; set; }
        public bool turno { get; set; }
    }
}

public class PersonajesJson
{
    void GuardarPersonajes(List<Personaje> pers , string url)
    {
        
    }


    List<string> PedirPalabras(lenguaje idio, int cantidad, int largo, HttpClient client)
    {
        url = $"https://random-word-api.herokuapp.com/word?lang={idio}&number={cantidad}&length={largo}";

        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        List<string> palabras = JsonSerializer.Deserialize <List<string>>(responseBody);

        return palabras;
    }

}

public enum lenguaje
{
    es,
    de,
    fr,
    it,
    zh
}
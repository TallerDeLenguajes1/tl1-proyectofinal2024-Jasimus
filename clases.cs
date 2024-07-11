using System.Diagnostics;
using System.Text.Json;
namespace clases
{
    public class Personaje
    {
        private Random randi = new Random();
        private float sumaVel = 0;
        private int cantPalabrasEs = 0;

        public HttpClient Client = new HttpClient();
        public int Fuerza { get; set; }
        public int Velocidad { get; set; }
        public int Suerte { get; set; }
        public bool turno { get; set; }
        public float SumaVelocidades { get => sumaVel; set => sumaVel = value; }
        public int CantPalabrasEscritas { get => cantPalabrasEs; set => cantPalabrasEs = value; }

        public float VelocidadMedia()
        {
            if(cantPalabrasEs == 0)
            {
                return 0;
            }
            else
            {
                return sumaVel/cantPalabrasEs;
            }
        }
        
        public double FuerzaGolpe(double S, double media, int cantGolpes, int TiempoRestante, int TiempoDisponible)
        {
            double b = -media*Math.Log(S);
            double MaxRand = 1/(Math.Exp((cantGolpes/2 - 1)*Math.Log(S)+Math.Log(b))+1);
            int numero = randi.Next(0, (int)MaxRand);
            
            return 1 + Sigmoide_inversa(numero, S, b)*Velocidad;
        }

        private double Sigmoide_inversa(double x, double S, double b)       //mapea los valores de una V.A con dist. uniforme a una con distribución aprox. normal
        {
            return Math.Log(1/S*x - 1/S)/Math.Log(b);
        }

        public void CalcularVelocidad(int TiempoDisponible, int TiempoRestante)
        {
            Velocidad = TiempoRestante/TiempoDisponible;
        }

    }

    public class Contrincante
    {
        public float cantGolpesInicial { get; set; }
        public Lenguaje idioma { get; set; }
    }

    public class Arbol
    {
        private bool cayo = false;

        public int LadoJugador { get; set; }
        public double LadoOtro { get; set; }
        public bool Cayo { get => cayo; };
        
        public void LadoJugadorMetodo(int cantGolpesInicial, int fuerzaPj)
        {
            LadoJugador = cantGolpesInicial - fuerzaPj;
        }

        public void LadoContrincante(int Nronda, int CantRondas, float VelocidadMediaPj)
        {
            LadoOtro = (LadoJugador - 1) - Nronda/CantRondas * (LadoJugador - 3) * VelocidadMediaPj;
        }

        public void GolpeJugador(double fuerzaGolpe)
        {
            LadoJugador -= Math.Round(fuerzaGolpe);

            if(LadoJugador <= 0)
            {
                cayo = true;
            }
        }

        public void GolpeContrincante()
        {
            LadoOtro--;

            if(LadoOtro <= 0)
            {
                cayo = true;
            }
        }
    }


    public class Partida()
    {
        public static int Nronda { get; set; }
        public static int CantRondas { get; set; }
        private static int TiempoDisponible { get; set; }
        private static int TiempoRestante { get; set; }
        private static string Palabra { get; set; }
        private static bool Critico { get; set; }

        static string Palabra = string.Empty;
        static Thread MainThread = Thread.CurrentThread;
        static System.Threading.Timer timer = null;
        static Stopwatch reloj = new Stopwatch();

        public void IniciarTurno()
        {
            try
            {
                timer = Timer(esperarPalabra, null, 0, Timeout.Infinite);
                Console.WriteLine("> ");
                reloj.Start();
                Thread.Sleep(TiempoDisponible);
                reloj.Stop();
                timer.Dispose();
                Critico = false;
                Console.WriteLine("perdiste tu turno");
            }
            catch(TimerInterruptedException)
            {
                if(reloj.ElapsedMilliseconds <= TiempoDisponible/3)
                {
                    Critico = true;
                    TiempoRestante = TiempoDisponible;
                }
                else
                {
                    TiempoRestante = TiempoDisponible - reloj.ElapsedMilliseconds;
                }
            }
        }

        private static esperarPalabra(object state)
        {
            Palabra = Console.ReadLine();
            MainThread.Interrupt();
        }

        async Task<List<string>> PedirPalabras(Lenguaje idio, int cantidad, int largo, HttpClient client)
        {
            string url = $"https://random-word-api.herokuapp.com/word?lang={idio}&number={cantidad}&length={largo}";

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            List<string> palabras = JsonSerializer.Deserialize <List<string>>(responseBody);

            return palabras;
        }
    }

    public class PersonajesJson
    {
        void GuardarPersonajes(List<Personaje> pers , string url)
        {
            
        }


    }

    public enum Lenguaje
    {
        es,
        de,
        fr,
        it
    }
}
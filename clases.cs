using System.Diagnostics;
using System.Text.Json;
using System.Threading;


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
        public double Suerte { get; set; }
        public bool Turno { get; set; }
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
        
        public double FuerzaGolpe(int cantGolpes)
        {
            double media = (cantGolpes/4 - 1)*Suerte;
            double S = 100*media;
            double b = Math.Exp(media*Math.Log(S));
            double MaxRand = 1/(Math.Exp(-(cantGolpes/2 - 1)*Math.Log(S)+Math.Log(b))+1);
            double numero = randi.NextDouble() * MaxRand;
            
            return 1 + Math.Abs(Sigmoide_inversa(numero, S, b))*Velocidad;
        }

        private double Sigmoide_inversa(double x, double S, double b)       //mapea los valores de una V.A con dist. uniforme a una con distribución aprox. normal
        {
            return ((Math.Log(b)-Math.Log(1/x - 1))/Math.Log(S));
        }

        public void CalcularVelocidad(int TiempoDisponible, int TiempoRestante)
        {
            Velocidad = TiempoRestante/TiempoDisponible;
            sumaVel += Velocidad;
            cantPalabrasEs++;
        }

        public void AumentarFuerza()
        {
            Fuerza++;
        }

        public void AumentarVelocidad()
        {
            Velocidad++;
        }

        public void AumentarSuerte(int cantRondas)
        {
            Suerte += 2/cantRondas;
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
        public int CantidadGolpesTotalpj { get; set; }
        public double LadoOtro { get; set; }
        public bool Cayo { get => cayo; }
        
        public void LadoJugadorMetodo(int cantGolpesInicial, int fuerzaPj)
        {
            CantidadGolpesTotalpj = cantGolpesInicial - fuerzaPj;
            LadoJugador = CantidadGolpesTotalpj;
        }

        public void LadoContrincante(int Nronda, int CantRondas, float VelocidadMediaPj)
        {
            LadoOtro = (LadoJugador - 1) - Nronda/CantRondas * (LadoJugador - 3) * VelocidadMediaPj;
        }

        public void GolpeJugador(double fuerzaGolpe)
        {
            LadoJugador -=(int) Math.Round(fuerzaGolpe);

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
        private static int tiempoDisponible = 5000;
        private static bool critico = false;
        private static bool escrito = false;
        private static string palabra = null;

        public int Nronda { get; set; }
        public int CantRondas { get; set; }
        public int TiempoRestante { get; set; }
        public int TiempoDisponible { get => tiempoDisponible; }
        public string Palabra { get => palabra; }
        public bool Critico { get => critico; }
        public bool Escrito { get => escrito; }

        static Thread MainThread = Thread.CurrentThread;
        static TimerCallback callback = new TimerCallback(esperarPalabra);
        static Stopwatch reloj = new Stopwatch();

        public void IniciarTurno()
        {
            try
            {
                critico = false;
                System.Threading.Timer timer = new Timer(callback, null, 0, Timeout.Infinite);
                Console.WriteLine("> ");
                reloj.Start();
                Thread.Sleep(tiempoDisponible);
                reloj.Reset();
                timer.Dispose();
                escrito = false;
                Console.WriteLine("perdiste tu turno");
            }
            catch(ThreadInterruptedException)
            {
                reloj.Stop();
                escrito = true;
                if(reloj.ElapsedMilliseconds <= tiempoDisponible/3)
                {
                    critico = true;
                    TiempoRestante = tiempoDisponible;
                }
                else
                {
                    TiempoRestante = tiempoDisponible - (int)reloj.ElapsedMilliseconds;
                }
                reloj.Reset();
            }
        }

        private static void esperarPalabra(object state)
        {
            palabra = Console.ReadLine();
            MainThread.Interrupt();
        }

        public async Task<List<string>> PedirPalabras(Lenguaje idio, int cantidad, int largo, HttpClient client)
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
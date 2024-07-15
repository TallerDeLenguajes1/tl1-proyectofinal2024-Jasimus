using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace clases
{
    public class Personaje
    {
        private Random randi = new Random();
        private double sumaVel = 0;
        private int cantPalabrasEs = 0;
        private double palabrasVel = 0;

        public HttpClient Client = new HttpClient();
        public int Fuerza { get; set; }
        public int Velocidad { get; set; }
        public double Suerte { get; set; }
        public bool Turno { get; set; }
        public double SumaVelocidades { get => sumaVel; set => sumaVel = value; }
        public int CantPalabrasEscritas { get => cantPalabrasEs; set => cantPalabrasEs = value; }

        public double VelocidadMedia()
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
        
        public double FuerzaGolpe(int cantGolpes, int cantRondas)
        {
            double media = (double)cantGolpes/4 - 1 + (double)cantGolpes/4*(Suerte/cantRondas);
            double S = 100*media;
            double b = Math.Exp(media*Math.Log(S));
            double MaxRand = 1/(Math.Exp(-(cantGolpes/2 - 1)*Math.Log(S)+Math.Log(b))+1);
            double numero = randi.NextDouble() * MaxRand;
            
            return 1 + Math.Abs(Sigmoide_inversa(numero, S, b))*palabrasVel;
        }

        private double Sigmoide_inversa(double x, double S, double b)       //mapea los valores de una V.A con dist. uniforme a una con distribución aprox. normal
        {
            return (Math.Log(b)-Math.Log(1/x - 1))/Math.Log(S);
        }

        public void CalcularVelocidad(int TiempoDisponible, int TiempoRestante)
        {
            palabrasVel = (double)TiempoRestante/TiempoDisponible;
            sumaVel += palabrasVel;
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

        public void AumentarSuerte()
        {
            Suerte ++;
        }

    }

    public class Arbol
    {
        private bool cayo = false;

        public int LadoJugador { get; set; }
        public int CantidadGolpesTotalpj { get; set; }
        public double LadoOtro { get; set; }
        public bool Cayo { get => cayo; }
        public bool Sigue { get; set; }
        
        public void LadoJugadorMetodo(int cantGolpesInicial, int fuerzaPj)
        {
            CantidadGolpesTotalpj = cantGolpesInicial - fuerzaPj;
            LadoJugador = CantidadGolpesTotalpj;
        }

        public void LadoContrincante(int Nronda, int CantRondas, double VelocidadMediaPj)
        {
            LadoOtro = CantidadGolpesTotalpj - 1 - Nronda/CantRondas * (CantidadGolpesTotalpj - 3) * VelocidadMediaPj;
        }

        public void GolpeJugador(double fuerzaGolpe)
        {
            LadoJugador -=(int) Math.Round(fuerzaGolpe);

            if(LadoJugador <= 0)
            {
                cayo = true;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("el árbol cayó para TU lado. GANASTE");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void GolpeContrincante(double fuerzaGolpeOtro)
        {
            LadoOtro -= (int) Math.Round(fuerzaGolpeOtro);

            if(LadoOtro <= 0)
            {
                cayo = true;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("el árbol cayó para el lado del otro. PERDISTE");
                Console.ForegroundColor = ConsoleColor.White;
                Sigue = false;
            }
        }
    }


    public class Partida()
    {
        private static int tiempoDisponible = 5000;
        private static bool critico = false;
        private static bool escrito = false;
        private static string? palabra = string.Empty;
        private static int tiempoOtro;
        private static Random randOtro = new Random();
        private static bool criticoOtro = false;

        public int Nronda { get; set; }
        public int CantRondas { get; set; }
        public int TiempoRestante { get; set; }
        public int TiempoDisponible { get => tiempoDisponible; }
        public string? Palabra { get => palabra; }
        public bool Critico { get => critico; }
        public bool Escrito { get => escrito; }
        public bool CriticoOtro { get => criticoOtro; }

        static Thread MainThread = Thread.CurrentThread;
        static TimerCallback callback = new TimerCallback(esperarPalabra);
        static Stopwatch reloj = new Stopwatch();

        public void IniciarTurno()
        {
            try
            {
                critico = false;
                Timer timer = new Timer(callback, null, 0, Timeout.Infinite);
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

        public void IniciarTurnoOtro(int VelocidadOtro)
        {
            Console.WriteLine("es turno del otro:");
            tiempoOtro = randOtro.Next(tiempoDisponible-VelocidadOtro, tiempoDisponible);
            criticoOtro = false;
            Thread.Sleep(tiempoOtro);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(@"\ /"+"\n"+@"/ \");
            Thread.Sleep(500);
            Console.WriteLine("Ya golpeó");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(400);
            if(tiempoOtro < tiempoDisponible/3)
            {
                criticoOtro = true;
            }
        }

        private static void esperarPalabra(object state)
        {
            palabra = Console.ReadLine();
            MainThread.Interrupt();
            byte[] bytes = Encoding.Default.GetBytes(palabra);
            palabra = Encoding.UTF8.GetString(bytes);
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

        public Personaje GenerarOtro()
        {
            Personaje p = new Personaje();
            Random rand = new Random();
            int fuerza = rand.Next(0, 3);
            int velocidad = rand.Next(0, 3);
            int suerte = rand.Next(0, 3);

            p.Fuerza = fuerza;
            p.Velocidad = velocidad;
            p.Suerte = suerte;

            return p;
        }
    }

    public class PersonajesJson
    {
        public void GuardarPersonajes(List<Personaje> pers)
        {
            string ruta = @"C:\Users\Juan\OneDrive\Escritorio\Facultad\3er_anio-1er_cuat\taller_de_lenguajes_I\tl1-proyectofinal2024-Jasimus\personajes.json";
            List<string> persS = new List<string>();
            if (!File.Exists(ruta))
            {
                foreach(Personaje p in pers)
                {
                    string pS = JsonSerializer.Serialize(p);
                    persS.Add(pS);
                }
                File.WriteAllLines(ruta, persS);
            }
            else
            {
                foreach(Personaje p in pers)
                {
                    string pS = JsonSerializer.Serialize(p);
                    persS.Add(pS);
                }
                File.AppendAllLines(ruta, persS);
            }
        }

        public List<Personaje> ObtenerPersonajes(int cant)
        {
            string ruta = @"C:\Users\Juan\OneDrive\Escritorio\Facultad\3er_anio-1er_cuat\taller_de_lenguajes_I\tl1-proyectofinal2024-Jasimus\personajes.json";
            List<Personaje> lp = new List<Personaje>();
            using (StreamReader reader = new StreamReader(ruta))
            {
                for (int i = 0; i < cant; i++)
                {
                    string line = reader.ReadLine();
                    if (line != null)
                    {
                        Personaje p = JsonSerializer.Deserialize<Personaje>(line);
                        lp.Add(p);
                    }
                    else
                    {
                        break; // Si se llega al final del archivo, se termina la lectura
                    }
                }
            }
            return lp;
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
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.IO;

namespace clases
{
    public class Personaje
    {
        private Random randi = new Random();
        private double sumaFG = 0;
        private int cantPalabrasEs = 0;
        private double palabrasVel = 1;
        
        public HttpClient Client = new HttpClient();
        public int Fuerza { get; set; }
        public int Velocidad { get; set; }
        public int Suerte { get; set; }
        public bool Turno { get; set; }
        public double SumaFG { get => sumaFG; set => sumaFG = value; }
        public int CantPalabrasEscritas { get => cantPalabrasEs; set => cantPalabrasEs = value; }

        public double FuerzaGolpeMedia()
        {
            if(cantPalabrasEs == 0)
            {
                return 1;
            }
            else
            {
                return sumaFG/cantPalabrasEs;
            }
        }
        
        public double FuerzaGolpe(int cantGolpes, int cantRondas)
        {

            double media = (double)cantGolpes/4 + (double)cantGolpes/4*(Suerte/cantRondas);
            double Maximo = cantGolpes/2;
            double poli = 4.4*(media/Maximo) - 0.8*Math.Pow(media/Maximo, 2);
            double S = Math.Exp(poli);
            double b = Math.Exp(media*Math.Log(S));
            double MaxRand = 1/(Math.Exp(-(cantGolpes/2)*Math.Log(S)+Math.Log(b))+1);
            double numero = randi.NextDouble() * MaxRand;
            double fuerzaG = Math.Abs(Sigmoide_inversa(numero, S, b, MaxRand))*palabrasVel + Fuerza;
            cantPalabrasEs++;
            sumaFG += fuerzaG;

            return fuerzaG;
        }

        private double Sigmoide_inversa(double x, double S, double b, double max)       //mapea los valores de una V.A con dist. uniforme a una con distribución aprox. normal
        {
            return (Math.Log(b)-Math.Log(max/x - 1))/Math.Log(S);
        }

        public void CalcularVelocidad(int TiempoDisponible, int TiempoRestante)
        {
            palabrasVel = (double)TiempoRestante/TiempoDisponible;
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
            Suerte++;
        }

    }

    public class Arbol
    {
        private bool cayo = false;

        public int LadoJugador { get; set; }
        public int CantidadGolpesTotalpj { get; set; }
        public double LadoOtro { get; set; }
        public bool Cayo { get => cayo; set => cayo = value; }
        public bool Sigue { get; set; }
        
        public void LadoJugadorMetodo(int cantGolpesInicial)
        {
            CantidadGolpesTotalpj = cantGolpesInicial;
            LadoJugador = CantidadGolpesTotalpj;
        }

        public void LadoContrincante()
        {
            LadoOtro = CantidadGolpesTotalpj;
        }

        public void GolpeJugador(double fuerzaGolpe)
        {
            LadoJugador -=(int) Math.Round(fuerzaGolpe);

            if(LadoJugador <= 0)
            {
                cayo = true;
            }
        }

        public void GolpeContrincante(double fuerzaGolpeOtro)
        {
            LadoOtro -= (int) Math.Round(fuerzaGolpeOtro);

            if(LadoOtro <= 0)
            {
                cayo = true;
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
        private static bool escribir = true;

        public int Nronda { get; set; }
        public int CantRondas { get; set; }
        public int TiempoRestante { get; set; }
        public int TiempoDisponible { get => tiempoDisponible; }
        public string? Palabra { get => palabra; }
        public bool Critico { get => critico; }
        public bool Escrito { get => escrito; }
        public bool CriticoOtro { get => criticoOtro; }

        static Stopwatch reloj = new Stopwatch();
        static Thread MainThread = Thread.CurrentThread;
        static Object lockObject = new Object();

        public void IniciarTurno()
        {
            try
            {
                MainThread = Thread.CurrentThread;
                Thread nuevoThread = new Thread(esperarPalabra);
                while(Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
                Console.Write("> ");
                critico = false;
                escribir = true;
                nuevoThread.Start();
                reloj.Start();
                Thread.Sleep(tiempoDisponible);
                escribir = false;
                reloj.Reset();
                lock(lockObject)
                {
                    escrito = false;
                }
                Console.WriteLine("\nperdiste tu turno");
            }
            catch(ThreadInterruptedException)
            {
                lock (lockObject)
                {
                    escrito = true;
                }
                reloj.Stop();
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

        private static void esperarPalabra()
        {
            string input = "";
            while (escribir)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Enter)
                    {
                        reloj.Stop();
                        MainThread.Interrupt();
                        Console.WriteLine();
                        break;
                    }

                    if(key.Key == ConsoleKey.Backspace)
                    {
                        if(input.Length > 0)
                        {
                            input = input.Substring(0, input.Length - 1);
                            Console.Write(" ");
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }
                    }
                    else
                    {
                        if(key.KeyChar != (char)0)
                        {
                            input += key.KeyChar;
                        }
                        else
                        {
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }
                    }
                }
            }

            palabra = input;
        }

        public void IniciarTurnoOtro(int VelocidadOtro)
        {
            Console.WriteLine("es turno del otro:");
            tiempoOtro = randOtro.Next(tiempoDisponible-VelocidadOtro*2, tiempoDisponible);
            criticoOtro = false;
            Thread.Sleep(tiempoOtro);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(@"\ /"+"\n"+@"/ \");
            Thread.Sleep(500);
            Console.WriteLine("Ya golpeó");
            Console.ForegroundColor = ConsoleColor.White;
            if(tiempoOtro < tiempoDisponible/3)
            {
                criticoOtro = true;
            }
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

        public int CalcularCantidadPalabras(int cantPalabras, int fuerzaOtro)
        {
            if(fuerzaOtro == 0)
            {
                return 20;
            }
            else
            {
                return cantPalabras/fuerzaOtro + 1;
            }
        }

        public Personaje GenerarOtro(int fuerzaPj, int velocidadPj, int suertePj)
        {
            Personaje p = new Personaje();
            Random rand = new Random();
            int fuerza = rand.Next(0, 2) + fuerzaPj;
            int velocidad = rand.Next(0, 2) + velocidadPj;
            int suerte = rand.Next(0, 2) + suertePj;

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
            string ruta = @".\personajes.json";
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
            string ruta = @".\personajes.json";
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
        fr,
        it,
        de
    }
}
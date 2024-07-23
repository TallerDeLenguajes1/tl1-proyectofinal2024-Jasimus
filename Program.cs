using clases;
// using pruebas;
using System.Text;
Console.InputEncoding = Encoding.GetEncoding("iso-8859-1");
Console.OutputEncoding = Encoding.UTF8;
Partida partida = new Partida();
Personaje pj1 = new Personaje();
Personaje Otro = new Personaje();
Arbol arbol = new Arbol();
Stream streamEntrada = Console.OpenStandardInput();
string alias;

string mejora;
string cantRondas;
string tipoLeniador;
int cantRondasInt;
int rondaActual = 1;
int cantPalabras = 5;
int LargoPalabras = 3;
double fuerzaGolpe;
bool sePudoAumentar = false;

List<string> PalabrasRonda = new List<string>();
Random idiomaRandom = new Random();
Console.Clear();
do
{
    Console.Write("ingrese la cantidad de rondas (mayor a cero): ");
    cantRondas = Console.ReadLine();
}while(!int.TryParse(cantRondas, out cantRondasInt) && cantRondasInt <= 0);

do
{
    Console.Write("ingrese el alias de su leñador: ");
    alias = Console.ReadLine();
}while(alias == string.Empty);

do
{
    Console.WriteLine("ingrese el tipo del leñador:\n> fuerte\n> hábil\n> ágil\n");
    tipoLeniador = Console.ReadLine();
}while(string.Compare(tipoLeniador, "fuerte") != 0 && string.Compare(tipoLeniador, "hábil") != 0 && string.Compare(tipoLeniador, "ágil") != 0);

switch (tipoLeniador)
{
    case "fuerte":
        pj1.Fuerza = 1;
        pj1.Suerte = 0;
        pj1.Velocidad = 0;
        break;

    case "hábil":
        pj1.Fuerza = 0;
        pj1.Suerte = 2;
        pj1.Velocidad = 0;
        break;
    
    case "ágil":
        pj1.Fuerza = 0;
        pj1.Suerte = 0;
        pj1.Velocidad = 2;
        break;
}
Console.Clear();
arbol.Sigue = true;
PersonajesJson personajesJson = new PersonajesJson();
List<Personaje> PContra = personajesJson.ObtenerPersonajes(1);
List<Personaje> PContraPartida = new List<Personaje>();

while(true)
{
    if(PContra.Count() != 0)
    {
        Otro = PContra[0];
    }
    else
    {
        Otro = partida.GenerarOtro(pj1.Fuerza, pj1.Velocidad, pj1.Suerte);
        PContraPartida.Add(Otro);
    }

    if (arbol.Sigue)
    {
        arbol.LadoJugadorMetodo(cantPalabras);
        arbol.LadoContrincante();
        Lenguaje idioma = (Lenguaje)idiomaRandom.Next(0, 3);
        PalabrasRonda = await partida.PedirPalabras(idioma/*Lenguaje.es*/, 20, LargoPalabras - pj1.Velocidad, pj1.Client);
        // PalabrasRonda = ["a", "s", "d", "f", " ", "mañana", "sin duda", "cuál año", "dea", "el chuqui", "dabo", "el chocas", "el rubius", "y así"];
        int indice = 0;
        pj1.Turno = true;
        arbol.Cayo = false;
        while(true)
        {
            if(pj1.Turno)
            {
                Console.WriteLine("es tu turno\nEscribe en:");
                for(int i = 3; i > 0; i--)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(1000);
                }
                Console.WriteLine(PalabrasRonda[indice]);

                streamEntrada.Flush();
                partida.IniciarTurno(pj1);
                pj1.CalcularVelocidad(partida.TiempoDisponible, partida.TiempoRestante);
                if(partida.Escrito)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Achazo");
                    Console.WriteLine("cant palabras escritas: "+pj1.CantPalabrasEscritas);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if(partida.Escrito && string.Compare(partida.Palabra, PalabrasRonda[indice]) == 0)
                {
                    fuerzaGolpe = pj1.FuerzaGolpe(arbol.CantidadGolpesTotalpj, cantRondasInt);
                    arbol.GolpeJugador(fuerzaGolpe);
                    if(partida.Critico)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("crítico: "+fuerzaGolpe);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("GOLPE: "+fuerzaGolpe);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FALLO");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Thread.Sleep(1000);
                pj1.Turno = false;
                indice++;
                if(indice > PalabrasRonda.Count - 1)
                {
                    indice = 0;
                }
            }
            else
            {
                partida.IniciarTurnoOtro(pj1.VelocidadMedia(), Otro.CoefImportancia);
                Otro.CalcularVelocidad(partida.TiempoDisponible, partida.TiempoRestanteOtro);
                if(partida.GolpeOtro)
                {
                    double fuerzaGolpeOtro = Otro.FuerzaGolpe(arbol.CantidadGolpesTotalpj, cantRondasInt);
                    arbol.GolpeContrincante(fuerzaGolpeOtro);
                    if(partida.CriticoOtro)
                    {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("hizo crítico: "+fuerzaGolpeOtro);
                            Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Thread.Sleep(1000);
                pj1.Turno = true;
            }
            if(!arbol.Cayo)
            {
                Console.Clear();
            }
            else
            {
                break;
            }
        }
        if(arbol.Sigue)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("el árbol cayó para TU lado. GANASTE");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1000);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("el árbol cayó para el lado del otro. PERDISTE");
            Console.ForegroundColor = ConsoleColor.White;
        }
        PContra.Remove(Otro);
        PalabrasRonda.Clear();
        rondaActual++;
        if(rondaActual <= cantRondasInt && arbol.Sigue)
        {
            Console.WriteLine("Pasas a la siguiente ronda");
            Thread.Sleep(2000);
            Console.Clear();
            if(LargoPalabras <= 7)
            {
                LargoPalabras++;
            }
            if(cantPalabras <= 10)
            {
                cantPalabras++;
            }
            
            while(!sePudoAumentar)
            {    
                do
                {
                    Console.WriteLine("¿Qué deseas mejorar?\nFuerza\nVelocidad\nSuerte");
                    mejora =Console.ReadLine();
                }while(string.Compare(mejora, "Fuerza") != 0 && string.Compare(mejora, "Velocidad") != 0 && string.Compare(mejora, "Suerte") != 0);
                switch(mejora)
                {
                    case "Fuerza":
                        if(pj1.Fuerza <= cantPalabras/2)
                        {
                            pj1.AumentarFuerza();
                            Console.WriteLine("aumentaste tu fuerza a: "+pj1.Fuerza);
                            sePudoAumentar = true;
                        }
                        else
                        {

                            Console.WriteLine("no pudiste aumentar tu fuerza");
                        }
                        break;
                    case "Velocidad":
                        if(pj1.Velocidad <= LargoPalabras - 2)
                        {
                            pj1.AumentarVelocidad();
                            Console.WriteLine("aumentaste tu velocidad a: "+pj1.Velocidad);
                            sePudoAumentar = true;
                        }
                        else
                        {
                            Console.WriteLine("no pudiste aumentar tu velocidad");
                        }
                        break;
                    case "Suerte":
                        pj1.AumentarSuerte();
                        Console.WriteLine("aumentaste tu suerte a: "+pj1.Suerte);
                        sePudoAumentar = true;
                        break;
                }
            }
            sePudoAumentar = false;
            Thread.Sleep(1000);
            Console.Clear();
        }
        else
        {
            break;
        }
    }
    else
    {
        break;
    }
}

personajesJson.GuardadJugador(pj1, alias, tipoLeniador, rondaActual-1);

if(PContraPartida.Count != 0)
{
    personajesJson.GuardarPersonajes(PContraPartida);
}

// PruebaConsola p = new PruebaConsola();
// p.Run();
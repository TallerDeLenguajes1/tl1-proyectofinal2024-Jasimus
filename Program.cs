using clases;
// using pruebas;
using System.Text;
Console.InputEncoding = Encoding.GetEncoding("iso-8859-1");
Console.OutputEncoding = Encoding.UTF8;
Partida partida = new Partida();
Personaje pj1 = new Personaje();
Personaje Otro = new Personaje();
Arbol arbol = new Arbol();

string? mejora;
string cantRondas;
string tipoLeniador;
int cantRondasInt;
int rondaActual = 1;
int cantPalabras = 5;
int LargoPalabras = 3;
double fuerzaGolpe;

List<string> PalabrasRonda = new List<string>();
Random idiomaRandom = new Random();

do
{
    Console.WriteLine("ingrese la cantidad de rondas (mayor a cero): ");
    cantRondas = Console.ReadLine();
}while(!int.TryParse(cantRondas, out cantRondasInt) && cantRondasInt <= 0);

do
{
    Console.WriteLine("ingrese el tipo del leñador:\n> fuerte\n> hábil\n> ágil\n");
    tipoLeniador = Console.ReadLine();
}while(string.Compare(tipoLeniador, "fuerte") != 0 && string.Compare(tipoLeniador, "hábil") != 0 && string.Compare(tipoLeniador, "ágil") != 0);

switch (tipoLeniador)
{
    case "fuerte":
        pj1.Fuerza = 2;
        pj1.Suerte = 0;
        pj1.Velocidad = 0;
        break;

    case "habil":
        pj1.Fuerza = 0;
        pj1.Suerte = 2;
        pj1.Velocidad = 0;
        break;
    
    case "agil":
        pj1.Fuerza = 0;
        pj1.Suerte = 0;
        pj1.Velocidad = 2;
        break;
}

arbol.Sigue = true;
PersonajesJson personajesJson = new PersonajesJson();
List<Personaje> PContra = personajesJson.ObtenerPersonajes(cantRondasInt);
List<Personaje> PContraPartida = new List<Personaje>();

while(rondaActual <= cantRondasInt)
{
    if(PContra.Count() != 0)
    {
        Otro = PContra[0];
    }
    else
    {
        Otro = partida.GenerarOtro();
    }

    if (arbol.Sigue)
    {
        arbol.LadoJugadorMetodo(cantPalabras, pj1.Fuerza);
        arbol.LadoContrincante(rondaActual, cantRondasInt, pj1.VelocidadMedia());
        // Lenguaje idioma = (Lenguaje)idiomaRandom.Next(0, 4);
        PalabrasRonda = await partida.PedirPalabras(/*idioma*/Lenguaje.es, cantPalabras, LargoPalabras - pj1.Velocidad, pj1.Client);
        int indice = 0;
        pj1.Turno = true;
        while(!arbol.Cayo)
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
                partida.IniciarTurno();
                if(partida.Escrito && string.Compare(partida.Palabra, PalabrasRonda[indice]) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("GOLPE!");
                    Console.ForegroundColor = ConsoleColor.White;
                    pj1.CalcularVelocidad(partida.TiempoDisponible, partida.TiempoRestante);
                    fuerzaGolpe = pj1.FuerzaGolpe(arbol.CantidadGolpesTotalpj, cantRondasInt);
                    arbol.GolpeJugador(fuerzaGolpe);
                    if(partida.Critico)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("crítico: "+fuerzaGolpe);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FALLO");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                pj1.Turno = false;
                indice++;
            }
            else
            {
                partida.IniciarTurnoOtro(Otro.Velocidad);
                double fuerzaGolpeOtro = Otro.FuerzaGolpe(arbol.CantidadGolpesTotalpj, cantRondasInt);
                arbol.GolpeContrincante(fuerzaGolpeOtro);
                if(partida.CriticoOtro)
                {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("hizo crítico: "+fuerzaGolpeOtro);
                        Console.ForegroundColor = ConsoleColor.White;
                }
                pj1.Turno = true;
            }
        }
    }
    PContra.Remove(Otro);
    PalabrasRonda.Clear();
    rondaActual++;
    Console.WriteLine("Pasas a la siguiente ronda");
    Thread.Sleep(500);
    do
    {
        Console.WriteLine("¿Qué deseas mejorar?\nFuerza\nVelocidad\nSuerte");
        mejora =Console.ReadLine();
    }while(string.Compare(mejora, "Fuerza") != 0 && string.Compare(mejora, "Velocidad") != 0 && string.Compare(mejora, "Suerte") != 0);

    switch(mejora)
    {
        case "Fuerza":
            pj1.AumentarFuerza();
            Console.WriteLine("aumentaste tu fuerza a: "+pj1.Fuerza);
            break;
        case "Velocidad":
            pj1.AumentarVelocidad();
            Console.WriteLine("aumentaste tu velocidad a: "+pj1.Velocidad);
            break;
        case "Suerte":
            pj1.AumentarSuerte();
            Console.WriteLine("aumentaste tu suete a: "+pj1.Suerte);
            break;
    }
    if(LargoPalabras <= 7)
    {
        LargoPalabras++;
    }
    if(cantPalabras <= 10)
    {
        cantPalabras++;
    }
}

// PruebaConsola p = new PruebaConsola();
// p.Suerte = 2;
// p.PalabrasVel = 1;
// Console.WriteLine(p.Sigmoide_inversa(0.43, 5, 12));
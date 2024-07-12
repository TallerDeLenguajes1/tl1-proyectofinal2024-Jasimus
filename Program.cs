using System;
using clases;
Console.OutputEncoding = System.Text.Encoding.UTF8;

Partida partida = new Partida();
Personaje pj1 = new Personaje();
Contrincante contra = new Contrincante();
Arbol arbol = new Arbol();

string cantRondas;
string tipoLeniador;
int cantRondasInt;
int rondaActual = 1;
int cantGolpesInicial = 10;
int largoInicial = 8;
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
    Console.WriteLine("ingrese el tipo del leñador:\n> fuerte\n> habil\n> agil\n");
    tipoLeniador = Console.ReadLine();
}while(string.Compare(tipoLeniador, "fuerte") != 0 && string.Compare(tipoLeniador, "habil") != 0 && string.Compare(tipoLeniador, "agil") != 0);

switch (tipoLeniador)
{
    case "fuerte":
        pj1.Fuerza = 2;
        pj1.Suerte = 1;
        pj1.Velocidad = 0;
        break;

    case "habil":
        pj1.Fuerza = 0;
        pj1.Suerte = (double)4/cantRondasInt;
        pj1.Velocidad = 0;
        break;
    
    case "agil":
        pj1.Fuerza = 0;
        pj1.Suerte = 1;
        pj1.Velocidad = 1;
        break;
}

while(rondaActual <= cantRondasInt)
{
    arbol.LadoJugadorMetodo(cantGolpesInicial, pj1.Fuerza);
    arbol.LadoContrincante(rondaActual, cantRondasInt, pj1.VelocidadMedia());
    // Lenguaje idioma = (Lenguaje)idiomaRandom.Next(0, 4);
    PalabrasRonda = await partida.PedirPalabras(/*idioma*/Lenguaje.es, arbol.LadoJugador, largoInicial - pj1.Velocidad, pj1.Client);
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
                Console.WriteLine("lado jugador antes del golpe: "+arbol.LadoJugador);
                pj1.CalcularVelocidad(partida.TiempoDisponible, partida.TiempoRestante);
                fuerzaGolpe = pj1.FuerzaGolpe(arbol.CantidadGolpesTotalpj);
                Console.WriteLine("golpe de jugador: "+fuerzaGolpe);
                arbol.GolpeJugador(fuerzaGolpe);
                Console.WriteLine("lado jugador después del golpe: "+arbol.LadoJugador);
                if(partida.Critico)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("crítico: "+fuerzaGolpe);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        if (indice < arbol.CantidadGolpesTotalpj-1)
        {
            indice++;
        }
        else
        {
            Console.WriteLine("se terminaron las oportunidades");
        }
    }
    PalabrasRonda.Clear();
    rondaActual++;
    
}

using clases;
using System;
using System.Threading;
Partida partida = new Partida();
Personaje pj1 = new Personaje();
Contrincante contra = new Contrincante();
Arbol arbol = new Arbol();

int cantRondasInt;
int rondaActual = 1;
int cantGolpesInicial = 10;
int largoInicial = 8;

List<string> PalabrasRonda = new List<string>();
Random idiomaRandom = new Random();

do
{
    Console.WriteLine("ingrese la cantidad de rondas (mayor a cero): ");
    string cantRondas = Console.ReadLine();
}while(!int.TryParse(cantRondas, out cantRondasInt) && cantRondasInt <= 0);

do
{
    Console.WriteLine("ingrese el tipo del leñador:\n> fuerte\n> hábil\n> ágil\n");
    string tipoLeniador = Console.ReadLine();
}while(string.Compare(tipoLeniador, "fuerte") != 0 && string.Compare(tipoLeniador, "hábil") != 0 && string.Compare(tipoLeniador, "ágil") != 0);

switch (tipoLeniador)
{
    case "fuerte":
        pj1.Fuerza = 2;
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
        pj1.Velocidad = 1;
        break;
}

while(rondaActual <= cantRondas)
{
    arbol.LadoJugadorMetodo(cantGolpesInicial, pj1.Fuerza);
    arbol.LadoContrincante(rondaActual, cantRondasInt, pj1.VelocidadMedia());
    (Lenguaje) idioma = (Lenguaje)idiomaRandom.Next(0, 4);
    Palabra = partida.PedirPalabras(idioma, arbol.LadoJugador, largoInicial - pj1.Velocidad, pj1.Client);
    
    PalabrasRonda.Clear();
}





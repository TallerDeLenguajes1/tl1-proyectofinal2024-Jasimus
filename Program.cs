﻿using clases;
// using pruebas;
using System.Text;
Console.InputEncoding = Encoding.GetEncoding("iso-8859-1");
Console.OutputEncoding = Encoding.UTF8;
Partida partida = new Partida();
Personaje pj1 = new Personaje();
Personaje Otro = new Personaje();
Arbol arbol = new Arbol();
Stream streamEntrada = Console.OpenStandardInput();

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
Console.Clear();
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
List<Personaje> PContra = personajesJson.ObtenerPersonajes(cantRondasInt);
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
        PalabrasRonda = await partida.PedirPalabras(idioma/*Lenguaje.es*/, partida.CalcularCantidadPalabras(cantPalabras, Otro.Fuerza), LargoPalabras - pj1.Velocidad, pj1.Client);
        // PalabrasRonda = ["migraña", "salúd", "Amén", "año", "drácula", "mañana", "sin duda", "cuál año", "dea", "el chuqui", "dabo", "el chocas", "el rubius", "y así"];
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
                partida.IniciarTurno();
                if(partida.Escrito)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Achazo");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if(partida.Escrito && string.Compare(partida.Palabra, PalabrasRonda[indice]) == 0)
                {
                    fuerzaGolpe = pj1.FuerzaGolpe(arbol.CantidadGolpesTotalpj, cantRondasInt);
                    pj1.CalcularVelocidad(partida.TiempoDisponible, partida.TiempoRestante);
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
if(PContraPartida.Count != 0)
{
    personajesJson.GuardarPersonajes(PContraPartida);
}

// PruebaConsola p = new PruebaConsola();
// p.Run();
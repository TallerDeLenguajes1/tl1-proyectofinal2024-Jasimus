// using clases;
using System;
using System.Threading;
using pruebas;
// Partida partida = new Partida();
// int cantRondasInt;

// do
// {
//     Console.WriteLine("ingrese la cantidad de rondas (mayor a cero): ");
//     string cantRondas = Console.ReadLine();
// }while(!int.TryParse(cantRondas, out cantRondasInt) || cantRondasInt <= 0);


// class Program
// {
//     static string Palabra = string.Empty;
//     static Thread MainThread = Thread.CurrentThread;
//     static System.Threading.Timer timer = null;
//     static void Main(string[] args)
//     {
//         try
//         {
//             TimerCallback timerCallback = new TimerCallback(PrintMessage);
//             timer = new Timer(timerCallback, null, 0, 4000);
//             Thread.Sleep(4000);
//             timer.Dispose();
//             Console.WriteLine("no se ingresó ninguna palabra");
//         }
//         catch(ThreadInterruptedException)
//         {
//             Console.WriteLine("se ingresó: "+Palabra);
//             timer.Dispose();
//         }
//      }

//     private static void PrintMessage(object state)
//     {
//         Palabra = Console.ReadLine();
//         MainThread.Interrupt();
//     }
// }

PruebaConsola p1 = new PruebaConsola();

p1.Run();
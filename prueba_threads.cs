using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using Microsoft.VisualBasic;
// using System.Diagnostics;
// using System.Threading;
namespace pruebas;
// public class PruebaConsola
// {
//     static string Palabra = string.Empty;
//     static Thread MainThread = Thread.CurrentThread;
//     static System.Threading.Timer timer = null;
//     static Stopwatch reloj = new Stopwatch();
//     static int tDisp = 3000;
//     public void Run()
//     {
//         try
//         {
//             TimerCallback timerCallback = new TimerCallback(PrintMessage);
//             timer = new Timer(timerCallback, null, 0, Timeout.Infinite);
//             Console.WriteLine("ingrese una palabra: ");
//             reloj.Start();
//             Thread.Sleep(tDisp);
//             reloj.Stop();
//             Console.WriteLine("no se ingresó ninguna palabra");
//         }
//         catch(ThreadInterruptedException)
//         {
//             reloj.Stop();
//             Console.WriteLine("tiempo transcurrido: "+ reloj.ElapsedMilliseconds +" milisegundos\n");
//             if(reloj.ElapsedMilliseconds <= tDisp/3)
//             {
//                 Console.WriteLine("se sacó crítico");
//             }
//             timer.Dispose();
//         }
//      }

//     private static void PrintMessage(object state)
//     {
//         Palabra = Console.ReadLine();
//         MainThread.Interrupt();
//     }
// }

// public class PruebaFuerza
// {
//     private Random randi = new Random();
//     public double FuerzaGolpe(int cantGolpes)
//     {
//         double media = (cantGolpes/4 - 1);
//         Console.WriteLine("media= "+media);
//         double S = 100*media;
//         Console.WriteLine("S= "+S);
//         double b = Math.Exp(media*Math.Log(S));
//         Console.WriteLine("b= "+b);
//         double MaxRand = 1/(Math.Exp(-(cantGolpes/2 - 1)*Math.Log(S)+Math.Log(b))+1);
//         Console.WriteLine("MaxRand= "+MaxRand);
//         double numero = randi.NextDouble() * MaxRand;
//         Console.WriteLine("numero= "+numero);
        
//         return 1 + Sigmoide_inversa(numero, S, b);
//     }

//     public double Sigmoide_inversa(double x, double S, double b)       //mapea los valores de una V.A con dist. uniforme a una con distribución aprox. normal
//     {
//         return ((Math.Log(b)-Math.Log(1/x - 1))/Math.Log(S));
//     }
// }

class PruebaConsola
{   
    ConsoleKeyInfo chi;
    bool activo = false;
    public void Run()
    {   
        while(true)
        {
            do
            {
                Console.WriteLine("presione x para bloquear la entrada");
                chi = Console.ReadKey(true);
                Console.WriteLine("presionaste {0}", chi.Key);
                activo = false;
            }while(chi.Key != ConsoleKey.X);
            
            do
            {
                Console.WriteLine("presione z para desbloquear la entrada");
                chi = Console.ReadKey(true);
            }while(chi.Key != ConsoleKey.Z);
        }
    }
}
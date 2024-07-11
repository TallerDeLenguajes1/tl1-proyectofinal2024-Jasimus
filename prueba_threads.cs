using System.Diagnostics;
using System.Threading;
namespace pruebas;

public class PruebaConsola
{
    static string Palabra = string.Empty;
    static Thread MainThread = Thread.CurrentThread;
    static System.Threading.Timer timer = null;
    static Stopwatch reloj = new Stopwatch();
    static int tDisp = 3000;
    public void Run()
    {
        try
        {
            TimerCallback timerCallback = new TimerCallback(PrintMessage);
            timer = new Timer(timerCallback, null, 0, Timeout.Infinite);
            Console.WriteLine("ingrese una palabra: ");
            reloj.Start();
            Thread.Sleep(tDisp);
            reloj.Stop();
            Console.WriteLine("no se ingresó ninguna palabra");
        }
        catch(ThreadInterruptedException)
        {
            reloj.Stop();
            Console.WriteLine("tiempo transcurrido: "+ reloj.ElapsedMilliseconds +" milisegundos\n");
            if(reloj.ElapsedMilliseconds <= tDisp/3)
            {
                Console.WriteLine("se sacó crítico");
            }
            timer.Dispose();
        }
     }

    private static void PrintMessage(object state)
    {
        Palabra = Console.ReadLine();
        MainThread.Interrupt();
    }
}
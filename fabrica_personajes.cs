using clases;

namespace fabrica
{
    public class fabricar
    {
       List<Personaje> lp = new List<Personaje>();

       public List<Personaje> Run()
       {
            Personaje p1 = new Personaje();
            p1.Fuerza = 1;
            p1.Velocidad = 1;
            p1.Suerte = 1;
            p1.Turno = false;
            lp.Add(p1);

            Personaje p2 = new Personaje();
            p2.Fuerza = 0;
            p2.Velocidad = 2;
            p2.Suerte = 0;
            p2.Turno = false;
            lp.Add(p2);

            Personaje p3 = new Personaje();
            p3.Fuerza = 2;
            p3.Velocidad = 0;
            p3.Suerte = 0;
            p3.Turno = false;
            lp.Add(p3);

            Personaje p4 = new Personaje();
            p4.Fuerza = 0;
            p4.Velocidad = 0;
            p4.Suerte = 2;
            p4.Turno = false;
            lp.Add(p4);


            Personaje p5 = new Personaje();
            p5.Fuerza = 1;
            p5.Velocidad = 0;
            p5.Suerte = 1;
            p5.Turno = false;
            lp.Add(p5);

            return lp;
       }
    }
}
## Idea principal del juego

El juego se va a centrar en la velocidad de escritura del jugador. 
El jugador tendrá que escribir una palabra, que va a aparecer en pantalla, lo más rápido que pueda.


## Cómo abordo las rúbricas

El personaje es un leñador, cuyo objetivo es llegar a ser el mejor de todos los leñadores. Para ello tiene que competir contra los _"mejores"_ leñadores del mundo.
La competencia se basa en talar un árbol antes que el oponente, dando achazos a un mismo árbol, que caerá del lado del ganador.


## Diseño del personaje

El leñador dará un achazo en el momento que el jugador termine una palabra.
A medida que se avanza con las competencias, el leñador puede mejorar una de estas características:

**>** _Fuerza_: el leñador necesitará menos achazos para deribar el árbol (esto puede verse como un parámetro inversamente proporcional a la cantidad de palabras que necesita escribir el jugador antes de que caiga el árbol de su lado). El límite de fuerza es aquella que no supera a la que deja aparecer 2 palabras.

**>** _Velocidad_: el leñador talará más rápido (esto puede verse como que al jugador le aparecen palabras más cortas). El límite de velocidad es aquella que no supera a la que deja aparecer 2 letras.

**>** _Suerte_: los contrincantes pueden ser de otros países, con otro idioma. Las palabras que tendrá que escribir el jugador serán en el idioma del contrincante. La suerte ayuda a que tu contrincante sea de algún país que hable español. La suerte puede aumentar hasta un 100%, es decir, el 100% de las veces tu contrincante hablará español.


## Diseño de partidas

El jugador podrá elegir la cantidad de partidas totales, en un intervalo de 3 a 10. 
Como las palabras van a tener un máximo de 12 letras, no tiene sentido ir más allá de las 10 partidas, además que ya va a ser largo para el jugador más que eso.

## API
https://random-word-api.herokuapp.com/


## Notas

Se modifica un poco la forma de las partidas si se cumple que es una batalla por turnos. Lo que haría es:

1- La fuerza con la que el leñador le pegue al árbol depende también de la velocidad con la que escriba, y que tenga un límite de tiempo para escribirlas. El árbol se derriba si se alcanza una fuerza total, que se calcularía: **cantGolpes = cantPalabrasO - Fuerza**, donde **cantPalabrasO** es la cantidad de palabras máxima que se pueden escribir.
La fuerza con la que pega el leñador es:
**fuerzaGolpe = 1 + maxGolpe*tiempoRestante/tiempoDisponible**
donde:
*maxGolpe*= es la fuerza máxima con la que puede pegar el leñador; la probabilidad de un valor alto es proporcional a la suete del mismo. Es algo así como un golpe crítico. Sería interesante que este valor se obtenga al momento de que termine el turno del jugador.

*tiempoRestante*= como el leñador tiene un límite de tiempo para escribir la palabra, este tiempo es: **tiempoDisponible - tiempoDeEscritura**, y a su vez *tiempoDeEscritura = **momentoDeFinalización - momentoDeIniciación***.

momentoDeFinalización es float --> tiempoDeEscritura es float --> tiempoRestante es float --> fuerzaGolpe es float; cantGolpes designa la cantidad de golpes que faltan para derribar el árbol. Si **cantGolpesSig = cantGolpesAnt - fuerzaGolpe**, lo conveniente es comprobar si *cantGolpesSig* es <= 0.

## Partida
> El jugador tiene un tiempo para escribir la palabra (*tiempoDispobible*). Si pasa ese tiempo, pierde el turno.
> El jugador tiene un tiempo de *tiempoDispoble/4* para escribir la palabra y sacar crítico. Si pasa ese tiempo, se continúa con la cuenta del *tiempoDisponible*.
> El golpe crítico se obtiene si el *tiempoDeEscritura = 0*, donde *tiempoDeEscritura = momentoDeFinalización - momentoDeIniciación*.
> Para que sea posible que *momentoDeFinalización = momentoDeIniciación*, el tiempo de escritura se empieza a contar *tiempoDisponible/4* después del inicio del turno.

El valor máximo de la velocidad es la velocidad para la cuál la cantidad de letras de las palabras es igual a 2.

El valor máximo de la fuerza es *cantidad de golpes Iniciales/2*.

EL valor máximo de la suerte es *1/(Math.Exp((cantGolpes/2 - 1)*Math.Log(S)+Math.Log(b))+1)*.


# Problemas que tuve

## Cómo esperar la entrada por teclado sin cortar la ejecución del programa
Lo solucioné con el uso de Timers: inicio la cuenta del timer al momento en que inicia el turno del jugador, y ejecuta un método llamado *"esperarPalabra"*. Dentro de un *Try* duermo el *Thread principal* un tiempo igual al *"TiempoDisponible"*; si se termina de ejecutar el método *"esperarPalabra"* antes de que pase el *"TiempoDisponible"*, se interrumpe el Thread principal y se ejecutan las instrucciones del *Catch*; si pasa el *"TiempoDisponible"* antes de que se termine de ejecutar el método *"esperarPalabra"* se corta la ejecución del *"esperarPalabra"* y se sigue con el programa.

## El Console.ReadLine() no detecta correctamente palabras con tildes ni "ñ"
Usé el Encoding *"iso-8859-1"* para la entrada.

## Si el jugador FALLA algún golpe, puede ser que se produzca un error de *"fuera de rango"* en la lista de palabras de la API
Hice que la cantidad de palabras pedidas a la API dependa de la *Fuerza del Contrincante*, pues el golpe que da no va a bajar de dicho valor y siempre va a pegar, por lo que la cantidad de golpes posibles para el jugador no va a ser mayor a la cantidad de golpes del otro.

## Problemas con el retorno del método *FuerzaGolpe*
El retorno de este método depende del retorno del método *Sigmoide_inversa*, que recibe como parámetros a *"x", "S" y "b"*. El valor que retornaba esta función era imaginario para algunos valores de *"x"*: 
**función Sigmoide_inversa imperfecta: (Math.Log(b)-Math.Log(1/x - 1))/Math.Log(S)**
**función Sigmoide_inversa corregida: (Math.Log(b)-Math.Log(max/x - 1))/Math.Log(S)**, donde "*max*" es un nuevo parámetro, y *x <= max*.

## La función derivada de la Sigmoide de la que proviene *Sigmoide_inversa* no tiene un máximo controlado
Mediante el uso del programa *funcion_sigmoide.py* pude ver el comportamiento real de la función que uso. Tomé tres puntos como condición inicial, y los interpolé con el método de *"Interpolación de Newton"* para obtener una expresión para S: impuse que S tenga la forma de una función exponencial elevada a una función de N, donde N = media/max, tomé tres valores de N y valores de S que les correspondería, y los interpolé para obtener la función polinómica a la que está elevado *e*.

## El usuario puede escribir fuera de su turno, y todo lo escrito aparece en consola en el memento de su turno
Leo todo lo del buffer y no hago nada con lo leído antes del Console.Read() que abre paso al jugador para ingresar la palabra.

## Si el jugador ingresa algo por teclado y aprieta Enter en el tiempo después del FALLO por tiempo, la ejecución se para
Lo que sucedía era que opté por usar un timer que invocaba la ejecución de un método *"esperarPalabra"* que habilitaba la entrada al usuario por teclado, el cúal se ejecutaba al momento de empezar el turno del jugador con el método *"IniciarTurno"*. Dentro del método *"IniciarTurno"* duermo el hilo principal un tiempo *"tiempoDisponible"*, y al momento de terminar la ejecución de *"esperarPalabra"* interrumpo el hilo principal para que salga del estado de Sleep. Pero si pasaba el *"tiempoDisponible"* antes de que terminase la ejecución de *"esperarPalabra"*, el jugador podía seguir escribiendo en consola pues el *Console.ReadLine()* de dicho método no se solucionó. Esto causaba que el usuario pudiera seguir escribiendo, y si apretaba ENTER, la ejecución se para, porque se ejecuta la interrupción del hilo principal.
Lo que hice para solucionar este problema es trabajar con *Console.ReadKey()* en vez de *Console.ReadLine()*, y lo ejecuto dentro de un bucle *While* que itera mientras el tiempo *"tiempoDisponible"* no haya pasado: si pasa el tiempo, la condición del *While* pasa a *false*, y si se aprieta ENTER antes de la terminación del tiempo disponible, se interrumpe la linea principal.


## La cadena recibida es incorrecta si se borró algún caractér
Este problema se generó al momento de solucionar el anterior. El caractér *Backspace* se guardaba en la cadena de salida.
Hice que se detectara cuando el jugador preciona *Backspace*, y que se borré el último caractér de la cadena de salida.


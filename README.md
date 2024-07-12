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
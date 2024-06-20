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


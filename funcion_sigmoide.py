import matplotlib.pyplot as plt
import numpy as np
import math


def sigmoide_inversa(x, S, b, max):
    return (math.log(b) - math.log(max/x - 1))/math.log(S)

def normal_aprox(x, S, b):
    return (b*S**(-x)*math.log(S))/(1+b*S**(-x))**2

max = 10
media = 10
n = media/max
poli = 4.4*n - 0.8*n**2
s = math.exp(poli)
b = math.exp(media*math.log(s))

X = np.arange(0.01, max, 0.01)
Y = []

for i in X:
    y = normal_aprox(i, s, b)
    Y.append(y)


plt.xlim(0, max)
plt.ylim(0, 1)
# plt.xlabel("número generado aleatoriamente")
# plt.ylabel("número transformado mediante la función sigmoide inversa")
plt.axhline(y=0, color="black")
plt.axvline(x=0, color="black")
plt.plot(X, Y, color="red")
# plt.plot(X[::2], np.zeros(shape=len(X)//2+1, dtype=float), "o", color="blue")
# plt.plot(np.zeros(len(Y)//2+1, dtype=float), Y[::2], "o", color="blue")
plt.show()
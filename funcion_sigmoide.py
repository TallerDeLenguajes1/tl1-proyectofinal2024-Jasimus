import matplotlib.pyplot as plt
import numpy as np
import math


def sigmoide_inversa(x, S, b, max):
    return (math.log(b) - math.log(max/x - 1))/math.log(S)

def normal_aprox(x, S, b):
    return (b*S**(-x)*math.log(S))/(1+b*S**(-x))**2

max = 18
media = 18
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
plt.plot(X, Y, color="red")
plt.show()
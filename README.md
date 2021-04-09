# Fake Gimp
## Implementováno
* Odstíny
  * Červené (Sériově)
  * Zelené (Sériově)
  * Modré (Sériově)
  * Šedé (Sériově)
* Rozostření
  * Sériově
  * Paralelně

## Vysvětlení vlastního filtru
* Rozostření funguje na principu, že se vezmou barevné hodnoty pixelu nad, pod, vpravo a vlevo pixelu, který chceme změnit
* Tyto hodnoty se zprůměrují a vzniklé barevné složky se aplikují náš pixel
### Sériově
* Pixel po pixelu
### Paralelně
* Práce je rozdělena podle počtu vláken a každá část si vezme vlastní část obrazu, na kterém provádí změny

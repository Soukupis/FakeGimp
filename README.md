# 2020p4prg-graficky-filtr-Soukupis
* V aplikaci je implentovaný odstín červené, zelené, modré a šedé (vše sériově)
* Jako vlastní filtr jsem zvolil Blur, což znamená, že se obrázek zneostří
* Funguje tak, že se vezme hodna pixelu nad, pod, vpravo a vlevo a zprůměrní červenou, zelenou a modrou složku a zapíše jí na danný pixel
* Jak moc zneostřený obraz chceme to je určeno na základě velikosti vzdálenosti od našeho pixelu (chceme menší, tak zvolíme pixely přímo nad našim pixelem a pokud chceme veliký, tak třeba pixely vzdáleny 10)
* Filtr je implementován seriově
* Paralelně je implementován algoritmus, kde se rozdělí práce na počet vláken, ale zatím není vyřešeno to, že když se vše zapíše do pole, tak jak ve funkci returnovat daný obraz
* Rozdělím práci a na konci funkce returnuju funkci, kde je jako parametr obrázek a array, ale do array se hodnoty zapisují na jiných jádrech, takže funkce returnuje funkci s původním array bez toho aby počkala než se v array provedou změny
* Zkoušel jsem veškeré metody pomocí join, kontrola stavu thredů, ale nic mi zatím nefungovalo

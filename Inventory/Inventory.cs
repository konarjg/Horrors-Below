using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    //Pod tym dodajesz listê itemów, obiekt List<T>, gdzie T to bêdzie moja klasa ItemStack


    //W tej metodzie wpisujesz dodawanie itemu do listy
    //(jeœli item w tej liœcie ju¿ istnieje, zwiêkszasz jego iloœæ o item.Count)
    //Do sprawdzenia czy coœ w liœcie istnieje, u¿ywamy metody Find,
    //i przyrównujemy jej wynik do wartoœci null(puste)
    //do dodawania elementu w liœcie u¿ywamy metody add(obiekt)
    //aby zwiêkszyæ iloœæ istniej¹cego itemka, u¿ywamy var existingItem = Find -> o tym wy¿ej
    //i nastêpnie u¿ywamy ++existingItem.Count;
    public void AddItem(ItemStack item)
    {

    }

    //W tej metodzie wpisujesz usuwanie itemu z listy
    //(jeœli item ju¿ istnieje, zmniejszasz jego iloœæ o item.Count, jeœli ta iloœæ osi¹gnie 0
    //usuwasz item z listy poprzez lista.Remove()
    public void RemoveItem(ItemStack item)
    {

    }
}

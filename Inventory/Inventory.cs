using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    //Pod tym dodajesz list� item�w, obiekt List<T>, gdzie T to b�dzie moja klasa ItemStack


    //W tej metodzie wpisujesz dodawanie itemu do listy
    //(je�li item w tej li�cie ju� istnieje, zwi�kszasz jego ilo�� o item.Count)
    //Do sprawdzenia czy co� w li�cie istnieje, u�ywamy metody Find,
    //i przyr�wnujemy jej wynik do warto�ci null(puste)
    //do dodawania elementu w li�cie u�ywamy metody add(obiekt)
    //aby zwi�kszy� ilo�� istniej�cego itemka, u�ywamy var existingItem = Find -> o tym wy�ej
    //i nast�pnie u�ywamy ++existingItem.Count;
    public void AddItem(ItemStack item)
    {

    }

    //W tej metodzie wpisujesz usuwanie itemu z listy
    //(je�li item ju� istnieje, zmniejszasz jego ilo�� o item.Count, je�li ta ilo�� osi�gnie 0
    //usuwasz item z listy poprzez lista.Remove()
    public void RemoveItem(ItemStack item)
    {

    }
}

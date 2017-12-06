using System.Collections.Generic;

public abstract class DataObject{

    /// <summary>
    /// permet de savoir si cette objet a ete rempli ou simplement cree
    /// (true = rempli)
    /// </summary>
    public bool Loaded { get; set; }

    public int DataID { get; set; }

    public DataObject() {
        Loaded = false;
    }

    // return la liste des evenets levés
    public abstract List<EventHolder> Update(DataObject obj);
}

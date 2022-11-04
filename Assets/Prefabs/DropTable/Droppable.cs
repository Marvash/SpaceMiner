using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Droppable
{
    public string ID { get; }
    public uint weight { get; set; }

    public Droppable(string ID, uint weight)
    {
        this.ID = ID;
        this.weight = weight;
    }
    abstract public PickupStack drop();
}

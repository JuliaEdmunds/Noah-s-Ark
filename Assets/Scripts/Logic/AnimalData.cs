using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public struct AnimalData
{
    public EAnimal AnimalType;
    public EGender Gender;

    public AnimalData(EGender gender, EAnimal species)
    {
        Gender = gender;
        AnimalType = species;
    }
}

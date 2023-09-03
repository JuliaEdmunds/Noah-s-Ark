using System;


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

    // Overrides in a struct to ensure equality operates properly
    public override bool Equals(object obj)
    {
        if (!(obj is AnimalData mys)) // type pattern here
        { 
            return false;
        }

        return this.AnimalType == mys.AnimalType && this.Gender == mys.Gender; // mys is already known here without explicit casting
    }

    public override int GetHashCode()
    {
        return AnimalType.GetHashCode() ^ Gender.GetHashCode();
    }
    public static bool operator ==(AnimalData x, AnimalData y)
    {
        return x.AnimalType == y.AnimalType && x.Gender == y.Gender;
    }
    public static bool operator !=(AnimalData x, AnimalData y)
    {
        return !(x == y);
    }
}

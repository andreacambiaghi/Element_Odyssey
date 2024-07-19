using System;

[Serializable]
public class ElementPair : IEquatable<ElementPair>
{
    public string Element1 { get; set; }
    public string Element2 { get; set; }

    public ElementPair(string element1, string element2)
    {
        Element1 = element1;
        Element2 = element2;
    }

    public bool Equals(ElementPair other)
    {
        return other != null && (Element1 == other.Element1 && Element2 == other.Element2);
                // || (Element1 == other.Element2 && Element2 == other.Element1));
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash1 = Element1.GetHashCode();
            int hash2 = Element2.GetHashCode();
            return hash1 ^ hash2;
        }
    }

    public override string ToString()
    {
        return $"({Element1}, {Element2})";
    }
}

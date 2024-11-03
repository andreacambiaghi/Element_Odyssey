using System.Collections.Generic;

public class VillageObject{
        public string Key; 
        public int Value;
        public List<string> Requirements;

        public string toString()
        {
            return Key + " " + Value + " " + (Requirements != null ? string.Join(", ", Requirements) : "No requirements");
        }
}

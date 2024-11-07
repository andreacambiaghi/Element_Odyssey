using System.Collections.Generic;

public class VillageData
    {
        public List<VillageObject> villageObjects;

        public string toString()
        {
            if (villageObjects == null)
            {
                return "No village objects available.";
            }
            return string.Join("\n", villageObjects.ConvertAll(v => v.toString()));
        }
}
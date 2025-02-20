using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepClear : MonoBehaviour
{
    public void Click() {
        VillagePlaneManager.Instance.resetConfiguration();
    }
}

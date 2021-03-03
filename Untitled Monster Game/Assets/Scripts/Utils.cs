using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

static class Utils
{
    static public List<GameObject> SortByDistance(this List<GameObject> objects, Vector2 ref_pos)
    {
        return objects.OrderBy(x => Vector2.Distance(x.transform.position, ref_pos)).ToList();
    }
};
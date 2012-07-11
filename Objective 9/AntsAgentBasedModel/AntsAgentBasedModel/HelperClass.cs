using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntsAgentBasedModel
{
    public static class HelperClass
    {
        internal static List<Phenomenon> help(List<Phenomenon> list, World world)
        {
            foreach (Phenomenon worldPhenomenon in world.getPhenomenonList())
            {
                for (int index = 0; index < list.Count; index++)
                {
                    if (list[index].getLocation() == worldPhenomenon.getLocation())
                    {
                        worldPhenomenon.refresh();
                        list.RemoveAt(index);
                    }
                }
            }
            return list;
        }

    }
}

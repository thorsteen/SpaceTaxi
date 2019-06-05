using System;
using DIKUArcade.Math;

namespace SpaceTaxiApplication.Customer {
    public class FindSymbolCoords {
        
        /// <summary>
        /// finds the coordinates of a given char, in an array of strings.
        /// </summary>
        /// <param name="map">An array of strings.</param>
        /// <param name="target">The target char.</param>
        /// <returns>Vec2I, containing coordinates of the given char.</returns>
        public static Vec2I Find(string[] map, char target) {
            for (int i = 0; i < 23; i++) {
                for (int j = 0; j < 40 ; j++) {
                    if (map[i][j] == target) {
                        return new Vec2I(i,j);
                    }
                }
            }
            throw new Exception("Symbol not in level");
        }
    }
}
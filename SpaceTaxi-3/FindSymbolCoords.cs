using System;
using DIKUArcade.Math;

namespace SpaceTaxi_3 {
    public class FindSymbolCoords {

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
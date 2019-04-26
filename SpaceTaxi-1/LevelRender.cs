using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_1 {
    public class LevelRender {
        /// <summary>
        /// Goes through the array portraying the map that is given. Via two for-loops
        /// the array goes through each element, and assigns the given path for the picture to the
        /// char. 
        /// </summary>
        /// <param name="level"></param>
        /// <returns>List_Entity </returns>
        public List<Entity> LevelToEntityList(Level level) {
            List<Entity> listEntity = new List<Entity>();
            for (int i = 0; i < 23; i++) {
                for (int j = 0; j < 40; j++) {
                    char symbol = level.map[i][j];
                    if (symbol != ' ') {
                        string fileName = level.keyLegend[symbol];
                        Entity entity = new Entity(new StationaryShape(new Vec2F(1f / 40f * j, 1f / 
                                                                                               23f * i), new 
                            Vec2F(
                                1f / 40f,
                                1f / 23f)), new Image(Path.Combine("Assets", "Images", fileName)));
                        listEntity.Add(entity);
                    }
                    
                }
            }

            return listEntity;
        }
    }
}
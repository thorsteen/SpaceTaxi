using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_2 {
    public class LevelRender {
        /// <summary>
        /// Goes through the array portraying the map that is given. Via two for-loops
        /// the array goes through each element, and assigns the given path for the picture to the
        /// char. 
        /// </summary>
        /// <param name="level"></param>
        /// <returns>List_Entity </returns>
        public EntityContainer LevelToEntityList(Level level) {
            EntityContainer listEntity = new EntityContainer();
            for (int i = 0; i < 23; i++) {
                for (int j = 0; j < 40; j++) {
                    
                    char symbol = level.map[i][j];
                    if (symbol != ' ' && symbol != '^' && symbol != '>') {
                        string fileName = level.keyLegend[symbol];
                        Entity entity = new Entity(new StationaryShape(new Vec2F(1f / 40f * j, 22f/23f - (1f / 
                                                                                               23f * i)), new 
                            Vec2F(
                                1f / 40f,
                                1f / 23f)), new Image(Path.Combine("Assets", "Images", fileName)));
                        listEntity.AddStationaryEntity(new StationaryShape(new Vec2F(1f / 40f * j, 22f/23f - (1f / 
                                                                                              23f * i)), new 
                            Vec2F(
                                1f / 40f,
                                1f / 23f)),new Image(Path.Combine("Assets", "Images", fileName)));
                    }
                    
                }
            }

            return listEntity;
        }
    }
}
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace SpaceTaxi_3 {
    public class LevelRender {
        /// <summary>
        /// Takes a Level object and translates its map into entities.
        /// </summary>
        /// <param name="level">A Level object</param>
        /// <returns>An EntityContainer, containing entities for each wall in the level.</returns>
        public EntityContainer LevelToEntityList(Level level) {
            EntityContainer listEntity = new EntityContainer();
            for (int i = 0; i < 23; i++) {
                for (int j = 0; j < 40; j++) {
                    char symbol = level.map[i][j];
                    if (symbol != ' ' && symbol != '^' && symbol != '>') {
                        string fileName = level.keyLegend[symbol];
                        listEntity.AddStationaryEntity(new StationaryShape(
                            new Vec2F(1f / 40f * j, 22f/23f - (1f / 23f * i)), 
                            new Vec2F(1f / 40f,1f / 23f)),new Image(Path.Combine("Assets", "Images", fileName)));
                    }
                }
            }

            return listEntity;
        }
    }
}
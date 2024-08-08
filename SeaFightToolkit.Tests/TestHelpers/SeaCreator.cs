using SeaFightToolkit.ClassicSeaFight.Constants;
using SeaFightToolkit.Common.Models;

namespace SeaFightToolkit.Tests.TestHelpers
{
    /// <summary>
    /// Вспомогательный класс для создания игровых полей
    /// </summary>
    public class SeaCreator
    {
        /// <summary>
        /// Создает игровое поле, где одна клетка отмечена как Непосещенная
        /// </summary>
        /// <returns>Игровое поле</returns>
        public static SeaCell[,] SeaWithOneNotVisitedCell()
        {
            var field = new SeaCell[FieldConstants.FieldWidth, FieldConstants.FieldHeight];
            for (var i = 0; i < FieldConstants.FieldWidth; i++)
            {
                for (var j = 0; j < FieldConstants.FieldHeight; j++)
                {
                    field[i, j] = new SeaCell(i, j);
                    field[i, j].IsVisited = true;
                }
            }

            field[4, 4].IsVisited = false;

            return field;
        }

        /// <summary>
        /// Создает игровое поле, где одна клетка отмечена как Посещенная
        /// </summary>
        /// <returns>Игровое поле</returns>
        public static SeaCell[,] SeaWithOneVisitedCell()
        {
            var field = new SeaCell[FieldConstants.FieldWidth, FieldConstants.FieldHeight];
            for (var i = 0; i < FieldConstants.FieldWidth; i++)
            {
                for (var j = 0; j < FieldConstants.FieldHeight; j++)
                {
                    field[i, j] = new SeaCell(i, j);
                }
            }

            field[4, 4].IsVisited = true;

            return field;
        }
    }
}

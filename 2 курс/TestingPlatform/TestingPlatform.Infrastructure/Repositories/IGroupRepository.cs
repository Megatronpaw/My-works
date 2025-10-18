using TestingPlatform.Models;

namespace TestingPlatform.Application.Interfaces;

public interface IGroupRepository
{
    /// <summary>
    /// Получить все группы.
    /// </summary>
    /// <returns>Массив групп.</returns>
    List<Group> GetAll();

    /// <summary>
    /// Получить группу по id.
    /// </summary>
    /// <param name="id">Идентификатор группы</param>
    /// <returns></returns>
    Group GetById(int id);

    /// <summary>
    /// Создать новую группу.
    /// </summary>
    /// <param name="group">Модель создания новой группы.</param>
    /// <returns>Идентификатор новой группы .</returns>
    int Create(Group group);

    /// <summary>
    /// Обновить информацию о группе.
    /// </summary>
    /// <param name="group">Модель обновления группы.</param>
    void Update(Group group);

    /// <summary>
    /// Удалить группу.
    /// </summary>
    /// <param name="id">Идентификатор группы.</param>
    void Delete(int id);
}


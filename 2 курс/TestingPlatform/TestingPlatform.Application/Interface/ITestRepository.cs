using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Application.Interfaces;

public interface ITestRepository
{
    /// <summary>
    /// Получить все тесты.
    /// </summary>
    /// <param name="isPublic">Опубликован ли тест</param>
    /// <param name="groupIds">Идентификаторы групп</param>
    /// <param name="studentIds">Идентификаторы студентов</param>
    /// <returns>Массив тестов.</returns>
    Task<IEnumerable<TestDto>> GetAllAsync(bool? isPublic, List<int> groupIds, List<int> studentIds);

    /// <summary>
    /// Получить все тесты для студента.
    /// </summary>
    /// <param name="studentId">Идентификатор студента</param>
    /// <returns></returns>
    Task<IEnumerable<TestDto>> GetAllForStudent(int studentId);

    /// <summary>
    /// Получить тест по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор теста.</param>
    /// <returns>Тест.</returns>
    Task<TestDto> GetByIdAsync(int id);

    /// <summary>
    /// Создать новый тест.
    /// </summary>
    /// <param name="testDto">Модель создания нового теста.</param>
    /// <returns>Идентификатор нового теста.</returns>
    Task<int> CreateAsync(TestDto testDto);

    /// <summary>
    /// Обновить информацию о тесте.
    /// </summary>
    /// <param name="testDto">Модель обновления теста.</param>
    Task UpdateAsync(TestDto testDto);

    /// <summary>
    /// Удалить тест.
    /// </summary>
    /// <param name="id">Идентификатор теста.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Получить последние N опубликованных тестов
    /// </summary>
    /// <param name="count">Количество тестов</param>
    /// <returns>Список тестов</returns>
    Task<IEnumerable<TestDto>> GetTopRecentAsync(int count = 5);

    /// <summary>
    /// Получить количество тестов по типам
    /// </summary>
    /// <returns>Статистика по типам тестов</returns>
    Task<IEnumerable<object>> GetTestCountByTypeAsync();

    /// <summary>
    /// Получить статистику по тестам для курсов
    /// </summary>
    /// <returns>Статистика по курсам</returns>
    Task<IEnumerable<object>> GetCourseStatsAsync();

    /// <summary>
    /// Получить средние значения по направлениям
    /// </summary>
    /// <returns>Статистика по направлениям</returns>
    Task<IEnumerable<object>> GetDirectionAveragesAsync();

    /// <summary>
    /// Получить временную шкалу тестов по публикации
    /// </summary>
    /// <returns>Статистика по времени публикации</returns>
    Task<IEnumerable<object>> GetTestTimelineByPublicAsync();

    /// <summary>
    /// Получить топ групп по количеству тестов
    /// </summary>
    /// <param name="top">Количество групп в топе</param>
    /// <returns>Статистика по группам</returns>
    Task<IEnumerable<object>> GetTopGroupsByTestCountAsync(int top = 10);
}
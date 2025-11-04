public class UpdateGroupRequest
{
    public int Id { get; set; }

    /// <summary>
    /// Название группы
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Идентификатор направления
    /// </summary>
    public int DirectionId { get; set; }

    /// <summary>
    /// Идентфикатор курса
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public int ProjectId { get; set; }
}
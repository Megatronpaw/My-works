using TestingPlatform.Responses.Course;
using TestingPlatform.Responses.Direction;
using TestingPlatform.Responses.Project;
using TestingPlatform.Responses;

namespace TestingPlatform.Responses.Group;

public class GroupResponse : BaseResponse
{
    /// <summary>
    /// Модель направления
    /// </summary>
    public DirectionResponse Direction { get; set; }

    /// <summary>
    /// Модель курса
    /// </summary>
    public CourseResponse Course { get; set; }

    /// <summary>
    /// Модель проекта
    /// </summary>
    public ProjectResponse Project { get; set; }
}
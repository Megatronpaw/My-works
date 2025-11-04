using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Requests.Student;
using TestingPlatform.Responses.Student;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Enums;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(IStudentRepository studentRepository, IUserRepository userRepository, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Получить список студентов
    /// </summary>
    /// <param name="groupIds">Идентификаторы групп</param>
    /// <remarks>Если идентификаторы групп не указаны - придет полный список студентов</remarks>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await studentRepository.GetAllAsync();

        return Ok(mapper.Map<IEnumerable<StudentResponse>>(students));
    }

    /// <summary>
    /// Получить данные студента по Id
    /// </summary>
    /// <param name="id">Идентификатор студента</param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await studentRepository.GetByIdAsync(id);

        return Ok(mapper.Map<StudentResponse>(student));
    }

    /// <summary>
    /// Добавить студента
    /// </summary>
    /// <param name="student">Модель создания студента</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest student)
    {
        var userDto = new UserDto()
        {
            Login = student.Login,
            Password = student.Password,
            Email = student.Email,
            FirstName = student.FirstName,
            MiddleName = student.MiddleName,
            LastName = student.LastName,
            Role = UserRole.Student
        };
        var userId = await userRepository.CreateAsync(userDto);

        var studentDto = new StudentDto()
        {
            UserId = userId,
            Phone = student.Phone,
            VkProfileLink = student.VkProfileLink
        };

        var studentId = await studentRepository.CreateAsync(studentDto);

        return StatusCode(StatusCodes.Status201Created, new { Id = studentId });
    }

    /// <summary>
    /// Обновить данные о студенте
    /// </summary>
    /// <param name="student">Модель обновления данных о студенте</param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentRequest student)
    {
        await studentRepository.UpdateAsync(mapper.Map<StudentDto>(student));

        return NoContent();
    }

    /// <summary>
    /// Удалить студента
    /// </summary>
    /// <param name="id">Идентификатор студента</param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        await studentRepository.DeleteAsync(id);

        return NoContent();
    }
}


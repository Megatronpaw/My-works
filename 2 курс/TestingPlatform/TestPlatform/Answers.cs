using Microsoft.AspNetCore.Mvc;

namespace TestingPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswerController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllAnswers() => Ok("Список ответов");

    [HttpGet("{id}")]
    public IActionResult GetAnswersById(int id)
    {
        if (id == 1) return Ok("Вопрос 1");
        return NotFound();
    }

    [HttpPost]
    public IActionResult CreateAnswers() => Created("/api/answers/1", "Создан ответ с ID=1");

    [HttpPut("{id}")]
    public IActionResult UpdateAnswers(int id) => NoContent();

    [HttpDelete("{id}")]
    public IActionResult DeleteAnswers(int id) => NoContent();
}


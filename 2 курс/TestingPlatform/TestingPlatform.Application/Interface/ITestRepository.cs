using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Application.Interfaces;

public interface ITestRepository
{
    Task<IEnumerable<TestDto>> GetAllAsync();
    Task<TestDto> GetByIdAsync(int id);
    Task<int> CreateAsync(TestDto testDto);
    Task UpdateAsync(TestDto testDto);
    Task DeleteAsync(int id);
}
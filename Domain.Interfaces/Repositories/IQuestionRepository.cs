using Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IQuestionRepository
    {
        Task<QuestionModel> CreateNewQuestion(NewQuestionModel newQuestion);
        Task DeleteQuestion(int questionId);
        Task DeactivateQuestion(int questionId);
        Task<QuestionModel> GetQuestion(int id);

    }
}

using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class FeedbackService
    {
        private readonly FeedbackRepository _feedbackRepository;

        public FeedbackService(IConfiguration configuration)
        {
            _feedbackRepository = new FeedbackRepository(configuration);
        }

        public Result<List<Feedback>> GetAllFeedbacks()
        {
            try
            {
                var feedbacks = _feedbackRepository.Get();
                return Result<List<Feedback>>.Success(feedbacks);
            }
            catch (Exception ex)
            {
                return Result<List<Feedback>>.Failure($"Falha ao obter os feedbacks: {ex.Message}");
            }
        }

        public Result<Feedback> GetFeedbackById(int feedbackId)
        {
            try
            {
                var feedback = _feedbackRepository.GetById(feedbackId);

                if (feedback == null)
                {
                    return Result<Feedback>.Failure("Feedback não encontrado.");
                }

                return Result<Feedback>.Success(feedback);
            }
            catch (Exception ex)
            {
                return Result<Feedback>.Failure($"Falha ao obter o feedback: {ex.Message}");
            }
        }

        public Result<Feedback> AddFeedback(FeedbackInsert feedbackModel)
        {
            try
            {
                var newFeedback = _feedbackRepository.Insert(feedbackModel);
                return Result<Feedback>.Success(newFeedback);
            }
            catch (Exception ex)
            {
                return Result<Feedback>.Failure($"Falha ao inserir o feedback: {ex.Message}");
            }
        }

        public Result<Feedback> UpdateFeedback(FeedbackUpdate feedback)
        {
            try
            {
                var updatedFeedback = _feedbackRepository.Update(feedback);
                return Result<Feedback>.Success(updatedFeedback);
            }
            catch (Exception ex)
            {
                return Result<Feedback>.Failure($"Falha ao atualizar o feedback: {ex.Message}");
            }
        }

        public Result<bool> SetActiveStatus(int feedbackId, bool isActive)
        {
            try
            {
                _feedbackRepository.SetActiveStatus(feedbackId, isActive);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status ativo do feedback: {ex.Message}");
            }
        }

        public Result<bool> SetDeletedStatus(int feedbackId, bool isDeleted)
        {
            try
            {
                _feedbackRepository.SetDeletedStatus(feedbackId, isDeleted);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao definir o status excluído do feedback: {ex.Message}");
            }
        }
    }
}

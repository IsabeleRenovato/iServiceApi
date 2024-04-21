using iServiceRepositories.Repositories;
using iServiceRepositories.Repositories.Models;
using iServiceRepositories.Repositories.Models.Request;
using iServiceServices.Services.Models;
using Microsoft.Extensions.Configuration;

namespace iServiceServices.Services
{
    public class FeedbackService
    {
        private readonly FeedbackRepository _feedbackRepository;
        private readonly AppointmentRepository _appointmentRepository;
        public FeedbackService(IConfiguration configuration)
        {
            _feedbackRepository = new FeedbackRepository(configuration);
            _appointmentRepository = new AppointmentRepository(configuration);
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

        //public async Result<Feedback> GetFeedbackByEstablishment(int establishmentProfileID)
        //{
        //    try
        //    {
        //        var appointments = await _appointmentRepository
        //        var feedback = _feedbackRepository.GetById(feedbackId);

        //        if (feedback == null)
        //        {
        //            return Result<Feedback>.Failure("Feedback não encontrado.");
        //        }

        //        return Result<Feedback>.Success(feedback);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<Feedback>.Failure($"Falha ao obter o feedback: {ex.Message}");
        //    }
        //}

        public Result<Feedback> AddFeedback(FeedbackModel model)
        {
            try
            {
                var newFeedback = _feedbackRepository.Insert(model);
                return Result<Feedback>.Success(newFeedback);
            }
            catch (Exception ex)
            {
                return Result<Feedback>.Failure($"Falha ao criar o feedback: {ex.Message}");
            }
        }

        public Result<Feedback> UpdateFeedback(Feedback feedback)
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

        public Result<bool> DeleteFeedback(int feedbackId)
        {
            try
            {
                bool success = _feedbackRepository.Delete(feedbackId);

                if (!success)
                {
                    return Result<bool>.Failure("Falha ao deletar o feedback ou feedback não encontrado.");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Falha ao deletar o feedback: {ex.Message}");
            }
        }
    }

}

using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class FormService : IFormService
    {
        private readonly IFormRepository _formRepository;

        public FormService(IFormRepository formRepository)
        {
            _formRepository = formRepository;
        }

        public async Task SubmitFormAsync(string formType, FormSubmissionDto formData)
        {
            var submission = new FormSubmission
            {
                StudentId = formData.StudentId,
                FormType = formType,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(formData),
                SubmissionDate = DateTime.UtcNow
            };
            await _formRepository.AddSubmissionAsync(submission);
        }

        public async Task<List<FormSubmissionDto>> GetFormSubmissionsAsync(string formType)
        {
            var submissions = await _formRepository.GetSubmissionsByTypeAsync(formType);
            return submissions.Select(s => Newtonsoft.Json.JsonConvert.DeserializeObject<FormSubmissionDto>(s.Data)).ToList();
        }
    }
}
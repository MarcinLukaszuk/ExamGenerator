using AutoMapper;
using ExamGeneratorModel.DTO;
using ExamGeneratorModel.Model;
using ExamGeneratorModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamGenerator
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<ExamCore, ExamDTO>()
                 .ForMember(destination => destination.QuestionsDTO, opts => opts.MapFrom(source => source.Questions));
            CreateMap<ExamDTO, ExamCore>()
                .ForMember(destination => destination.Questions, opts => opts.MapFrom(source => source.QuestionsDTO));
            CreateMap<ExamCoreViewModel, ExamCore>()
                    .ForMember(destination => destination.Questions, opts => opts.MapFrom(source => source.Questions));
            CreateMap<ExamCore, ExamCoreViewModel>()
                               .ForMember(destination => destination.Questions, opts => opts.MapFrom(source => source.Questions));

            CreateMap<Question, QuestionDTO>()
             .ForMember(destination => destination.AnswersDTO,
            opts => opts.MapFrom(source => source.Answers));
            CreateMap<QuestionDTO, Question>()
                .ForMember(destination => destination.Answers,
            opts => opts.MapFrom(source => source.AnswersDTO));

            CreateMap<QuestionViewModel, Question>()
               .ForMember(destination => destination.Answers, opts => opts.MapFrom(source => source.Answers));
            CreateMap<Question, QuestionViewModel>()
                .ForMember(destination => destination.Answers, opts => opts.MapFrom(source => source.Answers));

            CreateMap<Answer, AnswerDTO>();
            CreateMap<AnswerDTO, Answer>();
            CreateMap<AnswerViewModel, Answer>();
            CreateMap<Answer, AnswerViewModel>();

            CreateMap<AnswerPosition, AnswerPositionDTO>()
                 .ForMember(destination => destination.AnswerDTO, opts => opts.MapFrom(source => source.Answer)); ;
            CreateMap<AnswerPositionDTO, AnswerPosition>();

            CreateMap<Result, ResultDTO>();
            CreateMap<ResultDTO, Result>();

            CreateMap<Student, EditStudentViewModel>();
            CreateMap<EditStudentViewModel, Student>();

            CreateMap<ExamCore, EditExamViewModel>();
            CreateMap<EditExamViewModel, ExamCore>();
        }
    }
}
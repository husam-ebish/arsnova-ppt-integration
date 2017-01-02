﻿using System;
using System.Collections.ObjectModel;
using ARSnovaPPIntegration.Common.Contract.Translators;
using ARSnovaPPIntegration.Common.Enum;

namespace ARSnovaPPIntegration.Business.Model
{
    public class SlideQuestionModel
    {
        // TODO: Concept: which default values should be taken?
        private ObservableCollection<object> answerOptions;

        private string questionText;

        private QuestionTypeEnum questionType = QuestionTypeEnum.SingleChoiceClick;

        public SlideQuestionModel(IQuestionTypeTranslator questionTypeTranslator)
        {
            this.Id = Guid.NewGuid();

            this.QuestionTypeTranslator = questionTypeTranslator;
        }

        public event EventHandler ObjectChangedEventHandler;

        public readonly IQuestionTypeTranslator QuestionTypeTranslator;

        // TODO test data
        public int SlideNumber { get; set; } = 0;

        public int Index { get; set; }

        // TODO select on which slide this question should be

        public Guid Id { get; set; }

        public QuestionTypeEnum QuestionType
        {
            get
            {
                return this.questionType;
            }
            set
            {
                this.questionType = value;
                this.ObjectChangedEventHandler?.Invoke(this, EventArgs.Empty);
            }
        }

        public string QuestionTypeText => this.QuestionTypeTranslator.TranslateQuestionType(this.QuestionType);

        public bool QuestionTypeSet { get; set; } = false;

        public string QuestionText
        {
            get
            {
                return this.questionText;
            }
            set
            {
                this.questionText = value;
                this.QuestionTypeSet = true;
                this.ObjectChangedEventHandler?.Invoke(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<object> AnswerOptions

        {
            get { return this.answerOptions; }
            set
            {
                this.answerOptions = value;
                this.AnswerOptionsSet = true;
            }
        }

        public bool AnswerOptionsSet { get; set; } = false;

        public AnswerOptionType AnswerOptionType { get; set; }

        public int AnswerOptionAmount { get; set; } = 4;

        public AnswerOptionType AnswerOptionInitType { get; set; }

        public void AnswerOptionModelChanged()
        {
            this.ObjectChangedEventHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}

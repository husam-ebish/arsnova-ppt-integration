﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Practices.ServiceLocation;

using ARSnovaPPIntegration.Business.Contract;
using ARSnovaPPIntegration.Business.Model;
using ARSnovaPPIntegration.Common.Contract;
using ARSnovaPPIntegration.Common.Contract.Exceptions;
using ARSnovaPPIntegration.Common.Enum;
using ARSnovaPPIntegration.Communication.Contract;

namespace ARSnovaPPIntegration.Business
{
    public class SlideManipulator : ISlideManipulator
    {
        private readonly ILocalizationService localizationService;

        private readonly IArsnovaClickService arsnovaClickService;

        public SlideManipulator(ILocalizationService localizationService)
        {
            this.localizationService = localizationService;
            this.arsnovaClickService = ServiceLocator.Current.GetInstance<IArsnovaClickService>();
        }

        public void AddFooter(Slide slide, string header = "ARSnova Quiz")
        {
            slide.HeadersFooters.Footer.Visible = MsoTriState.msoTrue;
            slide.HeadersFooters.Footer.Text = this.localizationService.Translate(header);
        }

        public void SetArsnovaStyle(Slide slide)
        {
            throw new NotImplementedException();
        }

        public void SetArsnovaClickStyle(Slide arsnovaSlide, string hashtag)
        {
            var sessionConfiguration = this.arsnovaClickService.GetSessionConfiguration(hashtag);

            var themeName = string.Empty;

            // TODO create background-pictures
            switch (sessionConfiguration.theme)
            {
                case "theme-thm":
                    break;
                case "theme-elegant":
                    break;
                case "theme-arsnova":
                    break;
                case "theme-blackbeauty":
                    break;
                case "theme-hell":
                    break;
                case "theme-bluetouch":
                    break;
                case "theme-green":
                    break;
                case "theme-action":
                    break;
                case "theme-Psychology-Correct-Colours":
                    break;
                case "theme-arsnova-dot-click-contrast":
                    break;
                default:
                    throw new CommunicationException("Unexpected theme name");
            }

            // TODO
            // background
            arsnovaSlide.FollowMasterBackground = MsoTriState.msoFalse;
            arsnovaSlide.Background.Fill.UserPicture(@"C:\fox.jpg");
         
            // footer
            arsnovaSlide.HeadersFooters.Footer.Visible = MsoTriState.msoTrue;
            arsnovaSlide.HeadersFooters.Footer.Text = "Copyright arsnova team / Tjark Wilhelm Hoeck";
        }

        public void AddClickIntroSlide(Slide slide, string hashtag)
        {
            var titelObj = slide.Shapes[1].TextFrame.TextRange;
            titelObj.Text = this.localizationService.Translate("ARSnova.click");
            titelObj.Font.Name = "Arial";
            titelObj.Font.Size = 32;

            // Microsoft.Office.Interop.PowerPoint.Shape shape = slide.Shapes[2];
            // slide.Shapes.AddPicture(pictureFileName, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, shape.Left, shape.Top, shape.Width, shape.Height);

            var contentObj = slide.Shapes[2].TextFrame.TextRange;
            contentObj.Text = this.localizationService.Translate("This presentation uses arsnova.click, join the hashtag:");
            contentObj.Text += Environment.NewLine;
            contentObj.Text += Environment.NewLine;
            contentObj.Text += hashtag;
            contentObj.Paragraphs(-1).Lines(3, 1).Font.Name = "Arial";
            contentObj.Paragraphs(-1).Lines(3, 1).Font.Size = 26;
            // TODO create QR-Code / get it from click server
        }

        public void AddQuizToSlide(SlideQuestionModel slideQuestionModel, Slide slide)
        {
            slide.Layout = PpSlideLayout.ppLayoutText;

            // question
            var questionObj = slide.Shapes[1].TextFrame.TextRange;
            questionObj.Text = slideQuestionModel.QuestionText;
            questionObj.Font.Name = "Arial";
            questionObj.Font.Size = 26;

            // answer options
            // no answer options on ranged questions
            if (slideQuestionModel.AnswerOptionType != AnswerOptionType.ShowRangedAnswerOption)
            {
                var answerOptionsString = slideQuestionModel.AnswerOptions.Cast<GeneralAnswerOption>()
                    .Aggregate(string.Empty, (current, castedAnswerOption) => current + $"{this.PositionNumberToLetter(castedAnswerOption.Position, true)}: {castedAnswerOption.Text}{Environment.NewLine}");

                var answerOptionsObj = slide.Shapes[2].TextFrame.TextRange;
                answerOptionsObj.Text = answerOptionsString;
                answerOptionsObj.Font.Name = "Arial";
                answerOptionsObj.Font.Size = 20;
            }

            // action button: start quiz
            // TODO -> start quiz with context menü!
        }

        public void AddResultsToSlide(Slide slide, SlideQuestionModel slideQuestionModel)
        {
            // TODO
        }

        private string PositionNumberToLetter(int number, bool isCaps)
        {
            var c = (isCaps ? 65 : 97) + (number - 1);
            return c.ToString();
        }
    }
}
